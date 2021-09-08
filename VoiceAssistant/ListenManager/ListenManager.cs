using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using VoiceAssistant.Handles;
using VoiceAssistant.Server;
using KeysConverter = VoiceAssistant.Handles.KeysConverter;

namespace VoiceAssistant
{
    /// <summary как создать и запустить объект данного класса:>
    /// 
    /// ListenBuilder builder = new ListenBuilder();
    /// builder.Build();
    /// ListenManager lm = new ListenManager(builder);
    /// lm.Start();
    /// 
    /// </summary>


    class ListenManager
    {
        Process recognitionClient;
        Dictionary<string, RecogniseData> recogniseDictionary;
        List<List<string>> choicesList;
        ListenSettings ls;

        float requaredConfidence = 0.85f;
        Keys restartKey = Keys.Home;


        public ListenManager(ListenBuilder builder)
        {
            recogniseDictionary = builder.GetRecogniseDictionary;
            choicesList = builder.GetChoicesList;
            Init();
            StartRecogniseClient();
            Form1.onExit += StopRecogniseClient;
        }

        //https://social.msdn.microsoft.com/Forums/ru-RU/a8d2016b-7dee-48d8-8d7b-856e7b91bfbe/c-108210721082-quot10891074107710881085109110901100quot?forum=fordesktopru
        //как свернуть программу
        void StartRecogniseClient()
        {
            string filePath = "recognise module\\client.exe";

            if (!File.Exists(filePath))
                return;

            recognitionClient = new Process();
            recognitionClient.StartInfo.FileName = Path.GetFileName(filePath);
            recognitionClient.StartInfo.WorkingDirectory = Path.GetDirectoryName(filePath);
            recognitionClient.Start();
        }

        void StopRecogniseClient()
        {
            if (recognitionClient != null)
            {
                recognitionClient.Kill();
            }
        }


        void InitServices()
        {
            foreach (var elem in recogniseDictionary)
            {
                for (int i = 0; i < elem.Value.services.Count; i++)
                {
                    elem.Value.services[i].SetLM(this);
                }
            }
        }


        public void Start()
        {
            StartRecogniseAssistantName();
        }

        public void Restart()
        {
            // мб добавить
        }

        public void Stop()
        {
            StopCurrentListening();
            Deinit();
        }

        void Init()
        {
            LoadListenSettings();
            InitServices();

            PressKeyObserver.onKeyDown += KeyPressed;
        }

        void LoadListenSettings()
        {
            ls = ListenSettings.Load();
            requaredConfidence = ls.confidence;
            Keys newRestartKey = KeysConverter.StringToKeys(ls.restartKey);

            if ((int)newRestartKey == -1)
            {
                Debug.LogWarning("некорректная клавиша перезагрузки в файле настроек. клавиша изменена на \"Home\"");
                newRestartKey = Keys.Home;
            }
            restartKey = newRestartKey;
        }

        void Deinit()
        {
            PressKeyObserver.onKeyDown -= KeyPressed;
        }


        void StopCurrentListening()
        {

        }

        public void ReturnControl()
        {
            Debug.Log("Сервис закончил свою работу. повторный запуск ListenManager");
            StartRecogniseAssistantName();
        }


        #region Recognise new

        List<string[]> currentChoices;
        Action<string> currentOnRecognise;

        public void StartRecognise(List<string[]> choses, Action<string> onRecognise)
        {
            StartRecogniseInternal(choses, onRecognise);
        }

        void StartRecogniseAssistantName()
        {
            List<string[]> choses = new List<string[]> { new string[] { ls.AssistantName } };
            StartRecogniseInternal(choses, AssistentNameRecognised);
        }

        void StartRecogniseInternal(List<string[]> choices, Action<string> onRecognise)
        {
            currentChoices = choices;
            currentOnRecognise = onRecognise;
            RecognitionServer.NewListenAsync(CheckingRecognizedPhrase);
        }

        void CheckingRecognizedPhrase(string phrase)
        {
            if (ChoicesListContainPhrase(currentChoices, phrase))
            {
                Debug.Log("распознано \"" + phrase + "\"");
                currentOnRecognise?.Invoke(phrase);
            }
            else
            {
                RecognitionServer.NewListenAsync(CheckingRecognizedPhrase);
            }
        }

        bool ChoicesListContainPhrase(List<string[]> chosesList, string phrase)
        {
            string tempPhrase = phrase.ToLower();
            bool wordWasFound = false;

            for (int i = 0; i < chosesList.Count; i++)
            {
                for (int j = 0; j < chosesList[i].Length; j++)
                {

                    if (tempPhrase.StartsWith(chosesList[i][j].ToLower()))
                    {
                        tempPhrase = tempPhrase.Remove(0, chosesList[i][j].Length).Trim();
                        wordWasFound = true;
                        break;
                    }
                }
                if (!wordWasFound)
                {
                    return false;
                }
                wordWasFound = false;
            }

            return tempPhrase.Length == 0;

        }


        void AssistentNameRecognised(string phrase)
        {
            Debug.Log("распознано имя голосового ассистента");
            StartRecogniseInternal(ListListsToListArrays(choicesList), OnServiseCommandRecognise);
        }

        List<string[]> ListListsToListArrays(List<List<string>> list)
        {
            List<string[]> _out = new List<string[]>();

            for (int i = 0; i < list.Count; i++)
            {
                _out.Add(list[i].ToArray());
            }
            return _out;
        }

        void OnServiseCommandRecognise(string phrase)
        {
            string keyKommand = phrase.Split(new char[] {' ' })[0];
            string[] commands = phrase.Split(new char[] { ' ' });

            if (!recogniseDictionary.ContainsKey(keyKommand))
            {
                Debug.LogError("ключ " + keyKommand + " отсутствует в словаре");
                return;
            }
            Debug.Log("распознана ключевая команда \"" + keyKommand + "\". вся фраза: \"" + phrase + "\"");
            recogniseDictionary[keyKommand].Recognised(commands);
        }

        #endregion Recognise new


        #region Start Listen


        /*public void StartListenAssistantName(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            StartListenAssistantNameInternal(onRecognise);
        }*/

        /*void StartListenAssistantNameInternal(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            List<string[]> choses = new List<string[]>();
            choses.Add(new string[] { ls.AssistantName });
            onRecogniseCurrent = onRecognise;

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += RecognisedInternal;
            //sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("Recognize Completed");
            current_sre = sre;

            Debug.Log("Ожидание вызова голосового ассистента");
        }*/

        /*public void StartListenServiceCommand(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            StartListenServiceCommandInternal(onRecognise);
        }*/

        /*void StartListenServiceCommandInternal(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            List<string[]> choses = new List<string[]>();
            onRecogniseCurrent = onRecognise;

            for (int i = 0; i < choicesList.Count; i++)
            {
                choses.Add(choicesList[i].ToArray());
            }

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += RecognisedInternal;
            //sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("Recognize Completed");
            current_sre = sre;

            Debug.Log("Ожидание команды");
        }*/

        /*public void StartListenCustomCommand(List<string[]> choses, EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            
            StartListenCustomCommandInternal(choses, onRecognise);
        }*/

        /*void StartListenCustomCommandInternal(List<string[]> choses, EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            onRecogniseCurrent = onRecognise;

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += RecognisedInternal;
            //sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("Recognize Completed");
            current_sre = sre;

            Debug.Log("Ожидание команды");
        }*/


        #endregion Start Listen


        #region KeyPress


        void KeyPressed(Keys key)
        {
            if (key == restartKey)
            {
                Restart();
            }
        }



        #endregion KeyPress


        private class ListenSettings
        {

            [JsonIgnore]
            static string filePath = @"data\settings.txt";

            [JsonProperty("Имя голосового ассистента")]
            public string AssistantName { get; set; }

            [JsonProperty("Имя пользователя")]
            public string userName { get; set; }

            [JsonProperty("Кнопка перезапуска голосового ассистента")]
            public string restartKey { get; set; }

            [JsonProperty("Требуемая точность")]
            public float confidence { get; set; }

            public ListenSettings()
            {
                AssistantName = "Алиса";
                userName = "Рэд";
            }

            public static ListenSettings CreateNew()
            {
                return new ListenSettings();
            }

            public static ListenSettings Load()
            {
                ListenSettings ls = FileHandler.LoadFromFile<ListenSettings>(filePath);
                Debug.Log("Параметры голосового ассистента успешно загружены");
                return ls;

            }

            public static void Save(ListenSettings settings)
            {

                FileHandler.SaveToFile(filePath, settings);
                Debug.Log("настройки голосового ассистента успешно сохранены");
            }
        }
    }
}
