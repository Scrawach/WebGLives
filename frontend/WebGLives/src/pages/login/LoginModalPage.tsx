import React from "react";
import { 
    Button,
    useDisclosure,
    Modal, 
    ModalOverlay, 
    ModalContent, 
    ModalHeader, 
    ModalCloseButton, 
    ModalBody, 
    ModalFooter 
} from "@chakra-ui/react";
import { LoginForm } from "./LoginForm";

export const LoginModalPage: React.FC = () => {
    const {isOpen, onOpen, onClose} = useDisclosure()
    return (
        <>
            <Button onClick={onOpen}>Open Modal</Button>
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