/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using System;
using System.Runtime.InteropServices;
using System.Security;
using UMemory.Unmanaged.Array;
using UMemory.Unmanaged.Enums;

namespace UMemory.Unmanaged.Stream.Base
{
	/// <summary>
	/// Provides basic I/O operations on top of unmanaged array.
	/// Manages current position of stream.
	/// I/O depends of endianness type.
	/// </summary>
	[SecurityCritical]
	[ComVisible(false)]
	public abstract unsafe class UStream
	{
		#region Fields

		private UArray _array;
		private int _position;
		private bool _changeOrder;
		private bool _isx64;
		private EndiannessType _endianessType;

		#endregion

		#region Properties

		/// <summary>
		/// Object of <see cref="UArray"/> type.
		/// </summary>
		protected internal UArray Array
		{
			get { return _array; }
		}

		/// <summary>
		/// Position of the stream.
		/// </summary>
		public int Position
		{
			get { return _position; }
			internal set { _position = value; }
		}

		/// <summary>
		/// Pointer to stream position.
		/// </summary>
		protected internal virtual byte* PositionPtr
		{
			get { return _array.ArrayPtr + _position; }
		}

		/// <summary>
		/// Capacity of the underlying array.
		/// </summary>
		public int Capacity
		{
			get { return _array.Length; }
		}

		/// <summary>
		/// Endianess type
		/// </summary>
		public EndiannessType EndianessType
		{
			get { return _endianessType; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new <see cref="UStream"/> type.
		/// </summary>
		/// <param name="length">Length of the array to instantiate.</param>
		/// <param name="endiannessType">Type of endianness to use for Read/Write operations.</param>
		public UStream(int length, EndiannessType endiannessType)
		{
			_array = new UArray(length);
			_position = 0;
			_endianessType = endiannessType;
			_changeOrder = (BitConverter.IsLittleEndian && EndianessType == EndiannessType.BigEndian) || (!BitConverter.IsLittleEndian && EndianessType == EndiannessType.LittleEndian);
			_isx64 = IntPtr.Size == 8;
		}

		#endregion

		#region Methods

		#region Alloc

		/// <summary>
		/// Allocates new memory block for array.
		/// </summary>
		/// <param name="maxLength">Maximum length of the array.</param>
		public void Alloc(int maxLength)
		{
			_array.Alloc(maxLength);
		}

		#endregion

		#region Realloc

		/// <summary>
		/// Reallocates the underlying array.
		/// </summary>
		/// <param name="length">Length of the reallocated array.</param>
		public void Realloc(int length)
		{
			_array.Realloc(length);
		}

		#endregion

		#region Free

		/// <summary>
		/// Frees memory from the underlying array.
		/// </summary>
		public void Free()
		{
			_array.Free();
		}

		#endregion

		#region Seek

		/// <summary>
		/// Sets position to offset.
		/// Performs CanSeek first.
		/// </summary>
		/// <param name="offset">Offset.</param>
		public void Seek(int offset)
		{
			if (!CanSeek(offset))
				throw new IndexOutOfRangeException();

			_position = offset;
		}

		/// <summary>
		/// Seeks for value from current stream position.
		/// </summary>
		/// <param name="value">Byte value to seek.</param>
		/// <returns>Number of elements to seek value.</returns>
		public int SeekValue(byte value)
		{
			byte* positionPtr = PositionPtr;
			int count = 0;

			while (_array[positionPtr + count] != value)
				++count;

			return count;
		}

		/// <summary>
		/// Check if offset is withing the length of the array.
		/// </summary>
		/// <param name="offset"></param>
		/// <returns>Boolean value.</returns>
		public bool CanSeek(int offset)
		{
			return offset >= 0 && offset <= _array.Length;
		}

		#endregion

		#region Skip

		/// <summary>
		/// Increments position for count.
		/// </summary>
		/// <param name="count">Count.</param>
		public void Skip(int count)
		{
			if (!CanSeek(_position + count))
				throw new IndexOutOfRangeException();

			_position += count;
		}

		#endregion

		#region Write

		/// <summary>
		/// Writes boolean value to the underlying array.
		/// </summary>
		/// <param name="data">Boolean value to write.</param>
		public void WriteBool(bool data)
		{
			WriteInt8OnPtr(PositionPtr, data ? (byte)1 : (byte)0);
			_position += 1;
		}

		/// <summary>
		/// Writes byte value to the underlying array.
		/// </summary>
		/// <param name="data">Byte value to write.</param>
		public void WriteUInt8(byte data)
		{
			WriteInt8OnPtr(PositionPtr, data);
			_position += 1;
		}

		/// <summary>
		/// Writes unsigned 16bit integer value to underlying array.
		/// </summary>
		/// <param name="data">Ushort value to write.</param>
		public void WriteUInt16(ushort data)
		{
			WriteInt16OnPtr(PositionPtr, (short)data);
			_position += 2;
		}


		/// <summary>
		/// Writes unsigned 32 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Uint value to write.</param>
		public void WriteUInt32(uint data)
		{
			WriteInt32OnPtr(PositionPtr, (int)data);
			_position += 4;
		}

		/// <summary>
		/// Writes unsigned 64 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Ulong value to write.</param>
		public void WriteUInt64(ulong data)
		{
			WriteInt64OnPtr(PositionPtr, (long)data);
			_position += 8;
		}

		/// <summary>
		/// Writes signed 8 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Sbyte value to write.</param>
		public void WriteInt8(sbyte data)
		{
			WriteInt8OnPtr(PositionPtr, (byte)data);
			_position += 1;
		}

		/// <summary>
		/// Writes 16 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Ushort value to write</param>
		public void WriteInt16(short data)
		{
			WriteInt16OnPtr(PositionPtr, data);
			_position += 2;
		}


		/// <summary>
		/// Writes char value to the underlying array.
		/// </summary>
		/// <param name="data">Char value to write.</param>
		public void WriteChar(char data)
		{
			WriteInt16OnPtr(PositionPtr, (short)data);
			_position += 2;
		}

		/// <summary>
		/// Writes 32 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Int value to write.</param>
		public void WriteInt32(int data)
		{
			WriteInt32OnPtr(PositionPtr, data);
			_position += 4;
		}

		/// <summary>
		/// Writes 64 bit integer value to the underlying array.
		/// </summary>
		/// <param name="data">Long data to write.</param>
		public void WriteInt64(long data)
		{
			WriteInt64OnPtr(PositionPtr, data);
			_position += 8;
		}

		/// <summary>
		/// Writes 32 bit floating point value to the underlying array.
		/// </summary>
		/// <param name="data">Float value to write.</param>
		public void WriteFloat(float data)
		{
			WriteInt32OnPtr(PositionPtr, *(int*)&data);
			_position += 4;
		}

		/// <summary>
		/// Writes 64 bit floating point value to the underlying array.
		/// </summary>
		/// <param name="data">Double value to write</param>
		public void WriteDouble(double data)
		{
			WriteInt64OnPtr(PositionPtr, *(long*)&data);
			_position += 8;
		}

		/// <summary>
		/// Writes byte array to underlying array on current stream position.
		/// </summary>
		/// <param name="data">Byte array to write.</param>
		/// <param name="offset">Byte array offset.</param>
		/// <param name="count">Number of bytes to write from array.</param>
		public void WriteBytes(byte[] data, int offset, int count)
		{
			if (!CanSeek(count))
				throw new IndexOutOfRangeException();

			fixed (byte* dataPtr = data)
			{
				Copy(dataPtr, offset, PositionPtr, 0, count);
			}

			_position += count;
		}

		#endregion

		#region WriteOnPtr

		/// <summary>
		/// Writes 8-bit integer value to the underlying array.
		/// </summary>
		/// <param name="positionPtr">Pointer to where data should be written.</param>
		/// <param name="data">8-bit integer value to write.</param>
		private void WriteInt8OnPtr(byte* positionPtr, byte data)
		{
			_array[positionPtr] = data;
		}

		/// <summary>
		/// Writes 16-bit integer value to the underlying array.
		/// </summary>
		/// <param name="positionPtr">Pointer to where data should be written.</param>
		/// <param name="data">16-bit integer value to write.</param>
		private void WriteInt16OnPtr(byte* positionPtr, short data)
		{
			if (unchecked((int)positionPtr & 0x01) == 0 && !_changeOrder)
				_array[(short*)positionPtr] = data;
			else if (_endianessType == EndiannessType.BigEndian)
			{
				_array[positionPtr + 1] = (byte)data;
				_array[positionPtr] = (byte)(data >> 8);
			}
			else
			{
				_array[positionPtr] = (byte)data;
				_array[positionPtr + 1] = (byte)(data >> 8);
			}
		}

		/// <summary>
		/// Writes 32-bit integer value to the underlying array.
		/// </summary>
		/// <param name="positionPtr">Pointer to where data should be written.</param>
		/// <param name="data">32-bit integer value to write.</param>
		private void WriteInt32OnPtr(byte* positionPtr, int data)
		{
			if (unchecked((int)positionPtr & 0x03) == 0 && !_changeOrder)
				_array[(int*)positionPtr] = data;
			else if (_endianessType == EndiannessType.BigEndian)
			{
				_array[positionPtr + 3] = (byte)data;
				_array[positionPtr + 2] = (byte)(data >> 8);
				_array[positionPtr + 1] = (byte)(data >> 16);
				_array[positionPtr] = (byte)(data >> 24);

			}
			else
			{
				_array[positionPtr] = (byte)data;
				_array[positionPtr + 1] = (byte)(data >> 8);
				_array[positionPtr + 2] = (byte)(data >> 16);
				_array[positionPtr + 3] = (byte)(data >> 24);
			}
		}

		/// <summary>
		/// Writes 64-bit integer value to the underlying array.
		/// </summary>
		/// <param name="positionPtr">Pointer to where data should be written.</param>
		/// <param name="data">64-bit integer value to write.</param>
		private void WriteInt64OnPtr(byte* positionPtr, long data)
		{
			if (unchecked((int)positionPtr & 0x07) == 0 && !_changeOrder)
				_array[(long*)positionPtr] = data;
			else if (_endianessType == EndiannessType.BigEndian)
			{
				_array[positionPtr + 7] = (byte)data;
				_array[positionPtr + 6] = (byte)(data >> 8);
				_array[positionPtr + 5] = (byte)(data >> 16);
				_array[positionPtr + 4] = (byte)(data >> 24);
				_array[positionPtr + 3] = (byte)(data >> 32);
				_array[positionPtr + 2] = (byte)(data >> 40);
				_array[positionPtr + 1] = (byte)(data >> 48);
				_array[positionPtr] = (byte)(data >> 56);
			}
			else
			{
				_array[positionPtr] = (byte)data;
				_array[positionPtr + 1] = (byte)(data >> 8);
				_array[positionPtr + 2] = (byte)(data >> 16);
				_array[positionPtr + 3] = (byte)(data >> 24);
				_array[positionPtr + 4] = (byte)(data >> 32);
				_array[positionPtr + 5] = (byte)(data >> 40);
				_array[positionPtr + 6] = (byte)(data >> 48);
				_array[positionPtr + 7] = (byte)(data >> 56);
			}
		}

		#endregion

		#region Read

		/// <summary>
		/// Reads boolean value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public bool ReadBool()
		{
			bool retVal = ReadInt8OnPtr(PositionPtr) > 0;
			_position += 1;

			return retVal;
		}

		/// <summary>
		/// Reads 8-bit integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public byte ReadUInt8()
		{
			byte retVal = ReadInt8OnPtr(PositionPtr);
			_position += 1;

			return retVal;
		}

		/// <summary>
		/// Reads 8-bit signed integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public sbyte ReadInt8()
		{
			sbyte retVal = (sbyte)ReadInt8OnPtr(PositionPtr);
			_position += 1;

			return retVal;
		}

		/// <summary>
		/// Reads char value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public char ReadChar()
		{
			char retVal = (char)ReadInt16OnPtr(PositionPtr);
			_position += 2;

			return retVal;
		}

		/// <summary>
		/// Reads 16-bit unsigned integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public ushort ReadUInt16()
		{
			ushort retVal = (ushort)ReadInt16OnPtr(PositionPtr);
			_position += 2;

			return retVal;
		}

		/// <summary>
		/// Reads 16-bit signed integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public short ReadInt16()
		{
			short retVal = ReadInt16OnPtr(PositionPtr);
			_position += 2;

			return retVal;
		}

		/// <summary>
		/// Reads 32-bit unsigned integer value from the underlying array .
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public uint ReadUInt32()
		{
			uint retVal = (uint)ReadInt32OnPtr(PositionPtr);
			_position += 4;

			return retVal;
		}

		/// <summary>
		/// Reads 32-bit signed integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public int ReadInt32()
		{
			int retVal = ReadInt32OnPtr(PositionPtr);
			_position += 4;

			return retVal;
		}

		/// <summary>
		/// Reads 64-bit unsigned integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public ulong ReadUInt64()
		{
			ulong retVal = (ulong)ReadInt64OnPtr(PositionPtr);
			_position += 8;

			return retVal;
		}

		/// <summary>
		/// Reads 64-bit signed integer value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public long ReadInt64()
		{
			long retVal = ReadInt64OnPtr(PositionPtr);
			_position += 8;

			return retVal;
		}

		/// <summary>
		/// Reads 32-bit floating point value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array.</returns>
		public float ReadFloat()
		{
			int data = ReadInt32OnPtr(PositionPtr);
			_position += 4;

			return *((float*)&data);
		}

		/// <summary>
		/// Reads 64-bit floating point value from the underlying array on current stream position.
		/// </summary>
		/// <returns>Read value from the array</returns>
		public double ReadDouble()
		{
			long data = ReadInt64OnPtr(PositionPtr);
			_position += 8;

			return *((double*)&data);
		}

		/// <summary>
		/// Reads number of bytes in byte array from underlying array on current stream position.
		/// </summary>
		/// <param name="count">Number of bytes to read.</param>
		/// <returns>Byte array with specified number of elements.</returns>
		public byte[] ReadBytes(int count)
		{
			if (!CanSeek(count))
				throw new IndexOutOfRangeException();

			byte[] retVal = new byte[count];

			fixed (byte* destPtr = retVal)
			{
				Copy(PositionPtr, 0, destPtr, 0, count);
			}

			_position += count;
			return retVal;
		}

		#endregion

		#region ReadOnPtr

		/// <summary>
		/// Reads 8-bit integer value from the underlying array on specified memory location.
		/// </summary>
		/// <param name="positionPtr">Pointer from where value should be read.</param>
		/// <returns>Read value from the array.</returns>
		private byte ReadInt8OnPtr(byte* positionPtr)
		{
			return _array[positionPtr];
		}

		/// <summary>
		/// Reads 16-bit integer value from the underlying array on specified memory location.
		/// </summary>
		/// <param name="positionPtr">Pointer from where value should be read.</param>
		/// <returns>Read value from the array.</returns>
		private short ReadInt16OnPtr(byte* positionPtr)
		{
			short retVal = 0;

			if (unchecked((int)positionPtr & 0x01) == 0 && !_changeOrder)
				retVal = _array[(short*)positionPtr];
			else if (_endianessType == EndiannessType.BigEndian)
			{
				retVal |= (short)_array[positionPtr + 1];
				retVal |= (short)(_array[positionPtr] << 8);
			}
			else
			{
				retVal |= (short)_array[positionPtr];
				retVal |= (short)(_array[positionPtr + 1] << 8);
			}

			return retVal;
		}

		/// <summary>
		/// Reads 32-bit integer value from the underlying array on specified memory location.
		/// </summary>
		/// <param name="positionPtr">Pointer from where value should be read.</param>
		/// <returns>Read value from the array.</returns>
		private int ReadInt32OnPtr(byte* positionPtr)
		{
			int retVal = 0;

			if (unchecked((int)positionPtr & 0x03) == 0 && !_changeOrder)
				retVal = _array[(int*)positionPtr];
			else if (_endianessType == EndiannessType.BigEndian)
			{
				retVal |= (int)_array[positionPtr + 3];
				retVal |= (int)(_array[positionPtr + 2]) << 8;
				retVal |= (int)(_array[positionPtr + 1]) << 16;
				retVal |= (int)(_array[positionPtr]) << 24;
			}
			else
			{
				retVal |= (int)_array[positionPtr];
				retVal |= (int)(_array[positionPtr + 1]) << 8;
				retVal |= (int)(_array[positionPtr + 2]) << 16;
				retVal |= (int)(_array[positionPtr + 3]) << 24;
			}

			return retVal;
		}

		/// <summary>
		/// Reads 64-bit integer from the underlying array on specified memory location.
		/// </summary>
		/// <param name="positionPtr">Pointer from where value should be read.</param>
		/// <returns>Read value from the array.</returns>
		private long ReadInt64OnPtr(byte* positionPtr)
		{
			long retVal = 0;

			if (((int)positionPtr & 0x07) == 0 && !_changeOrder)
				retVal = _array[(long*)positionPtr];
			else if (_endianessType == EndiannessType.BigEndian)
			{
				retVal |= (long)_array[positionPtr + 7];
				retVal |= (long)(_array[positionPtr + 6]) << 8;
				retVal |= (long)(_array[positionPtr + 5]) << 16;
				retVal |= (long)(_array[positionPtr + 4]) << 24;
				retVal |= (long)(_array[positionPtr + 3]) << 32;
				retVal |= (long)(_array[positionPtr + 2]) << 40;
				retVal |= (long)(_array[positionPtr + 1]) << 48;
				retVal |= (long)(_array[positionPtr]) << 56;
			}
			else
			{
				retVal |= (long)_array[positionPtr];
				retVal |= (long)(_array[positionPtr + 1]) << 8;
				retVal |= (long)(_array[positionPtr + 2]) << 16;
				retVal |= (long)(_array[positionPtr + 3]) << 24;
				retVal |= (long)(_array[positionPtr + 4]) << 32;
				retVal |= (long)(_array[positionPtr + 5]) << 40;
				retVal |= (long)(_array[positionPtr + 6]) << 48;
				retVal |= (long)(_array[positionPtr + 7]) << 56;
			}

			return retVal;
		}

		#endregion

		#region Copy

		#region x86

		/// <summary>
		/// Performs array copy optimized for x86 platform.
		/// Modified https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/buffer.cs#L279
		/// </summary>
		/// <param name="src">Pointer of source first element.</param>
		/// <param name="dest">Pointer of destination first element.</param>
		/// <param name="len">Length to copy.</param>
		private void Copyx86(byte* src, byte* dest, uint len)
		{
			if (((uint)dest - (uint)src) < len)
				throw new AccessViolationException();

			switch (len)
			{
				case 0:
					return;
				case 1:
					*dest = *src;
					return;
				case 2:
					*(short*)dest = *(short*)src;
					return;
				case 3:
					*(short*)dest = *(short*)src;
					*(dest + 2) = *(src + 2);
					return;
				case 4:
					*(int*)dest = *(int*)src;
					return;
				case 5:
					*(int*)dest = *(int*)src;
					*(dest + 4) = *(src + 4);
					return;
				case 6:
					*(int*)dest = *(int*)src;
					*(short*)(dest + 4) = *(short*)(src + 4);
					return;
				case 7:
					*(int*)dest = *(int*)src;
					*(short*)(dest + 4) = *(short*)(src + 4);
					*(dest + 6) = *(dest + 6);
					return;
				case 8:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					return;
				case 9:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(dest + 8) = *(src + 8);
					return;
				case 10:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(short*)(dest + 8) = *(short*)(src + 8);
					return;
				case 11:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(short*)(dest + 8) = *(short*)(src + 8);
					*(dest + 10) = *(src + 10);
					return;
				case 12:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					return;
				case 13:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(dest + 12) = *(src + 12);
					return;
				case 14:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(short*)(dest + 12) = *(short*)(src + 12);
					return;
				case 15:
					*(int*)(dest) = *(int*)(src);
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(short*)(dest + 12) = *(short*)(src + 12);
					*(dest + 14) = *(src + 14);
					return;
				case 16:
					*(int*)dest = *(int*)src;
					*(int*)(dest + 4) = *(int*)(src + 4);
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(int*)(dest + 12) = *(int*)(src + 12);
					return;
				default:
					break;
			}
			if (((int)dest & 3) != 0)
			{
				if (((int)dest & 1) != 0)
				{
					*dest = *src;
					dest++;
					src++;
					len--;
				}

				if (((int)dest & 2) != 0)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
					len -= 2;
				}
			}

			uint count = len / 16;

			while (count > 0)
			{
				((int*)dest)[0] = ((int*)src)[0];
				((int*)dest)[1] = ((int*)src)[1];
				((int*)dest)[2] = ((int*)src)[2];
				((int*)dest)[3] = ((int*)src)[3];

				dest += 16;
				src += 16;
				count--;
			}

			if ((len & 8) != 0)
			{
				((int*)dest)[0] = ((int*)src)[0];
				((int*)dest)[1] = ((int*)src)[1];

				dest += 8;
				src += 8;
			}

			if ((len & 4) != 0)
			{
				((int*)dest)[0] = ((int*)src)[0];
				dest += 4;
				src += 4;
			}

			if ((len & 2) != 0)
			{
				((short*)dest)[0] = ((short*)src)[0];
				dest += 2;
				src += 2;
			}

			if ((len & 1) != 0)
				*dest = *src;

		}

		#endregion

		#region x64

		/// <summary>
		/// Performs array copy optimized for x64 platform.
		/// Modified https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/buffer.cs#L277
		/// </summary>
		/// <param name="src">Pointer of source first element.</param>
		/// <param name="dest">Pointer of destination first element.</param>
		/// <param name="len">Length to copy.</param>
		private void Copyx64(byte* src, byte* dest, ulong len)
		{
			if ((ulong)dest - (ulong)src < len)
				throw new AccessViolationException();

			switch (len)
			{
				case 0:
					return;
				case 1:
					*dest = *src;
					return;
				case 2:
					*(short*)dest = *(short*)src;
					return;
				case 3:
					*(short*)dest = *(short*)src;
					*(dest + 2) = *(src + 2);
					return;
				case 4:
					*(int*)dest = *(int*)src;
					return;
				case 5:
					*(int*)dest = *(int*)src;
					*(dest + 4) = *(src + 4);
					return;
				case 6:
					*(int*)dest = *(int*)src;
					*(short*)(dest + 4) = *(short*)(src + 4);
					return;
				case 7:
					*(int*)dest = *(int*)src;
					*(short*)(dest + 4) = *(short*)(src + 4);
					*(dest + 6) = *(dest + 6);
					return;
				case 8:
					*(long*)dest = *(long*)src;
					return;
				case 9:
					*(long*)dest = *(long*)src;
					*(dest + 8) = *(src + 8);
					return;
				case 10:
					*(long*)dest = *(long*)src;
					*(short*)(dest + 8) = *(short*)(src + 8);
					return;
				case 11:
					*(long*)dest = *(long*)src;
					*(short*)(dest + 8) = *(short*)(src + 8);
					*(dest + 10) = *(src + 10);
					return;
				case 12:
					*(long*)dest = *(long*)src;
					*(int*)(dest + 8) = *(int*)(src + 8);
					return;
				case 13:
					*(long*)dest = *(long*)src;
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(dest + 12) = *(src + 12);
					return;
				case 14:
					*(long*)dest = *(long*)src;
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(short*)(dest + 12) = *(short*)(src + 12);
					return;
				case 15:
					*(long*)dest = *(long*)src;
					*(int*)(dest + 8) = *(int*)(src + 8);
					*(short*)(dest + 12) = *(short*)(src + 12);
					*(dest + 14) = *(src + 14);
					return;
				case 16:
					*(long*)dest = *(long*)src;
					*(long*)(dest + 8) = *(long*)(src + 8);
					return;
				default:
					break;
			}
			if (((int)dest & 3) != 0)
			{
				if (((int)dest & 1) != 0)
				{
					*dest = *src;
					dest++;
					src++;
					len--;
				}

				if (((int)dest & 2) != 0)
				{
					*(short*)dest = *(short*)src;
					dest += 2;
					src += 2;
					len -= 2;
				}
			}

			if (((int)dest & 4) != 0)
			{
				*(int*)dest = *(int*)src;
				src += 4;
				dest += 4;
				len -= 4;
			}

			ulong count = len / 16;

			while (count > 0)
			{
				((long*)dest)[0] = ((long*)src)[0];
				((long*)dest)[1] = ((long*)src)[1];

				dest += 16;
				src += 16;
				count--;
			}

			if ((len & 8) != 0)
			{
				((long*)dest)[0] = ((long*)src)[0];

				dest += 8;
				src += 8;
			}

			if ((len & 4) != 0)
			{
				((int*)dest)[0] = ((int*)src)[0];
				dest += 4;
				src += 4;
			}

			if ((len & 2) != 0)
			{
				((short*)dest)[0] = ((short*)src)[0];
				dest += 2;
				src += 2;
			}

			if ((len & 1) != 0)
				*dest = *src;
		}

		#endregion

		#region Copy

		/// <summary>
		/// Copies data from one array at given location to another array with specified length.
		/// </summary>
		/// <param name="source">Pointer to source</param>
		/// <param name="srcOffset">Source offset from which copy should start.</param>
		/// <param name="destination">Pointer to destionation.</param>
		/// <param name="destOffset">Destination offset where copy should start.</param>
		/// <param name="length">Length of the data to copy.</param>
		protected void Copy(byte* source, int srcOffset, byte* destination, int destOffset, long length)
		{
			source += srcOffset;
			destination += destOffset;

			if (_isx64)
				Copyx64(source, destination, (ulong)length);
			else
				Copyx86(source, destination, (uint)length);
		}

		#endregion

		#endregion

		#endregion
	}
}
