using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PianoSenseDesktop
{
    public partial class ResultForm : Form
    {
        public ResultForm(List<DetectedNote> detectedNotes)
        {
            InitializeComponent();
            DisplayResults(detectedNotes);
        }

        private void DisplayResults(List<DetectedNote> detectedNotes)
        {
            foreach (var note in detectedNotes)
            {
                string correctness = note.IsCorrect ? "Doğru" : "Yanlış";
                listBox1.Items.Add($"Zaman: {note.TimeInSeconds:F2}s - Nota: {note.NoteName} - {correctness}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 anasayfa = new Form1();
            anasayfa.Show();
            this.Hide();
        }
    }
}

