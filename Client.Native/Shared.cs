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
            EventHandlers.Add(TriggerNsToClient + "SendNotification", new Action<string, bool, bool>(API.SendNotification));
            EventHandlers.Add(TriggerNsToClient + "SendPictureNotification", new Action<string, string, string, string, int>(API.SendPictureNotification));
            EventHandlers.Add(TriggerNsToClient + "SendSubtitle", new Action<string, int, bool>(API.SendSubtitle));
            EventHandlers.Add(TriggerNsToClient + "SetWaypoint", new Action<float, float>(API.SetWaypoint));
            EventHandlers.Add(TriggerNsToClient + "SetPlayerSkin", new Action<uint>(API.SetPlayerSkin));
            EventHandlers.Add(TriggerNsToClient + "SetPlayerFreeze", new Action<bool>(API.SetPlayerFreeze));
            EventHandlers.Add(TriggerNsToClient + "SetPlayerInvisible", new Action<bool>(API.SetPlayerInvisible));
            EventHandlers.Add(TriggerNsToClient + "TeleportPlayerToPosition", new Action<float, float, float>(API.TeleportPlayerToPosition));
            EventHandlers.Add(TriggerNsToClient + "PlayPlayerAnimation", new Action<string, string, int>(API.PlayPlayerAnimation));
            EventHandlers.Add(TriggerNsToClient + "StopPlayerAnimation", new Action(API.StopPlayerAnimation));
        }
    }
}