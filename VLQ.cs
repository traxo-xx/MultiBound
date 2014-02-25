using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public static class VLQ
{
	private static string concat(string[] str)
	{
		string s = "";
		foreach (string st in str) {
			s += st;
		}
		return s;
	}
	public static byte[] ToVLQ(int Value)
	{
		dynamic b = new Binary().NewInteger(Value);
		List<string> l = new List<string>();
		dynamic bb = b.Split7;
		for (int i = 0; i <= bb.Count - 1; i++) {
			if (i < bb.Count - 1) {
				l.Add("1" + bb(i));
			} else {
				l.Add("0" + bb(i));
			}
		}
		b = new Binary().NewString(concat(l.ToArray()));
		return b.ToBytes;
	}
	public static int FromVLQ(byte[] Value)
	{
		List<string> l = new List<string>();
		foreach (byte i in Value) {
			l.Add(Convert.ToString(i, 2));
		}
		dynamic b = new Binary().NewString(concat(l.ToArray()));
		l.Clear();
		foreach (string bb in b.Split8) {
			l.Add(bb.Substring(1, 7));
		}
		return Convert.ToInt32(concat(l.ToArray()), 2);
	}
	public static int FromsVLQ(byte[] Value)
	{
		List<string> l = new List<string>();
		foreach (byte i in Value) {
			l.Add(Convert.ToString(i, 2));
		}
		dynamic b = new Binary().NewString(concat(l.ToArray()));
		l.Clear();
		foreach (string bb in b.Split8) {
			l.Add(bb.Substring(1, 7));
		}
		return ((Convert.ToInt32(concat(l.ToArray()), 2)) + 0) / 2;
		//1) * 2
	}
	public static byte[] TosVLQ(int Value)
	{
		Value *= 2;
		//Value -= 1
		dynamic b = new Binary().NewInteger(Value);
		List<string> l = new List<string>();
		dynamic bb = b.Split7;
		for (int i = 0; i <= bb.Count - 1; i++) {
			if (i < bb.Count - 1) {
				l.Add("1" + bb(i));
			} else {
				l.Add("0" + bb(i));
			}
		}
		b = new Binary().NewString(concat(l.ToArray()));
		return b.ToBytes;
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
