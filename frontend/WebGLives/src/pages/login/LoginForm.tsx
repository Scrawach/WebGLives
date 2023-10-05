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

export interface LoginFormProps {
    onSignUp?: () => void;
}

export const LoginForm: React.FC<LoginFormProps> = ({onSignUp}) => {
    return (
        <> 
        <Stack spacing={4}>
            <FormControl id="login">
                <HStack justify='space-between'>
                    <FormLabel>Login</FormLabel>
                    <HStack>
                        <Text fontSize="sm">Need an account?</Text>
                        <Link fontSize="sm" color="blue.500" onClick={onSignUp}>Sign up</Link>
                    </HStack>
                </HStack>
                <Input type="login" />
            </FormControl>
            <FormControl id="password">
                <FormLabel>Password</FormLabel>
                <Input type="password" />
            </FormControl>
            <Stack spacing={5}>
                <Checkbox>Remember me</Checkbox>
                <Button
                    bg={'blue.400'}
                    color={'white'}
                    _hover={{
                    bg: 'blue.500',
                    }}>
                    Login
                </Button>
            </Stack>
        </Stack>
        </>
    )
}