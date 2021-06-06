using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{

    class RecogniseData
    {
        public event Action<string[], ListenManager> onRecognise;

        public List<ServiceBase> services;

        public RecogniseData()
        {
            onRecognise = null;
            services = new List<ServiceBase>();
        }

        public void Recognised(string[] words, ListenManager lm)
        {
            Debug.Log("Recognised " + (onRecognise == null));
            onRecognise?.Invoke(words, lm);
        }
    }
}
