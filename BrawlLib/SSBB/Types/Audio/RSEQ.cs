using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQHeader
    {
        public const uint Tag = 0x51455352;

        public NW4RCommonHeader _header;

        public bint _dataOffset;
        public bint _dataLength;
        public bint _lablOffset;
        public bint _lablLength;

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }

        public RSEQ_DATAHeader* Data { get { return (RSEQ_DATAHeader*)(Address + _dataOffset); } }
        public LABLHeader* Labl { get { return (LABLHeader*)(Address + _lablOffset); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct RSEQ_DATAHeader
    {
        public const uint Tag = 0x41544144;

        public uint _tag;
        public bint _size;
        public bint _baseOffset;

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }

        public VoidPtr MMLCommands { get { return (Address + _baseOffset); } }
    }

    public class MMLSong
    {
        public Dictionary<int, MMLCommand[]> Tracks;

        public MMLSong()
        {
            Tracks = new Dictionary<int, MMLCommand[]>();
        }

        public int CalculateLength()
        {
            int max = 0;

            foreach (var track in Tracks.Values)
            {
                int len = 0;
                foreach (var cmd in track)
                {
                    len += cmd.GetLength().GetValueOrDefault(0);
                }
                max = Math.Max(len, max);
            }

            return max;
        }
    }

    public class MMLCommand
    {
        public Mml _cmd { get; set; }
        public uint? _value1 { get; set; }
        public uint? _value2 { get; set; }
        public uint? _value3 { get; set; }
        public uint? _value4 { get; set; }
        public uint? _value5 { get; set; }
        public int _offset { get; set; }

        public MMLCommand() { }
        public MMLCommand(Mml cmd)
        {
            _cmd = cmd;
        }
        public MMLCommand(Mml cmd, uint value1)
        {
            _cmd = cmd;
            _value1 = value1;
        }
        public MMLCommand(Mml cmd, uint value1, uint value2)
        {
            _cmd = cmd;
            _value1 = value1;
            _value2 = value2;
        }
        public MMLCommand(Mml cmd, uint value1, uint value2, uint value3)
        {
            _cmd = cmd;
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
        }
        public MMLCommand(Mml cmd, uint value1, uint value2, uint value3, uint value4)
        {
            _cmd = cmd;
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
        }
        public MMLCommand(Mml cmd, uint value1, uint value2, uint value3, uint value4, uint value5)
        {
            _cmd = cmd;
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
            _value4 = value4;
            _value5 = value5;
        }

        public override string ToString()
        {
            return $"{_cmd}@{_offset}: 0x{_value1:x} 0x{_value2:x} 0x{_value3:x} 0x{_value4:x} 0x{_value5:x}";
        }

        public bool IsSigned()
        {
            return false;
        }

        public int? GetLength()
        {
            switch (_cmd)
            {
                case Mml.MML_WAIT:
                    return (int)_value1.Value;
                case Mml.NOTE_ON:
                    return (int)_value3.Value;
                default:
                    return null;
            }
        }
    }

    public unsafe class MMLParser
    {
        private static uint ReadVarLen(ref byte* addr)
        {
            uint value = 0;

            while (true)
            {
                uint b = (*addr++);
                value = (value << 7) | (b & 127);
                if ((b & 0x80) == 0)
                {
                    break;
                }
            }

            return value;
        }

        private static uint ReadUnsigned16(ref byte* addr)
        {
            uint value = (uint)(*addr++) << 8;
            value |= (uint)(*addr++);
            return value;
        }

        private static uint ReadUnsigned24(ref byte* addr)
        {
            uint value = (uint)(*addr++) << 16;
            value |= (uint)(*addr++) << 8;
            value |= (uint)(*addr++);
            return value;
        }

        private static string PrintBytes(byte[] byteArray)
        {
            var sb = new StringBuilder("{ ");
            for (var i = 0; i < byteArray.Length; i++)
            {
                var b = byteArray[i];
                sb.Append($"0x{b:x2}");
                if (i < byteArray.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");
            return sb.ToString();
        }

        public static MMLSong Parse(VoidPtr address)
        {
            MMLSong song = new MMLSong();

            byte* addr = (byte*)address;
            int offset = 0;

            Dictionary<int, int> knownTracks = new Dictionary<int, int>
            {
                { 0, 0 }
            };

            int track;
            while (knownTracks.TryGetValue(offset, out track))
            {
                List<MMLCommand> commands = new List<MMLCommand>();
                Mml cmd = 0x00;
                do
                {
                    uint value = 0;
                    uint value2 = 0;
                    uint value3 = 0;
                    uint value4 = 0;
                    uint subCmd = 0;
                    MMLCommand mmlCmd = null;

                    byte b = (*addr++);
                    if (b < 0x80)
                    {
                        value = ReadVarLen(ref addr);
                        uint key = b;
                        uint velocity = (*addr++);
                        uint length = ReadVarLen(ref addr);
                        mmlCmd = new MMLCommand(Mml.NOTE_ON, key, velocity, length);
                    }
                    else
                    {
                        cmd = (Mml)b;
                        switch (cmd)
                        {
                            // 0-arg commands
                            case Mml.MML_RET:
                            case Mml.MML_LOOP_END:
                            case Mml.MML_FIN:
                                mmlCmd = new MMLCommand(cmd);
                                break;

                            // varlen commands
                            case Mml.MML_WAIT:
                            case Mml.MML_PRG:
                                value = ReadVarLen(ref addr);
                                mmlCmd = new MMLCommand(cmd, value);
                                break;

                            case Mml.MML_OPEN_TRACK:
                                value = (*addr++);
                                value2 = ReadUnsigned24(ref addr);
                                mmlCmd = new MMLCommand(cmd, value, value2);
                                if (!knownTracks.ContainsKey((int)value2))
                                {
                                    knownTracks.Add((int)value2, (int)value);
                                }
                                break;
                            case Mml.MML_JUMP:
                            case Mml.MML_CALL:
                                value = ReadUnsigned24(ref addr);
                                mmlCmd = new MMLCommand(cmd, value);
                                break;

                            case Mml.MML_VARIABLE:
                                subCmd = (*addr++);
                                if (subCmd >= 0xb0 && subCmd <= 0xbd)
                                {
                                    value = (*addr++);
                                }
                                value2 = (*addr++);
                                mmlCmd = new MMLCommand(cmd, subCmd, value, value2);
                                break;
                            case Mml.MML_IF:
                                subCmd = (*addr++);
                                if (subCmd >= 0xb0 && subCmd <= 0xbd)
                                {
                                    value = (*addr++);
                                }
                                value2 = (*addr++);
                                value3 = (*addr++);
                                mmlCmd = new MMLCommand(cmd, subCmd, value, value2, value3);
                                break;
                            //case Mml.MML_TIME:
                            //case Mml.MML_TIME_RANDOM:
                            //case Mml.MML_TIME_VARIABLE:
                            //    break;

                            // u8 argument commands
                            case Mml.MML_TIMEBASE:
                            case Mml.MML_ENV_HOLD:
                            case Mml.MML_MONOPHONIC:
                            case Mml.MML_VELOCITY_RANGE:
                            case Mml.MML_BIQUAD_TYPE:
                            case Mml.MML_BIQUAD_VALUE:
                            case Mml.MML_PAN:
                            case Mml.MML_VOLUME:
                            case Mml.MML_MAIN_VOLUME:
                            case Mml.MML_TRANSPOSE:
                            case Mml.MML_PITCH_BEND:
                            case Mml.MML_BEND_RANGE:
                            case Mml.MML_PRIO:
                            case Mml.MML_NOTE_WAIT:
                            case Mml.MML_TIE:
                            case Mml.MML_PORTA:
                            case Mml.MML_MOD_DEPTH:
                            case Mml.MML_MOD_SPEED:
                            case Mml.MML_MOD_TYPE:
                            case Mml.MML_MOD_RANGE:
                            case Mml.MML_PORTA_SW:
                            case Mml.MML_PORTA_TIME:
                            case Mml.MML_ATTACK:
                            case Mml.MML_DECAY:
                            case Mml.MML_SUSTAIN:
                            case Mml.MML_RELEASE:
                            case Mml.MML_LOOP_START:
                            case Mml.MML_EXPRESSION:
                            case Mml.MML_PRINTVAR:
                            case Mml.MML_SURROUND_PAN:
                            case Mml.MML_LPF_CUTOFF:
                            case Mml.MML_FXSEND_A:
                            case Mml.MML_FXSEND_B:
                            case Mml.MML_MAINSEND:
                            case Mml.MML_INIT_PAN:
                            case Mml.MML_MUTE:
                            case Mml.MML_FXSEND_C:
                            case Mml.MML_DAMPER:
                                value = (*addr++);
                                mmlCmd = new MMLCommand(cmd, value);
                                break;

                            // extended commands
                            case Mml.MML_EX_COMMAND:
                                MmlEx cmdEx = (MmlEx)(*addr++);
                                switch (cmdEx)
                                {
                                    case MmlEx.MML_SETVAR: break;
                                    case MmlEx.MML_ADDVAR: break;
                                    case MmlEx.MML_SUBVAR: break;
                                    case MmlEx.MML_MULVAR: break;
                                    case MmlEx.MML_DIVVAR: break;
                                    case MmlEx.MML_SHIFTVAR: break;
                                    case MmlEx.MML_RANDVAR: break;
                                    case MmlEx.MML_ANDVAR: break;
                                    case MmlEx.MML_ORVAR: break;
                                    case MmlEx.MML_XORVAR: break;
                                    case MmlEx.MML_NOTVAR: break;
                                    case MmlEx.MML_MODVAR: break;
                                    case MmlEx.MML_CMP_EQ: break;
                                    case MmlEx.MML_CMP_GE: break;
                                    case MmlEx.MML_CMP_GT: break;
                                    case MmlEx.MML_CMP_LE: break;
                                    case MmlEx.MML_CMP_LT: break;
                                    case MmlEx.MML_CMP_NE: break;
                                    case MmlEx.MML_USERPROC: break;
                                }
                                value = (*addr++);
                                value2 = (*addr++);
                                value3 = (*addr++);
                                value4 = (*addr++);
                                mmlCmd = new MMLCommand(cmd, (uint)cmdEx, value, value2, value3, value4);
                                break;

                            case Mml.MML_ALLOC_TRACK:
                                value = ReadUnsigned16(ref addr);
                                mmlCmd = new MMLCommand(cmd, value);
                                break;

                            default:
                                byte[] arr = new byte[0x10];
                                Marshal.Copy((IntPtr)addr, arr, 0, 0x10);
                                var str = PrintBytes(arr);
                                Marshal.Copy((IntPtr)addr - 0x10, arr, 0, 0x10);
                                var str2 = PrintBytes(arr);
                                //throw new Exception($"Unknown MML command: 0x{b:x2}\n{str2}\n{str}");
                                break;
                        }
                    }

                    if (mmlCmd != null)
                    {
                        mmlCmd._offset = offset;
                        commands.Add(mmlCmd);
                    }
                    offset = (addr - address);
                } while (cmd != Mml.MML_FIN && !knownTracks.ContainsKey(offset));

                // TODO
                if (!song.Tracks.ContainsKey(track))
                {
                    song.Tracks.Add(track, commands.ToArray());
                }
            }
            return song;
        }
    }

    public enum Mml
    {
        // not part of spec

        [Description("Note On")]
        NOTE_ON = 0x00,

        // Variable length parameter commands.

        /// <summary>
        /// Args:
        ///   A: varlen
        /// Description:
        ///   Rests for A ticks.
        /// Forceable:
        ///   Yes
        /// </summary>
        [Description("Wait")]
        MML_WAIT = 0x80,

        /// <summary>
        /// Args:
        ///   A: varlen
        /// Description:
        ///   Sets program, and optionally sets bank.
        /// Forceable:
        ///   Yes
        /// </summary>
        [Description("Program")]
        MML_PRG = 0x81,

        // Control commands.

        /// <summary>
        /// Args:
        ///   A: u8
        ///   B: u24
        /// Description:
        ///   Creates track A, with instructions starting at offset B.
        /// </summary>
        [Description("Open Track")]
        MML_OPEN_TRACK = 0x88,

        /// <summary>
        /// Args:
        ///   A: u24
        /// Description:
        ///   Jumps to offset A.
        /// </summary>
        [Description("Jump")]
        MML_JUMP = 0x89,

        /// <summary>
        /// Args:
        ///   A: u24
        /// Description:
        ///   Pushes a return address and jumps to offset A.
        /// </summary>
        [Description("Call")]
        MML_CALL = 0x8a,

        // prefix commands

        /// <summary>
        /// Args:
        ///   A: command
        ///   B: s16
        ///   C: s16
        /// Description:
        ///   Runs command A with a random number between B and C.
        /// </summary>
        [Description("Random")]
        MML_RANDOM = 0xa0,

        /// <summary>
        /// Args:
        ///   A: command
        ///   B: u8
        /// Description:
        ///   Runs command A with the value of variable B.
        /// </summary>
        [Description("Variable")]
        MML_VARIABLE = 0xa1,

        [Description("If")]
        MML_IF = 0xa2,

        [Description("Time")]
        MML_TIME = 0xa3,

        [Description("Time Random")]
        MML_TIME_RANDOM = 0xa4,

        [Description("Time Variable")]
        MML_TIME_VARIABLE = 0xa5,

        // u8 parameter commands.
        [Description("Time Base")]
        MML_TIMEBASE = 0xb0,

        [Description("Env Hold")]
        MML_ENV_HOLD = 0xb1,

        [Description("Monophonic")]
        MML_MONOPHONIC = 0xb2,

        [Description("Velocity Range")]
        MML_VELOCITY_RANGE = 0xb3,

        [Description("Biquad Type")]
        MML_BIQUAD_TYPE = 0xb4,

        [Description("Biquad Value")]
        MML_BIQUAD_VALUE = 0xb5,

        [Description("Pan")]
        MML_PAN = 0xc0,

        [Description("Volume")]
        MML_VOLUME = 0xc1,

        [Description("Main Volume")]
        MML_MAIN_VOLUME = 0xc2,
        [Description("Transpose")]
        MML_TRANSPOSE = 0xc3,

        [Description("Pitch Bend")]
        MML_PITCH_BEND = 0xc4,

        [Description("Bend Range")]
        MML_BEND_RANGE = 0xc5,

        [Description("Priority")]
        MML_PRIO = 0xc6,

        [Description("Note Wait")]
        MML_NOTE_WAIT = 0xc7,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets tie.
        [Description("Tie")]
        MML_TIE = 0xc8,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets portamento.
        [Description("Portamento")]
        MML_PORTA = 0xc9,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets mod depth.
        [Description("Mod Depth")]
        MML_MOD_DEPTH = 0xca,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets mod speed.
        [Description("Mod Speed")]
        MML_MOD_SPEED = 0xcb,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets mod type.
        [Description("Mod Type")]
        MML_MOD_TYPE = 0xcc,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets mod range.
        [Description("Mod Range")]
        MML_MOD_RANGE = 0xcd,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets portamento.
        [Description("Portamento Sw")]
        MML_PORTA_SW = 0xce,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets portamento time.
        [Description("Portamento Time")]
        MML_PORTA_TIME = 0xcf,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets attack.
        [Description("Attack")]
        MML_ATTACK = 0xd0,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets decay.
        [Description("Decay")]
        MML_DECAY = 0xd1,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets sustain.
        [Description("Sustain")]
        MML_SUSTAIN = 0xd2,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets release.
        [Description("Release")]
        MML_RELEASE = 0xd3,

        /// <summary>
        /// Args:
        ///   (none)
        /// Description:
        ///   Marks start of loop. No effect?
        [Description("Loop Start")]
        MML_LOOP_START = 0xd4,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Sets expression of track A.
        [Description("Expression")]
        MML_EXPRESSION = 0xd5,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Print variable A?
        [Description("Print Variable")]
        MML_PRINTVAR = 0xd6,

        [Description("Surround Pan")]
        MML_SURROUND_PAN = 0xd7,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("LPF Cutoff")]
        MML_LPF_CUTOFF = 0xd8,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("FX Send A")]
        MML_FXSEND_A = 0xd9,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("FX Send B")]
        MML_FXSEND_B = 0xda,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("Main Send")]
        MML_MAINSEND = 0xdb,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("Init Pan")]
        MML_INIT_PAN = 0xdc,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   Mutes track?
        /// </summary>
        [Description("Mute")]
        MML_MUTE = 0xdd,

        /// <summary>
        /// Args:
        ///   A: u8
        /// Description:
        ///   ???
        [Description("FX Send C")]
        MML_FXSEND_C = 0xde,

        [Description("Damper")]
        MML_DAMPER = 0xdf,

        // s16 parameter commands.

        /// <summary>
        /// Args:
        ///   A: s16
        /// Description:
        ///   Sets mod delay.
        /// </summary>
        [Description("Mod Delay")]
        MML_MOD_DELAY = 0xe0,

        /// <summary>
        /// Args:
        ///   A: s16
        /// Description:
        ///   Sets tempo.
        /// </summary>
        [Description("Tempo")]
        MML_TEMPO = 0xe1,

        /// <summary>
        /// Args:
        ///   A: s16
        /// Description:
        ///   Sets sweep pitch.
        /// </summary>
        [Description("Sweep Pitch")]
        MML_SWEEP_PITCH = 0xe3,

        // Extended commands.

        /// <summary>
        /// Args:
        ///   A: u8
        ///   B: u16
        ///   C: u16
        ///   D: u16
        ///   E: u16
        /// Description:
        ///   Runs an extended command with opcode A.
        /// Forcable:
        ///   Yes.
        /// </summary>
        [Description("EX Command")]
        MML_EX_COMMAND = 0xf0,

        // Other
        [Description("Env Reset")]
        MML_ENV_RESET = 0xfb,

        /// <summary>
        /// Args:
        ///   (none)
        /// Description:
        ///   Ends a loop.
        /// </summary>
        [Description("Loop End")]
        MML_LOOP_END = 0xfc,

        /// <summary>
        /// Args:
        ///   (none)
        /// Description:
        ///   Returns to the address pushed by a MML_CALL instruction.
        /// </summary>
        [Description("Return")]
        MML_RET = 0xfd,

        /// <summary>
        /// Args:
        ///   A: u16
        /// Description:
        ///   Indicates track usage: For each bit in A, the track at that index is used.
        /// </summary>
        [Description("Alloc Track")]
        MML_ALLOC_TRACK = 0xfe,

        /// <summary>
        /// Args:
        ///   (none)
        /// Description:
        ///   Indicates end of track. Also ends loop.
        /// </summary>
        [Description("Finish")]
        MML_FIN = 0xff
    }

    public enum MmlEx
    {
        [Description("Set Var")]
        MML_SETVAR = 0x80,

        [Description("Add Var")]
        MML_ADDVAR = 0x81,

        [Description("Sub Var")]
        MML_SUBVAR = 0x82,

        [Description("Mul Var")]
        MML_MULVAR = 0x83,

        [Description("Div Var")]
        MML_DIVVAR = 0x84,

        [Description("Shift Var")]
        MML_SHIFTVAR = 0x85,

        [Description("Rand Var")]
        MML_RANDVAR = 0x86,

        [Description("And Var")]
        MML_ANDVAR = 0x87,

        [Description("Or Var")]
        MML_ORVAR = 0x88,

        [Description("Xor Var")]
        MML_XORVAR = 0x89,

        [Description("Not Var")]
        MML_NOTVAR = 0x8a,

        [Description("Mod Var")]
        MML_MODVAR = 0x8b,

        [Description("Comp ==")]
        MML_CMP_EQ = 0x90,

        [Description("Comp >=")]
        MML_CMP_GE = 0x91,

        [Description("Comp >")]
        MML_CMP_GT = 0x92,

        [Description("Comp <")]
        MML_CMP_LE = 0x93,

        [Description("Comp <=")]
        MML_CMP_LT = 0x94,

        [Description("Comp !=")]
        MML_CMP_NE = 0x95,

        [Description("User Proc")]
        MML_USERPROC = 0xe0
    }

    public enum SeqArgType
    {
        SEQ_ARG_NONE,
        SEQ_ARG_U8,
        SEQ_ARG_S16,
        SEQ_ARG_VMIDI,
        SEQ_ARG_RANDOM,
        SEQ_ARG_VARIABLE
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct LABLHeader
    {
        public const uint Tag = 0x4C42414C;

        public uint _tag;
        public bint _size;
        public bint _numEntries;

        public void Set(int size, int count)
        {
            _tag = Tag;
            _size = size;
            _numEntries = count;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }

        public bint* EntryOffset { get { return (bint*)(Address + 12); } }

        public LABLEntry* Get(int index)
        {
            bint* offset = (bint*)(Address + 8);
            return (LABLEntry*)((int)offset + offset[index + 1]);
        }

        public string GetString(int index)
        {
            bint* offset = (bint*)(Address + 8);
            return ((LABLEntry*)((int)offset + offset[index + 1]))->Name;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    unsafe struct LABLEntry
    {
        public buint _id;
        public buint _stringLength;

        public void Set(uint id, string str)
        {
            uint len = (uint)str.Length;
            int i = 0;
            sbyte* dPtr = (sbyte*)(Address + 8);
            char* sPtr;

            _id = id;
            _stringLength = len;

            fixed (char* s = str)
            {
                sPtr = s;
                while (i++ < len)
                    *dPtr++ = (sbyte)*sPtr++;
            }

            //Trailing zero
            *dPtr++ = 0;

            //Padding
            while ((i++ & 3) != 0)
                *dPtr++ = 0;
        }

        private VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }

        public string Name
        {
            get { return new string((sbyte*)Address + 8); }
        }
    }
}
