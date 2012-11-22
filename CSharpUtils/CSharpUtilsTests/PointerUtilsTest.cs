﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSharpUtils;

namespace CSPspEmu.Tests
{
	[TestClass]
	unsafe public class PointerUtilsTest
	{
		[TestMethod]
		public void TestMemset()
		{
			var Data = new byte[131];
			PointerUtils.Memset(Data, 0x3E, Data.Length);

			CollectionAssert.AreEqual(
				((byte)0x3E).Repeat(Data.Length),
				Data
			);
		}

		[TestMethod]
		public void TestMemcpy()
		{
			int SizeStart = 17;
			int SizeMiddle = 77;
			int SizeEnd = 17;
			var Dst = new byte[SizeStart + SizeMiddle + SizeEnd];
			fixed (byte* DstPtr = &Dst[SizeStart])
			{
				PointerUtils.Memcpy(DstPtr, ((byte)0x1D).Repeat(SizeMiddle).ToArray(), SizeMiddle);
			}

			var Expected = ((byte)0x00).Repeat(SizeStart).Concat(((byte)0x1D).Repeat(SizeMiddle)).Concat(((byte)0x00).Repeat(SizeEnd)).ToArray();

			//Console.WriteLine(BitConverter.ToString(Dst));
			//Console.WriteLine(BitConverter.ToString(Expected));

			CollectionAssert.AreEqual(
				Expected,
				Dst
			);
		}

		[TestMethod]
		public void TestMemcpy4()
		{
			int TotalSize = 12;
			var Source = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			var Dest = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

			foreach (var Set64 in new[] { false, true })
			{
				PointerUtils.Is64 = Set64;

				fixed (byte* SourcePtr = Source)
				fixed (byte* DestPtr = Dest)
				{
					for (int count = 0; count < TotalSize; count++)
					{
						for (int m = 0; m < TotalSize; m++) Dest[m] = 0;

						PointerUtils.Memcpy(DestPtr, SourcePtr, count);

						for (int m = 0; m < TotalSize; m++)
						{
							Assert.AreEqual((m < count) ? m : 0, Dest[m]);
						}
					}
				}
			}
		}

		[TestMethod]
		public void TestMemset4()
		{
			int TotalSize = 65;
			var Dest = new byte[TotalSize];

			foreach (var Set64 in new[] { false, true })
			{
				PointerUtils.Is64 = Set64;

				fixed (byte* DestPtr = Dest)
				{
					for (int count = 0; count < TotalSize; count++)
					{
						for (int m = 0; m < TotalSize; m++) Dest[m] = 0;

						PointerUtils.Memset(DestPtr, 1, count);

						for (int m = 0; m < TotalSize; m++)
						{
							Assert.AreEqual((m < count) ? 1 : 0, Dest[m]);
						}
					}
				}
			}
		}

	}
}
