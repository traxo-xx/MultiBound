Public Class ConnectionResponse
    Inherits Packet
    Public Overrides Property OPCode As Byte = &H1
    Public Property Success As Boolean
    Public Property ID As UInteger
    Public Property RejectionReason As String
    Sub New(ByVal _Success As Boolean, ByVal _ID As UInteger, ByVal _RejectionReason As String)
        Success = _Success
        ID = _ID
        RejectionReason = _RejectionReason
    End Sub
    Public Overrides Function GetByteArray() As Byte()
        Dim p As New PacketBuilder
        p.Write(Success)
        If Success Then
            p.Write(VLQ.ToVLQ(ID))
            p.Write(CByte(0))
        Else
            p.Write(VLQ.ToVLQ(0))
            p.Write(RejectionReason)
        End If
        Payload = p.GetBytes
        Return Package(False, True)
    End Function
End Class
