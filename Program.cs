global using MySql.Data.MySqlClient;
using server;

var builder = WebApplication.CreateBuilder(args);

Config config = new("server=127.0.0.1;uid=travel;pwd=travel;database=travel;");
builder.Services.AddSingleton(config);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapDelete("/db", db_reset_to_default);

app.Run();

async Task db_reset_to_default(Config config)
{
  string query_create_users_table = """
    CREATE TABLE countries
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              name VARCHAR(255) 
    );

    CREATE TABLE cities
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              country INTEGER,
              name VARCHAR(255),
              FOREIGN KEY (country) REFERENCES countries(id)
    );

    CREATE TABLE users
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              firstname VARCHAR(50) NOT NULL, 
              lastname VARCHAR(100) NOT NULL,
              password VARCHAR(128),
              email VARCHAR(254) UNIQUE NOT NULL,
              phone VARCHAR(50),
              nationalidnumber VARCHAR(16) UNIQUE NOT NULL,
              street VARCHAR(50),
              streetnumber VARCHAR(16),
              city INTEGER,
              country INTEGER,
              status ENUM('active', 'inactive', 'pending') DEFAULT 'active',
              FOREIGN KEY (city) REFERENCES cities(id),
              FOREIGN KEY (country) REFERENCES countries(id)
              
    );

     CREATE TABLE admins
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              firstname VARCHAR(50) NOT NULL, 
              lastname VARCHAR(100) NOT NULL,
              password VARCHAR(128),
              role ENUM('god', 'mid', 'base'),
              status ENUM('active', 'inactive', 'pending') DEFAULT 'active',
              office ENUM('halmstad', 'stockholm', 'berlin', 'london') DEFAULT 'halmstad',
              email VARCHAR(254) UNIQUE NOT NULL,
              phone VARCHAR(50),
              nationalidnumber VARCHAR(16) UNIQUE NOT NULL,
              street VARCHAR(50),
              streetnumber VARCHAR(16),
              city INTEGER,
              country INTEGER,
              FOREIGN KEY (city) REFERENCES cities(id),
              FOREIGN KEY (country) REFERENCES countries(id)
              
    );

    CREATE TABLE employees
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              firstname VARCHAR(50) NOT NULL, 
              lastname VARCHAR(100) NOT NULL,
              password VARCHAR(128),
              role ENUM('support', 'manager', 'administrator', 'tech'),
              status ENUM('active', 'inactive', 'pending') DEFAULT 'active',
              office ENUM('halmstad', 'stockholm', 'berlin', 'london') DEFAULT 'halmstad',
              email VARCHAR(254) UNIQUE NOT NULL,
              phone VARCHAR(50),
              nationalidnumber VARCHAR(16) UNIQUE NOT NULL,
              street VARCHAR(50),
              streetnumber VARCHAR(16),
              city INTEGER,
              country INTEGER,
              FOREIGN KEY (city) REFERENCES cities(id),
              FOREIGN KEY (country) REFERENCES countries(id)
              
    )
  """;

  await MySqlHelper.ExecuteNonQueryAsync(config.db, "DROP TABLE IF EXISTS users");
  await MySqlHelper.ExecuteNonQueryAsync(config.db, query_create_users_table);
}

/*
RUN IN WORKBENCH TO CREATE DATABASE AND USER FOR ACCESS TO DATABASE
-------------------
CREATE DATABASE;

USE travel;

DROP USER IF EXISTS 'travel'@'localhost';
FLUSH PRIVILEGES;
CREATE USER travel@localhost identified by 'travel';
GRANT ALL PRIVILEGES ON travel.* TO 'travel'@'localhost';
--------------------
*/