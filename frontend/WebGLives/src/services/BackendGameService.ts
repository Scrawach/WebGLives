import { GameService } from "./GameService";
import { Game } from "../types/Game";

export class BackendGameService extends GameService {
    public async all(): Promise<Game[]> {
        const games = await super.all();

        games.forEach(game => {
            this.convertRelativeToAbsolutePaths(game);
        });

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