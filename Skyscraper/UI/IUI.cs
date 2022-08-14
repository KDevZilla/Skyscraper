using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyscraper.UI
{
    public interface IUI
    {
        void RenderBoard();
        void NewGame(int BoardSize);
        void SetGame(Game game);
        void Initial(int BoardSize);
        void ReleaseResource();
        void InformUserWon();
        void ShowFin();
        // void KeyDown(int Numberkey);
        event EventHandler UIKeyNumberDown;
        event EventHandler UIKeyNavigatorDown;
        event PictureBoxBoard.PictureBoxCellClick CellClick;
    }
    public class IntegerEventArgs:EventArgs{
        public int Value { get; private set; }
        public IntegerEventArgs(int pValue)
        {
            Value = pValue;
        }
    }
    public class KeyCodeEventArgs : EventArgs
    {
       // public 
    }

}
