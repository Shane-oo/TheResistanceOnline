using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Server.Resistance;

public class SubmitMissionTeamHandler: ICommandHandler<SubmitMissionTeamCommand>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

    #endregion

    #region Construction

    public SubmitMissionTeamHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(SubmitMissionTeamCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return Result.Failure<string>(Error.NullValue);

        var gameModel = command.GameModel;

        var submitResult = gameModel.SubmitMissionTeam();
        if (submitResult.IsFailure)
        {
            return submitResult;
        }

        var missionTeam = gameModel.GetMissionTeam();

        await _resistanceHubContext.Clients.Group(command.LobbyId).VoteForMissionTeam(missionTeam);

        foreach(var bot in gameModel.Bots)
        {
            bot.BotModel.VoteForMissionTeam();
            await _resistanceHubContext.Clients.Group(command.LobbyId).PlayerVoted(bot.Name);
        }

        return Result.Success();
    }

    #endregion
}
