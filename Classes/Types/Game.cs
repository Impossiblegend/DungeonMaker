﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Types
{
    public class Game
    {
        public int gameID { get; set; }
        public User player { get; set; }
        public Map map { get; set; }
        public DateTime datePlayed { get; set; }
        public int time { get; set; }
        public int stars { get; set; }
        public int deaths { get; set; }
        public bool victory { get; set; }
        public Game() { }
        public Game(int gameID)
        {
            this.gameID = gameID;
            DataSet ds = GeneralService.GetDataSetByQuery("SELECT * FROM Games WHERE gameID = " + gameID, "Games");
            DataRow game = ds.Tables[0].Rows[0];
            this.player = new User(game["player"].ToString());
            this.map = new Map(Convert.ToInt32(game["mapID"]), false);
            this.datePlayed = (DateTime)game["datePlayed"];
            this.time = Convert.ToInt32(game["timeElapsed"]);
            this.stars = Convert.ToInt32(game["starsCollected"]);
            this.deaths = Convert.ToInt32(game["deathCount"]);
            this.victory = (bool)game["victory"];
        }
        public Game(int gameID, User player, Map map, DateTime datePlayed, int time, int stars, int deaths, bool victory)
        {
            this.gameID = gameID;
            this.player = player;
            this.map = map;
            this.datePlayed = datePlayed;
            this.time = time;
            this.stars = stars;
            this.deaths = deaths;
            this.victory = victory;
        }
    }
}