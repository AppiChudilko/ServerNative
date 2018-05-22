using System;
using System.Linq;
using CitizenFX.Core;

namespace Server.Native
{
    public class API : BaseScript 
    {   
        public static void SendNotificationToAll(string message, bool blink = true, bool saveToBrief = true)
        {
            TriggerClientEvent(Shared.TriggerNsToClient + "SendNotification", message, blink, saveToBrief);
        }

        public static void SendNotification(int playerServerId, string message, bool blink = true, bool saveToBrief = true)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SendNotification", message, blink, saveToBrief);
        }

        public static void SendPictureNotificationToAll(string text, string title, string subtitle, string icon, int type)
        {
            TriggerClientEvent(Shared.TriggerNsToClient + "SendPictureNotification", text, title, subtitle, icon, type);
        }

        public static void SendPictureNotification(int playerServerId, string text, string title, string subtitle, string icon, int type)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SendPictureNotification", text, title, subtitle, icon, type);
        }

        public static void SendSubtitleToAll(string message, int duration = 5000, bool drawImmediately = true)
        {
            TriggerClientEvent(Shared.TriggerNsToClient + "SendSubtitle", message, duration, drawImmediately);
        }

        public static void SendSubtitle(int playerServerId, string message, int duration = 5000, bool drawImmediately = true)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SendSubtitle", message, duration, drawImmediately);
        }
        
        public static void SetWaypoint(int playerServerId, float x, float y)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SetWaypoint", x, y);
        }
        
        public static void SetPlayerFreeze(int playerServerId, bool freeze)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SetPlayerFreeze", playerServerId, freeze);
        }
        
        public static void SetPlayerInvisible(int playerServerId, bool invisible)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "SetPlayerInvisible", playerServerId, invisible);
        }
        
        public static void PlayerTeleportToPosition(int playerServerId, float x, float y, float z)
        {
            TriggerClientEvent(ServerIdToPlayer(playerServerId), Shared.TriggerNsToClient + "PlayerTeleportToPosition", playerServerId, x, y, z);
        }
        
        public static int GetPlayerServerId(object handle)
        {
           return GetPlayerServerId(Convert.ToInt32(handle));
        }
        
        public static int GetPlayerServerId(int handle)
        {
            return handle <= 65535 ? handle : handle - 65535;
        }
        
        public static Player ServerIdToPlayer(int playerServerId)
        {
            return new PlayerList().FirstOrDefault(pl => GetPlayerServerId(pl.Handle) == playerServerId);
        }
    }
}