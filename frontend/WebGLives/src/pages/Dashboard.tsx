import { useEffect, useState } from "react";
import { Button, Grid } from "@chakra-ui/react";
import { GameCardData } from "../types/GameCardData";
import { GameCard } from "../components/GameCard"
import { Api } from "../services/Api"

export const Dashboard: React.FC = () => {
    const [gamePages, setGamePages] = useState<GameCardData[]>([]);

    useEffect(() => {
        async function fetchGamePages() {
            var pages = await Api.gamePages();
            setGamePages(pages);
        }
        fetchGamePages();
    }, [])

    return (
        <>
            <Grid templateColumns='repeat(5, 1fr)' gap={6}>
                {gamePages.map(game => (
                    <GameCard
                        id = {game.id}
                        title = {game.title}
                        description = {game.description}
                        icon = {game.icon}
                        url = {game.url}
                    />
                ))}
            </Grid>
        </>
    );
}