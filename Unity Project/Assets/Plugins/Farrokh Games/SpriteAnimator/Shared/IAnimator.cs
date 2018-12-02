using System;

namespace FarrokhGames.SpriteAnimation
{
    public interface IAnimator : IDisposable
    {
        Action OnClipComplete { get; set; }
        Action<string> OnTrigger { get; set; }
        void Play(IClip clip, bool withChildren = true);
        bool Play(string clipName, bool withChildren = true);
        void Pause(bool withChildren = true);
        void Resume(bool withChildren = true);
        bool IsPlaying { get; }
        IClip CurrentClip { get; }
    }
}