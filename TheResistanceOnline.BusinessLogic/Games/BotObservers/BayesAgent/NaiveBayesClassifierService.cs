using System.Reflection;
using TheResistanceOnline.BusinessLogic.Games.Commands;
using TheResistanceOnline.BusinessLogic.Games.Models;
using TheResistanceOnline.Data.Games;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers.BayesAgent;

public interface INaiveBayesClassifierService
{
    Dictionary<Guid, bool> GetPredictions(Dictionary<Guid, PlayerVariablesModel> playerIdToPlayerVariables);

    Task GetTrainingDataAsync();
}

public class NaiveBayesClassifierService: INaiveBayesClassifierService
{
    #region Fields

    private readonly IGameService _gameService;

    private int _resistanceRows;

    private int _spyRows;

    private readonly double _sqrtOfTwoPi = Math.Sqrt(2 * Math.PI);
    private readonly Dictionary<int, List<Stats>> _statistics = new();

    private int _totalRows;

    #endregion

    #region Construction

    // public List<> TrainingData;
    public NaiveBayesClassifierService(IGameService gameService)
    {
        // Get Training Data
        //TrainingData = GetTrainingData();
        _gameService = gameService;
    }

    #endregion

    #region Private Methods

    // returns a dictionary of the probability of both keys 0 for resistance and 1 for spy
    private Dictionary<int, double> CalculateIsSpyPredictions(PlayerVariablesModel playerVariables)
    {
        var probabilities = new Dictionary<int, double>();
        _statistics.TryGetValue(0, out var resistanceStats);
        _statistics.TryGetValue(1, out var spyStats);
        if (resistanceStats != null && spyStats != null)
        {
            // loops through twice, first for resistance second for spy
            foreach(var stats in _statistics)
            {
                var probability = stats.Key == 0 ? _resistanceRows : _spyRows / (double)_totalRows;
                var index = 0;
                foreach(var property in playerVariables.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (index == 11)
                    {
                        // Throws on the TeamModel Value
                        break;
                    }

                    Console.WriteLine(property.GetValue(playerVariables));
                    var stat = stats.Value[index];

                    probability *= GaussianProbability((int)property.GetValue(playerVariables)! + 1, stat.Mean, stat.StandardDeviation);
                    index++;
                }

                probabilities.Add(stats.Key, probability);
            }
        }

        return probabilities;
    }

    private static double CalculateMean(IEnumerable<int> values)
    {
        var mean = 0.0;
        var enumerable = values.ToList();
        if (!enumerable.Any()) return mean;
        var summedValues = enumerable.Sum();

        mean = (double)summedValues / (enumerable.Count - 1);

        return mean;
    }

    private static double CalculateStandardDeviation(IEnumerable<int> values)
    {
        double standardDeviation = 0;

        var enumerable = values.ToList();
        if (!enumerable.Any()) return standardDeviation;
        // Compute the average.     
        var avg = enumerable.Average();

        // Perform the Sum of (value-avg)_2_2.      
        var sum = enumerable.Sum(d => Math.Pow(d - avg, 2));

        // Put it all together.      
        standardDeviation = Math.Sqrt(sum / (enumerable.Count - 1));

        return standardDeviation;
    }

    private double GaussianProbability(int columnValue, double mean, double standardDeviation)
    {
        var exponent = Math.Exp(-(Math.Pow(columnValue - mean, 2) / (2 * Math.Pow(standardDeviation, 2))));
        var gaussian = 1 / (_sqrtOfTwoPi * standardDeviation) * exponent;
        // Can have NaN when mean and std are 0
        return double.IsNaN(gaussian) ? 1.0 : gaussian;
    }

    // returns 0 for a Resistance or 1 For Spy
    private bool Predict(PlayerVariablesModel playerVariables)
    {
        var probabilities = CalculateIsSpyPredictions(playerVariables);
        // Get the higher Resistance or Spy Probability
        probabilities.TryGetValue(0, out var resistanceProbability);
        probabilities.TryGetValue(1, out var spyProbability);
        // if spyProbability > resistanceProbability  then label player as spy
        return spyProbability > resistanceProbability;
    }

    #endregion

    #region Public Methods

    public Dictionary<Guid, bool> GetPredictions(Dictionary<Guid, PlayerVariablesModel> playerIdToPlayerVariables)
    {
        return playerIdToPlayerVariables.ToDictionary(playerVariable => playerVariable.Key, playerVariable => Predict(playerVariable.Value));
    }

    public async Task GetTrainingDataAsync()
    {
        //            return await _context.Query<IUserByNameOrEmailDbQuery>().WithParams(query.Name).ExecuteAsync(query.CancellationToken);
        var getGamePlayerValuesCommand = new GetGamePlayerValuesCommand();
        var gamePlayerValues = await _gameService.GetGamePlayerValuesAsync(getGamePlayerValuesCommand);
        // _mapper.Map<PlayerValue>(playerVariable.Value);
        //Seperate By Resistance and Spy values
        var spyValues = new List<PlayerValue>();
        var resistanceValues = new List<PlayerValue>();
        foreach(var playerValue in gamePlayerValues.SelectMany(gamePlayerValue => gamePlayerValue.PlayerValues))
        {
            if (playerValue.Team == 0)
            {
                resistanceValues.Add(playerValue);
            }
            else
            {
                spyValues.Add(playerValue);
            }
        }

        var resistanceStats = new List<Stats>();
        var spyStats = new List<Stats>();
        // Sorry for this monstrosity
        var votedForFailedMissions = resistanceValues.Select(pv => pv.VotedForFailedMission).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(votedForFailedMissions),
                                StandardDeviation = CalculateStandardDeviation(votedForFailedMissions),
                                Count = votedForFailedMissions.Count
                            });
        votedForFailedMissions = spyValues.Select(pv => pv.VotedForFailedMission).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(votedForFailedMissions),
                         StandardDeviation = CalculateStandardDeviation(votedForFailedMissions),
                         Count = votedForFailedMissions.Count
                     });

        var wentOnFailedMissions = resistanceValues.Select(pv => pv.WentOnFailedMission).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(wentOnFailedMissions),
                                StandardDeviation = CalculateStandardDeviation(wentOnFailedMissions),
                                Count = wentOnFailedMissions.Count
                            });
        wentOnFailedMissions = spyValues.Select(pv => pv.WentOnFailedMission).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(wentOnFailedMissions),
                         StandardDeviation = CalculateStandardDeviation(wentOnFailedMissions),
                         Count = wentOnFailedMissions.Count
                     });

        var wentOnSuccessfulMissions = resistanceValues.Select(pv => pv.WentOnSuccessfulMission).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(wentOnSuccessfulMissions),
                                StandardDeviation = CalculateStandardDeviation(wentOnSuccessfulMissions),
                                Count = wentOnSuccessfulMissions.Count
                            });
        wentOnSuccessfulMissions = spyValues.Select(pv => pv.WentOnSuccessfulMission).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(wentOnSuccessfulMissions),
                         StandardDeviation = CalculateStandardDeviation(wentOnSuccessfulMissions),
                         Count = wentOnSuccessfulMissions.Count
                     });


        var proposedTeamFailedMissions = resistanceValues.Select(pv => pv.ProposedTeamFailedMission).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(proposedTeamFailedMissions),
                                StandardDeviation = CalculateStandardDeviation(proposedTeamFailedMissions),
                                Count = proposedTeamFailedMissions.Count
                            });
        proposedTeamFailedMissions = spyValues.Select(pv => pv.ProposedTeamFailedMission).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(proposedTeamFailedMissions),
                         StandardDeviation = CalculateStandardDeviation(proposedTeamFailedMissions),
                         Count = proposedTeamFailedMissions.Count
                     });
        var rejectedTeamSuccessfulMissions = resistanceValues.Select(pv => pv.RejectedTeamSuccessfulMission).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(rejectedTeamSuccessfulMissions),
                                StandardDeviation = CalculateStandardDeviation(rejectedTeamSuccessfulMissions),
                                Count = rejectedTeamSuccessfulMissions.Count
                            });
        rejectedTeamSuccessfulMissions = spyValues.Select(pv => pv.RejectedTeamSuccessfulMission).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(rejectedTeamSuccessfulMissions),
                         StandardDeviation = CalculateStandardDeviation(rejectedTeamSuccessfulMissions),
                         Count = rejectedTeamSuccessfulMissions.Count
                     });
        var votedAgainstTeamProposals = resistanceValues.Select(pv => pv.VotedAgainstTeamProposal).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(votedAgainstTeamProposals),
                                StandardDeviation = CalculateStandardDeviation(votedAgainstTeamProposals),
                                Count = votedAgainstTeamProposals.Count
                            });
        votedAgainstTeamProposals = spyValues.Select(pv => pv.VotedAgainstTeamProposal).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(votedAgainstTeamProposals),
                         StandardDeviation = CalculateStandardDeviation(votedAgainstTeamProposals),
                         Count = votedAgainstTeamProposals.Count
                     });
        var votedNoTeamHasSuccessfulMembers = resistanceValues.Select(pv => pv.VotedNoTeamHasSuccessfulMembers).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(votedNoTeamHasSuccessfulMembers),
                                StandardDeviation = CalculateStandardDeviation(votedNoTeamHasSuccessfulMembers),
                                Count = votedNoTeamHasSuccessfulMembers.Count
                            });
        votedNoTeamHasSuccessfulMembers = spyValues.Select(pv => pv.VotedNoTeamHasSuccessfulMembers).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(votedNoTeamHasSuccessfulMembers),
                         StandardDeviation = CalculateStandardDeviation(votedNoTeamHasSuccessfulMembers),
                         Count = votedNoTeamHasSuccessfulMembers.Count
                     });
        var votedForTeamHasUnsuccessfulMembers = resistanceValues.Select(pv => pv.VotedForTeamHasUnsuccessfulMembers).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(votedForTeamHasUnsuccessfulMembers),
                                StandardDeviation = CalculateStandardDeviation(votedForTeamHasUnsuccessfulMembers),
                                Count = votedForTeamHasUnsuccessfulMembers.Count
                            });
        votedForTeamHasUnsuccessfulMembers = spyValues.Select(pv => pv.VotedForTeamHasUnsuccessfulMembers).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(votedForTeamHasUnsuccessfulMembers),
                         StandardDeviation = CalculateStandardDeviation(votedForTeamHasUnsuccessfulMembers),
                         Count = votedForTeamHasUnsuccessfulMembers.Count
                     });
        var votedForMissionNotOns = resistanceValues.Select(pv => pv.VotedForMissionNotOn).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(votedForMissionNotOns),
                                StandardDeviation = CalculateStandardDeviation(votedForMissionNotOns),
                                Count = votedForMissionNotOns.Count
                            });
        votedForMissionNotOns = spyValues.Select(pv => pv.VotedForMissionNotOn).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(votedForMissionNotOns),
                         StandardDeviation = CalculateStandardDeviation(votedForMissionNotOns),
                         Count = votedForMissionNotOns.Count
                     });
        var proposedTeamHasUnsuccessfulMembers = resistanceValues.Select(pv => pv.ProposedTeamHasUnsuccessfulMembers).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(proposedTeamHasUnsuccessfulMembers),
                                StandardDeviation = CalculateStandardDeviation(proposedTeamHasUnsuccessfulMembers),
                                Count = proposedTeamHasUnsuccessfulMembers.Count
                            });
        proposedTeamHasUnsuccessfulMembers = spyValues.Select(pv => pv.ProposedTeamHasUnsuccessfulMembers).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(proposedTeamHasUnsuccessfulMembers),
                         StandardDeviation = CalculateStandardDeviation(proposedTeamHasUnsuccessfulMembers),
                         Count = proposedTeamHasUnsuccessfulMembers.Count
                     });
        var proposedTeamThatHaventBeenOnMissions = resistanceValues.Select(pv => pv.ProposedTeamThatHaventBeenOnMissions).ToList();
        resistanceStats.Add(new Stats
                            {
                                Mean = CalculateMean(proposedTeamThatHaventBeenOnMissions),
                                StandardDeviation = CalculateStandardDeviation(proposedTeamThatHaventBeenOnMissions),
                                Count = proposedTeamThatHaventBeenOnMissions.Count
                            });
        proposedTeamThatHaventBeenOnMissions = spyValues.Select(pv => pv.ProposedTeamThatHaventBeenOnMissions).ToList();
        spyStats.Add(new Stats
                     {
                         Mean = CalculateMean(proposedTeamThatHaventBeenOnMissions),
                         StandardDeviation = CalculateStandardDeviation(proposedTeamThatHaventBeenOnMissions),
                         Count = proposedTeamThatHaventBeenOnMissions.Count
                     });

        _statistics.Add(0, resistanceStats);
        _statistics.Add(1, spyStats);
        _resistanceRows = resistanceStats.FirstOrDefault()!.Count;
        _spyRows = spyStats.FirstOrDefault()!.Count;
        _totalRows = _resistanceRows + _spyRows;
    }

    #endregion
}

public class Stats
{
    #region Properties

    public int Count { get; init; }

    public double Mean { get; init; }

    public double StandardDeviation { get; init; }

    #endregion
}
