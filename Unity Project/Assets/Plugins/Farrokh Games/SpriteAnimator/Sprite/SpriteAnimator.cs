using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    /// <inheritdoc />
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        [SerializeField, Tooltip("If false, any attempts to flip this sprite is suppressed")] bool _allowFlipping = true;

        SpriteRenderer _spriteRenderer;
        SpriteAnimator[] _spriteChildren = null;
        int _sortingOffset;

        /// <inheritdoc />
        protected override void Init()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _sortingOffset = _spriteRenderer.sortingOrder;
            _spriteChildren = GetListOfChildren<SpriteAnimator>();
        }

        /// <inheritdoc />
        public bool Visible
        {
            get { return _spriteRenderer.enabled; }
            set
            {
                // Set visibility of children
                if (_animateChildren && _spriteChildren != null && _spriteChildren.Length > 0)
                {
                    for (var i = 0; i < _spriteChildren.Length; i++)
                    {
                        var child = _spriteChildren[i];
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
                _spriteRenderer.sortingOrder = value + _sortingOffset;

                // Change offset of children
                if (_animateChildren && _spriteChildren != null && _spriteChildren.Length > 0)
                {
                    for (var i = 0; i < _spriteChildren.Length; i++)
                    {
                        var child = _spriteChildren[i];
                        if (child != null) { child.SortingOrder = value; }
                    }
                }
            }
        }

        /// <inheritdoc />
        public bool Flip
        {
            get { return _spriteRenderer.flipX; }
            set
            {
                // Flip children
                if (_animateChildren && _spriteChildren != null && _spriteChildren.Length > 0)
                {
                    for (var i = 0; i < _spriteChildren.Length; i++)
                    {
                        var child = _spriteChildren[i];
                        if (child != null) { child.Flip = value; }
                    }
                }

                if (_allowFlipping) { _spriteRenderer.flipX = value; }
            }
        }

        /// <inheritdoc />
        protected override void HandleFrameChanged(int index)
        {
            _spriteRenderer.sprite = _sprites[index];
        }
    }
}