using DungeonMaker.Classes.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DungeonMaker.classes.Types
{
    public class Map
    {
        public int mapID { get; set; }
        public User creator { get; set; }
        public string mapType { get; set; }
        public DateTime creationDate { get; set; }
        public string mapName { get; set; }
        public bool isPublic { get; set; }
        public int estTime { get; set; }
        public string thumbnail { get; set; }
        public ArrayList stars { get; set; } //strictly of type GameObject
        public ArrayList traps { get; set; } //strictly of type Trap
        public bool isValid { get; set; }
        public Map() { }
        public Map(int mapID)
        {
            this.mapID = mapID;
            string query = "SELECT * FROM Maps WHERE mapID = " + mapID;
            DataTable Maps = ProductService.GetProductsByQuery(query, "Maps").Tables[0];
            this.creator = new User(Maps.Rows[0]["creator"].ToString());
            this.mapType = Maps.Rows[0]["mapType"].ToString();
            this.creationDate = (DateTime)Maps.Rows[0]["creationDate"];
            this.isPublic = (bool)Maps.Rows[0]["isPublic"];
            this.estTime = (int)Maps.Rows[0]["estTime"];
            this.thumbnail = Maps.Rows[0]["thumbnail"].ToString();
            this.isValid = (bool)Maps.Rows[0]["isValid"];
            this.stars = MapService.GetStarsByMapID(mapID);
            this.traps = MapService.GetTrapsByMapID(mapID);
        }
    }
}