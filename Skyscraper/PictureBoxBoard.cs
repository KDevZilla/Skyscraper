using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Skyscraper.Board;

namespace Skyscraper
{
    public class PictureBoxBoard : PictureBox
    {
       
        
        private Board _BoardData = null;
        private Board BoardData
        {
            get
            {
                return _BoardData;
            }
        }
        
        private int CellSize = 60;
        //  public int NoofRow { get; private set; }
        //  public int NoofCole { get; private set; }
        private Rectangle[,] _arrRac = null;
        int RowSize = 5;
        int ColumnSize = 5;
        private Rectangle[,] arrRac
        {
            get
            {
                if (_arrRac == null)
                {
                    int i, j;
                    _arrRac = new Rectangle[RowSize + 2, ColumnSize + 2];
                    for (i = 0; i < RowSize + 2; i++)
                    {
                        for (j = 0; j < ColumnSize + 2; j++)
                        {
                            int IndexArrRec = (i *RowSize) + j;
                            _arrRac[i, j] = new Rectangle(CellSize * j, CellSize * i, CellSize, CellSize);
                        }
                    }
                }
                return _arrRac;
            }

        }
        private int ActualRowSize
        {
            get { return this.RowSize + 2; }
        }
        private int AcutalColumnSize
        {
            get { return this.ColumnSize + 2; }
        }
        public PictureBoxBoard(Board pBoardData, bool IsRenderFinMode)
        {
            this.IsRenderFinMode = IsRenderFinMode;
            this._BoardData = pBoardData;
            this.RowSize = this.BoardData.RowSize;
            this.ColumnSize = this.BoardData.RowSize;

            this.Paint -= PictureBoxBoard_Paint;
            this.Paint += PictureBoxBoard_Paint;
            this.Height = CellSize * this.ActualRowSize +2;
            this.Width = CellSize * this.AcutalColumnSize + 2;
            this.BorderStyle = BorderStyle.None;

            this.MouseDown -= PictureBoxBoard_MouseDown;
            this.MouseDown += PictureBoxBoard_MouseDown;

        }
        public delegate void PictureBoxCellClick(object sender, Position position);
        public event PictureBoxCellClick CellClick;

        private void PictureBoxBoard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            int X = e.X;
            int Y = e.Y;
            int RowClick = Y / CellSize;
            int ColClick = X / CellSize;
            if (RowClick > RowSize ||
                ColClick > ColumnSize ||
                RowClick < 1 ||
                ColClick < 1)
            {
                return;
            }
            RowClick -= 1;
            ColClick -= 1;
            if (CellClick != null)
            {
                CellClick(this, new Position(RowClick, ColClick));
            }
        }
        enum ArrowDirection
        {
            Up,
            Right,
            Left,
            Down
        }
        private Point[] GetPolyPoint(ArrowDirection direction, Rectangle r)
        {
            return GetPolyPoint(direction, r.X, r.Y, r.Height, r.Width);
        }
        private Point[] GetPolyPoint(ArrowDirection direction, int X, int Y, int CellHeight, int CellWidth)
        {
            int Width = 40;
            int Height = 40;
           // int XBegin = X + ((CellWidth - Width)/2);
            int YBegin = Y + ((CellHeight - Height) / 2);
            int XBegin = X;

            

            Point[] arrPoint = null;
            switch (direction)
            {
                case ArrowDirection.Up:
                    YBegin = Y + ((CellHeight - Height)) ;
                    XBegin = X + ((CellWidth - Width) / 2);
                    arrPoint = new Point[] { new Point(XBegin, YBegin + Height ),
                        new Point(XBegin , YBegin ),
                        new Point(XBegin + (Width / 2), YBegin  - (Height / 2)),
                        new Point(XBegin + Width, YBegin ),
                        new Point(XBegin + Width , YBegin + Height )
                    };
                    break;
                case ArrowDirection.Right:
                    arrPoint = new Point[] { new Point(XBegin, YBegin),
                        new Point(XBegin + Width, YBegin),
                        new Point(XBegin + Width + (Width / 2), YBegin + (Height / 2)),
                        new Point(XBegin + Width, YBegin + Height),
                        new Point(XBegin, YBegin + Height)
                    };
                    break;
                case ArrowDirection.Down:
                    YBegin = Y;
                    XBegin = X + ((CellWidth  - Width ) / 2);
                    arrPoint = new Point[] { new Point(XBegin, YBegin),
                        new Point(XBegin , YBegin + Height),
                        new Point(XBegin + (Width / 2), YBegin + Height + (Height / 2)),
                        new Point(XBegin + Width, YBegin + Height),
                        new Point(XBegin + Width , YBegin )
                    };
                    break;
                case ArrowDirection.Left:
                    arrPoint = new Point[] { new Point(XBegin + CellWidth , YBegin),
                        new Point(XBegin + CellWidth - Width, YBegin),
                        new Point(XBegin + CellWidth - Width - (Width / 2), YBegin + (Height / 2)),
                        new Point(XBegin + CellWidth - Width, YBegin + Height),
                        new Point(XBegin + CellWidth ,   YBegin + Height)
                    };


                    break;
            }

           // polyPoints = arrPoint.ToList();
            return arrPoint;
        }
        private void DrawArrow(Graphics g,Rectangle [,] arrRac)
        {
            Pen PenBorder = new Pen(Color.Black, 1.5f);
            Pen PenTest = new Pen(Color.Orange, 1);
            int i;
            int j;
            Point[] polyPoints = null;
            SolidBrush br = new SolidBrush(Color.FromArgb(100, Color.Yellow));
            for (i = 1; i < RowSize  + 1; i++)
            {
                Rectangle r = arrRac[i, 0];
                // List<Point> polyPoints = new List<Point>();

                polyPoints = GetPolyPoint(ArrowDirection.Right,r);
           
                
                g.FillPolygon(br, polyPoints);
                g.DrawPolygon(Pens.DarkBlue, polyPoints);

                r = arrRac[i, ColumnSize + 1];
                polyPoints = GetPolyPoint(ArrowDirection.Left, r);
                g.FillPolygon(br, polyPoints);
                g.DrawPolygon(Pens.DarkBlue, polyPoints);
            }

            for (i = 1; i < ColumnSize + 1; i++)
            {
                Rectangle r = arrRac[0, i];
              

                polyPoints = GetPolyPoint(ArrowDirection.Down , r);


                g.FillPolygon(br, polyPoints);
                g.DrawPolygon(Pens.DarkBlue, polyPoints);

                r = arrRac[RowSize + 1, i];
                polyPoints = GetPolyPoint(ArrowDirection.Up , r);
                g.FillPolygon(br, polyPoints);
                g.DrawPolygon(Pens.DarkBlue, polyPoints);
            }
        }
        private void DrawCurrentCell(Graphics g,Rectangle rec)
        {
            Rectangle r = rec;
           
            r.X += 2;
            r.Y += 2;
            r.Width -= 4;
            r.Height -= 4;
            
            //using (Pen PenTest = new Pen(Color.Orange, 1)) {
            using(Brush sBrush =new SolidBrush(Color.Orange)) { 
            //g.DrawRectangle(PenTest, r);
            g.FillRectangle(sBrush, r);
            }
        }
        private Rectangle  GetSmallRectangle(Rectangle orignalRec)
        {
            Rectangle r = orignalRec;
            
            r.X += 10;
            r.Y += 10;
            r.Width -= 20;
            r.Height -= 20;
          
            return r;

        }
        private void DrawBoard(Graphics g, Rectangle[,] arrRac)
        {

            
            //Pen PenTest = new Pen(Color.Orange, 1);
            int i;
            int j;
            for (i = 1; i < RowSize + 1; i++)
            {
                for (j = 1; j < ColumnSize + 1; j++)
                {
                    Rectangle r = GetSmallerRectangle(arrRac[i, j]);

                    using (Pen PenBorder = new Pen(Color.Black, 1.5f))
                    {
                        g.DrawRectangle(PenBorder, r);
                    }
                    
                   // g.DrawRectangle(PenTest, arrRac[i, j]);
                }
            }
            /*
            for (i = 0; i < RowSize+2; i++)
            {
                for (j = 0; j < ColumnSize+ 2; j++)
                {
                    g.DrawRectangle(PenTest, arrRac[i, j]);
                }
            }
            */


        }

        private Font _AnswerFont = null;
        private Font AnswerFont
        {
            get
            {
                if (_AnswerFont == null)
                {
                    _AnswerFont = new System.Drawing.Font("Segoe UI",
                        16F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                return _AnswerFont;
            }
        }
        private Font _SumFont = null;
        private Font SumFont
        {
            get
            {
                if (_SumFont == null)
                {
                    _SumFont = new System.Drawing.Font("Segoe UI",
                        16F,
                        System.Drawing.FontStyle.Bold,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                return _SumFont;
            }
        }
        private Font _WeightFont = null;
        private Font WeightFont
        {
            get
            {
                if (_WeightFont == null)
                {
                    _WeightFont = new System.Drawing.Font("Segoe UI",
                        12F,
                        System.Drawing.FontStyle.Regular,
                        System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                return _WeightFont;
            }
        }
        private Brush _SumBrushCorrect = null;
        private Brush SumBrushCorrect
        {
            get
            {
                if (_SumBrushCorrect == null)
                {
                    _SumBrushCorrect = new SolidBrush(Color.Green);
                }
                return _SumBrushCorrect;
            }
        }
        private Brush _SumBrushWrong = null;
        private Brush SumBrushWrong
        {
            get
            {
                if (_SumBrushWrong == null)
                {
                    _SumBrushWrong = new SolidBrush(Color.Red);
                }
                return _SumBrushWrong;
            }
        }
        private Brush _SumBrush = null;
        private Brush SumBrush
        {
            get
            {
                if (_SumBrush == null)
                {
                    _SumBrush = new SolidBrush(Color.Black);
                }
                return _SumBrush;
            }
        }
        private Brush _WeightBrush = null;
        private Brush WeightBrush
        {
            get
            {
                if (_WeightBrush == null)
                {
                    _WeightBrush = new SolidBrush(Color.Black);
                }
                return _WeightBrush;
            }
        }
        private Brush _AnswerBrush = null;
        private Brush AnswerBrush
        {
            get
            {
                if (_AnswerBrush == null)
                {
                    _AnswerBrush = new SolidBrush(Color.Black);
                }
                return _AnswerBrush;
            }
        }
        private Rectangle GetSmallerRectangle(Rectangle orignalRectangle)
        {
            /*
            Rectangle smallRectangle = new Rectangle(orignalRectangle.X + 2,
                orignalRectangle.Y + 2,
                orignalRectangle.Width - 4,
                orignalRectangle.Height - 4);
                */
            Rectangle smallRectangle = new Rectangle(orignalRectangle.X + 10,
    orignalRectangle.Y + 10,
    orignalRectangle.Width - 20,
    orignalRectangle.Height - 20);
            return smallRectangle;
        }
        /*
        private StringFormat _AlignCenter = null;
        private StringFormat AlignCenter
        {
            get
            {
                if(_AlignCenter == null)
                {
                    StringFormat _AlignCenter = new StringFormat();
                    _AlignCenter.LineAlignment = StringAlignment.Center;
                    _AlignCenter.Alignment = StringAlignment.Center;

                }
                return _AlignCenter;
            }
        }
        */

        private void DrawAnswerFromPlayer(Graphics g)
        {
            int i;
            int j;

            for (i = 0; i < this.BoardData.RowSize ; i++)
            {
                for (j = 0; j < this.BoardData.RowSize ; j++)
                {
                    Rectangle recAnswer = GetSmallerRectangle(arrRac[i + 1, j+ 1]);
                    StringFormat AlignCenter = new StringFormat();
                    
                    AlignCenter.LineAlignment = StringAlignment.Center;
                    AlignCenter.Alignment = StringAlignment.Center;
                    if(BoardData.CellvalueMatrix [i,j] == 0)
                    {
                        continue;
                    }
                    g.DrawString(BoardData.CellvalueMatrix[i, j].ToString (), AnswerFont , AnswerBrush, recAnswer,AlignCenter );
                    
                }
            }
        }

        private void DrawFinAnswer(Graphics g)
        {
            int i;
            int j;

            for (i = 0; i < RowSize; i++)
            {
                for (j = 0; j < ColumnSize; j++)
                {
                    Rectangle recAnswer = GetSmallerRectangle(arrRac[i + 1, j + 1]);
                    StringFormat AlignCenter = new StringFormat();

                    AlignCenter.LineAlignment = StringAlignment.Center;
                    AlignCenter.Alignment = StringAlignment.Center;
                    if (BoardData.CorrectCellvalueMatrix[i, j] == 0)
                    {
                        continue;
                    }
                    g.DrawString(BoardData.CorrectCellvalueMatrix[i, j].ToString(), AnswerFont, AnswerBrush, recAnswer, AlignCenter);

                }
            }
        }
       
        private void DrawSumString(Graphics g)
        {
            int i = 0;
           
            foreach (Direction dir in BoardData.Directions)
            {
               // int[] UserAnser =Board.DicUserAnswerlist[dir];
                // int j = 0;
                int line = 0;
                Position FirstPosition = BoardData.DicFirstCellPosition[dir];
                Position Del = BoardData.DeltaNextLine[dir];

                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
          
                for (line = 0; line < this.RowSize; line++)
                {
                    String CellValue = BoardData.DicCorrectAnswerlist[dir][line].ToString ();
                    Position position = Position.Empty;
                    switch (dir)
                    {
                        case Direction.Top:
                            position = new Position(0, line + 1);
                            //  format.LineAlignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Center  ;
                            break;
                        case Direction.Right:
                            position = new Position(line + 1, this.BoardData.RowSize + 1);
                            format.Alignment = StringAlignment.Center ;
                            break;
                        case Direction.Bottom:
                            position = new Position(this.BoardData.RowSize + 1, line + 1);
                            format.LineAlignment = StringAlignment.Center  ;
                            break;
                        case Direction.Left:
                            position = new Position(line + 1, 0);
                            format.Alignment = StringAlignment.Center;
                            break;
                        default:
                            throw new Exception("Please check dir");

                    }
                    Brush DrawBrush = SumBrush;
                    switch (BoardData.DicAnswerResultlist[dir][line])
                    {
                        case AnswerResult.Correct:
                            DrawBrush = SumBrushCorrect;
                            break;
                        case AnswerResult.SumIncorrect:
                            DrawBrush = SumBrushWrong;
                            break;
                            /*
                        default:
                            throw new Exception("Please check Answer Result");
                            break;
                            */
                    }
                    Rectangle recTan = arrRac[position.Row, position.Column];
                    recTan = GetSmallerRectangle(recTan);

                    g.DrawString(CellValue,SumFont , DrawBrush, recTan, format);
                //    DrawBrush.Dispose();
                //    DrawBrush = null;

                }
            }
                    

                }
        public Boolean IsRenderFinMode { get; private set; }
        private void PictureBoxBoard_Paint(object sender, PaintEventArgs e)
        {
            int i;
            int j;
            Rectangle currentCellRec = arrRac[this.BoardData.CurrentCell.Row + 1, this.BoardData.CurrentCell.Column + 1];
            currentCellRec = GetSmallerRectangle(currentCellRec);
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.White);

            DrawBoard(g, arrRac);
            DrawArrow(g, arrRac);
            if (!this.BoardData.IsFinshed)
            {
                DrawCurrentCell(g, currentCellRec);
            }
           // DrawWeigthString(g);
            DrawSumString(g);

            if (IsRenderFinMode)
            {
                DrawFinAnswer(g);
            }
            else
            {
                DrawAnswerFromPlayer(g);
            }
           

        }


    }
}
