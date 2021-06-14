using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceAssistant.Handles;

namespace VoiceAssistant
{
    class StartFileService : ServiceBase
    {
        List<string> firstWords = new List<string> { "запусти" };
        List<string> secondWords;
        Dictionary<string, string> commandDictionary;

        //набор слов в формате: [первые ключевые слова],[вторые ключевые слова]...
        public override ServiceData GetInitData()
        {
            InitCommandDictionary();
            secondWords = GetCommandList();
            List<List<string>> initData = new List<List<string>> { firstWords, secondWords };

            return new ServiceData(initData, this, GetType().Name);
        }

        public override void OnRecognised(string[] recognisedWords)
        {
            DoOpenFile(recognisedWords);
            ReturnControlToListenManager();
        }

        void InitCommandDictionary()
        {
            List<OpenFileData> folderData = OpenFileData.Load();
            commandDictionary = new Dictionary<string, string>();

            for (int i = 0; i < folderData.Count; i++)
            {
                commandDictionary.Add(folderData[i].commandName, folderData[i].folderPath);
            }
        }

        List<string> GetCommandList()
        {
            return commandDictionary.Keys.ToList();
        }

        void DoOpenFile(string[] recognisedWords)
        {
            string command = ConvertToCommand(recognisedWords);

            if (!commandDictionary.ContainsKey(command))
            {
                Debug.LogError("Сервис " + GetType() + " не содержит команды " + command);
                return;
            }
            System.Collections.Generic.List<int> ddd = new System.Collections.Generic.List<int>();

            ddd[7] = 3;
            string filePath = commandDictionary[command];

            if (filePath.EndsWith(".bat"))
            {
                BatFileOpen(filePath);
            }
            else
            {
                StandartOpen(filePath);
            }

        }

        void StandartOpen(string filePath)
        {
            if (PathExtension.FilePathContainArguments(filePath))
            {
                OpenFileWithArguments(filePath);
            }
            else
            {
                OpenFile(filePath);
            }
        }



        void BatFileOpen(string filePath)
        {
            if (PathExtension.FilePathContainArguments(filePath))
            {
                OpenBatFileWithArguments(filePath);
            }
            else
            {
                OpenBatFileWithoutArguments(filePath);
            }
        }

        void OpenBatFileWithArguments(string filePath)
        {
            string filePathWithoutArguments = PathExtension.GetFilePathWithoutArguments(filePath);
            string arguments = PathExtension.GetArguments(filePath);

            Process proc = new Process();
            proc.StartInfo.FileName = Path.GetFileName(filePathWithoutArguments);
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(filePathWithoutArguments);
            proc.StartInfo.Arguments = arguments;

            proc.Start();
        }

        void OpenBatFileWithoutArguments(string filePath)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Path.GetFileName(filePath);
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(filePath);

            proc.Start();
        }

        void OpenFile(string filePath)
        {
            Process.Start(filePath);
        }

        void OpenFileWithArguments(string filePathWithArguments)
        {
            string filePath = PathExtension.GetFilePathWithoutArguments(filePathWithArguments);
            string fileArguments = PathExtension.GetArguments(filePathWithArguments);

            ProcessStartInfo info = new ProcessStartInfo(filePath);
            info.Arguments = fileArguments;
            Process.Start(info);
        }



        string ConvertToCommand(string[] text)
        {
            string command = "";

            for (int i = 1; i < text.Length; i++)
            {
                command += text[i] + " ";
            }

            return command.Trim();
        }



        private class OpenFileData
        {
            [JsonIgnore]
            static string filePath = @"data\openFileData.swmodel";

            [JsonProperty("Имя команды"), JsonRequired]
            public string commandName { get; set; }

            [JsonProperty("Имя файла"), JsonRequired]
            public string folderPath { get; set; }

            public OpenFileData()
            {
                commandName = "мой компьютер";
                folderPath = "explorer";
            }

            public static void Save(List<OpenFileData> folderData)
            {
                FileHandler.SaveToFile(filePath, folderData);
            }

            public static List<OpenFileData> Load()
            {
                return FileHandler.LoadFromFile<List<OpenFileData>>(filePath);
            }
        }
    }
}
