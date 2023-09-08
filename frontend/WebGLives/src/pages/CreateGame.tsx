import { 
    Box, 
    FormControl, 
    FormLabel, 
    FormHelperText, 
    Input, 
    Textarea, 
    Button, 
    Checkbox, 
    Divider
  } from '@chakra-ui/react'
import { SyntheticEvent } from 'react'
import { Form, redirect, ActionFunction } from 'react-router-dom'
import { Dropzone } from '../components/Dropzone'
  
export const CreateGame: React.FC = () => {
    return (
        <Box alignItems="center">
        <Form method="post" action="/create">
            <FormControl isRequired mb="40px">
                <FormLabel>Game title:</FormLabel>
                <Input type="text" name="title" />
                <FormHelperText>Enter a game title.</FormHelperText>
            </FormControl>

            <FormControl isRequired mb="40px">
                <FormLabel>Game description:</FormLabel>
                <Textarea 
                    placeholder="Enter a detailed description for your game..." 
                    name="description"
                />
            </FormControl>

            <FormControl isRequired mb="40px">
                <FormLabel>Game Poster:</FormLabel>
                <Dropzone 
                    dragActiveText={"Drop image file here ..."}
                    dragDeactiveText={"Dran \`n\` drop image file here, or click to select files"}
                />
                <FormHelperText>Upload image poster for your game.</FormHelperText>
            </FormControl>

            <FormControl isRequired mb="40px">
                <FormLabel>Game Files:</FormLabel>
                <Dropzone 
                    dragActiveText={"Drop .zip file here ..."}
                    dragDeactiveText={"Dran \`n\` drop .zip file here, or click to select files"}
                />
                <FormHelperText>Upload .zip file with your game.</FormHelperText>
            </FormControl>

            <Button type="submit">Submit</Button>
        </Form>
        </Box>
    )
}

export const createAction : ActionFunction = async ( {params, request} ) => {
    const data = await request.formData()
    alert(data.get('dropzone'))
    const task = {
      title: data.get('title'),
      description: data.get('description')
    }
  
    console.log(task)
  
    return redirect('/')
  }