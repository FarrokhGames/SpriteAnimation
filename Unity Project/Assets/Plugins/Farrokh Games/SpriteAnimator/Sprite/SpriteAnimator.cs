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

        SpriteRenderer _spriteRenderer;

        int _sortingOffset;

        /// <inheritdoc />
        public bool Visible
        {
            get { return _spriteRenderer.enabled; }
            set
            {
                _spriteRenderer.enabled = value;
                PerformOnChildren<SpriteAnimator>((child) => { child.enabled = value; });
            }
        }

        /// <inheritdoc />
        public int SortingOrder
        {
            get { return _spriteRenderer.sortingOrder; }
            set
            {
                _spriteRenderer.sortingOrder = value + _sortingOffset;
                PerformOnChildren<SpriteAnimator>((child) => { child.SortingOrder = value; });
            }
        }

        /// <inheritdoc />
        public bool Flip
        {
            get { return _spriteRenderer.flipX; }
            set
            {
                if (!_allowFlipping) { return; }
                _spriteRenderer.flipX = value;
                PerformOnChildren<SpriteAnimator>((child) => { child.Flip = value; });
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _spriteRenderer.color; }
            set
            {
                _spriteRenderer.color = value;
                PerformOnChildren<SpriteAnimator>((child) => { child.Color = value; });
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

        #region MonoBehavior (Unity)

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _sortingOffset = _spriteRenderer.sortingOrder;
        }

        #endregion
    }
}