# UMemory

[![Join Discord](https://img.shields.io/badge/discord-join-7289DA.svg)](https://discord.gg/9AyZ9Ag) [![Build status](https://ci.appveyor.com/api/projects/status/k09ihk4xgvonkeyw?svg=true)](https://ci.appveyor.com/project/Kahath/umemory)

UMemory is lightweight library written in unsafe context for binary read/write operations in memory. It should outperform, but cannot replace .NET BinaryReader/BinaryWriter. Best use of this library is for read/write operations where array boundaries and operations are known.


## How to use
Extend [UMemoryStream](https://github.com/Kahath/UMemory/blob/master/UMemory/Unmanaged/Stream/Core/UMemoryStream.cs) type and create new instance of it.


## License
Product is licensed by The MIT License ([MIT](https://github.com/Kahath/UMemory/blob/master/LICENSE)). See LICENSE for more details.
