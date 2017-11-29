using System.IO;
using System.Net;

namespace IkariamBots
{
    public class PhoneConnection
    {
        private WebClient client;
        private bool validAPI;

        public PhoneConnection(string APIKey)
        {
            validAPI = !string.IsNullOrWhiteSpace(APIKey);
            client = new WebClient();
            client.Headers.Add("Access-Token", APIKey);
            client.Headers.Add("Content-Type", "application/json; charset=UTF-8");
        }

        public void SendPush(string title, string message)
        {
            if (!validAPI)
                return;
            string postData = "{\"type\": \"note\", \"title\": \"" + title + "\", \"body\": \"" + message + "\", \"application_name\": \"" + Config.PROGRAM_NAME + "\", \"url\": \"\", \"icon\": \"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\" }";
            client.UploadString("https://api.pushbullet.com/v2/pushes", postData);
        }

        public string Chat()
        {
            return client.DownloadString("https://api.pushbullet.com/v2/chats");
        }
    }
}
