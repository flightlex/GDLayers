using GdLayers.Attributes;
using GdLayers.Mvvm.Models.Pages.Layers;
using GeometryDashAPI.Levels;
using System.Collections.Generic;
using System.Linq;

namespace GdLayers.Mvvm.Services.Pages;

[DiService]
public sealed class LayersService
{
    public void ApplyEditorLayerForObjectId(BlockList blockList, int objectId, int layerIndex)
    {
        foreach (var block in blockList)
        {
            if (block.Id == objectId)
            {
                block.EditorL = (short)layerIndex;
                block.EditorL2 = 0;
            }
        }
    }
    public int TryGetFreeLayerIndex(IEnumerable<LayerModel> layers)
    {
        var nextFree = -1;
        for (int i = 0; i < 1000; i++)
        {
            if (!layers.Any(x => x.LayerIndex == i))
            {
                nextFree = i;
                break;
            }
        }

        return nextFree;
    }
}
