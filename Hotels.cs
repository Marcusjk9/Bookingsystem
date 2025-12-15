namespace server;


public class Hotel
{
  public int id { get; set; }
  public string name { get; set; } = "";
  public string description { get; set; } = "";
  public string street { get; set; } = "";
  public string streetNumber { get; set; } = "";
  public int? city { get; set; }
  public int? country { get; set; }
  public string frontdesk_open { get; set; } = "";
  public string frontdesk_close { get; set; } = "";
  public string check_in { get; set; } = "";
  public string check_out { get; set; } = "";
  public int distance_to_city_center { get; set; }
}















