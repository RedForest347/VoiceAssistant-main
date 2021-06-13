using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceAssistant.Handles;

namespace VoiceAssistant
{
    class PressButtonTankService : ServiceBase
    {
        List<string> firstWords = new List<string>() {"танки" };
        List<string> secondWords = new List<string>() {" " };
        Dictionary<string, string> commandDictionary;

        //набор слов в формате: [первые ключевые слова],[вторые ключевые слова]...
        public override ServiceData GetInitData()
        {
            InitCommandDictionary();
            List<List<string>> initData = new List<List<string>> { firstWords, secondWords };

            return new ServiceData(initData, this, GetType().Name);
        }

        public override void OnRecognised(string[] recognisedWords)
        {
            Debug.Log("сервис голосового управления пресет \"танки\" получил управление");
            StartRecognise();
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

        void StartRecognise()
        {
            List<string[]> choses = ConstructChosesFromDataFile();
            lm.StartListenCustomCommand(choses, OnRecogniseCustomCommand);
        }

        List<string[]> ConstructChosesFromDataFile()
        {
            List<string[]> choses = new List<string[]>();
            List<PressButtonData> buttonDatas = PressButtonData.Load();
            choses.Add(new string[buttonDatas.Count]);

            for (int i = 0; i < buttonDatas.Count; i++)
            {
                choses[0][i] = buttonDatas[i].commandName;
            }

            string[] additionalCommand = GetAdditionalCommand();
            choses[0] = choses[0].Union(additionalCommand).ToArray();
            return choses;
        }

        string[] GetAdditionalCommand()
        {
            return new string[] { "закрыть сервис" };
        }

        void OnRecogniseCustomCommand(object sender, Microsoft.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            string command = e.Result.Text;
            Debug.Log("команда " + command + " распознана. Этот сервис " + (commandDictionary.ContainsKey(command) ? "" : "не ") + "содержит эту команду");

            if (command == "закрыть сервис")
            {
                Debug.Log("необходимо закрыть сервис");
                ReturnControl();
                return;
            }

            Debug.Log("будет нажата клавиша " + commandDictionary[command]);

            StartRecognise();
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

        void ReturnControl()
        {
            Debug.Log("сервис голосового управления пресет \"танки\" вернул управление ListenManager_у");
            ReturnControlToListenManager();
        }



        void DebugChoses(List<string[]> choses)
        {
            Debug.Log("--------Start debug-------");

            for (int i = 0; i < choses.Count; i++)
            {
                Debug.Log("---------Level " + i + "------------");
                for (int j = 0; j < choses[i].Length; j++)
                {
                    Debug.Log(choses[i][j]);
                }
            }

            Debug.Log("--------End debug-------");
        }

        private class PressButtonData
        {
            [JsonIgnore]
            static string filePath = @"data\pressButtonTank.swmodel";

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
