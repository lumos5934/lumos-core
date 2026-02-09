using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LumosLib
{
    public class TutorialManager : MonoBehaviour, IPreInitializable, ITutorialManager
    {
        private TutorialTable _curTutorialTable;
        private BaseTutorial _curTutorial;
        private int _curStep;
        
        private void Update()
        {
            if (_curTutorial != null)
            {
                _curTutorial.Update();

                if (_curTutorial.IsComplete())
                {
                    ChangeNextStep();
                }
            }
        }
    
    
        public UniTask<bool> InitAsync()
        {
            GlobalService.Register<ITutorialManager>(this);
            return UniTask.FromResult(true);
        }
      
        public TutorialTable GetTable()
        {
            return _curTutorialTable;
        }
    
        public BaseTutorial GetTutorial()
        {
            return _curTutorial;
        }
    
        public void Play(TutorialTable table)
        {
            _curStep = 0;
            _curTutorialTable = table;
        
            _curTutorial = _curTutorialTable.CreateTutorial(_curStep);
            _curTutorial.Enter();
        }

        private void ChangeNextStep()
        {
            _curStep++;
        
            _curTutorial.Exit();
            _curTutorial = _curStep < _curTutorialTable.GetAssetCount() ? _curTutorialTable.CreateTutorial(_curStep) : null ;
            _curTutorial?.Enter();
        }
    }
}

