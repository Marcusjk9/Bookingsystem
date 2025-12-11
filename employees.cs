namespace server;

static class Employees
{
    public record Post_Args(
        string Firstname,
        string Lastname,
        string Password,
        string Role,
        string Status,
        string Office,
        string Email,
        string Phone,
        string NationalIdNumber,
        string Street,
        string StreetNumber,
        int City,
        int Country
    );
    public record Get_Data(
        int Id,
        string Firstname,
        string Lastname,
        string Role,
        string Status,
        string Office,
        string Email
    );

    public static async Task Post(Post_Args emp, Config config)
    {
        // queary som skapar nytt employee person
        string query = """
            INSERT INTO employees
            (firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country)
            VALUES
            (@firstname, @lastname, @password, @role, @status, @office, @mail, @phone, @nid, @street, @streetnumber, @city, @country)
        """;

        var parameters = new MySqlParameter[]
        {
            new("@firstname", emp.Firstname),
            new("@lastname", emp.Lastname),
            new("@password", emp.Password),
            new("@role", emp.Role),
            new("@status", emp.Status),
            new("@office", emp.Office),
            new("@mail", emp.Email),
            new("@phone", emp.Phone),
            new("@nid", emp.NationalIdNumber),
            new("@street", emp.Street),
            new("@streetnumber", emp.StreetNumber),
            new("@city", emp.City),
            new("@country", emp.Country)
        };

        await MySqlHelper.ExecuteNonQueryAsync(config.db, query, parameters);
    }

    public static async Task<List<Get_Data>> Get(Config config)
    {
        List<Get_Data> result = new();

        string query = """
            SELECT id, firstname, lastname, role, status, office, email
            FROM employees
        """;

        using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query);

        while (reader.Read())
        {
            result.Add(new(
                reader.GetInt32(0),   // id
                reader.GetString(1),  // firstname
                reader.GetString(2),  // lastname
                reader.GetString(3),  // role
                reader.GetString(4),  // status
                reader.GetString(5),  // office
                reader.GetString(6)   // email
            ));
        }

        return result;
    }
}

/*  CREATE TABLE employees
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
  """;  */