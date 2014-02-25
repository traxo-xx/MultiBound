using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class WorldStartPacket : Packet
{
	public override byte OPCode { get; set; }


	public WorldStartPacket(World World)
	{
	}

	public override byte[] GetByteArray()
	{
        return null;
	}
}
