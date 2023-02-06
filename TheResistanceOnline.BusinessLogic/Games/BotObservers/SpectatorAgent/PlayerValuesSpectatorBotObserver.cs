using TheResistanceOnline.BusinessLogic.Games.Models;

namespace TheResistanceOnline.BusinessLogic.Games.BotObservers.SpectatorAgent;

public class PlayerValuesSpectatorBotObserver: IBotObserver, ISpectatorBotObserver
{
    #region Fields

    private bool _beginningOfGame = true;
    private GameDetailsModel _gameDetails = new();
    private Dictionary<Guid, PlayerVariablesModel> _playerIdToGameData;

    #endregion

    #region Private Methods

    private PlayerDetailsModel GetMissionLeader()
    {
        return _gameDetails.PlayersDetails?.FirstOrDefault(p => p.IsMissionLeader);
    }

    private PlayerVariablesModel GetPlayerVariables(Guid playerId)
    {
        _playerIdToGameData.TryGetValue(playerId, out var playerVariables);
        return playerVariables;
    }

    private void RecordMissionData()
    {
        if (_gameDetails.MissionRounds.TryGetValue(_gameDetails.CurrentMissionRound, out var missionSuccessful)) // true if resistance win
        {
            foreach(var missionPlayer in _gameDetails.MissionTeam!)
            {
                var playerVariables = GetPlayerVariables(missionPlayer.PlayerId);
                if (missionSuccessful)
                {
                    playerVariables.WentOnSuccessfulMission++;
                }
                else
                {
                    var betrayals = _gameDetails.MissionOutcome.Count(mo => !mo);

                    playerVariables.WentOnFailedMission += playerVariables.WentOnFailedMission > 0
                                                               ? playerVariables.WentOnFailedMission
                                                               : 1 * _gameDetails.CurrentMissionRound *
                                                                 _gameDetails.MissionSize / betrayals;
                }
            }

            foreach(var player in _gameDetails.PlayersDetails!)
            {
                // suss if player did not want a successful mission to happen
                var playerVariables = GetPlayerVariables(player.PlayerId);
                switch(missionSuccessful)
                {
                    case true when !player.ApprovedMissionTeam:
                        GetPlayerVariables(player.PlayerId).RejectedTeamSuccessfulMission += 2 * _gameDetails.CurrentMissionRound;
                        break;
                    case false when player.ApprovedMissionTeam:
                        playerVariables.VotedForFailedMission += 2 * _gameDetails.CurrentMissionRound;
                        break;
                }

                // Proposer
                if (!player.IsMissionLeader) continue;
                if (missionSuccessful) continue;
                //different to source
                playerVariables.ProposedTeamFailedMission += 3 * playerVariables.ProposedTeamFailedMission * _gameDetails.CurrentMissionRound;

                // If proposer was in the mission that failed
                if (_gameDetails.MissionTeam.Any(p => p.PlayerId == player.PlayerId))
                {
                    playerVariables.ProposedTeamFailedMission += 3 * _gameDetails.CurrentMissionRound;
                }
            }
        }
    }

    private void RecordVoteData()
    {
        var missionLeader = GetMissionLeader();

        if (_gameDetails.CurrentMissionRound > 1)
        {
            var missionLeaderPlayerVariables = GetPlayerVariables(missionLeader.PlayerId);

            // If Proposer proposed a mission that contains no successful mission members
            var trustingMembersCount = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionMember.PlayerId).WentOnSuccessfulMission > 0);
            if (trustingMembersCount == 0)
            {
                // Proposed a team that havent even been on missions
                missionLeaderPlayerVariables.ProposedTeamThatHaventBeenOnMissions += 2 * _gameDetails.CurrentMissionRound;
            }

            // If Proposer proposed a mission that contains previously failed mission members
            var untrustingMembersCount = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionLeader.PlayerId).WentOnFailedMission > 0);
            if (untrustingMembersCount != 0)
            {
                missionLeaderPlayerVariables.ProposedTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * untrustingMembersCount;
            }
        }

        foreach(var player in _gameDetails.PlayersDetails!)
        {
            var playerVariables = GetPlayerVariables(player.PlayerId);
            if (!player.ApprovedMissionTeam)
            {
                playerVariables.VotedAgainstTeamProposal += _gameDetails.CurrentMissionRound;
                // If they say no to teams that contain successful mission members its suss
                var successfulMissionsMembers = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionMember.PlayerId).WentOnSuccessfulMission > 0);
                playerVariables.VotedNoTeamHasSuccessfulMembers += 3 * _gameDetails.CurrentMissionRound * successfulMissionsMembers;
            }

            if (player.ApprovedMissionTeam)
            {
                // If they vote yes for a mission that contains members that prevousily failed missions its suss
                var failedPriorMissionsMembers = _gameDetails.MissionTeam!.Count(missionMember => GetPlayerVariables(missionLeader.PlayerId).WentOnFailedMission > 0);
                playerVariables.VotedForTeamHasUnsuccessfulMembers += 4 * _gameDetails.CurrentMissionRound * failedPriorMissionsMembers > 0 ? failedPriorMissionsMembers : 1;

                // Voted For A mission they are not on
                if (_gameDetails.MissionTeam.All(p => p.PlayerId != player.PlayerId))
                {
                    playerVariables.VotedForMissionNotOn += _gameDetails.CurrentMissionRound;
                }
            }
        }
    }

    #endregion

    #region Public Methods

    public Dictionary<Guid, PlayerVariablesModel> GetPlayerVariablesDictionary()
    {
        return _playerIdToGameData;
    }

    public void Update(GameDetailsModel gameDetails)
    {
        _gameDetails = gameDetails;
        if (_beginningOfGame)
        {
            //_playerData = new List<PlayerDetailsModel>(_gameDetails.PlayersDetails!);
            _playerIdToGameData = new Dictionary<Guid, PlayerVariablesModel>();
            foreach(var player in _gameDetails.PlayersDetails!)
            {
                _playerIdToGameData.Add(player.PlayerId, new PlayerVariablesModel());
            }

            _beginningOfGame = false;
        }

        switch(_gameDetails.GameStage)
        {
            case GameStageModel.VoteResults:
                RecordVoteData();
                break;
            case GameStageModel.MissionResults:
                RecordMissionData();
                break;
            case GameStageModel.GameOverSpiesWon:
            case GameStageModel.GameOverResistanceWon:
                break;
        }
    }

    #endregion
}
