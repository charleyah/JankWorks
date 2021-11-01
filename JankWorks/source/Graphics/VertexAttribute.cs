
namespace JankWorks.Graphics
{
    public struct VertexAttribute
    {
        public int Index;

        public int Stride;

        public int Offset;

        public VertexAttributeFormat Format;

        public VertexAttributeUsage Usage;
    }

    public enum VertexAttributeFormat
    {
        UByte,
        Byte,
        UShort,
        Short,
        UInt,
        Int,        
        Float,
        Double,
        Vector2f,
        Vector3f,
        Vector4f,
        Vector2i,
        Vector3i,
        Vector4i,
    }

    
    public enum VertexAttributeUsage
    {
        Other = 0,
        Position,
        Colour,
        Normal,
        TextureCoordinate,
        Binormal,
        Tangent,
        BlendIndices,
        BlendWeight,
        PointSize,
    }
}
