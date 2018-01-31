/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using UMemory.Unmanaged.Stream.Base;
using UMemory.Unmanaged.Stream.Core;

namespace UMemory.Test
{
	public class TestSample : IUMemoryWrite, IUMemoryRead
	{
		#region Fields

		byte _sampleByte = 1;
		short _sampleShort = 2;
		int _sampleInt = 5;
		string _sampleString = "Sample string";

		#endregion

		#region Properties

		public byte SampleByte
		{
			get { return _sampleByte; }
		}

		public short SampleShort
		{
			get { return _sampleShort; }
		}

		public int SampleInt
		{
			get { return _sampleInt; }
		}

		public string SampleString
		{
			get { return _sampleString; }
		}

		#endregion

		#region Methods

		public void Read(UMemoryStream stream)
		{
			_sampleByte = stream.ReadUInt8();
			_sampleShort = stream.ReadInt16();
			_sampleInt = stream.ReadInt32();
			_sampleString = stream.ReadString();
		}

		public void Write(UMemoryStream stream)
		{
			stream.WriteUInt8(_sampleByte);
			stream.WriteInt16(_sampleShort);
			stream.WriteInt32(_sampleInt);
			stream.WriteString(_sampleString);
		}

		public void ChangeData()
		{
			_sampleByte = 100;
			_sampleShort = 100;
			_sampleInt = 100;
			_sampleString = "Wrong string";
		}

		#endregion
	}
}
