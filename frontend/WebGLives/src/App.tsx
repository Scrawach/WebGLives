import { ChakraProvider, Container } from "@chakra-ui/react"
import { Dashboard } from "./pages/dashboard/Dashboard"
import {
  Route,
  Routes,
  BrowserRouter,
} from "react-router-dom";
import { NavigationBar } from "./components/NavigationBar";
import { GameEdit } from "./pages/edit/GameEdit";
import { GamePage } from "./pages/GamePage";
import { Footer } from "./components/Footer";
import { AuthVerify } from "./common/AuthVerify";
import { Profile } from "./services/Profile";

export const App = () => (
  <ChakraProvider>
    <Container maxWidth="1080px" minHeight="100vh">
      <BrowserRouter>
        <NavigationBar />
        
        <Routes>
          <Route path="/">
            <Route index element={<Dashboard />} />
            <Route path="edit/:id" element={<GameEdit />} />
            <Route path="games/:id" element={<GamePage />} />
          </Route>
        </Routes>

        <AuthVerify onLogout={() => {
          Profile.logout();
          window.location.reload();
        }}/>

        <Footer />
      </BrowserRouter>
    </Container>
  </ChakraProvider>
)
