using Microsoft.VisualBasic;
using System;
using Ionic.Zlib;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
public static class Misc
{
	public static byte[] Combine(byte[][] arrays)
	{
		List<byte> ret = new List<byte>(); 
		foreach (byte[] data in arrays) {
            foreach (byte d in data) {
                ret.Add(d);
            }
		}
		return ret.ToArray();
	}
	public static byte[] TrimNull(byte[] Array)
	{
		for (int i = Array.Length - 1; i >= 0; i += -1) {
			if (Array[i] > 0) {
				return Misc.ToList(Array).GetRange(0, i + 1).ToArray();
			}
		}
		return null;
	}
	public static byte[] Deflate(byte[] Bytes)
	{
        return ZlibStream.UncompressBuffer(Bytes);
	}
	public static byte[] Inflate(byte[] Bytes)
	{
        return ZlibStream.CompressBuffer(Bytes);
	}
	public static string Hash(string account, string password, string challenge, int rounds)
	{
		dynamic salt = System.Text.Encoding.UTF8.GetBytes(account + challenge);
		dynamic sha = System.Security.Cryptography.SHA256.Create();
		byte[] hsh = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		for (int i = 0; i <= rounds; i++) {
			sha.Initialize();
			sha.TransformBlock(hsh, 0, hsh.Length, null, 0);
			sha.TransformFinalBlock(salt, 0, salt.Length);
			hsh = sha.Hash;
		}
		return Convert.ToBase64String(hsh);
	}
    public static List<byte> ToList(byte[] lel)
    {
        return new List<byte>(lel);
    }
}
public enum ChatChannel
{
	Universe = 1,
   	World = 0,
	Whisper = 2,
	CommandResult = 3,
	White = 4
}
