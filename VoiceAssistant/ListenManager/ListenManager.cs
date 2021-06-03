using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VoiceAssistant
{
    /// <summary>
    /// ListenManager lm = new();
    /// lm.Init();
    /// lm.Start();
    /// </summary>


    class ListenManager
    {
        Dictionary<string, RecogniseData> recogniseDictionary;
        List<List<string>> choicesList;



        public ListenManager(ListenBuilder builder)
        {
            recogniseDictionary = builder.GetRecogniseDictionary;
            choicesList = builder.GetChoicesList;

            if (recogniseDictionary.ContainsKey("открой"))
            {
                recogniseDictionary["открой"].Recognised(new string[] { "открой", "новый", "блокнот" });
            }
        }

        void Init()
        {
            recogniseDictionary = new Dictionary<string, RecogniseData>();
            choicesList = new List<List<string>>();
        }

        public void Start()
        {
            string content;
            StreamReader sr = new StreamReader(@"data\settings.txt");
            //Read the first line of text
            content = sr.ReadLine();
            Debug.Log(content);

            ListenData ld = new ListenData();
            ld.assistentName = "Алиса";
            ld.userName = "Рэд";

            //string json = JsonSerializer.Serialize(ld);
            //ld = JsonSerializer.Deserialize<ListenData>(json);
            //Debug.Log(JsonSerializer.Serialize(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("Рэд"))));
            string json = JsonConvert.SerializeObject(ld, Formatting.Indented);
            Debug.Log(json);
            Debug.Log(JsonConvert.DeserializeObject<ListenData>(json).userName);
            //Debug.Log(ld.userName);
            
        }

        public void Stop()
        {

        }





        private class ListenData
        {
            //[JsonPropertyName("assistentName")]
            [Newtonsoft.Json.JsonIgnore]
            public string assistentName { get; set; }

            //[JsonPropertyName("userName")]
            [Newtonsoft.Json.JsonProperty("ddd")]
            public string userName { get; set; }

        }
    }

}
