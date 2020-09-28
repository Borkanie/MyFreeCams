using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic_Booster.Guest_2._0
{
    //Clasa pentru modele(Fetele) codul este codul camerei,numarul este numarul botului pentru consola
    public class Model
    {
        public Model()
        {
        }

        public Model(string row,int numar)
        {
            Name = row;
            Code = 0;
            Number = numar;
        }
        public string Name { get; set; }
        public int Code { get; set; }
        public object Number { get; internal set; }
    }
}
