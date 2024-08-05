using GdLayers.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace GdLayers.Utils;

public static class GdObjectGroupUtils
{
    public static IEnumerable<GdObjectGroup> GetGdObjectGroups()
    {
        var sortedTypes = Encoding.UTF8.GetString(Properties.Resources.SortedTypes);
        return JsonConvert.DeserializeObject<IEnumerable<GdObjectGroup>>(sortedTypes)!;
    }
}
