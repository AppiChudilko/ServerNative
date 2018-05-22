using System;
using CitizenFX.Core;

namespace Client.Native
{
    public class Shared : BaseScript 
    {
        public static readonly string TriggerNsToServer = "NativeAPI:ToServer:";
        public static readonly string TriggerNsToClient = "NativeAPI:ToClient:";
        
        public Shared()
        {
            EventHandlers.Add(TriggerNsToClient + "SetWaypoint", new Action<float, float>(API.SetWaypoint));
            EventHandlers.Add(TriggerNsToClient + "SetWaypoint", new Action<float, float>(API.SetWaypoint));
            EventHandlers.Add(TriggerNsToClient + "SendNotification", new Action<string, bool, bool>(API.SendNotification));
            EventHandlers.Add(TriggerNsToClient + "SendPictureNotification", new Action<string, string, string, string, int>(API.SendPictureNotification));
            EventHandlers.Add(TriggerNsToClient + "SendSubtitle", new Action<string, int, bool>(API.SendSubtitle));
        }
    }
}