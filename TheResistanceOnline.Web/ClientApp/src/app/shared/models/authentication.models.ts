export interface AuthenticationModel {
    access_token: string,
    expires_in: number,
    refresh_token: string,
    id_token: string,
    scope: string,
    token_type: string,
    name: string,
    role: string
}

export enum Roles {
    Admin = "Admin",
    Moderator = "Moderator",
    User = "User"
}
