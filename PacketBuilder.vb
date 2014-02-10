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
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Value As ULong)
        Write(BitConverter.GetBytes(Value))
    End Sub
    Public Sub Write(ByVal Text As String)
        If Text.Length = 0 Then Bytes.AddRange({1, 0})
        Dim aarray = System.Text.Encoding.UTF8.GetBytes(Text)
        Dim barray = {CByte(Text.Length)}
        Bytes.AddRange(Combine({aarray, barray}))
    End Sub

    Public Function GetBytes() As Byte()
        Return Bytes.ToArray
    End Function
End Class
