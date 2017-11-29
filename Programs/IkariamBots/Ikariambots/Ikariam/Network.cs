using System;
using Library;
using IkariamBots;
using System.Threading;
using System.Text.RegularExpressions;

namespace Ikariam
{
    public class Network
    {
        private PlayableWebClient client;
        private string actionRequest, url;

        public Network(Connection connection, string url)
        {
            this.url = url;

            client = new PlayableWebClient();
            client.SetUA(connection.UA);
            if (connection.proxy != Connection.EMPTY_PROXY && connection.proxy != "")
                client.SetProxy(connection.proxy);
        }

        /// <exception cref="ArgumentException">actionRequest expired</exception>
        public string AjaxHandlerRequest(NetworkSettings settings)
        {
            string response = string.Empty;
            if (settings.autoAddActionRequest && !string.IsNullOrEmpty(actionRequest))
            {
                settings.parameters += "&actionRequest=" + actionRequest;
            }

            for (int i = 0; i < int.Parse(Settings.Instance.Get("ConnectionRetries", "5")); i++)
            {
                try
                {
                    if (!client.IsBusy)
                    {
                        if (settings.method.Equals("POST"))
                        {
                            response = client.UploadString(url + "?" + settings.parameters, settings.data);
                        }
                        else
                        {
                            response = client.DownloadString(url + "?" + settings.parameters);
                        }

                        //System.IO.File.AppendAllText("Log.txt", url + "?" + parameters + "&" + data + "\n" + response + "\n\n\n");

                        if (settings.checkError && response.Contains("[[\"changeView\",[\"error\""))
                        {
                            //AddToLog(lang.Get("Misc", "Error") + " " + lang.Get("Misc", "LoginToUser"));
                            if(null == settings.relogin)
                                throw new Exceptions.ActionRequestException("Error performing the action");
                            else
                            {
                                if (settings.relogin())
                                {
                                    if (settings.method.Equals("POST"))
                                    {
                                        response = client.UploadString(url + "?" + settings.parameters, settings.data);
                                    }
                                    else
                                    {
                                        response = client.DownloadString(url + "?" + settings.parameters);
                                    }
                                }
                                else
                                {
                                    throw new Exceptions.ActionRequestException("Error performing the action");
                                }
                            }
                        }

                        /*if(string.IsNullOrEmpty(currentCity.ID))
                            currentCity.ID = Regex.Match(response, "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;*/
                        UpdateActionRequest(response);
                        break;
                    }
                    else
                    {
                        Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000")));
                    }
                }
                catch (Exception e) { Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000"))); }
            }

            return response;
        }

        /// <exception cref="ArgumentException">actionRequest expired</exception>
        public string AjaxHandlerRequest(string parameter, Func<bool> relogin)
        {
            return AjaxHandlerRequest(new NetworkSettings(parameter, relogin));
        }

        public string DownloadString(string url, string method = "GET", string data = "")
        {
            string response = string.Empty;

            for (int i = 0; i < int.Parse(Settings.Instance.Get("ConnectionRetries", "5")); i++)
            {
                try
                {
                    if (!client.IsBusy)
                    {
                        if (method.Equals("POST"))
                        {
                            response = client.UploadString(url, data);
                        }
                        else
                        {
                            response = client.DownloadString(url);
                        }

                        break;
                    }
                    else
                    {
                        Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000")));
                    }
                }
                catch (Exception e) { Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000"))); }
            }

            return response;
        }

        public byte[] DownloadData(string url, string method = "GET", byte[] data = null)
        {
            byte[] response = null;

            for (int i = 0; i < int.Parse(Settings.Instance.Get("ConnectionRetries", "5")); i++)
            {
                try
                {
                    if (!client.IsBusy)
                    {
                        if (method.Equals("POST"))
                        {
                            response = client.UploadData(url, data);
                        }
                        else
                        {
                            response = client.DownloadData(url);
                        }

                        break;
                    }
                    else
                    {
                        Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000")));
                    }
                }
                catch (Exception e) { Thread.Sleep(int.Parse(Settings.Instance.Get("ConnectionRetriesTimeOut", "3000"))); }
            }

            return response;
        }

        private void UpdateActionRequest(string response)
        {
            response = response.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            GroupCollection actionRequestVal = Regex.Match(response, "\"actionRequest\":\"(.+?)\"").Groups;
            if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                actionRequest = actionRequestVal[1].Value;

            actionRequestVal = Regex.Match(response, "actionRequest:\"(.+?)\"").Groups;
            if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                actionRequest = actionRequestVal[1].Value;

            actionRequestVal = Regex.Match(response, "name=\"actionRequest\"value=\"(.+?)\"/>").Groups;
            if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                actionRequest = actionRequestVal[1].Value;
        }
        
        public class NetworkSettings
        {
            public string parameters, method, data;
            public bool autoAddActionRequest, checkError;
            public Func<bool> relogin;

            public NetworkSettings(string parameters, Func<bool> relogin)
            {
                this.parameters = parameters;
                this.relogin = relogin;
                method = "GET";
                data = "";
                autoAddActionRequest = true;
                checkError = false;
                relogin = null;
            }

            public NetworkSettings(string parameters, string method, string data, bool autoAddActionRequest, bool checkError, Func<bool> relogin)
            {
                this.parameters = parameters;
                this.method = method;
                this.data = data;
                this.autoAddActionRequest = autoAddActionRequest;
                this.checkError = checkError;
                this.relogin = relogin;
            }
        }
    }
}
