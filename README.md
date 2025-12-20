### Bookings API By Group 2 (Marcus, David, Dusan, Lukas & Kevin(OFF Course)) ###

## No frontend is included—this is an API-only project in Agile Developing course.

## Requirements

# .NET SDK 8.x
# MySQL Workbench (Local DB) IP: 127.0.0.1.3306(make notice that ip and port could be diferent)
# Postman (API testing)
# VS CODE (Development)
# Git Bash

# Wireframes

https://excalidraw.com/#room=da6cb73c6d10a098489a,npNcQ6K5GchMZO_71JoLWg
https://excalidraw.com/#room=44e936580ef6298f723c,K9hAhUIR-RVu3mVWLMaHxg


## Testing (Basics)

Base URL example: `http://localhost:5030` In Postman(make notice that port could be diffrent)
-------------------------------------
Create/reset schema and seed test data. Use these endpoints from Postman:
DELETE .../db Drop & Reset tables data
PUT .../db Insert Data for testing
-------------------------------------
Dotnet Run in GitBash
-------------------------------------
## POSTMAN Basic Queries:
-------------------------------------
# Admin 
Login: POST .../login/admin  -- email and password in Body field
POST .../admin -- create new admin
GET .../admin/users -- list all users
GET .../admin/employees -- list all employees
-------------------------------------
# Employee 
Login: POST .../login/employee -- Employee login
POST .../employees -- Create new employee
GET .../employees -- List all amployees
DELETE .../login/employee -- Log-out an employe
-------------------------------------
# User 
Login: POST .../login -- User Login
DELETE .../login -- User log-out
-------------------------------------

## POSTMAN Testing queries

LOGIN USER
---------------
http://localhost:5030/Login
POST 
{
    "email":"test.user@example.com",
    "password":"hashed_password_123"
}
----------------
ACCESS USER PROFILE (after login)
-------------------------------------
http://localhost:5030/users/profiles
POST
-------------------------------------
VIEW BOOKINGS FOR LOGGED IN USER (shows bookings that logged in user has)
-----------------------------------
http://localhost:5030/users/bookings
POST
-----------------------------------
ADD A BOOKING FOR A USER (must be logged in, data for user id 1)
------------------------------------
http://localhost:5030/users/bookings/book
POST 
{
    "Room":"10",
    "Checkin":"2026-01-01",
    "Checkout":"2026-01-25",
    "Price":"12000",
    "Message":"Wife is angry, please let me stay longer"
}
-------------------------------------

http://localhost:5030/hotels/search
POST
{
  "name": "Gra",
  "country": 1
}
---------------------------------------

{
  "country": 2,
  "min_price": 250
}
---------------------------------------

{
  "country": 2,
  "min_price": 250,
  "max_price": 330
}
---------------------------------------
{
  "city": "Berlin",
  "min_price": 250
}
---------------------------------------
{
"page": (Insert number)
}
---------------------------------------
{
  "page_size": 2
}
---------------------------------------
{
  "name": "Tower",
  "page": 2,
  "page_size": 8
}
---------------------------------------
{
  "name": "do",
  "page": 2,
  "page_size": 4
}
---------------------------------------

{
  "name": "hotel",
  "page": 1,
  "page_size": 30
}
---------------------------------------