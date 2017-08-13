using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;
using Newtonsoft.Json;

namespace ServiceApp.Domain.Common
{
    public class PushNotifications
    {
        public string DeviceID { get; set; }
        public dynamic MyProperty { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationBody { get; set; }
        public NotificationResponse SendNotification()
        {
            NotificationResponse ObjNotificationResponse = new NotificationResponse();
            try
            {
                string strServerKey = ConfigurationManager.AppSettings["NotificationServerKey"].ToString();

                WebRequest tRequest = WebRequest.Create(ConfigurationManager.AppSettings["NotificationURL"].ToString());
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                var NotificationData = new
                {
                    to = DeviceID,
                    priority = "high",
                    notification = new
                    {
                        body = NotificationBody,
                        title = NotificationTitle,
                        sound = "default",
                        icon = "myicon"
                    },
                    data = new
                    {
                        MyProperty
                    }
                };

                string jsonNotificationFormat = JsonConvert.SerializeObject(NotificationData);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", strServerKey));
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string strResponse = tReader.ReadToEnd();
                                ObjNotificationResponse = JsonConvert.DeserializeObject<NotificationResponse>(strResponse);
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {

            }

            return ObjNotificationResponse;
        }
    }

    public class NotificationResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public NotificationResponseResult[] results { get; set; }
    }

    public class NotificationResponseResult
    {
        public string error { get; set; }
        public string message_id { get; set; }
    }

}
