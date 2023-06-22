import { ChakraProvider, Box, Grid, theme } from "@chakra-ui/react"
import { ColorModeSwitcher } from "./ColorModeSwitcher"
import { Dashboard } from "./pages/Dashboard"

export const App = () => (
  <ChakraProvider theme={theme}>
    <Box textAlign="center" fontSize="xl">
      <Grid minH="100vh" p={3}>
        <ColorModeSwitcher justifySelf="flex-end" />
        <Dashboard/>
      </Grid>
    </Box>
  </ChakraProvider>
)
