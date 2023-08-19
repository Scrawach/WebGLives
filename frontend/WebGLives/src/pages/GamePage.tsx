import { useLocation } from "react-router";
import { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import { Api } from "../services/Api";
import { GameCardData } from "../types/GameCardData";

type GamePageDetails = {
    id: string;
}

export const GamePage: React.FC = () => {
    const { id } = useParams<GamePageDetails>();
    const [gameCard, setGameCard] = useState<GameCardData>()
    
    useEffect(() => {
        async function fetchGameData() {
            const data: GameCardData = await Api.gamePage(id!)
            setGameCard(data)
        }
        fetchGameData();
    }, [id])

    //const location = useLocation();
    //const {title, url} = location.state
    return (
        <>
            <div>
                {gameCard?.title}
            </div>
            <div>
                {gameCard?.description}
            </div>
            <div>
                <iframe src={gameCard?.url} width="1080" height="800"/>
            </div>
        </>
    );
} 