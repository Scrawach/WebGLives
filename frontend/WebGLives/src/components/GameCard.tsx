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

export interface GameCardProps {
    id: string;
    title: string;
    description?: string;
    icon?: string;
}

export const GameCard: React.FC<GameCardProps> = ({id, title, description, icon}) => {
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
                    <Button colorScheme='blue'>
                        Play
                    </Button>
                </CardFooter>
            </Card>
        </GridItem>
    );
}