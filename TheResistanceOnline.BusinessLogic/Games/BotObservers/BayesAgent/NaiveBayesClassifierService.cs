using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.DbQueries;
using TheResistanceOnline.Data;
using TheResistanceOnline.Data.Core.Queries;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers.BayesAgent;

public interface INaiveBayesClassifierService
{
    Task GetTrainingDataAsync();
}

public class NaiveBayesClassifierService: INaiveBayesClassifierService
{
    private readonly IGameService _gameService;
    // public List<> TrainingData;
    public  NaiveBayesClassifierService(IGameService gameService)
    {
        // Get Training Data
        //TrainingData = GetTrainingData();
        _gameService = gameService;
    }

    public async Task GetTrainingDataAsync()
    {
        //            return await _context.Query<IUserByNameOrEmailDbQuery>().WithParams(query.Name).ExecuteAsync(query.CancellationToken);
        var getGamePlayerValuesCommand = new GetGamePlayerValuesCommand();
        var gamePlayerValues = await _gameService.GetGamePlayerValuesAsync(getGamePlayerValuesCommand);
        
        Console.WriteLine(gamePlayerValues);
    }
}
