using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    /// <inheritdoc />
    [RequireComponent(typeof(Image))]
    public class ImageAnimator : AbstractSpriteAnimator, IImageAnimator
    {
        [SerializeField, Tooltip("If true, the image will update to its native size whenever it changes")] bool _forceNativeSize = true;
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] bool _allowFlipping = true;

        Image _image;
        IImageAnimator[] _children;
        Vector3 _startScale;

        /// <inheritdoc />
        protected override void Init()
        {
            _image = GetComponent<Image>();
            _startScale = _image.rectTransform.localScale;
            _children = GetComponentsInChildren<IImageAnimator>().Where(x => !x.Equals(this)).ToArray();
        }

        /// <inheritdoc />
        protected override void HandleFrameChanged(int index)
        {
            _image.sprite = _sprites[index];
            if (_forceNativeSize) { _image.SetNativeSize(); }
        }

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
        public bool Flip
        {
            get { return _image.rectTransform.localScale.x == -1; }
            set
            {
                // Flip children
                if (_animateChildren && _children != null && _children.Length > 0)
                {
                    for (var i = 0; i < _children.Length; i++)
                    {
                        var child = _children[i];
                        if (child != null) { child.Flip = value; }
                    }
                }

                if (_allowFlipping) { _image.rectTransform.localScale = new Vector3(value ? -_startScale.x : _startScale.x, _startScale.y, _startScale.z); }
            }
        }
    }
}