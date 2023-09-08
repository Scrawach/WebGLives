import { ChakraProvider, Container } from "@chakra-ui/react"
import { Dashboard } from "./pages/Dashboard"
import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import { NavigationBar } from "./components/NavigationBar";
import { CreateGame } from "./pages/CreateGame";
import { GamePage } from "./pages/GamePage";
import { Footer } from "./components/Footer";

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/">
      <Route index element={<Dashboard />} />
      <Route path="create" element={<CreateGame />} />
      <Route path="games/:id" element={<GamePage />} />
    </Route>
  )
)

export const App = () => (
  <ChakraProvider>
    <Container maxWidth="1080px" minHeight="100vh">
      <NavigationBar />
      <RouterProvider router={router} />
      <Footer />
    </Container>
  </ChakraProvider>
)
