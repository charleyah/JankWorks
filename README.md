# JankWorks

JankWorks is an API and framework intending to be suitable for developing games or multi-media applications similar to other frameworks such as XNA/MonoGame. JankWorks is built with C# 9.0 and only supports .NET 5 onwards. The project overall is still in development and currently only tested for Windows so mileage may vary. The aim is to be capable of small scale 2D game development with a ease into making multiplayer and potentially 3D in the future.

## JankWorks API

The [JankWorks](JankWorks) project is a API providing the basic application requirements including windowing, user input, positional audio and hardware accelerated graphics. The API is mostly abstract and supports different implementations provided by "Drivers".

## JankWorks Game Framework

The [JankWorks.Game](JankWorks.Game) project is the framework built on top of the JankWorks API to aid in game development by providing a skeleton application and structure to build from mostly inspired by but not limited to XNA framework. 

## Projects

| Project            | Description                                   |
| ------------------ | --------------------------------------------- |
| JankWorks          | API                                           |
| JankWorks.Game     | Game Framework                                |
| JankWorks.FreeType | FreeType driver that implements font loading  |
| JankWorks.Glfw     | Glfw driver that implements window management |
| JankWorks.OpenGL   | OpenGL driver that implements graphics API    |
| JankWorks.OpenAL   | OpenAL driver that implements audio API       |
| JankWorks.DotNet   | .NET driver that implements Image loading     |

