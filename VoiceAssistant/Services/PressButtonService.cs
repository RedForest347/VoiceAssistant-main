using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceAssistant.Handles;

namespace VoiceAssistant
{
    class PressButtonService : ServiceBase
    {
        List<string> firstWords;
        List<string> secondWords = new List<string>() {" " };
        Dictionary<string, string> commandDictionary;

        //набор слов в формате: [первые ключевые слова],[вторые ключевые слова]...
        public override ServiceData GetInitData()
        {
            InitCommandDictionary();
            firstWords = GetCommandList();
            List<List<string>> initData = new List<List<string>> { firstWords, secondWords };

            return new ServiceData(initData, this, GetType().Name);
        }

        public override void OnRecognised(string[] recognisedWords)
        {
            DoPressButton(recognisedWords);
            ReturnControlToListenManager();
        }

        void InitCommandDictionary()
        {
            List<PressButtonData> folderData = PressButtonData.Load();
            commandDictionary = new Dictionary<string, string>();

            for (int i = 0; i < folderData.Count; i++)
            {
                commandDictionary.Add(folderData[i].commandName, folderData[i].buttonString);
            }
        }

        List<string> GetCommandList()
        {
            return commandDictionary.Keys.ToList();
        }

        void DoPressButton(string[] recognisedWords)
        {
            string recognisedWord = recognisedWords[0];
            if (!commandDictionary.ContainsKey(recognisedWord))
            {
                Debug.LogError("PressButtonService не содержит команду " + recognisedWord);
                return;
            }
            string requaredButton = commandDictionary[recognisedWord];
            Debug.Log("необходимо нажать \"" + requaredButton + "\"");
        }


        private class PressButtonData
        {
            [JsonIgnore]
            static string filePath = @"data\pressButton.swmodel";

            [JsonProperty("Имя команды"), JsonRequired]
            public string commandName { get; set; }

            [JsonProperty("Нажать кнопку"), JsonRequired]
            public string buttonString { get; set; }

            public PressButtonData()
            {
                commandName = "мой компьютер";
                buttonString = "D";
            }

            public static void Save(List<PressButtonData> folderData)
            {
                FileHandler.SaveToFile(filePath, folderData);
            }

            public static List<PressButtonData> Load()
            {
                return FileHandler.LoadFromFile<List<PressButtonData>>(filePath);
            }
        }


    }
}
