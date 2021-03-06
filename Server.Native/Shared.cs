﻿using System;
using CitizenFX.Core;

namespace Server.Native
{
    public class Shared : BaseScript 
    {
        public static readonly string TriggerNsToServer = "NativeAPI:ToServer:";
        public static readonly string TriggerNsToClient = "NativeAPI:ToClient:";
        
        public Shared()
        {
            EventHandlers.Add(TriggerNsToServer + "SendNotification", new Action<int, string, bool, bool>(API.SendNotification));
            EventHandlers.Add(TriggerNsToServer + "SendNotificationToAll", new Action<string, bool, bool>(API.SendNotificationToAll));
            EventHandlers.Add(TriggerNsToServer + "SendPictureNotification", new Action<int, string, string, string, string, int>(API.SendPictureNotification));
            EventHandlers.Add(TriggerNsToServer + "SendPictureNotificationToAll", new Action<string, string, string, string, int>(API.SendPictureNotificationToAll));
            EventHandlers.Add(TriggerNsToServer + "SendSubtitle", new Action<int, string, int, bool>(API.SendSubtitle));
            EventHandlers.Add(TriggerNsToServer + "SendSubtitleToAll", new Action<string, int, bool>(API.SendSubtitleToAll));
            EventHandlers.Add(TriggerNsToServer + "SetWaypoint", new Action<int, float, float>(API.SetWaypoint));
            EventHandlers.Add(TriggerNsToServer + "SetPlayerSkin", new Action<int, uint>(API.SetPlayerSkin));
            EventHandlers.Add(TriggerNsToServer + "SetPlayerFreeze", new Action<int, bool>(API.SetPlayerFreeze));
            EventHandlers.Add(TriggerNsToServer + "SetPlayerInvisible", new Action<int, bool>(API.SetPlayerInvisible));
            EventHandlers.Add(TriggerNsToServer + "TeleportPlayerToPosition", new Action<int, float, float, float>(API.TeleportPlayerToPosition));
            EventHandlers.Add(TriggerNsToServer + "PlayPlayerAnimation", new Action<int, string, string, int>(API.PlayPlayerAnimation));
            EventHandlers.Add(TriggerNsToServer + "StopPlayerAnimation", new Action<int>(API.StopPlayerAnimation));
        }
    }
}