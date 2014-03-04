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
        byte[] lel = Payload;
		AssetDigest = ReadByteArray();
		Claim = ReadVariant();
        if (ReadBoolean()) UUID = ReadBytes(16);
        Name = ReadString();
        Species = ReadString();
        Shipworld = ReadByteArray();
        Account = ReadString();
	}

	public override byte[] GetByteArray()
	{
		return null;
	}
}
