import React, { ChangeEvent, useState } from "react";
import { Api } from "../services/Api";

interface ComponentProps {
    name?: string
}

const FileUploadPage: React.FC<ComponentProps> = (props) => {
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
        <div>FileUploadPage</div>;
        <div>
          <input type="file" name="file" onChange={handleFileChange} />
          <input type="button" value="Upload" onClick={uploadFile} />
        </div>
      </>
    );
  };
  
  export default FileUploadPage;