using System;
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
        [SerializeField] bool _animateChildren = true;

        IFrameAnimator _animator = new FrameAnimator();
        AbstractSpriteAnimator[] _children;

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
            var clip = _clips.FirstOrDefault(x => x.Name == clipName);
            Play(clip);
            return clip != null;
        }

        /// <inheritdoc />
        public void Play(IClip clip)
        {
            if (clip != null)
            {
                _animator.Play(clip);
                if (_animateChildren) { PerformOnChildren(animator => animator.Play(clip.Name)); }
                PerformOnShared(animator => animator.Play(clip));
            }
        }

        /// <inheritdoc />
        public void Pause()
        {
            _animator.Pause();
            if (_animateChildren) { PerformOnChildren(animator => animator.Pause()); }
            PerformOnShared(animator => animator.Pause());
        }

        /// <inheritdoc />
        public void Resume()
        {
            _animator.Resume();
            if (_animateChildren) { PerformOnChildren(animator => animator.Resume()); }
            PerformOnShared(animator => animator.Resume());
        }

        void PerformOnChildren(Action<AbstractSpriteAnimator> method)
        {
            PerformOnList<AbstractSpriteAnimator>(_children, method);
        }

        void PerformOnShared(Action<AbstractSpriteAnimator> method)
        {
            PerformOnList<AbstractSpriteAnimator>(_sharedAnimators, method);
        }

        protected void PerformOnChildren<T>(Action<T> method)where T : AbstractSpriteAnimator
        {
            PerformOnList<T>(_children, method);
        }

        void PerformOnList<T>(AbstractSpriteAnimator[] list, Action<T> method)where T : AbstractSpriteAnimator
        {
            if (list != null && list.Length > 0)
            {
                for (var i = 0; i < list.Length; i++)
                {
                    var animator = list[i];
                    if (animator != null) { method(list[i] as T); }
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