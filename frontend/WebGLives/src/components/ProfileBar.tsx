import { 
    Box, 
    HStack,
    Link, 
    Button, 
    Avatar, 
    Menu,
    MenuList,
    MenuItem,
    MenuGroup,
    MenuButton,
    IconButton,
    MenuDivider
} from "@chakra-ui/react";
import { SettingsIcon } from "@chakra-ui/icons";

export interface ProfileBarProps {
    username?: string;
    avatar?: string;
    onLogout?: () => void;
}

export const ProfileBar: React.FC<ProfileBarProps> = ({username, avatar, onLogout}) => {
    return (
        <>
            <HStack spacing="10px">
                <Menu>
                    <MenuButton > 
                        <Avatar size="sm" name={username} src={avatar} />
                    </MenuButton>
                    <MenuList>
                        <MenuGroup title={username}>
                            <MenuItem>My Account</MenuItem> 
                            <MenuItem>My Games</MenuItem>
                        </MenuGroup>
                        <MenuDivider />
                        <MenuItem onClick={onLogout}>Logout</MenuItem>
                    </MenuList>
                </Menu>

            </HStack>
        </>
    )
}