using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        [SerializeField] bool _allowFlipping = true;
        [SerializeField] bool _allowChangingColor = true;

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
        public Color Color
        {
            get { return _spriteRenderer.color; }
            set
            {
                // Change color of children
                if (_animateChildren && _spriteChildren != null && _spriteChildren.Length > 0)
                {
                    for (var i = 0; i < _spriteChildren.Length; i++)
                    {
                        var child = _spriteChildren[i];
                        if (child != null) { child.Color = value; }
                    }
                }

                if (_allowChangingColor) { _spriteRenderer.color = value; }
            }
        }

        /// <inheritdoc />
        public float Alpha
        {
            get { return Color.a; }
            set { Color = new Color(Color.r, Color.g, Color.b, value); }
        }

        protected override void HandleFrameChanged(int index)
        {
            _spriteRenderer.sprite = _sprites[index];
        }
    }
}