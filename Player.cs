using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class Player
{
	public string Name { get; set; }
	public string Species { get; set; }
	public string Account { get; set; }
	public byte[] UUID { get; set; }
	public List<string> ChatLog { get; set; }
    public ClientConnectPacket Raw { get; set; }
	public Player(ClientConnectPacket Connect)
	{
		Name = Connect.Name;
		Species = Connect.Species;
		Account = Connect.Account;
		UUID = Connect.UUID;
        Raw = Connect;
	}
}
