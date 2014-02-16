Public Class World
    Public Property RawBytes As Byte()
    Sub New(ByVal Bytes() As Byte)
        RawBytes = Bytes
        'No point writing anything else here for now
    End Sub
End Class
