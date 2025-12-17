global using MySql.Data.MySqlClient;
using server;




var builder = WebApplication.CreateBuilder(args);




Config config = new("server=127.0.0.1;uid=travel;pwd=travel;database=travel;");




builder.Services.AddSingleton(config);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    {
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
    });
var app = builder.Build();




app.UseSession();




app.MapGet("/", () => "Hello World!");
app.MapDelete("/db", db_reset_to_default);
app.MapPut("/db", populate_test_data);
app.MapPost("/login", Login.Post);
app.MapDelete("/login", Login.Delete);
app.MapPost("/profile", Profile.Post);
app.MapGet("/employees", Employees.Get);
app.MapPost("/employees", Employees.Post);
// app.MapPost("/employees/login", EmployeesLogin.Post);
// app.MapDelete("/employees/login", EmployeesLogin.Delete);
app.MapPost("/login/employee", Login.PostEmployee);
app.MapDelete("/login/employee", Login.Delete);
app.MapGet("/users", Users.Get);
app.MapPost("/users", Users.Post);
app.MapPost("/admin", Admin.Post);
app.MapPost("/login/admin", Login.PostAdmin);


app.MapGet("/admin/users", async (Config config, HttpContext ctx) =>


{
  if (!await Admin.HasPermission(ctx, config, AdminPermission.ViewAllData))
    return Results.Unauthorized();

  List<dynamic> users = new();
  using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, "SELECT id, firstname, lastname, email FROM users");
  while (await reader.ReadAsync())
  {
    users.Add(new
    {
      id = reader.GetInt32(0),
      firstname = reader.GetString(1),
      lastname = reader.GetString(2),
      email = reader.GetString(3)
    });
  }

  return Results.Ok(users);
});
app.MapPost("/hotels/search", async (Config config, hotel_search_params p) =>
{
  //The base of the sql query to use for filter/search
  var sql = """
    SELECT
      h.id, h.name, h.description, c.name AS city, co.name AS country, h.distancetocitycenter, r.id AS room_id, r.priceperday AS price_per_day
    FROM hotels h
    JOIN cities c ON h.city = c.id
    JOIN countries co ON h.country = co.id
    JOIN rooms r ON r.hotel = h.id
      WHERE 1=1
    """;
  //WHERE 1=1 means "true", and is written so that we dont  need to write a bool in our code


  var parameters = new List<MySqlParameter>();  //List holding SQL parameters




  if (!string.IsNullOrEmpty(p.name))    //Filters for names. IsNullOrEmpty is to avoid matching everything with LIKE
  {
    sql += " AND h.name LIKE @name";
    parameters.Add(new MySqlParameter("@name", $"%{p.name}%"));
  }


  if (!string.IsNullOrEmpty(p.city))    //Filters for cities
  {
    sql += " AND c.name = @city";
    parameters.Add(new MySqlParameter("@city", p.city));
  }


  if (p.country.HasValue)   //Filters for country but scountries are int in our program therefore the diffrence
  {
    sql += " AND h.country = @country";
    parameters.Add(new MySqlParameter("@country", p.country.Value));
  }




  if (p.min_price.HasValue)   //Filters by minimum price
  {
    sql += " AND r.priceperday >= @min_price";
    parameters.Add(new MySqlParameter("@min_price", p.min_price.Value));
  }




  if (p.max_price.HasValue)    //Filters by maximum price
  {
    sql += " AND r.priceperday <= @max_price";
    parameters.Add(new MySqlParameter("@max_price", p.max_price.Value));
  }



  if (p.conveniences != null && p.conveniences.Count > 0)   // make sure the hotel has convieniences. And the exist makes sure the hotel has all od the convieneces asked for
  {
    for (int i = 0; i < p.conveniences.Count; i++)
    {
      sql += $"""
      AND EXISTS (SELECT 1 FROM convenience cv WHERE cv.hotel = h.id AND cv.type = @conv{i})
    """;
      parameters.Add(new MySqlParameter($"@conv{i}", p.conveniences[i].ToString()));
    }
  }


  if (p.activities != null)   // same as the convinence code
  {
    for (int i = 0; i < p.activities.Count; i++)
    {
      sql += $"""
        AND EXISTS (SELECT 1 FROM activitiesdestinations ad JOIN activities a ON ad.activity = a.id
            WHERE ad.hotel = h.id AND a.type = @act{i})
    """;
      parameters.Add(new MySqlParameter($"@act{i}", p.activities[i].ToString()));
    }
  }




  sql += " LIMIT @offset, @pagesize";   //LIMIT to skip so you start on page 1
  parameters.Add(new MySqlParameter("@offset", ((p.page ?? 1) - 1) * (p.page_size ?? 10))); // Offset is a rowskip
  parameters.Add(new MySqlParameter("@pagesize", p.page_size ?? 10)); //limit the page to 10 rows




  var results = new List<object>();
  using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, sql, parameters.ToArray());
  while (reader.Read())
  {
    results.Add(new
    {
      id = reader.GetInt32("id"),
      name = reader.GetString("name"),
      room_id = reader.GetInt32("room_id"),
      price_per_day = reader.GetDecimal("price_per_day"),
      description = reader.GetString("description"),
      city = reader.GetString("city"),
      country = reader.GetString("country"),
      distance_to_city_center = reader.GetInt32("distancetocitycenter")
    });
  }


  return Results.Ok(results);
});


app.Run();


async Task db_reset_to_default(Config config)
{
  string query_create_tables = """


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
  await MySqlHelper.ExecuteNonQueryAsync(config.db, query_create_tables);
}


async Task populate_test_data(Config config)
{
  string db_populate = """


  USE travel;


  INSERT INTO countries (name) VALUES
              ('Sweden'),
              ('Germany'),
              ('United Kingdom'),
              ('France');


             
  INSERT INTO cities (country, name) VALUES
              -- Sweden (country_id = 1)
              (1, 'Stockholm'),
              (1, 'Halmstad'),
              (1, 'Gothenburg'),
              (1, 'Malmö'),
              -- Germany (country_id = 2)
              (2, 'Berlin'),
              (2, 'Munich'),
              (2, 'Hamburg'),
              -- United Kingdom (country_id = 3)
              (3, 'London'),
              (3, 'Manchester'),
              (3, 'Edinburgh'),
              -- France (country_id = 4)
              (4, 'Paris'),
              (4, 'Lyon'),
              (4, 'Marseille');




  INSERT INTO users (firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, city, country, status) VALUES
              ('Test', 'User', 'hashed_password_123', 'test.user@example.com', '+46701234567', '19900101-1234', 'Testgatan', '1', 1, 1, 'active'),
              ('John', 'Doe', 'hashed_password_456', 'john.doe@example.com', '+46702345678', '19850615-5678', 'Storgatan', '42', 2, 1, 'active'),
              ('Jane', 'Smith', 'hashed_password_789', 'jane.smith@example.com', '+44207123456', '19920320-9012', 'Oxford Street', '100', 8, 3, 'active'),
              ('Max', 'Mustermann', 'hashed_password_abc', 'max.mustermann@example.com', '+4930123456', '19880705-3456', 'Berliner Straße', '25', 5, 2, 'pending'),
              ('Alice', 'Testson', 'hashed_password_def', 'alice.test@example.com', '+33142345678', '19950812-7890', 'Rue de Test', '15', 11, 4, 'active'),
              ('Bob', 'Demo', 'hashed_password_ghi', 'bob.demo@example.com', '+46703456789', '19870430-2345', 'Demogatan', '7', 3, 1, 'inactive');




  INSERT INTO admins (firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country) VALUES
              ('Super', 'Admin', 'admin_password_001', 'god', 'active', 'stockholm', 'super.admin@company.com', '+46708111111', '19800101-0001', 'Adminvägen', '1', 1, 1),
              ('Chief', 'Administrator', 'admin_password_002', 'god', 'active', 'halmstad', 'chief.admin@company.com', '+46708222222', '19810202-0002', 'Chefsgatan', '2', 2, 1),
              ('Mid', 'Manager', 'admin_password_003', 'mid', 'active', 'stockholm', 'mid.manager@company.com', '+46708333333', '19820303-0003', 'Mellangatan', '3', 1, 1),
              ('Middle', 'Boss', 'admin_password_004', 'mid', 'active', 'halmstad', 'middle.boss@company.com', '+46708444444', '19830404-0004', 'Mittenvägen', '4', 2, 1),
              ('Basic', 'Admin', 'admin_password_005', 'base', 'active', 'stockholm', 'basic.admin@company.com', '+46708555555', '19840505-0005', 'Basvägen', '5', 1, 1),
              ('Entry', 'Level', 'admin_password_006', 'base', 'pending', 'halmstad', 'entry.level@company.com', '+46708666666', '19850606-0006', 'Nybörjargatan', '6', 2, 1);


  INSERT INTO employees (firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country) VALUES
              ('Support', 'Helpdesk', 'emp_password_001', 'support', 'active', 'stockholm', 'support.help@company.com', '+46709111111', '19860101-1001', 'Supportgatan', '10', 1, 1),
              ('Customer', 'Service', 'emp_password_002', 'support', 'active', 'halmstad', 'customer.service@company.com', '+46709222222', '19870202-1002', 'Servicegatan', '11', 2, 1),
              ('Team', 'Manager', 'emp_password_003', 'manager', 'active', 'stockholm', 'team.manager@company.com', '+46709333333', '19880303-1003', 'Chefsgatan', '12', 1, 1),
              ('Project', 'Lead', 'emp_password_004', 'manager', 'active', 'halmstad', 'project.lead@company.com', '+46709444444', '19890404-1004', 'Ledargatan', '13', 2, 1),
              ('System', 'Administrator', 'emp_password_005', 'administrator', 'active', 'stockholm', 'system.admin@company.com', '+46709555555', '19900505-1005', 'Systemvägen', '14', 1, 1),
              ('Network', 'Admin', 'emp_password_006', 'administrator', 'active', 'halmstad', 'network.admin@company.com', '+46709666666', '19910606-1006', 'Nätverksgatan', '15', 2, 1),
              ('Tech', 'Support', 'emp_password_007', 'tech', 'active', 'stockholm', 'tech.support@company.com', '+46709777777', '19920707-1007', 'Teknikvägen', '16', 1, 1),
              ('IT', 'Specialist', 'emp_password_008', 'tech', 'active', 'halmstad', 'it.specialist@company.com', '+46709888888', '19930808-1008', 'IT-gatan', '17', 2, 1);


  INSERT INTO hotels (name, description, street, streetnumber, city, country, frontdeskopen, frontdeskclose, checkin, checkout, distancetocitycenter) VALUES
              ('Grand Hotel Stockholm', 'Luxury waterfront hotel in the heart of Stockholm', 'Södra Blasieholmshamnen', '8', 1, 1, '06:00', '23:00', '15:00', '11:00', 500),
              ('Seaside Resort Halmstad', 'Beautiful beachfront resort with ocean views', 'Strandvägen', '1', 2, 1, '07:00', '22:00', '14:00', '10:00', 3000),
              ('Berlin Central Hotel', 'Modern hotel near Brandenburg Gate', 'Unter den Linden', '45', 5, 2, '00:00', '23:59', '15:00', '11:00', 1000),
              ('London Tower Hotel', 'Historic hotel with views of Tower Bridge', 'Tower Bridge Road', '100', 8, 3, '06:00', '23:00', '14:00', '10:00', 2000),
              ('Paris Eiffel Hotel', 'Charming boutique hotel near Eiffel Tower', 'Avenue de la Bourdonnais', '30', 11, 4, '07:00', '22:00', '16:00', '11:00', 1500);


  INSERT INTO convenience (hotel, type) VALUES
              -- Grand Hotel Stockholm (hotel_id = 1)
              (1, 'wifi'),
              (1, 'breakfast'),
              (1, 'spa'),
              (1, 'parking'),
              -- Seaside Resort Halmstad (hotel_id = 2)
              (2, 'wifi'),
              (2, 'pool'),
              (2, 'breakfast'),
              (2, 'pet'),
              (2, 'parking'),
              -- Berlin Central Hotel (hotel_id = 3)
              (3, 'wifi'),
              (3, 'breakfast'),
              (3, 'parking'),
              -- London Tower Hotel (hotel_id = 4)
              (4, 'wifi'),
              (4, 'pool'),
              (4, 'breakfast'),
              (4, 'spa'),
              -- Paris Eiffel Hotel (hotel_id = 5)
              (5, 'wifi'),
              (5, 'breakfast'),
              (5, 'pet');


  INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
              (101, 25, 1, 'standard', 2, 0, 2, 120, 'eur'),
              (102, 30, 1, 'standard', 0, 1, 2, 130, 'eur'),
              (103, 28, 1, 'standard', 2, 0, 2, 120, 'eur'),
              (201, 35, 1, 'deluxe', 0, 1, 2, 180, 'eur'),
              (202, 40, 1, 'deluxe', 1, 1, 3, 200, 'eur'),
              (203, 38, 1, 'deluxe', 0, 1, 2, 190, 'eur'),
              (301, 50, 1, 'suite', 0, 2, 4, 350, 'eur'),
              (302, 55, 1, 'suite', 1, 2, 5, 400, 'eur'),
              (303, 60, 1, 'suite', 0, 2, 4, 380, 'eur'),
              (304, 45, 1, 'deluxe', 0, 1, 2, 210, 'eur');


  INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
              (101, 30, 2, 'standard', 2, 0, 2, 90, 'eur'),
              (102, 32, 2, 'standard', 0, 1, 2, 95, 'eur'),
              (103, 30, 2, 'standard', 2, 0, 2, 90, 'eur'),
              (104, 35, 2, 'deluxe', 0, 1, 2, 140, 'eur'),
              (201, 40, 2, 'deluxe', 0, 1, 2, 150, 'eur'),
              (202, 42, 2, 'deluxe', 1, 1, 3, 170, 'eur'),
              (203, 40, 2, 'deluxe', 0, 1, 2, 150, 'eur'),
              (301, 60, 2, 'suite', 0, 2, 4, 280, 'eur'),
              (302, 65, 2, 'suite', 1, 2, 5, 320, 'eur'),
              (303, 55, 2, 'suite', 0, 2, 4, 300, 'eur');


    INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
              (101, 22, 3, 'standard', 1, 0, 1, 100, 'eur'),
              (102, 28, 3, 'standard', 0, 1, 2, 120, 'eur'),
              (103, 28, 3, 'standard', 0, 1, 2, 120, 'eur'),
              (201, 32, 3, 'deluxe', 0, 1, 2, 160, 'eur'),
              (202, 35, 3, 'deluxe', 1, 1, 3, 180, 'eur'),
              (203, 32, 3, 'deluxe', 0, 1, 2, 160, 'eur'),
              (301, 45, 3, 'suite', 0, 2, 4, 300, 'eur'),
              (302, 50, 3, 'suite', 1, 2, 5, 350, 'eur'),
              (303, 48, 3, 'suite', 0, 2, 4, 320, 'eur'),
              (304, 40, 3, 'deluxe', 0, 1, 2, 170, 'eur');


    INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
              (101, 26, 4, 'standard', 2, 0, 2, 140, 'eur'),
              (102, 30, 4, 'standard', 0, 1, 2, 150, 'eur'),
              (103, 28, 4, 'standard', 2, 0, 2, 140, 'eur'),
              (201, 38, 4, 'deluxe', 0, 1, 2, 200, 'eur'),
              (202, 40, 4, 'deluxe', 1, 1, 3, 220, 'eur'),
              (203, 38, 4, 'deluxe', 0, 1, 2, 200, 'eur'),
              (301, 55, 4, 'suite', 0, 2, 4, 400, 'eur'),
              (302, 60, 4, 'suite', 1, 2, 5, 450, 'eur'),
              (303, 58, 4, 'suite', 0, 2, 4, 420, 'eur'),
              (304, 42, 4, 'deluxe', 0, 1, 2, 210, 'eur');


    INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
              (101, 24, 5, 'standard', 1, 0, 1, 110, 'eur'),
              (102, 28, 5, 'standard', 0, 1, 2, 130, 'eur'),
              (103, 28, 5, 'standard', 0, 1, 2, 130, 'eur'),
              (201, 35, 5, 'deluxe', 0, 1, 2, 180, 'eur'),
              (202, 38, 5, 'deluxe', 1, 1, 3, 200, 'eur'),
              (203, 35, 5, 'deluxe', 0, 1, 2, 180, 'eur'),
              (301, 50, 5, 'suite', 0, 2, 4, 350, 'eur'),
              (302, 55, 5, 'suite', 1, 2, 5, 400, 'eur'),
              (303, 52, 5, 'suite', 0, 2, 4, 370, 'eur'),
              (304, 40, 5, 'deluxe', 0, 1, 2, 190, 'eur');


    INSERT INTO activities (type) VALUES
              ('f1'),
              ('soccer'),
              ('climbing');


    INSERT INTO activitiesdestinations (activity, hotel, activityprice, currency) VALUES
              -- Grand Hotel Stockholm
              (2, 1, 150, 'eur'), -- soccer
              (3, 1, 250, 'eur'), -- climbing
              -- Seaside Resort Halmstad
              (2, 2, 120, 'eur'), -- soccer
              (3, 2, 180, 'eur'), -- climbing
              -- Berlin Central Hotel
              (1, 3, 800, 'eur'), -- F1
              (2, 3, 100, 'eur'), -- soccer
              -- London Tower Hotel
              (1, 4, 1000, 'eur'), -- F1
              (3, 4, 200, 'eur'), -- climbing
              -- Paris Eiffel Hotel
              (1, 5, 950, 'eur'), -- F1
              (2, 5, 130, 'eur'), -- soccer
              (3, 5, 220, 'eur'); -- climbing


    INSERT INTO bookings (user, room, checkin, checkout, price, currency, status, message) VALUES
              (1, 1, '2025-01-15', '2025-01-18', 360, 'eur', 'confirmed', 'Looking forward to my stay!'),
              (2, 11, '2025-02-10', '2025-02-14', 380, 'eur', 'confirmed', 'Beach vacation'),
              (3, 31, '2025-03-05', '2025-03-08', 420, 'eur', 'pending', 'Business trip to London'),
              (4, 21, '2025-04-20', '2025-04-23', 480, 'eur', 'confirmed', NULL),
              (5, 41, '2025-05-12', '2025-05-15', 390, 'eur', 'confirmed', 'Anniversary trip'),
              (6, 5, '2025-06-01', '2025-06-03', 360, 'eur', 'cancelled', 'Plans changed');


    INSERT INTO cotravellers (bookingid, nationalidnumber, firstname, lastname, email, phone, room, checkin, checkout, price, currency) VALUES
              (1, '19920505-5555', 'Sarah', 'Testson', 'sarah.test@example.com', '+46704567890', 2, '2025-01-15', '2025-01-18', 390, 'eur'),
              (2, '19880915-6666', 'Emma', 'Doe', 'emma.doe@example.com', '+46705678901', 12, '2025-02-10', '2025-02-14', 380, 'eur'),
              (5, '19931120-7777', 'Pierre', 'Dupont', 'pierre.dupont@example.com', '+33143456789', 42, '2025-05-12', '2025-05-15', 390, 'eur');


""";
  await MySqlHelper.ExecuteNonQueryAsync(config.db, db_populate);
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
