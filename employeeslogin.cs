/* namespace server;

static class EmployeesLogin
{
    public static void Delete(HttpContext ctx)
    {
        if(ctx.Session.IsAvailable)
        {
            ctx.Session.Clear();
        }
    }

    public record Post_Args(string Email, string Password);
    public static async Task<bool> Post(Post_Args credentials, Config config, HttpContext ctx)
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




/* namespace server;

static class EmployeesLogin
{
    // Vad vi tar emot från Postman vid POST /employees/login
    public record Post_Args(string Email, string Password);

    // POST /employees/login
    public static async Task<bool> Post(Post_Args credentials, Config config, HttpContext ctx)
    {
        bool result = false;

        // Leta efter employee med denna email + password
        string query = "SELECT id FROM employees WHERE email = @mail AND password = @pwd";

        var parameters = new MySqlParameter[]
        {
            new("@mail", credentials.Email),
            new("@pwd",  credentials.Password)
        };

        object? query_result = await MySqlHelper.ExecuteScalarAsync(config.db, query, parameters);

        if (query_result is int id)
        {
            // Sätt employee_id i sessionen → nu är employee "inloggad"
            if (ctx.Session.IsAvailable)
            {
                ctx.Session.SetInt32("employee_id", id);
            }
            result = true;
        }

        return result;
    }
} */