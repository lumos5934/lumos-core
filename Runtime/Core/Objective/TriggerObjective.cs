namespace LumosLib
{
    public class TriggerObjective : BaseObjective
    {
        private readonly int _id;
        private readonly int _needCount;
        
        private bool _isTriggered;
        private int _count = -1;
        
        public TriggerObjective(string key, int triggerID) : base(key)
        {
            _id = triggerID;
        }
        
        public TriggerObjective(string key, int triggerID, int needCount) : base(key)
        {
            _id = triggerID;
            _needCount = needCount;
            _count = 0;
        }
        
        public override void Evaluate<T>(T targetEvent)
        {
            if (targetEvent is TriggerEvent triggerEvent)
            {
                if (triggerEvent.GetID() == _id)
                {
                    if (_count < 0)
                    {
                        _isTriggered = true;
                    }
                    else
                    {
                        if (_count < _needCount)
                        {
                            _count++;
                        }

                        if (_count == _needCount)
                        {
                            _isTriggered = true;
                        }
                    }
                }
            }
        }

        public override bool IsComplete()
        {
            return _isTriggered;
        }
    }
}