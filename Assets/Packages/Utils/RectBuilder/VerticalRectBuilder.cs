
using UnityEngine;

namespace Additions.Utils.Rects
{
    public class VerticalRectBuilder : RectBuilder
    {
        public float TotalHeight => CurrentY - BasePosition.y;

        public VerticalRectBuilder(Rect rect) : base(rect) { }

        public VerticalRectBuilder(Vector2 position, Vector2 size) : base(position, size) { }

        public VerticalRectBuilder(Vector2 position, float width, float height) : base(position, width, height) { }

        public VerticalRectBuilder(float x, float y, Vector2 size) : base(x, y, size) { }

        public VerticalRectBuilder(float x, float y, float width, float height) : base(x, y, width, height) { }

        /// <summary>
        /// Produce a <see cref="Rect"/> next to the last <see cref="Rect"/> made with this object.
        /// </summary>
        /// <param name="height">height of the <see cref="Rect"/> to make.</param>
        /// <returns>New <see cref="Rect"/>./returns>
        public override Rect GetRect(float height)
        {
            Rect rect = new Rect(CurrentX, CurrentY, BaseSize.x, height);
            CurrentY += height;
            return rect;
        }

        /// <summary>
        /// Produce a <see cref="Rect"/> next to the last <see cref="Rect"/> made with this object.
        /// </summary>
        /// <returns>New <see cref="Rect"/>./returns>
        public override Rect GetRect()
        {
            LastValue = BaseSize.y;
            return GetRect(BaseSize.y);
        }

        /// <summary>
        /// Increase <see cref="RectBuilder.CurrentY"/> in order to produce a vertical space.
        /// </summary>
        /// <param name="value">Space size.</param>
        public override void AddSpace(float value) => CurrentY += value;
    }
}