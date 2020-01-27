using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.Comparers
{
    public class Vector3Comparer : Comparer<Vector3>
    {
        public override int Compare(Vector3 a, Vector3 b)
        {
            int v;
            if ((v = a.x.CompareTo(b.x)) != 0)
                return v;
            if ((v = a.y.CompareTo(b.y)) != 0)
                return v;
            return a.z.CompareTo(b.z);
        }
    }
}