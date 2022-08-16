using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyscraper;
using static Skyscraper.Board;

namespace SkycraperTest
{
    [TestClass]
    public class IntegrateTest
    {

        private Boolean HasInitial = false;
        Skyscraper.Board OriginalBoard = null;
        int[,] arr2d = {
                    {3,2,1,4 },
                    {2,3,4,1 },
                    {4,1,2,3 },
                    {1,4,3,2 }
                };
        private void Initial()
        {
            if (HasInitial)
            {
                return;
            }


            Board.ManualGenerator manualGenerator = new Board.ManualGenerator(arr2d);

            //Kakurasu.KakurasuBoard.ManualGenerator manualGenerator = new KakurasuBoard.ManualGenerator(list);
            OriginalBoard = new Board(4, 4, manualGenerator);
            OriginalBoard.GenerateBoard();

            HasInitial = true;
        }
        [TestMethod]
        public void CheckCreateBoard()
        {
            Initial();


            Board Board2 = OriginalBoard.Clone();

            Trace.Assert(!Board2.IsFinshed);

            int i;
            int j;
            //The Correct Value
            for (i = 0; i < arr2d.GetUpperBound(0); i++)
            {
                for (j = 0; j < arr2d.GetUpperBound(1); j++)
                {

                    Trace.Assert(Board2.CorrectCellvalueMatrix[i, j] == arr2d[i, j], String.Format("{0},{1}", i, j));
                }
            }
            //Inital Value must be 0
            for (i = 0; i < arr2d.GetUpperBound(0); i++)
            {
                for (j = 0; j < arr2d.GetUpperBound(1); j++)
                {

                    Trace.Assert(Board2[i, j] == 0);
                }
            }

            int[] CorrectTopRow = { 2, 3, 2, 1 };
            int[] CorrectBottomRow = { 2, 1, 2, 3 };
            int[] CorrectRightRow = { 1, 2, 2, 3 };
            int[] CorrectLeftRow = { 2, 3, 1, 2 };
            for (i = 0; i < 4; i++)
            {
                Trace.Assert(Board2.DicCorrectAnswerlist[Board.Direction.Top][i] ==
                    CorrectTopRow[i]);

                Trace.Assert(Board2.DicCorrectAnswerlist[Board.Direction.Bottom][i] ==
                    CorrectBottomRow[i]);

                Trace.Assert(Board2.DicCorrectAnswerlist[Board.Direction.Right][i] ==
                    CorrectRightRow[i]);

                Trace.Assert(Board2.DicCorrectAnswerlist[Board.Direction.Left][i] ==
                    CorrectLeftRow[i]);

            }
            foreach (Direction dir in Board2.Directions)
            {
                for (i = 0; i < 4; i++)
                {
                    Trace.Assert(Board2.DicAnswerResultlist[dir][i] == AnswerResult.NotChooseYet);


                }
            }


        }


        [TestMethod]
        public void PutCell()
        {
            Initial();

           
            Board Board2 = OriginalBoard.Clone();
            int[,] arrInvalidPoition = {
                {-1,-1},
                {-1,1},
                {0,5 },
                {0,6 },

            };
            int i;
            Board boardResult = null;
            for (i = 0; i < arrInvalidPoition.GetUpperBound(0); i++)
            {
                Position InValidPosition = new Position(arrInvalidPoition[i, 0], arrInvalidPoition[i, 1]);
                boardResult = Board2.Clone();
                boardResult.EnterCellValue(InValidPosition, 2);
                Trace.Assert(Utility.IsBoardInputValueTheSame(Board2, boardResult));
            }
            int[] arrInvalidCellValue =
            {
                -1,
                5
            };
            for (i = 0; i < arrInvalidCellValue.GetUpperBound(0); i++)
            {
                Position ValidPosition = new Position(0,0);
                boardResult = Board2.Clone();
                boardResult.EnterCellValue(ValidPosition, arrInvalidCellValue [i]);
                Trace.Assert(Utility.IsBoardInputValueTheSame(Board2, boardResult));
            }
            /*
int[,] arr2d = {
            {3,2,1,4 },
            {2,3,4,1 },
            {4,1,2,3 },
            {1,4,3,2 }
 */
            boardResult = Board2.Clone();
            boardResult.EnterCellValue(new Position(0, 0), 3);
            boardResult.EnterCellValue(new Position(1, 0), 2);
            boardResult.EnterCellValue(new Position(2, 0), 4);
            boardResult.EnterCellValue(new Position(3, 0), 1);

            Trace.Assert(boardResult[0, 0] == 3);
            Trace.Assert(boardResult[1, 0] == 2);
            Trace.Assert(boardResult[2, 0] == 4);
            Trace.Assert(boardResult[3, 0] == 1);

            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Top][0] == 2);
            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Bottom][0] == 2);
            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Left][0] == 1);
            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Left][1] == 1);
            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Left][2] == 1);
            Trace.Assert(boardResult.DicUserAnswerlist[Direction.Left][3] == 1);

            Trace.Assert(boardResult.DicAnswerResultlist[Direction.Top][0] == AnswerResult.Correct);
            Trace.Assert(boardResult.DicAnswerResultlist[Direction.Bottom][0] == AnswerResult.Correct);


        }

    }
}
