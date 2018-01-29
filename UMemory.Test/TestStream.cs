using UMemory.Unmanaged.Enums;
using UMemory.Unmanaged.Stream.Core;

namespace UMemory.Test
{
	public class TestStream : UMemoryStream
	{
		#region Constructor

		public TestStream(int length, EndiannessType endiannessType) : base(length, endiannessType)
		{
		}

		#endregion
	}
}
