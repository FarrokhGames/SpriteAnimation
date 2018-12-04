namespace FarrokhGames.SpriteAnimation
{
    /// <summary>
    /// SpriteRenderer-based animator
    /// </summary>
    public interface ISpriteAnimator : IAnimator
    {
        /// <summary>
        /// Gets or sets wether this animator is visible or not
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the sorting order of this animator while keeping its offset
        /// </summary>
        /// <value></value>
        int SortingOrder { get; set; }
    }
}