#pragma warning disable CS8618

using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using JankWorks.Platform;

namespace JankWorks.FreeType.Native
{
    public static class Functions
    {
        public static class Delegates
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Init_FreeType(out FT_Library library);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Done_FreeType(FT_Library library);


            [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_New_Face(FT_Library library, string filepathname, int face_index, out FT_Face aface);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_New_Memory_Face(FT_Library library, IntPtr file_base, int file_size, int face_index, out FT_Face aface);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Open_Face(FT_Library library, IntPtr args, int face_index, out FT_Face aface);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Done_Face(FT_Face face);


            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Set_Char_Size(FT_Face face, nint char_width, nint char_height, uint horz_resolution, uint vert_resolution);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Set_Pixel_Sizes(FT_Face face, uint pixel_width, uint pixel_height);


            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate FT_Error FT_Load_Char(FT_Face face, uint char_code, int load_flags);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate ulong FT_Get_First_Char(FT_Face face, ref uint index);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
            public delegate ulong FT_Get_Next_Char(FT_Face face, ulong char_code, ref uint index);
        }

        public static Delegates.FT_Init_FreeType FT_Init_FreeType;
        public static Delegates.FT_Done_FreeType FT_Done_FreeType;

        public static Delegates.FT_New_Face FT_New_Face;
        public static Delegates.FT_New_Memory_Face FT_New_Memory_Face;
        public static Delegates.FT_Open_Face FT_Open_Face;
        public static Delegates.FT_Done_Face FT_Done_Face;

        public static Delegates.FT_Set_Char_Size FT_Set_Char_Size;
        public static Delegates.FT_Set_Pixel_Sizes FT_Set_Pixel_Sizes;

        public static Delegates.FT_Load_Char FT_Load_Char;
        public static Delegates.FT_Get_First_Char FT_Get_First_Char;
        public static Delegates.FT_Get_Next_Char FT_Get_Next_Char;

        public static LibraryLoader? loader;

        public static void Init()
        {
            if(loader == null)
            {
                lock(typeof(Functions))
                {
                    if(loader == null)
                    {
                       loader = LoadFunctions();
                    }
                }                      
            }
        }

        private static LibraryLoader LoadFunctions()
        {
            var env = SystemEnvironment.Current;

            var libname = env.OS switch
            {
                SystemPlatform.Windows => "freetype.dll",
                SystemPlatform.MacOS => "libfreetype.dylib",
                SystemPlatform.Linux => "libfreetype.so",
                _ => throw new NotSupportedException()
            };

            var loader = env.LoadLibrary(libname);

            Functions.FT_Init_FreeType = LoadFunction<Delegates.FT_Init_FreeType>(loader);
            Functions.FT_Done_FreeType = LoadFunction<Delegates.FT_Done_FreeType>(loader);

            Functions.FT_New_Face = LoadFunction<Delegates.FT_New_Face>(loader);
            Functions.FT_New_Memory_Face = LoadFunction<Delegates.FT_New_Memory_Face>(loader);
            Functions.FT_Open_Face = LoadFunction<Delegates.FT_Open_Face>(loader);
            Functions.FT_Done_Face = LoadFunction<Delegates.FT_Done_Face>(loader);

            Functions.FT_Set_Char_Size = LoadFunction<Delegates.FT_Set_Char_Size>(loader);
            Functions.FT_Set_Pixel_Sizes = LoadFunction<Delegates.FT_Set_Pixel_Sizes>(loader);

            Functions.FT_Load_Char = LoadFunction<Delegates.FT_Load_Char>(loader);
            Functions.FT_Get_First_Char = LoadFunction<Delegates.FT_Get_First_Char>(loader);
            Functions.FT_Get_Next_Char = LoadFunction<Delegates.FT_Get_Next_Char>(loader);

            return loader;
        }

        private static T LoadFunction<T>(LibraryLoader loader)
        {
            var fptr = loader.LoadFunction(typeof(T).Name);
            return Marshal.GetDelegateForFunctionPointer<T>(fptr);
        }
    }
}
