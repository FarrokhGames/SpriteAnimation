using System.Linq;
using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    /// <inheritdoc />
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        SpriteRenderer _spriteRenderer;
        IAnimator[] _children;
        int _originalSortingOrder;

        /// <inheritdoc />
        protected override void Init()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalSortingOrder = _spriteRenderer.sortingOrder;
            _children = GetComponentsInChildren<IAnimator>().Where(x => !x.Equals(this)).ToArray();
        }

        /// <inheritdoc />
        protected override void HandleFrameChanged(int index)
        {
            _spriteRenderer.sprite = _sprites[index];
        }

        /// <inheritdoc />
        public bool Visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
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
                        if (child != null && child is ISpriteAnimator)
                        {
                            (child as ISpriteAnimator).SortingOrder = value;
                        }
                    }
                }

                _spriteRenderer.sortingOrder = value + _originalSortingOrder;
            }
        }

        /// <inheritdoc />
        public override bool Flip
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