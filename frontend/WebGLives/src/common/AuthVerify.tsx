import React, { useEffect } from "react"
import { useLocation } from "react-router";
import { Profile } from "../services/Profile";

interface AuthVerifyProps {
    onLogout: () => void;
}

export const AuthVerify: React.FC<AuthVerifyProps> = ({onLogout}) => {
    const location = useLocation();

    useEffect(() => {
        const user = Profile.getUsername();

        if (user) {
            onLogout();
        }
        else {
            alert("what?")
        }
    }, [location, onLogout]);

    return (
        <> 
        </>
    )
}

