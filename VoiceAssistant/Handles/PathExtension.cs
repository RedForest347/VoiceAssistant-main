using System;
using System.Collections.Generic;
using System.IO;

namespace VoiceAssistant.Handles
{
    static class PathExtension
    {
        public static string GetFilePathWithoutArguments(string filePathWithArguments)
        {
            string fileDirectory = Path.GetDirectoryName(filePathWithArguments);
            string fileName = filePathWithArguments.Remove(0, fileDirectory.Length + 1).Split()[0];
            return Path.Combine(fileDirectory, fileName);
        }

        public static string GetArguments(string filePathWithArguments)
        {
            string filePath = GetFilePathWithoutArguments(filePathWithArguments);
            return filePathWithArguments.Remove(0, filePath.Length);
        }

        public static bool FilePathContainArguments(string filePathWithArguments)
        {
            string arguments = GetArguments(filePathWithArguments);
            Debug.Log("file " + filePathWithArguments + " " + (arguments.Trim().Length > 0) + " arguments = " + arguments);
            return arguments.Trim().Length > 0;
        }

    }
}
