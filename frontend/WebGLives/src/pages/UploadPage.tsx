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
    <>
    </>
  )
}