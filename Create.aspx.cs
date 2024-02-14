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

namespace DungeonMaker
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack) Session["mapType"] = "blank";
            if (((User)Session["user"]).email == null) Response.Redirect("Register.aspx");
        }
        protected void Selection_Change(object sender, EventArgs e)
        { //Sends data to javascript on map type change
            TB1.Text = MapTypesDDL.SelectedValue;
            Session["mapType"] = TB1.Text;
            switch (TB1.Text)
            {
                case "blank": TB2.Text = "20"; break;
                case "classic": TB2.Text = "25"; break;
                default: throw new Exception("Map type does not exist.");
            }
        }
        protected void Submit_Click(object sender, EventArgs e)
        { //Saves everything and finishes map creation
            //Random rand = new Random();
            MapService MS = new MapService();
            string mapName = mapName_TextBox.Text,
                //If (a) map(s) with the same name exist(s), adds count to name [0,1,2...], circumventing naming override problem. Analogous to the ticks solution.
                imagePath = "~/assets/screenshots/" + mapName + MapService.CountMapsWithName(mapName_TextBox.Text) + ".jpg",
                filePath = Server.MapPath(imagePath);
            if (string.IsNullOrEmpty(mapName)) ScriptManager.RegisterStartupScript(this, GetType(), "AlertScript", "alert('You must name your map!');", true);
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
                if (portalFull && portalEmpty) MapService.UploadMap(((User)Session["user"]).email, mapName + MapService.CountMapsWithName(mapName), Session["mapType"].ToString());
                else 
                { 
                    ScriptManager.RegisterStartupScript(this, GetType(), "AlertScript", "alert('You must have both the full and empty portals placed!');", true);
                    FileInfo thumbnail = new FileInfo(filePath);
                    try { thumbnail.Delete(); }
                    catch { ScriptManager.RegisterStartupScript(this, GetType(), "AlertScript", "console.log('Deletion error, likely due to physical path.');", true);  }
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
                Session["userPage"] = (User)Session["user"];
                Response.Redirect("Userpage.aspx");
            }
        }
    }
}