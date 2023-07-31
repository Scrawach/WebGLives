import { useLocation } from "react-router";
import { useParams } from "react-router-dom";

type GamePageDetails = {
    id: string;
}

export const GamePage: React.FC = () => {
    const { id } = useParams<GamePageDetails>();
    //const location = useLocation();
    //const {title, url} = location.state
    return (
        <>
            {id}
        </>
    );
} 