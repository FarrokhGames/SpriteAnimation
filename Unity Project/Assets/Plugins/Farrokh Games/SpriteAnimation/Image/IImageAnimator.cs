using UnityEngine;

namespace FarrokhGames.SpriteAnimation
{
    /// <summary>
    /// UI-Image-based animator
    /// </summary>
    public interface IImageAnimator : IAnimator
    {
        /// <summary>
        /// Gets or sets wether this animator is visible or not
        /// </summary>
        bool Visible { get; set; }
    }
}