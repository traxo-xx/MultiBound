Public Class ChatSendPacket
    Inherits Packet

    Public Overrides Property OPCode As Byte = &H4
    Public Property Length As Byte = 0
    Public Property World As String = ""
    Public Property ChatChannel As ChatChannel
    Public Property ID As UInt32 = 9001
    Public Property Name As String = ""
    Public Property Text As String = ""

    Sub New(ByVal _World As String, ByVal Channel As ChatChannel, ByVal PlayerName As String, ByVal Chat As String)
        MyBase.New()
        World = _World
        ChatChannel = Channel
        Name = PlayerName
        Text = Chat
    End Sub
    Public Overrides Function GetByteArray() As Byte()
        'Length = (6 + Name.Length + 1 + Text.Length) * 2
        'TextLength = Text.Length
        'NameLength = Name.Length
        'Dim aarray As Byte()
        'Dim barray As Byte() = System.Text.Encoding.UTF8.GetBytes(Text)
        'Dim carray As Byte() = System.Text.Encoding.UTF8.GetBytes(Name)
        'Dim darray As Byte() = {OPCode, Length, 1, 0, 0, 0, 0, 1, NameLength}
        'aarray = Misc.Combine({darray, carray})
        'Dim earray = {TextLength}
        'Return Misc.Combine({aarray, earray, barray})
        Dim p As New PacketBuilder()
        p.Write(CByte(ChatChannel))
        p.Write(World)
        p.Write(ID)
        p.Write(Name)
        p.Write(Text)
        Payload = p.GetBytes
        Return Package(False, True)
    End Function
End Class
