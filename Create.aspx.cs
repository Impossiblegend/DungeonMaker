using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Web.UI.WebControls;
using System.IO;
using DungeonMaker.Classes.Types;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Collections;
using DungeonMaker.Classes.Services;

namespace DungeonMaker
{
    public partial class Create : System.Web.UI.Page
    {
        private User user;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = (User)Session["user"]; 
            if (user.email == null || user.elevation < 1) Response.Redirect("Register.aspx");
            if (!IsPostBack) 
            {
                TB1.Text = Session["mapType"].ToString();
                StoreService SS = new StoreService();
                TB2.Text = StoreService.GetTypeCost(TB1.Text).ToString();
                foreach (string st in user.GetTrapTypes()) TB6.Text += st + "_";
            }
        }
        private void alert(string alert) { ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + alert + "');", true); }
        protected void Submit_Click(object sender, EventArgs e)
        { //Saves everything and finishes map creation
            //Random rand = new Random();
            MapService MS = new MapService();
            string mapName = mapName_TextBox.Text,
                //If (a) map(s) with the same name exist(s), adds count to name [0,1,2...], circumventing naming override problem. Analogous to the ticks solution.
                imagePath = "~/assets/screenshots/" + mapName + MapService.CountMapsWithName(mapName_TextBox.Text) + ".jpg",
                filePath = Server.MapPath(imagePath);
            if (string.IsNullOrEmpty(mapName)) alert("You must name your dungeon.");
            else
            {
                Point SourcePoint = new Point(0, 100);
                Point DestinationPoint = new Point(0, 0);
                Bitmap bt = new Bitmap(1150, 600);
                //bt.MakeTransparent();
                Graphics gr = Graphics.FromImage(bt);
                gr.CopyFromScreen(SourcePoint, DestinationPoint, new Size(1150, 600));
                bt.Save(filePath, ImageFormat.Jpeg);
                gr.Dispose(); bt.Dispose();
                string[] types = TB5.Text.Split('_');
                ArrayList types2 = new ArrayList();
                bool portalFull = false, portalEmpty = false;
                foreach (string type in types)
                {
                    if (type != "") types2.Add(type);
                    if (type == "portalFull") portalFull = true;
                    if (type == "portalEmpty") portalEmpty = true;
                }
                if (portalFull && portalEmpty) MapService.UploadMap(user.email, mapName + MapService.CountMapsWithName(mapName), Session["mapType"].ToString());
                else 
                {
                    alert("You must have both the full and empty portals placed.");
                    FileInfo thumbnail = new FileInfo(filePath);
                    try { thumbnail.Delete(); }
                    catch { alert("Deletion error, likely due to physical path.");  }
                    return;
                }
                Thread.Sleep(5);
                string[] xpos = TB1.Text.Split('_');
                string[] ypos = TB2.Text.Split('_');
                if (xpos[0] != "" && ypos[0] != "") //If stars have been placed (otherwise there will be a parsing error inserting)
                    for (int i = 0; i < xpos.Length; i++)
                        MapService.InsertStars(int.Parse(xpos[i]), int.Parse(ypos[i]));
                xpos = TB3.Text.Split('_');
                ypos = TB4.Text.Split('_');
                if (xpos[0] != "" && ypos[0] != "") //If traps have been placed (otherwise there will be a parsing error inserting)
                    for (int i = 0; i < xpos.Length; i++)
                        MapService.InsertTraps((int)double.Parse(xpos[i]), (int)double.Parse(ypos[i]), types2[i].ToString());
                MapService.DeleteAllButNewestByTrapType("portalFull"); //In case of a duplicates bug
                MapService.DeleteAllButNewestByTrapType("portalEmpty"); // -||-
                Session["userPage"] = user;
                Response.Redirect("Userpage.aspx");
            }
        }
    }
}