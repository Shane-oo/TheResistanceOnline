import {Component} from '@angular/core';
import {GameType, StartGameCommand, StartGameModel} from "./game.models";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent {
  public inGame: boolean = false;
  public startGameCommand!: StartGameCommand;

  public startGame(startGame: StartGameModel) {
    this.startGameCommand = {
      bots: startGame.bots,
      botsAllowed: startGame.botsAllowed,
      lobbyId: startGame.lobbyId,
      totalPlayers: startGame.totalPlayers,
      userNames: startGame.userNames,
      type: startGame.type
    };

    this.inGame = true;
  }

  protected readonly GameType = GameType;
}
