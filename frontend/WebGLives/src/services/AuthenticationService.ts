import { Tokens } from "../types/Tokens";

export class AuthenticationService {
    constructor(
        readonly url: string
        ) { }

    private readonly usersPath: string = `${this.url}/users`
    private readonly tokensPath: string = `${this.url}/tokens`

    public async register(login: string, password: string): Promise<Response> {
        const request = this.LoginFormData(login, password);
        const response = await fetch(`${this.usersPath}`, { method: 'POST', body: request });
        return await response.json();
    }

    public async login(login: string, password: string): Promise<Tokens> {
        const request = this.LoginFormData(login, password);
        const response = await fetch(`${this.tokensPath}`, { method: 'POST', body: request });
        return await response.json();
    }

    private LoginFormData(login: string, password: string): FormData {
        const data = new FormData();
        data.append('Login', login);
        data.append('Password', password);
        return data;
    }
}