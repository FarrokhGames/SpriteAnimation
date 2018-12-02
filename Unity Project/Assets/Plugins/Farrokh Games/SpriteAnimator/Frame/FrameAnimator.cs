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
                _currentFrame = _currentClip.RandomStart ? UnityEngine.Random.Range(0, _currentClip.Length) : 0;
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
                    if (_currentFrame >= _currentClip.Length)
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
            if (OnFrameChanged != null) { OnFrameChanged(currentFrame.Index - 1); }
            if (!string.IsNullOrEmpty(currentFrame.TriggerName))
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
        [SerializeField] string _name;
        [SerializeField] bool _loop;
        [SerializeField] bool _randomStart;
        [SerializeField] float _frameRate;
        [SerializeField] Frame[] _frames;

        /// <inheritdoc />
        public string Name { get { return _name; } }

        /// <inheritdoc />
        public bool Loop { get { return _loop; } }

        /// <inheritdoc />
        public bool RandomStart { get { return _randomStart; } }

        /// <inheritdoc />
        public float FrameRate { get { return _frameRate; } }

        /// <inheritdoc />
        public int Length { get { return _frames.Length; } }

        /// <inheritdoc />
        public IFrame this [int index] { get { return _frames[index]; } }
    }

    /// <inheritdoc />
    [Serializable]
    public class Frame : IFrame
    {
        [SerializeField] int _index;
        [SerializeField] float _speed;
        [SerializeField] string _triggerName;

        /// <inheritdoc />
        public int Index { get { return _index; } }

        /// <inheritdoc />
        public float Speed { get { return _speed; } }

        /// <inheritdoc />
        public string TriggerName { get { return _triggerName; } }
    }
}