# JankWorks (API)

JankWorks is a set of APIs that provide primitives for a video game or multi-media application. The API is broken up into 3 modules (namespaces) and are implemented by 3rd party drivers. See the Triangle project and the Test project for examples of using JankWorks API.

## JankWorks.Interface

Interface is an API for windowing and handling user input. It has 2 drivers for windowing and retrieving monitor information. See JankWorks.Glfw for an implementation example.

## JankWorks.Graphics

Graphics is an API for utilising hardware accelerated graphics designed around graphics shader pipeline. Graphics has separate 3 drivers; one for image loading, one for font loading and lastly a driver for graphics rendering. See JankWorks.OpenGL, JankWorks.DotNet and JankWorks.FreeType for examples of implementing drivers.

## JankWorks.Audio

Audio is an API for playing positional audio and has one driver. See JankWorks.OpenAL for an driver example.

## Supporting Namespaces

In addition to the abstract APIs, JankWorks also contains namespaces for supporting library code. 

- `Core`		contains public common elements across all 3 modules.
- `Util`		contains non-public supporting code.
- `Platform`	contains supporting code that is target platform specific