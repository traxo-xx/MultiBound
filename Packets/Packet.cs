using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public abstract partial class Packet
{
	public Packet(byte[] Bytes = null, bool ForcesVLQRecognition = false)
	{
		if (Bytes == null)
			return;
		if (Bytes.Length == 0)
			return;
		RawBytes = Bytes;
		OPCode = ReadByte(true);
		int length = ReadByte(true);
		index = 1;
		if (length != Bytes.Length - 2 | ForcesVLQRecognition) {
			//sVLQ packet
			length = (int)ReadsVLQ(true);
			if (length < 0) {
				//zlib compressed
                List<byte> metapayload = Misc.ToList(Bytes);
                Payload = Misc.Deflate(metapayload.GetRange(index, Bytes.Length - index).ToArray());
                isCompressed = true;
			} else {
				//uncompressed
                Payload = Misc.ToList(Bytes).GetRange(index, Bytes.Length - index).ToArray();
			}
		} else {
			//Non sVLQ packet
            Payload = Misc.ToList(Bytes).GetRange(2, Bytes.Length - 2).ToArray();
		}
		index = 0;
	}
	public virtual byte OPCode { get; set; }
    public bool isCompressed = false;
	public byte[] RawBytes { get; set; }
	public byte[] Payload { get; set; }
	public abstract byte[] GetByteArray();
	//Public MustOverride Sub LoadByteArray(ByVal ByteArray As Byte())
	private int index = 0;
	public byte[] Package(bool UsesVLQLengthing = false, bool DoubleLength = false)
	{
		dynamic size = null;
		dynamic mult = null;
		if (DoubleLength) {
			mult = 2;
		} else {
			mult = 1;
		}
		bool f = false;
        if ((Payload.Length * mult) > 256 | UsesVLQLengthing == true)
        {
			f = true;
            size = VLQ.TosVLQ(Payload.Length * mult);
		} else {
			f = false;
            size = Convert.ToByte(Payload.Length * mult);
		}
		if (!f) {
			byte[] ar = {
				OPCode,
				size
			};
            byte[][] lel = {ar,	Payload	};
            return Misc.Combine(lel);
		} else {
			byte[] ar = { OPCode };
            byte[][] lel = {ar,	size,Payload};
			return Misc.Combine(lel);
		}
	}
	public byte ReadByte(bool Raw = false)
	{
		dynamic r = null;
		if (Raw) {
			r = RawBytes[index];
		} else {
			r = Payload[index];
		}
		index += 1;
		return r;
	}
	public byte[] ReadByteArray(bool signed = false)
	{
        if (signed)
        {
            return ReadBytes((ulong)ReadsVLQ());
        }
        else
        {
            return ReadBytes(ReadVLQ());
        }
	}
	public byte[] ReadBytes(ulong Amount)
	{
		List<byte> r = new List<byte>();
		for (uint i = 1; i <= Amount; i++) {
			r.Add(Payload[index]);
			index += 1;
		}
		return r.ToArray();
	}
	public UInt32 ReadUInt32()
	{
		return BitConverter.ToUInt16(ReadBytes(4), 0);
	}
	public string ReadString(double Factor = 1)
	{
		dynamic len = ReadByte() * Factor;
		if (len < 1)
			return "";
		List<byte> l = new List<byte>();
		for (int i = 1; i <= len; i++) {
			l.Add(ReadByte());
		}
		return new string(System.Text.Encoding.UTF8.GetChars(l.ToArray()));
	}
	public ulong ReadVLQ(bool Raw = false)
	{
		List<byte> l = new List<byte>();
		bool b = false;
		dynamic i = index;
		while (!(b | i == RawBytes.Length))
        {
			l.Add(ReadByte(Raw));
			if ((new Binary().NewByte(l[l.Count - 1])).IsLastVLQByte())
				b = true;
		}
		if (b == false)
			throw new IndexOutOfRangeException("No VLQ (end) found");
		return VLQ.FromVLQ(l.ToArray());
	}
	public long ReadsVLQ(bool Raw = false)
	{
		List<byte> l = new List<byte>();
		bool b = false;
		dynamic i = index;
        while (!(b | i == RawBytes.Length))
        {
			l.Add(ReadByte(Raw));
            if ((new Binary().NewByte(l[l.Count - 1])).IsLastVLQByte())
				b = true;
		}
		if (b == false)
			throw new IndexOutOfRangeException("No VLQ (end) found");
		return VLQ.FromsVLQ(l.ToArray());
	}
    public dynamic ReadVariant()
	{
	  int varType = ReadByte();
	  
	  switch(varType)
	  {
        case 0x01:
          return null;
	    case 0x02:
          //double
          throw new NotImplementedException("Doubles aren't implemented as variants yet");
	    case 0x03:
	      //bool
          return ReadBoolean();
	    case 0x04:
	      //signed vlq
	      return ReadsVLQ();
	    case 0x05:
	      //string
	      return ReadString();
        case 0x06:
	      //variant
	      //return ReadVariant
	      uint varSize = ReadUInt32();
	      List<dynamic> varList = new List<dynamic>(); //[varSize];
	      for(int i = 0; i < varList.Count; i++)
	      {
	      	varList.Add(ReadVariant());
	      }
          return varList;
	    case 0x07:
	      //dict variant (map)
	      return ReadVariantDict();
        default:
          throw new Exception("ReadVariant failed!");
	  }
	}

    public Dictionary<string, int> ReadVariantDict(bool Raw = false)
	{
	  Dictionary<string, int> varDict = new Dictionary<string, int>();
	  UInt32 varSize = ReadUInt32();
	  try
	  {
          for (int i = 0; i < varSize; i++)
	    {
	      varDict.Add(ReadString(i), ReadVariant());
	    }
	    return varDict;
	  }
	  catch
	  {
	    throw new Exception("ReadVariant failed!");
	  }
	}

    public bool ReadBoolean()
    {
        return Convert.ToBoolean(ReadByte());
    }
}
