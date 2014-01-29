Module Main

    Sub Main()
        Console.Title = "MultiBound"
        Dim s As New StarServer(21025)
        Do
            System.Threading.Thread.Sleep(100)
        Loop
    End Sub

End Module
