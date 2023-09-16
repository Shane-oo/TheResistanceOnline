import {Component, Input, OnDestroy, OnInit, ViewChild} from '@angular/core';
import * as signalR from "@microsoft/signalr";
import {IHttpConnectionOptions} from "@microsoft/signalr";
import {environment} from "../../../environments/environment";
import {AuthenticationService} from "../../shared/services/authentication/authentication.service";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {SendAnswerCommand, SendCandidateCommand, SendOfferCommand} from "./game-stream.models";
import {GameStreamVideoComponent} from "./game-stream-video/game-stream-video.component";

@Component({
  selector: 'app-game-stream',
  templateUrl: './game-stream.component.html',
  styleUrls: ['./game-stream.component.css']
})
export class GameStreamComponent implements OnInit, OnDestroy {
  @ViewChild(GameStreamVideoComponent) childComponent!: GameStreamVideoComponent;

  @Input() lobbyId: string = "";
  private rtcConfiguration: RTCConfiguration = {
    iceServers: [
      {
        urls: [
          'stun:stun1.l.google.com:19305'
        ]
      }
    ],
    iceCandidatePoolSize: 10,
  }

  // todo make this a map which will be of up to size 9 and it will be each peer connection for each person in the lobby
  // so in total there will be up to 9 rtcPeerConnections for each person

  private rtcPeerConnection!: RTCPeerConnection;


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

        // add required initial listeners
        this.addReceiveErrorMessageListener();
        this.addReceiveHandleOfferListener();
        this.addReceiveHandleAnswerListener();
        this.addReceiveHandleCandidateListener();
      });
    } catch (err) {
      this.swalService.showSwal(SwalTypes.Error, 'Error Connecting To Stream Hub');
      setTimeout(() => this.start(), 30000);
    }
  }

  public async stop() {
    await this.streamHubConnection.stop();
  }

  public async makeCall() {
    await this.initRtcPeerConnection();

    const offer = await this.rtcPeerConnection.createOffer();
    await this.rtcPeerConnection.setLocalDescription(offer);

    const command: SendOfferCommand = {
      RTCSessionDescription: offer
    };

    this.streamHubConnection.invoke('SendOffer', command).catch(err => {
      // do nothing with errors because of the error listener
    });
  }

  private async initRtcPeerConnection() {
    this.rtcPeerConnection = new RTCPeerConnection(this.rtcConfiguration);

    const stream = await navigator.mediaDevices.getUserMedia({
      video: true,
      audio: true
    });
    const remoteStream = new MediaStream();

    this.childComponent.remoteVideo.nativeElement.srcObject = remoteStream;

    this.rtcPeerConnection.ontrack = (event: RTCTrackEvent) => {
      event.streams[0].getTracks().forEach((track: MediaStreamTrack) => {
        remoteStream.addTrack(track);
      });
    };

    stream.getTracks().forEach((track: MediaStreamTrack) => {
      this.rtcPeerConnection.addTrack(track, stream);
    });

    this.rtcPeerConnection.onicecandidate = (event: RTCPeerConnectionIceEvent) => {
      if (event.candidate) {
        const command: SendCandidateCommand = {
          RTCIceCandidate: event.candidate.toJSON()
        };
        this.streamHubConnection.invoke('SendCandidate', command).catch(err => {
        });

      }
    }
  }

  private addReceiveErrorMessageListener = () => {
    this.streamHubConnection.on("Error", (errorMessage: string) => {
      this.swalService.showSwal(SwalTypes.Error, errorMessage);
    });
  }

  private addReceiveHandleOfferListener = () => {
    this.streamHubConnection.on('HandleOffer', async (offer: RTCSessionDescription) => {
      await this.initRtcPeerConnection();

      await this.rtcPeerConnection.setRemoteDescription(new RTCSessionDescription(offer));

      const answer = await this.rtcPeerConnection.createAnswer();
      await this.rtcPeerConnection.setLocalDescription(answer);

      const command: SendAnswerCommand = {
        RTCSessionDescription: answer
      };
      this.streamHubConnection.invoke('SendAnswer', command).catch(err => {
        // do nothing with errors because of the error listener
      });
    })
  }

  private addReceiveHandleAnswerListener = () => {
    this.streamHubConnection.on('HandleAnswer', async (answer: RTCSessionDescription) => {
      await this.rtcPeerConnection.setRemoteDescription(new RTCSessionDescription(answer));
    });
  }

  private addReceiveHandleCandidateListener = () => {
    this.streamHubConnection.on("HandleCandidate", async (candidate: RTCIceCandidate) => {
      await this.rtcPeerConnection.addIceCandidate(new RTCIceCandidate(candidate));
    });
  }
}
