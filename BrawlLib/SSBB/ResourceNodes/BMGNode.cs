using BrawlLib.SSBB.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class BMGNode : ARCEntryNode
    {
        public override ResourceType ResourceType { get { return ResourceType.BMG; } }
        internal BMG* Header { get { return (BMG*)WorkingUncompressed.Address; } }

        internal BMGEncoding _encoding;
        internal bint _sections;
        internal Dictionary<int, int> _midToChild;

        [Category("BMG")]
        public BMGEncoding Encoding { get { return _encoding; } }
        [Category("BMG")]
        public bint Sections { get { return _sections; } }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            _encoding = Header->_encoding;
            _sections = Header->_sections;
            _midToChild = new Dictionary<int, int>();

            return true;
        }

        public override void OnPopulate()
        {
            VoidPtr p;
            for (int i = 0; i < Header->INFSection->_messageCount; i++)
            {
                if ((p = Header->INFSection->GetMessage(i)) != null)
                {
                    DATEntryNode d = new DATEntryNode(_encoding) { _mid = *Header->MIDSection->GetMessage(i) };
                    d.Initialize(this, p, 0);
                }
            }
            BuildMessageLookup();
        }

        private void BuildMessageLookup()
        {
            _midToChild.Clear();
            foreach (DATEntryNode child in Children)
            {
                if (child.CalculateSize(false) > 2)
                {
                    _midToChild.Add(child.MID, child.Index);
                }
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int datLen = (int)DAT.Size;

            int size = (int)(BMG.Size + Header->INFSection->_size + Header->MIDSection->_size);

            Console.WriteLine($"Len1: {Header->INFSection->_size} {Header->DATSection->_size} {Header->MIDSection->_size} {WorkingUncompressed.Length}");

            foreach (DATEntryNode d in Children)
            {
                datLen += d.CalculateSize(true);
            }

            datLen = datLen.Align(0x10);
            Console.WriteLine($"datLen: {datLen} {size} {size + datLen}");

            return size + datLen;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            BMG* newHeader = (BMG*)address;
            newHeader->_tag = BMG.Tag;
            newHeader->_tag2 = BMG.Tag2;
            newHeader->_size = BMG.Size;
            newHeader->_sections = _sections;
            newHeader->_encoding = _encoding;
            newHeader->_pad1 = 0x00;

            INF* infHeader = newHeader->INFSection;
            infHeader->_tag = INF.Tag;
            infHeader->_messageCount = (bshort)((ushort)Children.Count);
            infHeader->_messageSize = (bshort)INFEntry.Size;
            infHeader->_size = ((int)INF.Size + infHeader->_messageCount * infHeader->_messageSize).Align(0x10);
            infHeader->_pad1 = 0x00;

            DAT* datHeader = newHeader->DATSection;
            datHeader->_tag = DAT.Tag;

            int datLen = 0;
            var offset = 2;
            VoidPtr datMsgs = datHeader->Messages + offset;
            INFEntry* entries = infHeader->Offsets(0);
            INFEntry* prevEntries = Header->INFSection->Offsets(0);
            for (int i = 0; i < Header->INFSection->_messageCount; i++)
            {
                var d = Children[i] as DATEntryNode;
                var size = d.CalculateSize(true);
                d.Rebuild(datMsgs, size, force);
                datLen += size;
                datMsgs += size;

                entries[i]._flags = prevEntries[i]._flags;
                entries[i]._pad1 = prevEntries[i]._pad1;

                if (prevEntries[i]._offset == 0)
                {
                    entries[i]._offset = 0;
                }
                else
                {
                    entries[i]._offset = offset;
                    offset += size;
                }
            }
            datHeader->_size = ((int)DAT.Size + datLen).Align(0x10);

            MID* midHeader = newHeader->MIDSection;
            midHeader->_tag = MID.Tag;
            midHeader->_size = Header->MIDSection->_size;
            midHeader->_entryCount = Header->MIDSection->_entryCount;
            midHeader->_pad1 = Header->MIDSection->_pad1;
            midHeader->_pad2 = Header->MIDSection->_pad2;

            bint* ids = (bint*)midHeader->IDs;
            bint* prevIds = (bint*)Header->MIDSection->IDs;

            for (int i = 0; i < midHeader->_entryCount; i++)
            {
                (*ids++) = (*prevIds++);
            }

            BuildMessageLookup();
        }

        public DATEntryNode GetMessageByMID(int mid)
        {
            return Children[_midToChild[mid]] as DATEntryNode;
        }

        internal static ResourceNode TryParse(DataSource source)
        {
            return ((BMG*)source.Address)->_tag == BMG.Tag ? new BMGNode() : null;
        }
    }

    public unsafe class DATEntryNode : ResourceNode
    {
        public override ResourceType ResourceType { get { return ResourceType.DATEntry; } }

        internal BMGEncoding _encoding;
        internal string _message;
        internal int _mid;
        internal byte[] _encodedMessage;

        [Category("DAT")]
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                _encodedMessage = StringToBinary(_message, GetEncoding(_encoding));
                SignalPropertyChange();
            }
        }

        [Category("DAT")]
        public int MID
        {
            get
            {
                return _mid;
            }
        }

        public override string Name
        {
            get
            {
                return String.Format("DAT{0}", Index);
            }
            set
            {
                base.Name = value;
            }
        }

        public DATEntryNode(BMGEncoding encoding)
        {
            _encoding = encoding;
        }

        private static byte[] StringToBinary(string str, Encoding encoding)
        {
            MemoryStream output = new MemoryStream();

            str += "\0";

            UnescapeSequences(str, ref output, encoding);

            var temp = output.ToArray();
            output.Close();

            return temp;
        }

        private static string BinaryToString(byte[] binary, Encoding encoding)
        {
            char[] result = new char[0x3FF];
            int readBytes, readChars;
            bool success, replace;
            var decoder = encoding.GetDecoder();

            var pos = 0;
            var index = 0;
            var initial = index;

            while (index < binary.Length && pos < result.Length && !(binary[index] == 0x00 && binary[index + 1] == 0x00))
            {
                success = false;

                while (index < binary.Length && !success)
                {
                    decoder.Convert(binary, index, 2, result, pos, 1, false, out readBytes, out readChars, out success);

                    pos += readChars;
                    index += readBytes;
                }

                EscapeSequences(binary, ref index, result, ref pos);
            }

            return new string(result, 0, pos);
        }

        private static void EscapeSequences(byte[] binary, ref int index, char[] result, ref int pos)
        {
            int length;

            if (result[pos - 1] == '\\')
                result[pos++] = '\\';
            else if (result[pos - 1] == '\x001A')
            {
                result[pos - 1] = '\\';

                length = binary[index] - 2;
                if (binary[index + 1] == 1 && length == 6 && binary[index + 2] == 0 & binary[index + 3] == 0)
                {
                    result[pos++] = 'x';
                    index += 4;
                    length = 2;
                }
                else if (binary[index + 1] == 2 && length == 8)
                {
                    result[pos++] = 'q';
                    length -= 2;
                    index += 2;
                }
                else if (binary[index + 1] == 2 && length == 6)
                {
                    result[pos++] = 'p';
                    length -= 2;
                    index += 2;
                }
                else if (binary[index + 1] == 2 && length == 4)
                {
                    result[pos++] = 's';
                    length -= 2;
                    index += 2;
                }


                for (int i = 0; i < length; i++, index++)
                {
                    result[pos++] = binary[index].ToString("X2")[0];
                    result[pos++] = binary[index].ToString("X2")[1];
                }
            }
        }

        private static void UnescapeSequences(string value, ref MemoryStream output, Encoding encoding)
        {
            byte[] temp;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '\\' && value.Length >= i + 2)
                {
                    if (value[i + 1] == '\\')
                    {
                        temp = encoding.GetBytes(value.ToCharArray(), i++, 1);
                        output.Write(temp, 0, temp.Length);
                    }
                    else if (value[i + 1] == 'x' && value.Length >= i + 6)
                    {
                        temp = new byte[0x8];
                        temp[1] = 0x1a;
                        temp[2] = 0x8;
                        temp[3] = 0x1;
                        byte.TryParse(value.Substring(i + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x6]);
                        byte.TryParse(value.Substring(i + 4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x7]);
                        output.Write(temp, 0, temp.Length);
                        i += 5;
                    }
                    else if (value[i + 1] == 'q' && value.Length >= i + 14)
                    {
                        temp = new byte[0xa];
                        temp[1] = 0x1a;
                        temp[2] = 0xa;
                        temp[3] = 0x2;
                        byte.TryParse(value.Substring(i + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x4]);
                        byte.TryParse(value.Substring(i + 4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x5]);
                        byte.TryParse(value.Substring(i + 6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x6]);
                        byte.TryParse(value.Substring(i + 8, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x7]);
                        byte.TryParse(value.Substring(i + 10, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x8]);
                        byte.TryParse(value.Substring(i + 12, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x9]);
                        output.Write(temp, 0, temp.Length);
                        i += 13;
                    }
                    else if (value[i + 1] == 'p' && value.Length >= i + 10)
                    {
                        temp = new byte[0x8];
                        temp[1] = 0x1a;
                        temp[2] = 0x8;
                        temp[3] = 0x2;
                        byte.TryParse(value.Substring(i + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x4]);
                        byte.TryParse(value.Substring(i + 4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x5]);
                        byte.TryParse(value.Substring(i + 6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x6]);
                        byte.TryParse(value.Substring(i + 8, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x7]);
                        output.Write(temp, 0, temp.Length);
                        i += 9;
                    }
                    else if (value[i + 1] == 's' && value.Length >= i + 6)
                    {
                        temp = new byte[0x6];
                        temp[1] = 0x1a;
                        temp[2] = 0x6;
                        temp[3] = 0x2;
                        byte.TryParse(value.Substring(i + 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x4]);
                        byte.TryParse(value.Substring(i + 4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[0x5]);
                        output.Write(temp, 0, temp.Length);
                        i += 5;
                    }
                    else
                    {
                        byte length;
                        if (value.Length >= i + 3 && byte.TryParse(value.Substring(i + 1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out length) && value.Length >= i + 1 + (length - 2) * 2)
                        {
                            temp = new byte[length];
                            temp[1] = 0x1a;
                            temp[2] = length;

                            i += 3;
                            for (int j = 3; j < length; j++, i += 2)
                            {
                                byte.TryParse(value.Substring(i, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out temp[j]);
                            }
                            output.Write(temp, 0, temp.Length);
                            i--;
                        }
                        else
                        {
                            temp = encoding.GetBytes(value.ToCharArray(), i, 1);
                            output.Write(temp, 0, temp.Length);
                        }
                    }
                }
                else
                {
                    temp = encoding.GetBytes(value.ToCharArray(), i, 1);
                    output.Write(temp, 0, temp.Length);
                }
            }
        }

        private static Encoding GetEncoding(BMGEncoding _encoding)
        {
            var encodingStr = "UTF-8";

            switch (_encoding)
            {
                case BMGEncoding.CP1252:
                    encodingStr = "Windows-1252";
                    break;
                case BMGEncoding.UTF16:
                    encodingStr = "unicodeFFFE";
                    break;
                case BMGEncoding.SHIFT_JIS:
                    encodingStr = "Shift-JIS";
                    break;
                case BMGEncoding.UTF8:
                default:
                    encodingStr = "UTF-8";
                    break;
            }

            return Encoding.GetEncoding(encodingStr);
        }

        public override bool OnInitialize()
        {
            base.OnInitialize();

            var length = 0;
            if (_encoding == BMGEncoding.UTF16)
            {
                while (true)
                {
                    var s = Marshal.ReadInt16(WorkingUncompressed.Address + length);

                    if (s == 0x1A00)
                    {
                        length += 2;
                        s = Marshal.ReadInt16(WorkingUncompressed.Address + length);

                        // http://wiki.tockdom.com/wiki/BMG_(File_Format)#0x1A_Escape_Sequences
                        byte size = (byte)(s & 0x00FF);
                        if (size == 0x06)
                        {
                            length += 4;
                        }
                        else if (size == 0x08)
                        {
                            length += 6;
                        }
                        else if (size == 0x0A)
                        {
                            length += 8;
                        }
                        else if (size == 0x0C)
                        {
                            length += 10;
                        }
                    }
                    else if (s == 0x0000)
                    {
                        length += 2;
                        break;
                    }
                    else
                    {
                        length += 2;
                    }
                }
            }
            else
            {
                // TODO
                while (Marshal.ReadByte(WorkingUncompressed.Address + length) != 0)
                {
                    length++;
                }
            }

            _encodedMessage = new byte[length];
            Marshal.Copy(WorkingUncompressed.Address, _encodedMessage, 0, length);

            Message = BinaryToString(_encodedMessage, GetEncoding(_encoding));

            SetSizeInternal(length);

            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            if (length == 0)
            {
                return;
            }

            sbyte* ptr = (sbyte*)address;
            for (int i = 0; i < length; i++)
                *ptr++ = (sbyte)_encodedMessage[i];
            *ptr++ = 0;
        }

        public override int OnCalculateSize(bool force)
        {
            if (_encodedMessage.Length == 2)
            {
                return 0;
            }
            return _encodedMessage.Length;
        }
    }
}
