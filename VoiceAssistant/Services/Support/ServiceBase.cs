using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    class ServiceBase : IService
    {
        protected ListenManager lm;

        public void SetLM(ListenManager lm)
        {
            this.lm = lm;
        }

        public virtual ServiceData GetInitData()
        {
            List<List<string>> initData = new List<List<string>>();

            return new ServiceData(initData, null, "empty");
        }

        public virtual void OnRecognised(string[] recognisedWords)
        {

        }

        protected virtual void ReturnControlToListenManager()
        {
            lm.ReturnControl();
        }
    }
}
