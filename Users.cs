using Microsoft.VisualBasic;
using Org.BouncyCastle.Tls;


namespace server;




static class Users
{


  public record post_Args(
  int id,
  string firstname,
  string lastname,
  string password,
  string email,
  string? phone,
  string nationalidnumber,
  string street,
  string streetnumber,
  string region,
  int city,
  int country,
  string status
  );


  public record Get_Data(
      int id,
      string firstname,
      string lastname,
      string password,
      string email,
      string? phone,
      string nationalidnumber,
      string street,
      string streetnumber,
      string region,
      int city,
      int country,
      string status
      );


  public static async Task Post(post_Args usr, Config config)
  {
    string query = """
        INSERT INTO users
        (id, firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, region, city, country, status)
        VALUES
        (@id, @firstname, @lastname, @password, @email, @phone, @nationalidnumber, @street, @streetnumber, @region, @city, @country, @status)
        """;


    var parameters = new MySqlParameter[]
    {
    new("@firstname", usr.firstname),
    new("@lastname", usr.lastname),
    new("@password", usr.password),
    new("@email", usr.email),
    new("@phone", usr.phone),
    new("@nationalidnumber", usr.nationalidnumber),
    new("@street", usr.street),
    new("@streetnumber", usr.streetnumber),
    new("@region", usr.region),
    new("@city", usr.city),
    new("@country", usr.country),
    new("@status", usr.status)
    };


    await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
  }




  public static async Task<Get_Data?>
  Get(Config config, HttpContext ctx)
  {
    Get_Data? result = null;




    if (ctx.Session.IsAvailable && ctx.Session.GetInt32("user_id") is int user_id)
    {
      string query = @"
            SELECT id, firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, region, city, country, status
            FROM users
            WHERE id = @id";


      var parameters = new MySqlParameter[]
      {
                new("@id", user_id)
      };




      using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
      {
        if (reader.Read())
        {
          result = new Get_Data(
              reader.GetInt32(0),     // id
              reader.GetString(1),    // firstname
              reader.GetString(2),    // lastname
              reader.GetString(3),    // password
              reader.GetString(4),    // email
              reader.GetString(5),    // phone
              reader.GetString(6),    // nationalidnumber
              reader.GetString(7),    // street
              reader.GetString(8),    // streetnumber
              reader.GetString(9),    // region
              reader.GetInt32(10),    // city
              reader.GetInt32(11),    // country
              reader.GetString(12)    // status


          );
        }
      }
    }


    return result;
  }
}








/*
 CREATE TABLE users
    (
????????????  id INTEGER PRIMARY KEY AUTO_INCREMENT,
              firstname CHAR(50) NOT NULL,
              lastname CHAR(100) NOT NULL,
              password VARCHAR(128),
              Email CHAR(254) UNIQUE NOT NULL,
              phone VARCHAR(50),
              nationalidnumber VARCHAR(16) UNIQUE NOT NULL,
              street VARCHAR(50),
              streetnumber VARCHAR(16),
              region CHAR(50),
              city INTEGER,
              country INTEGER,
              status ENUM('active', 'inactive', 'pending') DEFAULT 'active',
              FOREIGN KEY (city) REFERENCES cities(id),
              FOREIGN KEY (country) REFERENCES countries(id)
              */













