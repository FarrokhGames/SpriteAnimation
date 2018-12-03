using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimator : AbstractSpriteAnimator, IImageAnimator
    {
        [SerializeField] bool _allowChangingColor = true;

        Image _image;
        ImageAnimator[] _children;

        /// <inheritdoc />
        public bool Visible
        {
            get { return _image.enabled; }
            set
            {
                // Set visibility of children
                if (_animateChildren && _children != null && _children.Length > 0)
                {
                    for (var i = 0; i < _children.Length; i++)
                    {
                        var child = _children[i];
                        if (child != null) { child.Visible = value; }
                    }
                }

                _image.enabled = value;
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _image.color; }
            set
            {
                // Change color of children
                if (_animateChildren && _children != null && _children.Length > 0)
                {
                    for (var i = 0; i < _children.Length; i++)
                    {
                        var child = _children[i];
                        if (child != null) { child.Color = value; }
                    }
                }

                if (_allowChangingColor) { _image.color = value; }
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
            _children = GetListOfChildren<ImageAnimator>();
        }

        #endregion
    }
}