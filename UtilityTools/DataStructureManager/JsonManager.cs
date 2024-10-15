using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UtilityTools.DataStructureManager
{
    public class JsonManager
    {
        public static T DeserializeObjectByFilePath<T>(string filePath)
        {
            if (filePath == null) return default;

            if (File.Exists(filePath))
            {
                string textFile = File.ReadAllText(filePath);
                if (string.IsNullOrEmpty(textFile))
                    return default;

                return JsonConvert.DeserializeObject<T>(textFile);
            }
            else
            {
                return default;
            }
        }
    }
}
