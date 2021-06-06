﻿using System;
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

        SpeechRecognitionEngine current_sre;
        EventHandler<SpeechRecognizedEventArgs> onRecogniseCurrent;


        public ListenManager(ListenBuilder builder)
        {
            recogniseDictionary = builder.GetRecogniseDictionary;
            choicesList = builder.GetChoicesList;
            ls = ListenSettings.Load();
        }

        void Init()
        {

        }

        public void Start()
        {
            StartListenAssistentNameBase(AssistentNameRecognised);
        }

        /*public void StartListenAssistentName(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            StartListenAssistentNameBase(onRecognise);
        }*/

        void Recognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.85f)
            {
                return;
            }

            Debug.Log("Распознано " + e.Result.Text + ". Точность " + e.Result.Confidence);
            StopCurrentListening();
            onRecogniseCurrent(sender, e);

        }

        void StartListenAssistentNameBase(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            List<string[]> choses = new List<string[]>();
            choses.Add(new string[] {ls.assistentName });
            onRecogniseCurrent = onRecognise;

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += Recognised;
            sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("RecognizeCompleted");
            current_sre = sre;

            Debug.Log("Ожидание вызова голосового ассистента");
        }

        void AssistentNameRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.85f)
            {
                return;
            }

            StopCurrentListening();
            StartListedCommand(SendMessageToServices);
        }

        void StartListedCommand(EventHandler<SpeechRecognizedEventArgs> onRecognise)
        {
            List<string[]> choses = new List<string[]>();
            onRecogniseCurrent = onRecognise;

            for (int i = 0; i < choicesList.Count; i++)
            {
                choses.Add(choicesList[i].ToArray());
            }

            SpeechRecognitionEngine sre = PrepareSpeechRecognition(choses);
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += SendMessageToServices;
            sre.RecognizeCompleted += (object sender, RecognizeCompletedEventArgs e) => Debug.Log("RecognizeCompleted");
            current_sre = sre;
            
            Debug.Log("Ожидание команды");
        }

        void SendMessageToServices(object sender, SpeechRecognizedEventArgs e)
        {
            string keyKommand = e.Result.Words.ElementAt(0).Text;
            string[] commands = new string[e.Result.Words.Count];

            for (int i = 0; i < e.Result.Words.Count; i++)
            {
                commands[i] = e.Result.Words.ElementAt(i).Text;
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
                //Debug.Log("добавлена команда " + choices[i]);
                gb.Append(new Choices(choices[i]));
            }

            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            return sre;
        }

        void StopCurrentListening()
        {
            current_sre.RecognizeAsyncCancel();
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
