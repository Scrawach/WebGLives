import React, { useEffect } from "react"
import { useLocation } from "react-router";
import { Profile } from "../services/Profile";

interface AuthVerifyProps {
    onLogout: () => void;
}

export const AuthVerify: React.FC<AuthVerifyProps> = ({onLogout}) => {
    const location = useLocation();

    useEffect(() => {
        if (Profile.hasUser() && Profile.isTokenExpired())
        {
            onLogout();
        }
    }, [location, onLogout]);

    return (
        <> 
        </>
    )
}

