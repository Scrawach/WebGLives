import { ChakraProvider, Container } from "@chakra-ui/react"
import { Dashboard } from "./pages/Dashboard"
import {
  Route,
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import { NavigationBar } from "./components/NavigationBar";
import UploadPage from "./pages/UploadPage";
import { CreateGame, createAction } from "./pages/CreateGame";

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/">
      <Route index element={<Dashboard />} />
      <Route path="create" element={<CreateGame />} action={createAction} />
    </Route>
  )
)

export const App = () => (
  <ChakraProvider>
    <Container maxWidth="1080px">
      <NavigationBar />
      <RouterProvider router={router} />
    </Container>
  </ChakraProvider>
)
