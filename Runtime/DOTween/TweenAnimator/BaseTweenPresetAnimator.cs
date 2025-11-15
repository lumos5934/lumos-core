using System.Collections.Generic;
using DG.Tweening;
using TriInspector;
using UnityEngine;

namespace LumosLib
{
    public abstract class BaseTweenPresetAnimator<T> : MonoBehaviour where T : BaseTweenPreset
    {
        #region >--------------------------------------------------- FIELD

        [Title("Base")]
        
        [TableList(Draggable = true, HideAddButton = false, HideRemoveButton = false, AlwaysExpanded = false)]
        [SerializeField] protected List<TweenPresetEntry<T>> _entries;

        protected Tweener _tweener;
        
        private Dictionary<string, TweenPresetEntry<T>> _entryDict = new();

        #endregion
        #region >--------------------------------------------------- UNITY

        protected virtual void Awake()
        {
            foreach (var entry in _entries)
            {
                _entryDict[entry.GetKey()] = entry;
            }
        }

        #endregion
        #region >--------------------------------------------------- Play

        public void Play(string key)
        {
            GetTweener(key).Play();
        }
        
        #endregion
        #region >--------------------------------------------------- GET

        public abstract Tweener GetTweener(string key);
        
        protected T GetPreset(string key)
        {
            if (_entryDict.ContainsKey(key))
            {
                return _entryDict[key].GetPreset();
            }
            
            return null;
        }
        
        
        protected Tweener OnGetTweener(T preset, Tweener baseTweener)
        {
            _tweener?.Kill();
            _tweener = baseTweener;
            
            if (preset.GetUseLoop())
            {
                _tweener.SetLoops(preset.GetLoopCount(), preset.GetLoopType());
            }
            
            return _tweener.SetEase(preset.GetEase()).Pause();
        }

        #endregion
    }
}