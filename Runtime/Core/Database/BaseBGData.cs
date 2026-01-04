using BansheeGz.BGDatabase;

namespace LumosLib
{
    public abstract class BaseBGData
    {
        public int TableID { get; private set; }
        public string Name { get; private set; }
        
        public BaseBGData(BGEntity entity)
        {
            TableID =  entity.Get<int>("table_id");
            Name = entity.Get<string>("name");
        }
    }
}

