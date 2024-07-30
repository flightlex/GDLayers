using GeometryDashAPI.Data;
using GeometryDashAPI.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GdLayers.Services;

public sealed class LocalLevelsService
{
    private LocalLevels _localLevels = null!;

    public async Task LoadLocalLevels(bool force = false)
    {
        // using task run because this shit is fucking broken
        await Task.Run(async delegate
        {
            if (force)
                _localLevels = await LocalLevels.LoadFileAsync();
            else
                _localLevels ??= await LocalLevels.LoadFileAsync();
        });
    }

    public async Task SaveLocalLevels()
    {
        await Task.Run(async delegate
        {
            await _localLevels.SaveAsync();
        });
    }

    public LevelCreatorModel GetLevel(int index) => _localLevels.GetLevel(index);

    public IReadOnlyCollection<LevelCreatorModel> GetLevels() => _localLevels;
}