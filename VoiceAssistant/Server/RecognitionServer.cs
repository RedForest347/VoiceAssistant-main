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
/// сообщения от клиента:
///     client started
///     распознанный текст
/// 
/// </summary>



namespace VoiceAssistant.Server
{
    class RecognitionServer
    {
        static int port = 8005; // порт для приема входящих запросов
        public static Socket listenSocket;
        public static Socket handler;

        static bool initialized = false;
        static bool connectionIaAlive = false;

        public static void Init()
        {
            Form1.onExit += Deinit;
        }

        public static void Deinit()
        {
            connectionIaAlive = false;

            if (handler != null)
            {
                SendMessageToClient("end");
            }

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
                handler = null;
            }

            string clientMessage = await Task.Run(ListenMessage);

            if (clientMessage == "client started")
            {
                Debug.Log("client started");
                NewListenAsync(onRecognise);
                return;
            }
            if (String.IsNullOrEmpty(clientMessage))
            {
                if (connectionIaAlive)
                {
                    NewListenAsync(onRecognise);
                    Debug.LogWarning("Empty clientMessage");
                    return;
                }
                else
                {
                    return;
                }
            }

            Message message = ConvertJsonToMes(clientMessage);
            onRecognise?.Invoke(message.Text);
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
            connectionIaAlive = true;
            Debug.Log("сервер начал свою работу");     
        }

        static string ListenMessage()
        {
            try
            {
                while (true)
                {
                    //Debug.Log("before Accept");
                    handler = listenSocket.Accept();
                    //Debug.Log("Accept");
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    // получаем сообщение
                    do
                    {
                        //Debug.Log("do получение сообщения");
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
                //connectionIaAlive = false;
                Debug.LogError("Соединение сброшено " + ex.Message);
                Deinit();
                //Console.WriteLine("[ERROR ListenMessage] " + ex.Message);
            }
            return "";
        }

        static Message ConvertJsonToMes(string clientMessage)
        {
            try
            {
                Message mes = JsonConvert.DeserializeObject<Message>(clientMessage);
                return mes;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            return new Message() { Text = "" };
        }


        static void SendMessageToClient(string answer)
        {
            byte[] data = Encoding.UTF8.GetBytes(answer);

            if (handler == null)
            {

                Debug.LogError("handler == null " + " answer = " + answer);
            }

            handler.Send(data); /// происходит ошибка если клиент принудительно разорвал соединение
        }

        static void CloseConnection(Socket socket)
        {
            if (socket == null)
                return;

            try
            {
                if (connectionIaAlive)
                    socket.Shutdown(SocketShutdown.Both);

                socket.Close();
                Debug.Log("соединение закрыто");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR CloseConnection] " + e.Message);
            }
            
        }
    }


    //класс, соответствующий принимаемой от Py клиента информации
    class Message
    {
        [JsonProperty("Text")]
        public string Text { get; set; }

        public Message() { }
    }

    //возможно, на будущее
    class MessageAdvansed
    {

    }

    enum ServerState
    {
        Disable = 0,
        Work = 1,
        Wait = 2,
    }
}
