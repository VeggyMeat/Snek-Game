using SQLite;

[Table("Items")]
public class ItemInfo
{
    [PrimaryKey, Column("id"), AutoIncrement]
    public int ID { get; set; }

    [Column("item_name")]
    public string Name { get; set; }

    [Column("run_id")]
    public int RunID { get; set; }

    [Column("item_level")]
    public int Level { get; set; }
}
