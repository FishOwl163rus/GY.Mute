using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace GY.Mute.Commands
{
    public class CommandUnMute : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "UnMute";
        public string Help => "";
        public string Syntax => "/UnMute [nick]";
        public List<string> Aliases => new List<string>{"um"};
        public List<string> Permissions => new List<string>{"gy.unmute"};
        
        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1)
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
            
            if (!Mute.UsersData.ChatMutes.Remove(target.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(caller, Mute.Instance.Translate("player_mute_not_found", target.DisplayName), Color.red);
                return;
            }

            JsonHelper.Save();
            
            UnturnedChat.Say(caller, Mute.Instance.Translate("command_unmute", target.DisplayName), Color.cyan);
            UnturnedChat.Say(target, Mute.Instance.Translate("chat_unmuted"), Color.magenta);
            
        }
    }
}