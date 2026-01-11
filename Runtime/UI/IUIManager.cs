namespace Lumos
{
    public interface IUIManager
    {
        public void SetEnable<T>(bool enable) where T : UIBase;
        public void SetToggle<T>() where T : UIBase;
        public T Get<T>() where T : UIBase;
    }
}