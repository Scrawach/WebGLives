import { Game } from "../types/Game"
import { UpdateGame } from "../types/UpdateGame";
import { Api } from "./Api";
import { BaseService } from "./BaseService";
import { Profile } from "./Profile";

export class GameService extends BaseService {
    constructor(
        readonly url: string
        ) { super() }

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
        const response = await fetch(this.gamesPath, 
            { 
                method: `POST`,
                headers: this.getAuthorizationHeader()
            });

        if (response.status == 401)
        {
            const tokens = await Api.auth.refresh(Profile.tokens()!);
            Profile.save(Profile.getUsername()!, tokens);
            const response = await fetch(this.gamesPath, 
                { 
                    method: `POST`,
                    headers: this.getAuthorizationHeader()
                });
            return await response.json();           
        }

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