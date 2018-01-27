/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using System;
using System.Security;
using System.Text;
using UMemory.Unmanaged.Enums;
using UMemory.Unmanaged.Stream.Base;

namespace UMemory.Unmanaged.Stream.Core
{
	[SecurityCritical]
	public abstract unsafe class UMemoryStream : UStream
	{
		#region Fields

		private int _adjustedPosition;
		private Encoding _encoding;

		#endregion

		#region Properties

		/// <summary>
		/// Adjusted position for copy.
		/// </summary>
		private int AdjustedPosition
		{
			get { return _adjustedPosition; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="UMemoryStream"/> type.
		/// </summary>
		/// <param name="length">Length of the underlying array.</param>
		public UMemoryStream(int length, EndiannessType endiannessType)
			: base(length, endiannessType)
		{
			_encoding = Encoding.UTF8;
		}

		~UMemoryStream()
		{
		}

		#endregion

		#region Methods

		#region Write

		/// <summary>
		/// Writes <see cref="IUMemoryWrite"/> instance to stream.
		/// </summary>
		/// <typeparam name="T">Type of instance to write.</typeparam>
		/// <param name="data">Instance to write.</param>
		public void Write<T>(T data)
			where T : IUMemoryWrite
		{
			if (data == null)
				throw new NullReferenceException("data");

			data.Write(this);
		}

		/// <summary>
		/// Writes boolean value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Boolean value to write.</param>
		public void Write(bool data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes byte value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Byte value to write.</param>
		public void Write(byte data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes char value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Char value to write.</param>
		public void Write(char data)
		{
			Write((short)data);
		}

		/// <summary>
		/// Writes unsigned short value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned short value to write.</param>
		public void Write(ushort data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes unsigned int value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned int value to write.</param>
		public void Write(uint data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes unsigned long value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Unsigned long value to write.</param>
		public void Write(ulong data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes short value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Short value to write.</param>
		public void Write(short data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes int value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Int value to write.</param>
		public void Write(int data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes long value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Long value to write.</param>
		public void Write(long data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes float value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Float value to write.</param>
		public void Write(float data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes double value to current stream position.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Double value to write.</param>
		public void Write(double data)
		{
			WriteOnPtr(PositionPtr, data);
		}

		/// <summary>
		/// Writes string value to current stream position with prefixed length as 7bit encoded int.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">string value to write.</param>
		public void Write(string data)
		{
			fixed (char* chrPtr = data)
			{
				int length = _encoding.GetByteCount(data);

				if (!CanSeek(length + 1))
					throw new IndexOutOfRangeException();

				Write7BitEncodedInt(length);

				_encoding.GetBytes(chrPtr, data.Length, PositionPtr, length);

				Position += length;
			}
		}

		/// <summary>
		/// Writes byte array value to current stream position with prefixed length as 7bit encoded int.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Byte array value to write.</param>
		public void Write(byte[] data)
		{
			int length = data.Length;
			Write7BitEncodedInt(length);

			CopyFrom(data, 0, Position, (uint)length);
			Position += length;
		}

		/// <summary>
		/// Writes 7bit encoded int to stream.
		/// Taken from https://github.com/Microsoft/referencesource/blob/4fe4349175f4c5091d972a7e56ea12012f1e7170/mscorlib/system/io/binarywriter.cs
		/// </summary>
		/// <param name="value"></param>
		private void Write7BitEncodedInt(int value)
		{
			// Write out an int 7 bits at a time.  The high bit of the byte,
			// when on, tells reader to continue reading more bytes.
			uint v = (uint)value;   // support negative numbers
			while (v >= 0x80)
			{
				Write((byte)(v | 0x80));
				v >>= 7;
			}
			Write((byte)v);
		}

		#endregion

		#region Read

		/// <summary>
		/// Reads <see cref="IUMemoryRead" /> instance from stream.
		/// If instance is null, new object is created.
		/// </summary>
		/// <typeparam name="T">Type of instance to read.</typeparam>
		/// <param name="value">instance to read.</param>
		/// <returns>Read instance from stream.</returns>
		public T Read<T>(T value = default(T))
			where T : IUMemoryRead, new()
		{
			T retVal = value;

			if (retVal == null)
				retVal = new T();

			retVal.Read(this);

			return retVal;
		}

		/// <summary>
		/// Reads boolean value on current stream position.
		/// </summary>
		/// <returns>Read boolean value.</returns>
		public bool ReadBoolean()
		{
			return ReadBooleanOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads byte value on current stream position.
		/// </summary>
		/// <returns>Read byte value.</returns>
		public byte ReadByte()
		{
			return ReadByteOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads signed byte value on current stream position.
		/// </summary>
		/// <returns>Read signed byte value.</returns>
		public sbyte ReadSByte()
		{
			return ReadSByteOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads char value on current stream position.
		/// </summary>
		/// <returns>Read char value.</returns>
		public char ReadChar()
		{
			return ReadCharOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads unsigned short value on current stream position.
		/// </summary>
		/// <returns>Read unsigned short value.</returns>
		public ushort ReadUInt16()
		{
			return ReadUInt16OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads short value on current stream position.
		/// </summary>
		/// <returns>Read short value.</returns>
		public short ReadInt16()
		{
			return ReadInt16OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads unsigned int value on current stream position.
		/// </summary>
		/// <returns>Read unsigned int value.</returns>
		public uint ReadUInt32()
		{
			return ReadUInt32OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads int value on current stream position.
		/// </summary>
		/// <returns>Read int value.</returns>
		public int ReadInt32()
		{
			return ReadInt32OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads unsigned long value on current stream position.
		/// </summary>
		/// <returns>Read unsigned long value.</returns>
		public ulong ReadUInt64()
		{
			return ReadUInt64OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads long value on current stream position.
		/// </summary>
		/// <returns>Read long value.</returns>
		public long ReadInt64()
		{
			return ReadInt64OnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads float value on current stream position.
		/// </summary>
		/// <returns>Read long value.</returns>
		public float ReadFloat()
		{
			return ReadFloatOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads double value on current stream position.
		/// </summary>
		/// <returns>Read double value.</returns>
		public double ReadDouble()
		{
			return ReadDoubleOnPtr(PositionPtr);
		}

		/// <summary>
		/// Reads string value on current stream position.
		/// String length is prefixed with 7bit encoded int.
		/// </summary>
		/// <returns>Read string value.</returns>
		public string ReadString()
		{
			int length = Read7BitEncodedInt();

			if (!CanSeek(length))
				throw new ArgumentOutOfRangeException();

			string retVal = _encoding.GetString(PositionPtr, length);
			Position += length;

			return retVal;
		}

		/// <summary>
		/// Reads byte array value on current stream position.
		/// Byte array length is prefixed with 7bit encoded int.
		/// </summary>
		/// <returns>Read byte array value.</returns>
		public byte[] ReadByteArray()
		{
			int length = Read7BitEncodedInt();

			if (!CanSeek(length))
				throw new ArgumentOutOfRangeException();

			byte[] retVal = new byte[length];
			CopyTo(Position, retVal, 0, (uint)length);
			Position += length;

			return retVal;
		}

		/// <summary>
		/// Reads 7bit encoded int.
		/// Taken from https://github.com/Microsoft/referencesource/blob/4fe4349175f4c5091d972a7e56ea12012f1e7170/mscorlib/system/io/binaryreader.cs
		/// </summary>
		/// <returns>int value.</returns>
		private int Read7BitEncodedInt()
		{
			// Read out an Int32 7 bits at a time.  The high bit
			// of the byte when on means to continue reading more bytes.
			int count = 0;
			int shift = 0;
			byte b;

			do
			{
				// Check for a corrupted stream.  Read a max of 5 bytes.
				// In a future version, add a DataFormatException.
				if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
					throw new FormatException("Wrong format");

				// ReadByte handles end of stream cases for us.
				b = ReadByte();
				count |= (b & 0x7F) << shift;
				shift += 7;
			} while ((b & 0x80) != 0);

			return count;
		}

		#endregion

		#region Copy

		#region CopyTo

		/// <summary>
		/// Copies unmanaged byte array to managed byte array from source offset to destination offset.
		/// </summary>
		/// <param name="srcOffset">Offset of the unmanaged array.</param>
		/// <param name="destination">Destionation of the managed array.</param>
		/// <param name="destOffset">Offset of the managed array.</param>
		/// <param name="length">Length to copy.</param>
		public void CopyTo(int srcOffset, byte[] destination, int destOffset, uint length)
		{
			fixed (byte* destPtr = destination)
			{
				if ((Array.ArrayPtr + srcOffset + AdjustedPosition + length > Array.ArrayPtr + Array.Length) || (destOffset + length > destination.Length))
					throw new IndexOutOfRangeException();

				Copy(Array.ArrayPtr, srcOffset + AdjustedPosition, destPtr, destOffset, length);
			}
		}

		#endregion

		#region CopyFrom

		/// <summary>
		/// Copyies managed byte array to unmanaged byte array from source offset to destination offset.
		/// </summary>
		/// <param name="source">Source of the managed array.</param>
		/// <param name="srcOffset">Offset of the managed array.</param>
		/// <param name="destOffset">Offset of the unmanaged array.</param>
		/// <param name="length">Length to copy.</param>
		public void CopyFrom(byte[] source, int srcOffset, int destOffset, uint length)
		{
			fixed (byte* srcPtr = source)
			{
				if ((srcOffset + AdjustedPosition + length > source.Length) || (Array.ArrayPtr + destOffset + length > Array.ArrayPtr + Array.Length))
					throw new IndexOutOfRangeException();

				Copy(srcPtr, srcOffset + AdjustedPosition, Array.ArrayPtr, destOffset, length);
			}
		}

		#endregion

		#endregion

		#region Adjust

		/// <summary>
		/// Adjusts position for copying.
		/// Copy starts from offset + this value
		/// </summary>
		/// <param name="count">Count to adjust position.</param>
		public void Adjust(int count)
		{
			_adjustedPosition = count;
		}

		#endregion

		#endregion
	}
}
