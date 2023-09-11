import { useEffect, useState } from "react";
import { Box, Grid, Tab, Tabs, TabList, TabPanels, TabPanel } from "@chakra-ui/react";
import { GameCardData } from "../types/GameCardData";
import { GameCard } from "../components/GameCard"
import { Api } from "../services/Api"
import { AddGameCard } from "../components/AddGameCard";

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
                {gamePages.map(game => 
                    (<GameCard
                        id = {game.id}
                        title = {game.title}
                        description = {game.description}
                        icon = {game.posterUrl}
                        url = {game.gameUrl}
                    />))}
                    <AddGameCard />
            </Grid>
            </>
        </Box>
    );
}