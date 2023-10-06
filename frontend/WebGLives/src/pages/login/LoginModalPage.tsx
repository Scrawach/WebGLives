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

export interface LoginModalPageProps {
    isOpen: boolean;
    onClose: () => void;
}

export const LoginModalPage: React.FC<LoginModalPageProps> = ({isOpen, onClose}) => {
    const [isSignUp, setSignUp] = useState<boolean>();
    
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
                            {!isSignUp && <LoginForm onSignUpClicked={() => setSignUp(true)}/>}
                            {isSignUp && <SignUpForm onLoginClicked={() => setSignUp(false)}/>}
                        </ModalBody>
                        <ModalFooter />
                    </ModalContent>
                </ModalOverlay>
            </Modal>
        </>
    )
}