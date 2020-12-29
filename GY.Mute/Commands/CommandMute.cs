using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace GY.Mute.Commands
{
    public class CommandMute : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "mute";
        public string Help => "";
        public string Syntax => "/Mute [nick] [time]";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string>{"gy.mute"};
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 2 || !double.TryParse(command[1], out var time))
            {
                UnturnedChat.Say(caller, Mute.Instance.Translate("command_invalid", Syntax), Color.red);
                return;
            }

            var target = UnturnedPlayer.FromName(command[0]);

            if (target == null)
            {
                UnturnedChat.Say(caller, Mute.Instance.Translate("player_not_found"), Color.red);
                return;
            }

            if (Mute.UsersData.ChatMutes.ContainsKey(target.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(caller, Mute.Instance.Translate("player_already_muted", target.DisplayName), Color.red);
                return;
            }
            
            Mute.UsersData.ChatMutes.Add(target.CSteamID.m_SteamID, new ChatMute(time));
            JsonHelper.Save();

            var timeStr = Mute.ToTimeString(TimeSpan.FromSeconds(time));
            
            UnturnedChat.Say(caller, Mute.Instance.Translate("command_voice_unmute", target.DisplayName, timeStr), Color.cyan);
            UnturnedChat.Say(target, Mute.Instance.Translate("chat_now_muted", timeStr), Color.yellow);
        }
    }
}