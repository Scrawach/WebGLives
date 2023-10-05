import React from "react";
import { 
    Text,
    HStack,
    Link,
    Button,
    Stack,
    FormControl,
    FormLabel,
    Checkbox,
    Input,
} from "@chakra-ui/react";

export interface SignUpFormProps {
    onLogin?: () => void;
}

export const SignUpForm: React.FC<SignUpFormProps> = ({onLogin}) => {
    return (
        <> 
        <Stack spacing={4}>
            <FormControl id="login" isRequired>
                <HStack justify={'space-between'}>
                    <FormLabel>Login</FormLabel>
                    <HStack>
                        <Text fontSize="sm">Already have an account?</Text>
                        <Link fontSize="sm" color="blue.500" onClick={onLogin}>Log in</Link>
                    </HStack>
                </HStack>
                <Input type="login" />
            </FormControl>
            <FormControl id="password" isRequired>
                <FormLabel>Password</FormLabel>
                <Input type="password" />
            </FormControl>
            <FormControl id="repeatPassword" isRequired>
                <FormLabel>Repeat Password</FormLabel>
                <Input type="repeatPassword" />
            </FormControl>
                <Button
                    bg={'blue.400'}
                    color={'white'}
                    _hover={{
                    bg: 'blue.500',
                    }}>
                    Sign in
                </Button>
        </Stack>
        </>
    )
}