import { Center, Text, useColorModeValue } from '@chakra-ui/react';
import { DownloadIcon } from '@chakra-ui/icons';
import { useCallback } from 'react';
import { useDropzone } from 'react-dropzone'

export interface DropzoneProps {
    onFileAccepted?: String;
    dragActiveText?: String;
    dragDeactiveText?: String;
}

export const Dropzone: React.FC<DropzoneProps> = ({onFileAccepted, dragActiveText, dragDeactiveText}) => {
    const onDrop = useCallback( (acceptedFiles: Blob[]) => {
        // Do something with the files
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
            <input {...getInputProps()} />
            <DownloadIcon mr={2} />
            <Text>{dropText}</Text>
        </Center>
    )
}