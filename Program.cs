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
              
    );
    
    CREATE TABLE hotels
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              name VARCHAR(50) NOT NULL, 
              description TEXT,
              street VARCHAR(50),
              streetnumber VARCHAR(16),
              city INTEGER,
              country INTEGER,
              frontdeskopen TIME DEFAULT '06:00',
              frontdeskclose TIME DEFAULT '23:00',
              checkin TIME default '11:00',
              checkout TIME default '08:00',
              distancetocitycenter MEDIUMINT,
              FOREIGN KEY (city) REFERENCES cities(id),
              FOREIGN KEY (country) REFERENCES countries(id)
    );
    
    CREATE TABLE convenience
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              hotel INTEGER,
              type ENUM('wifi','pool','parking','breakfast', 'pet', 'spa') NOT NULL,
              FOREIGN KEY (hotel) REFERENCES hotels(id)
    );
    
    CREATE TABLE rooms
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              roomnumber SMALLINT,
              size SMALLINT,
              hotel INTEGER,
              type ENUM('standard','deluxe','suite') DEFAULT 'standard',
              singlebed TINYINT,
              doublebed TINYINT,
              maxoccupancy TINYINT,
              priceperday INT,
              currency ENUM('eur','sek','usd') DEFAULT 'eur',
              FOREIGN KEY (hotel) REFERENCES hotels(id)
    );
    
    CREATE TABLE activities
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              type ENUM('f1','soccer','climbing') NOT NULL
    );
    
    CREATE TABLE activitiesdestinations
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              activity INTEGER,
              hotel INTEGER,
              activityprice INT,
              currency ENUM('eur','sek','usd') DEFAULT 'eur',
              FOREIGN KEY (activity) REFERENCES activities(id),
              FOREIGN KEY (hotel) REFERENCES hotels(id)
    );

    CREATE TABLE bookings
    (
              id INTEGER PRIMARY KEY AUTO_INCREMENT,
              user INTEGER,
              room INTEGER,
              checkin DATE,
              checkout DATE,
              price INT,
              currency ENUM('eur','sek','usd') default 'eur',
              status ENUM('pending','confirmed','cancelled') default 'pending',
              createddate DATETIME DEFAULT NOW(),
              statuschange DATETIME ON UPDATE CURRENT_TIMESTAMP,
              message TEXT,
              FOREIGN KEY (user) REFERENCES users(id),
              FOREIGN KEY (room) REFERENCES rooms(id)
    );

    CREATE TABLE cotravellers
    (
              bookingid INTEGER,
              nationalidnumber VARCHAR(16) UNIQUE NOT NULL,
              firstname VARCHAR(50) NOT NULL, 
              lastname VARCHAR(100) NOT NULL,
              email VARCHAR(254) UNIQUE NOT NULL,
              phone VARCHAR(50),
              room INTEGER,
              checkin DATE,
              checkout DATE,
              price INT,
              currency ENUM('eur','sek','usd') default 'eur',
              FOREIGN KEY (bookingid) REFERENCES bookings(id),
              FOREIGN KEY (room) REFERENCES rooms(id),
              PRIMARY KEY (bookingid, nationalidnumber)
    )
  """;

  await MySqlHelper.ExecuteNonQueryAsync(config.db, """
  DROP TABLE IF EXISTS admins;
  DROP TABLE IF EXISTS employees;
  DROP TABLE IF EXISTS cotravellers;
  DROP TABLE IF EXISTS bookings;
  DROP TABLE IF EXISTS users;
  DROP TABLE IF EXISTS rooms;
  DROP TABLE IF EXISTS activitiesdestinations;
  DROP TABLE IF EXISTS activities;
  DROP TABLE IF EXISTS convenience;
  DROP TABLE IF EXISTS hotels;
  DROP TABLE IF EXISTS cities;
  DROP TABLE IF EXISTS countries;
  """);
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