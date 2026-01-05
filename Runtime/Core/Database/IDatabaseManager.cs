using System.Collections.Generic;

namespace LumosLib
{
    public interface IDatabaseManager
    {
        public List<T> GetAll<T>() where T : BaseBGData;
        public T Get<T>(int tableID) where T : BaseBGData;
    }
}