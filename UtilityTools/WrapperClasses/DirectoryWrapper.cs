using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UtilityTools.WrapperClasses
{
    public static class DirectoryWrapper
    {
        public static bool ExistsIfNotCreate(string path) 
        {
            if (string.IsNullOrEmpty(path))
                return false;

            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            return Directory.Exists(Path.GetDirectoryName(path));
        }

        public static bool FileIsBusy(FileInfo fi)
        {
            bool isUsed = false;
            if (fi.Length == 0)
                return true;
            FileStream stream = null;

            try
            {
                stream = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                isUsed = true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return isUsed;
        }

    }

}
