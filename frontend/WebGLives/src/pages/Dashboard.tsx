import { useEffect, useState } from "react";
import { Container, SimpleGrid } from "@chakra-ui/react";
import { GameCardData } from "../types/GameCardData";
import { GameCard } from "../components/GameCard"
import { Api } from "../services/Api"

export const Dashboard: React.FC = () => {
    const [gamePages, setGamePages] = useState<GameCardData[]>([]);



    return (
        <SimpleGrid p="10px" columns={4} gap={5} minChildWidth={200}>
            <GameCard
                id = {"0"}
                title = {"Stone Soul"}
                description = {"Best of the best game!"}
                icon = {"https://img.itch.zone/aW1nLzczNTE1MzgucG5n/315x250%23c/lbfSqO.png"}
                url = {""}
            />
        </SimpleGrid>
    );
}