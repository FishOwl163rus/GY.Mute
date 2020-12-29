using System;
using System.Collections.Generic;

namespace GY.Mute
{
    public class UserData
    {
        public Dictionary<ulong, ChatMute> ChatMutes { get; set; } = new Dictionary<ulong, ChatMute>();
        public Dictionary<ulong, VoiceMute> VoiceMutes { get; set; } = new Dictionary<ulong, VoiceMute>();
    }
    public class ChatMute
    {
        public ChatMute(double muteSeconds)
        {
            MuteChatSeconds = muteSeconds;
        }
        public DateTime ChatMuteTime { get; set; } = DateTime.Now;
        public double MuteChatSeconds { get; set; }
    }
    
    public class VoiceMute
    {
        public VoiceMute(double muteSeconds)
        {
            VoiceMuteSeconds = muteSeconds;
        }
        
        public DateTime VoiceMuteTime { get; set; } = DateTime.Now;
        public double VoiceMuteSeconds { get; set; }
    }
}