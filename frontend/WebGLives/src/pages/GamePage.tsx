import { useLocation } from "react-router";

export const GamePage: React.FC = () => {
    const location = useLocation();
    const {title, url} = location.state
    return (
        <>
            {title}
            <div>
                <iframe src={url} width="1080" height="800"/>
            </div>
        </>
    );
} 