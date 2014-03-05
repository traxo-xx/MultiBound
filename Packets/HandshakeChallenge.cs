using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class HandshakeChallenge : Packet
{
	public override byte OPCode { get; set; }
	public string Salt { get; set; }
	public int Rounds { get; set; }
	public HandshakeChallenge(string _Salt, int _Rounds = 5000) : base()
	{
        OPCode = 0x3;
		Salt = _Salt;
        Rounds = _Rounds;
	}
	public override byte[] GetByteArray()
	{
		PacketBuilder p = new PacketBuilder();
		p.Write(((byte)0));
		p.Write(Salt);
		p.Write(Rounds);
		Payload = p.GetBytes();
        byte[] lel = Package();
        return Package();
	}
}
