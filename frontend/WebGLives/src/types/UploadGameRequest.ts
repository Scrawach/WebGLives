export interface UploadGameRequest {
    title: string;
    icon?: File | null | undefined;
    description: string;
    game?: File | null | undefined;
}