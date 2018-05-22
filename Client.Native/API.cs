using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client.Native
{
    public class API : BaseScript 
    {
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
        
        public static void SetWaypoint(int playerServerId, float x, float y)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SetWaypoint", playerServerId, x, y);
        }

        public static void SetWaypoint(float x, float y)
        {
            World.WaypointPosition = new Vector3(x, y, 0);
        }

        public static void SetPlayerSkin(int playerServerId, uint hash)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SetPlayerSkin", playerServerId, hash);
        }

        public static void SetPlayerSkin(string modelName)
        {
            SetPlayerSkin((uint) GetHashKey(modelName));
        }

        public static async void SetPlayerSkin(uint hash)
        {
            RequestModel(hash);
            while (!HasModelLoaded(hash))
            {
                RequestModel(hash);
                await Delay(1);
            }

            SetPlayerModel(PlayerId(), hash);
            SetModelAsNoLongerNeeded(hash);
        }
        
        public static void SetPlayerFreeze(int playerServerId, bool freeze)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SetPlayerFreeze", playerServerId, freeze);
        }
        
        public static void SetPlayerFreeze(bool freeze)
        {
            int playerId = PlayerId();
            var ped = GetPlayerPed(playerId);
            
            SetPlayerControl(playerId, !freeze, 0);
            if (!freeze)
                FreezeEntityPosition(ped, false);
            else
            {
                FreezeEntityPosition(ped, true);
                
                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }
        
        public static void SetPlayerInvisible(int playerServerId, bool invisible)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "SetPlayerInvisible", playerServerId, invisible);
        }
        
        public static void SetPlayerInvisible(bool invisible)
        {
            int playerId = PlayerId();
            var ped = GetPlayerPed(playerId);
            
            if (!invisible)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);
                
                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);
        
                SetPlayerInvincible(playerId, false);
            } 
            else 
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);
        
                SetEntityCollision(ped, false, true);
                SetPlayerInvincible(playerId, true);
            }
        }
        
        public static void TeleportPlayerToPosition(int playerServerId, float x, float y, float z)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "TeleportPlayerToPosition", playerServerId, x, y, z);
        }
        
        public static async void TeleportPlayerToPosition(float x, float y, float z)
        {
            DoScreenFadeOut(500);

            while (IsScreenFadingOut())
                await Delay(1);

            NetworkFadeOutEntity(GetPlayerPed(-1), true, true);
            SetPlayerFreeze(true);
            
            SetEntityCoords(GetPlayerPed(-1), x, y, z, true, false, false, true);

            await Delay(500);

            SetPlayerFreeze(false);
            
            await Delay(500);
            NetworkFadeInEntity(GetPlayerPed(-1), false);
            
            DoScreenFadeIn(500);
            
            while (IsScreenFadingIn())
                await Delay(1);
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