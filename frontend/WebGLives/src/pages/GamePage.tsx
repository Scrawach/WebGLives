import { useState, useEffect } from 'react';
import { useParams } from "react-router-dom";
import { Api } from "../services/Api";
import { GameCardData } from "../types/GameCardData";
import { Box, Heading, Text } from "@chakra-ui/react";

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

    return (
        <Box>
            <Heading mx="40px">{gameCard?.title}</Heading>
            <iframe key={id} src={gameCard?.gameUrl} seamless width="100%" height="660"/>
            <Text mx="40px">{gameCard?.description}</Text>
        </Box>
    );
} 