using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class ConnectionResponse : Packet
{
	public override byte OPCode { get; set; }
	public bool Success { get; set; }
	public int ID { get; set; }
	public string RejectionReason { get; set; }
	public ConnectionResponse(bool _Success, int _ID, string _RejectionReason)
	{
		Success = _Success;
		ID = _ID;
		RejectionReason = _RejectionReason;
	}
	public override byte[] GetByteArray()
	{
		PacketBuilder p = new PacketBuilder();
		p.Write(Success);
		if (Success) {
			p.Write(VLQ.ToVLQ(ID));
			p.Write(Convert.ToByte(0));
		} else {
			p.Write(Convert.ToByte(0));
			p.Write(RejectionReason);
		}
		Payload = p.GetBytes();
		return Package(false, true);
	}
}
