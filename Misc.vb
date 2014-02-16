Imports System.IO
Imports System.IO.Compression
Imports BasicUFL
Public Module Misc
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
    Public Function Hash(ByVal account As String, ByVal password As String, ByVal challenge As String, ByVal rounds As Integer) As String
        Dim salt = System.Text.Encoding.UTF8.GetBytes(account & challenge)
        Dim sha = Security.Cryptography.SHA256.Create()
        Dim hsh As Byte() = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
        For i = 0 To rounds
            sha.Initialize()
            sha.TransformBlock(hsh, 0, hsh.Length, Nothing, 0)
            sha.TransformFinalBlock(salt, 0, salt.Length)
            hsh = sha.Hash
        Next
        Return Convert.ToBase64String(hsh)
    End Function
    Public Enum ChatChannel
        Universe = 1
        World = 0
        Whisper = 2
        CommandResult = 3
        White = 4
    End Enum
End Module
