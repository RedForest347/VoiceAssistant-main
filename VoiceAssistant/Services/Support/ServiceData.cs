using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    class ServiceData
    {
        public List<List<string>> wordGroups;
        public ServiceBase service;
        public string serviceName;

        public ServiceData(List<List<string>> wordGroups, ServiceBase service, string serviceName)
        {
            this.wordGroups = wordGroups;
            this.service = service;
            this.serviceName = serviceName;
        }
    }
}
