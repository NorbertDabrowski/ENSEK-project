import React, { ChangeEvent, useRef, useState } from 'react';
import './App.css';
import { Box, Button, TextareaAutosize, TextField, Typography } from '@mui/material';
import axios from 'axios';

function App() {
  const [file, setFile] = useState<File | undefined>();
  const [uploadResponse, setUploadResponse] = useState<string>("");

  const handleFilePick = (event: ChangeEvent<HTMLInputElement>) => {
    var files = event.currentTarget.files;
    if (files) {
      console.log(files);
      setFile(files[0]);
      setUploadResponse("");
    }
  }

  const handleSubmit = () => {
    if (file) {
      const url = process.env.REACT_APP_WEBAPIHOST + '/meter-reading-uploads';
      const formData = new FormData();
      formData.append('file', file);
      formData.append('fileName', file.name);
      const config = {
        headers: {
          'content-type': 'multipart/form-data',
        },
      };
      axios.post(url, formData, config).then((response) => {
        console.log(response);
        if (response.status === 200) {
          var text = "Successfull Readings: " + response.data["numberOfSuccessfullReadings"]
            + "\r\nFailed Readings: " + + response.data["numberOfFailedReadings"];
          setUploadResponse(text);
        } else {
          setUploadResponse(JSON.stringify(response.data));
        }
      }).catch((error) => {
        if (error.response) {
          console.log(error.response.data); // => the response payload 
          setUploadResponse(JSON.stringify(error.response.data));
        }
      });
    }

    setFile(undefined);
  }

  return (
    <div className="App">
      <header className="App-header">
        <Box component="form" sx={{ '& .MuiTextField-root': { m: 1, width: '25ch' }, }}
          noValidate autoComplete="off" >

          <Typography variant="h5" gutterBottom>
            Uploading Meter Readings via WebAPI service
          </Typography>

          <Box m={2} pt={3}>
            <Button variant="contained" component="label">
              Select File
              <input type="file" hidden onChange={event => handleFilePick(event)} />
            </Button>
          </Box>

          <TextField id="filled-basic" variant="filled" label="File to upload" value={file ? file.name : ""} />

          <Box m={2} pt={3}>
            <Button variant="contained" component="label" disabled={!file}>
              Upload
              <input type="button" hidden onClick={() => handleSubmit()} />
            </Button>
          </Box>

          <TextareaAutosize
            aria-label="maximum height"
            value={uploadResponse}
            style={{ width: 450, height: 150 }} />

        </Box>
      </header>
    </div>
  );
}

export default App;
