# BevMan

## Setup Certificates
To be able to run the application locally, you need to create a self-signed certificate. You can do this by running the following command in directory `$HOME/.aspnet/https/`

`openssl req -x509 -sha256 -nodes -newkey rsa:2048 -days 365 -subj '/CN=bev-man' -keyout bev-man.key -out bev-man.pem`
After that you need to trust the certificate in your keychain

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```
