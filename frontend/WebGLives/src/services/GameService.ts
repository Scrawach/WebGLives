import { Game } from "../types/Game"
import { UpdateGame } from "../types/UpdateGame";

export class GameService {
    constructor(
        readonly url: string
        ) { }

    private readonly gamesPath: string = `${this.url}/games`

    public async all(): Promise<Game[]> {
        const response = await fetch(this.gamesPath);
        return await response.json();
    }

    public async get(id: string): Promise<Game> {
        const response = await fetch(`${this.gamesPath}/${id}`);
        return await response.json();
    }

    public async create(): Promise<Game> {
        const response = await fetch(this.gamesPath, { method: `POST` });
        return await response.json();
    }

    public async delete(id: string): Promise<Response> {
        return await fetch(`${this.gamesPath}/${id}`, { method: `DELETE` });
    }

    public async update(data: UpdateGame): Promise<Response> {
        const request = new FormData()
        if (data.title)
            request.append('title', data.title);
        if (data.description)
            request.append('description', data.description);
        if (data.poster)
            request.append('poster', data.poster);
        if (data.game)
            request.append(`game`, data.game);

        return await fetch(`${this.gamesPath}/${data.id}`, { method: `PUT`, body: request });
    }

    public async updateTitle(id: string, title: string): Promise<Response> {
        return await fetch(`${this.gamesPath}/${id}/title`, { method: `PUT`, body: title})
    }

    public async updateDescription(id: string, description: string): Promise<Response> {
        return await fetch(`${this.gamesPath}/${id}/description`, { method: `PUT`, body: description})
    }

    public async updatePoster(id: string, poster: File): Promise<Response> {
        const request = new FormData()
        request.append('poster', poster)
        return await fetch(`${this.gamesPath}/${id}/poster`, { method: `PUT`, body: request})
    }

    public async updateGame(id: string, game: File): Promise<Response> {
        const request = new FormData()
        request.append('game', game)
        return await fetch(`${this.gamesPath}/${id}/game`, { method: `PUT`, body: request})
    }
}