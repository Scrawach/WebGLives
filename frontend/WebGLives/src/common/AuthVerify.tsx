import React, { useEffect } from "react"
import { useCallback } from "react";
import { useLocation } from "react-router";
import { Api } from "../services/Api";
import { Profile } from "../services/Profile";

interface AuthVerifyProps {
    onLogout: () => void;
}

export const AuthVerify: React.FC<AuthVerifyProps> = ({onLogout}) => {
    const location = useLocation();

    const refreshTokens = useCallback(async() => {
        const previousTokens = Profile.tokens()!;
        const username = Profile.getUsername()!;
        const tokens = await Api.auth.refresh(previousTokens);
        Profile.save(username, tokens) 
    }, [])

    useEffect(() => {
        if (Profile.hasUser() && Profile.hasRefreshToken())
        {
            refreshTokens().catch(onLogout);
        }
    }, [location, refreshTokens, onLogout]);

    return (
        <> 
        </>
    )
}

