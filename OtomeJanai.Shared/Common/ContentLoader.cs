using SharpCompress.Archive;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading.Tasks;

namespace OtomeJanai.Shared.Common
{
    internal static class ContentLoader
    {
        internal static Stream GetFileStream(string filePath)
        {
            var pattern = @"[A-Z_a-z][A-Z_a-z0-9\.]*";
            Regex regex = new Regex(pattern);
            var matches = regex.Matches(filePath);
            var archive = ArchiveFactory.Open(File.OpenRead($"{matches[0].Value}.7z"));
            var filePathInZip = filePath.Remove(0, matches[0].Value.Length + 1);
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
