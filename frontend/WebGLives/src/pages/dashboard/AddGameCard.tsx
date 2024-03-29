import { AddIcon } from "@chakra-ui/icons";
import { Text, Button, VStack } from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";
import { Api } from "../../services/Api";

export const AddGameCard = () => {
    const navigate = useNavigate()

    const createGame = async () => {
        const game = await Api.games.create();
        navigate(`/edit/${game.id}`)
    }

    return (
            <Button
                rounded="xl" 
                position="relative"
                width="full"
                maxW="250px"
                height="200px"
                transition="all 0.3s"
                transition-timing-function="spring(1 100 10 10)"
                _hover={{ transform: "translateY(-4px)", shadow: "xl" }}
                onClick={createGame}
            >
                <VStack spacing={4}>
                    <AddIcon w={20} h={20} color="gray.400"></AddIcon>
                    <Text color="gray.400">New Game</Text>           
                </VStack>
            </Button>
    )
}