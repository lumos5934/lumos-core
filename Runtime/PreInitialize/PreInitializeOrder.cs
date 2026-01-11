namespace Lumos.Core
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