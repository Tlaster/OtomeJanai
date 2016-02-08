using SharpCompress.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace OtomeJanai.Shared.Common
{
    internal static class ContentLoader
    {
        /// <summary>
        /// Return file stream which is not seekable
        /// </summary>
        /// <param name="filePath">File path,the first pattern will be the 7z file name</param>
        /// <returns></returns>
        internal static Stream GetFileStream(string filePath)
        {
            var pattern = @"[A-Z_a-z][A-Z_a-z0-9\.]*";
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(filePath);
            var fstream = TitleContainer.OpenStream($"{matches[0].Value}.7z");
            var stream = new MemoryStream();
            fstream.CopyTo(stream);
            var archive = ArchiveFactory.Open(stream);
            var filePathInZip = filePath.Remove(0, matches[0].Value.Length + 1);
            //OpenEntryStream will return ReadOnlySubStream which is not seekable
            return archive.Entries.Where(item => item.Key == filePathInZip).FirstOrDefault()?.OpenEntryStream();
        }

        internal static async Task<string> GetFileString(string filePath)
        {
            return await GetFileString(filePath, Encoding.BigEndianUnicode);
        }


        internal static async Task<string> GetFileString(string filePath,Encoding encoding)
        {
            using (var reader = new StreamReader(GetFileStream(filePath), encoding))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
