import { Credentials } from "../types/Credentials";
import { Tokens } from "../types/Tokens";

export class TokenService {
    constructor(
        readonly url: string
        ) { }

    private readonly tokensPath: string = `${this.url}/tokens`

    public async create(credentials: Credentials): Promise<Response> {
        const data = new FormData();
        data.append(`Login`, credentials.login);
        data.append(`Password`, credentials.password);

        return await fetch(this.tokensPath, { method: `POST`, body: data});
    }

    public async decode(token: string): Promise<Response> {
        return await fetch(`${this.tokensPath}?token=${token}`, { method: `GET`})
    }

    public async refresh(tokens: Tokens): Promise<Tokens> {
        const data = new FormData();
        data.append(`AccessToken`, tokens.accessToken);
        data.append(`RefreshToken`, tokens.refreshToken);
        
        const response = await fetch(`${this.tokensPath}/refresh`, { method: `PUT`, body: data });
        return await response.json();
    }
}