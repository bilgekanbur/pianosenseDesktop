using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace PianoSenseDesktop
{
    public partial class Form1 : Form
    {
        private WaveInEvent waveIn;
        private WaveFileWriter waveFile;
        private bool isRecording = false;
        private PianoNoteDetector noteDetector;
        private List<DetectedNote> detectedNotes; // Tespit edilen notaların listesi

        public Form1()
        {
            InitializeComponent();
            noteDetector = new PianoNoteDetector(); // PianoNoteDetector sınıfını başlatıyoruz
            detectedNotes = new List<DetectedNote>(); // Tespit edilen notaları saklamak için liste
        }

        private void starting_record_button_Click(object sender, EventArgs e)
        {
            if (!isRecording)
            {
                StartRecording();
                MessageBox.Show("Kayıt başladı");
            }
        }

        private void stopping_record_button_Click(object sender, EventArgs e)
        {
            if (isRecording)
            {
                StopRecording();
                MessageBox.Show("Kayıt durdu");
                ShowResults(); // Kayıt durduğunda sonuçları göster
            }
            isRecording = !isRecording;
        }

        private void StartRecording()
        {
            var waveFormat = new WaveFormat(44100, 1); // Mono, 44100 Hz
            waveIn = new WaveInEvent
            {
                WaveFormat = waveFormat
            };
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;

            string songName = textBox1.Text.Replace(" ", "_");
            string selectedDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            string tryingNumber = textBox2.Text;
            string filename = Path.Combine(Environment.CurrentDirectory, $"{songName}_{selectedDate}_{tryingNumber}.wav");
            waveFile = new WaveFileWriter(filename, waveIn.WaveFormat);

            waveIn.StartRecording();
            isRecording = true;
        }

        private void StopRecording()
        {
            waveIn.StopRecording();
        }
        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            waveIn.Dispose();
            waveFile.Dispose();
            isRecording = false;
        }
        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            // Ses verilerini analiz etmek için kullanacağız
            double[] sampleBuffer = ConvertBytesToDoubles(e.Buffer);

        }

        private double[] ConvertBytesToDoubles(byte[] byteBuffer)
        {
            int bytesPerSample = 2;
            double[] samples = new double[byteBuffer.Length / bytesPerSample];

            for (int i = 0; i < samples.Length; i++)
            {
                short sample = BitConverter.ToInt16(byteBuffer, i * bytesPerSample);
                samples[i] = sample / 32768.0; // Normalize
            }

            return samples;
        }

        

        private void ShowResults()
        {
            ResultForm resultForm = new ResultForm(detectedNotes);
            resultForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResultForm sonuc= new ResultForm(detectedNotes);
            sonuc.Show();
            this.Hide();
        }
    }
}
