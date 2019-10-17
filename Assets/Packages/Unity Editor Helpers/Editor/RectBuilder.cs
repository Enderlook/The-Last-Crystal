using System;
using UnityEngine;

namespace UnityEditorHelper
{
    public abstract class RectBuilder
    {
        public float CurrentX { get; protected set; }
        public float CurrentY { get; protected set; }

        public Vector2 BasePosition { get; private set; }
        public Vector2 BaseSize { get; private set; }

        protected float LastValue { get; set; }

        private void Construct(Vector2 position, Vector2 size)
        {
            BasePosition = position;
            BaseSize = size;
            CurrentX = position.x;
            CurrentY = position.y;
        }

        protected RectBuilder(Rect rect) => Construct(rect.position, rect.size);
        protected RectBuilder(Vector2 position, Vector2 size) => Construct(position, size);
        protected RectBuilder(Vector2 position, float width, float height) => Construct(position, new Vector2(width, height));
        protected RectBuilder(float x, float y, Vector2 size) => Construct(new Vector2(x, y), size);
        protected RectBuilder(float x, float y, float width, float height) => Construct(new Vector2(x, y), new Vector2(width, height));

        public virtual Rect GetRect(float value)
        {
            LastValue = value;
            throw new NotImplementedException("This method must be overriden by the class child");
        }
        public abstract Rect GetRect();

        /// <summary>
        /// Produces a rect using the last size configuration.
        /// </summary>
        /// <returns>A rect with the last size configuration.</returns>
        public Rect GetRectWithLastConfiguration() => GetRect(LastValue);

        public abstract void AddSpace(float value);
    }
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

    public class VerticalRectBuilder : RectBuilder
    {
        public float TotalHeight => CurrentY - BaseSize.y;

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
            Rect rect = new Rect(CurrentX, CurrentY, height, BaseSize.y);
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