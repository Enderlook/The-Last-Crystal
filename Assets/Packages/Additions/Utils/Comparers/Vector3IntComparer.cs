using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.Comparers
{
    public class Vector3IntComparer : Comparer<Vector3Int>
    {
        public override int Compare(Vector3Int a, Vector3Int b)
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