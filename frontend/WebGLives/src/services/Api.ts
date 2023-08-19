import { GameCardData } from "../types/GameCardData";
import { UploadGameRequest } from "../types/UploadGameRequest";

export class Api {
    private static readonly url: string = "http://localhost:5072"

    public static async gamePages(): Promise<GameCardData[]> {
        const response = await fetch(`${Api.url}/GamePages`)
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
        await fetch(`${Api.url}/api/files`, {
            method: 'POST',
            headers: {"Content-Type": "apllication/json"},
            body: JSON.stringify(data),
        })
    }
}