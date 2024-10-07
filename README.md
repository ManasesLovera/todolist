# TodoList App

### Built using:

- Backend
    - ASP .NET Framework 4.6.2
    - SQLite
    - EntityFrameworkCore
    - NUnit
      
- Frontend
    - Angular 7
    - Bootstrap

![image](https://github.com/user-attachments/assets/fc16217d-37d3-41dc-9c7b-216282f4f39b)

## Setup

You must have installed NPM and .NET Framework for running both frontend and backend.
1. Clone the repository

### Running backend

Just click on the start button in visual studio or type `dotnet run` in the terminal to run the backend.

### Running frontend

If you check the package.json I already have the angular cli for version 7 installed, since I have another angular version, so the first thing you must do is going to the frontend project `cd TodoListFrontend`, then `npm install` to get angular, bootstrap and all dependencies needed, and last but not least do `npm start` and your application should run and request the backend that is also running.

## API docs

1. GET - api/todolist
<br> `response.statusCode : 200 OK`
```json
[
    {
        Id: number,
        Title: string,
        Description: string,
        Priority: string,
        IsCompleted: boolean
    }
]
```

2. GET - api/todolist/{id}: 
<br> `response.statusCode : 200 OK`
```json
{
    Id: number,
    Title: string,
    Description: string,
    Priority: string,
    IsCompleted: boolean
}
```
<br> `response.statusCode : 404 NOT FOUND`<br>
<br>
3. POST - api/todolist/
<br> `request.body`
```json
{
    Id: number,
    Title: string,
    Description: string,
    Priority: string,
    IsCompleted: boolean
}
```
`response.statusCode : 201 CREATED` <br>
`response.statusCode : 400 BAD REQUEST` <br>
`response.statusCode : 409 CONFLICT` <br><br>
4. PUT - api/todolist/{id}
<br> `request.body`
```json
{
    Title: string,
    Description: string,
    Priority: string,
    IsCompleted: boolean
}
```
`response.statusCode : 200 OK` <br>
`response.statusCode : 404 NOT FOUND` <br><br>
5. DELETE - api/todolist/{id} <br>
`response.statusCode : 204 NO CONTENT` <br>
`response.statusCode : 404 NOT FOUND` <br><br>
6. PATCH - api/todolist/{id}: Switch true|false to the IsCompleted property <br>
`response.statusCode : 200 OK` <br>
`response.statusCode : 404 NOT FOUND` <br><br>
