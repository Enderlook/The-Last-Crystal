﻿using UnityEngine;

namespace AdditionalExtensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Returns absolute <seealso cref="Vector2"/> of <paramref name="source"/>.
        /// </summary>
        /// <param name="source"><seealso cref="Vector2"/> to become absolute.</param>
        /// <returns>Absolute <seealso cref="Vector2"/>.</returns>
        public static Vector2 Abs(this Vector2 source) => new Vector2(Mathf.Abs(source.x), Mathf.Abs(source.y));
        /// <summary>
        /// Returns absolute <seealso cref="Vector3"/> of <paramref name="source"/>.
        /// </summary>
        /// <param name="source"><seealso cref="Vector3"/> to become absolute.</param>
        /// <returns>Absolute <seealso cref="Vector3"/>.</returns>
        public static Vector3 Abs(this Vector3 source) => new Vector3(Mathf.Abs(source.x), Mathf.Abs(source.y), Mathf.Abs(source.z));
        /// <summary>
        /// Returns absolute <seealso cref="Vector4"/> of <paramref name="source"/>.
        /// </summary>
        /// <param name="source"><seealso cref="Vector4"/> to become absolute.</param>
        /// <returns>Absolute <seealso cref="Vector4"/>.</returns>
        public static Vector4 Abs(this Vector4 source) => new Vector4(Mathf.Abs(source.x), Mathf.Abs(source.y), Mathf.Abs(source.z), Mathf.Abs(source.w));

        /// <summary>
        /// Returns the angle of the vector in degrees. Through Tan method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in degrees.</returns>
        public static float AngleByTg(this Vector2 origin, Vector2 target)
        {
            float Atg(float tg) => Mathf.Atan(tg) * 180 / Mathf.PI;
            Vector2 tO = origin.XYDistance(target);
            float tan = tO.y / tO.x;
            return Mathf.Round(Atg(tan));
        }

        /// <summary>
        /// Returns the angle of the vector in degrees. Through Sin method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in degrees.</returns>
        public static float AngleBySin(this Vector2 origin, Vector2 target)
        {
            float Asin(float s) => Mathf.Asin(s) * 180 / Mathf.PI;
            Vector2 tO = origin.XYDistance(target);
            float magnitude = tO.magnitude;
            float sin = tO.y / magnitude;
            return Mathf.Round(Asin(sin));
        }

        /// <summary>
        /// Returns the angle of the vector in degrees. Through Cos method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in degrees.</returns>
        public static float AngleByCos(this Vector2 origin, Vector2 target)
        {
            float Acos(float c) => Mathf.Acos(c) * 180 / Mathf.PI;
            Vector2 tO = origin.XYDistance(target);
            float magnitude = tO.magnitude;
            float cos = tO.x / magnitude;
            float result = cos >= 0 ? Mathf.Round(Acos(cos)) : Mathf.Round(Acos(-cos));
            return result;
        }

        /// <summary>
        /// Returns the angle of the vector in radians. Through Tan method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in radians.</returns>
        public static float AngleByTgRadian(this Vector2 origin, Vector2 target)
        {
            Vector2 tO = origin.XYDistance(target);
            float tan = tO.y / tO.x;
            return tan;
        }

        /// <summary>
        /// Returns the angle of the vector in radians. Through Sin method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in radians.</returns>
        public static float AngleBySinRadian(this Vector2 origin, Vector2 target)
        {
            Vector2 tO = origin.XYDistance(target);
            float magnitude = tO.magnitude;
            float sin = tO.y / magnitude;
            return sin;
        }

        /// <summary>
        /// Returns the angle of the vector in radians. Through Cos method generated by the origin and the target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the vector start.</param>
        /// <param name="target">The point in 2D space where the vector ends.</param>
        /// <returns><seealso cref="float"/> angle in radians.</returns>
        public static float AngleByCosRadian(this Vector2 origin, Vector2 target)
        {
            Vector2 tO = origin.XYDistance(target);
            float magnitude = tO.magnitude;
            float cos = tO.x / magnitude;
            float result = cos >= 0 ? cos : -cos;
            return result;
        }

        /// <summary>
        /// Generates a projectile motion between origin and target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the projectile motion start.</param>
        /// <param name="target">The point in 2D space where the projectile motion ends.</param>
        /// <returns><seealso cref="Vector2"/> with the initial momentun.</returns>
        public static Vector2 ProjectileMotion(this Vector2 origin, Vector2 target)
        {
            float Vx(float x) => x / origin.AngleByCosRadian(target);
            float Vy(float y) => y / Mathf.Abs(origin.AngleBySinRadian(target)) + .5f * Mathf.Abs(Physics2D.gravity.y);

            float hY = origin.YDistance(target);
            float dX = origin.XDistance(target);

            Vector2 v0 = new Vector2(dX, 0).normalized;
            v0 *= Vx(Mathf.Abs(dX));
            v0.y = Vy(hY);

            return v0;
        }

        /// <summary>
        /// Generates a projectile motion between origin and target.
        /// </summary>
        /// <param name="origin">The point in 2D space where the projectile motion start.</param>
        /// <param name="target">The point in 2D space where the projectile motion ends.</param>
        /// <param name="t">The time of flight of a projectile motion.</param>
        /// <returns><seealso cref="Vector2"/> with the initial momentun.</returns>
        public static Vector2 ProjectileMotion(this Vector2 origin, Vector2 target, float t)
        {
            float Vx(float x) => x / origin.AngleByCosRadian(target) * t;
            float Vy(float y) => y / (Mathf.Abs(origin.AngleBySinRadian(target)) * t) + .5f * Mathf.Abs(Physics2D.gravity.y) * t;

            float hY = origin.YDistance(target);
            float dX = origin.XDistance(target);

            Vector2 v0 = new Vector2(dX, 0).normalized;
            v0 *= Vx(Mathf.Abs(dX));
            v0.y = Vy(hY);

            return v0;
        }

        /// <summary>
        /// Returns the distance between a and b on the X axis.
        /// </summary>
        /// <param name="source"><seealso cref="Vector2"/>Start point.</param>
        /// <param name="target"><seealso cref="Vector2"/>End point.</param>
        /// <returns><seealso cref="float"/> value.</returns>
        public static float XDistance(this Vector2 source, Vector2 target) => target.x - source.x;

        /// <summary>
        /// Returns the distance between a and b on the X axis.
        /// </summary>
        /// <param name="source"><seealso cref="Vector3"/>Start point.</param>
        /// <param name="target"><seealso cref="Vector3"/>End point.</param>
        /// <returns><seealso cref="float"/> value.</returns>
        public static float XDistance(this Vector3 source, Vector3 target) => target.x - source.x;

        /// <summary>
        /// Returns the distance between a and b on the Y axis.
        /// </summary>
        /// <param name="source"><seealso cref="Vector2"/>Start point.</param>
        /// <param name="target"><seealso cref="Vector2"/>End point.</param>
        /// <returns><seealso cref="float"/> value.</returns>
        public static float YDistance(this Vector2 source, Vector2 target) => target.y - source.y;

        /// <summary>
        /// Returns the distance between a and b on the Y axis.
        /// </summary>
        /// <param name="source"><seealso cref="Vector3"/>Start point.</param>
        /// <param name="target"><seealso cref="Vector3"/>End point.</param>
        /// <returns><seealso cref="float"/> value.</returns>
        public static float YDistance(this Vector3 source, Vector3 target) => target.y - source.y;

        /// <summary>
        /// Returns the distance between a and b on the Z axis.
        /// </summary>
        /// <param name="source">Start point.</param>
        /// <param name="target">End point.</param>
        /// <returns><seealso cref="float"/> value.</returns>
        public static float ZDistance(this Vector3 source, Vector3 target) => target.z - source.z;

        /// <summary>
        /// Returns the distance between a and b on the X - Y axis.
        /// </summary>
        /// <param name="source">Start point.</param>
        /// <param name="target">End point.</param>
        /// <returns><seealso cref="Vector2"/> with the distance.</returns>
        public static Vector2 XYDistance(this Vector2 source, Vector2 target) => target - source;

        /// <summary>
        /// Returns the distance between a and b on the X - Y - Z axis.
        /// </summary>
        /// <param name="source">Start point.</param>
        /// <param name="target">End point.</param>
        /// <returns><seealso cref="Vector3"/> with the distance.</returns>
        public static Vector3 XYZDistance(this Vector3 source, Vector3 target) => target - source;

        public static Vector2Int ToVector2Int(this Vector2 source) => new Vector2Int((int)source.x, (int)source.y);
        public static Vector2Int ToVector2Int(this Vector3 source) => ToVector2Int(source);
        public static Vector3Int ToVector3Int(this Vector3 source) => new Vector3Int((int)source.x, (int)source.y, (int)source.z);
        public static Vector3Int ToVector3Int(this Vector2 source) => ToVector3Int(source);
    }
}