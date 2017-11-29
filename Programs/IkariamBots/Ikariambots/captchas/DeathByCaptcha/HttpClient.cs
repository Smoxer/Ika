/**
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;


namespace DeathByCaptcha
{
    /**
     * <summary>Death by Captcha HTTP API client.</summary>
     */
    public class HttpClient : Client
    {
        /**
         * <value>Base URL for API calls.</value>
         */
        public const string ServerUrl = "http://api.dbcapi.me/api";

        /**
         * <value>Desired API response MIME type.</value>
         */
        public const string ResponseContentType = "application/json";


        protected byte[] _payload;
        protected HttpWebResponse _response = null;
        protected ManualResetEvent _ready = new ManualResetEvent(false);


        /**
         * <value>HTTP proxy to use, if necessary.</value>
         */
        public WebProxy Proxy = null;


        protected void OnTimeout(Object state, bool timedOut)
        {
            if (timedOut) {
                try {
                    ((HttpWebRequest)state).Abort();
                } catch (System.Exception) {
                    //
                }
            }
        }

        protected void OnReadyToSend(IAsyncResult ar)
        {
            HttpWebRequest req = (HttpWebRequest)ar.AsyncState;
            using (Stream st = req.EndGetRequestStream(ar)) {
                st.Write(this._payload, 0, this._payload.Length);
            }
            this._ready.Set();
        }

        protected void OnReadyToReceive(IAsyncResult ar)
        {
            HttpWebRequest req = (HttpWebRequest)ar.AsyncState;
            try {
                this._response = (HttpWebResponse)req.EndGetResponse(ar);
            } catch (WebException e) {
                this._response = (HttpWebResponse)e.Response;
            } catch (System.Exception) {
                this._response = null;
            }
            this._ready.Set();
        }

        protected string SendReceive(string cmd, byte[] payload, string contentType)
        {
            AutoResetEvent done = new AutoResetEvent(false);
            string response = null;

            DateTime deadline = DateTime.Now.AddSeconds(Client.DefaultTimeout);
            string url = HttpClient.ServerUrl + "/" + cmd;
            while (deadline > DateTime.Now && null != url) {
                this.Log("SEND", url.ToString());

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                this._response = null;

                RegisteredWaitHandle timeoutHandler =
                    ThreadPool.RegisterWaitForSingleObject(done,
                                                           new WaitOrTimerCallback(this.OnTimeout),
                                                           req,
                                                           (int)((deadline - DateTime.Now).TotalMilliseconds),
                                                           true);

                req.AllowAutoRedirect = false;
                req.Proxy = this.Proxy;
                req.Accept = HttpClient.ResponseContentType;
                req.UserAgent = Client.Version;
                req.KeepAlive = false;

                this._payload = payload;
                if (0 < this._payload.Length) {
                    req.Expect = String.Empty;
                    req.Method = "POST";
                    req.ContentType = contentType;
                    req.ContentLength = this._payload.Length;

                    this._ready.Reset();
                    try {
                        req.BeginGetRequestStream(new AsyncCallback(this.OnReadyToSend),
                                                  req);
                    } catch (System.Exception e) {
                        throw new IOException("API connection failed", e);
                    }
                    this._ready.WaitOne();
                } else {
                    req.Method = "GET";
                }

                url = null;
                payload = new byte[0];

                this._ready.Reset();
                try {
                    req.BeginGetResponse(new AsyncCallback(this.OnReadyToReceive),
                                         req);
                } catch (System.Exception e) {
                    throw new IOException("API connection failed", e);
                }
                this._ready.WaitOne();

                if (null != this._response) {
                    if (HttpStatusCode.Forbidden == this._response.StatusCode) {
                        throw new AccessDeniedException(
                            "Access denied, check your credentials and/or balance"
                        );
                    } else if (HttpStatusCode.BadRequest == this._response.StatusCode) {
                        throw new InvalidCaptchaException(
                            "CAPTCHA was rejected, please check if it's a valid image"
                        );
                    } else if (HttpStatusCode.ServiceUnavailable == this._response.StatusCode) {
                        throw new ServiceOverloadException(
                            "CAPTCHA was rejected due to service overload, try again later"
                        );
                    } else if (HttpStatusCode.SeeOther == this._response.StatusCode) {
                        try {
                            url = (string)this._response.Headers["Location"];
                        } catch (System.Exception) {
                            //
                        }
                    } else {
                        using (StreamReader rd = new StreamReader(this._response.GetResponseStream(),
                                                                  Encoding.UTF8)) {
                            response = rd.ReadToEnd();
                        }
                        this.Log("RECV", response);
                    }
                    try {
                        this._response.Close();
                    } catch (System.Exception) {
                        //
                    }
                }

                timeoutHandler.Unregister(done);
            }

            return response;
        }

        protected Hashtable Call(string cmd, byte[] payload, string contentType)
        {
            if (contentType.Equals(String.Empty)) {
                contentType = "application/x-www-form-urlencoded";
            }

            string buf = null;
            lock (this._callLock) {
                buf = this.SendReceive(cmd, payload, contentType);
            }
            if (null != buf) {
                try {
                    return (Hashtable)SimpleJson.Reader.Read(buf);
                } catch (FormatException) {
                    return new Hashtable();
                }
            } else {
                return null;
            }
        }

        protected Hashtable Call(string cmd, Hashtable args)
        {
            string[] fields = new string[args.Count];
            int i = 0;
            foreach (DictionaryEntry e in args) {
                fields[i] = HttpUtility.UrlEncode((string)e.Key) + "=" + HttpUtility.UrlEncode((string)e.Value);
                i++;
            }
            return this.Call(cmd,
                             Encoding.ASCII.GetBytes(String.Join("&", fields)),
                             String.Empty);
        }

        protected Hashtable Call(string cmd)
        {
            return this.Call(cmd, new Hashtable());
        }


        /**
         * <see cref="M:DeathByCaptcha.Client(String, String)"/>
         */
        public HttpClient(string username, string password) : base(username, password)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate { return true; };
        }


        /**
         * <see cref="M:DeathByCaptcha.Client.Close()"/>
         */
        public override void Close()
        {}


        /**
         * <see cref="M:DeathByCaptcha.Client.GetUser()"/>
         */
        public override User GetUser()
        {
            return new User(this.Call("user", this.Credentials));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.GetCaptcha(int)"/>
         */
        public override Captcha GetCaptcha(int id)
        {
            return new Captcha(this.Call("captcha/" + id));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Upload(byte[])"/>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public override Captcha Upload(byte[] img, Hashtable ext_data = null)
        {
            string boundary = BitConverter.ToString(
                (new SHA1CryptoServiceProvider()).ComputeHash(
                    Encoding.ASCII.GetBytes(DateTime.Now.ToString("G"))
                )
            ).Replace("-", String.Empty);

            Hashtable args = this.Credentials;
            args["swid"] = Convert.ToString(Client.SoftwareVendorId);

			byte[] banner = null;
			if ( ext_data!=null ) {
				foreach (DictionaryEntry item in ext_data) {
					if (Convert.ToString(item.Key) == "banner") {
						banner = this.Load(ext_data["banner"]);
					} else {
						args [Convert.ToString(item.Key)] = item.Value.ToString();
					}
				}
			}

            string[] rawArgs = new string[args.Count + 2];
            int i = 0;
            foreach (DictionaryEntry e in args) {
                string v = (string)e.Value;
                rawArgs[i++] = String.Join("\r\n", new string[] {
                    "--" + boundary,
                    "Content-Disposition: form-data; name=\"" + (string)e.Key + "\"",
                    "Content-Length: " + v.Length,
                    "",
                    v
                });
            }
            rawArgs[i++] = String.Join("\r\n", new string[] {
                "--" + boundary,
                "Content-Disposition: form-data; name=\"captchafile\"; filename=\"captcha.jpeg\"",
                "Content-Type: application/octet-stream",
                "Content-Length: " + img.Length,
                ""
            });

			string banner_hdr = "";
			int banner_len = 0;
			byte[] b_hdr = new byte[]{};
			if (banner != null) {
				banner_hdr = String.Join ("\r\n", new string[] {
					"\r\n--" + boundary,
					"Content-Disposition: form-data; name=\"banner\"; filename=\"banner.jpeg\"",
					"Content-Type: application/octet-stream",
					"Content-Length: " + banner.Length,
					"\r\n"
				});
				b_hdr = Encoding.ASCII.GetBytes(banner_hdr);
				banner_len = b_hdr.Length + banner.Length;
			} else {
				banner = new byte[]{};
			}

            byte[] hdr = Encoding.ASCII.GetBytes(String.Join("\r\n", rawArgs));
            byte[] ftr = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
			byte[] body = new byte[hdr.Length + img.Length + banner_len + ftr.Length];
            hdr.CopyTo(body, 0);
            img.CopyTo(body, hdr.Length);

			if (banner_len>0) {
				b_hdr.CopyTo(body, hdr.Length + img.Length);
				banner.CopyTo(body, hdr.Length + img.Length + b_hdr.Length);
			}

			ftr.CopyTo(body, hdr.Length + img.Length + banner_len);

            Captcha c = new Captcha(this.Call(
                "captcha",
                body,
                "multipart/form-data; boundary=" + boundary
            ));
            return c.Uploaded ? c : null;
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Report(int)"/>
         */
        public override bool Report(int id)
        {
            return 0 < id && !(new Captcha(this.Call("captcha/" + id + "/report",
                                                     this.Credentials))).Correct;
        }
    }
}
