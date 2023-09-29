import { useState, useEffect } from 'react';
import { useNavigate, useParams } from "react-router-dom";
import { Api } from "../services/Api";
import { Game } from "../types/Game";
import { Box, Heading, Text, Button, HStack, Spacer } from "@chakra-ui/react";

type GamePageDetails = {
    id: string;
}

export const GamePage: React.FC = () => {
    const { id } = useParams<GamePageDetails>();
    const [gameCard, setGameCard] = useState<Game>()
    const navigate = useNavigate()
    
    useEffect(() => {
        async function fetchGameData() {
            const game: Game = await Api.games.get(id!)
            setGameCard(game)
        }
        fetchGameData();
    }, [id])

    const edit = () => {
        navigate(`/edit/${id}`)
    }

    return (
        <Box>
            <HStack>
                <Heading mx="40px">{gameCard?.title}</Heading>
                <Spacer />
                <Button onClick={edit}>Edit</Button>
            </HStack>
            <iframe key={id} src={gameCard?.gameUrl} seamless width="100%" height="660"/>
            <Text mx="40px">{gameCard?.description}</Text>
        </Box>
    );
} 