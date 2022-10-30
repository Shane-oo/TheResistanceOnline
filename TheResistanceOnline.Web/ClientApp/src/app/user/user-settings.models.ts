import { UserIdCommand } from '../shared/models/command.models';

export interface UserSettingsUpdateCommand extends UserIdCommand {
  updateUserWantsToUseDiscord: boolean;
  userWantsToUseDiscord: boolean;
}
