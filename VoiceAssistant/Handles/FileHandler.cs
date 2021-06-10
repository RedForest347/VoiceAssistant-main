using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceAssistant.Handles
{
    static class FileHandler
    {

        public static void SaveToFile<T>(string filePath, T data)
        {
            FileStream fstream = new FileStream(filePath, FileMode.Create);
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            fstream.Write(Encoding.Default.GetBytes(json), 0, json.Length);
            fstream.Close();
        }

        public static T LoadFromFile<T>(string filePath) where T : new()
        {
            FileStream fstream = File.OpenRead(filePath);
            byte[] array = new byte[fstream.Length];
            fstream.Read(array, 0, array.Length);

            string json = Encoding.Default.GetString(array);
            T folderData;

            try
            {
                folderData = JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonReaderException)
            {
                folderData = new T();
                Debug.LogError("Файл " + filePath + " поврежден или содержит некорректные данные");
            }
            catch(Exception e)
            {
                folderData = new T();
                Debug.LogError(e.Message);
            }

            fstream.Close();
            return folderData;
        }
    }
}
