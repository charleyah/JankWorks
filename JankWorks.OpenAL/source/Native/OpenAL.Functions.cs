using System;
using System.Numerics;
using System.Security;
using System.Runtime.InteropServices;

using JankWorks.Util;
using JankWorks.Platform;

namespace JankWorks.Drivers.OpenAL.Native
{
    public static class Functions
    {
        public static class Delegates
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr alcOpenDevice(CString devicename);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate bool alcCloseDevice(IntPtr device);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate IntPtr alcCreateContext(IntPtr device, int* attrlist);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate bool alcMakeContextCurrent(IntPtr context);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate IntPtr alcGetCurrentContext();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate bool alcDestroyContext(IntPtr context);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate ALError alGetError();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alDistanceModel(ALDistanceModel model);



            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alListenerf(ALListenerf parm, float value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alListener3f(ALListener3f parm, float x, float y, float z);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alListenerfv(ALListenerfv parm, float* values);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetListenerf(ALListenerf parm, float* value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetListener3f(ALListener3f parm, float* x, float* y, float* z);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetListenerfv(ALListenerfv parm, float* values);



            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGenSources(int count, uint* sources);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alDeleteSources(int count, uint* sources);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate bool alIsSource(uint source);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourcef(uint source, ALSourcef parm, float value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSource3f(uint source, ALSource3f parm, float x, float y, float z);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alSourcefv(uint source, ALSource3f parm, float* values);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourcei(uint source, ALSourcei parm, int value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetSourcef(uint source, ALSourcef parm, float* value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetSource3f(uint source, ALSource3f parm, float* x, float* y, float* z);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetSourcefv(uint source, ALSource3f parm, float* values);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetSourcei(uint source, ALGetSourcei parm, int* value);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePlay(uint source);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourcePause(uint source);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourceStop(uint source);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate void alSourceRewind(uint source);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alSourcePlayv(int count, uint* sources);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alSourcePausev(int count, uint* sources);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alSourceStopv(int count, uint* sources);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alSourceRewindv(int count, uint* sources);


            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGenBuffers(int count, uint* buffers);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alDeleteBuffers(int count, uint* buffers);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate bool alIsBuffer(uint buffer);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alBufferData(uint buffer, ALFormat format, void* data, int size, int freq);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public unsafe delegate void alGetBufferi(uint buffer, ALBufferi parm, int* value);

        }

        public static Delegates.alcOpenDevice alcOpenDevice;
        public static Delegates.alcCloseDevice alcCloseDevice;

        public static Delegates.alcCreateContext alcCreateContext;
        public static Delegates.alcMakeContextCurrent alcMakeContextCurrent;
        public static Delegates.alcGetCurrentContext alcGetCurrentContext;
        public static Delegates.alcDestroyContext alcDestroyContext;

        public static Delegates.alGetError alGetError;
        public static Delegates.alDistanceModel alDistanceModel;

        public static Delegates.alListenerf alListenerf;
        public static Delegates.alListener3f alListener3f;
        public static Delegates.alListenerfv alListenerfv;
        public static Delegates.alGetListenerf alGetListenerf;
        public static Delegates.alGetListener3f alGetListener3f;
        public static Delegates.alGetListenerfv alGetListenerfv;

        public static Delegates.alGenSources alGenSources;
        public static Delegates.alDeleteSources alDeleteSources;
        public static Delegates.alIsSource alIsSource;

        public static Delegates.alSourcef alSourcef;
        public static Delegates.alSource3f alSource3f;
        public static Delegates.alSourcefv alSourcefv;
        public static Delegates.alSourcei alSourcei;
        public static Delegates.alGetSourcef alGetSourcef;
        public static Delegates.alGetSource3f alGetSource3f;
        public static Delegates.alGetSourcefv alGetSourcefv;
        public static Delegates.alGetSourcei alGetSourcei;

        public static Delegates.alSourcePlay alSourcePlay;
        public static Delegates.alSourcePause alSourcePause;
        public static Delegates.alSourceStop alSourceStop;
        public static Delegates.alSourceRewind alSourceRewind;        
        public static Delegates.alSourcePlayv alSourcePlayv;
        public static Delegates.alSourcePausev alSourcePausev;        
        public static Delegates.alSourceStopv alSourceStopv;        
        public static Delegates.alSourceRewindv alSourceRewindv;
        
        public static Delegates.alGenBuffers alGenBuffers;
        public static Delegates.alDeleteBuffers alDeleteBuffers;
        public static Delegates.alIsBuffer alIsBuffer;
        public static Delegates.alBufferData alBufferData;
        public static Delegates.alGetBufferi alGetBufferi;

        public static volatile LibraryLoader loader;

        public static void Init()
        {
            if (loader == null)
            {
                lock (typeof(Functions))
                {
                    if (loader == null)
                    {
                        loader = LoadFunctions();
                    }
                }
            }
        }

        private static LibraryLoader LoadFunctions()
        {
            var env = SystemEnvironment.Current;

            var loader = env.OS switch
            {
                SystemPlatform.Windows => env.LoadLibrary("soft_oal.dll", "openal32.dll"),
                SystemPlatform.MacOS => env.LoadLibrary("/System/Library/Frameworks/OpenAL.framework/OpenAL"),
                SystemPlatform.Linux => env.LoadLibrary("libopenal.so.1"),
                _ => throw new NotSupportedException()
            };
               
            Functions.alcOpenDevice = LoadFunction<Delegates.alcOpenDevice>(loader);
            Functions.alcCloseDevice = LoadFunction<Delegates.alcCloseDevice>(loader);

            Functions.alcCreateContext = LoadFunction<Delegates.alcCreateContext>(loader);
            Functions.alcMakeContextCurrent = LoadFunction<Delegates.alcMakeContextCurrent>(loader);
            Functions.alcGetCurrentContext = LoadFunction<Delegates.alcGetCurrentContext>(loader);
            Functions.alcDestroyContext = LoadFunction<Delegates.alcDestroyContext>(loader);

            Functions.alGetError = LoadFunction<Delegates.alGetError>(loader);
            Functions.alDistanceModel = LoadFunction<Delegates.alDistanceModel>(loader);

            Functions.alListenerf = LoadFunction<Delegates.alListenerf>(loader);
            Functions.alListener3f = LoadFunction<Delegates.alListener3f>(loader);
            Functions.alListenerfv = LoadFunction<Delegates.alListenerfv>(loader);
            Functions.alGetListenerf = LoadFunction<Delegates.alGetListenerf>(loader);
            Functions.alGetListener3f = LoadFunction<Delegates.alGetListener3f>(loader);
            Functions.alGetListenerfv = LoadFunction<Delegates.alGetListenerfv>(loader);

            Functions.alGenSources = LoadFunction<Delegates.alGenSources>(loader);
            Functions.alDeleteSources = LoadFunction<Delegates.alDeleteSources>(loader);
            Functions.alIsSource = LoadFunction<Delegates.alIsSource>(loader);

            Functions.alSourcef = LoadFunction<Delegates.alSourcef>(loader);
            Functions.alSource3f = LoadFunction<Delegates.alSource3f>(loader);
            Functions.alSourcefv = LoadFunction<Delegates.alSourcefv>(loader);
            Functions.alSourcei = LoadFunction<Delegates.alSourcei>(loader);
            Functions.alGetSourcef = LoadFunction<Delegates.alGetSourcef>(loader);
            Functions.alGetSource3f = LoadFunction<Delegates.alGetSource3f>(loader);
            Functions.alGetSourcefv = LoadFunction<Delegates.alGetSourcefv>(loader);
            Functions.alGetSourcei = LoadFunction<Delegates.alGetSourcei>(loader);

            Functions.alSourcePlay = LoadFunction<Delegates.alSourcePlay>(loader);
            Functions.alSourcePause = LoadFunction<Delegates.alSourcePause>(loader);
            Functions.alSourceStop = LoadFunction<Delegates.alSourceStop>(loader);
            Functions.alSourceRewind = LoadFunction<Delegates.alSourceRewind>(loader);
            Functions.alSourcePlayv = LoadFunction<Delegates.alSourcePlayv>(loader);
            Functions.alSourcePausev = LoadFunction<Delegates.alSourcePausev>(loader);
            Functions.alSourceStopv = LoadFunction<Delegates.alSourceStopv>(loader);
            Functions.alSourceRewindv = LoadFunction<Delegates.alSourceRewindv>(loader);

            Functions.alGenBuffers = LoadFunction<Delegates.alGenBuffers>(loader);
            Functions.alDeleteBuffers = LoadFunction<Delegates.alDeleteBuffers>(loader);
            Functions.alIsBuffer = LoadFunction<Delegates.alIsBuffer>(loader);
            Functions.alBufferData = LoadFunction<Delegates.alBufferData>(loader);
            Functions.alGetBufferi = LoadFunction<Delegates.alGetBufferi>(loader);

            return loader;
        }

        private static T LoadFunction<T>(LibraryLoader loader)
        {
            var fptr = loader.LoadFunction(typeof(T).Name);
            return Marshal.GetDelegateForFunctionPointer<T>(fptr);
        }
    }
}