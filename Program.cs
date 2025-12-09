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
    CREATE TABLE users
    (
              id INT PRIMARY KEY AUTO_INCREMENT,
              name VARCHAR(255),
              email VARCHAR(254) NOT NULL UNIQUE,
              password VARCHAR(128) 
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