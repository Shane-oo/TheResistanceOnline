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
import {ConnectionModel} from "../game.models";

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
          'stun:stun2.l.google.com:19305' //todo fetch the most up to date stun urls from  https://raw.githubusercontent.com/pradt2/always-online-stun/master/valid_hosts.txt
        ]
      }
    ],
    iceCandidatePoolSize: 10,
  }


  // todo do I have to do anything about muting and disabling video?
  private rtcPeerConnectionsMap: Map<string, RTCPeerConnection> = new Map<string, RTCPeerConnection>();

  private connections: ConnectionModel[] = [];

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
    this.rtcPeerConnectionsMap.clear();
  }

  public async makeCall() {

    if (this.connections?.length) {
      for (let i = 0; i < this.connections.length; i++) {
        await this.initRtcPeerConnection(this.connections[i].connectionId, i);
      }

      for (let [connectionId, rtcPeerConnection] of this.rtcPeerConnectionsMap) {
        await this.createOffer(connectionId, rtcPeerConnection);
      }
    }
  }

  private async start() {
    try {
      // add query params
      this.streamHubConnection.baseUrl += `?lobbyId=${this.lobbyId}`

      this.addReceiveNewConnectionIdListener();
      this.addReceiveRemoveConnectionIdListener();

      this.addReceiveInitiateCall();
      this.addReceiveHandleOfferListener();
      this.addReceiveHandleAnswerListener();
      this.addReceiveHandleCandidateListener();
      // add required initial listeners
      this.addReceiveErrorMessageListener();

      // my monstrosity of a hack to hopefully stop two people from sending startGame at the same time
      // so that commenceGame isnt done twice


      await this.streamHubConnection.start().then(() => {


          console.log("getting connections ids in lobby")
          this.swalService.showSwal(SwalTypes.Info, "Waiting For Game To Start...")

      });


    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Stream Hub');
    }
  }

  private addReceiveInitiateCall() {
    this.streamHubConnection.on("InitiateCall", async (connection: ConnectionModel) => {
      await this.call(connection);
    });
  }


  private async stop() {
    await this.streamHubConnection.stop();
    this.rtcPeerConnectionsMap.clear();
  }

  private async call(connection: ConnectionModel) {
    console.log('call', connection)


    if (!this.rtcPeerConnectionsMap.has(connection.connectionId)) {

      await this.initRtcPeerConnection(connection.connectionId, this.rtcPeerConnectionsMap.size)

      const rtcPeerConnection = this.rtcPeerConnectionsMap.get(connection.connectionId);
      if (rtcPeerConnection) {
        await this.createOffer(connection.connectionId, rtcPeerConnection);
      }
    }
  }


  private addReceiveNewConnectionIdListener = () => {
    this.streamHubConnection.on("NewConnectionId", async (connection: ConnectionModel) => {
      if (this.connections?.length) {
        this.connections.push(connection);
      } else {
        this.connections = [connection];
      }
    });
  }

  private removeReceiveNewConnectionIdListener = () => {
    this.streamHubConnection.off("NewConnectionId");
  }

  private addReceiveRemoveConnectionIdListener = () => {
    this.streamHubConnection.on("RemoveConnectionId", (connectionId: string) => {
      this.connections = this.connections.filter(c => c.connectionId !== connectionId);
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
    console.log("init", connectionId, videoIndex)
    const stream = await navigator.mediaDevices.getUserMedia({
      video: true,
      audio: true
    }).catch((err) => {
      this.swalService.showSwal(SwalTypes.Error, "Unable to get Microphone and/or Camera");
    });

    if (stream) {
      const remoteStream = new MediaStream();

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
        const index = this.connections.findIndex(c => c.connectionId === offer.connectionIdOfWhoOffered);
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
      console.log("connectionIdOfWhoANswer", answer.connectionIdOfWhoAnswered)
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
