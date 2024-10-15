using System;
using System.Collections.Generic;
using System.Text;
using UtilityTools.DataStructureManager;

namespace UtilityTools.ExtensionMethod
{
    public static class StringExtensions
    {
        public static T XmlStringToObject<T>(this string xmlString)
        {
            return XmlManager.DeserializeFromString<T>(xmlString);
        }
        public static T XmlPathToObject<T>(this string pathFile)
        {
            return XmlManager.DeserializeFromPath<T>(pathFile);
        }
    }
}
