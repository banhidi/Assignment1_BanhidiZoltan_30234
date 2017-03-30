using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement {
    public struct Product {
        public int id { get; set; }
        public int stock { get; set; }
        public string title { get; set; }
        public string color { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int depth { get; set; }
        public float price { get; set; }

        public override string ToString() {
            string s = "id=" + id + ", title=" + title + ", color=" + color;
            s += ", size=" + width + "*" + height + "*" + depth;
            s += ", price=" + price + ", stock=" + stock;
            return s;
        }
    }
}
