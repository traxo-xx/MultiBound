using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public static class VLQ
{
	public static byte[] ToVLQ(long Value)
	{
		dynamic b = new Binary().NewLong(Value);
        string s = "";
		string[] bb = b.Split7;
		for (int i = 0; i <= bb.Length - 1; i++) {
            if (i < bb.Length - 1)
            {
				s += "1" + bb[i];
			} else {
				s += "0" + bb[i];
			}
		}
		b = new Binary().NewString(s);
		return b.ToBytes;
	}
	public static byte[] TosVLQ(long Value)
	{
        long v = Value;
        bool neg = v < 0;
        v *= 2;
        if (neg) {
            v=- 1;
            v *= -1;
        }
        return ToVLQ(v);
	}
	public static ulong FromVLQ(byte[] Value)
	{
        string s = "";
        foreach (byte i in Value)
        {
            s += Convert.ToString(i, 2);
        }
        Binary b = new Binary().NewString(s);
        s = "";
        foreach (string bb in b.Split8())
        {
            s += bb.Substring(1, 7);
        }
        return Convert.ToUInt64(s, 2);
	}
	public static long FromsVLQ(byte[] Value)
	{
        ulong vlq = FromVLQ(Value);
        bool neg = vlq % 2 == 1;
        if (neg) vlq++;
        vlq /= 2;
        if (neg) return -(long)vlq;
        return (long)vlq;
	}
}
public class Binary
{
	public string Bits { get; set; }
	public Binary NewLong(long Value)
	{
		Bits = Convert.ToString(Value, 2);
		return this;
	}
	public Binary NewInteger(int Value)
	{
		Bits = Convert.ToString(Value, 2);
		return this;
	}
	public Binary NewUInteger(uint Value)
	{
		Bits = Convert.ToString(Value, 2).PadLeft(32, '0');
		return this;
	}
	public Binary NewByte(byte Value)
	{
		Bits = Convert.ToString(Value, 2).PadLeft(8, '0');
		return this;
	}
	public Binary NewSByte(sbyte Value)
	{
		Bits = Convert.ToString(Value, 2);
		return this;
	}
	public Binary NewString(string Value)
	{
		Bits = Value;
		return this;
	}
	public byte[] ToBytes()
	{
		List<byte> l = new List<byte>();
		foreach (string s in Split8()) {
			if (s == "00000000")
				continue;
			l.Add(Convert.ToByte(s, 2));
		}
		return l.ToArray();
	}
	public string[] Split7()
	{
        int pad = 7 - (Bits.Length % 7);
		List<string> l = new List<string>();
		string str = Bits.PadLeft(Bits.Length + pad, '0');
        for (int i = 0; i <= str.Length - 1; i += 7)
        {
			l.Add(str.Substring(i, 7));
		}
		return l.ToArray();
	}
	public string[] Split8()
	{
		int pad = 8 - (Bits.Length % 8);
		List<string> l = new List<string>();
		string str = Bits.PadLeft(Bits.Length + pad, '0');
        for (int i = 0; i <= str.Length - 1; i += 8)
        {
			l.Add(str.Substring(i, 8));
		}
		return l.ToArray();
	}
	public bool IsLastVLQByte()
	{
		if (Bits[0] == '0') {
			return true;
		} else {
			return false;
		}
	}
}
