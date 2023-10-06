import { useState } from "react"

export const useLoginForm = () => {
    const [login, setLogin] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    
    const handleLogin = (loginProp: string) => {
        setLogin(loginProp);
    }

    const handlePassword = (passwordProp: string) => {
        setPassword(passwordProp);
    }

    return {login, password, handleLogin, handlePassword};
}