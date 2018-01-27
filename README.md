# KNet

[![Join Discord](https://img.shields.io/badge/discord-join-7289DA.svg)](https://discord.gg/5kK9eav) [![Build status](https://ci.appveyor.com/api/projects/status/oyxdordfnyuwedhv?svg=true)](https://ci.appveyor.com/project/Kahath/knet)

KNet is simple, lightweight and extensible TCP/IP server framework. It uses two third party libraries ([UMemory](https://github.com/Kahath/UMemory) and DILibrary) and EntityFramework as ORM. [UMemory](https://github.com/Kahath/UMemory) is unsafe context C# .NET 4.6 library used for reading and writing buffers to avoid some unnecessary calls from .NET GC. It has custom length format for arrays and strings. DILibrary is just lightweight dependency injection library.

</br>

## Getting started

### Requirements
* [Visual studio 2017](https://www.visualstudio.com/vs/)
* [.NET Framework 4.6+](https://www.microsoft.com/en-us/download/details.aspx?id=48130)
* [MS SQL Server 2014+](https://www.microsoft.com/en-us/download/details.aspx?id=42299)

### Setting up Database
First build and publish [KNetDatabase](https://github.com/Kahath/KNet/tree/master/KNetDatabase) onto MSSQL database engine instance. Then execute all scripts in [KNetDatabase/Data/KNet](https://github.com/Kahath/KNet/tree/master/KNetDatabase/Data/KNet). 

### Setting up server
After database is successfully set up, build [KNetFramework](https://github.com/Kahath/KNet/tree/master/KNetFramework). If release build configuration is selected, in bin/Release should be all the required .dll files and configuration file in Config folder. Create new project (console or whatever) and add reference of previously built KNetFramework .dll files. To start server create instance of [KNetServer](https://github.com/Kahath/KNet/blob/master/KNetFramework/KNetServer.cs) type and call Start() method. Make sure new thread is created if application doesn't serve only as server because it will block thread. Also make sure that [KNetConfig](https://github.com/Kahath/KNet/blob/master/KNetFramework/Configs/KNetConfig.xml) file is in folder where application is executed and is correctly configured.

### Extend it
Now, after server is successfully configured, the only thing left is to extend it. There are two main attributes ([CommandAttribute](https://github.com/Kahath/KNet/blob/master/KNetFramework/Attributes/Core/CommandAttribute.cs) And [OpcodeAttribute](https://github.com/Kahath/KNet/blob/master/KNetFramework/Attributes/Core/OpcodeAttribute.cs)) CommandAttribute is used for adding new commands to server. Commands are invoked via [CommandManager](https://github.com/Kahath/KNet/blob/master/KNetFramework/Managers/Core/CommandManager.cs) InvokeCommand method. All commands must derive from [CommandHandlerBase](https://github.com/Kahath/KNet/blob/master/KNetFramework/Commands/Base/CommandHandlerBase.cs) and are stored in database on [AssemblyManager](https://github.com/Kahath/KNet/blob/master/KNetFramework/Managers/Core/AssemblyManager.cs) initialization. OpcodeAttribute is used for executing network packets. Packets are distinct by OpcodeAttribute Opcode property. When packet arrives, method attribute with same opcode as in packet header is executed. It is important to know packet structure. Note that all packets and commands can be loaded from other assemblies. Framework will load all assemblies from folder whose path is in configuration file under key "assemblypath" and create packet methods and commands that are in assemblies. Note that all assemblies must also have above attributes added in AssemblyInfo.

</br>

## To do
* Compression
* Cryptography
* UDP support
* Overhaul PacketLogging
* Optimization... Optimization... Optimization...

</br>

## Ideas, references and influences
* Initial idea - [Arctium Emulation WoW Core](https://github.com/Arctium-Emulation/WoW-Core)
* [Signaler.cs](https://github.com/Kahath/KNet/blob/master/KNetFramework/Async/Semaphore/Signaler.cs) - [Building Async Coordination Primitives, Part 5: AsyncSemaphore](https://blogs.msdn.microsoft.com/pfxteam/2012/02/12/building-async-coordination-primitives-part-5-asyncsemaphore/)
* Network related stuff - [C# SocketAsyncEventArgs High Performance Socket Code](https://www.codeproject.com/Articles/83102/C-SocketAsyncEventArgs-High-Performance-Socket-Cod)
* Packet handling - [Arctium Emulation WoW Core](https://github.com/Arctium-Emulation/WoW-Core) (Earlier version)
* Commands handling - [TrinityCore](https://github.com/TrinityCore/TrinityCore)
* Entity Framework - [Ivan](https://github.com/zagorec92)
* Dependency Injection - [MSCommunity](http://www.mscommunity.hr/event/dependency-injection-win10/362)

</br>

## License
Product is licensed by The MIT License ([MIT](https://github.com/Kahath/KNet/blob/master/LICENSE)). See LICENSE for more details.
