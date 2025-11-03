namespace LLib.Core
{
    public interface IUIManager : IGlobal
    {
        public void SetEnable<T>(int id, bool enable) where T : UIBase;
        public T Get<T>(int id) where T : UIBase;
    }
}