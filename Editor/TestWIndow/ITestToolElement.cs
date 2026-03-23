namespace LumosLib.Editor
{
    public interface ITestToolElement
    {
        string Title { get; }
        int Priority { get; }
        void OnGUI();
    }
}