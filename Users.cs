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
      int city,
      int country,
      string status
      );


  public static async Task Post(post_Args usr, Config config)
  {
    string query = """
        INSERT INTO users
        (id, firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, city, country, status)
        VALUES
        (@id, @firstname, @lastname, @password, @email, @phone, @nationalidnumber, @street, @streetnumber, @city, @country, @status)
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
            SELECT id, firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, city, country, status
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
              reader.GetInt32(9),     // city
              reader.GetInt32(10),    // country
              reader.GetString(11)    // status


          );
        }
      }
    }


    return result;
  }
}



//Kontrollerar om user_id finns i session cookie. If exists, returnera full name, email och phonenumber. 
static class Profile
{
  public record Get_Data(string? Name, string Email, String Phone);
  public static async Task<Get_Data?> Post(Config config, HttpContext ctx)
  {
    Get_Data? result = null;

    if (ctx.Session.IsAvailable)
    {
      if (ctx.Session.Keys.Contains("user_id"))
      {
        string query = "SELECT CONCAT(u.firstname,\" \", u.lastname), u.email, u.phone FROM users AS u WHERE u.id = @id";
        var parameters = new MySqlParameter[]
        {
                    new("@id", ctx.Session.GetInt32("user_id"))
        };

        using (var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters))
        {
          if (reader.Read())
          {
            if (reader[0] is string name)
            {
              result = new(name, reader.GetString(1), reader.GetString(2));
            }
          }
        }
      }
    }
    return result;
  }
}

static class Booking
{
  public record Get_Data(int? UserId, int? BookingID, string? Name, string? Email, int? Room, DateOnly? CheckIn, DateOnly? CheckOut, decimal? Price, string? Currency, string? Status, DateOnly? Created, DateTime? Confirmed_Cancelled, string? YourMessage);
  public static async Task<List<Get_Data?>> Post(Config config, HttpContext ctx)
  {

    var bookings = new List<Get_Data?>();

    if (ctx.Session.IsAvailable)
    {
      if (ctx.Session.Keys.Contains("user_id"))
      {
        string query = """
        SELECT CONCAT(u.firstname,"" "", u.lastname) as fullname, u.id AS user_id, u.email, b.id AS booking_id, b.room, b.checkin, b.checkout, b.price, b.currency, b.status, b.createddate, b.statuschange, b.message 
        FROM users AS u 
        INNER JOIN bookings AS b on b.user = u.id
        WHERE u.id = @id
        """;

        var parameters = new MySqlParameter[]
        {
                    new("@id", ctx.Session.GetInt32("user_id"))
        };

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters);
        {



          while (reader.Read())
          {
            bookings.Add(new Get_Data(

              UserId: reader.GetInt32("user_id"),
              BookingID: reader.GetInt32("booking_id"),
              Name: reader.GetString("fullname"),
              Email: reader.GetString("email"),
              Room: reader.GetInt32("room"),
              CheckIn: DateOnly.FromDateTime(reader.GetDateTime("checkin")),
              CheckOut: DateOnly.FromDateTime(reader.GetDateTime("checkout")),
              Price: Convert.ToDecimal(reader.GetInt32("price")),
              Currency: reader.GetString("currency"),
              Status: reader.GetString("status"),
              Created: DateOnly.FromDateTime(reader.GetDateTime("createddate")),
              reader.IsDBNull(reader.GetOrdinal("statuschange"))  // Check if NULL, GetOrdinal converts column name string to int which is the only var type IsDbNull accepts
              ? null                                           // If NULL, use null
              : reader.GetDateTime(reader.GetOrdinal("statuschange")),
              YourMessage: reader.GetString("message")
            ));
          }
          return bookings;
        }

      }
      else { return bookings; }
    }
    else { return bookings; }
  }
}