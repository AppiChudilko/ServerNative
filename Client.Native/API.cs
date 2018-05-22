using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client.Native
{
    public class API : BaseScript 
    {
        public static void SetWaypoint(int playerServerId, float x, float y)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SetWaypoint", playerServerId, x, y);
        }

        public static void SetWaypoint(float x, float y)
        {
            World.WaypointPosition = new Vector3(x, y, 0);
        }

        public static void SendNotificationToAll(string message, bool blink = true, bool saveToBrief = true)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendNotificationToAll", message, blink, saveToBrief);
        }

        public static void SendNotification(int playerServerId, string message, bool blink = true, bool saveToBrief = true)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendNotification", playerServerId, message, blink, saveToBrief);
        }

        public static void SendNotification(string message, bool blink = true, bool saveToBrief = true)
        {
            SetNotificationTextEntry("THREESTRINGS");
            foreach (string msg in StringToArray(message))
                if (msg != null)
                    AddTextComponentSubstringPlayerName(msg);
            DrawNotification(blink, saveToBrief);
        }

        public static void SendPictureNotificationToAll(string text, string title, string subtitle, string icon, int type)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendPictureNotificationToAll", text, title, subtitle, icon, type);
        }

        public static void SendPictureNotification(int playerServerId, string text, string title, string subtitle, string icon, int type)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendPictureNotification", playerServerId, text, title, subtitle, icon, type);
        }
        
        public static void SendPictureNotification(string text, string title, string subtitle, string icon, int type)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(text);
            SetNotificationMessage(icon, icon, true, type, title, subtitle);
            DrawNotification(false, true);
        }

        public static void SendSubtitleToAll(string message, int duration = 5000, bool drawImmediately = true)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendSubtitleToAll", message, duration, drawImmediately);
        }

        public static void SendSubtitle(int playerServerId, string message, int duration = 5000, bool drawImmediately = true)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SendSubtitle", playerServerId, message, duration, drawImmediately);
        }
        
        public static void SendSubtitle(string message, int duration = 5000, bool drawImmediately = true)
        {
            BeginTextCommandPrint("THREESTRINGS");
            foreach (var msg in StringToArray(message))
                AddTextComponentSubstringPlayerName(msg);
            EndTextCommandPrint(duration, drawImmediately);
        }
        
        public static int GetPlayerServerId()
        {
            return CitizenFX.Core.Native.API.GetPlayerServerId(PlayerId());
        }
        
        public static string[] StringToArray(string inputString)
        {
            string[] outputString = new string[3];

            var lastSpaceIndex = 0;
            var newStartIndex = 0;
            outputString[0] = inputString;

            if (inputString.Length <= 99) return outputString;
            
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString.Substring(i, 1) == " ")
                {
                    lastSpaceIndex = i;
                }

                if (inputString.Length > 99 && i >= 98)
                {
                    if (i == 98)
                    {
                        outputString[0] = inputString.Substring(0, lastSpaceIndex);
                        newStartIndex = lastSpaceIndex + 1;
                    }
                    if (i > 98 && i < 198)
                    {
                        if (i == 197)
                        {
                            outputString[1] = inputString.Substring(newStartIndex, (lastSpaceIndex - (outputString[0].Length - 1)) - (inputString.Length - 1 > 197 ? 1 : -1));
                            newStartIndex = lastSpaceIndex + 1;
                        }
                        else if (i == inputString.Length - 1 && inputString.Length < 198)
                        {
                            outputString[1] = inputString.Substring(newStartIndex, ((inputString.Length - 1) - outputString[0].Length));
                            newStartIndex = lastSpaceIndex + 1;
                        }
                    }
                        
                    if (i <= 197) continue;
                        
                    if (i == inputString.Length - 1 || i == 296)
                    {
                        outputString[2] = inputString.Substring(newStartIndex, ((inputString.Length - 1) - outputString[0].Length) - outputString[1].Length);
                    }
                }
            }

            return outputString;
        }
    }
}