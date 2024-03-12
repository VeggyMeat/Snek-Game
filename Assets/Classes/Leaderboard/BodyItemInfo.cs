using SQLite;

public class BodyItemInfo
{
    [Column("body_name")]
    public string BodyName { get; set; }

    [Column("body_level")]
    public int BodyLevel { get; set; }

    [Column("item_name")]
    public string ItemName { get; set; }

    [Column("item_level")]
    public int ItemLevel { get; set; }
}
