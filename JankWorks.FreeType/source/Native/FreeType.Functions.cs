using System;
using System.Runtime.CompilerServices;

using JankWorks.Util;
using JankWorks.Platform;


namespace JankWorks.Drivers.FreeType.Native
{
	static unsafe class Functions
	{
		private static delegate* unmanaged[Cdecl]<FT_Library*, FT_Error> FT_Init_FreeTypePtr;
		private static delegate* unmanaged[Cdecl]<FT_Library, FT_Error> FT_Done_FreeTypePtr;
		private static delegate* unmanaged[Cdecl]<FT_Library, CString, int, FT_Face*, FT_Error> FT_New_FacePtr;
		private static delegate* unmanaged[Cdecl]<FT_Library, IntPtr, int, int, FT_Face*, FT_Error> FT_New_Memory_FacePtr;
		private static delegate* unmanaged[Cdecl]<FT_Library, IntPtr, int, FT_Face*, FT_Error> FT_Open_FacePtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, FT_Error> FT_Done_FacePtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, IntPtr, IntPtr, uint, uint, FT_Error> FT_Set_Char_SizePtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, uint, uint, FT_Error> FT_Set_Pixel_SizesPtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, uint, int, FT_Error> FT_Load_CharPtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, uint*, ulong> FT_Get_First_CharPtr;
		private static delegate* unmanaged[Cdecl]<FT_Face, ulong, uint*, ulong> FT_Get_Next_CharPtr;

		public static LibraryLoader loader;

		public static void Init()
		{
			var env = SystemEnvironment.Current;

			Functions.loader = env.OS switch
			{
				SystemPlatform.Windows => env.LoadLibrary("freetype.dll"),
				SystemPlatform.MacOS => env.LoadLibrary("libfreetype.dylib"),
				SystemPlatform.Linux => env.LoadLibrary("libfreetype.so"),
				_ => throw new NotSupportedException()
			};

			Functions.FT_Init_FreeTypePtr = (delegate* unmanaged[Cdecl]<FT_Library*, FT_Error>)Functions.LoadFunction("FT_Init_FreeType");
			Functions.FT_Done_FreeTypePtr = (delegate* unmanaged[Cdecl]<FT_Library, FT_Error>)Functions.LoadFunction("FT_Done_FreeType");
			Functions.FT_New_FacePtr = (delegate* unmanaged[Cdecl]<FT_Library, CString, int, FT_Face*, FT_Error>)Functions.LoadFunction("FT_New_Face");
			Functions.FT_New_Memory_FacePtr = (delegate* unmanaged[Cdecl]<FT_Library, IntPtr, int, int, FT_Face*, FT_Error>)Functions.LoadFunction("FT_New_Memory_Face");
			Functions.FT_Open_FacePtr = (delegate* unmanaged[Cdecl]<FT_Library, IntPtr, int, FT_Face*, FT_Error>)Functions.LoadFunction("FT_Open_Face");
			Functions.FT_Done_FacePtr = (delegate* unmanaged[Cdecl]<FT_Face, FT_Error>)Functions.LoadFunction("FT_Done_Face");
			Functions.FT_Set_Char_SizePtr = (delegate* unmanaged[Cdecl]<FT_Face, IntPtr, IntPtr, uint, uint, FT_Error>)Functions.LoadFunction("FT_Set_Char_Size");
			Functions.FT_Set_Pixel_SizesPtr = (delegate* unmanaged[Cdecl]<FT_Face, uint, uint, FT_Error>)Functions.LoadFunction("FT_Set_Pixel_Sizes");
			Functions.FT_Load_CharPtr = (delegate* unmanaged[Cdecl]<FT_Face, uint, int, FT_Error>)Functions.LoadFunction("FT_Load_Char");
			Functions.FT_Get_First_CharPtr = (delegate* unmanaged[Cdecl]<FT_Face, uint*, ulong>)Functions.LoadFunction("FT_Get_First_Char");
			Functions.FT_Get_Next_CharPtr = (delegate* unmanaged[Cdecl]<FT_Face, ulong, uint*, ulong>)Functions.LoadFunction("FT_Get_Next_Char");
		}

		private static void* LoadFunction(string name) => Functions.loader.LoadFunction(name).ToPointer();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Init_FreeType(FT_Library* library)
		{
			return Functions.FT_Init_FreeTypePtr(library);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Done_FreeType(FT_Library library)
		{
			return Functions.FT_Done_FreeTypePtr(library);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_New_Face(FT_Library library, CString filepathname, int face_index, FT_Face* aface)
		{
			return Functions.FT_New_FacePtr(library, filepathname, face_index, aface);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_New_Memory_Face(FT_Library library, IntPtr file_base, int file_size, int face_index, FT_Face* aface)
		{
			return Functions.FT_New_Memory_FacePtr(library, file_base, file_size, face_index, aface);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Open_Face(FT_Library library, IntPtr args, int face_index, FT_Face* aface)
		{
			return Functions.FT_Open_FacePtr(library, args, face_index, aface);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Done_Face(FT_Face face)
		{
			return Functions.FT_Done_FacePtr(face);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Set_Char_Size(FT_Face face, IntPtr char_width, IntPtr char_height, uint horz_resolution, uint vert_resolution)
		{
			return Functions.FT_Set_Char_SizePtr(face, char_width, char_height, horz_resolution, vert_resolution);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Set_Pixel_Sizes(FT_Face face, uint pixel_width, uint pixel_height)
		{
			return Functions.FT_Set_Pixel_SizesPtr(face, pixel_width, pixel_height);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FT_Error FT_Load_Char(FT_Face face, uint char_code, int load_flags)
		{
			return Functions.FT_Load_CharPtr(face, char_code, load_flags);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong FT_Get_First_Char(FT_Face face, uint* index)
		{
			return Functions.FT_Get_First_CharPtr(face, index);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong FT_Get_Next_Char(FT_Face face, ulong char_code, uint* index)
		{
			return Functions.FT_Get_Next_CharPtr(face, char_code, index);
		}
	}
}