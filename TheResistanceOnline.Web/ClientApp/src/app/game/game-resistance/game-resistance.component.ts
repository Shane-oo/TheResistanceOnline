import {AfterViewInit, Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Subject} from "rxjs";

import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";

import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {environment} from "../../../environments/environment";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {CommenceGameModel, Team, VoteResultsModel} from "./game-resistance.models";
import {GameType, StartGameCommand} from "../game.models";
import {CustomError} from "../../shared/models/error.models";


@Component({
  selector: 'app-game-resistance',
  templateUrl: './game-resistance.component.html',
  styleUrls: ['./game-resistance.component.css']
})
export class GameResistanceComponent implements OnInit, OnDestroy, AfterViewInit {
  public readonly gameCommenced: Subject<CommenceGameModel> = new Subject<CommenceGameModel>();
  public readonly startMissionBuildPhase: Subject<void> = new Subject<void>();
  public readonly newMissionTeamMember: Subject<string> = new Subject<string>();
  public readonly removeMissionTeamMember: Subject<string> = new Subject<string>();
  public readonly moveToVotingPhase: Subject<string[]> = new Subject<string[]>();
  public readonly removeVotingChoices: Subject<void> = new Subject<void>();
  public readonly playerSubmittedVote: Subject<string> = new Subject<string>();
  public readonly showVoteResults: Subject<VoteResultsModel> = new Subject<VoteResultsModel>();
  public readonly showMissionCards: Subject<boolean> = new Subject<boolean>();

  public showMissionTeamSubmit: boolean = false;

  @Input() startGameCommand!: StartGameCommand;
  protected readonly GameType = GameType;
  private options: IHttpConnectionOptions = {
    accessTokenFactory: () => {
      const token = this.authService.authenticationData?.access_token;
      if (token) {
        return token;

      }
      return '';
    },
    transport: signalR.HttpTransportType.WebSockets,
    skipNegotiation: true,
    timeout: 120000, // 2 minutes
  };
  private resistanceHubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.SERVER_URL + '/resistance',
      this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  async ngOnInit() {
    await this.start();
  }

  async ngAfterViewInit() {
  }

  async ngOnDestroy() {
    await this.stop();

    this.gameCommenced.complete();
    this.newMissionTeamMember.complete();
    this.removeMissionTeamMember.complete();
  }

  objectClickedEvent(name: string) {
    this.resistanceHubConnection.invoke("ObjectSelected", name)
      .catch(err => {
      });
  }

  submitMissionTeam() {
    this.resistanceHubConnection.invoke("SubmitMissionTeam")
      .catch(err => {
      });
  }

  private addRequiredListeners() {
    this.addReceiveErrorMessageListener();
    this.addReceiveCommenceGameModelListener();
    this.addReceiveStartMissionBuildPhase();
    this.addReceiveNewMissionTeamMember();
    this.addReceiveRemoveMissionTeamMember();
    this.addReceiveShowMissionTeamSubmit();
    this.addReceiveVoteForMissionTeam();
    this.addReceiveRemoveVotingChoices();
    this.addReceivePlayerVoted();
    this.addReceiveVoteResults();
    this.addReceiveShowMissionCards();
  }

  private async start() {
    if (this.startGameCommand) {
      try {
        this.swalService.showSwal(SwalTypes.Info, "Connecting To Resistance Hub...");

        // add query params to connection request
        this.resistanceHubConnection.baseUrl += `?lobbyId=${this.startGameCommand.lobbyId}`;

        await this.resistanceHubConnection.start().then(() => {
          this.swalService.showSwal(SwalTypes.Success, "Connected To Resistance Hub");

          // add Required Listeners
          // add required initial listeners
          this.addRequiredListeners();

          this.swalService.showSwal(SwalTypes.Info, "Waiting For Game To Start...")
          this.startGame();

        });
      } catch (err) {
        this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Resistance Hub');
      }
    }
  }

  private async stop() {
    await this.resistanceHubConnection.stop();
  }

  private startGame() {
    if (this.startGameCommand) {
      this.resistanceHubConnection.invoke("StartGame", this.startGameCommand)
        .catch(err => {
        });
    }
  }

  private addReceiveErrorMessageListener = () => {
    this.resistanceHubConnection.on("Error", (errorMessage: CustomError) => {
      this.swalService.showSwal(SwalTypes.Error, errorMessage.description);
    });
  }

  private addReceiveCommenceGameModelListener = () => {
    this.resistanceHubConnection.on("CommenceGame", (commenceGameModel: CommenceGameModel) => {
      if (commenceGameModel.team === Team.Resistance) {
        this.swalService.fireNotifyResistanceModal();
      } else {
        this.swalService.fireNotifySpiesModal(commenceGameModel.teamMates!)
      }

      this.gameCommenced.next(commenceGameModel);
      // CommenceGame only called once
      this.resistanceHubConnection.off("CommenceGame");
    });
  }

  private addReceiveStartMissionBuildPhase = () => {
    this.resistanceHubConnection.on("StartMissionBuildPhase", () => {
      this.startMissionBuildPhase.next();
    });
  }

  private addReceiveNewMissionTeamMember = () => {
    this.resistanceHubConnection.on("NewMissionTeamMember", (selectedPlayerName: string) => {
      this.newMissionTeamMember.next(selectedPlayerName);
    });
  }

  private addReceiveRemoveMissionTeamMember = () => {
    this.resistanceHubConnection.on("RemoveMissionTeamMember", (selectedPlayerName: string) => {
      this.removeMissionTeamMember.next(selectedPlayerName);
    });
  }

  private addReceiveShowMissionTeamSubmit = () => {
    this.resistanceHubConnection.on("ShowMissionTeamSubmit", (show: boolean) => {
      this.showMissionTeamSubmit = show;
    });
  }

  private addReceiveVoteForMissionTeam = () => {
    this.resistanceHubConnection.on("VoteForMissionTeam", (missionTeamMembers: string[]) => {
      this.showMissionTeamSubmit = false;
      this.moveToVotingPhase.next(missionTeamMembers);
    });
  }

  private addReceiveRemoveVotingChoices = () => {
    this.resistanceHubConnection.on("RemoveVotingChoices", () => {
      this.removeVotingChoices.next();
    })
  }

  private addReceivePlayerVoted = () => {
    this.resistanceHubConnection.on("PlayerVoted", (playerName: string) => {
      this.playerSubmittedVote.next(playerName);
    });
  }

  private addReceiveVoteResults = () => {
    this.resistanceHubConnection.on("ShowVotes", (resultsModel: VoteResultsModel) => {
      resultsModel.playerNameToVoteApproved = new Map(Object.entries(resultsModel.playerNameToVoteApproved));
      this.showVoteResults.next(resultsModel);
    });
  }

  private addReceiveShowMissionCards = () => {
    this.resistanceHubConnection.on("ShowMissionCards", (showSuccessAndFail: boolean) => {
      this.showMissionCards.next(showSuccessAndFail);
    })
  }
}
