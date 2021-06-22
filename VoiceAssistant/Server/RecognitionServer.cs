using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// последовательность работы сервера:
/// 1)  инициализация и создание подключения
/// 2)  ожидание приема информации от клиента
///     2.1) получение инфо от клиента (распознанной речи)
/// 3)  посылается ответ клиетнту, который означает необходимость продолжения прослушивания
/// 4)  возврат к пункту 2
/// 
/// 
/// сообщения клиенту:
///     continue
///     end
/// 
/// 
/// 
/// </summary>



namespace VoiceAssistant.Server
{
    class RecognitionServer
    {
        static int port = 8005; // порт для приема входящих запросов
        static Socket listenSocket;
        static Socket handler;

        static bool initialized = false;
        static bool connectionIaAlive = false;

        public static void Init()
        {
            Form1.onExit += Deinit;
        }

        public static void Deinit()
        {
            SendMessageToClient("end");

            CloseConnection(handler);
            handler = null;
            CloseConnection(listenSocket);
            listenSocket = null;

            initialized = false;
        }

        public static async void NewListenAsync(Action<string> onRecognise)
        {
            if (!initialized)
            {
                await Task.Run(InitListen);
            }
            else
            {
                SendMessageToClient("continue");
                CloseConnection(handler);
            }

            string clientMessage = await Task.Run(ListenMessage);
            Message mes = ConvertJsonMes(clientMessage);

            onRecognise?.Invoke(mes.Text);
        }

        static void InitListen()
        {
            // создаем сокет
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);           
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            // связываем сокет с локальной точкой, по которой будем принимать данные
            listenSocket.Bind(ipPoint);
            // начинаем прослушивание
            listenSocket.Listen(1);

            initialized = true;
            Debug.Log("сервер начал свою работу");     
        }

        static string ListenMessage()
        {
            try
            {
                while (true)
                {
                    handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string clientMessage = builder.ToString();

                    return clientMessage;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("[ERROR] " + ex.Message);
            }
            return "";
        }

        static Message ConvertJsonMes(string clientMessage)
        {
            try
            {
                Message mes = JsonConvert.DeserializeObject<Message>(clientMessage);
                return mes;
            }
            catch (Exception e)
            {
                Debug.Log("[ERROR] " + e.Message);
            }

            return new Message() { Text = "" };
        }


        static void SendMessageToClient(string answer)
        {
            // отправляем ответ
            byte[] data = Encoding.UTF8.GetBytes(answer);
            handler.Send(data);
        }

        static void CloseConnection(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            //Debug.Log("соединение закрыто");
        }

        /*public static void Abort()
        {
            Debug.Log("ConnectionInAvalible = " + ConnectionInAvalible(handler));

            //handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
        }*/

        /*static bool ConnectionInAvalible(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            //bool part2 = (s.Available == 0);
            bool part2 = true;
            if ((part1 && part2) || !s.Connected)
                return false;
            else
                return true;
        }*/
    }


    //класс, соответствующий принимаемой от Py клиента информации
    class Message
    {
        [JsonProperty("Text")]
        public string Text { get; set; }

        public Message() { }
    }


    class MessageAdvansed
    {

    }
}
