using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
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
        
        
        public static void PlayPlayerAnimation(int playerServerId, string name, string name2, int flag = 49)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "PlayPlayerAnimation", playerServerId, name, name2, flag);
        }

        public static async void PlayPlayerAnimation(string name, string name2, int flag = 49)
        {
            RequestAnimDict(name);
            while (!HasAnimDictLoaded(name))
                await Delay(1);

            TaskPlayAnim(GetPlayerPed(-1), name, name2, 8f, -8, -1, flag, 0, false, false, false);
        }
        
        public static void StopPlayerAnimation(int playerServerId)
        {
            TriggerServerEvent(Shared.TriggerNsToServer + "StopPlayerAnimation", playerServerId);
        }

        public static void StopPlayerAnimation()
        {
            ClearPedSecondaryTask(GetPlayerPed(-1));
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

        public static void CallNative(string nativeName, params InputArgument[] args)
        {
            foreach(uint hash in Enum.GetValues(typeof(Hash)))
            {
                string name = Enum.GetName(typeof(Hash), hash);
                if (nativeName != name) continue;

                switch (args.GetLength(0))
                {
                    case 1:
                        Function.Call((Hash) hash, args[0]);
                        break;
                    case 2:
                        Function.Call((Hash) hash, args[0], args[1]);
                        break;
                    case 3:
                        Function.Call((Hash) hash, args[0], args[1], args[2]);
                        break;
                    case 4:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3]);
                        break;
                    case 5:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4]);
                        break;
                    case 6:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5]);
                        break;
                    case 7:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                        break;
                    case 8:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                        break;
                    case 9:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                        break;
                    case 10:
                        Function.Call((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                        break;
                
                }
            }
        }

        //idk, work it or not
        public static T CallNativeWithReturn<T>(string nativeName, params InputArgument[] args)
        {
            foreach(uint hash in Enum.GetValues(typeof(Hash)))
            {
                string name = Enum.GetName(typeof(Hash), hash);
                if (nativeName != name) continue;

                switch (args.GetLength(0))
                {
                    case 1:
                        return Function.Call<T>((Hash) hash, args[0]);
                    case 2:
                        return Function.Call<T>((Hash) hash, args[0], args[1]);
                    case 3:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2]);
                    case 4:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3]);
                    case 5:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4]);
                    case 6:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5]);
                    case 7:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    case 8:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    case 9:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    case 10:
                        return Function.Call<T>((Hash) hash, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8], args[9]);
                
                }
                
                return Function.Call<T>((Hash) hash);
            }
            
            //lol
            return Function.Call<T>(Hash._RETURN_TWO);
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