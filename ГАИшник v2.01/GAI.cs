using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ГАИшник_v2._01
{
    public class GAI : Travel
    {
        protected int health;           //Здоровье
        protected int money;            //Монетки
        protected int k = 0;            //Кол-во остановленных машин
        protected Bitmap cost, heart, pogon;   //Изображение монеток, жизней и погонов
        protected int s = 30;           //Размер изображения жизней
        protected bool status;          //true - жезл поднят false - жезл опущен
        private bool dcd = true;        //Анимация движения
        protected int progress=10;      //Заполнение шкалы званий

        public GAI(int x, int y, int napr, int speed)
            : base(x, y, napr, speed)
        {
            Form1.GIBDD += new Form1.FormEvent(Parametr);           //Изменение параметров игрока
            health = 3;
            money = 0;
            status = false;
            cost = new Bitmap(Properties.Resources.Монетки);
            heart = new Bitmap(Properties.Resources.жизнь);
            skin = new Bitmap(Properties.Resources.DownPlayer_s);
        }

        public void show(Graphics g, Form1 f1)
        {
            g.DrawImage(cost, new Rectangle(f1.ClientRectangle.Right - 65, 45, 30, 30));
            switch (health)
            {
                case 1: g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 35, 5, s, s)); break;
                case 2: g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 35, 5, s, s)); g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 65, 5, s, s)); break;
                case 3: g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 35, 5, s, s)); g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 65, 5, s, s)); g.DrawImage(heart, new Rectangle(f1.ClientRectangle.Right - 95, 5, s, s)); break;
            }
            g.DrawImage(skin, new Rectangle(x, y, 50, 50));
            //g.DrawImage(pogon, new Rectangle());
        }

        void Parametr(int r)
        {
            switch (r)
            {
                case 1: money += 10; k++; break;
                case 2: x=800; y=120; health--; break;
            }
            switch (k)
            {
                case 0: progress = 10; break;           //Офицер
                case 10: progress = 10; break;          //Мл.Сержант
                case 20: progress = 10; break;          //Сержант
                case 30: progress = 10; break;          //Ст.Сержант
                case 40: progress = 10; break;          //Прапорщик
                case 50: progress = 10; break;          //Ст.Прапорщик
                case 60: progress = 10; break;          //Мл.Лейтенант
                case 75: progress = 6; break;           //Лейтенант
                case 90: progress = 6; break;           //Ст.Лейтенант
                case 105: progress = 6; break;          //Капитан
                case 120: progress = 6; break;          //Майор
                case 135: progress = 6; break;          //Подполковник
                case 155: progress = 5; break;          //Полковник
            }
        }

        public void Stop()
        {
            if (status)
            {
                status = false;
                switch (napr)
                {
                    case 1: skin = new Bitmap(Properties.Resources.UpPlayer_s); break;
                    case 2: skin = new Bitmap(Properties.Resources.DownPlayer_s); break;
                    case 3: skin = new Bitmap(Properties.Resources.LeftPlayer_s); break;
                    case 4: skin = new Bitmap(Properties.Resources.RigthPlayer_s); break;
                }
            }
            else
            {
                status = true;
                switch (napr)
                {
                    case 1: skin = new Bitmap(Properties.Resources.UpPlayer_stick); break;
                    case 2: skin = new Bitmap(Properties.Resources.DownPlayer_stick); break;
                    case 3: skin = new Bitmap(Properties.Resources.LeftPlayer_stick); break;
                    case 4: skin = new Bitmap(Properties.Resources.RigthPlayer_stick); break;
                }
            }
        }

        public void Move(int key)
        {
            if (!status)
            {
                switch (key)
                {
                    case 1: y -= speed; napr = 1; Draw(); break;        //Вверх
                    case 2: y += speed; napr = 2; Draw(); break;        //Вниз
                    case 3: x -= speed; napr = 3; Draw(); break;        //Влево
                    case 4: x += speed; napr = 4; Draw(); break;        //Вправо
                }
            }
        }

        protected void Draw()
        {
            if (dcd)
            {
                switch (napr)
                {
                    case 1: skin = new Bitmap(Properties.Resources.UpPlayer_go1); break;
                    case 2: skin = new Bitmap(Properties.Resources.DownPlayer_go1); break;
                    case 3: skin = new Bitmap(Properties.Resources.LeftPlayer_go1); break;
                    case 4: skin = new Bitmap(Properties.Resources.RigthPlayer_go1); break;
                }
            }
            else
            {
                switch (napr)
                {
                    case 1: skin = new Bitmap(Properties.Resources.UpPlayer_go2); break;
                    case 2: skin = new Bitmap(Properties.Resources.DownPlayer_go2); break;
                    case 3: skin = new Bitmap(Properties.Resources.LeftPlayer_go2); break;
                    case 4: skin = new Bitmap(Properties.Resources.RigthPlayer_go2); break;
                }
            }
        }

        public void Draw1()
        {
            switch (napr)
            {
                case 1: skin = new Bitmap(Properties.Resources.UpPlayer_s); break;
                case 2: skin = new Bitmap(Properties.Resources.DownPlayer_s); break;
                case 3: skin = new Bitmap(Properties.Resources.LeftPlayer_s); break;
                case 4: skin = new Bitmap(Properties.Resources.RigthPlayer_s); break;
            }
        }

        public bool Status()
        {
            return status;
        }

        public int Health()
        {
            return health;
        }

        public int Money()
        {
            return money;
        }

        public void t_Tick()
        {
            dcd = !dcd;
        }

        public int Progress()
        {
            return progress;
        }

        public int K()
        {
            return k;
        }
    }
}
