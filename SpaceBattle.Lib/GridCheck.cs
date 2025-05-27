using App;

namespace SpaceBattle.Lib
{
    public class GridCheck
    {
        private readonly int quadrantSize;

        private readonly IDictionary<string, List<IMoving>> quadrantObjects = new Dictionary<string, List<IMoving>>();
        private readonly IDictionary<IMoving, List<string>> objectQuadrants = new Dictionary<IMoving, List<string>>();

        public GridCheck()
        {
            quadrantSize = Ioc.Resolve<int>("Field.Quadrant.Size");
        }

        private string GetQuadrantKey(int x, int y) => $"{x},{y}";

        public void UpdateObjectQuadrants(IMoving obj, int[,] matrix)
        {
            if (objectQuadrants.TryGetValue(obj, out var oldQuadrants))
            {
                oldQuadrants.ForEach(quadrantKey =>
                {
                    if (quadrantObjects.TryGetValue(quadrantKey, out var ObjectInQuads))
                    {
                        ObjectInQuads.Remove(obj);
                    }
                });

                objectQuadrants.Remove(obj);
            }

            var newQuadrantKeys = GetOccupiedPoints(obj, matrix)
            .Select(point => (
                point.X / quadrantSize,
                point.Y / quadrantSize
            ))
            .Distinct()
            .Select(quadrant =>
            {
                var quadrantKey = GetQuadrantKey(quadrant.Item1, quadrant.Item2);

                if (!quadrantObjects.TryGetValue(quadrantKey, out var objectsList))
                {
                    objectsList = new List<IMoving>();
                    quadrantObjects[quadrantKey] = objectsList;
                }

                if (!objectsList.Contains(obj))
                {
                    objectsList.Add(obj);
                }

                return quadrantKey;
            })
            .ToList();

            objectQuadrants[obj] = newQuadrantKeys;
        }

        private static IEnumerable<(int X, int Y)> GetOccupiedPoints(IMoving obj, int[,] matrix)
        {
            return from y in Enumerable.Range(0, matrix.GetLength(0))
                   from x in Enumerable.Range(0, matrix.GetLength(1))
                   where matrix[y, x] == 1
                   select (
                       X: obj.Position.Values[0] + x,
                       Y: obj.Position.Values[1] + y
                   );
        }

        public List<IMoving> GetObjectsInSameQuadrant(int[] quadrantCoords)
        {
            var quadrantKey = GetQuadrantKey(quadrantCoords[0], quadrantCoords[1]);
            if (quadrantObjects.TryGetValue(quadrantKey, out var objectsList))
            {
                return objectsList;
            }

            return new List<IMoving>();
        }
    }
}
