import { Center, Text, useColorModeValue } from '@chakra-ui/react';
import { DownloadIcon } from '@chakra-ui/icons';
import { useCallback } from 'react';
import { useDropzone } from 'react-dropzone'
import { useState } from 'react';
import { Flex } from '@chakra-ui/react'

export interface DropzoneProps {
    onFileAccepted: CallableFunction;
    dragActiveText?: String;
    dragDeactiveText?: String;
}

export const Dropzone: React.FC<DropzoneProps> = ({onFileAccepted, dragActiveText, dragDeactiveText}) => {
    const [file, setFile] = useState<File>();

    const onDrop = useCallback( (acceptedFiles: File[]) => {
        setFile(acceptedFiles[0])
        onFileAccepted(acceptedFiles[0])
      }, [])
    
    const {getRootProps, getInputProps, isDragActive} = useDropzone({onDrop})

    const dropText = isDragActive ? dragActiveText : dragDeactiveText;
    const activeBg = useColorModeValue('gray.100', 'gray.600');
    const borderColor = useColorModeValue(
      isDragActive ? 'teal.300' : 'gray.300',
      isDragActive ? 'teal.500' : 'gray.500',
    );

    return (
        <Center
            p={10}
            cursor="pointer"
            bg={isDragActive ? activeBg : 'transparent'}
            _hover={{ bg: activeBg }}
            transition="background-color 0.2s ease"
            borderRadius={4}
            border="3px dashed"
            borderColor={borderColor}
            {...getRootProps()}
        >
            <input {...getInputProps()}/>            
            {
                file && 
                <Flex alignContent="center">
                    <Text>{file.name}</Text>
                </Flex>
            }
            {
                !file &&
                <Flex alignContent="center">
                    <DownloadIcon mr={2} />
                    <Text>{dropText}</Text>
                </Flex>
            }
        </Center>
    )
}