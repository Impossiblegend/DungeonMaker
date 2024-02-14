using DungeonMaker.Classes.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DungeonMaker.classes.Types
{
    public class Comment
    {
        public int feedbackID { get; set; }
        public User sender { get; set; }
        public string feedbackBody { get; set; }
        public int starRating { get; set; }
        public DateTime dateSent { get; set; }
        public bool isFeatured { get; set; }
        public Comment() { }
        public Comment(int feedbackID)
        {
            this.feedbackID = feedbackID;
            DataSet ds = ProductService.GetDataSetByQuery("SELECT * FROM Feedback WHERE feedbackID = " + feedbackID, "Feedback");
            DataRow comment = ds.Tables[0].Rows[0];
            this.sender = new User(comment["sender"].ToString());
            this.feedbackBody = comment["feedbackBody"].ToString();
            this.starRating = Convert.ToInt32(comment["starRating"]);
            this.dateSent = (DateTime)comment["dateSent"];
            this.isFeatured = (bool)comment["isFeatured"];
        }
    }
}