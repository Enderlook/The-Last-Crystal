using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.Comparers
{
    public class Vector2IntComparer : Comparer<Vector2Int>
    {
        public override int Compare(Vector2Int a, Vector2Int b)
        {
            int v;
            if ((v = a.x.CompareTo(b.x)) != 0)
                return v;
            return a.y.CompareTo(b.y);
        }
    }
}