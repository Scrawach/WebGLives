import { GameService } from "./GameService";
import { TokenService } from "./TokenService";

export class Api {
    private static readonly url: string = "http://localhost:5072"
    private static readonly jsonServerUrl: string = "http://localhost:3001"

    public static readonly games: GameService = new GameService(Api.url);
    public static readonly tokens: TokenService = new TokenService(Api.url);
}