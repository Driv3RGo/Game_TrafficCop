using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ГАИшник_v2._01
{
    public class Svetofor
    {
        public Bitmap skin;
        protected int x, y;     //Координаты
        protected int color;    //Цвет светофора( 0-красный, 1-зеленый )

        public Svetofor(int x, int y, int color)
        {
            Form1.Svetofor += new Form1.FormEvent(swap);           //Проверка светофоров
            this.x = x;
            this.y = y;
            this.color = color;
            switch (color)
            {
                case 0: skin = new Bitmap(Properties.Resources.Red); break;
                case 1: skin = new Bitmap(Properties.Resources.Green); break;
            }
        }

        void swap(int r)
        {
            color++;
            color %= 2;
            switch (color)
            {
                case 0: skin = new Bitmap(Properties.Resources.Red); break;
                case 1: skin = new Bitmap(Properties.Resources.Green); break;
            }
        }

        public int X()
        {
            return x;
        }
        public int Y()
        {
            return y;
        }
        public int Color()
        {
            return color;
        }
    }
}
