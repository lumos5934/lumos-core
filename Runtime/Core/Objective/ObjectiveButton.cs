using UnityEngine;
using UnityEngine.UI;

namespace LumosLib
{
    [RequireComponent(typeof(Button))]
    public class ObjectiveButton : MonoBehaviour
    {
        [SerializeField] private int _triggerID;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() =>
            {
                GlobalService.GetInternal<ObjectiveManager>().Evaluate(new TriggerEvent(_triggerID));
            });
        }

        public void SetTriggerID(int triggerID)
        {
            _triggerID = triggerID;
        }
        
    }
}


