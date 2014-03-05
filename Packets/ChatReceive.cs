using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class ChatReceivePacket : Packet
{

	public override byte OPCode { get; set; }
	public byte Length { get; set; }
	public string World { get; set; }
	public ChatChannel Channel { get; set; }
	public UInt32 ID { get; set; }
	public string Name { get; set; }
	public string Text { get; set; }

	public ChatReceivePacket(byte[] Bytes) : base(Bytes)
	{
		//Length = Bytes(1) / 2
		//Dim a = BasicUFL.ArrayConversion.ToList(Bytes)
		//a.RemoveRange(0, 2)
		//Text = System.Text.Encoding.UTF8.GetChars(a.toarray)
		//ChatChannel = ReadByte()
		//World = ReadString()
		//ID = ReadUInt32()
		//Name = ReadString()
		Text = ReadString();
	}
	public override byte[] GetByteArray()
	{
		return null;
	}
}
