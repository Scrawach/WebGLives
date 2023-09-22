export interface GameService {
    all(): Game[];
    get(id: number): number | Game; 
}

interface Game {

}

class BackendGameService implements GameService {
    constructor(
        readonly url: string
        ) { }

    public async all(): Promise<Game[]> {
        const response = await fetch(this.url);
        const json = await response.json();
        return json;
    }

    public async get(id: number): Promise<number | Game> {
        const response = await fetch(`${this.url}/${id}`);
        const json = await response.json();
        return json
    }
}

class Test {
    public static games: GameService = new BackendGameService("");
    
}