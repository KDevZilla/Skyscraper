using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyscraper.UI
{
    public class UIUtility
    {
        static Dictionary<Keys, Board.Direction> _DicKeyDirection =null;
        static Dictionary<Keys, Board.Direction> DicKeyDirection
        {
            get
            {
                if(_DicKeyDirection==null ||
                    _DicKeyDirection.Count == 0)
                {
                    _DicKeyDirection = new Dictionary<Keys, Board.Direction>();
                    _DicKeyDirection.Add(Keys.Left, Board.Direction.Left);
                    _DicKeyDirection.Add(Keys.Right, Board.Direction.Right );
                    _DicKeyDirection.Add(Keys.Up, Board.Direction.Top);
                    _DicKeyDirection.Add(Keys.Down, Board.Direction.Bottom);

                }
                return _DicKeyDirection;
            }
        }
            // = new Dictionary<Keys, Board.Direction>();
        public static bool IsKeyMatchDirection(Keys key)
        {
            return DicKeyDirection.ContainsKey(key);
        }
        public static Board.Direction GetBoardDirectionFromKey(Keys key)
        {
            
            return DicKeyDirection[key];
        }
        static Dictionary<int, int> DicKeyValueMax = new Dictionary<int, int>();
        public static int GetKeyValueDMax(int BoardSize)
        {
            if (DicKeyValueMax.Count == 0)
            {
                DicKeyValueMax.Add(0, (int)Keys.D0);
                DicKeyValueMax.Add(1, (int)Keys.D1);
                DicKeyValueMax.Add(2, (int)Keys.D2);
                DicKeyValueMax.Add(3, (int)Keys.D3);
                DicKeyValueMax.Add(4, (int)Keys.D4);
                DicKeyValueMax.Add(5, (int)Keys.D5);
                DicKeyValueMax.Add(6, (int)Keys.D6);
                DicKeyValueMax.Add(7, (int)Keys.D7);
                DicKeyValueMax.Add(8, (int)Keys.D8);
                DicKeyValueMax.Add(9, (int)Keys.D9);
            }
            return DicKeyValueMax[BoardSize];
        }

        static Dictionary<int, int> DicKeyValueNumpadMax = new Dictionary<int, int>();
        public static int GetKeyValueNumpadMax(int BoardSize)
        {
            if (DicKeyValueNumpadMax.Count == 0)
            {

                DicKeyValueNumpadMax.Add(0, (int)Keys.NumPad0);
                DicKeyValueNumpadMax.Add(1, (int)Keys.NumPad1);
                DicKeyValueNumpadMax.Add(2, (int)Keys.NumPad2);
                DicKeyValueNumpadMax.Add(3, (int)Keys.NumPad3);
                DicKeyValueNumpadMax.Add(4, (int)Keys.NumPad4);
                DicKeyValueNumpadMax.Add(5, (int)Keys.NumPad5);
                DicKeyValueNumpadMax.Add(6, (int)Keys.NumPad6);
                DicKeyValueNumpadMax.Add(7, (int)Keys.NumPad7);
                DicKeyValueNumpadMax.Add(8, (int)Keys.NumPad8);
                DicKeyValueNumpadMax.Add(9, (int)Keys.NumPad9);
            }
            return DicKeyValueNumpadMax[BoardSize];
        }
    }
}
