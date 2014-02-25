using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class PacketBuilder
{
	public List<byte> Bytes { get; set; }
	public void Write(byte Value)
	{
		Bytes.Add(Value);
	}
	public void Write(byte[] Values)
	{
		foreach (byte v in Values) {
			Write(v);
		}
	}
	public void Write(sbyte Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(short Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(int Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(long Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(ushort Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(uint Value)
	{
		Write(Reverse(BitConverter.GetBytes(Value)));
	}
	public void Write(ulong Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(bool Value)
	{
		Write(BitConverter.GetBytes(Value));
	}
	public void Write(string Text)
	{
		if (Text.Length == 0 | string.IsNullOrEmpty(Text)){Bytes.Add(0);return;
}
		byte[] aarray = System.Text.Encoding.UTF8.GetBytes(Text);
		byte[] barray = { Convert.ToByte(Text.Length) };
        byte[][] carray = { barray, aarray };
        Bytes.AddRange(Misc.Combine(carray));
	}
	private byte[] Reverse(byte[] Bytes)
	{
        List<byte> l = new List<byte>(Bytes);
		l.Reverse();
		return l.ToArray();
	}

	public byte[] GetBytes()
	{
		return Bytes.ToArray();
	}
}
