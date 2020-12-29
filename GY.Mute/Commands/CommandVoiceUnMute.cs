using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;

namespace GY.Mute.Commands
{
    public class CommandVoiceUnMute : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "VoiceUnMute";
        public string Help => "";
        public string Syntax => "/VoiceUnMute [nick]";
        public List<string> Aliases => new List<string>{"vum"};
        public List<string> Permissions => new List<string>{"gy.voice.unmute"};
        
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
            
            if (!Mute.UsersData.VoiceMutes.Remove(target.CSteamID.m_SteamID))
            {
                UnturnedChat.Say(caller, Mute.Instance.Translate("player_vmute_not_found", target.DisplayName), Color.red);
                return;
            }

            JsonHelper.Save();
            
            UnturnedChat.Say(caller, Mute.Instance.Translate("command_unmute", target.DisplayName), Color.cyan);
            UnturnedChat.Say(target, Mute.Instance.Translate("vchat_unmuted"), Color.magenta);
            
        }
    }
}