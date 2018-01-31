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
		#region Basic

		[TestCategory("Basic")]
		[TestMethod]
		public void NewUMemoryStreamTest()
		{
			TestStream stream = new TestStream(0, EndiannessType.LittleEndian);
			Assert.AreEqual(0, stream.Capacity);
			Assert.AreEqual(0, stream.Position);
			Assert.AreEqual(EndiannessType.LittleEndian, stream.EndianessType);
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void AllocTest()
		{
			TestStream stream = new TestStream(0, EndiannessType.LittleEndian);
			stream.Alloc(15);

			Assert.AreEqual(15, stream.Capacity);
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void ReallocTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);
			stream.Realloc(2);

			Assert.AreEqual(2, stream.Capacity);
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void FreeTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);
			stream.Free();

			Assert.AreEqual(0, stream.Capacity);
		}

		[TestCategory("Basic")]
		[TestMethod]
		[ExpectedException(typeof(IndexOutOfRangeException))]
		public void IndexOutOfRangeTest()
		{
			TestStream stream = new TestStream(0, EndiannessType.LittleEndian);
			stream.Seek(1);
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void SeekTest()
		{
			TestStream stream = new TestStream(1, EndiannessType.LittleEndian);

			stream.Seek(1);
			Assert.AreEqual(1, stream.Position);
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void SeekValueTest()
		{
			TestStream stream = new TestStream(5, EndiannessType.LittleEndian);
			stream.WriteUInt8(1);
			stream.WriteUInt8(2);
			stream.WriteUInt8(3);
			stream.WriteUInt8(4);
			stream.WriteUInt8(5);

			stream.Seek(0);
			Assert.AreEqual(2, stream.SeekValue(3));
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void CanSeekTest()
		{
			TestStream stream = new TestStream(5, EndiannessType.LittleEndian);

			Assert.IsTrue(stream.CanSeek(3));
			Assert.IsTrue(stream.CanSeek(5));
			Assert.IsTrue(!stream.CanSeek(6));
		}

		[TestCategory("Basic")]
		[TestMethod]
		public void SkipTest()
		{
			TestStream stream = new TestStream(5, EndiannessType.LittleEndian);

			stream.Skip(3);
			Assert.AreEqual(3, stream.Position);

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.Skip(3));
		}

		#endregion

		#region I/O

		[TestCategory("IO")]
		[TestMethod]
		public void BoolTest()
		{
			TestStream stream = new TestStream(2, EndiannessType.LittleEndian);
			stream.WriteBool(true);

			Assert.AreEqual(1, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(true, stream.ReadBool());
			Assert.AreEqual(false, stream.ReadBool());
			Assert.AreEqual(2, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void Int8Test()
		{
			TestStream stream = new TestStream(1, EndiannessType.LittleEndian);
			stream.WriteInt8(1);

			Assert.AreEqual(1, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt8());
			Assert.AreEqual(1, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void UInt8Test()
		{
			TestStream stream = new TestStream(1, EndiannessType.LittleEndian);
			stream.WriteUInt8(1);

			Assert.AreEqual(1, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadUInt8());
			Assert.AreEqual(1, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void Int16LittleEndianTest()
		{
			TestStream stream = new TestStream(2, EndiannessType.LittleEndian);
			stream.WriteInt16(1);

			Assert.AreEqual(2, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt16());
			Assert.AreEqual(2, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void Int16BigEndianTest()
		{
			TestStream stream = new TestStream(2, EndiannessType.BigEndian);
			stream.WriteInt16(1);

			Assert.AreEqual(2, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt16());
			Assert.AreEqual(2, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void UInt16LittleEndianTest()
		{
			TestStream stream = new TestStream(2, EndiannessType.LittleEndian);
			stream.WriteUInt16(1);

			Assert.AreEqual(2, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadUInt16());
			Assert.AreEqual(2, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void UInt16BigEndianTest()
		{
			TestStream stream = new TestStream(2, EndiannessType.BigEndian);
			stream.WriteUInt16(1);

			Assert.AreEqual(2, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadUInt16());
			Assert.AreEqual(2, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void Int32LittleEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.LittleEndian);
			stream.WriteInt32(1);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt32());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void Int32BigEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.BigEndian);
			stream.WriteInt32(1);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt32());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void UInt32LittleEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.LittleEndian);
			stream.WriteUInt32(1);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual((uint)1, stream.ReadUInt32());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void UInt32BigEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.BigEndian);
			stream.WriteUInt32(1);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual((uint)1, stream.ReadUInt32());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void Int64LittleEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.LittleEndian);
			stream.WriteInt64(1);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt64());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void Int64BigEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.BigEndian);
			stream.WriteInt64(1);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1, stream.ReadInt64());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void UInt64LittleEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.LittleEndian);
			stream.WriteUInt64(1);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual((ulong)1, stream.ReadUInt64());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void UInt64BigEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.BigEndian);
			stream.WriteUInt64(1);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual((ulong)1, stream.ReadUInt64());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void FloatLittleEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.LittleEndian);
			stream.WriteFloat(1.435F);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1.435F, stream.ReadFloat());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void FloatBigEndianTest()
		{
			TestStream stream = new TestStream(4, EndiannessType.BigEndian);
			stream.WriteFloat(1.435F);

			Assert.AreEqual(4, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1.435F, stream.ReadFloat());
			Assert.AreEqual(4, stream.Position);
		}

		[TestCategory("IO"), TestCategory("LittleEndian")]
		[TestMethod]
		public void DoubleLittleEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.LittleEndian);
			stream.WriteDouble(1.4456346534D);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1.4456346534D, stream.ReadDouble());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO"), TestCategory("BigEndian")]
		[TestMethod]
		public void DoubleBigEndianTest()
		{
			TestStream stream = new TestStream(8, EndiannessType.BigEndian);
			stream.WriteDouble(1.4456346534D);

			Assert.AreEqual(8, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(1.4456346534D, stream.ReadDouble());
			Assert.AreEqual(8, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void StringTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			stream.WriteString("Sample string");
			Assert.AreEqual(14, stream.Position);

			stream.WriteString("");
			Assert.AreEqual(15, stream.Position);

			stream.Seek(0);

			Assert.AreEqual("Sample string", stream.ReadString());
			Assert.AreEqual(14, stream.Position);

			Assert.AreEqual("", stream.ReadString());
			Assert.AreEqual(15, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void StringExceptionTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.WriteString("Sample string longer than 15 chars"));

			stream.WriteUInt8(16);
			stream.Seek(0);

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.ReadString());
		}

		[TestCategory("IO")]
		[TestMethod]
		public void CStringTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			stream.WriteCString("Sample string");
			Assert.AreEqual(14, stream.Position);

			stream.WriteCString("");
			Assert.AreEqual(15, stream.Position);

			stream.Seek(0);

			Assert.AreEqual("Sample string", stream.ReadCString());
			Assert.AreEqual(14, stream.Position);

			Assert.AreEqual("", stream.ReadCString());
			Assert.AreEqual(15, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void CStringExceptionTest()
		{
			TestStream stream = new TestStream(15, EndiannessType.LittleEndian);

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.WriteCString("Sample string longer than 15 chars"));

			// There shouldn't be null termination in first 15 bytes.
			stream.WriteString("Sample string1");
			stream.Seek(0);

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.ReadCString());
		}

		[TestCategory("IO")]
		[TestMethod]
		public void BytesTest()
		{
			TestStream stream = new TestStream(18, EndiannessType.LittleEndian);
			byte[] sampleBytes = new byte[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

			stream.WriteBytes(sampleBytes);
			Assert.AreEqual(10, stream.Position);

			stream.WriteBytes(sampleBytes, 4);
			Assert.AreEqual(15, stream.Position);

			// Length is not written here
			stream.WriteBytes(sampleBytes, 7, 2);
			Assert.AreEqual(17, stream.Position);

			stream.Seek(0);

			Assert.AreEqual(9, stream.ReadBytes().Length);
			Assert.AreEqual(10, stream.Position);

			Assert.AreEqual(4, stream.ReadBytes().Length);
			Assert.AreEqual(15, stream.Position);

			Assert.AreEqual(2, stream.ReadBytes(2).Length);
			Assert.AreEqual(17, stream.Position);
		}

		[TestCategory("IO")]
		[TestMethod]
		public void BytesExceptionTest()
		{
			TestStream stream = new TestStream(10, EndiannessType.LittleEndian);
			byte[] sampleBytes = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.WriteBytes(sampleBytes));
			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.WriteBytes(sampleBytes, 10));
			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.WriteBytes(sampleBytes, 0, 11));

			stream.WriteBytes(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

			Assert.ThrowsException<IndexOutOfRangeException>(() => stream.ReadBytes(11));
		}

		[TestCategory("IO")]
		[TestMethod]
		public void CustomTypeTest()
		{
			TestStream stream = new TestStream(21, EndiannessType.LittleEndian);
			TestSample sample = new TestSample();
			stream.Write(sample);

			Assert.AreEqual(21, stream.Position);

			stream.Seek(0);
			sample.ChangeData();

			stream.Read(sample);

			Assert.AreEqual(1, sample.SampleByte);
			Assert.AreEqual(2, sample.SampleShort);
			Assert.AreEqual(5, sample.SampleInt);
			Assert.AreEqual("Sample string", sample.SampleString);

			Assert.AreEqual(21, stream.Position);
			stream.Seek(0);

			TestSample newSample = stream.Read<TestSample>();

			Assert.AreEqual(1, sample.SampleByte);
			Assert.AreEqual(2, sample.SampleShort);
			Assert.AreEqual(5, sample.SampleInt);
			Assert.AreEqual("Sample string", sample.SampleString);

			Assert.AreEqual(21, stream.Position);
		}

		#endregion

		#region Endianness

		[TestCategory("Endianness")]
		[TestMethod]
		public void EndiannessTest()
		{
			TestStream littleEndianStream = new TestStream(8, EndiannessType.LittleEndian);
			short sampleShort = 0x1234;
			int sampleInt = 0x12345678;
			long sampleLong = 0x1234567891234567;

			littleEndianStream.WriteInt16(sampleShort);
			littleEndianStream.Seek(0);

			Assert.AreEqual(0x34, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x12, littleEndianStream.ReadUInt8());

			littleEndianStream.Seek(0);
			littleEndianStream.WriteInt32(sampleInt);
			littleEndianStream.Seek(0);

			Assert.AreEqual(0x78, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x56, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x34, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x12, littleEndianStream.ReadUInt8());

			littleEndianStream.Seek(0);
			littleEndianStream.WriteInt64(sampleLong);
			littleEndianStream.Seek(0);

			Assert.AreEqual(0x67, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x45, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x23, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x91, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x78, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x56, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x34, littleEndianStream.ReadUInt8());
			Assert.AreEqual(0x12, littleEndianStream.ReadUInt8());

			TestStream bigEndianStream = new TestStream(8, EndiannessType.BigEndian);

			bigEndianStream.WriteInt16(sampleShort);
			bigEndianStream.Seek(0);

			Assert.AreEqual(0x12, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x34, bigEndianStream.ReadUInt8());

			bigEndianStream.Seek(0);
			bigEndianStream.WriteInt32(sampleInt);
			bigEndianStream.Seek(0);

			Assert.AreEqual(0x12, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x34, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x56, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x78, bigEndianStream.ReadUInt8());

			bigEndianStream.Seek(0);
			bigEndianStream.WriteInt64(sampleLong);
			bigEndianStream.Seek(0);

			Assert.AreEqual(0x12, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x34, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x56, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x78, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x91, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x23, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x45, bigEndianStream.ReadUInt8());
			Assert.AreEqual(0x67, bigEndianStream.ReadUInt8());
		}

		#endregion

		#region Custom

		[TestCategory("Custom")]
		[TestMethod]
		public void GeneralIOTest()
		{
			const int iterations = 500;
			const int iterationBytes = 60;
			const int allocateBytes = iterations * iterationBytes;
			byte[] testArray = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

			TestStream testStream = new TestStream(allocateBytes, EndiannessType.LittleEndian);

			for (int i = 0; i < iterations; i++)
			{
				int add = iterationBytes * i;

				testStream.WriteBool(true);
				Assert.IsTrue(testStream.Position == 1 + add);

				testStream.WriteInt16(10);
				Assert.IsTrue(testStream.Position == 3 + add);

				testStream.WriteInt32(25);
				Assert.IsTrue(testStream.Position == 7 + add);

				testStream.WriteString("Sample string");
				Assert.IsTrue(testStream.Position == 21 + add);

				testStream.WriteDouble(2598D);
				Assert.IsTrue(testStream.Position == 29 + add);

				testStream.WriteDouble(2598.34253752D);
				Assert.IsTrue(testStream.Position == 37 + add);

				testStream.WriteFloat(2598.435F);
				Assert.IsTrue(testStream.Position == 41 + add);

				testStream.WriteBytes(testArray);
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

				Assert.IsTrue(testStream.ReadBool() == true);
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
		[TestCategory("Custom")]
		[TestMethod]
		public void PerformanceTest()
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
				testStream.WriteBool(true);
				testStream.WriteInt16(10);
				testStream.WriteInt32(25);
				testStream.WriteString("Sample string");
				testStream.WriteDouble(2598D);
				testStream.WriteDouble(2598.34253752D);
				testStream.WriteFloat(2598.435F);
				testStream.WriteBytes(testArray);
			}
			sw.Stop();

			umWriteElapsedTicks = sw.ElapsedTicks;

			testStream.Seek(0);
			sw.Restart();
			for (int j = 0; j < iterations; j++)
			{
				testStream.ReadBool();
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

			Trace.WriteLine($"MS write { msWriteElapsedTicks}");
			Trace.WriteLine($"UM write { umWriteElapsedTicks}");

			Trace.WriteLine($"MS read { msReadElapsedTicks}");
			Trace.WriteLine($"UM read { umReadElapsedTicks}");

			Assert.IsTrue(umWriteElapsedTicks < msWriteElapsedTicks);
			Assert.IsTrue(umReadElapsedTicks < msReadElapsedTicks);
		}
#endif
		#endregion
	}
}
