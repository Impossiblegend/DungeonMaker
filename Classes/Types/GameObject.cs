using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DungeonMaker.Classes.Types
{
    public class GameObject
    {
        public int x { get; set; }
        public int y { get; set; }
        public GameObject() { }
        public GameObject(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}