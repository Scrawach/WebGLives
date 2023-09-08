import { useEffect, useState } from "react";
import { Box, SimpleGrid, Tab, Tabs, TabList, TabPanels, TabPanel } from "@chakra-ui/react";
import { GameCardData } from "../types/GameCardData";
import { GameCard } from "../components/GameCard"
import { Api } from "../services/Api"
import { AddIcon } from "@chakra-ui/icons";
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
            <SimpleGrid p="10px" columns={4} gap={5} minChildWidth={200}>
                {gamePages.map(game => 
                    (<GameCard
                        id = {game.id}
                        title = {game.title}
                        description = {game.description}
                        icon = {game.posterUrl}
                        url = {game.gameUrl}
                    />))}
                    <AddGameCard />
            </SimpleGrid>
            </>
        </Box>
    );
}