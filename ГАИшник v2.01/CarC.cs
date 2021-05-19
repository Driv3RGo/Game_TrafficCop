using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ГАИшник_v2._01
{
    public class CarC : CarP
    {
        public static event CarEvent MoveEvent;          // событие на проверку других машин спереди

        public CarC(int x, int y, int napr, int speed)
            : base(x, y, napr, speed)
        {
            switch (napr)
            {
                case 0: skin = new Bitmap(Properties.Resources.Красная1_влево_); break;
                case 1: skin = new Bitmap(Properties.Resources.Оранжевая1_вправо_); break;
                case 2: skin = new Bitmap(Properties.Resources.Синия1_вверх_); break;
                case 3: skin = new Bitmap(Properties.Resources.Черная1_вниз_); break;
            }
            cop = false;
        }

        public override void Move(Form1 f1)
        {
            CarEventArgs ev = new CarEventArgs(); //создаем сообщение
            ev.napr = napr;
            ev.x = x;
            ev.y = y;
            switch (napr)
            {
                case 0: { ev.xnext = x - speed; ev.ynext = y; } break;
                case 1: { ev.xnext = x + speed + 70; ev.ynext = y; } break;
                case 2: { ev.xnext = x; ev.ynext = y - speed; } break;
                case 3: { ev.xnext = x; ev.ynext = y + speed + 70; } break;
            }
            ToMoveEvent(this, ev); // вызываем обработчик события
            if (!ev.danger)
                base.Move(f1);
        }

        protected void ToMoveEvent(object sender, CarEventArgs ev)
        {
            if (MoveEvent != null)
                MoveEvent(sender, ev);
        }
    }
}
