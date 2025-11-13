using System.Collections.Generic;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LumosLib
{
    [RequireComponent(typeof(SequencePresetContainer))]
    public class UISequencePresetTrigger : MonoBehaviour, 
        IPointerEnterHandler, 
        IPointerExitHandler,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        ICancelHandler
    {
        #region >--------------------------------------------------- FIELD
        
        [Title("Trigger Event")]
        [Space(10f)]
        
        [InfoBox("Duplicate 'EventType' detected — only the first one will be applied.", TriMessageType.Error, nameof(IsValidEventType))]
        [TableList(Draggable = true, HideAddButton = false, HideRemoveButton = false, AlwaysExpanded = false)]
        [SerializeField] private List<UISequencePreset> _entries;
        
        private SequencePresetContainer _container;
        private Sequence _runningSequence;
        
        
        #endregion
        #region >--------------------------------------------------- UNITY
        
        
        private void Awake()
        {
            _container = GetComponent<SequencePresetContainer>();
        }
        
        
        #endregion
        #region >--------------------------------------------------- UNITY_EVENT
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Play(UISequencePreset.Type.PointerEnter);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            Play(UISequencePreset.Type.PointerExit);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Play(UISequencePreset.Type.PointerClick);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Play(UISequencePreset.Type.PointerDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Play(UISequencePreset.Type.PointerUp);
        }

        public void OnSelect(BaseEventData eventData)
        {
            Play(UISequencePreset.Type.Select);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Play(UISequencePreset.Type.Deselect);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Play(UISequencePreset.Type.Submit);
        }

        public void OnCancel(BaseEventData eventData)
        {
            Play(UISequencePreset.Type.Cancel);
        }
        
        
        #endregion
        #region >--------------------------------------------------- PLAY

        
        private void Play(UISequencePreset.Type eventType)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                if (_entries[i].EventType == eventType)
                {
                    _runningSequence?.Kill();
                    
                    _runningSequence = _container.GetSequence(_entries[i].PresetKey, gameObject);

                    _runningSequence?.Play();
                    
                    break;
                }
            }
        }
        
        
        #endregion
        #region >--------------------------------------------------- INSPECTOR
        
        
        private bool IsValidEventType()
        {
            var containsTypes = new HashSet<UISequencePreset.Type>();
            
            for (int i = 0; i < _entries.Count; i++)
            {
                if (!containsTypes.Add(_entries[i].EventType))
                {
                    return true;
                }
            }

            return false;
        }
        
        
        #endregion
    }
}