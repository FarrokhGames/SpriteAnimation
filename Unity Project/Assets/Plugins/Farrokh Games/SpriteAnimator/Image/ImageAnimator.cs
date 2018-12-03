using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimator : AbstractSpriteAnimator, IImageAnimator
    {
        [SerializeField] bool _forceNativeSize = true;
        [SerializeField] bool _allowFlipping = true;
        [SerializeField] bool _allowChangingColor = true;

        Image _image;
        ImageAnimator[] _imageChildren;
        Vector3 _startScale;

        /// <inheritdoc />
        protected override void Init()
        {
            _image = GetComponent<Image>();
            _startScale = _image.rectTransform.localScale;
            _imageChildren = GetListOfChildren<ImageAnimator>();
        }

        /// <inheritdoc />
        public bool Visible
        {
            get { return _image.enabled; }
            set
            {
                // Set visibility of children
                if (_animateChildren && _imageChildren != null && _imageChildren.Length > 0)
                {
                    for (var i = 0; i < _imageChildren.Length; i++)
                    {
                        var child = _imageChildren[i];
                        if (child != null) { child.Visible = value; }
                    }
                }

                _image.enabled = value;
            }
        }

        /// <inheritdoc />
        public bool Flip
        {
            get { return _image.rectTransform.localScale.x == -1; }
            set
            {
                // Flip children
                if (_animateChildren && _imageChildren != null && _imageChildren.Length > 0)
                {
                    for (var i = 0; i < _imageChildren.Length; i++)
                    {
                        var child = _imageChildren[i];
                        if (child != null) { child.Flip = value; }
                    }
                }

                if (_allowFlipping) { _image.rectTransform.localScale = new Vector3(value ? -_startScale.x : _startScale.x, _startScale.y, _startScale.z); }
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _image.color; }
            set
            {
                // Change color of children
                if (_animateChildren && _imageChildren != null && _imageChildren.Length > 0)
                {
                    for (var i = 0; i < _imageChildren.Length; i++)
                    {
                        var child = _imageChildren[i];
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
            if (_forceNativeSize) { _image.SetNativeSize(); }
        }
    }
}