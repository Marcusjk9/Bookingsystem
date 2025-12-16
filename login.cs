namespace server;

static class Login
{
    public static void Delete(HttpContext ctx)
    {
        if(ctx.Session.IsAvailable)
        {
            ctx.Session.Clear();
        }
    }

    public record Post_Args(string Email, string Password);
    public static async Task<bool> 
    Post(Post_Args credentials, Config config, HttpContext ctx)
    {
        bool result = false;
        string query = "SELECT id FROM users WHERE email = @email AND password = @password";
        var parameters = new MySqlParameter[]
        {
            new("@email", credentials.Email),
            new("@password", credentials.Password),
        };

        object query_result = await MySqlHelper.ExecuteScalarAsync(config.db, query, parameters);
        if(query_result is int id)
        {
            if(ctx.Session.IsAvailable)
            {
                ctx.Session.SetInt32("user_id", id);
                result = true;
            }
        }

        return result;
    }

    public static async Task<bool>   // admin login metod
    PostAdmin(Post_Args credentials, Config config, HttpContext ctx)
    {
        bool result = false;
        string query = "SELECT id, role FROM admins WHERE email = @email AND password = @password";
        var parameters = new MySqlParameter[]
        {
            new("@email", credentials.Email),
            new("@password", credentials.Password),
        };

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, parameters);
        if (await reader.ReadAsync())
        {
            int id = reader.GetInt32(0);
            string role = reader.GetString(1);

            if (ctx.Session.IsAvailable)
            {
                ctx.Session.SetInt32("admin_id", id);
                ctx.Session.SetString("admin_role", role);
                result = true;
            }
        }
        return result;
    }

    public static async Task<bool>   // employee login 
    PostEmployee(Post_Args credentials, Config config, HttpContext ctx)
    {
        bool result = false;
        string query = "SELECT id FROM employees WHERE email = @email AND password = @password";
        var parameters = new MySqlParameter[]
        {
            new("@email", credentials.Email),
            new("@password", credentials.Password),
        };

        object query_result = await MySqlHelper.ExecuteScalarAsync(config.db, query, parameters);
        if(query_result is int id)
        {
            if(ctx.Session.IsAvailable)
            {
                ctx.Session.SetInt32("employee_id", id);
                result = true;
            }
        }
        return result;
    }
}
