
using UnityEngine;

namespace Additions.Utils.Rects
{
    public class HorizontalRectBuilder : RectBuilder
    {
        public float RemainingWidth => BaseSize.x - (CurrentX - BasePosition.x);

        public HorizontalRectBuilder(Rect rect) : base(rect) { }

        public HorizontalRectBuilder(Vector2 position, Vector2 size) : base(position, size) { }

        public HorizontalRectBuilder(Vector2 position, float width, float height) : base(position, width, height) { }

        public HorizontalRectBuilder(float x, float y, Vector2 size) : base(x, y, size) { }

        public HorizontalRectBuilder(float x, float y, float width, float height) : base(x, y, width, height) { }

        /// <summary>
        /// Produce a <see cref="Rect"/> next to the last <see cref="Rect"/> made with this object.<br>
        /// </summary>
        /// <param name="width">Width of the <see cref="Rect"/> to make.</param>
        /// <returns>New <see cref="Rect"/>./returns>
        public override Rect GetRect(float width)
        {
            Rect rect = new Rect(CurrentX, CurrentY, width, BaseSize.y);
            CurrentX += width;
            return rect;
        }

        /// <summary>
        /// Produce a <see cref="Rect"/> next to the last <see cref="Rect"/> made with this object.<br>
        /// </summary>
        /// <returns>New <see cref="Rect"/>.</returns>
        public override Rect GetRect()
        {
            LastValue = BaseSize.x;
            return GetRect(BaseSize.x);
        }

        /// <summary>
        /// Increase <see cref="RectBuilder.CurrentX"/> in order to produce an horizontal space.
        /// </summary>
        /// <param name="value">Space size.</param>
        public override void AddSpace(float value) => CurrentX += value;

        /// <summary>
        /// Produce a <see cref="Rect"/> next to the last <see cref="Rect"/> made with this object, using all the <see cref="RemainingWidth"/> as width.
        /// </summary>
        /// <param name="amount">Amount of <see cref="Rect"/> to make. The width will be equally splitted among them.</param>
        /// <returns>New <see cref="Rect"/>./returns>
        public Rect[] GetRemainingRect(int amount)
        {
            float width = RemainingWidth / amount;
            Rect[] rects = new Rect[amount];
            for (int i = 0; i < rects.Length; i++)
            {
                rects[i] = GetRect(width);
            }
            return rects;
        }

        public Rect GetRemainingRect() => GetRect(RemainingWidth);
    }
}