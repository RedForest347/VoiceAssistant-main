using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using Newtonsoft.Json;

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



        public ListenManager(ListenBuilder builder)
        {
            recogniseDictionary = builder.GetRecogniseDictionary;
            choicesList = builder.GetChoicesList;
            ls = ListenSettings.Load();

            /*if (recogniseDictionary.ContainsKey("открой"))
            {
                recogniseDictionary["открой"].Recognised(new string[] { "открой", "новый", "блокнот" });
            }*/
        }

        void Init()
        {

        }

        public void Start()
        {
            StartListenAssistentName(ListenAssistentNameRecognised);
        }

        void ListenAssistentNameRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            
            Debug.Log("Распознано имя " + e.Result.Text + ". Точность " + e.Result.Confidence);
        }

        void StartListenAssistentName(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            /**System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-Ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;

            gb.Append(new Choices(new string[] { ls.assistentName }));

            Grammar g = new Grammar(gb);

            sre.LoadGrammar(g);

            sre.SpeechRecognized += onRecognise;
            sre.RecognizeAsync(RecognizeMode.Multiple);*/

            List<string[]> choses = new List<string[]>();
            choses.Add(new string[] {ls.assistentName });

            SpeechRecognitionEngine sre = StartRecognise(choses, RecognizeMode.Multiple);
            sre.SpeechRecognized += onRecognise;
            Debug.Log("Ожидание вызова голосового ассистента");
        }

        SpeechRecognitionEngine StartRecognise(List<string[]> choices, RecognizeMode recognizeMode)
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

            //sre.SpeechRecognized += onRecognise;
            sre.RecognizeAsync(RecognizeMode.Multiple);

            return sre;
        }

        public void Stop()
        {

        }

        private class ListenSettings
        {

            [JsonIgnore]
            static string filePath = @"data\settings.txt";

            [JsonProperty("Имя голосового ассистента")]
            public string assistentName { get; set; }

            [JsonProperty("Имя пользователя")]
            public string userName { get; set; }

            private ListenSettings()
            {
                assistentName = "Алиса";
                userName = "Рэд";
            }

            public static ListenSettings CreateNew()
            {
                return new ListenSettings();
            }

            public static ListenSettings Load()
            {
                StreamReader sr = new StreamReader(filePath);
                string json = sr.ReadToEndAsync().Result;

                ListenSettings ls;
                try
                {
                    ls = JsonConvert.DeserializeObject<ListenSettings>(json);
                }
                catch (JsonReaderException)
                {
                    ls = new ListenSettings();
                    Debug.Log("Файл поврежден или содержит некорректные данные");
                }

                sr.Close();
                return ls;
            }

            public static void Save(ListenSettings settings)
            {
                
                FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate);

                string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

                fstream.Write(Encoding.Default.GetBytes(json), 0, json.Length);
                fstream.Close();
                Debug.Log("настройки успешно сохранены");
            }





        }
    }

}
