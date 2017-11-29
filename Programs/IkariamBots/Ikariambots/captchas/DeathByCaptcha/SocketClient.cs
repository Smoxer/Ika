/**
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace DeathByCaptcha
{
    /**
     * <summary>Death by Captcha socket API client.</summary>
     */
    public class SocketClient : Client
    {
        /**
         * <value>API host.</value>
         */
        public const string ServerHost = "api.dbcapi.me";

        /**
         * <value>First API port.</value>
         */
        public const int ServerFirstPort = 8123;

        /**
         * <value>Last API port.</value>
         */
        public const int ServerLastPort = 8130;

        /**
         * <value>API request/response terminator.</value>
         */
        public const string Terminator = "\r\n";


        protected TcpClient tcpClient = null;


        /**
         * <value>Flag whether the client is connected to the API.</value>
         */
        public bool Connected
        {
            get
            {
                return null != tcpClient && tcpClient.Connected;
            }
        }


        protected void Disconnect()
        {
            if (this.Connected) {
                if (null != this.tcpClient) {
                    this.Log("CLOSE");
                    try {
                        this.tcpClient.GetStream().Close();
                    } catch (System.Exception) {
                        //
                    }
                    try {
                        this.tcpClient.Close();
                    } catch (System.Exception) {
                        //
                    }
                    this.tcpClient = null;
                }
            }
        }

        protected bool Connect()
        {
            if (!this.Connected) {
                this.Log("CONN");
                TcpClient tcpClient = new TcpClient();
                tcpClient.ReceiveTimeout = tcpClient.SendTimeout =
                    Client.DefaultTimeout * 1000;
                tcpClient.Connect(SocketClient.ServerHost,
                                  new Random().Next(SocketClient.ServerFirstPort,
                                                    SocketClient.ServerLastPort + 1));
                if (tcpClient.Connected) {
                    this.tcpClient = tcpClient;
                }
            }
            return this.Connected;
        }

        protected SocketClient Send(byte[] buf)
        {
            this.Log("SEND", Encoding.ASCII.GetString(buf, 0, buf.Length - 2));
            this.tcpClient.GetStream().Write(buf, 0, buf.Length);
            return this;
        }

        protected string Receive()
        {
            int n = 0;
            byte[] buf = new byte[256];
            StringBuilder sb = new StringBuilder();
            NetworkStream st = this.tcpClient.GetStream();
            while (0 < (n = st.Read(buf, 0, buf.Length))) {
                sb.Append(Encoding.ASCII.GetString(buf, 0, n));
                if (sb.ToString(n - 2, 2).Equals(SocketClient.Terminator)) {
                    this.Log("RECV", sb.ToString());
                    return sb.ToString();
                }
            }
            throw new IOException("API connection lost");
        }

        protected Hashtable Call(string cmd, Hashtable args)
        {
            args["cmd"] = cmd;
            args["version"] = Client.Version;

            int attempts = 2;
            byte[] payload = Encoding.ASCII.GetBytes(SimpleJson.Writer.Write(args) + SocketClient.Terminator);
            Hashtable response = null;
            while (0 < attempts && null == response) {
                attempts--;
                if (!this.Connected && !cmd.Equals("login")) {
                    this.Call("login", this.Credentials);
                }
                lock (this._callLock) {
                    try {
                        if (this.Connect()) {
                            response = (Hashtable)SimpleJson.Reader.Read(this.Send(payload).Receive());
                        }
                    } catch (FormatException) {
                        this.Disconnect();
                        response = new Hashtable();
                    } catch (System.Exception) {
                        this.Disconnect();
                    }
                }
            }

            if (null == response) {
                throw new IOException("API connection lost or timed out");
            } else if (response.ContainsKey("error")) {
                lock (this._callLock) {
                    this.Disconnect();
                }

                string error = Convert.ToString(response["error"]);
                if (error.Equals("not-logged-in")) {
                    throw new AccessDeniedException(
                        "Access denied, check your credentials"
                    );
                } else if (error.Equals("banned")) {
                    throw new AccessDeniedException(
                        "Access denied, account is suspended"
                    );
                } else if (error.Equals("insufficient-funds")) {
                    throw new AccessDeniedException(
                        "Access denied, balance is too low"
                    );
                } else if (error.Equals("invalid-captcha")) {
                    throw new InvalidCaptchaException(
                        "CAPTCHA was rejected, please check if it's a valid image"
                    );
                } else if (error.Equals("service-overload")) {
                    throw new ServiceOverloadException(
                        "CAPTCHA was rejected due to service overload"
                    );
                } else {
                    throw new IOException(
                        "API server error occured: " + error
                    );
                }
            } else {
                return response;
            }
        }

        protected Hashtable Call(string cmd)
        {
            return this.Call(cmd, new Hashtable());
        }


        /**
         * <see cref="M:DeathByCaptcha.Client(String, String)"/>
         */
        public SocketClient(string username, string password) : base(username, password)
        {}

        ~SocketClient()
        {
            this.Close();
        }


        /**
         * <see cref="M:DeathByCaptcha.Client.Close()"/>
         */
        public override void Close()
        {
            lock (this._callLock) {
                this.Disconnect();
            }
        }


        /**
         * <see cref="M:DeathByCaptcha.Client.GetUser()"/>
         */
        public override User GetUser()
        {
            return new User(this.Call("user"));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.GetCaptcha(int)"/>
         */
        public override Captcha GetCaptcha(int id)
        {
            Hashtable args = new Hashtable();
            args["captcha"] = id;
            return new Captcha(this.Call("captcha", args));
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Upload(byte[])"/>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public override Captcha Upload(byte[] img, Hashtable ext_data = null)
        {
            Hashtable args = new Hashtable();
            args["swid"] = Client.SoftwareVendorId;
            args["captcha"] = Convert.ToBase64String(img);
			if ( ext_data!=null && ext_data.ContainsKey("type") && (int)ext_data["type"] > 0) {
				args["type"] = Convert.ToString(ext_data["type"]);
			}

			if ( ext_data!=null ) {
				foreach (DictionaryEntry item in ext_data) {
					if (Convert.ToString(item.Key) == "banner") {
						byte[] banner = this.Load(ext_data["banner"]);
						args ["banner"] = Convert.ToBase64String(banner);
					} else {
						args [Convert.ToString(item.Key)] = item.Value.ToString();
					}
				}
			}


            Captcha c = new Captcha(this.Call("upload", args));
            return c.Uploaded ? c : null;
        }

        /**
         * <see cref="M:DeathByCaptcha.Client.Report(int)"/>
         */
        public override bool Report(int id)
        {
            Hashtable args = new Hashtable();
            args["captcha"] = id;
            return !(new Captcha(this.Call("report", args))).Correct;
        }
    }
}
