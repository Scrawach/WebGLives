import { Profile } from "./Profile";

export class AuthBaseService {
    public getAuthenticationHeader(): Headers {
        const headers = new Headers();
        const tokens = Profile.tokens();
        headers.set("Authorization", `Bearer ${tokens?.accessToken}`)
        return headers;
    }
}