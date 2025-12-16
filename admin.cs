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
    int country,
    List<AdminPermission> Permissions
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

    public record AdminData(
            int Id,
            string Firstname,
            string Lastname,
            string Role,
            List<AdminPermission> Permissions
        );

    public static async Task<AdminData?> GetAdmin(int id, Config config)
        {
            string query = "SELECT id, firstname, lastname, role FROM admins WHERE id = @id";
            var param = new MySqlParameter("@id", id);
            
            using var reader = await MySqlHelper.ExecuteReaderAsync(config.db, query, param);
            if (await reader.ReadAsync())
            {
                string role = reader.GetString(3);
                var permissions = GetPermissionsFromRole(role);
                return new(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    role,
                    permissions
                );
            }
            return null;
        }
    public static bool HasPermission(List<AdminPermission> permissions, AdminPermission required)
    {
        return permissions.Contains(required);
    }

    private static List<AdminPermission> GetPermissionsFromRole(string role)
    {
        return role.ToLower() switch
        {
            "god" => new List<AdminPermission> 
            { 
                AdminPermission.ManagePermissions, AdminPermission.AddLocations,
                AdminPermission.CreatePersonnelAccounts, AdminPermission.ManageBookings,
                AdminPermission.ViewAllData, AdminPermission.ViewPermissionsList 
            },
            "mid" => new List<AdminPermission> 
            { 
                AdminPermission.AddLocations, AdminPermission.CreatePersonnelAccounts,
                AdminPermission.ManageBookings, AdminPermission.ViewPermissionsList 
            },
            "base" => new List<AdminPermission> 
            { 
                AdminPermission.ViewPermissionsList 
            },
            _ => new List<AdminPermission> { AdminPermission.None }
        };
    }

    public static async Task<bool> HasPermission(HttpContext ctx, Config config, AdminPermission required)
    {
        if (ctx.Session.GetInt32("admin_id") is not int adminId)
            return false;

        var adminData = await GetAdmin(adminId, config);
        return adminData != null && HasPermission(adminData.Permissions, required);
    }
    
}