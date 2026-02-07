using UnityEngine;

namespace LumosLib
{
    public interface IPopupManager
    {
        public T Get<T>() where T : UIPopup;
        public T Open<T>() where T : UIPopup;
        public void Close();
        public void Close<T>() where T : UIPopup;
        public void CloseAll();
    }
}