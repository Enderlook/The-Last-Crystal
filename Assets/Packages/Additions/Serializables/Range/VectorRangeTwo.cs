using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Additions.Serializables.Ranges
{
    public abstract class VectorRangeTwo
    {
        /// <summary>
        /// Whenever it should return only the initial vector or a random vector made by two vectors.
        /// </summary>
        [Tooltip("Whenever it should return only the initial vector or a random vector made by two vectors.")]
        public bool isRandom = true;

        /// <summary>
        /// Return the start parameter in <see cref="Vector3"/>
        /// </summary>
        protected abstract Vector3 StartVector3 { get; set; }
        /// <summary>
        /// Return the end parameter in <see cref="Vector3"/>
        /// </summary>
        protected abstract Vector3 EndVector3 { get; set; }
        /// <summary>
        /// Whenever it should return only the initial vector or a random vector made by two vectors.
        /// </summary>
        protected Vector3 Vector3 {
            get {
                if (isRandom)
                    return new Vector3(Random.Range(StartVector3.x, EndVector3.x),
                        Random.Range(StartVector3.y, EndVector3.y),
                        Random.Range(StartVector3.z, EndVector3.z));
                else
                    return StartVector3;
            }
        }

        /// <summary>
        /// Return the start parameter in <see cref="Vector2"/>
        /// </summary>
        protected Vector2 StartVector2 => StartVector3;
        /// <summary>
        /// Return the end parameter in <see cref="Vector2"/>
        /// </summary>
        protected Vector2 EndVector2 => EndVector3;
        /// <summary>
        /// Return a <seealso cref="Vector2"/> position. If <see cref="IsRandom"/> is <see langword="true"/>, it will return a random <seealso cref="UnityEngine.Vector2"/> between the <see cref="StartVector2"/> and the <see cref="EndVector2"/>. On <see langword="false"/> it will return the position of the <see cref="StartVector2"/>.
        /// </summary>
        protected Vector2 Vector2 {
            get {
                if (isRandom)
                    return new Vector2(Random.Range(StartVector2.x, EndVector2.x),
                        Random.Range(StartVector2.y, EndVector2.y));
                else
                    return StartVector2;
            }
        }
    }

    [Serializable]
    public class TransformRange : VectorRangeTwo
    {
#pragma warning disable CA2235, CS0649
        [SerializeField, Tooltip("Start transform.")]
        private Transform startTransform;
        [SerializeField, Tooltip("End transform.")]
        private Transform endTransform;
#pragma warning restore CA2235, CS0649

        protected override Vector3 StartVector3 {
            get => startTransform.position;
            set => startTransform.position = value;
        }

        protected override Vector3 EndVector3 {
            get => endTransform.position;
            set => endTransform.position = value;
        }

        /// <summary>
        /// Return a <seealso cref="Vector3"/> position. If <see cref="VectorRangeTwo.isRandom"/> is <see langword="true"/> it will return the position of the <see cref="StartVector"/>. On <see langword="false"/>, it will return a random <seealso cref="Vector3"/> between the <see cref="StartVector"/> and the <see cref="EndVector"/>.
        /// </summary>
        /// <param name="x"><see cref="TransformRange"/> instance used to determine the random <seealso cref="Vector3"/>.</param>
        public static explicit operator Vector3(TransformRange x) => x.Vector3;
        /// <summary>
        /// Return a <seealso cref="Vector2"/> position. If <see cref="VectorRangeTwo.isRandom"/> is <see langword="true"/> it will return the position of the <see cref="StartVector"/>. On <see langword="false"/>, it will return a random <seealso cref="Vector2"/> between the <see cref="StartVector"/> and the <see cref="EndVector"/>.
        /// </summary>
        /// <param name="x"><see cref="TransformRange"/> instance used to determine the random <seealso cref="Vector3"/>.</param>
        public static explicit operator Vector2(TransformRange x) => x.Vector2;
    }

    [Serializable]
    public class Vector2RangeTwo : VectorRangeTwo
    {
#pragma warning disable CA2235
        [SerializeField, Tooltip("Start vector.")]
        private Vector2 startVector;

        [SerializeField, Tooltip("End vector.")]
        private Vector2 endVector;
#pragma warning restore CA2235

        protected override Vector3 StartVector3 {
            get => startVector;
            set => startVector = value;
        }

        protected override Vector3 EndVector3 {
            get => endVector;
            set => endVector = value;
        }

        /// <summary>
        /// Return a <seealso cref="Vector3"/> position. If <see cref="VectorRangeTwo.isRandom"/> is <see langword="true"/> it will return the position of the <see cref="StartVector"/>. On <see langword="false"/>, it will return a random <seealso cref="Vector3"/> between the <see cref="StartVector"/> and the <see cref="EndVector"/>.
        /// </summary>
        /// <param name="x"><see cref="Vector2RangeTwo"/> instance used to determine the random <seealso cref="Vector3"/>.</param>
        public static explicit operator Vector3(Vector2RangeTwo x) => x.Vector3;

        /// <summary>
        /// Return a <seealso cref="Vector2"/> position. If <see cref="VectorRangeTwo.isRandom"/> is <see langword="true"/> it will return the position of the <see cref="StartVector"/>. On <see langword="false"/>, it will return a random <seealso cref="Vector2"/> between the <see cref="StartVector"/> and the <see cref="EndVector"/>.
        /// </summary>
        /// <param name="x"><see cref="Vector2RangeTwo"/> instance used to determine the random <seealso cref="Vector3"/>.</param>
        public static explicit operator Vector2(Vector2RangeTwo x) => x.Vector2;

        /// <summary>
        /// Multiplicatives a given range of two <seealso cref="Vector2"/> (<seealso cref="Vector2RangeTwo"/>) with a <see langword="float"/>.<br/>
        /// The float multiplies each <seealso cref="Vector2"/>.
        /// </summary>
        /// <param name="left"><see cref="Vector2RangeTwo"/> to multiply.</param>
        /// <param name="right"><see langword="float"/> to multiply.</param>
        /// <returns>The multiplication of the <seealso cref="Vector2"/> inside <paramref name="left"/> with the number <paramref name="right"/>.</returns>
        public static Vector2RangeTwo operator *(Vector2RangeTwo left, float right) => new Vector2RangeTwo { startVector = left.startVector * right, endVector = left.endVector * right, isRandom = left.isRandom };
    }
}