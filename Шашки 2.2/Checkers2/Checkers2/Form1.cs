using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region global
        bool player = true;
        static Player[] players = new Player[2];
        Image ch_black = Properties.Resources._4;
        Image ch_white = Properties.Resources._5;
        Image qw_black = Properties.Resources._2;
        Image qw_white = Properties.Resources._3;
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            start2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (player)
            {
                players[0].checkers.Clear();
            }
            else
            {
                players[1].checkers.Clear();
            }
            winning();
        }
        private void Pic_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            string[] s = pic.Name.Split('_');
            int x = int.Parse(s[0]);
            int y = int.Parse(s[1]);
            label1.Text = $"Click on {pic.Name}";
            
            if (player)
            {
                if (pic.BackColor == Color.Green)
                {
                    players[0].move(x, y);
                }
                else if(pic.Image !=null)
                {
                    players[0].step(x, y);
                }
            }
            else
            {
                players[1].step(x,y);
            }
            draw2();
        }
        void start2()
        {
            players[0] = new Human(true);
            players[1] = new Human(false);
            int size = panel1.Width / 8;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    PictureBox pic = new PictureBox
                    {
                        Name = $"{x}_{y}",
                        BorderStyle = BorderStyle.FixedSingle,
                        Width = size,
                        Height = size,
                        Location = new Point(x * size, y * size),
                        BackColor = Color.White,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                    };

                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        pic.BackColor = Color.Gray;
                        if (y < 3)
                        {
                            players[1].checkers.Add(new Checker(false, true, x, y));
                        }
                        else if (y > 4)
                        {
                            players[0].checkers.Add(new Checker(true, true, x, y));
                        }
                        
                    }
                    pic.MouseClick += Pic_MouseClick;
                    panel1.Controls.Add(pic);
                }
            }
            
            draw2();

        }
        void draw2()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        PictureBox pic = panel1.Controls[$"{ x}_{ y}"] as PictureBox;
                        pic.BackColor = Color.Gray;
                        pic.Image = null;
                    }
                } 
            }
            for (int i = 0; i < 2; i++)
            {
                foreach (Checker ch in players[i].checkers)
                {
                    PictureBox pic = panel1.Controls[$"{ch.x}_{ch.y}"] as PictureBox;
                    if (ch.player && ch.type)
                    {
                        pic.Image = ch_white;
                    }
                    if (!ch.player && ch.type)
                    {
                        pic.Image = ch_black;
                    }
                    pic.BackColor = ch.color;
                    foreach(int[] block in ch.greens)
                    {
                        PictureBox pic1 = panel1.Controls[$"{block[0]}_{block[1]}"] as PictureBox;
                        pic1.BackColor = Color.Green;
                    }
                }
            }
        }
        private void winning()
        {
            if (players[0].checkers.Count == 0)
            {
                MessageBox.Show("Black is a winner");
            }
            else if (players[1].checkers.Count == 0)
            {
                MessageBox.Show("White is a winner");
            }
        }
        public static bool[] check(int x, int y)
        {
            foreach (var p in players)
            {
                for(int i = 0;i < p.checkers.Count; i++)
                {
                    if(p.checkers[i].x == x && p.checkers[i].y == y)
                    {
                        return new bool[2] { p.checkers[i].player, p.checkers[i].type };
                    }
                }
            }
            return new bool[1] { false };
        }
    }
    class Checker
    {
        public bool player;
        public bool type;
        public Color color = Color.Gray;
        public int x;
        public int y;
        public List<int[]> greens = new List<int[]>();
        public List<int[]> reds = new List<int[]>();
        public List<int[]> purples = new List<int[]>();

        public Checker(bool p, bool t, int _x, int _y)
        {
            player = p;
            type = t;
            x = _x;
            y = _y;
        }
        public void make_gray()
        {
            color = Color.Gray;
        }
        public void make_yellow() 
        {
            color = Color.Yellow;
            make_green();
        }
        void make_green()
        {
            if (player) 
            {
                bool[] b = Form1.check(x - 1, y - 1);
                if (b.Length == 1)
                    greens.Add(new int[2] { x - 1, y - 1 });
                b = Form1.check(x + 1, y - 1);
                if (b.Length == 1)
                    greens.Add(new int[2] { x + 1, y - 1 });
            }
        }
        void make_red()
        {

        }
        void make_purple()
        {

        }
        public void clear()
        {
            greens.Clear();
            reds.Clear();
            purples.Clear();
        }
        
    }
    class Player
    {
        public bool color;
        public List<Checker> checkers = new List<Checker>();
        public virtual void step(int x, int y) { }
        public void move(int x, int y) 
        { 
            foreach(var ch in checkers)
            {
                foreach(var g in ch.greens)
                {
                    if(g[0] == x && g[1] == y)
                    {
                        ch.x = x;
                        ch.y = y;
                        ch.clear();
                        ch.make_gray();
                        return;
                    }
                }
            }
        }
        
        public void end_turn() { }
    }
    class Human : Player
    {
        public Human(bool _color)
        {
            color = _color;
        }
        public override void step(int x, int y)
        {
            int j = 0;
            for (int i = 0; i < checkers.Count; i++)
            {
                if(checkers[i].x == x && checkers[i].y == y)
                {
                    j = i;
                }
                checkers[i].make_gray();
                checkers[i].clear();
            }
            if(j < checkers.Count)
            {
                checkers[j].make_yellow();
            }
        }
    }
    class AI : Player
    {
        public override void step(int x, int y)
        {

        }
    }
}