import { ChakraProvider, Box, Grid, theme } from "@chakra-ui/react"
import { ColorModeSwitcher } from "./ColorModeSwitcher"
import { Dashboard } from "./pages/Dashboard"
import { FileUploadPage } from "./pages/FileUploadPage";
import {
  RouterProvider,
  createBrowserRouter,
} from "react-router-dom";
import { GamePage } from "./pages/GamePage";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <div className="App">
        <Dashboard />
      </div>
    ),
  },
  {
    path: "/upload",
    element: (
      <div className="App">
        <FileUploadPage />
      </div>
    ),
  },
  {
    path: "/play",
    element: (
      <div className="App">
        <GamePage />
      </div>
    ),
  },
]);

export const App = () => (
  <ChakraProvider theme={theme}>
    <Box textAlign="center" fontSize="xl">
      <Grid minH="100vh" p={3}>
        <ColorModeSwitcher justifySelf="flex-end" />
        <RouterProvider router={router} />
      </Grid>
    </Box>
  </ChakraProvider>
)
