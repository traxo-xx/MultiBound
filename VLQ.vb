Public Class VLQ
    Public Property IsNotLast As Boolean
    Public Property IsNegative As Boolean
    Public Property Bytes As Byte()
End Class
Public Module VLQFunctions
    Private Function SplitIntervals(Of T)(ByVal Interval As Integer, ByVal Bytes As T()) As List(Of List(Of T))
        Dim i As Integer = 0
        Dim bb As New List(Of List(Of T))
        Dim b As New List(Of T)
        For i1 As Integer = 1 To Bytes.Length
            If i = Interval Then
                bb.Add(b)
                b.Clear()
                i = 0
            End If
            b.Add(Bytes(i1))
            i += 1
        Next
        bb.Add(b)
        Return bb
    End Function
    Private Function ByteToBoolean(ByVal BByte As Byte) As Boolean
        If BByte = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function NOTRWORKINGConvertFromVLQBytes(ByVal Bytes As Byte()) As List(Of VLQ)
        Dim b = SplitIntervals(8, Bytes)
        b(b.Count - 2).Add(b(b.Count - 1)(0))
        b.RemoveAt(b.Count - 1)
        Dim v As New List(Of VLQ)
        For i As Integer = 0 To b.Count - 1
            Dim vv As New VLQ
            If b(i)(0) = 0 Then
                vv.IsNotLast = False
                vv.IsNegative = ByteToBoolean(b(i)(8))
                vv.Bytes = BasicUFL.ArrayConversion.FromList(b(i).GetRange(1, 7))
                v.Add(vv)
            Else
                vv.IsNotLast = True
                vv.IsNegative = Nothing
                vv.Bytes = BasicUFL.ArrayConversion.FromList(b(i).GetRange(1, 7))
            End If
        Next
        Return v
    End Function

    Public Function NormalToVLQBytes()

    End Function

End Module
