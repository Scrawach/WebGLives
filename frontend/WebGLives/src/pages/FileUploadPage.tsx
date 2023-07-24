import React, { ChangeEvent, useState } from "react";
import { Input } from "@chakra-ui/react";
import { Api } from "../services/Api";

interface ComponentProps {
    name?: string
}

export const FileUploadPage: React.FC<ComponentProps> = (props) => {
    const uploadFile = async () => {
      if (!file) {
        return;
      }
  
      await Api.upload(file);
    };
  
    const [file, setFile] = useState<File>();
  
    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
      if (e.target.files) {
        setFile(e.target.files[0]);
      }
    };
  
    return (
      <>
        <div>Upload page</div>
        <div>
          <Input type="file" name="file" onChange={handleFileChange} />
          <Input type="button" value="Upload" onClick={uploadFile} />
        </div>
      </>
    );
  };