import React from "react";
import { 
    Modal, 
    ModalOverlay, 
    ModalContent, 
    ModalHeader, 
    ModalCloseButton, 
    ModalBody, 
    ModalFooter 
} from "@chakra-ui/react";
import { LoginForm } from "./LoginForm";

export interface LoginModalPageProps {
    isOpen: boolean;
    onClose: () => void;
}

export const LoginModalPage: React.FC<LoginModalPageProps> = ({isOpen, onClose}) => {
    return (
        <>
            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalOverlay>
                    <ModalContent>
                        <ModalHeader>
                            Sing up
                        </ModalHeader>
                        <ModalCloseButton />
                        <ModalBody>
                            <LoginForm />
                        </ModalBody>
                        <ModalFooter />
                    </ModalContent>
                </ModalOverlay>
            </Modal>
        </>
    )
}