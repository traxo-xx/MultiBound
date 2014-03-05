using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

public class StarServer
{
	public virtual ushort Port { get; set; }
	public virtual TcpListener Server { get; set; }
	public List<ConnectedClient> Clients { get; set; }
	public int IDCount { get; set; }
    public string Salt { get; set; }
    public int Rounds { get; set; }
    public string Password { get; set; }


    public event newConsoleLineEventHandler newConsoleLine = new newConsoleLineEventHandler((a, b, c) => { });
    public delegate void newConsoleLineEventHandler(string NewLine, System.Drawing.Color TextColor, bool Verbose);

    public void NewConsoleLine(string NewLine, System.Drawing.Color TextColor, bool Verbose)
    {
        newConsoleLine(NewLine, TextColor, Verbose);
    }

	public StarServer()
	{
        Port = 21025;
        Salt = "F7oF03xerVhWfRkVSbxY8YwzuZUYVTDt";
        Rounds = 5000;
        Password = "";
		Server = new TcpListener(IPAddress.Any, Port);
        Clients = new List<ConnectedClient>();
		Server.Start();
		System.Threading.Thread t = new System.Threading.Thread(DoListen);
		t.IsBackground = true;
		t.Start();
	}
	public void DoListen()
	{
        System.Net.Sockets.TcpClient incomingClient = null;
        NewConsoleLine("Initialized server on port " + Port.ToString(), System.Drawing.Color.Green, false);
        NewConsoleLine("Ready.", System.Drawing.Color.Lime, false);
		do {
			incomingClient = Server.AcceptTcpClient();
			ConnectedClient connClient = new ConnectedClient(incomingClient);
            NewConsoleLine("Incoming Connection @ " + connClient.mClient.Client.RemoteEndPoint.ToString(), System.Drawing.Color.Goldenrod, false);
			connClient.dataReceived += messageReceived;
			Clients.Add(connClient);
			ProtocolVersionPacket p = new ProtocolVersionPacket();
			Clients[Clients.Count - 1].SendMessage(p.GetByteArray());
		} while (true);
	}
    public event DataRecievedEventHandler DataRecieved = new DataRecievedEventHandler((a, c) => { });
	public delegate void DataRecievedEventHandler(ConnectedClient client, List<byte> Message);
	public void messageReceived(ConnectedClient sender, List<byte> Data)
	{
        if (DataRecieved != null)
        {
            DataRecieved(sender, Data);
        }
		try {
			byte[] arr = Data.ToArray();
			if (Data[0] != 0)
                //NewConsoleLine("Opcode 0x" + Data[0].ToString("X2"), System.Drawing.Color.White, true);
            switch ((OPCodes)Data[0]) {
				case OPCodes.ClientConnect:
                    NewConsoleLine("Reading client statement...", System.Drawing.Color.White, true);
					ClientConnectPacket con = new ClientConnectPacket(Misc.TrimNull(Data.ToArray()));
                    Clients[Clients.IndexOf(sender)].player = new Player(con);
                    NewConsoleLine("Success! Sending HandshakeChallenge...", System.Drawing.Color.Lime, true);
					sender.BufferSize = 0x4ff;
					HandshakeChallenge o = new HandshakeChallenge(Salt, Rounds);
					sender.SendMessage(o.GetByteArray());
					break;
				case OPCodes.HandshakeResponse:
                    NewConsoleLine("Hashing and checking password...", System.Drawing.Color.White, true);
					HandshakeResponse op = new HandshakeResponse(Misc.TrimNull(Data.ToArray()));
					dynamic hsh = Misc.Hash("", Password, Salt, Rounds);
					if (op.ProcessedHash == hsh | string.IsNullOrEmpty(Password)) {
                        NewConsoleLine("Password accepted.", System.Drawing.Color.Lime, false);
                        NewConsoleLine("!NOT FULLY IMPLEMENTED, WILL ACCEPT ALL PASSWORDS!", System.Drawing.Color.Red, false);
                        NewConsoleLine("Sending connect flag...", System.Drawing.Color.White, false);
						ConnectionResponse c = new ConnectionResponse(true, (IDCount + 1), "");
						IDCount += 1;
						sender.SendMessage(c.GetByteArray());
					} else {
                        NewConsoleLine("Incorrect password, rejecting client.", System.Drawing.Color.Red, false);
						ConnectionResponse c = new ConnectionResponse(false, 0, "Incorrect Password");
						sender.SendMessage(c.GetByteArray());
						Clients.Remove(sender);
						sender.mClient.Close();
					}
					break;
				case OPCodes.ClientDisconnect:
					sender.mClient.Close();
					break;
				case OPCodes.ClientContextUpdate:

					break;
				case OPCodes.ChatRecieve:
					ChatReceivePacket chatpacket = new ChatReceivePacket(Misc.TrimNull(Data.ToArray()));
                    NewConsoleLine("New chat: " + sender.player.Name + ": " + chatpacket.Text, System.Drawing.Color.White, false);
					ChatSendPacket chat = new ChatSendPacket("", ChatChannel.Universe, sender.player.Name, chatpacket.Text);
					chat.Text = chatpacket.Text;
					foreach (ConnectedClient p in Clients) {
						byte[] lel = chat.GetByteArray();
                        p.SendMessage(lel);
					}

					break;
				case 0:
					break;
				//Blank Data
				default:
					dynamic a = Misc.TrimNull(arr);
                    NewConsoleLine("Invalid opcode 0x" + Data[0].ToString("X2"), System.Drawing.Color.Red, true);
					// & "; data sent: " & BitConverter.ToString(Misc.TrimNull(arr)))
					break;
			}
		} catch {
		}
	}
}
public enum OPCodes
{
	ProtocolVersion = 0x0,
	ClientConnect = 0x7,
	ClientDisconnect = 0x8,
	HandshakeResponse = 0x9,
	ConnectResponse = 0x1,
	ClientContextUpdate = 0xd,
	ChatRecieve = 0xb
}
public class ConnectedClient
{

	public System.Net.Sockets.TcpClient mClient;
	public int BufferSize = 0xffff;
	private System.Threading.Thread readThread;

	public event dataReceivedEventHandler dataReceived = new dataReceivedEventHandler((a, b) => { });
	public delegate void dataReceivedEventHandler(ConnectedClient sender, List<byte> message);
    public event opcodeRecievedEventHandler opcodeRecieved = new opcodeRecievedEventHandler((a, b) => { });
	public delegate void opcodeRecievedEventHandler(ConnectedClient sender, byte opcode);

    public Player player;

	public ConnectedClient(System.Net.Sockets.TcpClient client)
	{
		mClient = client;

		readThread = new System.Threading.Thread(doRead);
		readThread.IsBackground = true;
		readThread.Start();
	}

	private void doRead()
	{
		mClient.ReceiveBufferSize = BufferSize;
		do {
			byte[] l = new byte[mClient.ReceiveBufferSize + 1];
			mClient.GetStream().Read(l, 0, mClient.ReceiveBufferSize);
			List<byte> l1 = Misc.ToList(l);
			if (dataReceived != null) {
				dataReceived(this, l1);
			}
			l1.Clear();
		} while (true);
	}

	public void SendMessage(byte[] msg)
	{
		try {
			lock (mClient.GetStream()) {
				mClient.GetStream().Write(msg, 0, msg.Length);
				mClient.GetStream().Flush();
			}
		} catch (Exception ex) {
			dynamic lel = ex;
		}
	}

}
