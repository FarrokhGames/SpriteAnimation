using System;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Frame
{
    /// <inheritdoc />
    public class FrameAnimator : IFrameAnimator
    {
        /// <inheritdoc />
        public Action OnClipComplete { get; set; }

        /// <inheritdoc />
        public Action<int> OnFrameChanged { get; set; }

        /// <inheritdoc />
        public Action<string> OnTrigger { get; set; }

        /// <inheritdoc />
        public IClip CurrentClip { get { return _currentClip; } }

        /// <inheritdoc />
        public bool IsPlaying { get { return _isPlaying; } }

        IClip _currentClip;
        bool _isPlaying;
        float _currentTime;
        int _currentFrame;

        /// <inheritdoc />
        public void Play(IClip clip)
        {
            if (clip != null && _currentClip != clip)
            {
                _currentClip = clip;
                _currentTime = 0f;
                _currentFrame = _currentClip.RandomStart ? UnityEngine.Random.Range(0, _currentClip.FrameCount) : 0;
                HandleFrame(_currentFrame);
                _isPlaying = true;
            }
        }

        /// <inheritdoc />
        public void Pause()
        {
            _isPlaying = false;
        }

        /// <inheritdoc />
        public void Resume()
        {
            _isPlaying = true;
        }

        /// <inheritdoc />
        public void Tick(float deltaTime)
        {
            if (_isPlaying && _currentClip != null)
            {
                var currentFrame = _currentClip[_currentFrame];
                _currentTime += deltaTime * _currentClip.FrameRate * currentFrame.Speed;

                if (_currentTime >= 1f)
                {
                    _currentTime -= 1f;
                    _currentFrame++;
                    if (_currentFrame >= _currentClip.FrameCount)
                    {
                        if (!_currentClip.Loop)
                        {
                            if (OnClipComplete != null) { OnClipComplete(); }
                            Pause();
                            return;
                        }
                        _currentFrame = 0; // Loop
                    }
                    HandleFrame(_currentFrame);
                }
            }
        }

        /*
        Invokes the appropriate actions of a given frame
        */
        void HandleFrame(int frame)
        {
            var currentFrame = _currentClip[frame];
            if (OnFrameChanged != null) { OnFrameChanged(currentFrame.Index); }
            if (currentFrame.HasTrigger)
            {
                if (OnTrigger != null) { OnTrigger(currentFrame.TriggerName); }
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            OnClipComplete = null;
            OnFrameChanged = null;
            OnTrigger = null;
            _currentClip = null;
        }
    }

    /// <inheritdoc />
    [Serializable]
    public class Clip : IClip
    {
        [SerializeField, Tooltip("The name of the clip")] string _name;
        [SerializeField, Tooltip("Wether this clip loops or not")] bool _loop = true;
        [SerializeField, Tooltip("Wether this clip starts on a random frame")] bool _randomStart = false;
        [SerializeField, Tooltip("The frame rate of this clip in frames per second")] float _frameRate = 8f;
        [SerializeField, Tooltip("The frames of this clip")] Frame[] _frames;

        /// <inheritdoc />
        public string Name { get { return _name; } }

        /// <inheritdoc />
        public bool Loop { get { return _loop; } }

        /// <inheritdoc />
        public bool RandomStart { get { return _randomStart; } }

        /// <inheritdoc />
        public float FrameRate { get { return _frameRate; } }

        /// <inheritdoc />
        public int FrameCount { get { return _frames.Length; } }

        /// <inheritdoc />
        public IFrame this [int index] { get { return _frames[index]; } }
    }

    /// <inheritdoc />
    [Serializable]
    public class Frame : IFrame
    {
        [SerializeField, Tooltip("The index of this clip")] int _index = 0;
        [SerializeField, Tooltip("The speed of this clip. Use this to slow down or speed up individual frames")] float _speed = 1f;
        [SerializeField, Tooltip("The name of the trigger. Leave empty if you don't want a trigger")] string _triggerName;

        /// <inheritdoc />
        public int Index { get { return _index; } }

        /// <inheritdoc />
        public float Speed { get { return _speed; } }

        /// <inheritdoc />
        public bool HasTrigger { get { return !string.IsNullOrEmpty(_triggerName); } }

        /// <inheritdoc />
        public string TriggerName { get { return _triggerName; } }
    }
}