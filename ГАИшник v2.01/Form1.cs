using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ГАИшник_v2._01
{
    public partial class Form1 : Form
    {
        public delegate void FormEvent(int r);          // делегат
        public static event FormEvent Svetofor;         // событие на переключение светофора
        public static event FormEvent GIBDD;            // событие гаишнику
        static Random rnd = new Random();
        protected Form2 form2 = new Form2();
        protected Bitmap fon;              //Игровая карта
        protected Bitmap fonp;             //Затемнение карты
        protected CarP[] car;              //Объект машина
        protected Svetofor[] svetofor;     //Объект светофор
        protected GAI player;              //Объект игрок 
        int x, y, random;                  //Параметры для рандомного появления машин
        Boolean statistika = true;         //Проверка чтобы начисляли монетки за машину только 1 раз
        protected int n;                   //Кол-во машин
        Boolean pause = false;             //Поставить игру на паузу
        private bool dcd = true;           //Анимация паузы

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();
            CarP.Svetofor += new CarP.CarEvent(Car_Svetofor);           //Проверка светофоров
            CarP.GIBDD += new CarP.CarEvent(Car_GIBDD);                 //Проверка на гаишника
            CarC.MoveEvent += new CarC.CarEvent(Car_MoveEvent);         //Проверка машин
            fon = Properties.Resources.Фон;
            fonp = Properties.Resources.Pause2;
            n = Form2.n;
            car = new CarP[n];
            for (int i = 0; i < n; i++)
            {
                switch (i % 5)
                {
                    case 0:
                        switch (rnd.Next(1))
                        {
                            case 0: y = 212; break;
                            case 1: y = 242; break;
                        }
                        car[i] = new CarC(ClientRectangle.Right, y, 0, rnd.Next(3, 5)); break;         //Машина едет влево
                    case 1:
                        switch (rnd.Next(1))
                        {
                            case 0: y = 304; break;
                            case 1: y = 272; break;
                        }
                        car[i] = new CarC(ClientRectangle.Left - 150, y, 1, rnd.Next(3, 5)); break;         //Машина едет вправо
                    case 2:
                        switch (rnd.Next(1))
                        {
                            case 0: x = 600; break;
                            case 1: x = 560; break;
                        }
                        car[i] = new CarC(x, ClientRectangle.Bottom, 2, rnd.Next(3, 5)); break;       //Машина едет вверх
                    case 3:
                        switch (rnd.Next(1))
                        {
                            case 0: x = 478; break;
                            case 1: x = 520; break;
                        }
                        car[i] = new CarC(x, ClientRectangle.Top - 150, 3, rnd.Next(3, 5)); break;         //Машина едет вниз
                    case 4:
                        switch (random = rnd.Next(4))
                        {
                            case 0: x = ClientRectangle.Right; y = 212; break;
                            case 1: x = ClientRectangle.Left - 150; y = 304; break;
                            case 2: x = 600; y = ClientRectangle.Bottom; break;
                            case 3: x = 478; y = ClientRectangle.Top - 150; break;
                        }
                        car[i] = new CarP(x, y, random, rnd.Next(4, 6)); break;      //Спец.транспорт
                }
            }
            svetofor = new Svetofor[4];
            svetofor[0] = new Svetofor(637, 165, 0);    //Светофор для т/с приблежающегося слева
            svetofor[1] = new Svetofor(422, 337, 0);    //Светофор для т/с приблежающегося справа
            svetofor[2] = new Svetofor(637, 337, 1);    //Светофор для т/с приблежающегося сверху
            svetofor[3] = new Svetofor(422, 165, 1);    //Светофор для т/с приблежающегося снизу
            player = new GAI(800, 120, 3, 3);
        }

        void Car_Svetofor(object sender, CarEventArgs ev)      // обработчик события «проверка светофоров»
        {
            int d0 = 0;
            int r = 40;
            switch (ev.napr)    // проверяем, близко ли светофор
            {
                case 0: d0 = ev.x - svetofor[0].X(); break;
                case 1: d0 = svetofor[1].X() - ev.x; break;
                case 2: d0 = ev.y - svetofor[2].Y(); break;
                case 3: d0 = svetofor[3].Y() - ev.y; break;
            }
            switch (ev.napr)
            {
                case 0:
                    if (d0 < r && d0 > 0 && svetofor[0].Color() == 0)
                        ev.danger = true;
                    else ev.danger = false;
                    break;
                case 1:
                    if (d0 < r && d0 > 0 && svetofor[1].Color() == 0)
                        ev.danger = true;
                    else ev.danger = false;
                    break;
                case 2:
                    if (d0 < r && d0 > 0 && svetofor[2].Color() == 0)
                        ev.danger = true;
                    else ev.danger = false;
                    break;
                case 3:
                    if (d0 < r && d0 > 0 && svetofor[3].Color() == 0)
                        ev.danger = true;
                    else ev.danger = false;
                    break;
            }
        }

        void Car_GIBDD(object sender, CarEventArgs ev)
        {
            if (player.Status() && !ev.cop && ev.xnext >= player.X() && ev.xnext <= player.X() + 60 && ev.ynext >= player.Y() && ev.ynext <= player.Y() + 60)
            {
                ev.danger = true;
                if (statistika)
                {
                    GIBDD(1);
                    progressBar1.Value += player.Progress();
                    if (progressBar1.Value == 100 || (progressBar1.Value == 90 && player.Progress() == 6))
                    {
                        progressBar1.Value = 0;
                    }
                    statistika = false;
                }
            }
            else
            {
                if (player.Status() && ev.cop && ev.xnext >= player.X() && ev.xnext <= player.X() + 60 && ev.ynext >= player.Y() && ev.ynext <= player.Y() + 60)
                {
                    GIBDD(2);
                }
                /*switch (ev.napr)
                {
                    case 0:
                        if (!player.Status() && ((ev.x + 60 >= player.X() && ev.x <= player.X() && ev.y + 24 >= player.Y() && ev.y <= player.Y()) || (ev.x + 60 >= player.X() + 50 && ev.x <= player.X() + 50 && ev.y + 24 >= player.Y() + 50 && ev.y <= player.Y() + 50)))
                            GIBDD(2);
                        break;
                    case 1:
                        if (!player.Status() && ((ev.x + 60 >= player.X() && ev.x <= player.X() && ev.y + 24 >= player.Y() && ev.y <= player.Y()) || (ev.x + 60 >= player.X() + 50 && ev.x <= player.X() + 50 && ev.y + 24 >= player.Y() + 50 && ev.y <= player.Y() + 50)))
                            GIBDD(2);
                        break;
                    case 2:
                        if (!player.Status() && ((ev.x + 25 >= player.X() && ev.x <= player.X() && ev.y + 60 >= player.Y() && ev.y <= player.Y()) || (ev.x + 25 >= player.X() + 50 && ev.x <= player.X() + 50 && ev.y + 60 >= player.Y() + 50 && ev.y <= player.Y() + 50)))
                            GIBDD(2);
                        break;
                    case 3:
                        if (!player.Status() && ((ev.x + 25 >= player.X() && ev.x <= player.X() && ev.y + 60 >= player.Y() && ev.y <= player.Y()) || (ev.x + 25 >= player.X() + 50 && ev.x <= player.X() + 50 && ev.y + 60 >= player.Y() + 50 && ev.y <= player.Y() + 50)))
                            GIBDD(2);
                        break;
                }*/
                if (!player.Status() && ev.x >= player.X() && ev.x <= player.X() + 40 && ev.y >= player.Y() && ev.y <= player.Y() + 40)
                {
                    GIBDD(2);
                }
            }
        }

        void Car_MoveEvent(object sender, CarEventArgs ev)      // обработчик события «запрос автомобиля»
        {
            for (int i = 0; i < n; i++)
            {
                if (ev.xnext >= car[i].X() && ev.xnext <= car[i].X() + 70 && ev.ynext >= car[i].Y() && ev.ynext <= car[i].Y() + 70)
                {
                    ev.danger = true;
                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!pause)
            {
                switch (player.K())
                {
                    case 0: label3.Text = "Офицер"; break;      
                    case 10: label3.Text = "Мл.Сержант"; break;          
                    case 20: label3.Text = "Сержант"; break;          
                    case 30: label3.Text = "Ст.Сержант"; break;       
                    case 40: label3.Text = "Прапорщик"; break;          
                    case 50: label3.Text = "Ст.Прапорщик"; break;         
                    case 60: label3.Text = "Мл.Лейтенант"; break;          
                    case 75: label3.Text = "Лейтенант"; break;           
                    case 90: label3.Text = "Ст.Лейтенант"; break;         
                    case 105: label3.Text = "Капитан"; break;        
                    case 120: label3.Text = "Майор"; break;          
                    case 135: label3.Text = "Подполковник"; break;         
                    case 155: label3.Text = "Полковник"; break;    
                }
                label1.Text = Convert.ToString(player.Money());
                if (player.Health() == 0)
                {
                    timer1.Enabled = false;
                    Form3 Over = new Form3();
                    Over.ShowDialog();
                    Close();
                }
                for (int i = 0; i < n; i++)
                    car[i].Move(this);
            }
            Refresh();
        }       //Таймер для движения машин

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!pause)
            {
                Svetofor(1);
            }
        }       //Таймер для смены сигнала светофора

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Enabled = false;
            player.Stop();
            statistika = true;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(fon, new Rectangle(0, 0, 1280, 720));
            player.show(g, this);
            for (int i = 0; i < n; i++)
                g.DrawImage(car[i].skin, car[i].X(), car[i].Y());
            for (int i = 0; i < 4; i++)
                g.DrawImage(svetofor[i].skin, new Rectangle(svetofor[i].X(), svetofor[i].Y(), 40, 40));
            if (pause)
                g.DrawImage(fonp, new Rectangle(-125, -10, 1280, 720));
        }   //Отрисовка графики

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                switch (e.KeyData)
                {
                    case Keys.W: player.Move(1); break;
                    case Keys.S: player.Move(2); break;
                    case Keys.A: player.Move(3); break;
                    case Keys.D: player.Move(4); break;
                    case Keys.F:
                        if (!player.Status())
                        {
                            timer3.Enabled = true;
                            player.Stop();
                        }
                        break;
                }
            }
            if (e.KeyValue == 32)
                if (!pause)
                    pause = true;
                else pause = false;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            player.t_Tick();
            if (pause)
            {
                dcd = !dcd;
                if (dcd)
                    fonp = Properties.Resources.Pause1;
                else
                    fonp = Properties.Resources.Pause2;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyData!=Keys.F)
                player.Draw1();
        }
    }
}
