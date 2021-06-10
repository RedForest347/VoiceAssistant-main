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
            //OpenFileData.Save(new List<OpenFileData>() {new OpenFileData(), new OpenFileData() });
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

        void DoOpenFolder(string[] recognisedWords)
        {
            string command = ConvertToCommand(recognisedWords);

            if (!commandDictionary.ContainsKey(command))
            {
                Debug.LogError("Сервис " + GetType() + " не содержит команды " + command);
                return;
            }

            string filePath = commandDictionary[command];

            if (filePath.EndsWith(".bat"))
            {
                OpenBatFile(filePath);
            }
            else
            {
                StandartOpen(filePath);
            }

        }

        void StandartOpen(string filePath)
        {
            OpenWithArguments(filePath);
            //Process.Start(filePath);
        }



        void OpenBatFile(string filePath)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = Path.GetFileName(filePath);
            proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
            proc.Start();
        }

        bool ContainParams()
        {
            return false; ///
        }

        void OpenWithArguments(string filePathWithArguments)
        {
            //Enumerable.ToArray
            string filePath = filePathWithArguments.Split(new char[] { ' ' })[0];
            string fileArguments = filePathWithArguments.Remove(0, filePath.Length);
            //Debug.Log("filePath =" + filePath + " fileParams =" + fileParams + ".");

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
