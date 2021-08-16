cd /d "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy"
pyinstaller client.py

echo copy vosk
xcopy /y /o /e "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy\vosk\" "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy\dist\client\vosk\*.*"

echo copy model
xcopy /y /o /e "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy\model\" "D:\indi projects\VoiceAssistentProj\VoiceAssistant main\RecogniseClientPy\dist\client\model\*.*"

PAUSE

