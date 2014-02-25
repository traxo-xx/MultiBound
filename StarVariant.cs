using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
public class StarVariant
{
	public object Value { get; set; }
	public VariantTypes Type { get; set; }

}
public enum VariantTypes
{
	_Null = 1,
	_Double = 2,
	_Boolean = 3,
	_VLQ = 4,
	_String = 5,
	_VariantArray = 6,
	_Dictionary = 7
}
