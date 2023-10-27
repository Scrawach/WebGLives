import React from "react";
import { 
    Text,
    HStack,
    Link,
    Button,
    Stack,
    FormControl,
    FormLabel,
    Input,
} from "@chakra-ui/react";
import { useLoginForm } from "./useLoginForm";

export interface SignUpFormProps {
    onSignUp?: (login: string, password: string) => void;
    onLoginClicked?: () => void;
}

export const SignUpForm: React.FC<SignUpFormProps> = ({onSignUp, onLoginClicked}) => {
    const { login, password, handleLogin, handlePassword } = useLoginForm();

    const onSignUpProcess = async () => {
        if (onSignUp)
            await onSignUp(login, password);
    }

    return (
        <>
            <Stack spacing={4}>
                <FormControl id="login" isRequired>
                    <HStack justify={'space-between'}>
                        <FormLabel>Login</FormLabel>
                        <HStack>
                            <Text fontSize="sm">Already have an account?</Text>
                            <Link fontSize="sm" color="blue.500" onClick={onLoginClicked}>Log in</Link>
                        </HStack>
                    </HStack>
                    <Input type="login" onChange={(e) => handleLogin(e.target.value)}/>
                </FormControl>
                <FormControl id="password" isRequired>
                    <FormLabel>Password</FormLabel>
                    <Input type="password" onChange={(e) => handlePassword(e.target.value)}/>
                </FormControl>
                <FormControl id="reapeatedPassword" isRequired>
                    <FormLabel>Repeat Password</FormLabel>
                    <Input type="password" />
                </FormControl>
                    <Button
                        type="submit"
                        bg={'blue.400'}
                        color={'white'}
                        _hover={{ bg: 'blue.500',}}
                        onClick={onSignUpProcess}>
                        Sign in
                    </Button>
            </Stack>
        </>
    )
}