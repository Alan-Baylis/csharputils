﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpPspEmulator.Utils
{
    static public class BitExtensions
    {
        static public uint Mask(int Value)
        {
            return (uint)((1 << Value) - 1);
        }

        static public uint ExtractUnsigned(this uint Value, int Offset, int Length)
        {
            return (uint)(Value >> Offset) & Mask(Length);
        }

        static public uint Insert(this uint Value, int Offset, int Length, int ValueToInsert)
        {
            Value &= ~(uint)(Mask(Length) << Offset);
            Value |= (uint)(ValueToInsert << Offset);
            return Value;
        }
    }
}