import { GamePage } from "../types/GamePage";

export class Api {
    private static readonly url: string = "http://localhost:5072"
    private static instance: Api;

    private constructor() { }

    public static getInstance(): Api {
        if (!Api.instance)
            Api.instance = new Api();
        return Api.instance;
    }

    public async getGamePages(): Promise<GamePage[]> {
        const response = await fetch(`${Api.url}/GamePages`)
        const json = await response.json();
        return json;
    }

    public static async upload(file: File): Promise<void> {
        const formData = new FormData();
        formData.append('file', file);
        await fetch(`${Api.url}/files`, {
            method: 'POST',
            body: formData
        });
    }
}