using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeleteAllDuplicates
{
    static class Program
    {
        static void Main(string[] args)
        {
            string cwd = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(cwd);
            Dictionary<long, List<string>> possibleDuplicates = new Dictionary<long, List<string>>();

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                long fileSize = fileInfo.Length;
                if (!possibleDuplicates.Any(kvp => kvp.Key == fileSize))
                {
                    possibleDuplicates.Add(fileSize, new List<string>() { file });
                }
                else
                {
                    possibleDuplicates[fileSize].Add(file);
                }
            }

            foreach (KeyValuePair<long,List<string>> kvp in possibleDuplicates)
            {
                var dupes = kvp.Value;
                int jShouldStartFrom = 1;

                for (int i = 0; i < dupes.Count; i++)
                {
                    bool isDeleted = false;
                    for (int j = jShouldStartFrom; j < dupes.Count; j++)
                    {
                        if (dupes[i].IsFileEqualTo(dupes[j]))
                        {
                            File.SetAttributes(dupes[i], FileAttributes.Normal);
                            File.Delete(dupes[i]);
                            isDeleted = true;
                            jShouldStartFrom++;
                            break;
                        }
                    }
                    if (isDeleted)
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Are the twp given files, the same?
        /// </summary>
        /// <param name="filePath1">First file path</param>
        /// <param name="filePath2">Second file path</param>
        /// <returns>Whether files are binary equivalent or not.</returns>
        public static bool IsFileEqualTo(this string filePath1, string filePath2) 
            => File.ReadAllBytes(filePath1).SequenceEqual(File.ReadAllBytes(filePath2));
    }
}
