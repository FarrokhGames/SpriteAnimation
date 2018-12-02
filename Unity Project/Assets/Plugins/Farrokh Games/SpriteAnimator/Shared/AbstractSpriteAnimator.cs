using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Shared
{
    public abstract class AbstractSpriteAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField] AbstractSpriteAnimator[] _sharedAnimators;
        [SerializeField] Clip[] _clips;

        IFrameAnimator _animator = new FrameAnimator();
        IAnimator[] _children;

        /// <inheritdoc />
        public Action OnClipComplete { get; set; }

        /// <inheritdoc />
        public Action<string> OnTrigger { get; set; }

        /// <inheritdoc />
        public bool IsPlaying { get { return _animator.IsPlaying; } }

        /// <inheritdoc />
        public IClip CurrentClip { get { return _animator.CurrentClip; } }

        /// <inheritdoc />
        public bool Play(string clipName, bool withChildren = true)
        {
            var clip = _clips.FirstOrDefault(x => x.Name == clipName);
            Play(clip, withChildren);
            return clip != null;
        }

        /// <inheritdoc />
        public void Play(IClip clip, bool withChildren = true)
        {
            if (clip != null)
            {
                _animator.Play(clip);
                if (withChildren) { PerformOnList(_children, animator => animator.Play(clip.Name, withChildren)); }
                PerformOnList(_sharedAnimators, animator => animator.Play(clip.Name, withChildren));
            }
        }

        /// <inheritdoc />
        public void Pause(bool withChildren = true)
        {
            _animator.Pause();
            if (withChildren) { PerformOnList(_children, animator => animator.Pause(withChildren)); }
            PerformOnList(_sharedAnimators, animator => animator.Pause(withChildren));
        }

        /// <inheritdoc />
        public void Resume(bool withChildren = true)
        {
            _animator.Resume();
            if (withChildren) { PerformOnList(_children, animator => animator.Resume(withChildren)); }
            PerformOnList(_sharedAnimators, animator => animator.Resume(withChildren));
        }

        void PerformOnList(IAnimator[] list, Action<IAnimator> method)
        {
            if (list != null && list.Length > 0)
            {
                for (var i = 0; i < list.Length; i++)
                {
                    var animator = list[i];
                    if (animator != null) { method(list[i]); }
                }
            }
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
            _children = GetComponentsInChildren<AbstractSpriteAnimator>();
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