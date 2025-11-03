namespace LLib.Core
{
    public enum PreInitializeOrder
    {
        Resource = int.MinValue,
        Pool,
        UI,
        Audio,
    }
}