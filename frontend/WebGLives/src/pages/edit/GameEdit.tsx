import { 
    Box, 
    HStack, 
    Input, 
    Textarea, 
    Button, 
    Text,
    Spacer,
    useToast,
  } from '@chakra-ui/react'
import { ChangeEvent, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { Dropzone } from './Dropzone'
import { Api } from '../../services/Api';

type EditDetails = {
    id: string;
}

export const GameEdit: React.FC = () => {
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
        const targetId = id!

        if (title)
            await Api.updateTitle(targetId, title)
        
        if (description)
            await Api.updateDescription(targetId, description)

        if (poster)
            await Api.updatePoster(targetId, poster)

        if (game)
            await Api.updateGame(targetId, game)

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

    return (
        <Box alignItems="center" p={5}>
            <Text>Game title:</Text>
            <Input placeholder="Enter a game title..." type="text" name="title" onChange={handleTitleChange}/>
            <Spacer my="20px"/>

            <Text>Game description:</Text>
            <Textarea 
                onChange={handleDescriptionChange}
                placeholder="Enter a detailed description for your game..." 
                name="description"
            />
            <Spacer my="20px"/>

            <Text>Game Poster:</Text>
            <Dropzone 
                onFileAccepted={setPoster}
                dragActiveText={"Drop image file here ..."}
                dragDeactiveText={"Dran \`n\` drop image file here, or click to select files"}
            />
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