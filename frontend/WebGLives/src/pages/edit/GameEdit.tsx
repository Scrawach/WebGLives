import { 
    Box, 
    HStack, 
    Input, 
    Textarea, 
    Button, 
    Text,
    Spacer,
    useToast,
    Image,
  } from '@chakra-ui/react'
import { ChangeEvent, useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { Dropzone } from './Dropzone'
import { Api } from '../../services/Api';
import { GameCardData } from '../../types/GameCardData';

type EditDetails = {
    id: string;
}

export const GameEdit: React.FC = () => {
    const [gamePage, setGamePage] = useState<GameCardData>();
    const { id } = useParams<EditDetails>();

    const navigate = useNavigate()
    const [title, setTitle] = useState<string>();
    const [description, setDescription] = useState<string>();
    const [game, setGame] = useState<File>();
    const [poster, setPoster] = useState<File>();
    const toast = useToast()

    const handleDescriptionChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
        if (e.target)
            setDescription(e.target.value)
    }

    const handleTitleChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target)
            setTitle(e.target.value)
    }

    const saveEdit = async () => {
        await Api.update(id!, title, description, poster, game)

        toast({
            title: "Game success edit.",
            status: 'success',
            duration: 3000,
            isClosable: true,
        })
        navigate(`/`)
    }

    const deleteGame = async () => {
        await Api.delete(id!)
        navigate(`/`)
    }

    useEffect(() => {
        async function fetchGamePages() {
            var page = await Api.gamePage(id!);
            setGamePage(page);
        }
        fetchGamePages();
    }, [])

    return (
        <Box alignItems="center" p={5}>
            <Text>Game title:</Text>
            <Input defaultValue={gamePage?.title} placeholder="Enter a game title..." type="text" name="title" onChange={handleTitleChange}/>
            <Spacer my="20px"/>

            <Text>Game description:</Text>
            <Textarea 
                defaultValue={gamePage?.description}
                onChange={handleDescriptionChange}
                placeholder="Enter a detailed description for your game..." 
                name="description"
            />
            <Spacer my="20px"/>

            <Text>Game Poster:</Text>

            <HStack direction="row" align="stretch">
                <Image 
                    rounded="xl"
                    h="250px"
                    fit="cover"
                    src={gamePage?.posterUrl}
                    bg="white"
                /> 
                <Dropzone 
                    onFileAccepted={setPoster}
                    dragActiveText={"Drop image file here ..."}
                    dragDeactiveText={"Dran \`n\` drop image file here, or click to select files"}
                />
            </HStack>
            <Spacer my="20px"/>

            <Text>Game Files:</Text>
            <Dropzone 
                onFileAccepted={setGame}
                dragActiveText={"Drop .zip file here ..."}
                dragDeactiveText={"Dran \`n\` drop .zip file here, or click to select files"}
            />
            <Spacer my="20px"/>

            <HStack>
                <Button onClick={saveEdit}>Save</Button>
                <Spacer />
                <Button onClick={deleteGame} colorScheme="red">Delete</Button>
            </HStack>
        </Box>
    )
}