import { useDisclosure, Text, Box, Spacer, HStack, Button, Input, InputLeftElement, LinkOverlay, InputGroup } from "@chakra-ui/react";
import { SmallAddIcon, SearchIcon, CalendarIcon } from "@chakra-ui/icons";
import { ColorModeSwitcher } from "../ColorModeSwitcher";
import { useNavigate } from "react-router-dom";
import { Api } from "../services/Api";
import { LoginModalPage } from "../pages/login/LoginModalPage";
import { Profile } from "../services/Profile";

export const NavigationBar : React.FC = () => {
    const navigate = useNavigate()

    const createGame = async () => {
        const game = await Api.games.create();
        navigate(`/edit/${game.id}`);
    }

    const handleLogout = async () => {
      Profile.logout();
      window.location.reload();
    }

    const {isOpen, onOpen, onClose} = useDisclosure()

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

            {!Profile.isAuthorized() && 
              <Button onClick={onOpen}>
                Login
              </Button>
            }

            {Profile.isAuthorized() && 
              <>
                <Button leftIcon={<SmallAddIcon />} onClick={createGame}>
                  New Game
                </Button>
                <Text>
                  {Profile.getUsername()}
                </Text>
                <Button onClick={handleLogout}>
                  Logout
                </Button>
              </>
            }

            <LoginModalPage isOpen={isOpen} onClose={onClose}/>
            <ColorModeSwitcher />
          </HStack>
        </Box>
      );
}