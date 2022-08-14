using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyscraper
{
    public class Game
    {
        public Board board { get; private set; }

        UI.IUI UIBoard = null;
        public int RowSize { get; private set; }
        public int ColumnSize { get; private set; }
        public Game(UI.IUI pUIBoard, int pRowSize,
            int pColumnSize,
           Board.IBoardGenerator Generator
            )
        {
            RowSize = pRowSize;
            ColumnSize = pColumnSize;

            board = new Board(RowSize,
                ColumnSize,
                Generator);
            this.UIBoard = pUIBoard;
            this.UIBoard.SetGame(this);
            this.UIBoard.RenderBoard();

            this.UIBoard.CellClick -= UIBoard_CellClick;
            this.UIBoard.UIKeyNavigatorDown -= UIBoard_UIKeyNavigatorDown;
            this.UIBoard.UIKeyNumberDown -= UIBoard_UIKeyNumberDown;
            this.UIBoard.CellClick += UIBoard_CellClick;
            this.UIBoard.UIKeyNavigatorDown += UIBoard_UIKeyNavigatorDown;
            this.UIBoard.UIKeyNumberDown += UIBoard_UIKeyNumberDown;

            this.board.CellValueUpdated -= Board_CellValueUpdated;
            this.board.CellValueUpdated += Board_CellValueUpdated;
                
            this.board.GenerateBoard();
        }

        private void UIBoard_UIKeyNumberDown(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            // KeyEventArgs ke = (KeyEventArgs)e;
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                if (board.IsFinshed)
                {
                    return;
                }
            }
            UI.IntegerEventArgs ke = (UI.IntegerEventArgs)e;
            int CellValue = ke.Value;

            this.board.EnterCellValue(this.board.CurrentCell, CellValue);
            this.UIBoard.RenderBoard();


        }

        private void UIBoard_UIKeyNavigatorDown(object sender, EventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                if (board.IsFinshed)
                {
                    return;
                }
            }
            
            // throw new NotImplementedException();
            KeyEventArgs ke = (KeyEventArgs)e;

            if(!UI.UIUtility.IsKeyMatchDirection(ke.KeyCode))
            {
                return;
            }
            this.board.MoveCurrentCellToDirection(UI.UIUtility.GetBoardDirectionFromKey(ke.KeyCode));
            this.UIBoard.RenderBoard();
            
        }

        private void Board_CellValueUpdated(object sender, Position e)
        {
            // throw new NotImplementedException();
            if (this.UIBoard == null)
            {
                return;
            }

            this.UIBoard.RenderBoard();
            if (this.board.IsFinshed)
            {
                this.UIBoard.InformUserWon();
            }
        }

        public void ReleaseResource()
        {
            if (this.UIBoard != null)
            {

                this.UIBoard.CellClick -= UIBoard_CellClick;
                this.UIBoard.UIKeyNavigatorDown -= UIBoard_UIKeyNavigatorDown;
                this.UIBoard.UIKeyNumberDown -= UIBoard_UIKeyNumberDown;

                this.UIBoard.ReleaseResource();
            }
            if (this.board != null)
            {
                this.board.CellValueUpdated -= Board_CellValueUpdated;
            }

        }

        private void UIBoard_CellClick(object sender, Position position)
        {
            // throw new NotImplementedException();
            if (this.board.IsFinshed)
            {
                return;
            }

           // this.board.SwitchValue(position);
           
            if (this.board.IsFinshed)
            {
                UIBoard.InformUserWon();
            }
            //  this.Board.CellvalueMatrix[position.Row, position.Column] = !this.Board.CellvalueMatrix [position ;
            // this.UIBoard.RenderBoard();
        }

    }
}
