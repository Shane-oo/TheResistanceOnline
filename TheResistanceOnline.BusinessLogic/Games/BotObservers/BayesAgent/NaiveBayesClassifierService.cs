using TheResistanceOnline.Data;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers.BayesAgent;

public interface INaiveBayesClassifierService
{
}

public class NaiveBayesClassifierServiceService: INaiveBayesClassifierService
{
    private readonly IDataContext _context;
    // public List<> TrainingData;
    public NaiveBayesClassifierServiceService(IDataContext context)
    {
        _context = context;
        // Get Training Data
        //TrainingData = GetTrainingData();
    }

    public void GetTrainingData()
    {
        //            return await _context.Query<IUserByNameOrEmailDbQuery>().WithParams(query.Name).ExecuteAsync(query.CancellationToken);

    }
    
}
