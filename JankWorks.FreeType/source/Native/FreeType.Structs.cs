using System;
using System.Runtime.InteropServices;

using FT_Byte = System.Byte;
using FT_Char = System.SByte;

using FT_Int = System.Int32;
using FT_UInt = System.UInt32;

using FT_Int16 = System.Int16;
using FT_UInt16 = System.UInt16;

using FT_Int32 = System.Int32;
using FT_UInt32 = System.UInt32;

using FT_Int64 = System.Int64;
using FT_UInt64 = System.UInt64;

using FT_Short = System.Int16;
using FT_UShort = System.UInt16;

using FT_Long = nint;
using FT_ULong = nuint;

using FT_Fixed = nint;

using FT_String = JankWorks.Util.CString;

using FT_Pos = nint;

namespace JankWorks.Drivers.FreeType.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Library
    {
        IntPtr handle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Face
    {
        public FT_FaceRec* Rec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Bitmap_Size
    {
        public FT_Short height;
        public FT_Short width;

        public FT_Pos size;

        public FT_Pos x_ppem;
        public FT_Pos y_ppem;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Generic
    {
        public IntPtr data;
        public void* finalizer;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FT_Generic_Finalizer(IntPtr obj);

    [StructLayout(LayoutKind.Sequential)]
    public struct FT_BBox
    {
        public FT_Pos xMin, yMin;
        public FT_Pos xMax, yMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Vector
    {
        public FT_Pos x;
        public FT_Pos y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_FaceRec
    {
        public FT_Long num_faces;
        public FT_Long face_index;

        public FT_Long face_flags;
        public FT_Long style_flags;

        public FT_Long num_glyphs;

        public FT_String family_name;
        public FT_String style_name;

        public FT_Int num_fixed_sizes;
        public FT_Bitmap_Size* available_sizes;

        public FT_Int num_charmaps;
        public IntPtr charmaps;

        public FT_Generic generic;

        public FT_BBox bbox;

        public FT_UShort units_per_EM;
        public FT_Short ascender;
        public FT_Short descender;
        public FT_Short height;

        public FT_Short max_advance_width;
        public FT_Short max_advance_height;

        public FT_Short underline_position;
        public FT_Short underline_thickness;

        public FT_GlyphSlot glyph;
        public IntPtr size;
        public IntPtr charmap;

        public IntPtr driver;
        public IntPtr memory;
        public IntPtr stream;

        public FT_ListRec sizes_list;

        public FT_Generic autohint;
        public void* extensions;

        public IntPtr @internal;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FT_ListRec
    {
        public IntPtr head;
        public IntPtr tail;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphSlot
    {
        public FT_GlyphSlotRec* Rec;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_GlyphSlotRec
    {
        public FT_Library library;
        public FT_Face face;
        public FT_GlyphSlot next;
        public FT_UInt glyph_index; /* new in 2.10; was reserved previously */
        public FT_Generic generic;

        public FT_Glyph_Metrics metrics;
        public FT_Fixed linearHoriAdvance;
        public FT_Fixed linearVertAdvance;
        public FT_Vector advance;

        public FT_Glyph_Format format;

        public FT_Bitmap bitmap;
        public FT_Int bitmap_left;
        public FT_Int bitmap_top;

        public FT_Outline outline;

        public uint num_subglyphs;
        public IntPtr subglyphs;

        public void* control_data;
        public long control_len;

        public FT_Pos lsb_delta;
        public FT_Pos rsb_delta;

        public void* other;

        public IntPtr @internal;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FT_Glyph_Metrics
    {
        public FT_Pos width;
        public FT_Pos height;

        public FT_Pos horiBearingX;
        public FT_Pos horiBearingY;
        public FT_Pos horiAdvance;

        public FT_Pos vertBearingX;
        public FT_Pos vertBearingY;
        public FT_Pos vertAdvance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Bitmap
    {
        public uint rows;
        public int width;
        public int pitch;
        public byte* buffer;
        public ushort num_grays;
        public FT_Pixel_Mode pixel_mode;
        public byte palette_mode;
        public void* palette;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct FT_Outline
    {
        public short n_contours;
        public short n_points;

        public FT_Vector* points;
        public char* tags;
        public short* contours;

        public int flags;           
    }
}