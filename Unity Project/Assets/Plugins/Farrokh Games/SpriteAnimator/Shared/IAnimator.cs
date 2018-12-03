using System;

namespace FarrokhGames.SpriteAnimation
{
    public interface IAnimator : IDisposable
    {
        Action OnClipComplete { get; set; }
        Action<string> OnTrigger { get; set; }
        void Play(IClip clip);
        bool Play(string clipName);
        void Pause();
        void Resume();
        bool IsPlaying { get; }
        IClip CurrentClip { get; }
    }
}