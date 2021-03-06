using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class ProtocolVersionPacket : Packet
{
	public override byte OPCode { get; set; }
	public const byte Length = 0x8;

	public const UInt32 Version = 642;
	public ProtocolVersionPacket() : base()
	{
	}

	public override byte[] GetByteArray()
	{
        OPCode = 0x0;
		PacketBuilder p = new PacketBuilder();
		p.Write(Version);
		Payload = p.GetBytes();
		return Package();
	}
}
