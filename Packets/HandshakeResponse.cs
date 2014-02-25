using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class HandshakeResponse : Packet
{
	public override byte OPCode { get; set; }
	public string ClaimResponse { get; set; }
	public string ProcessedHash { get; set; }
	public HandshakeResponse(byte[] Bytes) : base(Bytes, false, true)
	{
		dynamic b = Payload;
		ClaimResponse = ReadString();
		ProcessedHash = ReadString();
	}

	public override byte[] GetByteArray()
	{
		return null;
	}
}
