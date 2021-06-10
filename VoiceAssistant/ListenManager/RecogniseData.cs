using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{

    class RecogniseData
    {
        public event Action<string[]> onRecognise;

        public List<ServiceBase> services;

        public RecogniseData()
        {
            onRecognise = null;
            services = new List<ServiceBase>();
        }

        public void Recognised(string[] words)
        {
            onRecognise?.Invoke(words);
        }
    }
}
