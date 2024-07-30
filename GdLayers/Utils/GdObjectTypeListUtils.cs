using GdLayers.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace GdLayers.Utils;

public static class GdObjectTypeListUtils
{
    public static IEnumerable<GdObjectTypeList> GetObjectTypeList()
    {
        var sortedTypes = Encoding.UTF8.GetString(Properties.Resources.SortedTypes);
        return JsonConvert.DeserializeObject<IEnumerable<GdObjectTypeList>>(sortedTypes)!;
    }
}
