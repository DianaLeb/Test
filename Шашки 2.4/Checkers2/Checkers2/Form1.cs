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
        public static bool player = true;
        static Player[] players = new Player[2];
        static bool mandatory_step = false;
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
                if (pic.BackColor == Color.Green || pic.BackColor == Color.Red)
                {
                    players[0].move(x, y);
                }
                else if(pic.Image != null && !mandatory_step)
                {
                    players[0].step(x, y);
                }
            }
            else
            {
                if (pic.BackColor == Color.Green || pic.BackColor == Color.Red)
                {
                    players[1].move(x, y);
                }
                else if (pic.Image != null && !mandatory_step)
                {
                    players[1].step(x, y);
                }
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
            //players[0].checkers.Add(new Checker(true, false, 4, 3));
            draw2();

        }
        void draw2()
        {
            if (player)
            {
                toolStripStatusLabel1.Text = "White";
            }
            else
            {
                toolStripStatusLabel1.Text = "Black";
            }
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
                    if(ch.player && !ch.type)
                    {
                        pic.Image = qw_white;
                    }
                    if (!ch.player && !ch.type)
                    {
                        pic.Image = qw_black;
                    }
                    pic.BackColor = ch.color;
                    foreach(int[] block in ch.greens)
                    {
                        PictureBox pic1 = panel1.Controls[$"{block[0]}_{block[1]}"] as PictureBox;
                        pic1.BackColor = Color.Green;
                    }
                    foreach (int[] block in ch.reds)
                    {
                        PictureBox pic1 = panel1.Controls[$"{block[0]}_{block[1]}"] as PictureBox;
                        pic1.BackColor = Color.Red;
                    }
                }
            }
        }
        public static void winning()
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
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {
                foreach (var p in players)
                {
                    for (int i = 0; i < p.checkers.Count; i++)
                    {
                        if (p.checkers[i].x == x && p.checkers[i].y == y)
                        {
                            return new bool[2] { p.checkers[i].player, p.checkers[i].type };
                        }
                    }
                }
                return new bool[1] { false };
            }
            return new bool[0];
        }
        public static void delete_checker(int x, int y)
        {
            if (player)
            {
                int i = 0;
                for(; i < players[1].checkers.Count;i++)
                {
                    if(players[1].checkers[i].x == x && players[1].checkers[i].y == y)
                    {
                        break;
                    }
                }
                players[1].checkers.RemoveAt(i);
            }
            else
            {
                int i = 0;
                for (; i < players[0].checkers.Count; i++)
                {
                    if (players[0].checkers[i].x == x && players[0].checkers[i].y == y)
                    {
                        break;
                    }
                }
                players[0].checkers.RemoveAt(i);
            }
        }
        public static void mandatory()
        {
            mandatory_step = false;
            if (player)
            {
                foreach(var ch in players[0].checkers)
                {
                    if (ch.make_red())
                    {
                        mandatory_step = true;
                    }
                }
            }
            else
            {
                foreach (var ch in players[1].checkers)
                {
                    if (ch.make_red())
                    {
                        mandatory_step = true;
                    }
                }
            }
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
            make_red();
            if (reds.Count == 0)
            {
                make_green();
            }
            
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
            else
            {
                bool[] b = Form1.check(x - 1, y + 1);
                if (b.Length == 1)
                    greens.Add(new int[2] { x - 1, y + 1 });
                b = Form1.check(x + 1, y + 1);
                if (b.Length == 1)
                    greens.Add(new int[2] { x + 1, y + 1 });
            }
            if (!type)
            {
                for(int i = 1; i < 8; i++)
                {
                    //up left
                    bool[] b = Form1.check(x - i, y - i);
                    if (b.Length == 1)
                        greens.Add(new int[2] { x - i, y - i });
                    //up right
                    b = Form1.check(x + i, y - i);
                    if (b.Length == 1)
                        greens.Add(new int[2] { x + i, y - i });
                    //down left
                    b = Form1.check(x - i, y + i);
                    if (b.Length == 1)
                        greens.Add(new int[2] { x - i, y + i });
                    //down right
                    b = Form1.check(x + i, y + i);
                    if (b.Length == 1)
                        greens.Add(new int[2] { x + i, y + i });
                }
            }
        }
        public bool make_red()
        {
            if (type)
            {
                //up left
                bool[] b = Form1.check(x - 1, y - 1);
                if (b.Length == 2)
                {
                    if (b[0] != player)
                    {
                        b = Form1.check(x - 2, y - 2);
                        if (b.Length == 1)
                            reds.Add(new int[4] { x - 2, y - 2, x - 1, y - 1 });
                    }
                }
                //up right
                b = Form1.check(x + 1, y - 1);
                if (b.Length == 2)
                {
                    if (b[0] != player)
                    {
                        b = Form1.check(x + 2, y - 2);
                        if (b.Length == 1)
                            reds.Add(new int[4] { x + 2, y - 2, x + 1, y - 1 });
                    }
                }
                //down left
                b = Form1.check(x - 1, y + 1);
                if (b.Length == 2)
                {
                    if (b[0] != player)
                    {
                        b = Form1.check(x - 2, y + 2);
                        if (b.Length == 1)
                            reds.Add(new int[4] { x - 2, y + 2, x - 1, y + 1 });
                    }
                }
                //down right
                b = Form1.check(x + 1, y + 1);
                if (b.Length == 2)
                {
                    if (b[0] != player)
                    {
                        b = Form1.check(x + 2, y + 2);
                        if (b.Length == 1)
                            reds.Add(new int[4] { x + 2, y + 2, x + 1, y + 1 });
                    }
                }
            }
            else
            {
                //up left - -
                for (int i = 0; i < 8; i++)
                {
                    bool[] b = Form1.check(x - i, y - i);
                    if (b.Length == 2)
                    {
                        if(b[0] != player)
                        {
                            for(int j = i + 1;j < 8; j++)
                            {
                                b = Form1.check(x - j, y - j);
                                if (b.Length == 1)
                                    reds.Add(new int[4] { x - j, y - j, x - i, y - i });
                                else
                                    break;
                            }
                        }
                    }
                }
                //up right + -
                for (int i = 0; i < 8; i++)
                {
                    bool[] b = Form1.check(x + i, y - i);
                    if (b.Length == 2)
                    {
                        if (b[0] != player)
                        {
                            for (int j = i + 1; j < 8; j++)
                            {
                                b = Form1.check(x + j, y - j);
                                if (b.Length == 1)
                                    reds.Add(new int[4] { x + j, y - j, x + i, y - i });
                                else
                                    break;
                            }
                        }
                    }
                }
                //down left - +
                for (int i = 0; i < 8; i++)
                {
                    bool[] b = Form1.check(x - i, y + i);
                    if (b.Length == 2)
                    {
                        if (b[0] != player)
                        {
                            for (int j = i + 1; j < 8; j++)
                            {
                                b = Form1.check(x - j, y + j);
                                if (b.Length == 1)
                                    reds.Add(new int[4] { x - j, y + j, x - i, y + i });
                                else
                                    break;
                            }
                        }
                    }
                }
                //down right + +
                for (int i = 0; i < 8; i++)
                {
                    bool[] b = Form1.check(x + i, y + i);
                    if (b.Length == 2)
                    {
                        if (b[0] != player)
                        {
                            for (int j = i + 1; j < 8; j++)
                            {
                                b = Form1.check(x + j, y + j);
                                if (b.Length == 1)
                                    reds.Add(new int[4] { x + j, y + j, x + i, y + i });
                                else
                                    break;
                            }
                        }
                    }
                }
            }
            if (reds.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void clear()
        {
            greens.Clear();
            reds.Clear();
        }
        public void queen()
        {
            if (player)
            {
                if(y == 0)
                {
                    type = false;
                }
            }
            else
            {
                if(y == 7)
                {
                    type = false;
                }
            }
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
                foreach (var r in ch.reds)
                {
                    if(r[0]==x && r[1] == y)
                    {
                        ch.x = x;
                        ch.y = y;
                        Form1.delete_checker(r[2], r[3]);
                        ch.clear();
                        ch.queen();
                        ch.make_red();
                        if (ch.reds.Count == 0)
                        {
                            ch.make_gray();
                            end_turn();
                        }
                        return;
                    }
                }
                foreach(var g in ch.greens)
                {
                    if(g[0] == x && g[1] == y)
                    {
                        ch.x = x;
                        ch.y = y;
                        ch.clear();
                        ch.queen();
                        ch.make_gray();
                        end_turn();
                        return;
                    }
                }
                
            }
        }
        public void end_turn() 
        {
            if (Form1.player)
            {
                Form1.player = false;
            }
            else
            {
                Form1.player = true;
            }
            Form1.winning();
            Form1.mandatory();
        }
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