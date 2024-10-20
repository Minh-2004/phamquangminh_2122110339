using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace phamquangminh_2122110339
{
    
    public partial class bai28 : Form
    {
        PictureBox pbBasket=new PictureBox();
        PictureBox pbEgg=new PictureBox();
        PictureBox pbChicken=new PictureBox(); 
        PictureBox pbBom=new PictureBox();
        Timer tmrBom = new Timer();
        Timer tmEgg=new Timer();
        Timer tmChicken=new Timer();
       
        Timer tmExplosion = new Timer();
        Timer tmStartBom = new Timer();
        

        int xBasket = 300;
        int yBasket = 285;
        int xDeltaBasket = 30;

        int xChicken = 300;
        int yChicken = 10;
        int xDeltaChicken = 3;

        int xEgg = 300;
        int yEgg = 10;
        int yDeltaEgg = 2;

        int xBom = 300;
        int yBom = 10;
        int yDeltaBom = 2;

        bool isEggBroken = false;//sự kiện trứng vỡ
        bool isBomBroken = false;//sự kiện bom nổ

        int score = 0;
        int second = 0;

        public bai28()
        {
            InitializeComponent();
        }

        private void bai28_Load(object sender, EventArgs e)
        {
            
            //this.BackgroundImage = Image.FromFile("../../Images/bg.png");
            //this.BackgroundImageLayout = ImageLayout.Stretch;
            tmStartBom.Interval = 5000;  // 5000 milliseconds = 5 seconds
    
             tmStartBom.Start();
            
            //thời gian của trứng rơi
            tmEgg.Interval = 30;
            tmEgg.Tick += TmEgg_Tick;
            tmEgg.Start();
            //Thời gian bom rơi
            tmrBom.Interval = 25;
            tmrBom.Tick += TmBom_Tick;
           
            //thời gian gà di chuyển
            tmChicken.Interval = 30;
            tmChicken.Tick += TmChicken_Tick;
            tmChicken.Start();
            //Thời Gian
            tmStopwatch.Interval = 1000;
            tmStopwatch.Tick += TmStopwatch_Tick;
            tmStopwatch.Start();
            //sự kiện của giỏ trứng
            pbBasket.SizeMode=PictureBoxSizeMode.StretchImage;
            pbBasket.Size = new Size(60, 60);
            pbBasket.Location = new Point(xBasket, yBasket);    
            pbBasket.BackColor = Color.Transparent;
            this.Controls.Add(pbBasket);
            pbBasket.Image = Image.FromFile("../../Images/basket.png");
            //thuộc tính của trứng
            pbEgg.SizeMode= PictureBoxSizeMode.StretchImage;
            pbEgg.Size = new Size(40,40);
            pbEgg.Location = new Point(xEgg, yEgg);
            pbEgg.BackColor = Color.Transparent;
            this.Controls.Add(pbEgg);
            pbEgg.Image = Image.FromFile("../../Images/egg.png");
            //thuộc tính gà
            pbChicken.SizeMode= PictureBoxSizeMode.StretchImage;
            pbChicken.Size = new Size(65,65);
            pbChicken.Location = new Point(xChicken, yChicken);
            pbChicken.BackColor = Color.Transparent;
            this.Controls.Add (pbChicken);
            pbChicken.Image = Image.FromFile("../../Images/chicken.png");
            //thuộc tính của bom
            pbBom.SizeMode= PictureBoxSizeMode.StretchImage;
            pbBom.Size= new Size(40,40);
            pbBom .BackColor = Color.Transparent;
            this.Controls.Add(pbBom);
           
            
            

            pbBasket.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        }

       



        private void TmBom_Tick(object sender, EventArgs e)
        {
            yBom += yDeltaBom;

            Rectangle unionRect = Rectangle.Intersect(pbBom.Bounds, pbBasket.Bounds);
            if (unionRect.IsEmpty == false)
            {
                // Bom chạm giỏ
                tmrBom.Stop();
                tmEgg.Stop();
                tmChicken.Stop();

                // Hiển thị ảnh bom nổ
                pbBom.Image = Image.FromFile("../../Images/bomno.png");

                // Bắt đầu Timer để hiển thị ảnh nổ trong 1 giây
                tmExplosion.Interval = 1000;  // 1000 milliseconds = 1 second
                tmExplosion.Tick += TmExplosion_Tick;
                tmExplosion.Start();
            }

            pbBom.Location = new Point(xBom, yBom);
        }

        private void TmExplosion_Tick(object sender, EventArgs e)
        {
            // Dừng tất cả các Timer đang chạy (trứng, bom, gà, đồng hồ...)
            tmEgg.Stop();
            tmrBom.Stop();
            tmChicken.Stop();
            tmStopwatch.Stop();

            // Dừng giỏ (không nhận sự kiện từ bàn phím nữa)
            this.KeyPreview = false;

            // Hiển thị ảnh bom nổ
            pbBom.Image = Image.FromFile("../../Images/bomno.png");

            // Dừng việc hiển thị bom nổ sau 1 giây, và chuyển sang màn hình trắng
            tmExplosion.Stop();

            // Đổi màu nền thành trắng
            this.BackColor = Color.White;

            // Ẩn các đối tượng trên màn hình
            pbBasket.Visible = false;
            pbEgg.Visible = false;
            pbChicken.Visible = false;
            pbBom.Visible = false;

            ShowEndGameOptions();   
        }



        private void TmStopwatch_Tick(object sender, EventArgs e)
        {
            second++;

            lblDisplay.Text = second.ToString();

            if (second % 10 == 0)
            {
                // Chỉ hiển thị bom sau khi đã bắt trứng
                if (second % 10 == 0 && !isEggBroken)
                {
                    // Logic to set the bomb's position should only happen after catching an egg
                    Random random = new Random();
                    int offsetX = random.Next(-30, 30); // Random offset to position bomb
                    xBom = xEgg + offsetX; // Position bomb near egg
                    yBom = pbChicken.Location.Y + pbChicken.Height; // Above chicken

                    // Make the bomb visible and set its initial position
                    pbBom.Location = new Point(xBom, yBom);
                    pbBom.Image = Image.FromFile("../../Images/bom.png");
                    pbBom.Visible = true; // Make bomb visible

                    tmrBom.Start(); // Start the bomb falling
                }

                if (isEggBroken)
                    tmStopwatch.Stop();
            }
        }

        private void TmChicken_Tick(object sender, EventArgs e)
        {
            if (isEggBroken)
            {
                return;  
            }
            xChicken += xDeltaChicken;
          
            if (xChicken > this.ClientSize.Width - pbChicken.Width || xChicken <= 0)
            {
                xDeltaChicken =- xDeltaChicken;
            }
            pbChicken.Location = new Point(xChicken, yChicken);
            
        }

        private void TmEgg_Tick(object sender, EventArgs e)
        {
            yEgg += yDeltaEgg;

            if (yEgg > this.ClientSize.Height - pbEgg.Height || yEgg <= 0)
            {
                // Trứng rơi xuống đất, hiển thị trứng vỡ
                pbEgg.Image = Image.FromFile("../../Images/broken_egg.png");
                isEggBroken = true;

                tmEgg.Stop(); // Dừng trứng rơi khi trứng vỡ

                // Call the method to show end game options
                ShowEndGameOptions();

                return;
            }

            Rectangle unionRect = Rectangle.Intersect(pbEgg.Bounds, pbBasket.Bounds);
            if (unionRect.IsEmpty == false)
            {
                // Trứng được bắt
                score++;
                lbDiem.Text = "Điểm: " + score;

                // Tăng tốc độ rơi của trứng và di chuyển gà nhanh hơn khi đạt điểm nhất định
                if (score % 10 == 0)
                {
                    xDeltaChicken += 1;
                    yDeltaEgg += 1;
                }

                // Đặt lại vị trí trứng sau khi được bắt
                yEgg = pbChicken.Location.Y + pbChicken.Height;
                xEgg = pbChicken.Location.X + (pbChicken.Width / 2) - (pbEgg.Width / 2);
                isEggBroken = false;
                pbEgg.Image = Image.FromFile("../../Images/egg.png");

                // Khởi động lại trứng rơi
                tmEgg.Start();
            }

            pbEgg.Location = new Point(xEgg, yEgg);
        }
        private Button btnPlayAgain = new Button();
        private Button btnExit = new Button();
        private void ShowEndGameOptions()
        {
            // Tạo nút "Chơi lại"
            btnPlayAgain.Text = "Chơi lại";
            btnPlayAgain.Size = new Size(100, 50);
            btnPlayAgain.Location = new Point(this.ClientSize.Width / 2 - 110, this.ClientSize.Height / 2);
            btnPlayAgain.Click += BtnPlayAgain_Click;
            this.Controls.Add(btnPlayAgain);

            // Tạo nút "Thoát"
            btnExit.Text = "Thoát";
            btnExit.Size = new Size(100, 50);
            btnExit.Location = new Point(this.ClientSize.Width / 2 + 10, this.ClientSize.Height / 2);
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnExit);
            //
            lblScore.Text = "Điểm của bạn: " + score;
            lblScore.Size = new Size(200, 30);
            lblScore.Location = new Point(this.ClientSize.Width / 2 - 100, this.ClientSize.Height / 2 - 30);
            lblScore.Font = new Font("Arial", 14, FontStyle.Bold);
            lblScore.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblScore);
        }
        private Label lblScore = new Label();

        // Xử lý khi nhấn nút "Chơi lại"
        private void BtnPlayAgain_Click(object sender, EventArgs e)
        {
            // Khởi động lại trò chơi (có thể khởi tạo lại form)
            Application.Restart();
        }

        // Xử lý khi nhấn nút "Thoát"
        private void BtnExit_Click(object sender, EventArgs e)
        {
            // Thoát ứng dụng
            Application.Exit();
        }
        private void bai28_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                if (xBasket < this.ClientSize.Width - pbBasket.Width)
                    xBasket += xDeltaBasket;
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (xBasket > 0)
                    xBasket -= xDeltaBasket;
            }

            pbBasket.Location = new Point(xBasket, yBasket);
        }

        
    }
}
