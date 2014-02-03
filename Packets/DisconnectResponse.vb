Public Class DisconnectResponse
    Inherits Packet

    Public Overrides Property OPCode As Byte = &H2
    Public Property Unknown As Integer = 9001

    Sub New()
        MyBase.New(Nothing)
    End Sub

    Public Overrides Function GetByteArray() As Byte()
        Payload = BitConverter.GetBytes(Unknown)
        Return Package()
    End Function
End Class
