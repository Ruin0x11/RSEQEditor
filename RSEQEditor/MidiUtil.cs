using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSEQEditor
{
    public static class MidiUtil
    {
        private static readonly bool[] _KEY_TYPE = new bool[] {
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
            false,
            true,
            true,
            false,
            true,
        };

        public static bool IsNoteWhiteKey(int note)
        {
            if (0 <= note && note <= 127)
            {
                return _KEY_TYPE[note];
            }
            else
            {
                int odd = note % 12;
                switch (odd)
                {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 10:
                        return false;
                    default:
                        return true;
                }
            }
        }

        public static int GetNoteOctave(int note)
        {
            int odd = note % 12;
            return (note - odd) / 12 - 2;
        }

        public static string GetNoteStringBase(int note)
        {
            int odd = note % 12;
            switch (odd)
            {
                case 0:
                case 1:
                    return "C";
                case 2:
                    return "D";
                case 3:
                case 4:
                    return "E";
                case 5:
                case 6:
                    return "F";
                case 7:
                case 8:
                    return "G";
                case 9:
                    return "A";
                case 10:
                case 11:
                    return "B";
                default:
                    return "";
            }
        }

        public static string GetNoteString(int note)
        {
            int odd = note % 12;
            int order = (note - odd) / 12 - 2;
            switch (odd)
            {
                case 0:
                    return "C" + order;
                case 1:
                    return "C#" + order;
                case 2:
                    return "D" + order;
                case 3:
                    return "Eb" + order;
                case 4:
                    return "E" + order;
                case 5:
                    return "F" + order;
                case 6:
                    return "F#" + order;
                case 7:
                    return "G" + order;
                case 8:
                    return "G#" + order;
                case 9:
                    return "A" + order;
                case 10:
                    return "Bb" + order;
                case 11:
                    return "B" + order;
                default:
                    return "";
            }
        }
    }
}
