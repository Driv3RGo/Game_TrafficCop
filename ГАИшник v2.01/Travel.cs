using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ГАИшник_v2._01
{
    public class Travel
    {
        public Bitmap skin;     //Изображение объекта
        protected int x, y;      //Координаты
        protected int napr;     //Направление
        protected int speed;    //Скорость

        public Travel(int x, int y, int napr, int speed)
        {
            this.x = x;
            this.y = y;
            this.napr = napr;
            this.speed = speed;
        }

        public int X()
        {
            return x;
        }
        public int Y()
        {
            return y;
        }
        public int Napr()
        {
            return napr;
        }
    }
}
