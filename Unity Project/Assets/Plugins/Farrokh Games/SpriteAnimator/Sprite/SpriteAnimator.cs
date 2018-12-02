using FarrokhGames.SpriteAnimation.Shared;
using UnityEngine;

namespace FarrokhGames.SpriteAnimation.Sprite
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : AbstractSpriteAnimator, ISpriteAnimator
    {
        [SerializeField] UnityEngine.Sprite[] _sprites;

        SpriteRenderer _spriteRenderer;

        /// <inheritdoc />
        public bool Visible
        {
            get { return _spriteRenderer.enabled; }
            set { _spriteRenderer.enabled = value; }
        }

        /// <inheritdoc />
        public int SortingOrder
        {
            get { return _spriteRenderer.sortingOrder; }
            set { _spriteRenderer.sortingOrder = value; }
        }

        /// <inheritdoc />
        public bool Flip
        {
            get { return _spriteRenderer.flipX; }
            set { _spriteRenderer.flipX = value; }
        }

        /// <inheritdoc />
        public Color Color
        {
            get { return _spriteRenderer.color; }
            set { _spriteRenderer.color = value; }
        }

        /// <inheritdoc />
        public float Alpha
        {
            get { return _spriteRenderer.color.a; }
            set { _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, value); }
        }

        protected override void HandleFrameChanged(int index)
        {
            _spriteRenderer.sprite = _sprites[index];
        }

        #region MonoBehavior (Unity)

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #endregion
    }
}