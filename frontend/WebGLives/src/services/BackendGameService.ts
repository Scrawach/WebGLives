import { GameServiceBase } from "./GameServiceBase";
import { Game } from "../types/Game";

export class BackendGameService extends GameServiceBase {
    public async all(): Promise<Game[]> {
        const games = await super.all();
        games.forEach(this.convertRelativeToAbsolutePaths);
        return games;
    }

    public async get(id: string): Promise<Game> {
        const game = await super.get(id);
        return this.convertRelativeToAbsolutePaths(game);
    }

    convertRelativeToAbsolutePaths(game: Game): Game{
        if (game.gameUrl)
            game.gameUrl = `${this.url}${game.gameUrl}`
        
        if (game.posterUrl)
            game.posterUrl = `${this.url}${game.posterUrl}`

        return game;
    }
}