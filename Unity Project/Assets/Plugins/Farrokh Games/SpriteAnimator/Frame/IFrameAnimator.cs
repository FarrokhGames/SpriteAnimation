using System;

namespace FarrokhGames.SpriteAnimation
{
    public interface IFrameAnimator : IDisposable
    {
        Action OnClipComplete { get; set; }
        Action<int> OnFrameChanged { get; set; }
        Action<string> OnTrigger { get; set; }
        void Play(IClip clip);
        void Pause();
        void Resume();
        bool IsPlaying { get; }
        IClip CurrentClip { get; }
        void Tick(float deltaTime);
    }

    public interface IClip
    {
        string Name { get; }
        bool Loop { get; }
        bool RandomStart { get; }
        float FrameRate { get; }
        int Length { get; }
        IFrame this [int index] { get; }
    }

    public interface IFrame
    {
        int Index { get; }
        float Speed { get; }
        string TriggerName { get; }
    }
}