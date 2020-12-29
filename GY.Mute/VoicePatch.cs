using System;
using HarmonyLib;
using SDG.Unturned;

namespace GY.Mute
{
    [HarmonyPatch(typeof(SteamChannel), nameof(SteamChannel.sendVoicePacket), typeof(SteamPlayer), typeof(byte[]), typeof(int))]
    public class VoicePatch
    {
        [HarmonyPrefix]
        public static bool SendVoicePatch(SteamPlayer player, byte[] packet, int packetSize)
        {
            if (!Mute.UsersData.VoiceMutes.TryGetValue(player.playerID.steamID.m_SteamID, out var muteData)) return true;

            if (!((DateTime.Now - muteData.VoiceMuteTime).TotalSeconds >= muteData.VoiceMuteSeconds)) return false;
            
            Mute.UsersData.VoiceMutes.Remove(player.playerID.steamID.m_SteamID);
            JsonHelper.Save();
            
            return true;

        }
    }
}