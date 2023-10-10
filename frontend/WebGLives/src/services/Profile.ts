import { Tokens } from "../types/Tokens";

export class Profile {
    private static readonly tokensKey: string = "tokens"
    private static readonly usernameKey: string = "username"

    public static getUsername(): string | null {
        return localStorage.getItem(this.usernameKey);
    }

    public static login(username: string, tokens: Tokens): void {
        const data = JSON.stringify(tokens);
        localStorage.setItem(this.usernameKey, username);
        localStorage.setItem(this.tokensKey, data);
    }

    public static logout(): void {
        localStorage.removeItem(this.tokensKey);
        localStorage.removeItem(this.usernameKey);
    }

    public static isAuthorized(): boolean {
        return this.tokens()?.accessToken != null;
    }

    public static tokens(): Tokens | null {
        const json = localStorage.getItem(this.tokensKey);
        return json ? JSON.parse(json) : null;
    }
}