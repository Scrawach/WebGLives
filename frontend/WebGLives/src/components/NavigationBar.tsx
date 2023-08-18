import { Box, Spacer, Link, HStack, useColorModeValue } from "@chakra-ui/react";
import { ColorModeSwitcher } from "../ColorModeSwitcher";

const NavLink = ({ to = "/", }) => {
    return (
      <Link href={to}>
        {to}
      </Link>
    );
  };

export const NavigationBar : React.FC = () => {
    return (
        <Box
          bg = {useColorModeValue('gray.300', 'gray.700')}
        >
          <HStack
            p={4}
            spacing={8}
            align="center"
          >
            <NavLink to="Dashboard"></NavLink>
            <NavLink to="Upload"></NavLink>
            <Spacer />
            <ColorModeSwitcher />
          </HStack>
        </Box>
      );
}