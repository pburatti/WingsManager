using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WingsManager.Model.Imports;

namespace WingsManager.BLL
{
    public class Common
    {
        public static async Task<WingsXmlDocument> GetWingsXmlDocumentByFile(string fileName, CancellationToken cancellationToken)
        {
            WingsXmlDocument wingsXmlDocument = null;
            FileStream xmlFileStream = null;
            try
            {
                xmlFileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                if (xmlFileStream.CanRead && xmlFileStream.Length > 0)
                {
                    wingsXmlDocument = await WingsXmlDocument.GetInstance(xmlFileStream, cancellationToken);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (xmlFileStream != null)
                {
                    xmlFileStream.Close();
                    await xmlFileStream.DisposeAsync();
                }
            }
            return wingsXmlDocument;
        }
    }
}
