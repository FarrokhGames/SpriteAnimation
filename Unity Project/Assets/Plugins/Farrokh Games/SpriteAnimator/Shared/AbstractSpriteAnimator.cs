using System;
using System.Collections.Generic;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Shared
{
    public abstract class AbstractSpriteAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField] protected UnityEngine.Sprite[] _sprites;
        [SerializeField] AbstractSpriteAnimator[] _sharedAnimators;
        [SerializeField] Clip[] _clips;
        [SerializeField] protected bool _animateChildren = true;

        IFrameAnimator _animator = new FrameAnimator();
        AbstractSpriteAnimator[] _children;
        Dictionary<string, Clip> _nameToClip = null;

        /// <inheritdoc />
        public Action OnClipComplete { get; set; }

        /// <inheritdoc />
        public Action<string> OnTrigger { get; set; }

        /// <inheritdoc />
        public bool IsPlaying { get { return _animator.IsPlaying; } }

        /// <inheritdoc />
        public IClip CurrentClip { get { return _animator.CurrentClip; } }

        /// <inheritdoc />
        public bool Play(string clipName)
        {
            CacheClipNames();
            if (_nameToClip.ContainsKey(clipName))
            {
                var clip = _nameToClip[clipName];
                Play(clip);
                return true;
            }
            return false;
        }

        void CacheClipNames()
        {
            if (_nameToClip == null)
            {
                _nameToClip = new Dictionary<string, Clip>();
                for (var i = 0; i < _clips.Length; i++)
                {
                    var clip = _clips[i];
                    _nameToClip.Add(clip.Name, clip);
                }
            }
        }

        /// <inheritdoc />
        public void Play(IClip clip)
        {
            if (clip != null)
            {
                _animator.Play(clip);

                // Play Children
                if (_animateChildren && _children != null && _children.Length > 0)
                {
                    for (var i = 0; i < _children.Length; i++)
                    {
                        var animator = _children[i];
                        if (animator != null) { animator.Play(clip.Name); }
                    }
                }

                // Play Shared
                if (_sharedAnimators != null && _sharedAnimators.Length > 0)
                {
                    for (var i = 0; i < _sharedAnimators.Length; i++)
                    {
                        var animator = _sharedAnimators[i];
                        if (animator != null) { animator.Play(clip); }
                    }
                }
            }
        }

        /// <inheritdoc />
        public void Pause()
        {
            _animator.Pause();

            // Pause Children
            if (_animateChildren && _children != null && _children.Length > 0)
            {
                for (var i = 0; i < _children.Length; i++)
                {
                    var animator = _children[i];
                    if (animator != null) { animator.Pause(); }
                }
            }

            // Pause Shared
            if (_sharedAnimators != null && _sharedAnimators.Length > 0)
            {
                for (var i = 0; i < _sharedAnimators.Length; i++)
                {
                    var animator = _sharedAnimators[i];
                    if (animator != null) { animator.Pause(); }
                }
            }
        }

        /// <inheritdoc />
        public void Resume()
        {
            _animator.Resume();

            // Resume Children
            if (_animateChildren && _children != null && _children.Length > 0)
            {
                for (var i = 0; i < _children.Length; i++)
                {
                    var animator = _children[i];
                    if (animator != null) { animator.Resume(); }
                }
            }

            // Resume Shared
            if (_sharedAnimators != null && _sharedAnimators.Length > 0)
            {
                for (var i = 0; i < _sharedAnimators.Length; i++)
                {
                    var animator = _sharedAnimators[i];
                    if (animator != null) { animator.Resume(); }
                }
            }
        }

        protected T[] GetListOfChildren<T>()
        {
            var list = new List<T>();
            if (_children != null) { list.AddRange(_children.Cast<T>()); }
            if (_sharedAnimators != null) { list.AddRange(_sharedAnimators.Cast<T>()); }
            return list.Distinct().ToArray();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            OnClipComplete = null;
            OnTrigger = null;
            _animator.Dispose();
            _animator = null;
            _children = null;
        }

        #region MonoBehavior (Unity)

        void OnEnable()
        {
            _animator.OnClipComplete += HandleClipComplete;
            _animator.OnFrameChanged += HandleFrameChanged;
            _animator.OnTrigger += HandleTrigger;

            var children = GetComponentsInChildren<AbstractSpriteAnimator>();
            _children = children.Where(x => x != this).ToArray();
        }

        void OnDisable()
        {
            _animator.OnClipComplete -= HandleClipComplete;
            _animator.OnFrameChanged -= HandleFrameChanged;
            _animator.OnTrigger -= HandleTrigger;
        }

        void HandleClipComplete()
        {
            if (OnClipComplete != null) { OnClipComplete(); }
        }

        protected abstract void HandleFrameChanged(int index);

        void HandleTrigger(string triggerName)
        {
            if (OnTrigger != null) { OnTrigger(triggerName); }
        }

        void Update()
        {
            _animator.Tick(Time.deltaTime);
        }

        void OnDestroyed()
        {
            Dispose();
        }

        #endregion
    }
}