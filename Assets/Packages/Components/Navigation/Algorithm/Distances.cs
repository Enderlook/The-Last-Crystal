using UnityEngine;

namespace Navigation
{
    public static class Distances
    {
        // https://codereview.stackexchange.com/questions/120933/calculating-distance-with-euclidean-manhattan-and-chebyshev-in-c
        public static float CalculateEuclideanDistance(Vector2 v1, Vector2 v2)
        {
            // Vector2.Distance(start, end) does the same...
            Vector2 difference = v1 - v2;
            return Mathf.Sqrt(difference.x * difference.x + difference.y * difference.y);
        }

        public static float CalculateManhattanDistance(Vector2 v1, Vector2 v2)
        {
            Vector2 difference = v1 - v2;
            return Mathf.Abs(difference.x) + Mathf.Abs(difference.y);
        }

        public static float CalculateChebyshovDistance(Vector2 v1, Vector2 v2)
        {
            Vector2 difference = v2 - v1;
            return Mathf.Max(Mathf.Abs(difference.x) + Mathf.Abs(difference.y));
        }
    }
}