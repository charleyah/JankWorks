using System;
using System.Runtime.CompilerServices;

using JankWorks.Util;
using JankWorks.Platform;

namespace JankWorks.Drivers.OpenAL.Native
{
    static unsafe class Functions
    {
        private static delegate* unmanaged[Cdecl]<CString, IntPtr> alcOpenDevicePtr;
        private static delegate* unmanaged[Cdecl]<IntPtr, bool> alcCloseDevicePtr;
        private static delegate* unmanaged[Cdecl]<IntPtr, int*, IntPtr> alcCreateContextPtr;
        private static delegate* unmanaged[Cdecl]<IntPtr, bool> alcMakeContextCurrentPtr;
        private static delegate* unmanaged[Cdecl]<IntPtr> alcGetCurrentContextPtr;
        private static delegate* unmanaged[Cdecl]<IntPtr, bool> alcDestroyContextPtr;
        private static delegate* unmanaged[Cdecl]<ALError> alGetErrorPtr;
        private static delegate* unmanaged[Cdecl]<ALDistanceModel, void> alDistanceModelPtr;
        private static delegate* unmanaged[Cdecl]<ALListenerf, float, void> alListenerfPtr;
        private static delegate* unmanaged[Cdecl]<ALListener3f, float, float, float, void> alListener3fPtr;
        private static delegate* unmanaged[Cdecl]<ALListenerfv, float*, void> alListenerfvPtr;
        private static delegate* unmanaged[Cdecl]<ALListenerf, float*, void> alGetListenerfPtr;
        private static delegate* unmanaged[Cdecl]<ALListener3f, float*, float*, float*, void> alGetListener3fPtr;
        private static delegate* unmanaged[Cdecl]<ALListenerfv, float*, void> alGetListenerfvPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alGenSourcesPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alDeleteSourcesPtr;
        private static delegate* unmanaged[Cdecl]<uint, bool> alIsSourcePtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSourcef, float, void> alSourcefPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSource3f, float, float, float, void> alSource3fPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, void> alSourcefvPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSourcei, int, void> alSourceiPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSourcef, float*, void> alGetSourcefPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, float*, float*, void> alGetSource3fPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, void> alGetSourcefvPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALGetSourcei, int*, void> alGetSourceiPtr;
        private static delegate* unmanaged[Cdecl]<uint, void> alSourcePlayPtr;
        private static delegate* unmanaged[Cdecl]<uint, void> alSourcePausePtr;
        private static delegate* unmanaged[Cdecl]<uint, void> alSourceStopPtr;
        private static delegate* unmanaged[Cdecl]<uint, void> alSourceRewindPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alSourcePlayvPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alSourcePausevPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alSourceStopvPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alSourceRewindvPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alGenBuffersPtr;
        private static delegate* unmanaged[Cdecl]<int, uint*, void> alDeleteBuffersPtr;
        private static delegate* unmanaged[Cdecl]<uint, bool> alIsBufferPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALFormat, void*, int, int, void> alBufferDataPtr;
        private static delegate* unmanaged[Cdecl]<uint, ALBufferi, int*, void> alGetBufferiPtr;

        public static LibraryLoader loader;

        public static void Init()
        {
            var env = SystemEnvironment.Current;

            Functions.loader = env.OS switch
            {
                SystemPlatform.Windows => env.LoadLibrary("soft_oal.dll", "openal32.dll"),
                SystemPlatform.MacOS => env.LoadLibrary("/System/Library/Frameworks/OpenAL.framework/OpenAL"),
                SystemPlatform.Linux => env.LoadLibrary("libopenal.so.1"),
                _ => throw new NotSupportedException()
            };
            
            Functions.alcOpenDevicePtr = (delegate* unmanaged[Cdecl]<CString, IntPtr>)Functions.LoadFunction("alcOpenDevice");
            Functions.alcCloseDevicePtr = (delegate* unmanaged[Cdecl]<IntPtr, bool>)Functions.LoadFunction("alcCloseDevice");
            Functions.alcCreateContextPtr = (delegate* unmanaged[Cdecl]<IntPtr, int*, IntPtr>)Functions.LoadFunction("alcCreateContext");
            Functions.alcMakeContextCurrentPtr = (delegate* unmanaged[Cdecl]<IntPtr, bool>)Functions.LoadFunction("alcMakeContextCurrent");
            Functions.alcGetCurrentContextPtr = (delegate* unmanaged[Cdecl]<IntPtr>)Functions.LoadFunction("alcGetCurrentContext");
            Functions.alcDestroyContextPtr = (delegate* unmanaged[Cdecl]<IntPtr, bool>)Functions.LoadFunction("alcDestroyContext");
            Functions.alGetErrorPtr = (delegate* unmanaged[Cdecl]<ALError>)Functions.LoadFunction("alGetError");
            Functions.alDistanceModelPtr = (delegate* unmanaged[Cdecl]<ALDistanceModel, void>)Functions.LoadFunction("alDistanceModel");
            Functions.alListenerfPtr = (delegate* unmanaged[Cdecl]<ALListenerf, float, void>)Functions.LoadFunction("alListenerf");
            Functions.alListener3fPtr = (delegate* unmanaged[Cdecl]<ALListener3f, float, float, float, void>)Functions.LoadFunction("alListener3f");
            Functions.alListenerfvPtr = (delegate* unmanaged[Cdecl]<ALListenerfv, float*, void>)Functions.LoadFunction("alListenerfv");
            Functions.alGetListenerfPtr = (delegate* unmanaged[Cdecl]<ALListenerf, float*, void>)Functions.LoadFunction("alGetListenerf");
            Functions.alGetListener3fPtr = (delegate* unmanaged[Cdecl]<ALListener3f, float*, float*, float*, void>)Functions.LoadFunction("alGetListener3f");
            Functions.alGetListenerfvPtr = (delegate* unmanaged[Cdecl]<ALListenerfv, float*, void>)Functions.LoadFunction("alGetListenerfv");
            Functions.alGenSourcesPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alGenSources");
            Functions.alDeleteSourcesPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alDeleteSources");
            Functions.alIsSourcePtr = (delegate* unmanaged[Cdecl]<uint, bool>)Functions.LoadFunction("alIsSource");
            Functions.alSourcefPtr = (delegate* unmanaged[Cdecl]<uint, ALSourcef, float, void>)Functions.LoadFunction("alSourcef");
            Functions.alSource3fPtr = (delegate* unmanaged[Cdecl]<uint, ALSource3f, float, float, float, void>)Functions.LoadFunction("alSource3f");
            Functions.alSourcefvPtr = (delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, void>)Functions.LoadFunction("alSourcefv");
            Functions.alSourceiPtr = (delegate* unmanaged[Cdecl]<uint, ALSourcei, int, void>)Functions.LoadFunction("alSourcei");
            Functions.alGetSourcefPtr = (delegate* unmanaged[Cdecl]<uint, ALSourcef, float*, void>)Functions.LoadFunction("alGetSourcef");
            Functions.alGetSource3fPtr = (delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, float*, float*, void>)Functions.LoadFunction("alGetSource3f");
            Functions.alGetSourcefvPtr = (delegate* unmanaged[Cdecl]<uint, ALSource3f, float*, void>)Functions.LoadFunction("alGetSourcefv");
            Functions.alGetSourceiPtr = (delegate* unmanaged[Cdecl]<uint, ALGetSourcei, int*, void>)Functions.LoadFunction("alGetSourcei");
            Functions.alSourcePlayPtr = (delegate* unmanaged[Cdecl]<uint, void>)Functions.LoadFunction("alSourcePlay");
            Functions.alSourcePausePtr = (delegate* unmanaged[Cdecl]<uint, void>)Functions.LoadFunction("alSourcePause");
            Functions.alSourceStopPtr = (delegate* unmanaged[Cdecl]<uint, void>)Functions.LoadFunction("alSourceStop");
            Functions.alSourceRewindPtr = (delegate* unmanaged[Cdecl]<uint, void>)Functions.LoadFunction("alSourceRewind");
            Functions.alSourcePlayvPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alSourcePlayv");
            Functions.alSourcePausevPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alSourcePausev");
            Functions.alSourceStopvPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alSourceStopv");
            Functions.alSourceRewindvPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alSourceRewindv");
            Functions.alGenBuffersPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alGenBuffers");
            Functions.alDeleteBuffersPtr = (delegate* unmanaged[Cdecl]<int, uint*, void>)Functions.LoadFunction("alDeleteBuffers");
            Functions.alIsBufferPtr = (delegate* unmanaged[Cdecl]<uint, bool>)Functions.LoadFunction("alIsBuffer");
            Functions.alBufferDataPtr = (delegate* unmanaged[Cdecl]<uint, ALFormat, void*, int, int, void>)Functions.LoadFunction("alBufferData");
            Functions.alGetBufferiPtr = (delegate* unmanaged[Cdecl]<uint, ALBufferi, int*, void>)Functions.LoadFunction("alGetBufferi");
        }

        private static void* LoadFunction(string name) => Functions.loader.LoadFunction(name).ToPointer();
        


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr alcOpenDevice(CString devicename)
        {
            return Functions.alcOpenDevicePtr(devicename);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool alcCloseDevice(IntPtr device)
        {
            return Functions.alcCloseDevicePtr(device);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr alcCreateContext(IntPtr device, int* attrlist)
        {
            return Functions.alcCreateContextPtr(device, attrlist);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool alcMakeContextCurrent(IntPtr context)
        {
            return Functions.alcMakeContextCurrentPtr(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr alcGetCurrentContext()
        {
            return Functions.alcGetCurrentContextPtr();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool alcDestroyContext(IntPtr context)
        {
            return Functions.alcDestroyContextPtr(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ALError alGetError()
        {
            return Functions.alGetErrorPtr();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alDistanceModel(ALDistanceModel model)
        {
            Functions.alDistanceModelPtr(model);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alListenerf(ALListenerf parm, float value)
        {
            Functions.alListenerfPtr(parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alListener3f(ALListener3f parm, float x, float y, float z)
        {
            Functions.alListener3fPtr(parm, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alListenerfv(ALListenerfv parm, float* values)
        {
            Functions.alListenerfvPtr(parm, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetListenerf(ALListenerf parm, float* value)
        {
            Functions.alGetListenerfPtr(parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetListener3f(ALListener3f parm, float* x, float* y, float* z)
        {
            Functions.alGetListener3fPtr(parm, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetListenerfv(ALListenerfv parm, float* values)
        {
            Functions.alGetListenerfvPtr(parm, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGenSources(int count, uint* sources)
        {
            Functions.alGenSourcesPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alDeleteSources(int count, uint* sources)
        {
            Functions.alDeleteSourcesPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool alIsSource(uint source)
        {
            return Functions.alIsSourcePtr(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcef(uint source, ALSourcef parm, float value)
        {
            Functions.alSourcefPtr(source, parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSource3f(uint source, ALSource3f parm, float x, float y, float z)
        {
            Functions.alSource3fPtr(source, parm, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcefv(uint source, ALSource3f parm, float* values)
        {
            Functions.alSourcefvPtr(source, parm, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcei(uint source, ALSourcei parm, int value)
        {
            Functions.alSourceiPtr(source, parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetSourcef(uint source, ALSourcef parm, float* value)
        {
            Functions.alGetSourcefPtr(source, parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetSource3f(uint source, ALSource3f parm, float* x, float* y, float* z)
        {
            Functions.alGetSource3fPtr(source, parm, x, y, z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetSourcefv(uint source, ALSource3f parm, float* values)
        {
            Functions.alGetSourcefvPtr(source, parm, values);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetSourcei(uint source, ALGetSourcei parm, int* value)
        {
            Functions.alGetSourceiPtr(source, parm, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcePlay(uint source)
        {
            Functions.alSourcePlayPtr(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcePause(uint source)
        {
            Functions.alSourcePausePtr(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourceStop(uint source)
        {
            Functions.alSourceStopPtr(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourceRewind(uint source)
        {
            Functions.alSourceRewindPtr(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcePlayv(int count, uint* sources)
        {
            Functions.alSourcePlayvPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourcePausev(int count, uint* sources)
        {
            Functions.alSourcePausevPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourceStopv(int count, uint* sources)
        {
            Functions.alSourceStopvPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alSourceRewindv(int count, uint* sources)
        {
            Functions.alSourceRewindvPtr(count, sources);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGenBuffers(int count, uint* buffers)
        {
            Functions.alGenBuffersPtr(count, buffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alDeleteBuffers(int count, uint* buffers)
        {
            Functions.alDeleteBuffersPtr(count, buffers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool alIsBuffer(uint buffer)
        {
            return Functions.alIsBufferPtr(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alBufferData(uint buffer, ALFormat format, void* data, int size, int freq)
        {
            Functions.alBufferDataPtr(buffer, format, data, size, freq);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void alGetBufferi(uint buffer, ALBufferi parm, int* value)
        {
            Functions.alGetBufferiPtr(buffer, parm, value);
        }

    }






    /*
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
    */
}