using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Table("Runs")]
public class Run
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }

    [Column("player_name")]
    public string PlayerName { get; set; }

    [Column("score")]
    public int Score { get; set; }

    [Column("time")]
    public int Time { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }
}
