export function getMissionTeamCount(roundNumber: number, playerCount: number): number {
  switch (playerCount) {
    case 5:
      switch (roundNumber) {
        case 1:
        case 3:
          return 2;
        case 2:
        case 4:
        case 5:
          return 3;
      }
      break;
    case 6:
      switch (roundNumber) {
        case 1:
          return 2;
        case 2:
        case 4:
          return 3;
        case 3:
        case 5:
          return 4;
      }
      break;
    case 7:
      switch (roundNumber) {
        case 1:
          return 2;
        case 2:
        case 3:
          return 3;
        case 4:
        case 5:
          return 4;
      }
      break;
    case 8:
    case 9:
    case 10:
      switch (roundNumber) {
        case 1:
          return 3;
        case 2:
        case 3:
          return 4;
        case 4:
        case 5:
          return 5;

      }
      break;
  }
  return -1;
}
