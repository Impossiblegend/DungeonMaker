using DungeonMaker.Classes.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Types
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
        public List<GameObject> stars { get; set; }
        public List<Trap> traps { get; set; }
        public bool isValid { get; set; }
        public Map() { }
        public Map(int mapID)
        {
            this.mapID = mapID;
            string query = "SELECT * FROM Maps WHERE mapID = " + mapID;
            DataTable Maps = GeneralService.GetDataSetByQuery(query, "Maps").Tables[0];
            creator = new User(Maps.Rows[0]["creator"].ToString());
            mapType = Maps.Rows[0]["mapType"].ToString();
            creationDate = (DateTime)Maps.Rows[0]["creationDate"];
            mapName = Maps.Rows[0]["mapName"].ToString();
            isPublic = (bool)Maps.Rows[0]["isPublic"];
            estTime = (int)Maps.Rows[0]["estTime"];
            thumbnail = Maps.Rows[0]["thumbnail"].ToString();
            isValid = (bool)Maps.Rows[0]["isValid"];
            stars = GetStars();
            traps = GetTraps();
        }
        public Map(int mapID, bool needLists)
        {
            this.mapID = mapID;
            string query = "SELECT * FROM Maps WHERE mapID = " + mapID;
            DataTable Maps = GeneralService.GetDataSetByQuery(query, "Maps").Tables[0];
            DataRow map = Maps.Rows[0];
            creator = new User(map["creator"].ToString());
            mapType = map["mapType"].ToString();
            creationDate = (DateTime)map["creationDate"];
            mapName = map["mapName"].ToString();
            isPublic = (bool)map["isPublic"];
            estTime = (int)map["estTime"];
            thumbnail = map["thumbnail"].ToString();
            isValid = (bool)map["isValid"];
            stars = needLists ? GetStars() : new List<GameObject>();
            traps = needLists ? GetTraps() : new List<Trap>();
        }
        public Map(int mapID, User creator, string mapType, DateTime creationDate, string mapName, 
            bool isPublic, int estTime, string thumbnail, List<GameObject> stars, List<Trap> traps, bool isValid)
        {
            this.mapID = mapID;
            this.creator = creator;
            this.mapType = mapType;
            this.creationDate = creationDate;
            this.mapName = mapName;
            this.isPublic = isPublic;
            this.estTime = estTime;
            this.thumbnail = thumbnail;
            this.stars = stars;
            this.traps = traps;
            this.isValid = isValid;
        }
        public void Delete()
        {
            MapService.MapID = this.mapID;
            MapService.DeleteByMapID("Maps");
            MapService.DeleteByMapID("Traps");
            MapService.DeleteByMapID("Stars");
        }

        private List<GameObject> GetStars()
        { //Returns all records in Stars table with foreign key mapID
            List<GameObject> stars = new List<GameObject>();
            string query = "SELECT xpos, ypos FROM Stars WHERE Stars.mapID = " + this.mapID;
            DataTable StarsTbl = GeneralService.GetDataSetByQuery(query, "Stars").Tables[0];
            foreach (DataRow dr in StarsTbl.Rows) stars.Add(new GameObject(Convert.ToInt32(dr["xpos"]), Convert.ToInt32(dr["ypos"])));
            return stars;
        }

        private List<Trap> GetTraps()
        { //Returns all records in Traps table with foreign key mapID
            List<Trap> traps = new List<Trap>();
            string query = "SELECT xpos, ypos, type FROM Traps WHERE Traps.mapID = " + this.mapID;
            DataTable TrapsTbl = GeneralService.GetDataSetByQuery(query, "Traps").Tables[0];
            foreach (DataRow dr in TrapsTbl.Rows) traps.Add(new Trap(Convert.ToInt32(dr["xpos"]), Convert.ToInt32(dr["ypos"]), dr["type"].ToString()));
            return traps;
        }
    }
}