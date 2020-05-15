using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BrawlLib.SSBB.Types
{
    public enum BMGEncoding : byte
    {
        CP1252 = 0x1,
        UTF16 = 0x2,
        SHIFT_JIS = 0x3,
        UTF8 = 0x4
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct BMG
    {
        public const uint Tag = 0x4753454D;
        public const uint Tag2 = 0x31676D62;
        public const int Size = 0x20;

        public uint _tag;
        public uint _tag2;
        public bint _size;
        public bint _sections;
        public BMGEncoding _encoding;
        public int _pad1;

        public VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }

        public INF* INFSection { get { return (INF*)((byte*)Address + Size); } }
        public DAT* DATSection { get { return (DAT*)((byte*)INFSection + INFSection->_size); } }
        public MID* MIDSection { get { return (MID*)((byte*)DATSection + DATSection->_size); } }

        public BMG(bint size, bint sections, BMGEncoding encoding)
        {
            _tag = Tag;
            _tag2 = Tag2;
            _size = size;
            _sections = sections;
            _encoding = encoding;
            _pad1 = 0x00;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct INF
    {
        public const uint Tag = 0x31464E49;
        public const uint Size = 0x10;

        public uint _tag;
        public bint _size;
        public bshort _messageCount;
        public bshort _messageSize;
        public int _pad1;
        
        public VoidPtr GetMessage(int index)
        {
            var ofs = Offsets(index)->_offset;
            return (VoidPtr)((byte*)Address + _size + Offsets(index)->_offset + DAT.Size); 
        }

        public INFEntry* Offsets(int index) 
        {
            return (INFEntry*)((byte*)Address + Size + index * _messageSize); 
        }

        public VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct INFEntry
    {
        public const uint Size = 0xC;

        public bint _offset;
        public bint _flags;
        public bint _pad1;

        public VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DAT
    {
        public const uint Tag = 0x31544144;
        public const uint Size = 0x8;

        public uint _tag;
        public bint _size;

        public VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
        public VoidPtr Messages { get { return ((byte*)Address + 8); } }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct MID
    {
        public const uint Tag = 0x3144494D;
        public const uint Size = 0x10;

        public uint _tag;
        public bint _size;
        public bshort _entryCount;
        public bshort _pad1;
        public bint _pad2;

        public VoidPtr Address { get { fixed (void* ptr = &this) return ptr; } }
        public VoidPtr IDs { get { return ((byte*)Address + Size); } }
        public bint* GetMessage(int index) { if (index >= 0 && index < _entryCount) { return (bint*)((byte*)IDs + index * 4); } else { return null; } }
    }
}
