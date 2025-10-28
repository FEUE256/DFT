using System;
using System.Drawing;
using System.Windows.Forms;

namespace Daily_Focus_Tracker
{
    public class MainForm : Form
    {
        private System.Windows.Forms.Timer focusTimer;
        private int remainingSeconds;
        private const int FocusDuration = 25 * 60;
        private const int BreakDuration = 5 * 60;
        private bool onFocus = true;

        private Label timerLabel;
        private ListBox taskList;
        private TextBox taskInput;
        private ProgressBar progressBar;
        private Button startBtn;
        private Button stopBtn;

        public MainForm()
        {
            InitializeUI();
            InitializeTimer();
        }

        private void InitializeUI()
        {
            this.Text = "Daily Focus Tracker";
            this.Width = 600;
            this.Height = 450; // lite högre för layouten
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Uppgiftslista
            taskList = new ListBox
            {
                Top = 10,
                Left = 10,
                Width = 250,
                Height = 250,
                Font = new Font("Segoe UI", 10),
                BackColor = Color.White
            };
            this.Controls.Add(taskList);

            // Input för ny uppgift
            taskInput = new TextBox
            {
                Top = 270,
                Left = 10,
                Width = 170,
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(taskInput);

            Button addTaskBtn = new Button
            {
                Text = "Add Task",
                Top = 270,
                Left = 190,
                Width = 70,
                BackColor = Color.LightGreen
            };
            addTaskBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(taskInput.Text))
                {
                    taskList.Items.Add(taskInput.Text);
                    taskInput.Clear();
                }
            };
            this.Controls.Add(addTaskBtn);

            Button removeTaskBtn = new Button
            {
                Text = "Remove Task",
                Top = 310,
                Left = 10,
                Width = 250,
                BackColor = Color.LightCoral
            };
            removeTaskBtn.Click += (s, e) =>
            {
                if (taskList.SelectedItem != null)
                    taskList.Items.Remove(taskList.SelectedItem);
            };
            this.Controls.Add(removeTaskBtn);

            // Timer label
            timerLabel = new Label
            {
                Text = "25:00",
                Top = 20,               // flyttad lite ner
                Left = 300,
                Width = 250,
                Height = 80,            // högre så hela texten syns
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(timerLabel);

            // Progress bar
            progressBar = new ProgressBar
            {
                Top = 120,              // längre ner
                Left = 300,
                Width = 250,
                Height = 25,
                Minimum = 0,
                Maximum = FocusDuration
            };
            this.Controls.Add(progressBar);

            // Start-knapp
            startBtn = new Button
            {
                Text = "Start Focus",
                Top = 160,
                Left = 300,
                Width = 100,
                BackColor = Color.LightSkyBlue
            };
            startBtn.Click += (s, e) =>
            {
                remainingSeconds = FocusDuration;
                onFocus = true;
                progressBar.Maximum = FocusDuration;
                progressBar.Value = 0;
                focusTimer.Start();
            };
            this.Controls.Add(startBtn);

            // Stop-knapp
            stopBtn = new Button
            {
                Text = "Stop",
                Top = 200,
                Left = 300,
                Width = 100,
                BackColor = Color.LightGray
            };
            stopBtn.Click += (s, e) => focusTimer.Stop();
            this.Controls.Add(stopBtn);
        }

        private void InitializeTimer()
        {
            focusTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            focusTimer.Tick += (s, e) =>
            {
                remainingSeconds--;
                int minutes = remainingSeconds / 60;
                int seconds = remainingSeconds % 60;
                timerLabel.Text = $"{minutes:D2}:{seconds:D2}";

                progressBar.Value = Math.Max(0, progressBar.Maximum - remainingSeconds);

                if (remainingSeconds <= 0)
                {
                    focusTimer.Stop();
                    string message = onFocus ? "Focus session finished! Take a break." : "Break finished! Back to work.";
                    MessageBox.Show(message, "Daily Focus Tracker");

                    remainingSeconds = onFocus ? BreakDuration : FocusDuration;
                    onFocus = !onFocus;
                    progressBar.Maximum = remainingSeconds;
                    progressBar.Value = 0;
                    focusTimer.Start();
                }
            };
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
