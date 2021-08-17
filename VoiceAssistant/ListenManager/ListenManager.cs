using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Dictionary<string, RecogniseData> recogniseDictionary;
        List<List<string>> choicesList;
        ListenSettings ls;

        float requaredConfidence = 0.85f;
        Keys restartKey = Keys.Home;
        //SpeechRecognitionEngine current_sre;
        //EventHandler<SpeechRecognizedEventArgs> onRecogniseCurrent;


        public ListenManager(ListenBuilder builder)
        {
            recogniseDictionary = builder.GetRecogniseDictionary;
            choicesList = builder.GetChoicesList;
            Init();
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

        //останавливает текущее прослушивание и запускает новое
        public void Start()
        {
            StartRecogniseAssistantName();
            //RecognitionServer.Init();
            //StopCurrentListening();
            //StartListenAssistantNameInternal(AssistantNameRecognised);
        }

        public void Restart()
        {
            //Deinit();
            //Init();
            //Start();
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



        #region Recognise


        /*void RecognisedInternal(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < requaredConfidence)
            {
                Debug.Log("не распознано \"" + e.Result.Text + "\". Точность " + e.Result.Confidence);
                return;
            }

            Debug.Log("Распознано \"" + e.Result.Text + "\". Точность " + e.Result.Confidence);
            StopCurrentListening();
            onRecogniseCurrent(sender, e);

        }*/

        /*void AssistantNameRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < requaredConfidence)
            {
                return;
            }

            StartListenServiceCommandInternal(ServiceCommandRecognised);
        }*/

        /*void ServiceCommandRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            string keyKommand = e.Result.Words.ElementAt(0).Text;
            string[] commands = new string[e.Result.Words.Count];

            for (int i = 0; i < e.Result.Words.Count; i++)
            {
                commands[i] = e.Result.Words.ElementAt(i).Text;
            }

            if (!recogniseDictionary.ContainsKey(keyKommand))
            {
                Debug.LogError("ключ " + keyKommand + " отсутствует в словаре");
                return;
            }

            recogniseDictionary[keyKommand].Recognised(commands);
        }*/


        #endregion Recognised


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
            if (key == restartKey /*&& PressKeyObserver.KeyPressed(Keys.LShiftKey)*/)
            {
                Restart();
            }
        }



        #endregion KeyPress


        void ChangeConfidence(float newConfidence)
        {
            requaredConfidence = newConfidence;
        }

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
