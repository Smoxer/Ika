/**
 */

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;


namespace DeathByCaptcha
{
    /**
     * <summary>Delegate for asyncronous CAPTCHA decoding.  To be called when CAPTCHA is solved or timed out.</summary>
     * <param name="captcha">Uploaded and solved CAPTCHA on success, null otherwise.</param>
     */
    public delegate void DecodeDelegate(Captcha captcha);


    /**
     * <summary>Death by Captcha API client.</summary>
     */
    abstract public class Client
    {
        /**
         * <value>API client version.</value>
         */
        public const string Version = "DBC/.NET v4.1.2";

        /**
         * <value>Arbitrary 3rd-party software ID.</value>
         */
        public const int SoftwareVendorId = 0;

        /**
         * <value>Default CAPTCHA solving timeout (in seconds).</value>
         */
        public const int DefaultTimeout = 60;

        /**
         * <value>Interval between CAPTCHA status polls (in seconds).  Keep if above 2 seconds or you might be banned for too abusive polling.</value>
         */
        public const int PollsInterval = 5;


        /**
         * <value>API client verbosity flag; when set to true, the client will dump API requests and responses.</value>
         */
        public bool Verbose = false;


        protected string _username = "";
        protected string _password = "";
        protected Object _callLock = new Object();


        /**
         * <value>DBC account credentials dictionary -- "username" and "password" items only for now.</value>
         */
        public Hashtable Credentials
        {
            get
            {
                Hashtable userpwd = new Hashtable();
                userpwd["username"] = _username;
                userpwd["password"] = _password;
                return userpwd;
            }
        }

        /**
         * <value>DBC account details.</value>
         * <see cref="DeathByCaptcha.User"/>
         */
        public User User
        {
            get
            {
                return GetUser();
            }
        }

        /**
         * <value>DBC account balance (in US cents).</value>
         */
        public double Balance
        {
            get
            {
                return GetBalance();
            }
        }


        protected void Log(string call, string msg)
        {
            if (this.Verbose) {
                Console.WriteLine(DateTime.Now.Ticks + " " + call + (null != msg ? ": " + msg : ""));
            }
        }

        protected void Log(string call)
        {
            this.Log(call, null);
        }

		protected byte[] Load(Object data)
		{
			if(data is string){
				return this.Load ((string)data);
			}else if (data is Stream) {
				return this.Load ((Stream)data);
			}else if (data is byte[]) {
				return (byte[])data;
			}
			this.Log ("Loading data with invalid type.");
			return null;
		}

        protected byte[] Load(Stream st)
        {
            long pos = -1;
            if (st.CanSeek) {
                pos = st.Position;
                st.Position = 0;
            }
            int n = 0, offset = 0, chunk_size = 1024;
            byte[] buf = new byte[chunk_size];
            while (st.CanRead && 0 < (n = st.Read(buf, offset, chunk_size))) {
                offset += n;
                Array.Resize(ref buf, offset + chunk_size);
            }
            if (-1 < pos) {
                st.Position = pos;
            }
            Array.Resize(ref buf, offset);
            return buf;
        }

        protected byte[] Load(string fn)
        {
            if (!File.Exists(fn)) {
                throw new FileNotFoundException(
                    "CAPTCHA image file " + fn + " not found"
                );
            } else {
                using (FileStream st = File.OpenRead(fn)) {
                    return this.Load(st);
                }
            }
        }

        /**
         * <summary>Wait for a CAPTCHA to be solved.</summary>
         * <param name="captcha">Uploaded CAPTCHA.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <returns>CAPTCHA if solved, null otherwise.</returns>
         */
        protected Captcha Poll(Captcha captcha, int timeout)
        {
            if (null != captcha) {
                DateTime deadline =
                    DateTime.Now.AddSeconds(0 < timeout
                        ? timeout
                        : Client.DefaultTimeout);
                while (deadline > DateTime.Now && !captcha.Solved) {
                    Thread.Sleep(Client.PollsInterval * 1000);
                    try {
                        captcha = this.GetCaptcha(captcha);
                    } catch (System.Exception e) {
                        if (this.Verbose) {
                            Console.WriteLine(DateTime.Now.Ticks + " POLL " + e.Message);
                        }
                        return null;
                    }
                }
                if (captcha.Solved && captcha.Correct) {
                    return captcha;
                }
            }
            return null;
        }

        protected void PollWithCallback(object state)
        {
            PollPayload payload = (PollPayload)state;
            payload.Callback(this.Poll(payload.Captcha, payload.Timeout));
        }


        /**
         * <summary>Instantiate a DBC API client.</summary>
         * <param name="username">DBC account username.</param>
         * <param name="password">DBC account password.</param>
         */
        public Client(string username, string password)
        {
            this._username = username;
            this._password = password;
        }


        /**
         * <summary>Close API connection if opened.</summary>
         */
        abstract public void Close();


        /**
         * <returns>DBC account details.</returns>
         */
        abstract public User GetUser();

        /**
         * <returns>DBC account balance (in US cents).</returns>
         */
        public double GetBalance()
        {
            return this.GetUser().Balance;
        }


        /**
         * <param name="id">CAPTCHA ID.</param>
         * <returns>Uploaded CAPTCHA if exists, null otherwise.</returns>
         */
        abstract public Captcha GetCaptcha(int id);

        /**
         * <param name="captcha">CAPTCHA.</param>
         * <returns>Uploaded CAPTCHA if exists, null otherwise.</returns>
         */
        public Captcha GetCaptcha(Captcha captcha)
        {
            return this.GetCaptcha(captcha.Id);
        }

        /**
         * <param name="id">CAPTCHA ID.</param>
         * <returns>Uploaded CAPTCHA text, null if not found or not solved yet.</returns>
         */
        public string GetText(int id)
        {
            return this.GetCaptcha(id).Text;
        }

        /**
         * <param name="captcha">CAPTCHA.</param>
         * <returns>Uploaded CAPTCHA text, null if not found or not solved yet.</returns>
         */
        public string GetText(Captcha captcha)
        {
            return this.GetCaptcha(captcha).Text;
        }


        /**
         * <summary>Upload a CAPTCHA.</summary>
         * <param name="img">Raw CAPTCHA image.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         * <returns>Uploaded CAPTCHA, null if failed.</returns>
         */
		abstract public Captcha Upload(byte[] img, Hashtable ext_data = null);

        /**
         * <summary>Upload a CAPTCHA.</summary>
         * <param name="st">CAPTCHA image byte stream.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         * <returns>Uploaded CAPTCHA, null if failed.</returns>
         */
		public Captcha Upload(Stream st, Hashtable ext_data = null)
        {
			return this.Upload(this.Load(st), ext_data);
        }

        /**
         * <summary>Upload a CAPTCHA.</summary>
         * <param name="fn">CAPTCHA image file name.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         * <returns>Uploaded CAPTCHA details, null if failed.</returns>
         */
		public Captcha Upload(string fn, Hashtable ext_data = null)
        {
			return this.Upload(this.Load(fn), ext_data);
        }


        /**
         * <summary>Report an incorrectly solved CAPTCHA.</summary>
         * <param name="id">CAPTCHA ID.</param>
         * <returns>true on success.</returns>
         */
        abstract public bool Report(int id);

        /**
         * <summary>Report an incorrectly solved CAPTCHA.</summary>
         * <param name="captcha">CAPTCHA.</param>
         * <returns>true on success.</returns>
         */
        public bool Report(Captcha captcha)
        {
            return this.Report(captcha.Id);
        }

        /**
         * <summary>Upload and wait for a CAPTCHA to be solved.</summary>
         * <param name="img">Raw CAPTCHA image.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         * <returns>CAPTCHA if solved, null otherwise.</returns>
         */
		public Captcha Decode(byte[] img, int timeout, Hashtable ext_data = null)
        {
			return this.Poll(this.Upload(img, ext_data), timeout);
        }


        /**
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="img">Raw CAPTCHA image.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, byte[] img, int timeout, Hashtable ext_data = null)
        {
            PollPayload payload = new PollPayload();
            payload.Callback = callback;
			payload.Captcha = this.Upload(img, ext_data);
            payload.Timeout = timeout;
            new Thread(PollWithCallback).Start(payload);
        }

        /**
         * <see cref="M:Client.Decode(byte[], int)"/>
         * <param name="img">Raw CAPTCHA image.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public Captcha Decode(byte[] img, Hashtable ext_data = null)
        {
			return this.Decode(img, 0, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(DecodeDelegate, byte[], int)"/>
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="img">Raw CAPTCHA image.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, byte[] img, Hashtable ext_data = null)
        {
			this.Decode(callback, img, 0, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(byte[], int)"/>
         * <param name="st">CAPTCHA image byte stream.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public Captcha Decode(Stream st, int timeout, Hashtable ext_data = null)
        {
			return this.Decode(this.Load(st), timeout, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(DecodeDelegate, byte[], int)"/>
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="st">CAPTCHA image byte stream.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, Stream st, int timeout, Hashtable ext_data = null)
        {
			this.Decode(callback, this.Load(st), timeout, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(byte[], int)"/>
         * <param name="st">CAPTCHA image byte stream.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public Captcha Decode(Stream st, Hashtable ext_data = null)
        {
			return this.Decode(st, 0, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(DecodeDelegate, byte[], int)"/>
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="st">CAPTCHA image byte stream.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, Stream st, Hashtable ext_data = null)
        {
			this.Decode(callback, this.Load(st), 0, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(byte[], int)"/>
         * <param name="fn">CAPTCHA image file name.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public Captcha Decode(string fn, int timeout, Hashtable ext_data = null)
        {
			return this.Decode(this.Load(fn), timeout, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(DecodeDelegate, byte[], int)"/>
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="fn">CAPTCHA image file name.</param>
         * <param name="timeout">Solving timeout (in seconds).</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, string fn, int timeout, Hashtable ext_data = null)
        {
			this.Decode(callback, this.Load(fn), timeout, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(byte[], int)"/>
         * <param name="fn">CAPTCHA image file name.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public Captcha Decode(string fn, Hashtable ext_data = null)
        {
			return this.Decode(fn, 0, ext_data);
        }

        /**
         * <see cref="M:Client.Decode(DecodeDelegate, byte[], int)"/>
         * <param name="callback">A delegate to call when the CAPTCHA is solved or timed out.</param>
         * <param name="fn">CAPTCHA image file name.</param>
         * <param name="ext_data">Extra data used by special captchas types.</param>
         */
		public void Decode(DecodeDelegate callback, string fn, Hashtable ext_data = null)
        {
			this.Decode(callback, this.Load(fn), 0, ext_data);
        }
    }
}
