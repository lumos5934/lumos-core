using UnityEngine;

namespace LumosLib
{
    public interface IPopupManager
    {
        public T Get<T>() where T : UIPopup;
        public T Open<T>() where T : UIPopup;
        public void Close<T>() where T : UIPopup;
        public void Close();
        public void CloseAll();
        public void Register(UIPopup popup);
        public void Unregister(UIPopup popup);
    }
}