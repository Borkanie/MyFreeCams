using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Booster.Guest_2._0
{
    public class Message
    {
        public int c { get; set; }
        public int f { get; set; }
        public int t { get; set; }
        public int a1 { get; set; }
        public int a2 { get; set; }
        public string Data { get; set; }

        private bool _disposed = false;

      
       

        public override string ToString()
        {
            return c.ToString() + " " + t.ToString() + " " + f.ToString() + " " + a1.ToString() + " " + a2.ToString() + " " + Data.ToLower() + "\n\0";
        }
    }
}
