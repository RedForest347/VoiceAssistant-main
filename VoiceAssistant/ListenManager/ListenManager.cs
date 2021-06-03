using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        List<List<string>> ChoicesList;



        public ListenManager()
        {

        }

        public void Init()
        {
            recogniseDictionary = new Dictionary<string, RecogniseData>();
            ChoicesList = new List<List<string>>();

            LoadAllServices();

            DebugRecogniseDictionary();
        }

        public void Start()
        {

        }

        public void Stop()
        {

        }


        void LoadAllServices()
        {
            LoadService(new OpenFolderService());

            recogniseDictionary["открой"].Recognised(new string[] {"открой", "новый", "блокнот" });
            //AddServiceWordsToChoicesList(new OpenFolderService().GetInitData());
        }

        void LoadService(ServiceBase service)
        {
            ServiceData data = service.GetInitData();
            AddServiceWordsToChoicesList(data);
            AddServiceToEvents(data);
            //service.Init(this);
        }

        void AddServiceWordsToChoicesList(ServiceData serviceData)
        {
            PrepareRecogniseDictionary(serviceData);
            PrepareChoicesList(serviceData);

            List<List<string>> wordGroups = serviceData.wordGroups;

            for (int group = 0; group < wordGroups.Count; group++)
            {
                ChoicesList[group].AddRange(wordGroups[group]);
                ChoicesList[group].Distinct();
            }
        }

        //создаются все недостающие ключи
        void PrepareRecogniseDictionary(ServiceData serviceData)
        {
            List<List<string>> wordGroups = serviceData.wordGroups;
            
            if (wordGroups.Count != 0 && wordGroups[0].Count != 0)
            {
                for (int word = 0; word < wordGroups[0].Count; word++)
                {
                    string key = wordGroups[0][word];
                    if (!recogniseDictionary.ContainsKey(key))
                    {
                        recogniseDictionary.Add(key, new RecogniseData());
                    }
                    else
                    {
                        Debug.LogWarning("ключевое слово " + key + " присутствует в другом сервисе, помимо сервиса"
                            + serviceData.serviceName + ". это может вызвать ложные вызовы");
                    }
                }
            }


        }

        //добавляет необходимые уровни ключевых слов
        void PrepareChoicesList(ServiceData serviceData)
        {
            List<List<string>> wordGroups = serviceData.wordGroups;

            for (int group = 0; group < wordGroups.Count; group++)
            {
                if (ChoicesList.Count <= group)
                {
                    ChoicesList.Add(new List<string>());
                }
            }
        }

        void AddServiceToEvents(ServiceData serviceData)
        {
            List<List<string>> wordGroups = serviceData.wordGroups;

            if (wordGroups.Count != 0 && wordGroups[0].Count != 0)
            {
                for (int word = 0; word < wordGroups[0].Count; word++)
                {
                    string key = wordGroups[0][word];
                    recogniseDictionary[key].onRecognise += serviceData.service.OnRecognised;
                    recogniseDictionary[key].services.Add(serviceData.service);
                }
            }
        }


        void DebugRecogniseDictionary()
        {
            //Debug.LogWarning("Start DebugRecogniseDictionary");

            for (int i = 0; i < ChoicesList.Count; i++)
            {
                Debug.Log("------------------------------------------");
                for (int j = 0; j < ChoicesList[i].Count; j++)
                {
                    Debug.Log("    " + (i + 1) + "." + (j + 1) + ") = " + ChoicesList[i][j]);
                }
            }
            Debug.Log("------------------------------------------");




            //Debug.LogWarning("End DebugRecogniseDictionary");
        }

    }

}
