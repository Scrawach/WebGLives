import { 
    Card, 
    CardBody,
    CardFooter,
    Heading, 
    Text,
    GridItem, 
    Image,
    Stack,
    Divider,
    Button 
} from "@chakra-ui/react";
import { useNavigate } from "react-router";

export interface GameCardProps {
    id: string;
    title: string;
    description?: string;
    icon?: string;
    url: string;
}

export const GameCard: React.FC<GameCardProps> = ({id, title, description, icon, url}) => {
    let navigate = useNavigate();
    const toGamePage = (game_id: string) => {
        navigate("/games/" + game_id);
    }

    return (
        <GridItem>
            <Card maxW='sm'>
                <CardBody>
                    <Image src={icon} alt="Game Poster" borderRadius='lg'/>
                    <Stack mt='6' spacing='3'>
                        <Heading size='md'>{title} ({id})</Heading>
                        <Text>{description}</Text>
                    </Stack>
                </CardBody>
                <Divider />
                <CardFooter>
                    <Button colorScheme='blue' onClick={() => toGamePage(id)}>
                        Play
                    </Button>
                </CardFooter>
            </Card>
        </GridItem>
    );
}