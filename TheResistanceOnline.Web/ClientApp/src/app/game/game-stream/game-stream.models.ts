export interface SendOfferCommand {
  rtcSessionDescription: RTCSessionDescriptionInit;
  connectionIdOfWhoOfferIsFor: string;
}

export interface SendCandidateCommand {
  rtcIceCandidate: RTCIceCandidateInit;
  connectIdOfWhoCandidateIsFor: string;
}


export interface SendAnswerCommand {
  rtcSessionDescription: RTCSessionDescriptionInit;
  connectionIdOfWhoAnswerIsFor: string; // who to tell you answered their call
}


export interface HandleOfferModel {
  connectionIdOfWhoOffered: string,
  rtcSessionDescription: RTCSessionDescriptionInit;
}

export interface HandleAnswerModel {
  connectionIdOfWhoAnswered: string;
  rtcSessionDescription: RTCSessionDescriptionInit;
}

export interface HandleCandidateModel {
  connectionIdOfWhoSentCandidate: string;
  rtcIceCandidate: RTCIceCandidateInit;
}


