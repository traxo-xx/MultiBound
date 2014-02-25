using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class ChatSendPacket : Packet
{

	public override byte OPCode { get; set; }
	public byte Length { get; set; }
	public string World { get; set; }
	public ChatChannel Channel { get; set; }
	public UInt32 ID { get; set; }
	public string Name { get; set; }
	public string Text { get; set; }

	public ChatSendPacket(string _World, ChatChannel _Channel, string PlayerName, string Chat) : base()
	{
		World = _World;
		Channel = _Channel;
		Name = PlayerName;
		Text = Chat;
	}
	public override byte[] GetByteArray()
	{
		//Length = (6 + Name.Length + 1 + Text.Length) * 2
		//TextLength = Text.Length
		//NameLength = Name.Length
		//Dim aarray As Byte()
		//Dim barray As Byte() = System.Text.Encoding.UTF8.GetBytes(Text)
		//Dim carray As Byte() = System.Text.Encoding.UTF8.GetBytes(Name)
		//Dim darray As Byte() = {OPCode, Length, 1, 0, 0, 0, 0, 1, NameLength}
		//aarray = Misc.Combine({darray, carray})
		//Dim earray = {TextLength}
		//Return Misc.Combine({aarray, earray, barray})
		PacketBuilder p = new PacketBuilder();
		p.Write(Convert.ToByte(Channel));
		p.Write(World);
		p.Write(ID);
		p.Write(Name);
		p.Write(Text);
		Payload = p.GetBytes();
		return Package(false, true);
	}
}
