using System;
using System.Windows.Forms;

namespace VoiceAssistant
{
    public partial class Form1 : Form
    {
        public static event Action<float> OnConfidenceChanged;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Debug.form1 = this;
            LoadListenManager();
        }

        #region Click

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            Debug.ClearLog();
        }

        private void RecogniseButton_Click(object sender, EventArgs e)
        {
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
