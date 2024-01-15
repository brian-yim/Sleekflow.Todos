
# Sleekflow Todos

A demo TODOs backend project built with ASP.NET Core. 


## Features

- TODOs CRUD
- TODOs filtering & sorting
- User Authentication & Registration


## Tech Stack

- ASP.NET Core 8
- Entity Framework Core 8
- XUnit
- BCrypt

## Get Started

Restore dependency:
```bash
dotnet restore
```

Launch the app:
```bash
dotnet run --project Sleekflow.Todos.Web\Sleekflow.Todos.Web.csproj
```


## Database

The project is configured to use SQL Server by default. 


## Environment Variables

To run this project, you will need to add the following variables to your appsettings.json

`ConnectionStrings:DefaultConnection`

For MSSQL connection string

`JwtSettings:Issuer`

`JwtSettings:SignKey`

`JwtSettings:ExpiredTime`

For JWT local authentication


## Deployment

To deploy this project with Docker

```bash
docker build . -t sleekflow.todos.web
```

```bash
docker run -p 8080:8080 sleekflow.todos.web
```

## Futher Enhancement

- Add GraphQL publish/subscription for real-time update
- Add Todo permiission for share to others