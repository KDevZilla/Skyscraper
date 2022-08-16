using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skyscraper;
using static Skyscraper.Board;

namespace SkycraperTest
{
    class Utility
    {
        public static Boolean IsBoardInputValueTheSame(Board board1, Board board2)
        {
            int i;
            int j;
            if (board1.IsFinshed != board2.IsFinshed)
            {
                return false;
            }
            if (board1.RowSize != board2.RowSize )
            {
                return false;
            }

            for (i = 0; i < board1.RowSize; i++)
            {
                for (j = 0; j < board1.RowSize; j++)
                {
                    if (board1[i, j] != board2[i, j])
                    {
                        return false;
                    }
                    if (board1.CorrectCellvalueMatrix[i, j] != board2.CorrectCellvalueMatrix[i, j])
                    {
                        return false;
                    }
                }
            }
            foreach (Direction dir in board1.Directions)
            {
                for(i=0;i<board1.DicAnswerResultlist[dir].Count (); i++)
                {
                    if(board1.DicAnswerResultlist [dir][i] !=
                        board2.DicAnswerResultlist[dir][i])
                    {
                        return false;
                    }
                }
                for (i = 0; i < board1.DicCorrectAnswerlist[dir].Count(); i++)
                {
                    if(board1.DicCorrectAnswerlist [dir][i] !=
                        board2.DicCorrectAnswerlist[dir][i])
                    {
                        return false;
                    }
                }
                for(i=0;i< board1.DicUserAnswerlist[dir].Count(); i++)
                {
                    if(board1.DicUserAnswerlist [dir][i] !=
                        board2.DicUserAnswerlist[dir][i])
                    {
                        return false;
                    }
                }
                
                // board1.DicCorrectAnswerlist 
            }
            

            return true;
        }
    }
}
