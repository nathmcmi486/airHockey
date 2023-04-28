namespace airHockey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Rectangle player1Body = new Rectangle(10, 200, 50, 90);
        Rectangle player2Body = new Rectangle(700, 200, 50, 90);
        Rectangle puckBody = new Rectangle(370, 200, 20, 20);

        Rectangle barierRect = new Rectangle(800 / 2, 0, 10, 450);
        Rectangle p1Net = new Rectangle(0, 100, 40, 250);
        Rectangle p2Net = new Rectangle(760, 100, 40, 250);

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush blueBrush = new SolidBrush(Color.Blue);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush cyanBrush = new SolidBrush(Color.Cyan);

        int player1Score = 0;
        int player2Score = 0;

        int puckXS = 0;
        int puckYS = 0;

        int[] player1Movements = { 0, 0 };
        int[] player2Movement = { 0, 0 };

        bool wDown = false;
        bool sDown = false;
        bool aDown = false;
        bool dDown = false;

        // ha ha
        bool upDown = false;
        bool downDown = false;
        bool leftDown = false;
        bool rightDown = false;

        private void resetPuck()
        {
            puckXS = 0;
            puckYS = 0;

            puckBody.X = 370;
            puckBody.Y = 200;
        }

        private void checkGoal()
        {
            if (puckBody.IntersectsWith(p1Net))
            {
                player2Score += 1;
                resetPuck();
            }
            else if (puckBody.IntersectsWith(p2Net))
            {
                player1Score += 1;
                resetPuck();
            }
        }

        // Should be called when the puck collides with something, picks a random number to add/subtract
        // the puck[X/Y]S and multiplies it by -1 to change it's direction
        private void changePuckXYS()
        {
            Random random = new Random();

            if (player1Body.IntersectsWith(puckBody))
            {

                puckXS *= -1;
                puckXS *= -1;

                puckXS += random.Next(1, 2);
                puckYS += random.Next(1, 2);
            }
            else if (player2Body.IntersectsWith(puckBody))
            {

                puckXS *= -1;
                puckXS *= -1;

                puckXS += random.Next(-2, 1);
                puckYS += random.Next(-2, 1);
            }
            else if (puckBody.Y <= 0 + puckBody.Height || puckBody.Y >= this.Height - puckBody.Height)
            {
                puckYS *= -1;
            }
            else if (puckBody.X <= 0 || puckBody.X >= this.Width)
            {
                // If the puck goes off the screen reset it's postion (kind of like a real game (I don't watch hockey))
                puckXS *= -1;
                puckYS *= -1;
            }
        }

        private void stopPlayer()
        {
            if (player1Body.IntersectsWith(barierRect))
            {
                player1Body.X -= player1Body.Width;
            }
            else if (player2Body.IntersectsWith(barierRect))
            {
                player2Body.X += player2Body.Width;
            }
        }

        private void resetGame()
        {
            if (player1Score >= 5 || player2Score >= 5)
            {
                System.Threading.Thread.Sleep(1000);
                winnerLabel.Text += " Reseting...";
                this.Refresh();

                System.Threading.Thread.Sleep(2000);
                resetPuck();

                player1Body.X = 10;
                player1Body.Y = 200;
                player2Body.X = 700;
                player2Body.Y = 200;
                player1Score = 0;
                player2Score = 0;

                player1ScoreLabel.Text = "0";
                player2ScoreLabel.Text = "0";
                winnerLabel.Text = "";
            }
        }

        private void checkWinner()
        {
            if (player1Score == 5)
            {
                winnerLabel.Text = "Player 1 Wins!";
            }
            else if (player2Score == 5)
            {
                winnerLabel.Text = "Player 2 Wins!";
            }
        }

        private void mainLoop(object _o, EventArgs _e)
        {
            if (wDown == true)
            {
                player1Body.Y -= 3;
            }
            
            if (sDown == true)
            {
                player1Body.Y += 3;
            }

            if (aDown == true)
            {
                player1Body.X -= 3;
            }

            if (dDown == true)
            {
                player1Body.X += 3;
            }

            if (upDown == true)
            {
                player2Body.Y -= 3;
            }

            if (downDown == true)
            {
                player2Body.Y += 3;
            }

            if (leftDown == true)
            {
                player2Body.X -= 3;
            }

            if (rightDown == true)
            {
                player2Body.X += 3;
            }

            checkGoal();
            changePuckXYS();
            stopPlayer();
            checkWinner();

            puckBody.X += puckXS;
            puckBody.Y += puckYS;

            this.player1ScoreLabel.Text = $"{player1Score}";
            this.player2ScoreLabel.Text = $"{player2Score}";

            player1Body.X += player1Movements[0];
            player1Body.Y += player1Movements[1];

            player2Body.X += player2Movement[0];
            player2Body.Y += player2Movement[1];

            this.Refresh();

            player1Movements[0] = 0;
            player1Movements[1] = 0;
            player2Movement[0] = 0;
            player2Movement[1] = 0;

            resetGame();
        }

        private void paintHandler(object _o, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(cyanBrush, p1Net);
            e.Graphics.FillRectangle(cyanBrush, p2Net);
            e.Graphics.FillRectangle(blackBrush, barierRect);
            e.Graphics.FillRectangle(redBrush, player1Body);
            e.Graphics.FillRectangle(blueBrush, player2Body);
            e.Graphics.FillRectangle(blackBrush, puckBody);
        }

        private void keyHandler(object _o, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    player1Movements[1] = -3;
                    break;
                case Keys.S:
                    player1Movements[1] = 3;
                    break;
                case Keys.A:
                    player1Movements[0] = -3;
                    break;
                case Keys.D:
                    player1Movements[0] = 3;
                    break;
                case Keys.Up:
                    player2Movement[1] = -3;
                    break;
                case Keys.Down:
                    player2Movement[1] = 3;
                    break;
                case Keys.Left:
                    player2Movement[0] = -3;
                    break;
                case Keys.Right:
                    player2Movement[0] = 3;
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }
    }
}

