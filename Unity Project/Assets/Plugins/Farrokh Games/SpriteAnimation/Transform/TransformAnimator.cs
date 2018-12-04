using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Transform
{
    /// <summary>
    /// IAnimatorBase-implementation for Unity's Transforms
    /// </summary>
    [RequireComponent(typeof(UnityEngine.Transform))]
    public class TransformAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField, Tooltip("The clip names that will enable this transform")] string[] _clips;
        [SerializeField, Tooltip("How to handle the relationship with a parent animator")] AnimatorChildMode _childMode;
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] bool _allowFlipping = true;

        Vector3 _startScale;

        /// <inheritdoc />
        public Action OnClipComplete
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public Action<string> OnTrigger
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public bool IsPlaying { get { return gameObject.activeSelf; } }

        /// <inheritdoc />
        public IClip CurrentClip { get { return null; } }

        /// <inheritdoc />
        public AnimatorChildMode ChildMode { get { return _childMode; } }

        /// <inheritdoc />
        public bool Flip
        {
            get { return transform.localScale.x < 0; }
            set
            {
                if (_allowFlipping && Flip != value)
                {
                    transform.localScale = new Vector3(value ? -_startScale.x : _startScale.x, _startScale.y, _startScale.z);
                }
            }
        }

        /// <inheritdoc />
        public void Play(IClip clip)
        {
            if (clip != null)
            {
                Play(clip.Name);
            }
        }

        /// <inheritdoc />
        public bool Play(string clipName)
        {
            var shouldBeActive = _clips.Contains(clipName);
            gameObject.SetActive(shouldBeActive);
            return shouldBeActive;
        }

        /// <inheritdoc />
        public void Pause()
        {
            // Do nothing
        }

        /// <inheritdoc />
        public void Resume()
        {
            // Do nothing
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Do nothing
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _startScale = transform.localScale;
        }

        #endregion
    }
}