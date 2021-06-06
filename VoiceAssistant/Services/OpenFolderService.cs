using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    class OpenFolderService : ServiceBase
    {
        List<string> firstWords = new List<string> {"открой", "закрой" };
        List<string> secondWords = new List<string> {"блокнот", "книгу" };
        List<string> thirdWords = new List<string> {"сейчас", "через время" };

        //возвращает набор слов в формате: [первые ключевые слова],[вторые ключевые слова]...[]
        public override ServiceData GetInitData()
        {
            List<List<string>> initData = new List<List<string>> { firstWords, secondWords, thirdWords };

            return new ServiceData(initData, this, GetType().Name);
        }

        public override void OnRecognised(string[] recognisedWords)
        {
            string _out = "сервис OpenFolderService. распознаны слова: ";
            for (int i = 0; i < recognisedWords.Length; i++)
            {
                _out += recognisedWords[i] + ", ";
            }
            Debug.Log(_out);
        }

        /*public override void Init(ListenManager listenManager)
        {

        }*/

    }
}
