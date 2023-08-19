import { ChakraProvider, Box, Grid, theme } from "@chakra-ui/react"
import { Dashboard } from "./pages/Dashboard"
import { FileUploadPage } from "./pages/FileUploadPage";
import {
  Routes,
  Route,
} from "react-router-dom";
import { GamePage } from "./pages/GamePage";
import { NavigationBar } from "./components/NavigationBar";
import UploadPage from "./pages/UploadPage";


export const App = () => (
  <ChakraProvider theme={theme}>
    <Box textAlign="center" fontSize="xl">
      <NavigationBar />
      <Routes>
        <Route path="/" element={<Dashboard />}></Route>
        <Route path="/dashboard" element={<Dashboard />}></Route>
        <Route path="/upload" element={<UploadPage />}></Route>
        <Route path="/games/:id" element={<GamePage />}></Route>
      </Routes>
    </Box>
  </ChakraProvider>
)
