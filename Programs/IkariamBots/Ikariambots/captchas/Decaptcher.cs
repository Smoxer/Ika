using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace Captchas
{
    public class Decaptcher : CaptchaSolver
    {
        public Decaptcher() : base()
        {
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Encoding = System.Text.Encoding.UTF8;
        }

        public override string SendCaptcha(out string captchaID, out bool success)
        {
            /*try
            {
                uint p_pict_to = 0;
                uint p_pict_type = 1996;
                uint load;
                uint major_id;
                uint minor_id;
                string captchaValue = null;

                int response = DecaptcherLib.Decaptcher.SystemDecaptcherLoad(hostname, port, APIKeyOrName, APISourceOrPassword, out load);
                if (response < 0)
                {
                    captchaValue = response.ToString();
                    success = false;
                    captchaID = null;
                    return null;
                }

                response = DecaptcherLib.Decaptcher.RecognizePicture(hostname, port, APIKeyOrName, APISourceOrPassword, image, out p_pict_to, out p_pict_type, out captchaValue, out major_id, out minor_id);
                captchaID = major_id.ToString() + "|" + minor_id.ToString();
                if (response < 0)
                {
                    captchaValue = response.ToString();
                    CaptchaWorked(captchaID, false);
                }
                success = response >= 0;
                return captchaValue;
            }
            catch (Exception e)
            {
                success = false;
                captchaID = null;
                return e.ToString();
            }*/

            var reqparm = new NameValueCollection();
            reqparm.Add("function", "picture2");
            reqparm.Add("username", APIKeyOrName);
            reqparm.Add("password", APISourceOrPassword);
            reqparm.Add("pict_type", "1996");

            //client.UploadValues(hostname, "POST", reqparm);
            string imageName = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString() + new Random().Next(0,100000);
            string captchaValue = "";
            try
            {
                File.WriteAllBytes(imageName, image);
                string response = UploadFilesToRemoteUrl(hostname, imageName, reqparm);
                File.Delete(imageName);
                //ResultCode|MajorID|MinorID|Type|Timeout|Text

                if (response.Split('|').Length < 6)
                {
                    captchaID = null;
                    captchaValue = response.ToString();
                    CaptchaWorked(captchaID, false);
                }
                else
                {
                    captchaID = response.Split('|')[1].ToString() + "|" + response.Split('|')[2].ToString();
                    captchaValue = response.Split('|')[5].ToString();
                }
                success = response.Length >= 0;
                return captchaValue;
            }
            catch (Exception e)
            {
                success = false;
                captchaID = null;
                return e.ToString();
            }
        }

        public override void CaptchaWorked(string captchaID, bool worked)
        {
            if (!worked)
            {
                //DecaptcherLib.Decaptcher.BadPicture(hostname, port, APIKeyOrName, APISourceOrPassword, uint.Parse(captchaID.Split('|')[0]), uint.Parse(captchaID.Split('|')[1]));
                var reqparm = new NameValueCollection();
                reqparm.Add("function", "picture_bad2");
                reqparm.Add("username", APIKeyOrName);
                reqparm.Add("password", APISourceOrPassword);
                reqparm.Add("major_id", captchaID.Split('|')[0]);
                reqparm.Add("minor_id", captchaID.Split('|')[1]);
                client.UploadValues(hostname, "POST", reqparm);
            }
        }

        public override string GetBalance()
        {
            /*uint load;
            int response = DecaptcherLib.Decaptcher.SystemDecaptcherLoad(hostname, port, APIKeyOrName, APISourceOrPassword, out load);
            if (response < 0)
            {
                return "";
            }

            double balance;
            response = DecaptcherLib.Decaptcher.Balance(hostname, port, APIKeyOrName, APISourceOrPassword, out balance);
            if(response < 0)
            {
                balance = response;
            }

            return balance.ToString();*/
            try
            {
                var reqparm = new NameValueCollection();
                reqparm.Add("function", "balance");
                reqparm.Add("username", APIKeyOrName);
                reqparm.Add("password", APISourceOrPassword);

                return System.Text.Encoding.UTF8.GetString(client.UploadValues(hostname, "POST", reqparm));
            } catch(Exception e) { }
            return string.Empty;
        }

        private static string UploadFilesToRemoteUrl(string url, string file, NameValueCollection formFields = null)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" +
                                    boundary;
            request.Method = "POST";
            request.KeepAlive = true;

            Stream memStream = new MemoryStream();

            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                    boundary + "\r\n");
            var endBoundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" +
                                                                        boundary + "--");


            string formdataTemplate = "\r\n--" + boundary +
                                        "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

            if (formFields != null)
            {
                foreach (string key in formFields.Keys)
                {
                    string formitem = string.Format(formdataTemplate, key, formFields[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    memStream.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: application/octet-stream\r\n\r\n";

                memStream.Write(boundarybytes, 0, boundarybytes.Length);
                var header = string.Format(headerTemplate, "pict", file);
                var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    var buffer = new byte[1024];
                    var bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }
                }
            

            memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse())
            {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }
    }
}
