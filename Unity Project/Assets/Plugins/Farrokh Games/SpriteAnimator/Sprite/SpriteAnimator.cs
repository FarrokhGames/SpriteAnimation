using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    /// <inheritdoc />
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        [SerializeField, Tooltip("If false, any attempts to flip this image is suppressed")] bool _allowFlipping = true;

        SpriteRenderer _spriteRenderer;
        ISpriteAnimator[] _children;
        int _originalSortingOrder;

        /// <inheritdoc />
        protected override void Init()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalSortingOrder = _spriteRenderer.sortingOrder;
            _children = GetComponentsInChildren<ISpriteAnimator>().Where(x => !x.Equals(this)).ToArray();
        }

        /// <inheritdoc />
        protected override void HandleFrameChanged(int index)
        {
            _spriteRenderer.sprite = _sprites[index];
        }

        /// <inheritdoc />
        public bool Visible
        {
            get { return _spriteRenderer.enabled; }
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

                _spriteRenderer.enabled = value;
            }
        }

        /// <inheritdoc />
        public int SortingOrder
        {
            get { return _spriteRenderer.sortingOrder; }
            set
            {
                // Set sorting order of children
                if (_animateChildren && _children != null && _children.Length > 0)
                {
                    for (var i = 0; i < _children.Length; i++)
                    {
                        var child = _children[i];
                        if (child != null) { child.SortingOrder = value; }
                    }
                }

                _spriteRenderer.sortingOrder = value + _originalSortingOrder;
            }
        }

        /// <inheritdoc />
        public bool Flip
        {
            get { return _spriteRenderer.flipX; }
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

                if (_allowFlipping) { _spriteRenderer.flipX = value; }
            }
        }
    }
}