# Decentraland Scene Runtime Environment
Unity of Decentraland scene runtime environment

The program embeds a JavaScript runtime basic functions to execute them in a sandboxed environment written in Unity ECS application.
It provides access to functionality outside the sandbox with a bridge between JavaScript functions and C# environment. 
It is a simplified version of a minimal Decentraland scene runtime.


# Features

## Basic functionality

1. Create a game environment with ECS-like functionality (adding entities, attaching components, and updating data).
2. Load the test file in the sandboxed environment 
3. Establish a bidirectional channel between the game and sanboxed environments.
4. Correctly render and animate a cube according to the script’s instructions.
5. Provide keyboard input from the game engine to the script.
6. Provide keyboard input from the game engine to the script.

# Usage
run from the root of your project:
``` $ ./bin/run ```

## Requirements 
The project run in a Windows 10 shell environment with dotnet and .NET 6 installed.

# Technical description - decision process

The following libraries were considered for the implementation of JavaScript in .net 6 environment:

* [Jering.Javascript.NodeJS](https://github.com/JeringTech/Javascript.NodeJS)
* [ChakraCore](https://github.com/chakra-core/ChakraCore)
* [Microsoft ClearScript.V8](https://github.com/Microsoft/ClearScript)
* [Jint](https://github.com/sebastienros/jint)
* [Jurassic](https://github.com/paulbartrum/jurassic)
* [MSIE JavaScript Engine for .NET](https://github.com/Taritsyn/MsieJavaScriptEngine)
* [NiL.JS](https://github.com/nilproject/NiL.JS)
* [VroomJs](https://github.com/pauldotknopf/vroomjs-core)

In addition, a following library was considered [JavaScriptEngineSwitcher](https://github.com/Taritsyn/JavaScriptEngineSwitcher)

Finally, [Microsoft ClearScript.V8](https://github.com/Microsoft/ClearScript) was selected due to simplicity, support and rich range of functions.


# License
[Apache License Version 2.0](http://github.com/Taritsyn/JavaScriptEngineSwitcher/blob/master/LICENSE.txt)