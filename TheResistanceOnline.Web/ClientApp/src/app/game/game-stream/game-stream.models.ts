export interface SendOfferCommand {
  RTCSessionDescription: RTCSessionDescriptionInit;
}

export interface SendCandidateCommand {
  RTCIceCandidate: RTCIceCandidateInit;
}


export interface SendAnswerCommand {
  RTCSessionDescription: RTCSessionDescriptionInit;
}
