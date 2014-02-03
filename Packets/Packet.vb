Partial Public MustInherit Class Packet
    Sub New(ByVal Bytes() As Byte)
        FullBytes = Bytes
    End Sub
    Public Overridable Property OPCode As Byte
    Public Property FullBytes As Byte()
    Public Property Payload As Byte()
    Public MustOverride Function GetByteArray() As Byte()
    'Public MustOverride Sub LoadByteArray(ByVal ByteArray As Byte())
    Private index As UInteger = 0
    Public Function Package() As Byte()
        Throw New NotImplementedException("VLQ section not finished, this will be completed by then")
    End Function
    Public Function ReadByte() As Byte
        Dim r = Payload(index)
        index += 1
        Return r
    End Function
    Public Function ReadByteArray() As Byte()
        Return ReadBytes(ReadByte())
    End Function
    Public Function ReadBytes(ByVal Amount As UInteger) As Byte()
        Dim r As New List(Of Byte)
        For i As UInteger = 1 To Amount
            r.Add(Payload(index))
            index += 1
        Next
        Return r.ToArray
    End Function
    Public Function ReadUInt32() As UInt32
        Return BitConverter.ToUInt16(ReadBytes(4), 0)
    End Function
    Public Function ReadString(Optional ByVal Factor As Single = 1) As String
        Dim len = ReadByte() * Factor
        Dim l As New List(Of Byte)
        For i = 1 To len
            l.Add(ReadByte)
        Next
        Return System.Text.Encoding.UTF8.GetChars(l.ToArray)
    End Function
End Class
