Public Module VLQ
    Private Function concat(ByVal str As String()) As String
        Dim s As String = ""
        For Each st As String In str
            s &= st
        Next
        Return s
    End Function
    Public Function ToVLQ(ByVal Value As Integer) As Byte()
        Dim b = New Binary().NewInteger(Value)
        Dim l As New List(Of String)
        Dim bb = b.Split7
        For i = 0 To bb.Count - 1
            If i < bb.Count - 1 Then
                l.Add("1" & bb(i))
            Else
                l.Add("0" & bb(i))
            End If
        Next
        b = New Binary().NewString(concat(l.ToArray))
        Return b.ToBytes
    End Function
    Public Function FromVLQ(ByVal Value As Byte()) As Integer
        Dim l As New List(Of String)
        For Each i As Byte In Value
            l.Add(Convert.ToString(i, 2))
        Next
        Dim b = New Binary().NewString(concat(l.ToArray))
        l.Clear()
        For Each bb As String In b.Split8
            l.Add(bb.Substring(1, 7))
        Next
        Return Convert.ToInt32(concat(l.ToArray), 2)
    End Function
    Public Function FromsVLQ(ByVal Value As Byte()) As Integer
        Dim l As New List(Of String)
        For Each i As Byte In Value
            l.Add(Convert.ToString(i, 2))
        Next
        Dim b = New Binary().NewString(concat(l.ToArray))
        l.Clear()
        For Each bb As String In b.Split8
            l.Add(bb.Substring(1, 7))
        Next
        Return ((Convert.ToInt32(concat(l.ToArray), 2)) + 0) \ 2 '1) * 2
    End Function
    Public Function TosVLQ(ByVal Value As Integer) As Byte()
        Value *= 2
        'Value -= 1
        Dim b = New Binary().NewInteger(Value)
        Dim l As New List(Of String)
        Dim bb = b.Split7
        For i = 0 To bb.Count - 1
            If i < bb.Count - 1 Then
                l.Add("1" & bb(i))
            Else
                l.Add("0" & bb(i))
            End If
        Next
        b = New Binary().NewString(concat(l.ToArray))
        Return b.ToBytes
    End Function
End Module
Public Class Binary
    Public Property Bits As String
    Function NewLong(ByVal Value As Long) As Binary
        Bits = Convert.ToString(Value, 2)
        Return Me
    End Function
    Function NewInteger(ByVal Value As Integer) As Binary
        Bits = Convert.ToString(Value, 2)
        Return Me
    End Function
    Function NewUInteger(ByVal Value As UInteger) As Binary
        Bits = Convert.ToString(Value, 2).PadLeft(32, "0"c)
        Return Me
    End Function
    Function NewByte(ByVal Value As Byte) As Binary
        Bits = Convert.ToString(Value, 2).PadLeft(8, "0"c)
        Return Me
    End Function
    Function NewSByte(ByVal Value As SByte) As Binary
        Bits = Convert.ToString(Value, 2)
        Return Me
    End Function
    Function NewString(ByVal Value As String) As Binary
        Bits = Value
        Return Me
    End Function
    Public Function ToBytes() As Byte()
        Dim l As New List(Of Byte)
        For Each s As String In Split8()
            If s = "00000000" Then Continue For
            l.Add(Convert.ToByte(s, 2))
        Next
        Return l.ToArray
    End Function
    Public Function Split7() As String()
        Dim pad As Integer = 7 - (Bits.Count Mod 7)
        Dim l As New List(Of String)
        Dim str As String = Bits.PadLeft(Bits.Length + pad, "0"c)
        For i = 0 To str.Count - 1 Step 7
            l.Add(str.Substring(i, 7))
        Next
        Return l.ToArray
    End Function
    Public Function Split8() As String()
        Dim pad As Integer = 8 - (Bits.Count Mod 8)
        Dim l As New List(Of String)
        Dim str As String = Bits.PadLeft(Bits.Length + pad, "0"c)
        For i = 0 To str.Count - 1 Step 8
            l.Add(str.Substring(i, 8))
        Next
        Return l.ToArray
    End Function
    Public Function IsLastVLQByte() As Boolean
        If Bits(0) = "0"c Then
            Return True
        Else
            Return False
        End If
    End Function
End Class
