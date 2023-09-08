import { Box, Spacer, HStack, useColorModeValue, Button, Input, InputLeftElement, LinkOverlay, InputGroup } from "@chakra-ui/react";
import { SmallAddIcon, SearchIcon, CalendarIcon } from "@chakra-ui/icons";
import { ColorModeSwitcher } from "../ColorModeSwitcher";

export const NavigationBar : React.FC = () => {
    const bgColor = useColorModeValue("gray.300", "gray.700");

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

            
            <Button leftIcon={<SmallAddIcon />}>
              <LinkOverlay href="/create"> 
                New Game
              </LinkOverlay>
            </Button>
 
            <ColorModeSwitcher />
          </HStack>
        </Box>
      );
}