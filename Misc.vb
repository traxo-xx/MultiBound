Imports System.IO
Imports System.IO.Compression
Imports BasicUFL
Module Misc
    Public Function Combine(ByVal arrays As Byte()()) As Byte()
        Dim ret As Byte() = New Byte(arrays.Sum(Function(x) x.Length) - 1) {}
        Dim offset As Integer = 0
        For Each data As Byte() In arrays
            Buffer.BlockCopy(data, 0, ret, offset, data.Length)
            offset += data.Length
        Next
        Return ret
    End Function
    Public Function TrimNull(ByVal Array As Byte()) As Byte()
        For i As Integer = Array.Length - 1 To 0 Step -1
            If Array(i) > 0 Then
                Return ToList(Array).GetRange(0, i + 1).ToArray()
            End If
        Next
        Return Nothing
    End Function
    Public Function Deflate(ByVal Bytes As Byte()) As Byte()
        Dim m As New MemoryStream()
        Dim d As New DeflateStream(m, CompressionMode.Compress)
        d.Write(Bytes, 0, Bytes.Length)
        d.Flush()
        d.Close()
        Return m.ToArray
    End Function
    Public Function Inflate(ByVal Bytes As Byte()) As Byte()
        Dim m As New MemoryStream(Bytes)
        Dim d As New DeflateStream(m, CompressionMode.Decompress)
        d.Write(Bytes, 0, Bytes.Length)
        d.Flush()
        d.Close()
        Return m.ToArray
    End Function
End Module
