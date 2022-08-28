# ENSEK-project

## EnsekBackend
.NET Core 3.1 WebAPI service with a single controller exposing POST /meter-reading-uploads endpoint
- Includes initial set of unit tests
- Provides Swagger UI at https://localhost:PORT/swagger url


## EnsekFrontend-React
Upload File simple interface made in ReactJS

Before running tha application the REACT_APP_WEBAPIHOST in '.env' file needs to be updated with the WebAPI server url

- npm install restore
- npm start


## EnsekFrontend-Angular
Upload File simple interface made in Angular

Before running tha application the baseUrl in 'file-upload.service.ts' file needs to be updated with the WebAPI server url

- npm install restore
- ng serve -o