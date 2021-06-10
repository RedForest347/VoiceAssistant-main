using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VoiceAssistant.Handles;

namespace VoiceAssistant
{
    class OpenFolderService : ServiceBase
    {
        List<string> firstWords = new List<string> {"открой"};
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
            DoOpenFolder(recognisedWords);
            ReturnControlToListenManager();
        }

        void InitCommandDictionary()
        {
            List<OpenFolderData> folderData = OpenFolderData.Load();
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

        void DoOpenFolder(string[] recognisedWords)
        {
            string command = ConvertToCommand(recognisedWords);

            if (!commandDictionary.ContainsKey(command))
            {
                Debug.LogError("Сервис " + GetType() + " не содержит команды " + command);
                return;
            }

            string filePath = commandDictionary[command];

            System.Diagnostics.Process.Start(filePath);
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



        private class OpenFolderData
        {
            [JsonIgnore]
            static string filePath = @"data\foldersData.swmodel";

            [JsonProperty("Имя команды"), JsonRequired]
            public string commandName { get; set; }

            [JsonProperty("Имя папки"), JsonRequired]
            public string folderPath { get; set; }

            public OpenFolderData()
            {
                commandName = "мой компьютер";
                folderPath = "explorer";
            }

            public static void Save(List<OpenFolderData> folderData)
            {
                FileHandler.SaveToFile(filePath, folderData);
            }

            public static List<OpenFolderData> Load()
            {
                return FileHandler.LoadFromFile<List<OpenFolderData>>(filePath);
            }
        }

    }
}
