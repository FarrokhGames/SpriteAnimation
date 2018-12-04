using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Shared
{
    public abstract class AbstractSpriteAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField, Tooltip("A list of sprites to use with this animator. A frames index directly corresponds to an index in this list.")] protected UnityEngine.Sprite[] _sprites;
        [SerializeField, Tooltip("The clips that can be used by this animator")] protected Clip[] _clips;
        [SerializeField, Tooltip("If true, any children under this animator will be played, paused and resumed together with this parent")] protected bool _animateChildren = true;
        [SerializeField, Tooltip("How to handle the relationship with a parent animator")] AnimatorChildMode _childMode;
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] protected bool _allowFlipping = true;

        internal IFrameAnimator _frameAnimator;

        /// <inheritdoc />
        public Action OnClipComplete { get; set; }

        /// <inheritdoc />
        public Action<string> OnTrigger { get; set; }

        /// <inheritdoc />
        public bool IsPlaying { get { return _frameAnimator.IsPlaying; } }

        /// <inheritdoc />
        public IClip CurrentClip { get { return _frameAnimator.CurrentClip; } }

        /// <inheritdoc />
        public AnimatorChildMode ChildMode { get { return _childMode; } }

        /// <inheritdoc />
        public abstract bool Flip { get; set; }

        /// <summary>
        /// Invoked when this animator is created
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Invoked when the frame of the animation changes
        /// </summary>
        protected abstract void HandleFrameChanged(int index);

        void HandleClipComplete()
        {
            if (OnClipComplete != null) { OnClipComplete(); }
        }

        void HandleTrigger(string triggerName)
        {
            if (OnTrigger != null) { OnTrigger(triggerName); }
        }

        /// <inheritdoc />
        public void Play(IClip clip)
        {
            _frameAnimator.Play(clip);
        }

        /// <inheritdoc />
        public bool Play(string clipName)
        {
            return _frameAnimator.Play(clipName);
        }

        /// <inheritdoc />
        public void Pause()
        {
            _frameAnimator.Pause();
        }

        /// <inheritdoc />
        public void Resume()
        {
            _frameAnimator.Resume();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            OnClipComplete = null;
            OnTrigger = null;
            _frameAnimator.Dispose();
            _frameAnimator = null;
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _frameAnimator = new FrameAnimator(_clips, _childMode);
            Init();
        }

        void Start()
        {
            if (_animateChildren)
            {
                var childAnimators = GetComponentsInChildren<IAnimator>().Where(x => !x.Equals(this)).ToArray();
                _frameAnimator.SetChildren(childAnimators);
            }

            // Start first animation
            if (!_frameAnimator.IsPlaying && _clips != null && _clips.Length > 0)
            {
                _frameAnimator.Play(_clips[0]);
            }
        }

        void OnEnable()
        {
            _frameAnimator.OnClipComplete += HandleClipComplete;
            _frameAnimator.OnFrameChanged += HandleFrameChanged;
            _frameAnimator.OnTrigger += HandleTrigger;
        }

        void OnDisable()
        {
            _frameAnimator.OnClipComplete -= HandleClipComplete;
            _frameAnimator.OnFrameChanged -= HandleFrameChanged;
            _frameAnimator.OnTrigger -= HandleTrigger;
        }

        void Update()
        {
            _frameAnimator.Tick(Time.deltaTime);
        }

        void OnDestroyed()
        {
            Dispose();
        }

        #endregion
    }
}