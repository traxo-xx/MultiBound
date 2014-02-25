using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class ClientConnectPacket : Packet
{

	public override byte OPCode { get; set; }
	public byte[] AssetDigest { get; set; }
	public object Claim { get; set; }
	public byte[] UUID { get; set; }
	public string Name { get; set; }
	public string Species { get; set; }
	public byte[] Shipworld { get; set; }
	public string Account { get; set; }

	public ClientConnectPacket(byte[] Bytes) : base(Bytes)
	{
		Payload = Misc.Deflate(Payload);
		AssetDigest = ReadByteArray();
		dynamic b = ReadBytes(100);
		//Dim c = Payload
		//Dim cc = System.Text.Encoding.UTF8.GetChars(Payload)
		//GetByteArray() 'lel, this is placeholder for claim
		//Dim u = ReadByte()
		//If u = 1 Then UUID = ReadBytes(16)
		//Name = ReadString()
		//Species = ReadString()
		//There's no point in continuing here, but i'll finish this for another day
	}

	public override byte[] GetByteArray()
	{
		return null;
	}
}
