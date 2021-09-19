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
}