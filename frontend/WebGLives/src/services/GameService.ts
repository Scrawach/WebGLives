import { Game } from "../types/Game";
import { UpdateGame } from "../types/UpdateGame";

export interface GameService {
    all(): Promise<Game[]>;
    get(id: string): Promise<Game>;
    create(): Promise<Game>;
    delete(id: string): Promise<Response>;
    update(data: UpdateGame): Promise<Response>;

    updateTitle(id: string, title: string): Promise<Response>;
    updateDescription(id: string, description: string): Promise<Response>;
    updatePoster(id: string, poster: File): Promise<Response>;
    updateGame(id: string, game: File): Promise<Response>;
}