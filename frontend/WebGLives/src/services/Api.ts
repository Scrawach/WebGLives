import { GameService } from "./GameService";
import { GameServiceBase } from "./GameServiceBase";

export class Api {
    private static readonly url: string = "http://localhost:5072"
    private static readonly jsonServerUrl: string = "http://localhost:3001"

    public static readonly games: GameService = new GameServiceBase(Api.jsonServerUrl);
}