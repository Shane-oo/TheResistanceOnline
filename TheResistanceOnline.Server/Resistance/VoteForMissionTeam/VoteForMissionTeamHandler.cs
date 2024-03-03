using Microsoft.AspNetCore.SignalR;
using TheResistanceOnline.Core.Errors;
using TheResistanceOnline.Core.Exchange.Requests;
using TheResistanceOnline.Core.Exchange.Responses;

namespace TheResistanceOnline.Server.Resistance.VoteForMissionTeam;

public class VoteForMissionTeamHandler: ICommandHandler<VoteForMissionTeamCommand>
{
    #region Fields

    private readonly IHubContext<ResistanceHub, IResistanceHub> _resistanceHubContext;

    #endregion

    #region Construction

    public VoteForMissionTeamHandler(IHubContext<ResistanceHub, IResistanceHub> resistanceHubContext)
    {
        _resistanceHubContext = resistanceHubContext;
    }

    #endregion

    #region Public Methods

    public async Task<Result> Handle(VoteForMissionTeamCommand command, CancellationToken cancellationToken)
    {
        if (command is null) return Result.Failure(Error.NullValue);

        var gameModel = command.GameModel;

        var player = gameModel.GetPlayerModel(command.CallerPlayerName);

        var approved = command.VotePiece == VotePiece.ApproveVotePiece;
        player.Vote(approved);

        await _resistanceHubContext.Clients.Client(command.ConnectionId).RemoveVotingChoices();

        await _resistanceHubContext.Clients.Group(command.LobbyId).PlayerVoted(command.CallerPlayerName);

        var voteOverResult = gameModel.GetVoteResults();

        if (!voteOverResult.IsSuccess)
        {
            // still waiting for everyone's votes
            return Result.Success();
        }

        var results = voteOverResult.Value;

        await _resistanceHubContext.Clients.Group(command.LobbyId).ShowVotes(results);

        // todo dododo I am here

        if (results.VoteSuccessful)
        {
            // start mission phase
        }
        else
        {
            // start mission build phase again
            // dont forget to send vote track
            // potentially overlaps with CommendGame so some refactoring may need to be done
        }

        return Result.Success();
    }

    #endregion
}
