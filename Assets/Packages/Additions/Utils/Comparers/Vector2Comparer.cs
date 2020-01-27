using System.Collections.Generic;

using UnityEngine;

namespace Additions.Utils.Comparers
{
    public class Vector2Comparer : Comparer<Vector2>
    {
        public override int Compare(Vector2 a, Vector2 b)
        {
            int v;
            if ((v = a.x.CompareTo(b.x)) != 0)
                return v;
            return a.y.CompareTo(b.y);
        }
    }
}