using TheResistanceOnline.GamePlay.Common;

namespace TheResistanceOnline.GamePlay.ObserverPattern;

public interface IGameModelSubject: ISubject
{
    public int Mission { get; }

    public string MissionLeader { get; }

    public int MissionSize { get; }

    public List<string> MissionTeam { get; }

    public Phase Phase { get; }

    public List<string> PlayerNames { get; }
}
