import {
  Flex,
  Box,
  FormControl,
  FormLabel,
  Input,
  Checkbox,
  Stack,
  Button,
  Heading,
  Text,
  useColorModeValue,
} from '@chakra-ui/react'
import { ChangeEvent, useState } from 'react'
import { UploadGameRequest } from '../types/UploadGameRequest'
import { Api } from '../services/Api';

export default function UploadPage() {
  const [gameRequest, setGameRequest] = useState<UploadGameRequest>({
    title: '',
    description: '',
  }); 

  const handleTitleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setGameRequest((prev) => ({ ...prev, title: event.target.value }));
  }

  const handleDescriptionChange = (event: ChangeEvent<HTMLInputElement>) => {
    setGameRequest((prev) => ({ ...prev, description: event.target.value }));
  }

  const handleGameFileChange = (event: ChangeEvent<HTMLInputElement>) => {
    const archive: File | null | undefined = event.target?.files?.item(0)
    setGameRequest((prev) => ({ ...prev, game: archive }));
  }

  const handleIconFileChange = (event: ChangeEvent<HTMLInputElement>) => {
    const archive: File | null | undefined = event.target?.files?.item(0)
    setGameRequest((prev) => ({ ...prev, icon: archive }));
  }

  const uploadFile = async () => {
    await Api.uploadGame(gameRequest)
  };

  return (
    <Flex
      minH={'100vh'}
      minW={'100vw'}
      align={'center'}
      justify={'center'}>
      <Stack spacing={8} mx={'auto'} py={12} w={'80vw'}>
        <Stack align={'center'}>
          <Heading fontSize={'4xl'}>Upload game</Heading>
        </Stack>
        <Box
          rounded={'lg'}
          bg={useColorModeValue('white', 'gray.700')}
          boxShadow={'lg'}
          p={8}>
          <Stack spacing={4}>
            <FormControl id="name">
                <FormLabel>Name</FormLabel>
                <Input onChange={handleTitleChange}/>
            </FormControl>
            <FormControl id="icon">
                <FormLabel>Icon</FormLabel>
                <Input type='file' onChange={handleIconFileChange}/>
            </FormControl>
            <FormControl id="description">
                <FormLabel>Short Description</FormLabel>
                <Input onChange={handleDescriptionChange}/>
            </FormControl>
            <FormControl id="game-archive">
                <FormLabel>Zip Archive</FormLabel>
                <Input type="file" onChange={handleGameFileChange}/>
            </FormControl>
            <Button bg={'blue.400'} color={'white'} _hover={{ bg: 'blue.500',}} onClick={uploadFile}> 
                Upload
            </Button>
          </Stack>
        </Box>
      </Stack>
    </Flex>
  )
}