import { 
    Box, 
    FormControl, 
    FormLabel, 
    FormHelperText, 
    Input, 
    Textarea, 
    Button, 
    Image,
    Checkbox, 
    FormErrorMessage,
    InputGroup,
    Icon,
    Divider
  } from '@chakra-ui/react'
import { UploadGameRequest } from "../types/UploadGameRequest";
import { ChangeEvent, useState } from 'react'
import { Form, redirect, ActionFunction, useNavigate } from 'react-router-dom'
import { Dropzone } from '../components/Dropzone'
import { Api } from '../services/Api';

export const CreateGame: React.FC = () => {
    const navigate = useNavigate()
    const [title, setTitle] = useState<string>();
    const [description, setDescription] = useState<string>();
    const [game, setGame] = useState<File>();
    const [poster, setPoster] = useState<File>();

    const handleDescriptionChange = (e: ChangeEvent<HTMLTextAreaElement>) => {
        if (e.target)
            setDescription(e.target.value)
    }

    const handleTitleChange = (e: ChangeEvent<HTMLInputElement>) => {
        if (e.target)
            setTitle(e.target.value)
    }

    const send = async () => {
        const uploadGameRequest : UploadGameRequest = {
            title: title as string,
            description: description as string,
            icon: poster,
            game: game
        }
    
        await Api.uploadGame(uploadGameRequest);
        navigate("/")
    }

    return (
        <Box alignItems="center" p={5}>
        <Form method="post" action="/create">
            <FormControl isRequired mb="40px">
                <FormLabel>Game title:</FormLabel>
                <Input type="text" name="title" onChange={handleTitleChange}/>
                <FormHelperText>Enter a game title.</FormHelperText>
            </FormControl>

            <FormControl isRequired mb="40px">
                <FormLabel>Game description:</FormLabel>
                <Textarea 
                    onChange={handleDescriptionChange}
                    placeholder="Enter a detailed description for your game..." 
                    name="description"
                />
            </FormControl>

            <FormControl mb="40px">
                <FormLabel>Game Poster:</FormLabel>
                <Dropzone 
                    onFileAccepted={setPoster}
                    dragActiveText={"Drop image file here ..."}
                    dragDeactiveText={"Dran \`n\` drop image file here, or click to select files"}
                />
                <FormHelperText>Upload image poster for your game.</FormHelperText>
            </FormControl>

            <FormControl mb="40px">
                <FormLabel>Game Files:</FormLabel>
                <Dropzone 
                    onFileAccepted={setGame}
                    dragActiveText={"Drop .zip file here ..."}
                    dragDeactiveText={"Dran \`n\` drop .zip file here, or click to select files"}
                />
                <FormHelperText>Upload .zip file with your game.</FormHelperText>
            </FormControl>

            <Button onClick={send}>Submit</Button>
        </Form>
        </Box>
    )
}

export const createAction : ActionFunction = async ( {params, request} ) => {
    const data = await request.formData()
    const uploadGameRequest : UploadGameRequest = {
        title: data.get('title') as string,
        description: data.get('description') as string,
        icon: data.get('poster') as File,
        game: data.get('game') as File
    }
  
    await Api.uploadGame(uploadGameRequest);
  

  }