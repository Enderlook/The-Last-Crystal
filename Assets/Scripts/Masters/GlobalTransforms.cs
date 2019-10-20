using System.Collections.Generic;
using UnityEngine;
using static Master.Global;

namespace Master
{
    public static partial class Global
    {
        public enum TransformCreature { Crystal, Warrior, Wizard };
    }

    public static class GlobalTransforms
    {
        private static readonly Dictionary<TransformCreature, Transform> transforms = new Dictionary<TransformCreature, Transform>();

        public static void ResetTransforms() => transforms.Clear();

        private static Transform CheckDestroyedTransform(this TransformCreature key, Transform transform)
        {
            if (transform == null)
            {
                key.ResetTransform();
                return null;
            }
            else
                return transform;
        }

        public static Transform GetTranform(this TransformCreature key) => transforms.TryGetValue(key, out Transform value) ? key.CheckDestroyedTransform(value) : null;

        public static bool TryGetTransform(this TransformCreature key, out Transform transform)
        {
            if (transforms.TryGetValue(key, out transform))
            {
                transform = CheckDestroyedTransform(key, transform);
                return transform == null;
            }
            return false;
        }

        public static void ResetTransform(this TransformCreature key) => transforms.Remove(key);

        public static void SetTransform(this TransformCreature key, Transform transform) => transforms[key] = transform;
    }
}