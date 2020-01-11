using System;

using UnityEngine;

namespace Additions.Utils.Rects
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
            throw new NotImplementedException("This method must be overridden by the class child");
        }

        public abstract Rect GetRect();

        /// <summary>
        /// Produces a <see cref="Rect"/> using the last size configuration.
        /// </summary>
        /// <returns>A <see cref="Rect"/> with the last size configuration.</returns>
        public Rect GetRectWithLastConfiguration() => GetRect(LastValue);

        public abstract void AddSpace(float value);
    }
}