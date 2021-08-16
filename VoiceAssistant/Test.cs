using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace VoiceAssistant
{
    class Test
    {

        public static void Listen()
        {
            StartRecognise();
        }

        static void OnRecognised(string words)
        {
            Debug.Log("распознано " + words);
        }

        static async void StartRecognise()
        {
            string recognisedWords = await Task.Run(StartPython);
            OnRecognised(recognisedWords);
        }

        static string StartPython()
        {
            string text = "new text";

            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string python39 = @"C:\Users\Admin\AppData\Local\Programs\Python\Python39\";
            engine.SetSearchPaths(new string[] { python39 + @"Lib",
                                                 python39 + @"Lib\site-packages\vosk",
                                                 python39 + @"Lib\site-packages",
                                                 python39 + @"Lib\site-packages\vosk\libvosk.dll"});
            Prepare(engine);
            //scope.SetVariable("text", text);
            engine.ExecuteFile("test.py", scope);
            dynamic function = scope.GetVariable("result");
            // вызываем функцию и получаем результат
            dynamic result = function();
            return ((string)result);
        }

        static void Prepare(ScriptEngine engine)
        {
            string dir = Path.GetDirectoryName(@"C:\Users\Admin\AppData\Local\Programs\Python\Python39\Lib");
            ICollection<string> paths = engine.GetSearchPaths();

            if (!String.IsNullOrWhiteSpace(dir))
            {
                paths.Add(dir);
            }
            else
            {
                paths.Add(Environment.CurrentDirectory);
            }
            engine.SetSearchPaths(paths);
        }
    }
}
