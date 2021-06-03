using System;
using System.Windows.Forms;

namespace VoiceAssistant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.form1 = this;
        }

        #region Click

        private void TestButton_Click(object sender, EventArgs e)
        {
            Debug.Log("TestButton_Click");
        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            Debug.ClearLog();
        }

        private void RecogniseButton_Click(object sender, EventArgs e)
        {
            //GrammarTest.Start();
            LoadListenManager();
        }

        #endregion Click

        void LoadListenManager()
        {
            ListenBuilder builder = new ListenBuilder();
            builder.Build();
            ListenManager lm = new ListenManager(builder);
            lm.Start();
        }



        #region ForDebugLog

        delegate void WriteLogDelegate(string text);
        delegate void ClearLogDelegate();
        delegate void ProgressDelegate(int value);       

        public void WriteMassage(string text)
        {
            Action writeLog = () => MessageForm.Text += "\n" + text + "\n";
            Invoke(writeLog);
        }

        public void ClearLog()
        {
            Action clearLogDelegate = new Action(() => MessageForm.Text = "Log: ");
            Invoke(clearLogDelegate);
        }

        public void ProgressMaxValue(int value)
        {
            Action<int> progressDelegate = new Action<int>((_value) => ProgressNeuro.Maximum = _value);
            Invoke(progressDelegate, new object[] { value });
        }

        public void ProgressCurrentValue(int value)
        {
            Action<int> progressDelegate = new Action<int>((_value) => ProgressNeuro.Value = _value);
            Invoke(progressDelegate, new object[] { value });
        }

        public void ProgressAddCurrentValue(int value)
        {
            Action<int> progressDelegate = new Action<int>((_value) => ProgressNeuro.Value += value);
            Invoke(progressDelegate, new object[] { value });
        }

        #endregion ForDebugLog





    }
}
