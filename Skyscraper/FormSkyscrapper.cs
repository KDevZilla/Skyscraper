using Skyscraper.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyscraper
{
    public partial class FormSkyscrapper : Form, IUI
    {
        public FormSkyscrapper()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;
            this.KeyDown -= Form1_KeyDown1;
            this.KeyDown += Form1_KeyDown1;
            NewGame(4);
           // this.Initial(4);
           // this.ShowFin();

        }



        private void OnUIKeyNavigatorDown(KeyEventArgs e)
        {
             this.UIKeyNavigatorDown?.Invoke(this, e);
        }
        private void OnUIKeyNumberDown(UI.IntegerEventArgs e)
        {
            this.UIKeyNumberDown?.Invoke(this, e);
        }
        /*
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        */

        private void HandleKeyDown(KeyEventArgs e,int BoardSize)
        {
            if (e.KeyCode == Keys.Left ||
             e.KeyCode == Keys.Right ||
             e.KeyCode == Keys.Up ||
             e.KeyCode == Keys.Down)
            {
                KeyEventArgs key = new KeyEventArgs(e.KeyData);
                this.OnUIKeyNavigatorDown(key);
                return;
            }



            int keyValueDMin = (int)Keys.D1;
            int keyValueDMax = (int)Keys.D6;

            keyValueDMax =UI.UIUtility.GetKeyValueDMax(BoardSize);


            int keyValueNumMin = (int)Keys.NumPad1;
            int keyValueNumMax = (int)Keys.NumPad2;
            keyValueNumMax = UI.UIUtility.GetKeyValueNumpadMax(BoardSize);

            Boolean IsValidDKey = true;
            Boolean IsValidNumKey = true;
            int keyCode = (int)e.KeyCode;
            if (keyCode < keyValueDMin || keyCode > keyValueDMax)
            {
                IsValidDKey = false;
            }
            if (keyCode < keyValueNumMin || keyCode > keyValueNumMax)
            {
                IsValidNumKey = false;
            }

            if ((!IsValidDKey && !IsValidNumKey))
            {
                return;
            }

            int keyEnter = 0;
            if (IsValidDKey)
            {
                keyEnter = keyCode - keyValueDMin + 1;
            }
            else
            {
                keyEnter = keyCode - keyValueNumMin + 1;
            }
            UI.IntegerEventArgs KeyEnterEvent = new IntegerEventArgs(keyEnter);
            
            this.OnUIKeyNumberDown(KeyEnterEvent);

        }
       
        private void Form1_KeyDown1(object sender, KeyEventArgs e)
        {

            HandleKeyDown(e,this.game.ColumnSize);
        }



        private void button1_Click(object sender, EventArgs e)
        {

        }

        private DateTime _TimeBegin;
        private DateTime _TimeEnd;

        private PictureBoxBoard picboard = null;
        private PictureBoxBoard picboardFin = null;
        private Game game = null;

        public event PictureBoxBoard.PictureBoxCellClick CellClick;
        public event EventHandler UIKeyNumberDown;
        public event EventHandler UIKeyNavigatorDown;
       
        public void NewGame(int BoardSize)
        {
            if (game != null)
            {
                if (!game.board.IsFinshed)
                {
                    if (MessageBox.Show("Do you want to end this game ?", "", MessageBoxButtons.OKCancel)
                        != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            this.Initial(BoardSize);
        }
       
        public void Initial(int BoardSize)
        {
            if (this.Controls.Contains(picboard))
            {
                picboard.CellClick -= Picboard_CellClick;
                this.Controls.Remove(picboard);
            }

            if (this.Controls.Contains(picboardFin))
            {
                this.Controls.Remove(picboardFin);
            }

            if (game != null)
            {
                game.ReleaseResource();
            }


            // int BaseNumberofBlackCell = DictionaryBoadsizeNumberofBlackCell[BoardSize];
            //    int RandomNumberofBlackCell = Utility.Random(1, BoardSize);
            //KakurasuBoard.IBoardGenerator BoardGen = new KakurasuBoard.BasicGenerator();
            int[,] arr2d = {
                    {3,2,1,4 },
                    {2,3,4,1 },
                    {4,1,2,3 },
                    {1,4,3,2 }
                };
            Board.IBoardGenerator BoardGen = new Board.BasicGenerator(Utility.PreGenerateDigitsPath);
           // BoardGen = new Board.ManualGenerator(arr2d);

            game = new Game(this, BoardSize, BoardSize, BoardGen);
            picboard = new PictureBoxBoard(game.board, false);
            picboard.CellClick -= Picboard_CellClick;
            picboard.CellClick += Picboard_CellClick;
            picboard.Top = this.menuStrip1.Height + 10;
            picboard.Left = 0;
            this.Controls.Add(picboard);
            this.Height = picboard.Height + picboard.Top + 40;
            this.Width = picboard.Width + picboard.Left + 15;
            _TimeBegin = DateTime.Now;
            IsGiveup = false;

        }
        private void Picboard_CellClick(object sender, Position position)
        {
            //throw new NotImplementedException();
            CellClick?.Invoke(this, position);
        }


        public void RenderBoard()
        {
            if (game.board == null)
            {
                return;
                
            }
            if (picboard == null)
            {
                return;
            }
            picboard.Invalidate();



        }

        public void SetGame(Game pgame)
        {
            this.game = pgame;

           
        }

        public void ReleaseResource()
        {
            // throw new NotImplementedException();
            picboard.CellClick -= Picboard_CellClick;
        }

        public void InformUserWon()
        {
            // throw new NotImplementedException();
            _TimeEnd = DateTime.Now;


            TimeSpan T = _TimeEnd - _TimeBegin;
            String Wording = "";
            Wording = "Congreatulations, you solved this puzzle. It took you " + T.Minutes.ToString("00") + ":" + T.Seconds.ToString("00") + " to finish.";
            MessageBox.Show(Wording);

        }
        Boolean IsGiveup = false;
        public void ShowFin()
        {

            if (this.Controls.Contains(picboardFin))
            {

                this.Controls.Remove(picboardFin);
            }

            this.game.board.GiveUp();
            picboardFin = new PictureBoxBoard(this.game.board, true);
            picboardFin.Top = this.picboard.Top;
            picboardFin.Left = this.picboard.Left + this.picboard.Width + 50;
            this.Controls.Add(picboardFin);
            this.Width = picboardFin.Width + picboardFin.Left + 17;
            IsGiveup = true;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            NewGame(4);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(5);
        }

        private void x6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(6);
        }

        private void x7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(7);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (IsGiveup)
            {
                return;
            }
            if (game.board.IsFinshed)
            {
                return;
            }
            this.ShowFin();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }
            Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormAbout f = new FormAbout();
            f.ShowDialog();

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormHowToPlay f = new FormHowToPlay();
            f.ShowDialog();

        }
    }
}
