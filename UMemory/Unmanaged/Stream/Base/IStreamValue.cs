using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMemory.Unmanaged.Stream.Core;

namespace UMemory.Unmanaged.Stream.Base
{
	public interface IStreamValue
	{
		T ReadValue<T>(UMemoryStream stream);
		void WriteValue<T>(UMemoryStream stream);
	}
}
