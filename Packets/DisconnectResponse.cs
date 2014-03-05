using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class DisconnectResponse : Packet
{

	public override byte OPCode { get; set; }
	public int Unknown { get; set; }

	public DisconnectResponse() : base()
	{
	}

	public override byte[] GetByteArray()
	{
		Payload = BitConverter.GetBytes(Unknown);
		return Package();
	}
}
