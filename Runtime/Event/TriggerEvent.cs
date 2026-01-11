namespace Lumos.Core
{
    public struct TriggerEvent : IGameEvent
    {
        private int _id;
        
        public TriggerEvent(int triggerID) 
        {
            _id = triggerID;
        }
        
        public int GetID()
        {
            return _id;
        }
    }
}