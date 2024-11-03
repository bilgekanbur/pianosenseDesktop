using System;
using System.Collections.Generic;
using System.Numerics; // System.Numerics.Complex kullanmak için
using NAudio.Wave;
using MathNet.Numerics.IntegralTransforms;

namespace PianoSenseDesktop
{
    public class DetectedNote
    {
        public string NoteName { get; set; }
        public double TimeInSeconds { get; set; }
        public bool IsCorrect { get; set; }

        public DetectedNote(string noteName, double timeInSeconds, bool isCorrect)
        {
            NoteName = noteName;
            TimeInSeconds = timeInSeconds;
            IsCorrect = isCorrect;
        }
    }

    public class AudioAnalyzer
    {
        private static double threshold = 0.1; // Örnek bir eşik değeri

        public static List<DetectedNote> AnalyzeAudio(string filePath)
        {
            var detectedNotes = new List<DetectedNote>();

            using (var reader = new AudioFileReader(filePath))
            {
                int sampleRate = reader.WaveFormat.SampleRate;
                int bufferSize = 4096; // FFT işlemi için örnek büyüklüğü
                var buffer = new float[bufferSize];
                int bytesRead;

                while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // FFT Analizi için veri hazırlama
                    Complex[] complexBuffer = new Complex[bufferSize];
                    for (int i = 0; i < bufferSize; i++)
                    {
                        complexBuffer[i] = new Complex(buffer[i], 0); // Reel kısmı dolduruyoruz
                    }

                    // MathNet FFT Uygulama
                    Fourier.Forward(complexBuffer, FourierOptions.Matlab);

                    // Frekansları analiz etme
                    for (int i = 0; i < complexBuffer.Length / 2; i++)
                    {
                        double frequency = i * sampleRate / bufferSize;

                        // Baskın frekansı ve güç seviyesini bulma
                        double magnitude = complexBuffer[i].Magnitude; // Magnitude özelliğini kullanıyoruz

                        if (magnitude > threshold) // Belirli bir eşik üzerinde olan frekansları alıyoruz
                        {
                            string noteName = FrequencyToNoteName(frequency);
                            double timeInSeconds = reader.CurrentTime.TotalSeconds;

                            // Çalınan notayı listeye ekleyelim
                            detectedNotes.Add(new DetectedNote(noteName, timeInSeconds, true));
                        }
                    }
                }
            }

            return detectedNotes;
        }

        // Frekansı nota adına dönüştüren metod
        private static string FrequencyToNoteName(double frequency)
        {
            // Basit bir frekans eşleştirme algoritması
            if (frequency >= 261.63 && frequency < 277.18)
                return "C4";
            else if (frequency >= 277.18 && frequency < 293.66)
                return "C#4";
            else if (frequency >= 293.66 && frequency < 311.13)
                return "D4";
            else if (frequency >= 311.13 && frequency < 329.63)
                return "D#4";
            else if (frequency >= 329.63 && frequency < 349.23)
                return "E4";
            else if (frequency >= 349.23 && frequency < 369.99)
                return "F4";
            else if (frequency >= 369.99 && frequency < 392.00)
                return "F#4";
            else if (frequency >= 392.00 && frequency < 415.30)
                return "G4";
            else if (frequency >= 415.30 && frequency < 440.00)
                return "G#4";
            else if (frequency >= 440.00 && frequency < 466.16)
                return "A4";
            else if (frequency >= 466.16 && frequency < 493.88)
                return "A#4";
            else if (frequency >= 493.88 && frequency < 523.25)
                return "B4";
            else
                return "Unknown";
        }
    }
}
