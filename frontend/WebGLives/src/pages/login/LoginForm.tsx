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
import { useLoginForm } from "./useLoginForm";

export interface LoginFormProps {
    onLogin?: (login: string, password: string) => void;
    onSignUpClicked?: () => void;
}


export const LoginForm: React.FC<LoginFormProps> = ({onLogin, onSignUpClicked}) => {
    const { login, password, handleLogin, handlePassword } = useLoginForm();

    const onLoginProcess = async () => {
        if (onLogin)
            await onLogin(login, password);
        window.location.reload();
    }

    return (
        <> 
        <Stack spacing={4}>
            <FormControl id="login">
                <HStack justify='space-between'>
                    <FormLabel>Login</FormLabel>
                    <HStack>
                        <Text fontSize="sm">Need an account?</Text>
                        <Link fontSize="sm" color="blue.500" onClick={onSignUpClicked}>Sign up</Link>
                    </HStack>
                </HStack>
                <Input type="login" onChange={(e) => handleLogin(e.target.value)}/>
            </FormControl>
            <FormControl id="password">
                <FormLabel>Password</FormLabel>
                <Input type="password" onChange={(e) => handlePassword(e.target.value)}/>
            </FormControl>
            <Stack spacing={5}>
                <Checkbox>Remember me</Checkbox>
                <Button
                    bg={'blue.400'}
                    color={'white'}
                    _hover={{bg: 'blue.500',}}      
                    onClick={onLoginProcess}        
                    >
                    Login
                </Button>
            </Stack>
        </Stack>
        </>
    )
}