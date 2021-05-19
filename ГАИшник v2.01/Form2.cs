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
    public partial class Form2 : Form
    {
        static public int n;   //Кол-во машин

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = trackBar1.Value;
            Hide();
            Form1 Играть = new Form1();
            Играть.ShowDialog();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Игра была разработана студентом ИГЭУ группы 2 - 41хх Ткачёвым Н.В. в качестве курсового проекта по курсу Объектно-ориентированное программирование", "Об авторе", MessageBoxButtons.OK);

            if (res == DialogResult.OK)
                MessageBox.Show("По жанру TraFfiC Cop v1.0 – симулятор ГАИшника, в котором игрок управляет полицейским, который ходит по дороге и останавливает нарушителей. Цель игры состоит в получение звания 'Полковника'. Для достижения цели нужно останавливить порядка 100 нарушителей. Чем выше звание, тем более легче становится тормозить машины.", "Правила игры");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            n = trackBar1.Value;
        }
    }
}
