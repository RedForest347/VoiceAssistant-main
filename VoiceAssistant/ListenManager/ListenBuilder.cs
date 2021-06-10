using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    class ListenBuilder
    {
        Dictionary<string, RecogniseData> recogniseDictionary;
        List<List<string>> choicesList;

        public Dictionary<string, RecogniseData> GetRecogniseDictionary { get => recogniseDictionary; }
        public List<List<string>> GetChoicesList { get => choicesList; }

        public ListenBuilder()
        {

        }

        public void Build()
        {
            Init();
            LoadAllServices();

            DebugRecogniseDictionary();
        }

        public void Init()
        {
            recogniseDictionary = new Dictionary<string, RecogniseData>();
            choicesList = new List<List<string>>();
        }

        void LoadAllServices()
        {
            LoadService(new OpenFolderService());
            LoadService(new StartFileService());
        }

        void LoadService(ServiceBase service)
        {
            ServiceData data = service.GetInitData();
            AddServiceWordsToChoicesList(data);
            AddServiceToEvents(data);
        }

        void AddServiceWordsToChoicesList(ServiceData serviceData)
        {
            PrepareRecogniseDictionary(serviceData);
            PrepareChoicesList(serviceData);

            List<List<string>> wordGroups = serviceData.wordGroups;

            for (int group = 0; group < wordGroups.Count; group++)
            {
                choicesList[group].AddRange(wordGroups[group]);
                choicesList[group].Distinct();
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
                if (choicesList.Count <= group)
                {
                    choicesList.Add(new List<string>());
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
            Debug.Log("-----------Список всех команд-----------");
            for (int i = 0; i < choicesList.Count; i++)
            {
                Debug.Log("------------------------------------------");
                for (int j = 0; j < choicesList[i].Count; j++)
                {
                    Debug.Log("    " + (i + 1) + "." + (j + 1) + ") = " + choicesList[i][j]);
                }
            }
            Debug.Log("------------------------------------------");
        }
    }
}
