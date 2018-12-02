using System;
using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        [SerializeField] UnityEngine.Sprite[] _sprites;
        [SerializeField] bool _allowFlipping = true;

        SpriteRenderer _spriteRenderer;
        SpriteAnimator[] _children;

        int _sortingOffset;

        /// <inheritdoc />
        public bool Visible
        {
            get { return _spriteRenderer.enabled; }
            set
            {
                _spriteRenderer.enabled = value;
                PerformOnChildren((child) => { child.enabled = value; });
            }
        }

        /// <inheritdoc />
        public int SortingOrder
        {
            get { return _spriteRenderer.sortingOrder; }
            set
            {
                _spriteRenderer.sortingOrder = value + _sortingOffset;
                PerformOnChildren((child) => { child.SortingOrder = value; });
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
                PerformOnChildren((child) => { child.Flip = value; });
            }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _spriteRenderer.color; }
            set
            {
                _spriteRenderer.color = value;
                PerformOnChildren((child) => { child.Color = value; });
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

        void PerformOnChildren(Action<SpriteAnimator> method)
        {
            if (_children != null && _children.Length > 0)
            {
                for (var i = 0; i < _children.Length; i++)
                {
                    var child = _children[i];
                    if (child != null) { method(child); }
                }
            }
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _sortingOffset = _spriteRenderer.sortingOrder;

            var children = GetComponentsInChildren<SpriteAnimator>();
            _children = children.Where(x => x != this).ToArray();
        }

        #endregion
    }
}