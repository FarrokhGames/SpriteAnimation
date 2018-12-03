using UnityEngine;

namespace FarrokhGames.SpriteAnimation
{
    public interface IImageAnimator : IAnimator
    {
        /// <summary>
        /// Gets or sets wether this animator is visible or not
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets wether to flip this animator or not
        /// </summary>
        bool Flip { get; set; }
    }
}