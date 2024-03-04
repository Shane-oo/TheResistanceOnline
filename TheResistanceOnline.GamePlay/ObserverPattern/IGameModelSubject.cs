using TheResistanceOnline.GamePlay.Common;
using TheResistanceOnline.GamePlay.PlayerModels;

namespace TheResistanceOnline.GamePlay.ObserverPattern;

public interface IGameModelSubject: ISubject
{
    public int Mission { get; }

    public PlayerModel MissionLeader { get; }

    public int MissionSize { get; }

    public List<PlayerModel> MissionTeam { get; }

    public Phase Phase { get; }

    public List<string> PlayerNames { get; }

    public void AddMissionTeamMember(string playerName);

    public void RemoveMissionTeamMember(string playerName);


}
