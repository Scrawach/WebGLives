import { useState } from "react";
import { Button, Grid } from "@chakra-ui/react";
import { GamePage } from "../types/GamePage";
import { GameCard } from "../components/GameCard"
import { Api } from "../services/Api"

export const Dashboard: React.FC = () => {
    const [gamePages, setGamePages] = useState<GamePage[]>([]);
    const getGamePages = async () => {
        var pages = await Api.getInstance().getGamePages();
        setGamePages(pages);
    }

    return (
        <>
            <Button onClick={getGamePages}>Load Games</Button>
            <Grid templateColumns='repeat(5, 1fr)' gap={6}>
                {gamePages.map(game => (
                    <GameCard
                        id = {game.id}
                        title = {game.title}
                        description = {game.description}
                        icon = {game.icon}
                    />
                ))}
            </Grid>
        </>
    );
}