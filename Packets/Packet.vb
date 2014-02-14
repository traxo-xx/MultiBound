Partial Public MustInherit Class Packet
    Sub New(Optional ByVal Bytes() As Byte = Nothing, Optional ByVal ForcesVLQRecognition As Boolean = False, Optional ByVal DoubleLength As Boolean = False)
        If Bytes Is Nothing Then Exit Sub
        If Bytes.Length = 0 Then Exit Sub
        RawBytes = Bytes
        OPCode = ReadByte(True)
        Dim length As Integer = ReadByte(True)
        If DoubleLength Then length /= 2
        index = 0
        If length <> Bytes.Length - 2 Or ForcesVLQRecognition Then
            'sVLQ packet
            index += 1
            length = ReadsVLQ(True)
            If length < 0 Then
                'zlib compressed
                Payload = Misc.Deflate(BasicUFL.ToList(Bytes).GetRange(index, Bytes.Count - index).ToArray)
            Else
                'uncompressed
                Payload = BasicUFL.ToList(Bytes).GetRange(index, Bytes.Count - index).ToArray
            End If
        Else
            'Non sVLQ packet
            Payload = BasicUFL.ToList(Bytes).GetRange(2, Bytes.Count - 2).ToArray
        End If
        index = 0
    End Sub
    Public Overridable Property OPCode As Byte
    Public Property RawBytes As Byte()
    Public Property Payload As Byte()
    Public MustOverride Function GetByteArray() As Byte()
    'Public MustOverride Sub LoadByteArray(ByVal ByteArray As Byte())
    Private index As UInteger = 0
    Public Function Package(Optional ByVal UsesVLQLengthing As Boolean = False, Optional ByVal DoubleLength As Boolean = False) As Byte()
        Dim size
        Dim mult
        If DoubleLength Then
            mult = 2
        Else
            mult = 1
        End If
        Dim f As Boolean
        If (Payload.Count * mult) > 256 Or UsesVLQLengthing = True Then
            f = True
            size = TosVLQ(Payload.Count * mult)
        Else
            f = False
            size = CByte(Payload.Count * mult)
        End If
        If Not f Then
            Dim ar As Byte() = {OPCode, size}
            Return Combine({ar, Payload})
        Else
            Dim ar As Byte() = {OPCode}
            Return Combine({ar, size, Payload})
        End If
    End Function
    Public Function ReadByte(Optional ByVal Raw As Boolean = False) As Byte
        Dim r
        If Raw Then
            r = RawBytes(index)
        Else
            r = Payload(index)
        End If
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
        If len < 1 Then Return ""
        Dim l As New List(Of Byte)
        For i = 1 To len
            l.Add(ReadByte)
        Next
        Return System.Text.Encoding.UTF8.GetChars(l.ToArray)
    End Function
    Public Function ReadVLQ(Optional ByVal Raw As Boolean = False) As Integer
        Dim l As New List(Of Byte)
        Dim b As Boolean = False
        Dim i = index
        Do Until b Or i = RawBytes.Count
            l.Add(ReadByte(Raw))
            If (New Binary(l.Last)).IsLastVLQByte Then b = True
        Loop
        If b = False Then Throw New IndexOutOfRangeException("No VLQ (end) found")
        Return VLQ.FromVLQ(l.ToArray)
    End Function
    Public Function ReadsVLQ(Optional ByVal Raw As Boolean = False) As Integer
        Dim l As New List(Of Byte)
        Dim b As Boolean = False
        Dim i = index
        Do Until b Or i = RawBytes.Count
            l.Add(ReadByte(Raw))
            If (New Binary(l.Last)).IsLastVLQByte Then b = True
        Loop
        If b = False Then Throw New IndexOutOfRangeException("No VLQ (end) found")
        Return VLQ.FromsVLQ(l.ToArray)
    End Function
End Class
