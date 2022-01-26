# JankWorks.Game

JankWorks Game is a framework intended to be a basis for developing a video game application similar to other frameworks such as XNA/MonoGame. The framework is built upon the JankWorks API and provide structure with client-server architecture in mind. See [Pong project](../Examples/Pong) as an example of using JankWorks Game Framework.

## Usage & Status

As a Framework, JankWorks is still in development and only tested on Windows so your mileage may vary. The framework provides a structure around creating game objects that either run on a client or a host and utilise a message-passing API to communicate between them. At time of writing, applications can run without a host or with a "offline" host running its own dedicated thread. A network host is currently in development.

## Terminology

### Application

All applications made with JankWorks Game Framework start with the Application class. This class through derived implementations provides application wide information for the framework including the app name, where to locate assets, what JankWorks drivers to use, configure aspects of the framework and register available scenes. When starting the application, the `Application.Run` and `Application.RunWithoutHost` functions are used to specify what derived application class to execute.

### Scene

A scene is a class that defines and initialises game objects to be run by either the client or host if one is present. A example of a scene could be a main menu, a player lobby or a match of the game. Scenes have a complex initialisation process and this is covered in *Scenes In Depth*.

### Game Object

A game object is a arbitrary user defined object that implements at least one of the many interfaces that allow it to be registered and executed by a client or a host depending on the objects purpose. Game objects should be seen as a system or a module of a system that are responsible for logical parts of a game rather than a individual game entity. Any game object that's registered on the client will run on the main UI thread and should take this into consideration if executing any long blocking tasks.

### Client

The client is in essence what runs the application and is always required. The client processes user input, handles windowing, plays audio and renders graphics. The Client will execute any client-side game objects that are registered by the current scene.

### Host

The host is an optional element that's independent of the client and is intended for executing game logic and simulation. The host achieves this by executing any host-side game objects registered by the current scene.

### Channel

A channel is a one directional message pipe used by game objects to communicate across client/host boundaries. A channel's direction is either up (client-to-host) or down (host-to-client) and can only have one object receiving and one object sending messages. The framework is also limited to a maximum of 256 different channels.

## Scenes In Depth

Scenes are the basis for creating a 2D or 3D environment. A scene can be as simple as a games main menu, or as complex as a match of the game. Scenes achieve this is through initialising arbitrary game objects that implement various interfaces to fulfil their role and purpose for a given scene. For example, a match scene could have a `WorldRenderer` object that implements `IRenderable` and purpose is to draw the game world. Scenes construct these game objects and then register them with either the client or a host using `RegisterClientObject` and `RegisterHostObject` methods respectively. 

Where game objects should be constructed and registered in a scene class depends heavily on the objects responsibility and the scene itself. All objects should be well defined to be a client or a host object. Any object that appears to have shared responsibility across client and host should be split into two separate objects and use channels to communicate. For example, a scene could have a `World` host object containing world data and a `WorldRenderer` client object that requests and receives data from `World` object via channels to render the world on the client. Another important note is the order in which objects are registered is also order they get processed which is vitally important for any renderable objects. Once objects are registered they can't be unregistered or have the order they are processed changed.

### Scene Initialisation

Scene initialisation will be broken down into different scenarios depending on what host is being used if any. The tables below are in order in which methods are executed. They also specify if calls to `RegisterClientObject` and `RegisterHostObject` are allowed.

**Scene Initialisation Order With Client And No Host**

| Method                        | Side        | Register Client Objects | Register Host Objects |
| ----------------------------- | ----------- | ----------------------- | --------------------- |
| `.ctor`                       | Application | No                      | No                    |
| `PreInitialise`               | Application | No                      | No                    |
| `Initialise`                  | Application | No                      | No                    |
| `ClientInitialise`            | Client      | **Yes**                 | No                    |
| `InitialiseGraphicsResources` | Client      | No                      | No                    |
| `InitialiseSoundResources`    | Client      | No                      | No                    |
| `ClientInitialised`           | Client      | No                      | No                    |
| `Initialised`                 | Application | No                      | No                    |
| `SubscribeInputs`             | Client      | No                      | No                    |

**Scene Initialisation Order With Client And Local Host**

| Method                        | Side        | Register Client Objects | Register Host Objects |
| ----------------------------- | ----------- | ----------------------- | --------------------- |
| `.ctor`                       | Application | No                      | No                    |
| `PreInitialise`               | Application | No                      | No                    |
| `Initialise`                  | Application | No                      | No                    |
| `HostInitialise`              | Host        | No                      | **Yes**               |
| `SharedInitialise`            | Shared      | **Yes**                 | **Yes**               |
| `HostInitialised`             | Host        | No                      | No                    |
| `SharedInitialised`           | Shared      | No                      | No                    |
| `ClientInitialise`            | Client      | **Yes**                 | No                    |
| `InitialiseChannels`          | Shared      | No                      | No                    |
| `InitialiseGraphicsResources` | Client      | No                      | No                    |
| `InitialiseSoundResources`    | Client      | No                      | No                    |
| `ClientInitialised`           | Client      | No                      | No                    |
| `UpSynchronise`               | Shared      | No                      | No                    |
| `DownSynchronise`             | Shared      | No                      | No                    |
| `Initialised`                 | Application | No                      | No                    |
| `SubscribeInputs`             | Client      | No                      | No                    |

**Scene Initialisation Order With Client And Remote Host**

| Method                        | Side        | Register Client Objects | Register Host Objects |
| ----------------------------- | ----------- | ----------------------- | --------------------- |
| `.ctor`                       | Application | No                      | No                    |
| `PreInitialise`               | Application | No                      | No                    |
| `Initialise`                  | Application | No                      | No                    |
| `SharedInitialise`            | Shared      | **Yes**                 | **Yes**               |
| `SharedInitialised`           | Shared      | No                      | No                    |
| `ClientInitialise`            | Client      | **Yes**                 | No                    |
| `InitialiseChannels`          | Shared      | No                      | No                    |
| `InitialiseGraphicsResources` | Client      | No                      | No                    |
| `InitialiseSoundResources`    | Client      | No                      | No                    |
| `ClientInitialised`           | Client      | No                      | No                    |
| `UpSynchronise`               | Shared      | No                      | No                    |
| `DownSynchronise`             | Shared      | No                      | No                    |
| `Initialised`                 | Application | No                      | No                    |
| `SubscribeInputs`             | Client      | No                      | No                    |

## Async Support

JankWorks Game Framework has active support for `async await`. The initialisation steps of a scene and active game objects both support `async` with a few exceptions. Methods that support `async` can be marked `async` and or call `async void` methods. Any method that doesn't support `async` are expected to be executed synchronously and block on any tasks they initiate before returning.

### Scene Methods

Majority of scene initialisation and disposal methods support `async`. The methods that aren't supported are pre/post initialise methods, data sync methods and input subscription methods.

| Method                        | Support Async |
| ----------------------------- | ------------- |
| `Initialise`                  | Yes           |
| `HostInitialise`              | Yes           |
| `SharedInitialise`            | Yes           |
| `ClientInitialise`            | Yes           |
| `InitialiseGraphicsResources` | Yes           |
| `InitialiseSoundResources`    | Yes           |
| `InitialiseChannels`          | Yes           |
| `Dispose`                     | Yes           |
| `HostDispose`                 | Yes           |
| `SharedDispose`               | Yes           |
| `ClientDispose`               | Yes           |
| `DisposeSoundResources`       | Yes           |
| `DisposeGraphicsResources`    | Yes           |
| `PreInitialise`               | No            |
| `PreDispose`                  | No            |
| `Initialised`                 | No            |
| `HostInitialised`             | No            |
| `SharedInitialised`           | No            |
| `ClientInitialised`           | No            |
| `UpSynchronise`               | No            |
| `DownSynchronise`             | No            |
| `SubscribeInputs`             | No            |
| `UnsubscribeInputs`           | No            |

### Game Object Methods

Methods provided by interfaces for game objects likewise also support `async`. The exceptions to this rule are parallel processing, data sync and input subscription methods.

| Method                        | Interface             | Support Async |
| ----------------------------- | --------------------- | ------------- |
| `Dispose`                     | `IDisposable`         | Yes           |
| `InitialiseResources`         | `IResource`           | Yes           |
| `DisposeResources`            | `IResource`           | Yes           |
| `InitialiseGraphicsResources` | `IGraphicsResource`   | Yes           |
| `DisposeGraphicsResources`    | `IGraphicsResource`   | Yes           |
| `InitialiseSoundResources`    | `ISoundResource`      | Yes           |
| `DisposeSoundResources`       | `ISoundResource`      | Yes           |
| `InitialiseChannels`          | `IDispatchable`       | Yes           |
| `UpSynchronise`               | `IDispatchable`       | No            |
| `DownSynchronise`             | `IDispatchable`       | No            |
| `SubscribeInputs`             | `IInputListener`      | No`*`         |
| `UnsubscribeInputs`           | `IInputListener`      | No`*`         |
| `Tick`                        | `ITickable`           | Yes`**`       |
| `Update`                      | `IUpdatable`          | Yes`**`       |
| `Render`                      | `IRenderable`         | Yes`**`       |
| `ForkTick`                    | `IParallelTickable`   | No            |
| `JoinTick`                    | `IParallelTickable`   | No            |
| `ForkUpdate`                  | `IParallelUpdatable`  | No            |
| `JoinUpdate`                  | `IParallelUpdatable`  | No            |
| `ForkRender`                  | `IParallelRenderable` | No            |
| `JoinRender`                  | `IParallelRenderable` | No            |

`*` - The `SubscribeInputs` and `UnsubscribeInputs` methods do not support `async`, however any method they subscribe to listen for user input do support `async`. For example a method subscribed to a key press can be modified with `async`.

`**` - The `Tick`, `Update` and `Render` methods require explicit configuration to enable `async` support. See *Async & Interval Behaviour*.

### Async & Interval Behaviour

Game objects with `Tick`, `Update` or `Render` methods have some special rules regarding asynchronous execution. These methods are considered *Interval Methods* and their respective interface has a default property used to determine how the framework should handle asynchronous execution every tick/frame interval. The configurable Interval behaviour is as follows:

- `NoAsync` -  Method is invoked every interval and does not support async method invocation. This is the default behaviour.
- `Asynchronous` -  Method is invoked or resumes awaited tasks on interval. Suitable for most `async` uses.
- `Synchronous` -  Method is invoked every interval. If the method is async it will block until all awaited tasks are completed.
- `Overlapped` -  Method is invoked every interval. If the method is async it can be invoked multiple times while awaiting tasks.

```csharp
class FooBarThinker : ITickable
{
    // enable async for Tick method
    ITickable.TickInterval => IntervalBehaviour.Asynchronous; 
        
    public async void Tick(ulong tick, GameTime time)
    {
        // every tick will either invoke this method or resume awaited tasks
    }
}
```

Its important to only mark the interval method with `async` or call `async void` methods if the Interval property has been set to a value other than `NoAsync`. The example below is considered a bug because the `Update` will be called every update interval and any completed tasks running asynchronously will resume on a different thread to the game loop and likely not in the order game objects are processed either.

```csharp
class FooBarUpdater : IUpdatable
{        
    // This is the same as having the UpdateInterval code omitted and is not valid for a async Update method
    IUpdatable.UpdateInterval => IntervalBehaviour.NoAsync; 
    
    public async void Update(GameTime time)
    {
         // any await statements will resume out of order and on any thread
    }
}
```

Finally, its also discouraged to use `ConfigureAwait(false)` on awaited tasks as yielding back to the game loop thread is important to retain order game objects are processed. Where graphics is concerned, its also important to yield back to the game loop thread if the graphics API has context to track like OpenGL does.

