using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace GY.Mute
{
    public class Mute : RocketPlugin<Config>
    {
        public static UserData UsersData;
        public static Mute Instance;
        private static Harmony _harmonyInstance;
        
        public override TranslationList DefaultTranslations => new TranslationList
        {
            {"command_invalid", "Команда введена неверно, используйте {0}."},
            {"player_not_found", "Игрок не найден!"},
            
            {"chat_now_muted", "Вам заблокировали чат на {0}"},
            {"chat_still_muted", "Вы не можете писать в чат еще {0}"},
            {"chat_unmuted", "Вам разблокировали чат."},
            
            {"command_mute", "Вы заблокировали чат игрока {0} на {1}"},
            {"command_unmute","Вы заблокировали чат игрока {0}."},
            {"player_already_muted", "У игрока {0} уже заблокирован чат."},
            {"player_mute_not_found", "У игрока {0} нет блокировки чата."},
            
            {"vchat_now_muted", "Вам заблокировали voice-чат на {0}"},
            {"vchat_unmuted", "Вам разблокировали voice-чат."},
            
            {"command_voice_mute", "Вы заблокировали voice-чат игрока {0} на {1}"},
            {"command_voice_unmute", "Вы разблокировали voice-чат игрока {0}"},
            {"player_already_vmuted", "У игрока {0} уже заблокирован voice-чат."},
            {"player_vmute_not_found", "У игрока {0} нет блокировки voice-чата."},
        };

        protected override void Load()
        {
            Instance = this;
            UsersData = JsonHelper.Read();
            
            _harmonyInstance = new Harmony("gy.harmony.instance");
            _harmonyInstance.PatchAll();
            
            UnturnedPlayerEvents.OnPlayerChatted += EventOnPlayerChatted;
        }
        

        private void EventOnPlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode mode, ref bool cancel)
        {
            if (!UsersData.ChatMutes.TryGetValue(player.CSteamID.m_SteamID, out var muteData)) return;

            var totalSec = (DateTime.Now - muteData.ChatMuteTime).TotalSeconds;
            if (totalSec >= muteData.MuteChatSeconds)
            {
                UsersData.ChatMutes.Remove(player.CSteamID.m_SteamID);
                JsonHelper.Save();
                return;
            }

            cancel = true;
            UnturnedChat.Say(player, Translate("chat_still_muted", ToTimeString(TimeSpan.FromSeconds(muteData.MuteChatSeconds - totalSec)), Color.gray));
        }

        public static string ToTimeString(TimeSpan span)
        {
           return string.Join("", span.ToString("d'д. 'h'ч. 'm'м. 's'c.'").Split(' ').Where(s => s[0] != '0'));
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerChatted -= EventOnPlayerChatted;
            _harmonyInstance.UnpatchAll();
            _harmonyInstance = null;
        }
    }
}