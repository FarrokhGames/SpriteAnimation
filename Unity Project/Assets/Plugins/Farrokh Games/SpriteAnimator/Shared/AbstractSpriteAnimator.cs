using System;
using System.Collections.Generic;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Shared
{
    public abstract class AbstractSpriteAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField, Tooltip("A list of sprites to use with this animator. A frames index directly corresponds to an index in this list.")] protected UnityEngine.Sprite[] _sprites;
        [SerializeField, Tooltip("Shared animators will play the same clip as this parent animator. This is useful as it allows you to share clips between animators")] AbstractSpriteAnimator[] _sharedAnimators;
        [SerializeField, Tooltip("The clips that can be used by this animator")] Clip[] _clips;
        [SerializeField, Tooltip("If true, any children under this animator will be played, paused and resumed together with this parent")] protected bool _animateChildren = true;

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

        /* 
        Caches the clips of this animator for quick access using clip-names
        */
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

        /// <summary>
        /// Returns a casted list of children
        /// </summary>
        /// <typeparam name="T">A implementation type of AbstractSpriteAnimator</typeparam>
        protected T[] GetListOfChildren<T>()where T : AbstractSpriteAnimator
        {
            var list = new List<T>();
            if (_children != null) { list.AddRange(_children.Cast<T>()); }
            if (_sharedAnimators != null) { list.AddRange(_sharedAnimators.Cast<T>()); }
            return list.Distinct().ToArray();
        }

        /// <summary>
        /// Invoked when this animator is created
        /// </summary>
        protected abstract void Init();

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

        void Awake()
        {
            var children = GetComponentsInChildren<AbstractSpriteAnimator>();
            _children = children.Where(x => x != this).ToArray();

            Init();

            // Autoplay first clip
            if (_clips.Length > 0)
            {
                Play(_clips[0]);
            }
        }

        void OnEnable()
        {
            _animator.OnClipComplete += HandleClipComplete;
            _animator.OnFrameChanged += HandleFrameChanged;
            _animator.OnTrigger += HandleTrigger;
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