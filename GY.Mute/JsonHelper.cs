using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rocket.Core;

namespace GY.Mute
{
    public static class JsonHelper
    {
        private static readonly string DataBaseFile = Mute.Instance.Directory + "/UsersData.json";
        
        public static void Save()
        {
            File.WriteAllText(DataBaseFile, JsonConvert.SerializeObject(Mute.UsersData, Formatting.Indented));
        }

        public static UserData Read()
        {
            if (File.Exists(DataBaseFile))
            {
                return JsonConvert.DeserializeObject<UserData>(File.ReadAllText(DataBaseFile));
            }
            
            Save();
            return new UserData();
        }
        
    }
}