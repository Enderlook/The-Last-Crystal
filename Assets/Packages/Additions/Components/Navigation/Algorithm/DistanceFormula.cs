namespace Additions.Components.Navigation
{
    public enum DistanceFormula
    {
        Euclidean, // Squared grids that allow any direction
        Manhattan, // Squared grids that only allow 4 directions of movement ('+' shape)
        Chebyshov, // Squared grids that allow 8 directions of movement ('+' and 'x' shapes)
        None, // Dijkstra without heuristic distance
    };
}