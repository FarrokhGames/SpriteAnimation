using UnityEngine;

namespace FarrokhGames.SpriteAnimation
{
    public interface ISpriteAnimator : IAnimator
    {
        bool Visible { get; set; }
        int SortingOrder { get; set; }
        bool Flip { get; set; }
        Color Color { get; set; }
        float Alpha { get; set; }
    }
}