using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ГАИшник_v2._01
{
    public class CarEventArgs : EventArgs  //класс сообщения
    {
        public int x, y;            //Координаты машины
        public int napr;            //Направление движения 
        public bool danger;         //Есть помеха
        public int xnext, ynext;    //Следующие координаты машины
        public Boolean cop;
    }

    public class CarP : Travel
    {
        public delegate void CarEvent(object sender, CarEventArgs ev);      // делегат
        public static event CarEvent Svetofor;                              // событие проверка светофора
        public static event CarEvent GIBDD;                                 // событие проверка на гаишника
        static Random rnd = new Random();
        protected Boolean status, status1;  //status-проверка выполнен ли поворот; status1-выбрана ли полоса для поворота
        protected int e;                    //Следущее направление машины
        protected int e1, e2;               //Границы поворота
        protected Boolean cop;              //Статус машины true–спец false–обычный

        public CarP(int x, int y, int napr, int speed)
            : base(x, y, napr, speed)
        {
            switch (napr)
            {
                case 0: skin = new Bitmap(Properties.Resources.Police); break;
                case 1: skin = new Bitmap(Properties.Resources.Police2); break;
                case 2: skin = new Bitmap(Properties.Resources.Police1); break;
                case 3: skin = new Bitmap(Properties.Resources.Police3); break;
            }
            cop = true;
        }

        public virtual void Move(Form1 f1)
        {
            CarEventArgs ev = new CarEventArgs();   //создаем сообщение
            ev.napr = napr;
            ev.x = x;
            ev.y = y;
            ev.cop = cop;
            switch (napr)
            {
                case 0: { ev.xnext = x - speed; ev.ynext = y; } break;
                case 1: { ev.xnext = x + speed + 70; ev.ynext = y + 23; } break;
                case 2: { ev.xnext = x + 23; ev.ynext = y - speed; } break;
                case 3: { ev.xnext = x; ev.ynext = y + speed + 70; } break;
            }
            OnMoveEvent(this, ev);                  // вызываем обработчик события
            if (!ev.danger)
            {
                switch (napr)
                {
                    case 0:     //Влево
                        if (x <= f1.ClientRectangle.Right + 100 && x >= f1.ClientRectangle.Left - 200)
                        {
                            x -= speed;
                            if (status1 == false && x >= f1.ClientRectangle.Right - 150 && x <= f1.ClientRectangle.Right - 100)
                            {
                                e = rnd.Next(2);
                                status1 = true;
                                switch (e)
                                {
                                    case 0: e1 = 0; e2 = 0; break;
                                    case 1: y = 212; e1 = 560; e2 = 600; break;
                                    case 2: y = 242; e1 = 478; e2 = 500; break;
                                }
                            }
                            else
                            {
                                if (x >= e1 && x <= e2 && status == false)
                                    switch (e)
                                    {
                                        case 0:
                                            napr = 0; status = true; break;                                                                 //Прямо
                                        case 1:
                                            napr = 2; x = 600; status = true; skin.RotateFlip(RotateFlipType.Rotate90FlipX); break;           //Направо
                                        case 2:
                                            napr = 3; x = 478; status = true; skin.RotateFlip(RotateFlipType.Rotate270FlipX); break;          //Налево
                                    }
                            }
                        }
                        else
                        {
                            e1 = -300;
                            e2 = -300;
                            status = false;
                            status1 = false;
                            y = 304;
                            x = -90;
                            skin.RotateFlip(RotateFlipType.Rotate180FlipY);
                            napr = 1;
                        }
                        break;
                    case 1:     //Вправо
                        if (x <= f1.ClientRectangle.Right + 100 && x >= f1.ClientRectangle.Left - 200)
                        {
                            x += speed;
                            if (status1 == false && x >= f1.ClientRectangle.Left + 200 && x <= f1.ClientRectangle.Left + 250)
                            {
                                e = rnd.Next(2);
                                status1 = true;
                                switch (e)
                                {
                                    case 0: e1 = 0; e2 = 0; break;
                                    case 1: y = 304; e1 = 478; e2 = 520; break;
                                    case 2: y = 272; e1 = 580; e2 = 600; break;
                                }
                            }
                            else
                            {
                                if (x >= e1 && x <= e2 && status == false)
                                    switch (e)
                                    {
                                        case 0:
                                            napr = 1; status = true; break;                                                                 //Прямо
                                        case 1:
                                            napr = 3; x = 478; status = true; skin.RotateFlip(RotateFlipType.Rotate90FlipX); break;           //Направо
                                        case 2:
                                            napr = 2; x = 600; status = true; skin.RotateFlip(RotateFlipType.Rotate270FlipX); break;          //Налево
                                    }
                            }
                        }
                        else
                        {
                            e1 = -300;
                            e2 = -300;
                            status1 = false;
                            status = false;
                            y = 212;
                            x = f1.ClientRectangle.Right + 90;
                            skin.RotateFlip(RotateFlipType.Rotate180FlipY);
                            napr = 0;
                        }
                        break;
                    case 2:     //Вверх
                        if (y <= f1.ClientRectangle.Bottom + 100 && y >= f1.ClientRectangle.Top - 200)
                        {
                            y -= speed;
                            if (status1 == false && y >= f1.ClientRectangle.Bottom - 150 && y <= f1.ClientRectangle.Bottom - 100)
                            {
                                e = rnd.Next(2);
                                status1 = true;
                                switch (e)
                                {
                                    case 0: e1 = 0; e2 = 0; break;
                                    case 1: x = 600; e1 = 272; e2 = 304; break;
                                    case 2: x = 560; e1 = 212; e2 = 222; break;
                                }
                            }
                            else
                            {
                                if (y >= e1 && y <= e2 && status == false)
                                    switch (e)
                                    {
                                        case 0:
                                            napr = 2; status = true; break;                                                               //Прямо
                                        case 1:
                                            napr = 1; y = 304; status = true; skin.RotateFlip(RotateFlipType.Rotate90FlipY); break;         //Направо
                                        case 2:
                                            napr = 0; y = 212; status = true; skin.RotateFlip(RotateFlipType.Rotate270FlipY); break;        //Налево
                                    }
                            }
                        }
                        else
                        {
                            e1 = -300;
                            e2 = -300;
                            status1 = false;
                            status = false;
                            y = -90;
                            x = 478;
                            skin.RotateFlip(RotateFlipType.Rotate180FlipX);
                            napr = 3;
                        }
                        break;
                    case 3:     //Вниз
                        if (y <= f1.ClientRectangle.Bottom + 100 && y >= f1.ClientRectangle.Top - 200)
                        {
                            y += speed;
                            if (status1 == false && y >= f1.ClientRectangle.Top + 10 && y <= f1.ClientRectangle.Top + 150)
                            {
                                e = rnd.Next(2);
                                status1 = true;
                                switch (e)
                                {
                                    case 0: e1 = 0; e2 = 0; break;
                                    case 1: x = 478; e1 = 212; e2 = 242; break;
                                    case 2: x = 520; e1 = 294; e2 = 304; break;
                                }
                            }
                            else
                            {
                                if (y >= e1 && y <= e2 && status == false)
                                    switch (e)
                                    {
                                        case 0:
                                            napr = 3; status = true; break;                                                               //Прямо
                                        case 1:
                                            napr = 0; y = 212; status = true; skin.RotateFlip(RotateFlipType.Rotate90FlipY); break;         //Направо
                                        case 2:
                                            napr = 1; y = 304; status = true; skin.RotateFlip(RotateFlipType.Rotate270FlipY); break;        //Налево
                                    }
                            }
                        }
                        else
                        {
                            e1 = -300;
                            e2 = -300;
                            status1 = false;
                            status = false;
                            y = f1.ClientRectangle.Bottom + 90;
                            x = 600;
                            skin.RotateFlip(RotateFlipType.Rotate180FlipX);
                            napr = 2;
                        }
                        break;
                }
            }
        }

        protected void OnMoveEvent(object sender, CarEventArgs ev)
        {
            if (Svetofor != null)
                Svetofor(sender, ev);
            if (GIBDD != null)
                GIBDD(sender, ev);
        }
    }
}
