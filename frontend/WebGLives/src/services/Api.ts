import { GameCardData } from "../types/GameCardData";
import { UploadGameRequest } from "../types/UploadGameRequest";

export class Api {
    private static readonly url: string = "http://localhost:5072"

    public static async gamePages(): Promise<GameCardData[]> {
        const response = await fetch(`${Api.url}/GamePages`)
        const json = await response.json();
        return json;
    }

    public static async gamePage(id: string): Promise<GameCardData> {
        const response = await fetch(`${Api.url}/GamePages/${id}`)
        const json = await response.json();
        return json;
    }

    public static async upload(file: File): Promise<void> {
        const formData = new FormData();
        formData.append('file', file);
        await fetch(`${Api.url}/api/files`, {
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
        await fetch(`${Api.url}/api/files`, {
            method: 'POST',
            body: request,
        })
    }
}