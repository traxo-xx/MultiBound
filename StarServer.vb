Imports System.Net
Imports System.Net.Sockets
Imports HashLib

Public Class StarServer
    'Inherits RCore.SS

    Public Overridable Property Port As UShort = 0
    Public Overridable Property Server As Sockets.TcpListener
    Public Property Clients As New List(Of ConnectedClient)
    Public Property IDCount As Integer = 0

    Public Sub New(ByVal ServerPort As UShort)
        Server = New TcpListener(IPAddress.Any, ServerPort)
        Server.Start()
        Dim t As New Threading.Thread(AddressOf DoListen)
        t.IsBackground = True
        t.Start()
        Console.WriteLine("Initialized server on port " & ServerPort.ToString)
    End Sub
    Public Sub DoListen()
        Dim incomingClient As System.Net.Sockets.TcpClient
        Console.WriteLine("Ready.")
        Do
            incomingClient = Server.AcceptTcpClient
            Dim connClient As New ConnectedClient(incomingClient)
            Console.WriteLine("Incoming Connection @ " & connClient.mClient.Client.LocalEndPoint.ToString)
            AddHandler connClient.dataReceived, AddressOf messageReceived
            Clients.Add(connClient)
            Dim p As New ProtocolVersionPacket()
            Dim pack = p.GetByteArray
            Clients.Last.SendMessage(p.GetByteArray)
        Loop
    End Sub
    Public Event DataRecieved(ByVal client As ConnectedClient, ByVal Message As List(Of Byte))
    Public Sub messageReceived(ByVal sender As ConnectedClient, ByVal Data As List(Of Byte))
        RaiseEvent DataRecieved(sender, Data)
        Try
            Dim arr As Byte() = BasicUFL.ArrayConversion.FromList(Data)
            If Data(0) <> 0 Then Console.WriteLine("Opcode 0x" & Data(0).ToString("X2"))
            Select Case Data(0)
                Case OPCodes.ClientConnect
                    Console.WriteLine("Reading client statement...")
                    'Dim con As New ClientConnectPacket(Misc.TrimNull(Data.ToArray))
                    Console.WriteLine("Success! Sending HandshakeChallenge...")
                    Dim o As New HandshakeChallenge(Main.Salt, Main.Rounds)
                    sender.SendMessage(o.GetByteArray())
                Case OPCodes.HandshakeResponse
                    Console.WriteLine("Hashing and checking password...")
                    Dim o As New HandshakeResponse(Misc.TrimNull(Data.ToArray))
                    Dim hsh = Hash(Main.Password, Main.Salt, Main.Rounds)
                    If o.ProcessedHash = hsh Or Main.Password = "" Then
                        Console.WriteLine("Password confirmed, sending connect response...")
                        Dim c As New ConnectionResponse(True, IDCount + 1, "")
                        IDCount += 1
                        sender.SendMessage(c.GetByteArray)
                    Else
                        Console.WriteLine("Incorrect password, rejecting client...")
                        Dim c As New ConnectionResponse(False, 0, "Incorrect Password")
                        sender.SendMessage(c.GetByteArray)
                        Clients.Remove(sender)
                        sender.mClient.Close()
                    End If
                Case OPCodes.ClientDisconnect
                    sender.mClient.Close()
                Case OPCodes.ClientContextUpdate

                Case OPCodes.ChatRecieve
                    Dim chatpacket As New ChatReceivePacket(Misc.TrimNull(Data.ToArray))
                    Console.WriteLine("New chat: " & sender.Username & ": " & chatpacket.Text)
                    Dim chat As New ChatSendPacket("", ChatChannel.Universe, "player", chatpacket.Text)
                    chat.Text = chatpacket.Text
                    For Each p In Clients
                        Dim lel = chat.GetByteArray
                        p.SendMessage(lel)
                    Next
                Case 0
                    'Blank Data
                Case Else
                    Dim a = Misc.TrimNull(arr)
                    Console.WriteLine("Invalid opcode 0x" & Data(0).ToString("X2")) ' & "; data sent: " & BitConverter.ToString(Misc.TrimNull(arr)))
#If DEBUG Then
                    My.Computer.FileSystem.WriteAllBytes("C:\Users\Jesse\Documents\0x" & Data(0).ToString("X2"), a, False)
#End If
            End Select
        Catch
        End Try
    End Sub
    Public Function Hash(ByVal Password As String, ByVal Salt As String, ByVal Rounds As Integer) As String
        'Dim hh As IHash = HashFactory.Crypto.CreateSHA256()
        'hh.Initialize()
        Dim h As New System.Security.Cryptography.SHA256Managed
        Dim hsh As String = Password
        For i As Integer = 1 To Rounds
            h.Initialize()
            hsh = Convert.ToBase64String(h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hsh & Salt)))
        Next
        Return hsh
    End Function
    Public Enum OPCodes
        ProtocolVersion = &H0
        ClientConnect = &H6
        HandshakeChallenge = &H3
        ClientDisconnect = &H7
        HandshakeResponse = &H8
        ConnectResponse = &H1
        ClientContextUpdate = &HB
        ChatRecieve = &HA
    End Enum
    'Public HandshakeChallenge As Byte() = {&H3, &H4C, &H0, &H20, &H7A, &H64, &H71, &H6B, &H35, &H6B, &H62, &H54, &H6D, &H42, &H4D, &H4C, &H37, &H42, &H6F, &H73, &H52, &H58, &H79, &H35, &H53, &H6C, &H74, &H46, &H72, &H73, &H69, &H55, &H4A, &H6C, &H69, &H47, &H0, &H0, &H13, &H88}
    'Public ConnectResponse As Byte() = {&H1, &H6, &H1, &H47, &H0, &HD, &H81, &HF8, &H7D, &H78, &H9C, &HED, &HDC, &H7B, &HB0, &HA4, &HF9, &H59, &HD8, &HF7, &HEE, &HF7, &H9C, &HEE, &H73, &HBA, &HFB, &H9C, &HB7, &HFB, &HED, &H7E, &HEF, &H7B, &HBF, &HDF, &HEF, &HB3, &HB3, &HB3, &HB3, &HB3, &H33, &H67, &H67, &HCE, &HDE, &H77, &HC3, &H45, &HA, &H89, &H14, &H9C, &H64, &H11, &H8B, &HB4, &H68, &HD1, &HA, &HED, &HA, &H84, &H42, &H2, &H2, &HC9, &H84, &HD8, &H5C, &H14, &HCA, &HC2, &HB8, &H20, &H45, &H2A, &HC1, &H94, &HC1, &H8A, &H63, &H8C, &H53, &H5C, &HA, &H8, &H89, &H83, &H43, &HA2, &H24, &H24, &H85, &H9D, &H38, &H65, &H63, &H92, &HB2, &H63, &H1C, &HE3, &HA, &H29, &HFA, &H7E, &H53, &H76, &H66, &H57, &HBF, &H4F, &H52, &HB5, &H23, &H8E, &HFD, &HA7, &H2B, &H5B, &HB5, &H55, &HDF, &H3A, &HE7, &H47, &H9F, &HD1, &HF3, &H3C, &H9F, &HD1, &HFC, &H31, &HE8, &HB3, &H1F, &H68, &H34, &H76, &H9F, &H7A, &HFB, &HDF, &HC1, &H67, &H9F, &H6E, &HFC, &H49, &HFF, &H5C, &HFC, &HE9, &H3F, &HF1, &HC9, &HFF, &HFF, &HCF, &HBF, &HD0, &HFF, &H1C, &H7F, &HFA, &H4F, &H7A, &H91, &HC5, &H7, &H2F, &H7D, &HF7, &HA3, &H47, &HDF, &HD3, &H68, &H7C, &HF6, &H77, &H87, &HCD, &H4B, &HDB, &H46, &HE3, &HD2, &H97, &H1B, &H8D, &HEA, &H81, &H37, &H3F, &HF2, &HDA, &HC7, &HDF, &H7C, &HE0, &H23, &H9F, &HFC, &HB6, &H57, &H3E, &HF6, &H4E, &HBF, &HFD, &HEF, &HFD, &H1F, &HFF, &HD8, &H87, &H1B, &H1F, &H6D, &H34, &HA2, &HBD, &HB7, &HFF, &H1D, &HC5, &HAF, &H7D, &HEC, &H3B, &H5E, &H7B, &HF3, &HB5, &H6F, &H7A, &HFD, &HD5, &HD7, &H5F, &HFB, &HF0, &H47, &HDE, &H6A, &HEC, &H35, &H7B, &H9F, &HFC, &HD8, &H37, &H7D, &HE2, &HD5, &H57, &H3E, &HFA, &HCA, &HDB, &H5F, &HDB, &H69, &HBE, &HFD, &H66, &HFF, &H84, &HEF, &H3A, &H27, &H7C, &HD7, &H3B, &HE1, &HBB, &HC3, &H13, &HBE, &HEB, &H9F, &HF0, &HDD, &HC3, &H27, &H7C, &H77, &HEA, &H84, &HEF, &H4E, &H9F, &HF0, &HDD, &H99, &H13, &HBE, &H3B, &H7B, &HC2, &H77, &H97, &HF7, &H36, &H38, &HE1, &H3E, &H4E, &HF2, &HAE, &H77, &HC2, &H77, &H87, &H27, &H7C, &HD7, &H3F, &HE1, &HBB, &HE4, &H84, &HEF, &HF2, &H13, &HBE, &H2B, &H4F, &HF8, &HAE, &H3E, &HE1, &HBB, &H6B, &H4F, &HF8, &HEE, &HFA, &H13, &HBE, &HBB, &HE5, &H84, &HEF, &H6E, &H3B, &HE1, &HBB, &H3B, &H4E, &HF8, &HEE, &HC1, &H13, &HBE, &H7B, &HF8, &H84, &HEF, &H4E, &H9D, &HF0, &HDD, &HE9, &H13, &HBE, &H3B, &H73, &HC2, &H77, &H67, &H4F, &HF8, &HEE, &HDC, &H9, &HDF, &HC5, &H97, &H6F, &HFA, &HF0, &H95, &H8F, &HBF, &HFA, &HA9, &HCB, &HBF, &H2D, &HBE, &HFB, &HAC, &HF1, &HEE, &H21, &HBD, &HE7, &H37, &H6E, &HB8, &HDA, &H37, &H6E, &HBF, &HDA, &H37, &HEE, &HBB, &HDA, &H37, &H2E, &HF, &HFB, &HF0, &H84, &HC3, &H3E, &HC9, &HBB, &HD3, &H27, &H7C, &H77, &HE6, &H84, &HEF, &HCE, &H9E, &HF0, &HDD, &HB9, &H13, &HBE, &H8B, &HDE, &HFE, &HF7, &HE0, &H4, &HEF, &H76, &H4F, &HF8, &HAE, &H7D, &HC2, &H77, &HFB, &H27, &H7C, &H77, &H79, &H1F, &HDD, &H13, &HEE, &HE3, &H24, &HEF, &H4E, &H9F, &HF0, &HDD, &H99, &H13, &HBE, &H3B, &H7B, &HC2, &H77, &HE7, &H4E, &HF8, &HEE, &HFC, &H9, &HDF, &H5D, &HDE, &HDB, &HFE, &H9, &HDE, &HED, &H9C, &HF0, &HDD, &HEE, &H9, &HDF, &HB5, &H4E, &HF8, &HAE, &H7D, &HC2, &H77, &H7B, &H27, &H7C, &HF7, &HE8, &HE5, &HB7, &H57, &HC0, &H7E, &HCB, &H27, &H5F, &H7D, &HFD, &H23, &HAF, &HBC, &HF5, &HA1, &H8F, &HBC, &HC7, &HB3, &HE6, &HE6, &HF2, &H8F, &H4E, &HBE, &HE9, &H8D, &H37, &HDE, &H7C, &HEB, &HD5, &H4F, &H7C, &HCB, &HEB, &HAF, &H7C, &HDB, &HAB, &H57, &HFE, &H90, &H75, &HB5, &HFF, &H5A, &H6A, &HC7, &HE1, &HB7, &H80, &H37, &H3E, &HF4, &HD1, &H57, &H3F, &HD1, &HD8, &HDB, &HF9, &HFF, &HBE, &H6B, &HBD, &HFE, &HEA, &H77, &HBC, &HFA, &HFA, &H6E, &H74, &HF8, &HD6, &HDB, &H5F, &H7B, &HF3, &H93, &H9F, &H78, &HF5, &H6B, &HDF, &H78, &HE3, &HF5, &H37, &HDB, &HCD, &H56, &H7A, &HE5, &H73, &HDF, &HF7, &HD6, &H2B, &H9F, &H78, &HFB, &H7, &HBD, &HFF, &HDD, &H6F, &HBE, &HFB, &H1B, &H4B, &HBB, &HF7, &HD6, &HAB, &H1F, &HFA, &HC8, &H9B, &H6F, &HBD, &HF2, &HD6, &H6B, &H6F, &HBC, &HE7, &H8F, &HBE, &HBC, &HBC, &HD6, &H9, &H97, &H72, &H92, &H77, &HED, &H13, &HBE, &HDB, &H3F, &HE1, &HBB, &HCB, &HBF, &H3, &HEF, &H76, &HDF, &H7A, &HF5, &HF5, &H57, &H3F, &HFE, &HC6, &HE5, &HFF, &H7C, &H57, &H83, &HB9, &H7B, &HF0, &H95, &HF1, &H7D, &HF3, &H1B, &H6F, &H7C, &HE2, &H9D, &HDF, &H40, &H1F, &HBF, &HFC, &HF5, &HE4, &HF2, &HD7, &H3F, &HF4, &HCA, &HC7, &HDF, &H7A, &HE5, &HB5, &H8F, &HBD, &HF9, &HA1, &H8F, &HBC, &HF2, &HDA, &H7B, &H7E, &HC4, &HE5, &HDD, &H37, &H4F, &HF8, &H7, &HA6, &H93, &HBC, &HEB, &H9D, &HF0, &HDD, &HE1, &H9, &HDF, &HF5, &H4F, &HF8, &H2E, &H39, &HE1, &HBB, &H7, &H4F, &HF8, &HEE, &HE1, &H13, &HBE, &H3B, &H75, &HC2, &H77, &HA7, &H4F, &HF8, &HEE, &HCC, &H9, &HDF, &H9D, &H3D, &HE1, &HBB, &H73, &H27, &H7C, &H77, &HFE, &H84, &HEF, &HB2, &HB7, &H81, &H7F, &H39, &H79, &HF3, &HDB, &H5E, &H79, &HFD, &HF5, &HFF, &HB7, &HF0, &HF7, &H7A, &H7A, &HE3, &HC9, &H9F, &HDE, &H7B, &HF2, &HA7, &H6F, &H1F, &H6C, &H73, &H7B, &H32, &H67, &H27, &H7A, &HD7, &H39, &HE1, &HBB, &HDE, &H9, &HDF, &H1D, &H9E, &HF0, &H5D, &HFF, &H84, &HEF, &H1E, &H3C, &HE1, &HBB, &H87, &H4F, &HF8, &HEE, &HD4, &H9, &HDF, &H9D, &H3E, &HE1, &HBB, &H33, &H27, &H7C, &H77, &HF6, &H84, &HEF, &HCE, &HFD, &H33, &HCC, &H65, &H75, &HC2, &HB9, &H9C, &HE4, &HDD, &HA9, &H13, &HBE, &H3B, &H7D, &HC2, &H77, &H67, &H4E, &HF8, &HEE, &HEC, &H49, &HDE, &HFD, &HE4, &HE0, &HED, &H57, &HE3, &HB7, &H5F, &H5F, &HF3, &HFD, &H5F, &HFE, &HEE, &HB7, &H73, &H22, &HA7, &H72, &H26, &HE7, &H72, &H21, &H97, &H72, &H25, &HD7, &H72, &H23, &HB7, &HF2, &HCB, &H21, &HA3, &H86, &H6C, &HCA, &H48, &HEE, &HC8, &H5D, &HD9, &H92, &H6D, &HB9, &H27, &HF7, &H65, &H47, &H76, &H65, &H4F, &H1E, &HC8, &H43, &H19, &HCB, &HBE, &H1C, &HC8, &H44, &HE, &HE5, &H48, &HA6, &H32, &H93, &HB9, &H2C, &H64, &H29, &H2B, &H59, &HCB, &H6B, &HE4, &HB5, &HF2, &H3A, &H79, &HBD, &HBC, &H41, &HDE, &H28, &H6F, &H92, &H37, &HCB, &H5B, &HE4, &HAD, &HF2, &H36, &H79, &HBB, &HBC, &H43, &HDE, &H29, &HEF, &H92, &H77, &HCB, &H7B, &HE4, &HBD, &HF2, &H3E, &H79, &HBF, &H7C, &H40, &H3E, &H28, &H1F, &H92, &HF, &HCB, &H47, &HE4, &H29, &HF9, &HA8, &H3C, &H2D, &H1F, &H93, &H67, &HE4, &HE3, &HF2, &HAC, &H7C, &H42, &H9E, &H93, &H4F, &HCA, &HF3, &HF2, &H82, &H3C, &H92, &H4F, &HC9, &H8B, &HF2, &H92, &H3C, &H96, &H4F, &HCB, &H67, &HE4, &HB3, &HF2, &H39, &HF9, &HBC, &H7C, &H41, &HBE, &H28, &H5F, &H92, &H2F, &H87, &HBC, &HA2}
End Class
Public Class ConnectedClient
    Public mClient As System.Net.Sockets.TcpClient

    Private readThread As System.Threading.Thread
    Private heartbeatThread As System.Threading.Thread

    Public Event dataReceived(ByVal sender As ConnectedClient, ByVal message As List(Of Byte))
    Public Event opcodeRecieved(ByVal sender As ConnectedClient, ByVal opcode As Byte)

    Sub New(ByVal client As System.Net.Sockets.TcpClient)
        mClient = client

        readThread = New System.Threading.Thread(AddressOf doRead)
        readThread.IsBackground = True
        readThread.Start()
        heartbeatThread = New System.Threading.Thread(AddressOf heartbeat)
        heartbeatThread.IsBackground = True
        Console.WriteLine(mClient.GetStream.CanRead.ToString)
        'heartbeatThread.Start()
    End Sub

    Public Property Username As String

    Private Sub doRead()
        mClient.ReceiveBufferSize = &HAFFF
        Do
            Dim l(mClient.ReceiveBufferSize) As Byte
            mClient.GetStream.Read(l, 0, mClient.ReceiveBufferSize)
            Dim l1 = BasicUFL.ArrayConversion.ToList(l)
            RaiseEvent dataReceived(Me, l1)
            l1.Clear()
        Loop
    End Sub
    Private Sub doReadSafeguraded()
        Dim ll As New List(Of Byte)
        mClient.ReceiveBufferSize = &HAFFF
        Do
            Dim l(mClient.ReceiveBufferSize) As Byte
            mClient.GetStream.Read(l, 0, mClient.ReceiveBufferSize)
            Dim l1 = BasicUFL.ArrayConversion.ToList(l)
            If l1.Last = 0 And l1.First = 0 Then
                If ll.Count = 0 Then Continue Do
                If ll(0) > 0 Then
                    RaiseEvent dataReceived(Me, ll)
                End If
                ll.Clear()
                Continue Do
            End If
            ll.AddRange(l1)
        Loop
    End Sub

    Private heartbeatcount As Byte = 0
    Private Sub heartbeat()
        Do
            If heartbeatcount = 255 Then
                heartbeatcount = 0
            End If
            heartbeatcount += 1
            SendMessage({&H30, &H2, heartbeatcount})
            Threading.Thread.Sleep(150)
        Loop
    End Sub

    Public Sub SendMessage(ByVal msg As Byte())
        Try
            SyncLock mClient.GetStream
                mClient.GetStream.Write(msg, 0, msg.Length)
                mClient.GetStream.Flush()
            End SyncLock
        Catch ex As Exception
            Console.WriteLine("ERROR: " & ex.Message)
        End Try
    End Sub

End Class
