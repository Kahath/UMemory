/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using UMemory.Unmanaged.Stream.Core;

namespace UMemory.Unmanaged.Stream.Core
{
	/// <summary>
	/// Provides custom type Read operations
	/// </summary>
	public interface IUMemoryWrite
	{
		/// <summary>
		/// Reads custom type from stream.
		/// </summary>
		/// <param name="stream"><see cref="UMemoryStream"/> instance.</param>
		void Write(UMemoryStream stream);
	}
}
