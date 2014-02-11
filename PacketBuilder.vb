Public Class PacketBuilder
    Public Property Bytes As New List(Of Byte)
    Public Sub Write(ByVal Value As Byte)
        Bytes.Add(Value)
    End Sub
    Public Sub Write(ByVal Values As Byte())
        For Each v As Byte In Values
            Write(v)
        Next
    End Sub
    Public Sub Write(ByVal Value As SByte)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As Short)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As Integer)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As Long)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As UShort)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As UInteger)
        Write(Reverse(BitConverter.GetBytes(Value)))
    End Sub
    Public Sub Write(ByVal Value As ULong)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As Boolean)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Text As String)
        If Text.Length = 0 Or Text = "" Then Bytes.Add(0) : Exit Sub
        Dim aarray = System.Text.Encoding.UTF8.GetBytes(Text)
        Dim barray = {CByte(Text.Length)}
        Bytes.AddRange(Combine({barray, aarray}))
    End Sub
    Private Function Reverse(ByVal Bytes As Byte()) As Byte()
        Dim l As List(Of Byte) = BasicUFL.ToList(Bytes)
        l.Reverse()
        Return l.ToArray
    End Function

    Public Function GetBytes() As Byte()
        Return Bytes.ToArray
    End Function
End Class
