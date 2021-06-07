using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VoiceAssistant
{
    class OpenFolderService : ServiceBase
    {
        List<string> firstWords = new List<string> {"открой"};
        List<string> secondWords = new List<string>/*();*/ {"блокнот", "книгу" };
        Dictionary<string, string> commandDictionary;
        //List<string> thirdWords = new List<string> {"сейчас", "через время" };

        
        string[] currentCommand;

        //набор слов в формате: [первые ключевые слова],[вторые ключевые слова]...
        public override ServiceData GetInitData()
        {
            InitCommandDictionary();
            //secondWords = GetCommandList();
            List<List<string>> initData = new List<List<string>> { firstWords, secondWords };

            return new ServiceData(initData, this, GetType().Name);
        }

        public override void OnRecognised(string[] recognisedWords)
        {
            currentCommand = recognisedWords;
            OpenFolderData.Save(new List<OpenFolderData>() { new OpenFolderData(), new OpenFolderData(), new OpenFolderData() });
            DoOpenFolder();
            ReturnControlToListenManager();
        }

        void InitCommandDictionary()
        {
            List<OpenFolderData> folderData = OpenFolderData.Load();
            commandDictionary = new Dictionary<string, string>();

            for (int i = 0; i < folderData.Count; i++)
            {
                Debug.Log("Add " + folderData[i].commandName);
                //commandDictionary.Add(folderData[i].commandName, folderData[i].folderPath);
            }
        }

        List<string> GetCommandList()
        {
            /*List<string> commands = new List<string>(commandDictionary.Count);

            foreach (var data in commandDictionary)
            {
                commands.Add(data.Value);
            }*/

            return commandDictionary.Values.ToList();
        }

        void DoOpenFolder()
        {
            /*switch (selection)
            {
                case "Y":
                    Console.WriteLine("Вы нажали букву Y");
                    break;
                case "N":
                    Console.WriteLine("Вы нажали букву N");
                    break;
                default:
                    Console.WriteLine("Вы нажали неизвестную букву");
                    break;
            }*/
            System.Diagnostics.Process.Start("explorer");
        }





        private class OpenFolderData
        {
            [JsonIgnore]
            static string filePath = @"data\foldersData.swmodel";

            [JsonProperty("command name")]
            public string commandName { get; set; }// = "мой компьютер";

            [JsonProperty("Имя папки")]
            public string folderPath { get; set; }// = "explorer";

            public OpenFolderData()
            {
                commandName = "мой компьютер";
                folderPath = "explorer";
            }

            public static void Save(List<OpenFolderData> folderData)
            {
                FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate);
                string json = JsonConvert.SerializeObject(folderData, Formatting.Indented);
                //Debug.Log("json save = " + json);

                List<OpenFolderData> des = JsonConvert.DeserializeObject<List<OpenFolderData>>(json);
                //Debug.Log("DDD = " + des[2].commandName);

                fstream.Write(Encoding.Default.GetBytes(json), 0, json.Length);
                fstream.Close();
                Debug.Log("сохранено");
            }

            public static List<OpenFolderData> Load()
            {
                StreamReader sr = new StreamReader(filePath);
                string json = sr.ReadToEndAsync().Result;
                //Debug.Log("json = " + json);
                List<OpenFolderData> folderData;
                try
                {
                    folderData = JsonConvert.DeserializeObject<OpenFolderData[]>(json).ToList();
                }
                catch (JsonReaderException)
                {
                    folderData = new List<OpenFolderData>();
                    Debug.Log("Файл поврежден или содержит некорректные данные");
                }

                sr.Close();
                Debug.Log("Load");

                /*for (int i = 0; i < folderData.Count; i++)
                {
                    Debug.Log(i + ") " + folderData[i].commandName);
                }*/

                return folderData;
            }
        }

    }
}
