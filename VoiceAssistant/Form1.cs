using System;
using System.Windows.Forms;
using VoiceAssistant.Handles;
using VoiceAssistant.Server;

namespace VoiceAssistant
{
    public partial class Form1 : Form
    {
        public event Action<float> OnConfidenceChanged;
        public static event Action onExit;

        bool alive = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void Init()
        {
            Debug.form1 = this;
            PressKeyObserver.Start();
            RecognitionServer.Init();
        }

        private void Deinit()
        {
            alive = false;
            PressKeyObserver.Stop();
            lm?.Stop();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Deinit();
            onExit?.Invoke();
        }


        #region Click

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            Debug.ClearLog();
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            LoadListenManager();
        }


        #endregion Click

        ListenManager lm;

        void LoadListenManager()
        {
            if (lm != null)
            {
                lm.Stop();
                lm.Restart();
            }
            else
            {
                ListenBuilder builder = new ListenBuilder();
                builder.Build();
                lm = new ListenManager(builder);
                lm.Start();
            }
        }



        #region ForDebugLog


        public void WriteMassage(string text)
        {
            if (alive)
            {
                Action writeLog = () => MessageForm.Text += "\n" + text + "\n";
                Invoke(writeLog);
                panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;
            }
        }

        public void ClearLog()
        {
            if (alive)
            {
                Action clearLogDelegate = new Action(() => MessageForm.Text = "Log: ");
                Invoke(clearLogDelegate);
            }
        }


        #endregion ForDebugLog




        private void ConfidenceBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {


                if (int.TryParse(ConfidenceBox.Text, out int result) && result >= 0 && result <= 100)
                {
                    OnConfidenceChanged?.Invoke(result / 100f);
                    ConfidenceBox.Text = "";
                    label1.Focus();
                    Debug.Log("требуемая точность изменена на " + result / 100f);
                }
                else
                {
                    Debug.Log("введены некорректное значение");
                }
            }
        }
    }
}
