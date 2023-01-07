UPDATE PlayerStatistics Set Won = 1
    FROM Games Join PlayerStatistics on Games.Id = PlayerStatistics.GameId
WHERE PlayerStatistics.Team = Games.WinningTeam
