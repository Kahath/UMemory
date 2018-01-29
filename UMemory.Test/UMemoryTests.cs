using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using UMemory.Unmanaged.Enums;

namespace UMemory.Test
{
	[TestClass]
	public class UMemoryTests
	{
		[TestMethod]
		public void TestSignedPositions()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			stream.Write((sbyte)1);
			Assert.IsTrue(stream.Position == 1);

			stream.Write((short)45);
			Assert.IsTrue(stream.Position == 3);

			stream.Write(54);
			Assert.IsTrue(stream.Position == 7);

			stream.Write((long)4893);
			Assert.IsTrue(stream.Position == 15);
		}

		[TestMethod]
		public void TestUnsignedositions()
		{
			TestStream stream = new TestStream(17, EndiannessType.LittleEndian);

			stream.Write((byte)1);
			Assert.IsTrue(stream.Position == 1);

			stream.Write((char)12);
			Assert.IsTrue(stream.Position == 3);

			stream.Write((ushort)45);
			Assert.IsTrue(stream.Position == 5);

			stream.Write((uint)54);
			Assert.IsTrue(stream.Position == 9);

			stream.Write((ulong)4893);
			Assert.IsTrue(stream.Position == 17);
		}

		[TestMethod]
		public void TestFloatingPointPositions()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);

			stream.Write(435.62F);
			Assert.IsTrue(stream.Position == 4);

			stream.Write(4893.4523432D);
			Assert.IsTrue(stream.Position == 12);
		}

		[TestMethod]
		public void TestStringPositions()
		{
			TestStream stream = new TestStream(34, EndiannessType.LittleEndian);

			stream.Write("Test string position");
			Assert.IsTrue(stream.Position == 21);

			stream.WriteCString("Test CString");
			Assert.IsTrue(stream.Position == 34);
		}

		[TestMethod]
		public void TestArrayPositions()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			byte[] testArray = new byte[10];
			byte[] testCountArray = new byte[10];

			stream.Write(testArray);
			Assert.IsTrue(stream.Position == 11);

			stream.Write(testCountArray, 4);
			Assert.IsTrue(stream.Position == 15);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestStringWriteIndexOutOfRange()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);

			stream.Write("This string is longer than 12 chars");
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestArrayWriteIndexOutOfRange()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);
			byte[] testLongerArray = new byte[12];

			stream.Write(testLongerArray);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestWriteIndexOutOfRange()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);

			stream.Write((sbyte)1);
			stream.Write((short)45);
			stream.Write(54);
			stream.Write((long)4893);
		}

		[TestMethod]
		public void TestReadSigned()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			stream.Write((sbyte)1);
			stream.Write((short)45);
			stream.Write(54);
			stream.Write((long)4893);

			stream.Seek(0);

			Assert.IsTrue(stream.ReadSByte() == 1);
			Assert.IsTrue(stream.ReadInt16() == 45);
			Assert.IsTrue(stream.ReadInt32() == 54);
			Assert.IsTrue(stream.ReadInt64() == 4893);
		}

		[TestMethod]
		public void TestReadUnsigned()
		{
			TestStream stream = new TestStream(17, EndiannessType.LittleEndian);

			stream.Write((byte)1);
			stream.Write((char)12);
			stream.Write((ushort)45);
			stream.Write((uint)54);
			stream.Write((ulong)4893);

			stream.Seek(0);

			Assert.IsTrue(stream.ReadByte() == 1);
			Assert.IsTrue(stream.ReadChar() == 12);
			Assert.IsTrue(stream.ReadUInt16() == 45);
			Assert.IsTrue(stream.ReadUInt32() == 54);
			Assert.IsTrue(stream.ReadUInt64() == 4893);
		}

		[TestMethod]
		public void TestReadFloatingPoint()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);

			stream.Write(435.62F);
			stream.Write(4893.4523432D);

			stream.Seek(0);

			Assert.IsTrue(stream.ReadFloat() == 435.62F);
			Assert.IsTrue(stream.ReadDouble() == 4893.4523432D);
		}

		[TestMethod]
		public void TestReadString()
		{
			TestStream stream = new TestStream(34, EndiannessType.LittleEndian);

			stream.Write("Test string position");
			stream.WriteCString("Test CString");

			stream.Seek(0);

			Assert.IsTrue(stream.ReadString() == "Test string position");
			Assert.IsTrue(stream.ReadCString() == "Test CString");

		}

		[TestMethod]
		public void TestReadArray()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			byte[] testArray = new byte[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			byte[] testCountArray = new byte[10] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

			stream.Write(testArray);
			stream.Write(testCountArray, 4);

			stream.Seek(0);

			byte[] readArray = stream.ReadBytes();
			for (int i = 0; i < 10; i++)
				Assert.IsTrue(readArray[i] == (byte)(i + 1));

			byte[] readCountArray = stream.ReadBytes(4);
			Assert.IsTrue(readCountArray[0] == 10);
			Assert.IsTrue(readCountArray[1] == 9);
			Assert.IsTrue(readCountArray[2] == 8);
			Assert.IsTrue(readCountArray[3] == 7);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestStringReadndexOutOfRange()
		{
			TestStream stream = new TestStream(28, EndiannessType.LittleEndian);

			stream.Write("This string is 28 chars long");
			Assert.IsTrue(stream.Position == 29);

			stream.Seek(1);
			stream.ReadString();
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestArrayReadIndexOutOfRange()
		{
			TestStream stream = new TestStream(12, EndiannessType.LittleEndian);
			byte[] testLongerArray = new byte[11];

			stream.Write(testLongerArray);
			Assert.IsTrue(stream.Position == 12);

			stream.Seek(2);
			stream.ReadBytes(11);
		}

		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void TestReadIndexOutOfRange()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			stream.Write((sbyte)1);
			stream.Write((short)45);
			stream.Write(54);
			stream.Write((long)4893);
			Assert.IsTrue(stream.Position == 15);

			stream.Seek(1);
			stream.ReadSByte();
			stream.ReadInt16();
			stream.ReadInt32();
			stream.ReadInt64();
		}

		[TestMethod]
		public void TestGeneralReadWrite()
		{
			const int iterations = 500;
			const int iterationBytes = 60;
			const int allocateBytes = iterations * iterationBytes;
			byte[] testArray = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

			TestStream testStream = new TestStream(allocateBytes, EndiannessType.LittleEndian);

			for (int i = 0; i < iterations; i++)
			{
				int add = iterationBytes * i;

				testStream.Write(true);
				Assert.IsTrue(testStream.Position == 1 + add);

				testStream.Write((short)10);
				Assert.IsTrue(testStream.Position == 3 + add);

				testStream.Write(25);
				Assert.IsTrue(testStream.Position == 7 + add);

				testStream.Write("Sample string");
				Assert.IsTrue(testStream.Position == 21 + add);

				testStream.Write(2598D);
				Assert.IsTrue(testStream.Position == 29 + add);

				testStream.Write(2598.34253752D);
				Assert.IsTrue(testStream.Position == 37 + add);

				testStream.Write(2598.435F);
				Assert.IsTrue(testStream.Position == 41 + add);

				testStream.Write(testArray);
				Assert.IsTrue(testStream.Position == 50 + add);

				testStream.WriteCString("CString10");
				Assert.IsTrue(testStream.Position == 60 + add);
			}
			Assert.IsTrue(testStream.Position == iterationBytes * iterations);

			testStream.Seek(0);
			Assert.IsTrue(testStream.Position == 0);

			for (int i = 0; i < iterations; i++)
			{
				int add = iterationBytes * i;

				Assert.IsTrue(testStream.ReadBoolean() == true);
				Assert.IsTrue(testStream.Position == 1 + add);

				Assert.IsTrue(testStream.ReadInt16() == 10);
				Assert.IsTrue(testStream.Position == 3 + add);

				Assert.IsTrue(testStream.ReadInt32() == 25);
				Assert.IsTrue(testStream.Position == 7 + add);

				Assert.IsTrue(testStream.ReadString() == "Sample string");
				Assert.IsTrue(testStream.Position == 21 + add);

				Assert.IsTrue(testStream.ReadDouble() == 2598D);
				Assert.IsTrue(testStream.Position == 29 + add);

				Assert.IsTrue(testStream.ReadDouble() == 2598.34253752D);
				Assert.IsTrue(testStream.Position == 37 + add);

				Assert.IsTrue(testStream.ReadFloat() == 2598.435F);
				Assert.IsTrue(testStream.Position == 41 + add);

				Assert.IsTrue(testStream.ReadBytes().Length == 8);
				Assert.IsTrue(testStream.Position == 50 + add);

				Assert.IsTrue(testStream.ReadCString() == "CString10");
				Assert.IsTrue(testStream.Position == 60 + add);
			}

			Assert.IsTrue(testStream.Position == iterationBytes * iterations);
		}

#if !DEBUG
		[TestMethod]
		public void TestSpeed()
		{
			const int iterations = 500;
			const int allocateBytes = iterations * 50;
			byte[] testArray = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

			long msWriteElapsedTicks = 0;
			long msReadElapsedTicks = 0;

			long umWriteElapsedTicks = 0;
			long umReadElapsedTicks = 0;

			Stopwatch sw = new Stopwatch();

			MemoryStream ms = new MemoryStream(allocateBytes);
			BinaryWriter writer = new BinaryWriter(ms);
			BinaryReader reader = new BinaryReader(ms);

			ms.Position = 0;
			sw.Restart();
			for (int j = 0; j < iterations; j++)
			{
				writer.Write(true);
				writer.Write((short)10);
				writer.Write(25);
				writer.Write("Sample string");
				writer.Write(2598D);
				writer.Write(2598.34253752D);
				writer.Write(2598.435F);
				writer.Write(testArray);
			}
			sw.Stop();

			msWriteElapsedTicks = sw.ElapsedTicks;

			ms.Position = 0;
			sw.Restart();
			for (int j = 0; j < iterations; j++)
			{
				reader.ReadBoolean();
				reader.ReadInt16();
				reader.ReadInt32();
				reader.ReadString();
				reader.ReadDouble();
				reader.ReadDouble();
				reader.ReadUInt32();
				reader.ReadBytes(testArray.Length);
			}
			sw.Stop();

			msReadElapsedTicks = sw.ElapsedTicks;

			TestStream testStream = new TestStream(allocateBytes, EndiannessType.LittleEndian);

			sw.Restart();
			for (int j = 0; j < iterations; j++)
			{
				testStream.Write(true);
				testStream.Write((short)10);
				testStream.Write(25);
				testStream.Write("Sample string");
				testStream.Write(2598D);
				testStream.Write(2598.34253752D);
				testStream.Write(2598.435F);
				testStream.Write(testArray);
			}
			sw.Stop();

			umWriteElapsedTicks = sw.ElapsedTicks;

			testStream.Seek(0);
			sw.Restart();
			for (int j = 0; j < iterations; j++)
			{
				testStream.ReadBoolean();
				testStream.ReadInt16();
				testStream.ReadInt32();
				testStream.ReadString();
				testStream.ReadDouble();
				testStream.ReadDouble();
				testStream.ReadFloat();
				testStream.ReadBytes();
			}
			sw.Stop();

			umReadElapsedTicks = sw.ElapsedTicks;

			Assert.IsTrue(umWriteElapsedTicks < msWriteElapsedTicks);
			Assert.IsTrue(umReadElapsedTicks < msReadElapsedTicks);
		}
#endif
	}
}
