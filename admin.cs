namespace server;
public record post_args


(
    int id,
    string firstname,
    string lastname,
    string password,
    string role,
    string status,
    string office,
    string email,
    string phone,
    string nationalidnumber,
    string street,
    string streetnumber,
    int city,
    int country
);
static class Admin
{
   
     public static async Task Post(post_args adm, Config config)
    {
        string query = @"
        INSERT INTO admins
        (
         firstname,lastname,password,role,status,office,email,phone,nationalidnumber,street,city,country
        )
        VALUES 
        (
        @firstname,@lastname,@password,@role,@status,@office,@email,@phone,@nationalidnumber,@street,@city,@country
        )
      ";
      var parameters = new MySqlParameter[]
      {
          new("@firstname", adm.firstname),
          new("@lastname", adm.lastname),
          new("@password", adm.password),
          new("@role", adm.role),
          new("@status",adm.status),
          new("@office",adm.office),
          new("@email",adm.email),
          new("@phone",adm.phone ?? (object)DBNull.Value),
          new("@nationalidnumber",adm.nationalidnumber),
          new("@street",adm.street ?? (object)DBNull.Value),
          new("@streetnumber",adm.streetnumber ?? (object)DBNull.Value),
          new("@city",adm.city),
          new("@country",adm.country)
      };
    await MySqlHelper.ExecuteNonQueryAsync(config.db, query,parameters);
     
}

}