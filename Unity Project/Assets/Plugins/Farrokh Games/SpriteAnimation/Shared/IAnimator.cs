using System;
using FarrokhGames.SpriteAnimation.Shared;

namespace FarrokhGames.SpriteAnimation
{
    /// <summary>
    /// Shared interface for all animators
    /// </summary>
    public interface IAnimator : IDisposable
    {
        /// <summary>
        /// Invoked when the current clip has completed its animation
        /// </summary>
        Action OnClipComplete { get; set; }

        /// <summary>
        /// Invoked when playing a frame with a trigger on it
        /// </summary>
        Action<string> OnTrigger { get; set; }

        /// <summary>
        /// Returns true if an animation is currently playing
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Returns the current clip
        /// </summary>
        IClip CurrentClip { get; }

        /// <summary>
        /// Plays the given clip
        /// </summary>
        /// <param name="clip">The clip to play</param>
        void Play(IClip clip);

        /// <summary>
        /// Tries to find and play a clip of given name
        /// </summary>
        /// <param name="clipName">The name of the clip to play</param>
        bool Play(string clipName);

        /// <summary>
        /// Pauses the current animation
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the current animation
        /// </summary>
        void Resume();

        /// <summary>
        /// Gets or sets wether to flip this animator or not
        /// </summary>
        bool Flip { get; set; }

        /// <summary>
        /// How to handle the relationship with a parent animator
        /// </summary>
        AnimatorChildMode ChildMode { get; }
    }
}