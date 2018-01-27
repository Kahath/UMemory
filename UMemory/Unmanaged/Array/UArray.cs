/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace UMemory.Unmanaged.Array
{
	/// <summary>
	/// Provides fast security critical unsafe array.
	/// </summary>
	[SecurityCritical]
	[ComVisible(false)]
	public unsafe class UArray
	{
		#region Fields

		private byte* _arrayPtr;
		private int _length;
		private int _index;

		#endregion

		#region Properties

		/// <summary>
		/// Pointer to first index in array.
		/// </summary>
		public byte* ArrayPtr
		{
			get { return _arrayPtr; }
		}

		/// <summary>
		/// Length of the array.
		/// </summary>
		public int Length
		{
			get { return _length; }
		}

		/// <summary>
		/// Gets byte value from array on index.
		/// </summary>
		/// <param name="i">Index of the array.</param>
		/// <returns>byte on specified index in array.</returns>
		internal byte this[int i]
		{
			get
			{
				if (_index < 0 || i < 0 || i > _index)
					throw new IndexOutOfRangeException();

				return *(_arrayPtr + i);
			}
			set
			{
				if (_index < 0 || i < 0 || i > _index)
					throw new IndexOutOfRangeException();

				*(_arrayPtr + i) = value;
			}
		}

		/// <summary>
		/// Gets byte value from array on byte pointer.
		/// </summary>
		/// <param name="i">byte pointer.</param>
		/// <returns>byte on specified pointer in array.</returns>
		internal byte this[byte* i]
		{
			get
			{
				if (_index < 0 || i < _arrayPtr || i > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				return *i;
			}
			set
			{
				if (_index < 0 || i < _arrayPtr || i > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				*i = value;
			}
		}

		/// <summary>
		/// Gets short value from array on short pointer.
		/// </summary>
		/// <param name="i">short pointer.</param>
		/// <returns>short on specified pointer in array.</returns>
		internal short this[short* i]
		{
			get
			{
				if (_index < 0 || (i < _arrayPtr) || (byte*)i + 1 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				return *i;
			}
			set
			{
				if (_index < 0 || i < _arrayPtr || (byte*)i + 1 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				*i = value;
			}
		}

		/// <summary>
		/// Gets int value from array on int pointer.
		/// </summary>
		/// <param name="i">int pointer.</param>
		/// <returns>int on specified pointer in array.</returns>
		internal int this[int* i]
		{
			get
			{
				if (_index < 0 || i < _arrayPtr || (byte*)i + 3 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				return *i;
			}
			set
			{
				if (_index < 0 || i < _arrayPtr || (byte*)i + 3 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				*i = value;
			}
		}

		/// <summary>
		/// Gets long value from array on long pointer.
		/// </summary>
		/// <param name="i">long pointer.</param>
		/// <returns>long on specified pointer in array.</returns>
		internal long this[long* i]
		{
			get
			{
				if (_index < 0 || i < _arrayPtr || (byte*)i + 7 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				return *i;
			}
			set
			{
				if (_index < 0 || i < _arrayPtr || (byte*)i + 7 > _arrayPtr + _index)
					throw new IndexOutOfRangeException();

				*i = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Instantiates new object of <see cref="UArray"/> type.
		/// </summary>
		/// <param name="capacity">Capacity of the array.</param>
		internal UArray(int capacity)
		{
			Alloc(capacity);
		}

		~UArray()
		{
			Free();
		}

		#endregion

		#region Methods

		#region Alloc

		/// <summary>
		/// Frees currently used memory block if exists
		/// and Allocates new memory block.
		/// </summary>
		/// <param name="length">length of the array</param>
		internal void Alloc(int length)
		{
			Free();
			_length = length;
			_index = length - 1;
			_arrayPtr = (byte*)Marshal.AllocHGlobal(length);
		}

		#endregion

		#region Realloc

		/// <summary>
		/// Reallocates new memory block.
		/// </summary>
		/// <param name="length">Length of the array.</param>
		internal void Realloc(int length)
		{
			_length = length;
			_index = length - 1;
			_arrayPtr = (byte*)Marshal.ReAllocHGlobal((IntPtr)_arrayPtr, (IntPtr)length);
		}

		#endregion

		#region Free

		/// <summary>
		/// Frees currently used memory block.
		/// </summary>
		internal void Free()
		{
			if (_arrayPtr != null)
			{
				Marshal.FreeHGlobal((IntPtr)_arrayPtr);
				_arrayPtr = null;
			}
		}

		#endregion

		#endregion
	}
}
