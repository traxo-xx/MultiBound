using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class World
{
	public byte[] RawBytes { get; set; }
	public World(byte[] Bytes)
	{
		RawBytes = Bytes;
		//No point writing anything else here for now
	}
}
