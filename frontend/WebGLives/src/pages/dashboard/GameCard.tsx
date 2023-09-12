import { 
    Box, 
    Flex, 
    Image, 
    Text, 
    Spacer, 
    HStack, 
    LinkOverlay 
} from "@chakra-ui/react";

import { ViewIcon } from '@chakra-ui/icons'
import { RatingBar } from "../../components/RatingBar";

export interface GameCardProps {
    id: string;
    title: string;
    description?: string;
    icon?: string;
    url: string;
}

export const GameCard: React.FC<GameCardProps> = ({id, title, description, icon, url}) => {
    const gamePageUrl = `games/${id}`
    
    return (
        <Box
            key={id}
            rounded="xl" 
            position="relative"
            width="full"
            maxW="250px"
            height="200px"
            transition="all 0.3s"
            transition-timing-function="spring(1 100 10 10)"
            _hover={{ transform: "translateY(-4px)", shadow: "xl" }}
        >
            <LinkOverlay href={gamePageUrl}>
            
            <Image 
                rounded="xl" 
                h="full" 
                w="full" 
                fit="cover"
                src={icon}
                bg="white"
            />

            <Box
                position="absolute"
                top="0"
                h="30%"
                w="100%"
                bgGradient="linear(#000000FF, #FFFFFF00)"
                borderTopLeftRadius="xl"
                borderTopRightRadius="xl"
            >
                <Box display="flex" mx="2" my="2" alignItems="center" fontSize="xs">
                    <RatingBar number={4}/>
                </Box>
            </Box>

            <Box 
                position="absolute" 
                bottom="0" 
                h="40%" 
                w="100%" 
                bgGradient="linear(#FFFFFF00, #000000FF)"
                borderBottomLeftRadius="xl"
                borderBottomRightRadius="xl"
            >
                <Flex p={3} my="40px" alignItems="center">
                    <Text align="left" color="gray.400">{title}</Text>
                    <Spacer />
                    <HStack fontSize="xs">
                        <ViewIcon color="gray.400" verticalAlign="middle"/>
                        <Text align="left" color="gray.400">1.4k</Text>
                    </HStack>
                </Flex>
            </Box>

            </LinkOverlay>
        </Box>
    );
}