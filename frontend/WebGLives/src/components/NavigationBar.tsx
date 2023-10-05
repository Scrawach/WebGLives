import { useDisclosure, Box, Spacer, HStack, Button, Input, InputLeftElement, LinkOverlay, InputGroup } from "@chakra-ui/react";
import { SmallAddIcon, SearchIcon, CalendarIcon } from "@chakra-ui/icons";
import { ColorModeSwitcher } from "../ColorModeSwitcher";
import { useNavigate } from "react-router-dom";
import { Api } from "../services/Api";
import { LoginModalPage } from "../pages/login/LoginModalPage";

export const NavigationBar : React.FC = () => {
    const navigate = useNavigate()

    const createGame = async () => {
        const game = await Api.games.create();
        navigate(`/edit/${game.id}`)
    }

    const {isOpen, onOpen, onClose} = useDisclosure()
    const isLogin = true;

    return (
        <Box>
          <HStack
            p={4}
            spacing={8}
            align="center"
          >
            
            <Button variant="ghost" leftIcon={<CalendarIcon />}>
              <LinkOverlay href="/">
                Dashboard
              </LinkOverlay>
            </Button>

            <Spacer />
            
            <InputGroup width = "50%">
              <InputLeftElement pointerEvents="none">
                <SearchIcon />
              </InputLeftElement>
              <Input type="tel" placeholder="Search..." />
            </InputGroup>

            {isLogin && 
              <Button leftIcon={<SmallAddIcon />} onClick={onOpen}>
                Sign Up
              </Button>
            }

            {!isLogin && 
              <Button leftIcon={<SmallAddIcon />} onClick={createGame}>
                New Game
              </Button>
            }

            <LoginModalPage isOpen={isOpen} onClose={onClose}/>
            <ColorModeSwitcher />
          </HStack>
        </Box>
      );
}