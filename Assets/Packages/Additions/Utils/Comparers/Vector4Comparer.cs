using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.Comparers
{
    public class Vector4Comparer : Comparer<Vector4>
    {
        public override int Compare(Vector4 a, Vector4 b)
        {
            int v;
            if ((v = a.x.CompareTo(b.x)) != 0)
                return v;
            if ((v = a.y.CompareTo(b.y)) != 0)
                return v;
            if ((v = a.z.CompareTo(a.z)) != 0)
                return v;
            return a.w.CompareTo(b.w);
        }
    }
}