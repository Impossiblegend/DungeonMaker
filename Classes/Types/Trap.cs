using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DungeonMaker.classes.Types
{
    public class Trap : GameObject
    {
        public string type { get; set; }
        public Trap() : base() { }
        public Trap(int x, int y) : base(x, y) { }
        public Trap(int x, int y, string type) : base(x, y) { this.type = type; }
    }
}