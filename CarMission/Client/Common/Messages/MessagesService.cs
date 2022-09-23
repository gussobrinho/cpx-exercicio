using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;

namespace CarMission.Client.Common.Messages
{
    public class MessagesService
    {
        public static void SendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { "[" + title + "]", message }
            };

            BaseScript.TriggerEvent("chat:addMessage", msg);
        }

        public static void Notify(string message)
        {
            API.AddTextEntry("MESSAGE_C&K", message);
            API.SetNotificationTextEntry("MESSAGE_C&K");
            API.DrawNotification(true, false);
        }
    }
}
