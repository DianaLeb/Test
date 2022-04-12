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
    public partial class Form1old
    {
        #region global
        bool player = true;
        int size;
        int[,] matrix = new int[8, 8];
        int whites = 12;
        int blacks = 12;
        bool win = false;
        bool step = true;
        bool red = false;
        Image ch_black = Properties.Resources._4;
        Image ch_white = Properties.Resources._5;
        Image qw_black = Properties.Resources._2;
        Image qw_white = Properties.Resources._3;
        #endregion
        private void start()
        {
            //toolStripStatusLabel1.Text = "White";
            //int size = panel1.Width / 8;
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
                            matrix[x, y] = 3;
                        }
                        else if (y > 4)
                        {
                            matrix[x, y] = 1;
                        }
                        else
                        {
                            matrix[x, y] = 0;
                        }
                    }
                    //pic.MouseClick += Pic_MouseClick;
                    //panel1.Controls.Add(pic);
                }
            }
            player = true;
            //toolStripStatusLabel1.Text = "White";
            draw();
        }

        private void draw()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        PictureBox pic = null; //= panel1.Controls[$"{x}_{y}"] as PictureBox;
                        pic.BackColor = Color.Gray;
                        switch (matrix[x, y])
                        {
                            case 0:
                                pic.Image = null;
                                break;
                            case 1:
                                pic.Image = ch_white;
                                break;
                            case 2:
                                pic.Image = qw_white;
                                break;
                            case 3:
                                pic.Image = ch_black;
                                break;
                            case 4:
                                pic.Image = qw_black;
                                break;
                            case 5:
                                pic.BackColor = Color.Green;
                                break;
                            case 6:
                                pic.BackColor = Color.Red;
                                break;
                        }
                        if (matrix[x, y] < 0)
                        {
                            pic.BackColor = Color.Yellow;
                        }
                    }
                }
            }
        }

        private void Pic_MouseClick2(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            string[] s = pic.Name.Split('_');
            int x = int.Parse(s[0]);
            int y = int.Parse(s[1]);
            //label1.Text = $"Click on {pic.Name}";

            if (matrix[x, y] >= 5)
            {
                move(x, y);
                return;
            }
            //old_pic = pic;
            if (player && matrix[x, y] >= 1 && matrix[x, y] <= 2)
            {
                right_click(x, y, false);
            }
            else if (!player && matrix[x, y] >= 3 && matrix[x, y] <= 4)
            {
                right_click(x, y, false);
            }
        }
        private void right_click(int x, int y, bool swp)
        {

            clear_matrix();
            matrix[x, y] *= -1;
            steps(x, y);
            draw();
        }

        private void clear_matrix()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
                    {
                        matrix[x, y] = Math.Abs(matrix[x, y]);
                        if (matrix[x, y] >= 5)
                        {
                            matrix[x, y] = 0;
                        }
                    }
                }
            }
        }

        private void delete_chess(int x, int y, int r_x, int r_y)
        {
            if (player)
            {
                blacks--;
            }
            else
            {
                whites--;
            }
            winning();
            if (x > r_x && y > r_y)
            {
                for (int i = x, j = y; i > 0 && j > 0; i--, j--)//right up
                {
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        matrix[i, j] = 0;
                        return;
                    }
                }
            }
            if (x < r_x && y > r_y)
            {
                for (int i = x, j = y; i < 8 && j > 0; i++, j--)//left up
                {
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        matrix[i, j] = 0;
                        return;
                    }
                }
                //matrix[x + 1, y - 1] = 0;
            }
            if (x > r_x && y < r_y)
            {
                for (int i = x, j = y; i > 0 && j < 8; i--, j++)//right down
                {
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        matrix[i, j] = 0;
                        return;
                    }
                }
                //matrix[x - 1, y + 1] = 0;
            }
            if (x < r_x && y < r_y)
            {
                for (int i = x, j = y; i < 8 && j < 8; i++, j++)//left down
                {
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        matrix[i, j] = 0;
                        return;
                    }
                }
                //matrix[x + 1, y + 1] = 0;
            }

        }

        private void steps(int x, int y)
        {
            if (!step && (matrix[x, y] == -1 || matrix[x, y] == -3))
            {
                make_green(x - 1, y - 1, step);
                make_green(x + 1, y - 1, step);
                make_green(x - 1, y + 1, step);
                make_green(x + 1, y + 1, step);
            }
            else if (matrix[x, y] == -1)
            {
                make_green(x - 1, y - 1, true);
                make_green(x + 1, y - 1, true);
                make_green(x - 1, y + 1, false);
                make_green(x + 1, y + 1, false);
            }
            else if (matrix[x, y] == -3)
            {
                make_green(x - 1, y + 1, true);
                make_green(x + 1, y + 1, true);
                make_green(x - 1, y - 1, false);
                make_green(x + 1, y - 1, false);
            }
            else if (matrix[x, y] == -2 || matrix[x, y] == -4)
            {
                for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)//left up
                {
                    make_green(i, j, step);
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        break;
                    }
                }
                for (int i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--)//right up
                {
                    make_green(i, j, step);
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        break;
                    }
                }
                for (int i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)//left down
                {
                    make_green(i, j, step);
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        break;
                    }
                }
                for (int i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)//right down
                {
                    make_green(i, j, step);
                    if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                    {
                        break;
                    }
                }
            }
        }

        private void make_green(int x, int y, bool forward)
        {
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
            {

                if (matrix[x, y] == 0 && forward)
                {
                    matrix[x, y] = 5;
                }
                else
                {
                    if (player && (matrix[x, y] == 3 || matrix[x, y] == 4))
                    {
                        make_red(x, y);
                    }
                    if (!player && (matrix[x, y] == 1 || matrix[x, y] == 2))
                    {
                        make_red(x, y);
                    }
                }
            }
        }
        private void make_red(int r_x, int r_y)
        {
            red = true;
            int x = 0, y = 0;
            for (; x < 8; x++)
            {
                for (y = 0; y < 8; y++)
                {
                    if (matrix[x, y] < 0)
                    {
                        x += 8;
                        break;
                    }
                }
            }
            x -= 9;
            if (matrix[x, y] == -1 || matrix[x, y] == -3)
            {
                x = r_x - x + r_x;
                y = r_y - y + r_y;
                if (x >= 0 && x < 8 && y >= 0 && y < 8)
                {
                    if (matrix[x, y] == 0)
                    {
                        matrix[x, y] = 6;
                    }
                }
            }
            else
            {
                if (x > r_x && y > r_y)//left up
                {
                    for (int i = r_x - 1, j = r_y - 1; i >= 0 && j >= 0; i--, j--)
                    {
                        if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                        {
                            break;
                        }
                        else
                        {

                            matrix[i, j] = 6;
                        }
                    }
                }
                if (x < r_x && y > r_y)//right up
                {
                    for (int i = r_x + 1, j = r_y - 1; i < 8 && j >= 0; i++, j--)
                    {
                        if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                        {
                            break;
                        }
                        else
                        {
                            matrix[i, j] = 6;
                        }
                    }
                }
                if (x > r_x && y < r_y)//left down
                {
                    for (int i = r_x - 1, j = r_y + 1; i >= 0 && j < 8; i--, j++)//left down
                    {
                        if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                        {
                            break;
                        }
                        else
                        {
                            matrix[i, j] = 6;
                        }
                    }
                }
                if (x < r_x && y < r_y)//right down
                {
                    for (int i = r_x + 1, j = r_y + 1; i < 8 && j < 8; i++, j++)//right down
                    {
                        if (matrix[i, j] >= 1 && matrix[i, j] <= 4)
                        {
                            break;
                        }
                        else
                        {
                            matrix[i, j] = 6;
                        }
                    }
                }
            }
        }

        private void move(int g_x, int g_y)
        {
            int x = 0, y = 0;
            for (; x < 8; x++)
            {
                for (y = 0; y < 8; y++)
                {
                    if (matrix[x, y] < 0)
                    {
                        x += 8;
                        break;
                    }
                }
            }
            x -= 9;
            bool eated = false;
            if (matrix[g_x, g_y] == 6)
            {
                delete_chess(x, y, g_x, g_y);
                eated = true;
            }
            if (!win)
            {
                matrix[g_x, g_y] = matrix[x, y];
                matrix[x, y] = 0;
                clear_matrix();
                make_queen(g_x, g_y);
                draw();
                if (eated)
                {
                    step = false;
                    right_click(g_x, g_y, false);
                    bool s = true;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (i % 2 == 1 && j % 2 == 0 || i % 2 == 0 && j % 2 == 1)
                            {
                                if (matrix[i, j] == 6)
                                {
                                    s = false;
                                    i = 8;
                                    break;
                                }
                            }
                        }
                    }
                    if (s)
                    {
                        step = true;
                        swap();
                    }
                }
                else
                {
                    swap();

                }
            }
            else
            {
                win = false;
            }
        }
        private void swap()
        {
            if (player)
            {
                player = false;
                //toolStripStatusLabel1.Text = "Black";
            }
            else
            {
                player = true;
                //toolStripStatusLabel1.Text = "White";
            }
            //step = false;
            //for (int x = 0; x < 8; x++)
            //{
            //    for (int y = 0; y < 8; y++)
            //    {
            //        if (x % 2 == 1 && y % 2 == 0 || x % 2 == 0 && y % 2 == 1)
            //        { 
            //            if(player && (matrix[x, y] == 1 || matrix[x, y] == 2))
            //            {
            //               // right_click(x, y, false);
            //            }
            //            else if(!player && (matrix[x, y] == 3 || matrix[x, y] == 4))
            //            {

            //            }
            //        }
            //    }
            //}
            //step = true;
        }
        private void make_queen(int x, int y)
        {
            if (y == 0 && matrix[x, y] == 1)
            {
                matrix[x, y] = 2;
            }
            else if (y == 7 && matrix[x, y] == 3)
            {
                matrix[x, y] = 4;
            }
        }

        private void winning()
        {
            if (whites == 0)
            {
                MessageBox.Show("Black is a winner");
                start();
                win = true;
            }
            else if (blacks == 0)
            {
                MessageBox.Show("White is a winner");
                start();
                win = true;
            }
        }

        private void button2_Click2(object sender, EventArgs e)
        {
            if (player)
            {
                whites = 0;
            }
            else
            {
                blacks = 0;
            }
            winning();
        }
    }
}
