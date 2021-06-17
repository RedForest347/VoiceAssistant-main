using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoiceAssistant.Handles
{
    static class KeysConverter
    {

        // return Keys by its name or -1 if name is wrong
        public static Keys StringToKeys(string keyName)
        {
            if (Enum.TryParse(keyName, out Keys key))
            {
                return key;
            }
            else
            {
                return (Keys)(-1);
            }
        }

        public static string KeysToString(Keys key)
        {
            return key.ToString();
        }

    }
}
