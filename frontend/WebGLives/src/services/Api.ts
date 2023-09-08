import { GameCardData } from "../types/GameCardData";
import { UploadGameRequest } from "../types/UploadGameRequest";

export class Api {
    private static readonly url: string = "http://localhost:5072"

    public static async gamePages(): Promise<GameCardData[]> {
        const response = await fetch(`${Api.url}/games`)
        const json = await response.json();
        const gameCards = json as GameCardData[];
        
        gameCards.forEach(element => {
            element.gameUrl = `${Api.url}${element.gameUrl}`
            element.posterUrl = `${Api.url}${element.posterUrl}`
        });

        return gameCards;
    }

    public static async gamePage(id: string): Promise<GameCardData> {
        const response = await fetch(`${Api.url}/games/${id}`)
        const json = await response.json();

        json.gameUrl = `${Api.url}${json.gameUrl}`
        json.posterUrl = `${Api.url}${json.posterUrl}`
        return json;
    }

    public static async upload(file: File): Promise<void> {
        const formData = new FormData();
        formData.append('file', file);
        await fetch(`${Api.url}/games`, {
            method: 'POST',
            body: formData
        });
    }

    public static async uploadGame(data: UploadGameRequest): Promise<void> {
        const request = new FormData()
        request.append('game', data.game as File)
        request.append('title', data.title)
        request.append('description', data.description)
        request.append('icon', data.icon as File)
        await fetch(`${Api.url}/games`, {
            method: 'POST',
            body: request,
        })
    }
}