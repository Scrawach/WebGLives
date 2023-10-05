import { useEffect, useState } from "react";
import { Box, Grid, Tab, Tabs, TabList, TabPanels, TabPanel } from "@chakra-ui/react";
import { GameCard } from "./GameCard"
import { Api } from "../../services/Api"
import { AddGameCard } from "./AddGameCard";
import { Game } from "../../types/Game";

export const Dashboard: React.FC = () => {
    const [games, setGames] = useState<Game[]>([]);

    useEffect(() => {
        async function fetchGames() {
            var games = await Api.games.all();
            setGames(games);
        }
        fetchGames();
    }, [])

    return (
        <Box>
            <Tabs isManual variant="enclosed">
                <TabList>
                    <Tab>Recently</Tab>
                    <Tab>Top rated</Tab>
                    <Tab>Most played</Tab>
                </TabList>
                <TabPanels>
                    <TabPanel></TabPanel>
                    <TabPanel></TabPanel>
                    <TabPanel></TabPanel>
                </TabPanels>
            </Tabs>
            <>
            <Grid templateColumns="repeat(4, 1fr)" p="10px" gap={5}>
                {games.map(game => 
                    (<GameCard
                        id = {game.id}
                        title = {game.title}
                        icon = {game.posterUrl}
                    />))}
                    <AddGameCard />
            </Grid>
            </>
        </Box>
    );
}