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
	/// <summary>
	/// Provides extended fast memory I/O operations on top of unmanaged array.
	/// </summary>
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
		/// <param name="endiannessType">Endianness order to Read/Write.</param>
		public UMemoryStream(int length, EndiannessType endiannessType)
			: base(length, endiannessType)
		{
			_encoding = Encoding.UTF8;
		}

		#endregion

		#region Methods

		#region Write

		/// <summary>
		/// Writes <see cref="IUMemoryWrite"/> instance to stream.
		/// </summary>
		/// <param name="data">Instance to write.</param>
		public void Write(IUMemoryWrite data)
		{
			if (data == null)
				throw new NullReferenceException("data");

			data.Write(this);
		}

		/// <summary>
		/// Writes string value to current stream position with prefixed length as 7bit encoded int.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">String value to write.</param>
		public void WriteString(string data)
		{
			int length = _encoding.GetByteCount(data);
			byte lengthCount = Get7BitEncodedIntPlaces(length);

			if (!CanSeek(length + lengthCount))
				throw new IndexOutOfRangeException();

			Write7BitEncodedInt(length);
			fixed (char* chrPtr = data)
			{
				_encoding.GetBytes(chrPtr, data.Length, PositionPtr, length);
			}

			Position += length;
		}

		/// <summary>
		/// Writes null terminated string value to current stream position.
		/// Increases stream position
		/// </summary>
		/// <param name="data">String value to write.</param>
		public void WriteCString(string data)
		{
			if (String.IsNullOrEmpty(data))
			{
				WriteUInt8(0);
			}
			else
			{
				int count = _encoding.GetByteCount(data);
				if (!CanSeek(count + 1))
					throw new IndexOutOfRangeException();

				fixed (char* chrPtr = data)
				{
					_encoding.GetBytes(chrPtr, data.Length, PositionPtr, count);
				}

				Position += count;
				WriteUInt8(0);
			}
		}

		/// <summary>
		/// Writes byte array value to current stream position with prefixed length as 7bit encoded int.
		/// Increases stream position.
		/// </summary>
		/// <param name="data">Byte array value to write.</param>
		public void WriteBytes(byte[] data)
		{
			WriteBytes(data, data.Length);
		}

		/// <summary>
		/// Writes byte array value to current stream position with specified elements count.
		/// </summary>
		/// <param name="data">Byte array value to write.</param>
		/// <param name="count">Elements count to write.</param>
		public void WriteBytes(byte[] data, int count)
		{
			byte countPlaces = Get7BitEncodedIntPlaces(count);

			if (!CanSeek(count + countPlaces))
				throw new IndexOutOfRangeException();

			Write7BitEncodedInt(count);
			WriteBytes(data, 0, count);
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
				WriteUInt8((byte)(v | 0x80));
				v >>= 7;
			}
			WriteUInt8((byte)v);
		}

		/// <summary>
		/// Gets number of bytes to write for 7bit encoded int.
		/// </summary>
		/// <param name="value">Value to write as 7bit encoded value.</param>
		/// <returns>Number of bytes.</returns>
		private byte Get7BitEncodedIntPlaces(int value)
		{
			byte retVal = 1;

			uint v = (uint)value;
			while (v >= 0x80)
			{
				++retVal;
				v >>= 7;
			}

			return retVal;
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
		/// Reads string value on current stream position.
		/// String length is prefixed with 7bit encoded int.
		/// </summary>
		/// <returns>Read string value.</returns>
		public string ReadString()
		{
			int length = Read7BitEncodedInt();

			if (!CanSeek(length))
				throw new IndexOutOfRangeException();

			string retVal = _encoding.GetString(PositionPtr, length);
			Position += length;

			return retVal;
		}

		/// <summary>
		/// Reads null terminated string value on current stream position.
		/// </summary>
		/// <returns>Read string value.</returns>
		public string ReadCString()
		{
			int count = SeekValue(0);

			if (!CanSeek(count + 1))
				throw new IndexOutOfRangeException();

			string retVal = _encoding.GetString(PositionPtr, count);
			Position += count;
			ReadUInt8();

			return retVal;
		}

		/// <summary>
		/// Reads byte array value on current stream position.
		/// Byte array length is prefixed with 7bit encoded int.
		/// </summary>
		/// <returns>Read byte array value.</returns>
		public byte[] ReadBytes()
		{
			int length = Read7BitEncodedInt();
			return ReadBytes(length);
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
				b = ReadUInt8();
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
