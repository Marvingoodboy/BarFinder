using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace BarFinder
{
    class DataProcessor
    {
        private const string BAR_DATA_URL = "https://op.mos.ru/EHDWSREST/catalog/export/get?id=788381";
        private static byte[] GetData()
        {
            WebClient client = new WebClient();
            byte[] archive = client.DownloadData(BAR_DATA_URL);
            return archive;
        }

        public string UnZipToMemory()
        {
            var zippedBuffer = GetData();
            using (var zippedStream = new MemoryStream(zippedBuffer))
            {
                using (var archive = new ZipArchive(zippedStream))
                {
                    var entry = archive.Entries.FirstOrDefault();

                    if (entry != null)
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var ms = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(ms);
                                var unzippedArray = ms.ToArray();

                                return Encoding.Default.GetString(unzippedArray);
                            }
                        }
                    }

                    return null;
                }
            }
        }
    }
}
