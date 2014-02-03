Public Class DisconnectResponse
    Inherits Packet

    Public Overrides Property OPCode As Byte = &H2
    Public Property Unknown As Int32 = 9001

    Sub New()
        MyBase.New(Nothing)
    End Sub

    Public Overrides Function GetByteArray() As Byte()

    End Function
End Class
