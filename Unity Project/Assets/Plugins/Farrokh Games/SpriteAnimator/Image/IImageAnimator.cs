using UnityEngine;

namespace FarrokhGames.SpriteAnimation
{
    public interface IImageAnimator : IAnimator
    {
        bool Visible { get; set; }
        bool Flip { get; set; }
        Color Color { get; set; }
        float Alpha { get; set; }
    }
}