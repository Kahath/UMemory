/*
 * Copyright © Kahath 2018
 * Licensed under MIT license.
 */

using UMemory.Unmanaged.Stream.Core;

namespace UMemory.Unmanaged.Stream.Core
{
	/// <summary>
	/// Provides custom type Write operations.
	/// </summary>
	public interface IUMemoryRead
	{
		/// <summary>
		/// Writes custom type to stream.
		/// </summary>
		/// <param name="stream"><see cref="UMemoryStream"/> instance.</param>
		void Read(UMemoryStream stream);
	}
}
