import socket
from vosk import Model, KaldiRecognizer, SetLogLevel
import os, json
from datetime import datetime
import time
import pyaudio


port = 8005

def WaitServer(tick):
    while True:
        try:
            time.sleep(tick) 
            sock = socket.socket()
            sock.connect(('localhost', port))
            sock.send("client started".encode())
            sock.close()
            print("server was found")
            break
        except Exception as ex:
            print("server not found", ex)
            pass



def SendMessage(mes):
    try:
        sock = socket.socket()
        sock.connect(('localhost', port))
        sock.send(mes.encode())
        answer = sock.recv(1024).decode()
        print(answer)
        sock.close()
    except Exception as ex:
        # print("сервер не найден")
        print("[ERROR]", ex.args)
        exit()

def ListenCommand(rec):
    p = pyaudio.PyAudio()
    stream = p.open(format=pyaudio.paInt16, channels=1, rate=16000, input=True, frames_per_buffer=8000)
    stream.start_stream() 

    while True:
        data = stream.read(4000, exception_on_overflow=False)
        if len(data) == 0:
            break
        if rec.AcceptWaveform(data):
            x=json.loads(rec.Result())
            # print("result", rec.Result())
            # print("[", datetime.now().time(), "] " + "result = " + str(x))
            result = x["text"]
            if len(result) > 0:
                print("[", datetime.now().time(), "] " + "result = " + result)
                SendMessage(str(x))
            
        # else:
        #     x=json.loads(rec.PartialResult())
        #     text = x["partial"]
        #     if len(text) != 0:
        #         print("[", datetime.now().time(), "] " + "partial = " + text)


def LoadVosk():
    if not os.path.exists("model"):
        print ("Please download the model from https://github.com/alphacep/vosk-api/blob/master/doc/models.md and unpack as 'model' in the current folder.")
        exit (1)
    model = Model("model")
    global rec
    wordArray = '["oh one two three four five six seven eight nine zero", "[привет]"]'
    rec = KaldiRecognizer(model, 16000, wordArray)
    # rec.SetMaxAlternatives(2)
    SetLogLevel(-1)




LoadVosk()
WaitServer(1)
print("loaded")
ListenCommand(rec)
print("end")




# cd "D:\indi projects\Socket\ClientPython"

#
# cd "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy"
# pyinstaller test.py
