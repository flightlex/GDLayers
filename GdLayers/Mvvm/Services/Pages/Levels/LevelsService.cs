using GdLayers.Attributes;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Services;
using GeometryDashAPI.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GdLayers.Mvvm.Services.Pages.Levels;

[DependencyInjectionService]
public sealed class LevelsService
{
    private readonly LocalLevelsService _localLevelsService;

    public LevelsService(LocalLevelsService localLevelsService)
    {
        _localLevelsService = localLevelsService;
    }

    public async Task<ICollection<LevelModel>> GetLevelsAsync()
    {
        await _localLevelsService.LoadLocalLevels(force: false);
        var levels = _localLevelsService.GetLevels();

        LevelModel[] models = new LevelModel[levels.Count];

        int index = 0;
        foreach (var level in levels)
            models[index] = new(level, ++index);

        return models;
    }
    public async Task SaveLevelsAsync()
    {
        await _localLevelsService.SaveLocalLevels();
    }


    public LevelCreatorModel GetLevel(int index)
    {
        return _localLevelsService.GetLevel(index);
    }
}
