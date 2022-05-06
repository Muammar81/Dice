using System.Collections.Generic;
using System.Linq;

public static class Helpers
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> originalList)
    {
        var rnd = new System.Random();
        return originalList.OrderBy(item => rnd.Next()).ToList();
    }
}
