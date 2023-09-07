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
  import { Form } from 'react-router-dom'
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

            <FormControl mb="40px">
                <FormLabel>Game description:</FormLabel>
                <Textarea 
                    placeholder="Enter a detailed description for your game..." 
                    name="description"
                />
            </FormControl>

            <FormControl display="flex" alignItems="center" mb="40px">
                <Checkbox 
                    name="isPriority" 
                    colorScheme="purple"
                    size="lg"
                />
                <FormLabel mb="0" ml="10px">Make a priority task</FormLabel>
            </FormControl>

            <Dropzone />

            <Button type="submit">Submit</Button>
        </Form>
        </Box>
    )
}