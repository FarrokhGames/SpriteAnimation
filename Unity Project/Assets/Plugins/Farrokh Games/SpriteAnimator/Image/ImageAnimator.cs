using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimator : AbstractSpriteAnimator, IImageAnimator
    {
        Image _image;

        /// <inheritdoc />
        public bool Visible
        {
            get { return _image.enabled; }
            set
            {
                _image.enabled = value;
                PerformOnChildren<SpriteAnimator>((child) => { child.enabled = value; });
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _image.color; }
            set
            {
                _image.color = value;
                PerformOnChildren<SpriteAnimator>((child) => { child.Color = value; });
            }
        }

        /// <inheritdoc />
        public float Alpha
        {
            get { return _image.color.a; }
            set { Color = new Color(Color.r, Color.g, Color.b, value); }
        }

        protected override void HandleFrameChanged(int index)
        {
            _image.sprite = _sprites[index];
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        #endregion
    }
}