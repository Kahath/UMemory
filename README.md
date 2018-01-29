# UMemory

[![Join Discord](https://img.shields.io/badge/discord-join-7289DA.svg)](https://discord.gg/9AyZ9Ag) [![Build status](https://ci.appveyor.com/api/projects/status/k09ihk4xgvonkeyw?svg=true)](https://ci.appveyor.com/project/Kahath/umemory)

UMemory is lightweight library written in unsafe context for memory I/O operations that supports endianness type without performance loss. It should outperform, but not replace .NET BinaryReader/BinaryWriter on MemoryStream. Best use of this library would be for memory I/O operations where array boundaries and operations are known since there is no real length of stream, only position and capacity. For example TCP/IP packets where new arrays are constantly created. UMemory allocates unmanaged memory so GC will not trigger as much, but that memory must be freed manualy.


### Pros
* Faster memory I/O operations
* Endianness type support
* Less GC triggers
* Interface for custom type Read/Write

### Cons
* Unsafe context
* Less security checks
* Manual memory allocation/free

## How to use
Extend [UMemoryStream](https://github.com/Kahath/UMemory/blob/master/UMemory/Unmanaged/Stream/Core/UMemoryStream.cs) type and create new instance of it.


## License
Product is licensed by The MIT License ([MIT](https://github.com/Kahath/UMemory/blob/master/LICENSE)). See LICENSE for more details.
