using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Particle
{
    /// <summary>
    /// IAnimatorBase-implementation for Unity's ParticleSystem
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemAnimator : MonoBehaviour, IAnimator
    {
        [SerializeField, Tooltip("The clip names that will trigger this particle system")] string[] _clips;
        [SerializeField, Tooltip("Wether the particle system should operate together with its children")] bool _withChildren;
        [SerializeField, Tooltip("How should the particle system behave when stopping an animation")] StopMode _stopMode;
        [SerializeField, Tooltip("How to handle the relationship with a parent animator")] AnimatorChildMode _childMode;
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] bool _allowFlipping = true;

        ParticleSystem _particleSystem;
        bool _isPaused = false;
        Vector3 _startScale;

        public enum StopMode
        {
            Stop,
            Clear,
        }

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
        public bool IsPlaying { get { return _particleSystem.isPlaying; } }

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
                    if (_particleSystem.isPlaying)
                    {
                        _particleSystem.Clear();
                        Play();
                    }
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
            if (_clips.Contains(clipName))
            {
                Play();
                return true;
            }
            else
            {
                _isPaused = false;
                _particleSystem.Stop(_withChildren, _stopMode == StopMode.Clear ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
                return false;
            }
        }

        void Play()
        {
            if (!IsPlaying)
            {
                _particleSystem.Play(_withChildren);
                _isPaused = false;
            }
        }

        /// <inheritdoc />
        public void Pause()
        {
            _particleSystem.Pause(_withChildren);
            _isPaused = true;
        }

        /// <inheritdoc />
        public void Resume()
        {
            if (_isPaused)
            {
                Play();
            }
        }

        public void Dispose()
        {
            // Do nothing
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _startScale = gameObject.transform.localScale;
        }

        #endregion
    }
}