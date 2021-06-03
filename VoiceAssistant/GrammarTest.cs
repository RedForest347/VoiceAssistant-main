//using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition.SrgsGrammar;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;

namespace VoiceAssistant
{
    class GrammarTest
    {
        public static void Start()
        {
            SpecificStart();
            return;
        }

        static void SpecificStart()
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-Ru");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);
            sre.SetInputToDefaultAudioDevice();

            //sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;

            gb.Append(new Choices(new string[]{" ", "открой", "закрой" }));
            gb.Append(new Choices(new string[]{" ", "новый", "старый"}));
            gb.Append(new Choices(new string[] { " ", "блокнот", "майнкрафт" }));

            Grammar g = new Grammar(gb);

            sre.LoadGrammar(g);

            sre.SpeechDetected += SpecificSpeechDetected;
            sre.RecognizeCompleted += SpecificRecognizeCompleted;
            sre.SpeechRecognized += SpecificSpeechRecognized;
            sre.SpeechHypothesized += SpecificSpeechHypothesized;

            sre.RecognizeAsync(RecognizeMode.Multiple);
            Debug.Log("SpecificStart loaded");
        }

        static void SpecificSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence > .7f)
            {
                Debug.Log("утверждение. найдено " + e.Result.Words.Count + " слов. текст = " + e.Result.Text + " Confidence = " + e.Result.Confidence);
            }
        }

        static void SpecificSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //Debug.Log("гепотеза. найдено " + e.Result.Words.Count + " слов. текст = " + e.Result.Text + " Confidence = " + e.Result.Confidence);
        }
        static void SpecificSpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            //Debug.Log("начата речь");
        }
        static void SpecificRecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            //Debug.Log("распознавание закончено Confidence = " + e.Result.Confidence);
        }




        //Процедура распознавания
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Debug.Log(e.Result.Text);
            LogResult(e);


            SpeechSynthesizer speaker = new SpeechSynthesizer();
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.Speak(e.Result.Text);
        }


        static void LogResult(SpeechRecognizedEventArgs e)
        {
            Debug.Log("text = " + e.Result.Text);

            for (int i = 0; i < e.Result.Words.Count; i++)
            {
                RecognizedWordUnit wordUnit = e.Result.Words.ElementAt(i);
                Debug.Log(i + ") text = " + wordUnit.Text + " Confidence = " + wordUnit.Confidence + " Pronunciation = " + wordUnit.Pronunciation);
            }
        }

    }
}
