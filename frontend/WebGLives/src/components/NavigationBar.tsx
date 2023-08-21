import { Box, Spacer, Link, HStack, useColorModeValue } from "@chakra-ui/react";
import { ColorModeSwitcher } from "../ColorModeSwitcher";

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
            <Link href="/dashboard">Dashboard</Link>
            <Link href="/upload">Upload</Link>
            <Spacer />
            <ColorModeSwitcher />
          </HStack>
        </Box>
      );
}