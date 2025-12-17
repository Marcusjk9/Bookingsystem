namespace server;

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
        string query = "SELECT CONCAT(firstname,\" \", lastname), email, phone FROM users WHERE id = @id";
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
