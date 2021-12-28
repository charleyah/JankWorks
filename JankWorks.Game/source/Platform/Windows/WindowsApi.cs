using System;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;

namespace JankWorks.Game.Platform.Windows
{
    internal enum MMRESULT : uint
    {
        MMSYSERR_NOERROR = 0,
        MMSYSERR_ERROR = 1,
        MMSYSERR_BADDEVICEID = 2,
        MMSYSERR_NOTENABLED = 3,
        MMSYSERR_ALLOCATED = 4,
        MMSYSERR_INVALHANDLE = 5,
        MMSYSERR_NODRIVER = 6,
        MMSYSERR_NOMEM = 7,
        MMSYSERR_NOTSUPPORTED = 8,
        MMSYSERR_BADERRNUM = 9,
        MMSYSERR_INVALFLAG = 10,
        MMSYSERR_INVALPARAM = 11,
        MMSYSERR_HANDLEBUSY = 12,
        MMSYSERR_INVALIDALIAS = 13,
        MMSYSERR_BADDB = 14,
        MMSYSERR_KEYNOTFOUND = 15,
        MMSYSERR_READERROR = 16,
        MMSYSERR_WRITEERROR = 17,
        MMSYSERR_DELETEERROR = 18,
        MMSYSERR_VALNOTFOUND = 19,
        MMSYSERR_NODRIVERCB = 20,
        WAVERR_BADFORMAT = 32,
        WAVERR_STILLPLAYING = 33,
        WAVERR_UNPREPARED = 34
    }

    internal struct TIMECAPS
    {
        public uint wPeriodMin;
        public uint wPeriodMax;
    }

    internal sealed class WindowsApi : PlatformApi
    {
        [DllImport("Winmm.dll"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe MMRESULT timeGetDevCaps(TIMECAPS* ptc, uint cbtc);

        [DllImport("Winmm.dll"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe MMRESULT timeBeginPeriod(uint uPeriod);

        [DllImport("Winmm.dll"), SuppressUnmanagedCodeSecurity]
        private static extern unsafe MMRESULT timeEndPeriod(uint uPeriod);

        private readonly TIMECAPS caps;

        public WindowsApi()
        {
            var tc = default(TIMECAPS);
            unsafe
            {
                timeGetDevCaps(&tc, (uint)sizeof(TIMECAPS));
            }
            this.caps = tc;
        }

        public override void Sleep(TimeSpan time)
        {
            var min = this.caps.wPeriodMin;

            if(time.TotalMilliseconds > min)
            {
                timeBeginPeriod(min);
                Thread.Sleep(time);
                timeEndPeriod(min);
            }            
        }
    }
}