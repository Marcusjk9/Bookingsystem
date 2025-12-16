namespace server;


public class hotel_search_params
{
  public string? name { get; set; }
  public string? city { get; set; }
  public int? country { get; set; }
  public int? min_price { get; set; }
  public int? max_price { get; set; }
  public bool convenience { get; set; }
  public int? page { get; set; } = 1;
  public int? page_size { get; set; } = 10;
  public List<convenience_type>? conveniences { get; set; }
  public List<activity_type>? activities { get; set; }
}



