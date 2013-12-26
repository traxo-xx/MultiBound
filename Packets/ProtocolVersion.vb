﻿Public Class ProtocolVersionPacket
    Inherits Packet
    Public Const OPCode As Byte = &H1
    Public Const Length As Byte = &H8
    Public Const Version As UInt32 = &H274

    Public Overrides Function GetByteArray() As Byte()
        Dim v As Byte() = {OPCode, Length}
        Dim a As Byte() = BitConverter.GetBytes(Version)
        Array.Reverse(a)
        Dim val As Byte()() = {v, a}
        Return Misc.Combine(val)
    End Function

    Public Overrides Sub LoadByteArray(ByteArray() As Byte)
    End Sub
End Class
