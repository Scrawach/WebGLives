import React from "react";
import { 
    Text,
    Modal, 
    ModalOverlay, 
    ModalContent, 
    ModalHeader, 
    ModalCloseButton, 
    ModalBody, 
    ModalFooter 
} from "@chakra-ui/react";
import { useState } from "react";
import { LoginForm } from "./LoginForm";
import { SignUpForm } from "./SignUpForm";
import { Api } from "../../services/Api";
import { Profile } from "../../services/Profile";

export interface LoginModalPageProps {
    isOpen: boolean;
    onClose: () => void;
}

export const LoginModalPage: React.FC<LoginModalPageProps> = ({isOpen, onClose}) => {
    const [isSignUp, setSignUp] = useState<boolean>();
    
    const handleLogin = async (login: string, password: string) => {
        const tokens = await Api.auth.login(login, password);
        Profile.save(login, tokens);
    }

    const handleSignUp = async (login: string, password: string) => {
        const response = await Api.auth.register(login, password);
        const tokens = await Api.auth.login(login, password);
        Profile.save(login, tokens);
    }

    return (
        <>
            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalOverlay>
                    <ModalContent>
                        <ModalHeader>
                            {!isSignUp && <Text>Login</Text>}
                            {isSignUp && <Text>Sign Up</Text>}
                        </ModalHeader>
                        <ModalCloseButton />
                        <ModalBody>
                            {!isSignUp && <LoginForm onLogin={handleLogin} onSignUpClicked={() => setSignUp(true)}/>}
                            {isSignUp && <SignUpForm onSignUp={handleSignUp} onLoginClicked={() => setSignUp(false)}/>}
                        </ModalBody>
                        <ModalFooter />
                    </ModalContent>
                </ModalOverlay>
            </Modal>
        </>
    )
}