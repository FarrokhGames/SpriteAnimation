using System;

namespace FarrokhGames.SpriteAnimation
{
    /// <summary>
    /// Returns frame changes when playing given clips. Used as a base for other, more concerete, animators.
    /// </summary>
    public interface IFrameAnimator : IDisposable
    {
        /// <summary>
        /// Invoked when the current clip has completed its animation
        /// </summary>
        Action OnClipComplete { get; set; }

        /// <summary>
        /// Invoked when the current frame was changed.
        /// int - The index of the new frame
        /// </summary>
        /// <value></value>
        Action<int> OnFrameChanged { get; set; }

        /// <summary>
        /// Invoked when playing a frame with a trigger on it
        /// string - The name of the trigger
        /// </summary>
        Action<string> OnTrigger { get; set; }

        /// <summary>
        /// Plays the given clip
        /// </summary>
        /// <param name="clip">The clip to play</param>
        void Play(IClip clip);

        /// <summary>
        /// Tries to find and play a clip of given name
        /// </summary>
        /// <param name="clipName">The name of the clip to play</param>
        bool Play(string name);

        /// <summary>
        /// Pauses the current animation
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes the current animation
        /// </summary>
        void Resume();

        /// <summary>
        /// Returns true if an animation is currently playing
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Returns the current clip
        /// </summary>
        IClip CurrentClip { get; }

        /// <summary>
        /// Returns true if this animator can play clips of parent animator
        /// </summary>
        bool AllowClipSharing { get; }

        /// <summary>
        /// Ticks the animator using given delta time
        /// This must be done in order to run the animations
        /// </summary>
        /// <param name="deltaTime">The time delta</param>
        void Tick(float deltaTime);

        /// <summary>
        /// Sets the children of this animator
        /// </summary>
        void SetChildren(IFrameAnimator[] children);
    }

    /// <summary>
    /// Contains information about a single animation clip
    /// </summary>
    public interface IClip
    {
        /// <summary>
        /// Returns the name of the clip
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns wether this clip loops or not.
        /// Note that looping clips never actually completes
        /// </summary>
        bool Loop { get; }

        /// <summary>
        /// Returns true if this clip should start on a random frame or the first
        /// </summary>
        bool RandomStart { get; }

        /// <summary>
        /// Returns the framerate (in frames per seconds) of this clip
        /// </summary>
        float FrameRate { get; }

        /// <summary>
        /// Returns the number of frames in this clip
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// Returns the frame at given index
        /// </summary>
        IFrame this [int index] { get; }
    }

    /// <summary>
    /// Contains information about a single frame
    /// </summary>
    public interface IFrame
    {
        /// <summary>
        /// Returns the index of this frame
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Returns the speed modifier of this frame (usually 1.0)
        /// Use this to speed up or slow down a single frame
        /// </summary>
        float Speed { get; }

        /// <summary>
        /// Returns true if this frame contains a trigger
        /// </summary>
        bool HasTrigger { get; }

        /// <summary>
        /// Returns the name of the trigger associated with this frame.
        /// </summary>
        string TriggerName { get; }
    }
}