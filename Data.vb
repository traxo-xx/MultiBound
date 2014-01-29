Public Module Data
    Public Function WriteString(ByVal Text As String) As Byte()
        If Text.Length = 0 Then Return {1, 0}
        Dim aarray = System.Text.Encoding.UTF8.GetBytes(Text)
        Dim barray = {CByte(Text.Length)}
        Return Combine({aarray, barray})
    End Function
End Module
