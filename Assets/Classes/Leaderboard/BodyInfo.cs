using SQLite;

[Table("Bodies")]
public class BodyInfo
{
    [PrimaryKey, Column("id"), AutoIncrement]
    public int ID { get; set; }

    [Column("body_name")]
    public string Name { get; set; }

    [Column("run_id")]
    public int RunID { get; set; }

    [Column("body_level")]
    public int Level { get; set; }
}
