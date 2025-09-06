using FirebaseAdmin.Messaging;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CoreXNugetPackage.Utilities.Notifications
{
    public class NotificationHelper
    {
        public static async void SendNotification(MessageRequest messageRequest)
        {
            try
            {
                var notification = new Notification
                {
                    Title = messageRequest.Title,
                    Body = messageRequest.Body
                };

                if (messageRequest.DeviceTokens?.Count() > 0)
                {
                    var messages = new MulticastMessage()
                    {
                        Notification = notification,
                        Tokens = messageRequest.DeviceTokens
                    };

                    if (messageRequest.Data != null)
                        messages.Data = messageRequest.Data;

                    var messaging = FirebaseMessaging.DefaultInstance;
                    //await messaging.SendAllAsync()
                    var result = await messaging.SendMulticastAsync(messages);
                }
                else
                {
                    var message = new Message()
                    {
                        Notification = notification,
                        Apns = new ApnsConfig
                        {
                            Aps = new Aps
                            {
                                Sound = "default"
                            }
                        }
                        //Data = new Dictionary<string, string>()
                        //{
                        //    ["CustomData"] = "Value1"
                        //},
                    };

                    if (messageRequest.Data != null)
                        message.Data = messageRequest.Data;
                    if (messageRequest.DeviceToken != null)
                        message.Token = messageRequest.DeviceToken;
                    else
                        message.Topic = "all";

                    var messaging = FirebaseMessaging.DefaultInstance;
                    var result = await messaging.SendAsync(message);

                    if (!string.IsNullOrEmpty(result))
                    {
                        //return
                    }
                    else
                    {
                        throw new Exception("Error sending the message.");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    public class MessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string? DeviceToken { get; set; }
        [DefaultValue(false)]
        public bool AllDevice { get; set; }
        [JsonIgnore]
        public List<string>? DeviceTokens { get; set; }

        public Dictionary<string, string>? Data { get; set; }
    }
}
