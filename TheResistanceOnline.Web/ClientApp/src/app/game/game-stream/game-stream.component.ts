import {AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {environment} from "../../../environments/environment";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {
  HandleAnswerModel,
  HandleCandidateModel,
  HandleOfferModel,
  SendAnswerCommand,
  SendCandidateCommand,
  SendOfferCommand
} from "./game-stream.models";
import {GameStreamVideoComponent} from "./game-stream-video/game-stream-video.component";

@Component({
  selector: 'app-game-stream',
  templateUrl: './game-stream.component.html',
  styleUrls: ['./game-stream.component.css']
})
export class GameStreamComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(GameStreamVideoComponent) childComponent!: GameStreamVideoComponent;

  @Input() lobbyId: string = "";
  private rtcConfiguration: RTCConfiguration = {
    iceServers: [
      {
        urls: [
          'stun:stun1.l.google.com:19302' //todo fetch the most up to date stun urls from  https://raw.githubusercontent.com/pradt2/always-online-stun/master/valid_hosts.txt
        ]
      }
    ],
    iceCandidatePoolSize: 10,
  }


  // todo do I have to do anything about muting and disabling video?
  private rtcPeerConnectionsMap: Map<string, RTCPeerConnection> = new Map<string, RTCPeerConnection>();
  private connectionIds: string[] = []; //todo instead of connection ids this should probaby be userids or usernames
                                        // so that order is maintained around the screen i.e. one user has one video
                                        // so that placement is not lost around the screen


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
  private streamHubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl(environment.SERVER_URL + '/stream',
      this.options
    ).withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Error)
    .build();

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  async ngOnInit() {
  }

  async ngAfterViewInit() {
    await this.start();
  }

  async ngOnDestroy() {
    await this.stop();
  }

  public async start() {
    try {
      this.swalService.showSwal(SwalTypes.Info, "Connecting To Stream Hub...");

      // add query params
      this.streamHubConnection.baseUrl += `?lobbyId=${this.lobbyId}`

      await this.streamHubConnection.start().then(() => {
        this.swalService.showSwal(SwalTypes.Success, 'Connected To Stream Hub');

        this.getConnectionIdsInLobby();

        // add required initial listeners
        this.addReceiveErrorMessageListener();
      });
    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Stream Hub');
      setTimeout(() => this.start(), 30000);
    }
  }

  public async stop() {
    await this.streamHubConnection.stop();
    this.rtcPeerConnectionsMap.clear();
  }

  public async makeCall() {

    if (this.connectionIds?.length) {
      for (let i = 0; i < this.connectionIds.length; i++) {
        await this.initRtcPeerConnection(this.connectionIds[i], i);
      }

      for (let [connectionId, rtcPeerConnection] of this.rtcPeerConnectionsMap) {
        await this.createOffer(connectionId, rtcPeerConnection);
      }
    }
  }

  private getConnectionIdsInLobby() {
    this.streamHubConnection.invoke("GetConnectionIds", {})
      .then(async (connectionIds: string[]) => {
        this.connectionIds = connectionIds;

        console.log("got these connectionids", this.connectionIds)


        this.addReceiveHandleOfferListener();
        this.addReceiveHandleAnswerListener();
        this.addReceiveHandleCandidateListener();


        // listen for new lobbies created/updated
        this.addReceiveNewConnectionIdListener();
        this.addReceiveRemoveConnectionIdListener();
        //this.addReceiveUpdateConnectionIdListener();     a m

      });
  }

  private addReceiveNewConnectionIdListener = () => {
    this.streamHubConnection.on("NewConnectionId", async (connectionId: string) => {
      if (this.connectionIds?.length) {
        this.connectionIds.push(connectionId);
      } else {
        this.connectionIds = [connectionId];
      }
      if (!this.rtcPeerConnectionsMap.has(connectionId)) {
        const index = this.connectionIds.findIndex(c => c === connectionId);
        if (index === -1) {
          return;
        }
        await this.initRtcPeerConnection(connectionId, index)


        const rtcPeerConnection = this.rtcPeerConnectionsMap.get(connectionId);
        if (rtcPeerConnection) {
          await this.createOffer(connectionId, rtcPeerConnection);
        }
      }


    })
  }

  private removeReceiveNewConnectionIdListener = () => {
    this.streamHubConnection.off("NewConnectionId");
  }

  private addReceiveRemoveConnectionIdListener = () => {
    this.streamHubConnection.on("RemoveConnectionId", (connectionId: string) => {
      this.connectionIds = this.connectionIds.filter(c => c !== connectionId);
      this.rtcPeerConnectionsMap.delete(connectionId);
      console.log("deleted", connectionId, "from maps")
    })
  }

  private removeReceiveRemoveConnectionIdListener = () => {
    this.streamHubConnection.off("RemoveConnectionId");
  }

  private async createOffer(connectionId: string, rtcPeerConnection: RTCPeerConnection) {
    const offer = await rtcPeerConnection.createOffer();
    await rtcPeerConnection.setLocalDescription(offer);

    const command: SendOfferCommand = {
      rtcSessionDescription: offer,
      connectionIdOfWhoOfferIsFor: connectionId
    };
    this.streamHubConnection.invoke('SendOffer', command).catch(err => {
      // do nothing with errors because of the error listener
    });
  }

  private async initRtcPeerConnection(connectionId: string, videoIndex: number) {
    console.log("initRtcPeerConnection for", connectionId, videoIndex)


    const stream = await navigator.mediaDevices.getUserMedia({
      video: true,
      audio: true
    }).catch((err) => {
      console.log("there was an error with media")
      this.swalService.showSwal(SwalTypes.Error, "Unable to get Microphone and/or Camera");
    });

    if (stream) {


      const remoteStream = new MediaStream();

      const idk = this.childComponent.remoteVideoRefs[videoIndex];

      this.childComponent.remoteVideoRefs[videoIndex].nativeElement.srcObject = remoteStream;
      const rtcPeerConnection = new RTCPeerConnection(this.rtcConfiguration);

      rtcPeerConnection.ontrack = (event: RTCTrackEvent) => {
        event.streams[0].getTracks().forEach((track: MediaStreamTrack) => {
          remoteStream.addTrack(track);
        })
      };

      stream.getTracks().forEach((track: MediaStreamTrack) => {
        rtcPeerConnection.addTrack(track, stream);
      });


      rtcPeerConnection.onicecandidate = (event: RTCPeerConnectionIceEvent) => {
        if (event.candidate) {
          const command: SendCandidateCommand = {
            rtcIceCandidate: event.candidate.toJSON(),
            connectIdOfWhoCandidateIsFor: connectionId
          };
          this.streamHubConnection.invoke('SendCandidate', command).catch(err => {
          });
        }
      };
      this.rtcPeerConnectionsMap.set(connectionId, rtcPeerConnection);
    }
  }

  private addReceiveErrorMessageListener = () => {
    this.streamHubConnection.on("Error", (errorMessage: string) => {
      this.swalService.showSwal(SwalTypes.Error, errorMessage);
    });
  }

  private addReceiveHandleOfferListener = () => {
    this.streamHubConnection.on('HandleOffer', async (offer: HandleOfferModel) => {

      if (!this.rtcPeerConnectionsMap.has(offer.connectionIdOfWhoOffered)) {
        const index = this.connectionIds.findIndex(c => c === offer.connectionIdOfWhoOffered);
        if (index === -1) {
          return;
        }
        await this.initRtcPeerConnection(offer.connectionIdOfWhoOffered, index)
      }

      const rtcPeerConnection = this.rtcPeerConnectionsMap.get(offer.connectionIdOfWhoOffered);

      if (rtcPeerConnection) {

        await rtcPeerConnection.setRemoteDescription(offer.rtcSessionDescription);

        const answer = await rtcPeerConnection.createAnswer();
        await rtcPeerConnection.setLocalDescription(answer);

        const command: SendAnswerCommand = {
          rtcSessionDescription: answer,
          connectionIdOfWhoAnswerIsFor: offer.connectionIdOfWhoOffered
        };

        this.streamHubConnection.invoke('SendAnswer', command).catch(err => {
          // do nothing with errors because of the error listener
        });
      }
    })
  }

  private addReceiveHandleAnswerListener = () => {
    this.streamHubConnection.on('HandleAnswer', async (answer: HandleAnswerModel) => {
      const rtcPeerConnection = this.rtcPeerConnectionsMap.get(answer.connectionIdOfWhoAnswered);
      if (rtcPeerConnection) {
        await rtcPeerConnection.setRemoteDescription(answer.rtcSessionDescription);
      }
    });
  }

  private addReceiveHandleCandidateListener = () => {
    this.streamHubConnection.on("HandleCandidate", async (candidate: HandleCandidateModel) => {
      const rtcPeerConnection = this.rtcPeerConnectionsMap.get(candidate.connectionIdOfWhoSentCandidate);
      if (rtcPeerConnection) {
        await rtcPeerConnection.addIceCandidate(candidate.rtcIceCandidate);
      }
    });
  }

}
