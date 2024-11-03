using System;
using System.Collections.Generic;
using NAudio.Dsp;

namespace PianoSenseDesktop
{
    public class Note
    {
            public string SolfejNote { get; set; }    // Solfej nota adı, örneğin: "Do", "Re", "Mi"
            public string WesternNote { get; set; }   // Batı nota adı, örneğin: "C", "D", "E"
            public int Octave { get; set; }           // Oktav numarası, örneğin: 1, 2, 3
            public double Frequency { get; set; }     // Frekans değeri

            public Note (string solfejNote, string westernNote, int octave, double frequency)
            {
                SolfejNote = solfejNote;
                WesternNote = westernNote;
                Octave = octave;
                Frequency = frequency;
            }

            public override string ToString()
            {
                return $"{SolfejNote} ({WesternNote}{Octave}) - {Frequency} Hz";
            }
        
    }


    public class PianoNoteDetector
    {
        private List<Note> pianoNotes;
        private double tolerance = 5.0; // Frekans farkı toleransı (Hz)

        public PianoNoteDetector()
        {
            InitializePianoNotes();
        }

        // Piyano notalarını ve frekanslarını tanımlıyoruz
        private void InitializePianoNotes()
        {
            pianoNotes = new List<Note>
            {
                new Note("Do","C", 1, 32.70), new Note("Do#","C#", 1, 34.65), new Note("Re","D", 1, 36.71),
                new Note("Re#", "D#", 1, 38.89), new Note("Mi","E", 1, 41.20), new Note("Fa", "F", 1, 43.65),
                new Note("Fa#","F#", 1, 46.25), new Note("Sol","G", 1, 49.00), new Note("Sol#", "G#", 1, 51.91),
                new Note("La","A", 1, 55.00), new Note("La#","A#", 1, 58.27), new Note("Si","B", 1, 61.74),

                new Note("Do", "C", 2, 65.41), new Note("Do#", "C#", 2, 69.30), new Note("Re", "D", 2, 73.42),
                new Note("Re#","D#", 2, 77.78), new Note("Mi", "E", 2, 82.41), new Note("Fa","F", 2, 87.31),
                new Note("Fa#", "F#", 2,92.50), new Note("Sol","G", 2, 98.00), new Note("Sol#", "G#", 2, 103.83),
                new Note("La","A",2, 110.00), new Note("La#","A#", 2, 116.54), new Note("Si","B", 2, 123.47),

                new Note("Do","C", 3, 130.81), new Note("Do#","C#", 3, 138.59), new Note("Re","D", 3, 146.83),
                new Note("Re#","D#", 3, 155.56), new Note("Mi","E", 3, 164.81), new Note("Fa","F",3 , 174.61),
                new Note("Fa#","F#", 3,185.00), new Note("Sol","G", 3, 196.00), new Note("Sol#","G#", 3, 207.65),
                new Note("La","A",3, 220.00), new Note("La#","A#", 3, 233.08), new Note("Si","B", 3, 246.94),

                new Note("Do","C", 4, 261.63), new Note("Do#","C#", 4, 277.18), new Note("Re","D", 4, 293.66),
                new Note("Re#","D#", 4, 311.13), new Note("Mi","E", 4, 329.63), new Note("Fa","F",4 , 349.23),
                new Note("Fa#","F#", 4,369.99), new Note("Sol","G", 4, 392.00), new Note("Sol#","G#", 4, 415.30),
                new Note("La","A",4, 440.00), new Note("La#","A#", 4, 466.16), new Note("Si","B", 4, 493.88),

                new Note("Do","C", 5, 523.25), new Note("Do#","C#", 5, 554.37), new Note("Re","D", 5, 587.33),
                new Note("Re#","D#", 5, 622.25), new Note("Mi","E", 5, 659.25), new Note("Fa","F",5 , 698.46),
                new Note("Fa#","F#", 5,739.99), new Note("Sol","G", 5, 783.99), new Note("Sol#","G#", 5, 830.61),
                new Note("La","A",5, 880.0), new Note("La#","A#", 5, 932.33), new Note("Si","B", 5, 987.77),

                new Note("Do","C", 6, 1046.50), new Note("Do#","C#", 6, 1108.73), new Note("Re","D", 6, 1174.66),
                new Note("Re#","D#", 6, 1244.51), new Note("Mi","E", 6, 1318.51), new Note("Fa","F",6, 1396.91),
                new Note("Fa#","F#", 6, 1479.98), new Note("Sol","G", 6, 1567.98), new Note("Sol#","G#", 6, 1661.22),
                new Note("La","A",6, 1760.00), new Note("La#","A#", 6, 1864.66), new Note("Si","B", 6, 1975.53),

                new Note("Do","C", 7, 2093.00), new Note("Do#","C#", 7, 2217.46), new Note("Re", "D", 7, 2349.32),
                new Note("Re#","D#", 7, 2489.02), new Note("Mi","E", 7, 2637.02), new Note("Fa","F",7, 2793.83),
                new Note("Fa#","F#", 7, 2959.96), new Note("Sol","G", 7, 3135.96), new Note("Sol", "G#", 7, 3322.44),
                new Note("La","A",7, 3520.00), new Note("La#","A#", 7, 3729.31), new Note("Si","B", 7, 3951.07),


                new Note("Do", "C", 8, 4186.01)
                
            };
        }

        // Belirli bir frekansa en yakın notayı toleransa göre bulur
        public Note FindClosestNote(double frequency)
        {
            Note closestNote = null;
            double smallestDifference = double.MaxValue;

            foreach (var note in pianoNotes)
            {
                double difference = Math.Abs(note.Frequency - frequency);
                if (difference < smallestDifference && difference <= tolerance)
                {
                    smallestDifference = difference;
                    closestNote = note;
                }
            }

            return closestNote ?? new Note("Unknown","Unknown", 0, frequency); // Yakın bir not bulamazsa 'Unknown' döner
        }
    }
}
