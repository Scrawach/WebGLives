import { Tokens } from "../types/Tokens";

export class Profile {
    private static readonly tokensKey: string = "tokens"

    public static save(tokens: Tokens): void {
        const data = JSON.stringify(tokens);
        localStorage.setItem(this.tokensKey, data);
    }

    public static tokens(): Tokens | null {
        const json = localStorage.getItem(this.tokensKey);
        return json ? JSON.parse(json) : null;
    }
}