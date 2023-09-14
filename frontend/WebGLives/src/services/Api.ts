import { GameCardData } from "../types/GameCardData";
import { UploadGameRequest } from "../types/UploadGameRequest";

export class Api {
    private static readonly url: string = "http://localhost:5072"
    //private static readonly url: string = "http://localhost:3000"

    public static async gamePages(): Promise<GameCardData[]> {
        const response = await fetch(`${Api.url}/games`)
        const json = await response.json();
        const gameCards = json as GameCardData[];
        //return gameCards;
        gameCards.forEach(element => {
            element.gameUrl = `${Api.url}${element.gameUrl}`
            element.posterUrl = `${Api.url}${element.posterUrl}`
        });

        return gameCards;
    }

    public static async gamePage(id: string): Promise<GameCardData> {
        const response = await fetch(`${Api.url}/games/${id}`)
        const json = await response.json();
        //return json;
        json.gameUrl = `${Api.url}${json.gameUrl}`
        json.posterUrl = `${Api.url}${json.posterUrl}`
        return json;
    }

    public static async create(): Promise<GameCardData> {
        const response = await fetch(`${Api.url}/games`, { method: `POST` })
        const newGame = await response.json();
        return newGame;
    }

    public static async delete(id: string): Promise<void> {
        await fetch(`${Api.url}/games/${id}`, { method: 'DELETE' });
    }

    public static async update(id: string, title?: string, description?: string,
        poster?: File, game?: File) : Promise<void> {
        const request = new FormData()

        if (title)
            request.append('title', title)
        
        if (description)
            request.append('description', description)
        
        if (poster) {
            alert(poster)
            request.append('poster', poster)
        }
        
        if (game)
            request.append('game', game)

        await fetch(`${Api.url}/games/${id}`, { method: `PUT`, body: request })
    }

    public static async updateTitle(id: string, title: string): Promise<void> {
        await fetch(`${Api.url}/games/${id}/title?title=${title}`, { method: `PUT`})
    }

    public static async updateDescription(id: string, description: string): Promise<void> {
        await fetch(`${Api.url}/games/${id}/description?description=${description}`, { method: `PUT`})
    }

    public static async updatePoster(id: string, poster: File): Promise<void> {
        const request = new FormData()
        request.append('poster', poster)
        await fetch(`${Api.url}/games/${id}/poster`, { method: `PUT`, body: request})
    }

    public static async updateGame(id: string, game: File): Promise<void> {
        const request = new FormData()
        request.append('game', game)
        await fetch(`${Api.url}/games/${id}/game`, { method: `PUT`, body: request})
    }
}