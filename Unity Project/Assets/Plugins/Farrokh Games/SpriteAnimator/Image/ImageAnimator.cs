using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(Image))]
    public class ImageAnimator : AbstractSpriteAnimator, IImageAnimator
    {
        [SerializeField, Tooltip("If true, the image will update to its native size whenever it changes")] bool _forceNativeSize = true;
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] bool _allowFlipping = true;

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
        protected override void HandleFrameChanged(int index)
        {
            _image.sprite = _sprites[index];
            if (_forceNativeSize) { _image.SetNativeSize(); }
        }
    }
}