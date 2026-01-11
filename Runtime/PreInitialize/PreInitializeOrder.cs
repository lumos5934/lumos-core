namespace Lumos
{
    public enum PreInitializeOrder
    {
        Data = int.MinValue,
        Resource,
        Pool,
        UI,
        Audio,
        Pointer,
    }
}