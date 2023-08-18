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

export default function UploadPage() {
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
                <Input />
            </FormControl>
            <FormControl id="icon">
                <FormLabel>Icon</FormLabel>
                <Input type="file" name="file" />
            </FormControl>
            <FormControl id="description">
                <FormLabel>Short Description</FormLabel>
                <Input />
            </FormControl>
            <FormControl id="game-archive">
                <FormLabel>Zip Archive</FormLabel>
                <Input type="file" name="file" />
            </FormControl>
            <Button bg={'blue.400'} color={'white'} _hover={{ bg: 'blue.500',}}> 
                Upload
            </Button>
          </Stack>
        </Box>
      </Stack>
    </Flex>
  )
}