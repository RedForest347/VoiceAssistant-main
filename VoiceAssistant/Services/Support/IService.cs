﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    interface IService
    {

        ServiceData GetInitData();

        void OnRecognised(string[] recognisedWords, ListenManager lm);

        //void Init(ListenManager listenManager);

    }
}
