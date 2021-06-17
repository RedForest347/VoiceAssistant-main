using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Newtonsoft.Json;
using VoiceAssistant.Handles;
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
        SpeechRecognitionEngine current_sre;
        EventHandler<SpeechRecognizedEventArgs> onRecogniseCurrent;


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
            StopCurrentListening();
            StartListenAssistantNameInternal(AssistantNameRecognised);
        }

        public void Restart()
        {
            Deinit();
            Init();
            Start();
        }

        public void Stop()
        {
            StopCurrentListening();
            Deinit();
        }

        void Init()
        {
            ls = ListenSettings.Load();
            requaredConfidence = ls.confidence;
            Keys newKey = KeysConverter.StringToKeys(ls.restartKey);

            if ((int)newKey == -1)
            {
                Debug.LogWarning("некорректная клавиша перезагрузки в файле настроек. клавиша изменена на \"Home\"");
                newKey = Keys.Home;
            }
            restartKey = newKey;

            InitServices();
            PressKeyObserver.onKeyDown += KeyPressed;
        }

        void Deinit()
        {
            PressKeyObserver.onKeyDown -= KeyPressed;
        }


        void StopCurrentListening()
        {
            if (current_sre != null)
            {
                current_sre.RecognizeAsyncStop();
                current_sre.RecognizeAsyncCancel();
                current_sre = null;
            }
        }

        public void ReturnControl()
        {
            Debug.Log("Сервис закончил свою работу. повторный запуск ListenManager");
            StartListenAssistantNameInternal(AssistantNameRecognised);
        }


        #region Recognise


        void RecognisedInternal(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < requaredConfidence)
            {
                Debug.Log("не распознано \"" + e.Result.Text + "\". Точность " + e.Result.Confidence);
                return;
            }

            Debug.Log("Распознано \"" + e.Result.Text + "\". Точность " + e.Result.Confidence);
            StopCurrentListening();
            onRecogniseCurrent(sender, e);

        }

        void AssistantNameRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < requaredConfidence)
            {
                return;
            }

            StartListenServiceCommandInternal(ServiceCommandRecognised);
        }

        void ServiceCommandRecognised(object sender, SpeechRecognizedEventArgs e)
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
        }

        SpeechRecognitionEngine PrepareSpeechRecognition(List<string[]> choices)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-Ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;

            for (int i = 0; i < choices.Count; i++)
            {
                gb.Append(new Choices(choices[i]));
            }

            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            return sre;
        }


        #endregion Recognised


        #region Start Listen


        public void StartListenAssistantName(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            StartListenAssistantNameInternal(onRecognise);
        }

        void StartListenAssistantNameInternal(EventHandler<SpeechRecognizedEventArgs> onRecognise)
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
        }

        public void StartListenServiceCommand(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            StartListenServiceCommandInternal(onRecognise);
        }

        void StartListenServiceCommandInternal(EventHandler<SpeechRecognizedEventArgs> onRecognise)
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
        }

        public void StartListenCustomCommand(List<string[]> choses, EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            
            StartListenCustomCommandInternal(choses, onRecognise);
        }

        void StartListenCustomCommandInternal(List<string[]> choses, EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            onRecogniseCurrent = onRecognise;

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += RecognisedInternal;
            //sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("Recognize Completed");
            current_sre = sre;

            Debug.Log("Ожидание команды");
        }


        #endregion Start Listen


        #region KeyPress


        void KeyPressed(Keys key)
        {
            if (key == restartKey && PressKeyObserver.KeyPressed(Keys.LShiftKey))
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
