Module Main
    Public Property Password As String = ""
    Public Property Salt As String = "5uAciRZkmwKUkek3krU+s2LTvPHE6v2P"
    Public Property Rounds As UInteger = 5000
    Sub Main()
        'Good evening everybody, and welcome to "Whose Packet is it Anyway?".
        'On tonight's show, it's the first one I ever got, the ProtocolVersion Packet.
        'The rambler that goes on and one and doesn't stop, the ClientConnect Packet.
        'And last but not least, the one you love to say goodbye to, the DisconnectResponse Packet.
        'And I'm ROFLCopter64bit, your host, come on down, let's have some fun!
        'Hello, good evening, and welcome to "Whose Packet is it Anyway?", the show where all the packets are made up and the HandshakeResponses don't matter.
        'That's right, I don't even check to see if the passwords are correct!
        'This episode is all about networking compression, in other words, zlib is the name and decompression is the game.
        Console.Title = "MultiBound"
        Dim s As New StarServer(21025)
        Do While True
            Console.Read()
        Loop
    End Sub

End Module
