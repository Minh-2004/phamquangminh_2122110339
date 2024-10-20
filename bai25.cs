using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phamquangminh_2122110339
{
    public partial class bai25 : Form
    {
        PictureBox pb =new PictureBox();
        Timer tmGame = new Timer();
        int xBall=0;
        int yBall=0;
        int xDeltal = 5;
        int yDeltal = 5;
        public bai25()
        {
            InitializeComponent();
        }

        private void bai25_Load(object sender, EventArgs e)
        {
            tmGame.Interval = 10;
            tmGame.Tick += TmGame_Tick;
            tmGame.Start();
            pb.SizeMode =PictureBoxSizeMode.StretchImage;
            pb.Location = new Point(xBall, yBall);
            pb.Size = new Size(100, 100);
            this.Controls.Add(pb);
            pb.ImageLocation = @"D:\trung.png";

        }

        private void TmGame_Tick(object sender, EventArgs e)
        {
            xBall += xDeltal;
            yBall += yDeltal;
            if (xBall > this.ClientSize.Width - pb.Width || xBall <= 0)
                xDeltal=-xDeltal;
            if(yBall > this.ClientSize.Height - pb.Height || yBall <= 0)
                yDeltal=-yDeltal;
            pb.Location=new Point(xBall, yBall);
            
        }
    }
}
