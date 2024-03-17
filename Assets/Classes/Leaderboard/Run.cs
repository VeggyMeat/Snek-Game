using SQLite;
using System;

// COMPLETE

[Table("Runs")]
public class Run
{
    [PrimaryKey, Column("id")]
    public int ID { get; set; }

    [Column("player_name")]
    public string PlayerName { get; set; }

    [Column("score")]
    public int Score { get; set; }

    [Column("time")]
    public int Time { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }
}
