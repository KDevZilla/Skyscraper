using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyscraper
{
    public class Board
    {
        public Position CurrentCell { get; private set; } = new Position(0, 0);
        public int RowSize { get; private set; }
        
        public int[,] CellvalueMatrix { get; set; }
        public int[,] CorrectCellvalueMatrix { get; set; }
        public int GetCellValueMatrix(Position position)
        {
            return CellvalueMatrix[position.Row, position.Column];
        }
        public int GetCorrectCellValueMatrix(Position position)
        {
            return CorrectCellvalueMatrix[position.Row, position.Column];
        }
        public Dictionary<Direction, int[]> DicUserAnswerlist { get; private set; }
        public Dictionary<Direction, int[]> DicCorrectAnswerlist { get; private set; }
        public Dictionary<Direction, AnswerResult[]> DicAnswerResultlist { get; private set; }

       // public AnswerResult[] ColAnswerResultlist { get; private set; }
        public bool IsFinshed { get; private set; }
        public IBoardGenerator BoardGenerator { get; private set; }
        public void GiveUp()
        {
            IsFinshed = true;
        }
        public enum Direction
        {
            Top,
            Right,
            Bottom,
            Left
        }
        public Boolean IsPostionValid(Position position)
        {
         
            if (position.Row < 0 || 
                position.Column < 0 ||
                position.Row > this.RowSize -1 ||
                position.Column > this.RowSize - 1)
            {
                return false;
            }
            return true;
        }
        public Boolean IsCellValueValid(int cellValue)
        {
            if (cellValue < 0 ||
                cellValue > this.RowSize)
            {
                return false;
            }

            return true;
        }
        public void MoveCurrentCellToDirection(Direction dir)
        {
            Position NewCellPosition = this.CurrentCell.Add(DeltaMoveCell[dir]);
            if(!IsPostionValid(NewCellPosition))
            {
                return;
            }
            this.CurrentCell = NewCellPosition;
            
        }
        private Dictionary<Direction, Position> _DeltaMoveCell = null;
        public Dictionary<Direction, Position> DeltaMoveCell
        {
            get
            {
                if (_DeltaMoveCell == null)
                {
                    _DeltaMoveCell = new Dictionary<Direction, Position>();
                    _DeltaMoveCell.Add(Direction.Top, new Position(-1, 0));
                    _DeltaMoveCell.Add(Direction.Right, new Position(0, 1));
                    _DeltaMoveCell.Add(Direction.Bottom, new Position(1, 0));
                    _DeltaMoveCell.Add(Direction.Left, new Position(0, -1));
                }
                return _DeltaMoveCell;
            }
        }
        public Direction[] Directions => (Direction[])Enum.GetValues(typeof(Direction));
        private Dictionary<Direction, Position> _DeltaNextCell = null;
        public Dictionary<Direction, Position> DeltaNextCell 
        {
            get
            {
                if (_DeltaNextCell == null)
                {
                    _DeltaNextCell = new Dictionary<Direction, Position>();
                    _DeltaNextCell.Add(Direction.Top, new Position(1, 0));
                    _DeltaNextCell.Add(Direction.Right, new Position(0, -1));
                    _DeltaNextCell.Add(Direction.Bottom, new Position(-1, 0));
                    _DeltaNextCell.Add(Direction.Left, new Position(0, 1));
                }
                return _DeltaNextCell;
            }
        }
        private Dictionary<Direction, Position> _DicFirstCellPosition = null;
        public Dictionary<Direction, Position> DicFirstCellPosition
        {
            get
            {
                if (_DicFirstCellPosition == null)
                {
                    _DicFirstCellPosition = new Dictionary<Direction, Position>();
                    _DicFirstCellPosition.Add(Direction.Top, new Position(0, 0));
                    _DicFirstCellPosition.Add(Direction.Right, new Position(0, this.RowSize - 1));
                    _DicFirstCellPosition.Add(Direction.Bottom, new Position(this.RowSize -1, 0));
                    _DicFirstCellPosition.Add(Direction.Left, new Position(0, 0));
                }
                return _DicFirstCellPosition;
            }
        }
        private Dictionary<Direction, Position> _DeltaNextLine = null;
        public Dictionary<Direction, Position> DeltaNextLine
        {
            get{
                if (_DeltaNextLine == null)
                {
                    _DeltaNextLine = new Dictionary<Direction, Position>();
                    _DeltaNextLine.Add(Direction.Top, new Position(0, 1));
                    _DeltaNextLine.Add(Direction.Right, new Position(1, 0));
                    _DeltaNextLine.Add(Direction.Bottom, new Position(0, 1));
                    _DeltaNextLine.Add(Direction.Left, new Position(1, 0));
                }
                return _DeltaNextLine;
            }
        }

        public enum AnswerResult
        {
            NotChooseYet,
            Correct,
            Incorrect
        }
        //public event EventHandler CellValueUpdated;
        public event EventHandler<Position> CellValueUpdated;
       
        public void SetValue(int Row,int Column, int Value)
        {
            SetValue(new Position(Row, Column), Value);
        }

        public void SetValue(Position position, int Value)
        {
            // SetValue(new Position(Row, Column), Value);

            this.CellvalueMatrix[position.Row, position.Column] = Value;
            CalculateAnswer();
            CellValueUpdated?.Invoke(this, position);

        }
        private void CalculateEachRowNumberofBuilding(bool IsForUserAnswer)
        {
            int i = 0;
            Dictionary<Direction, int[]> DicCalculate = this.DicCorrectAnswerlist;
            var CellMatrix = this.CorrectCellvalueMatrix;
            if (IsForUserAnswer)
            {
                DicCalculate = this.DicUserAnswerlist;
                CellMatrix = this.CellvalueMatrix;
            }


            foreach (Direction dir in Directions)
            {
                int[] UserAnser = DicCalculate[dir];//  DicUserAnswerlist[dir];
                // int j = 0;
                int line = 0;
                Position positionLine = DeltaNextLine[dir];
                Position positionDeltaCell = DeltaNextCell[dir];
                for (line = 0; line < this.RowSize; line++)
                {


                    Position positionCell = DicFirstCellPosition[dir];
                    int SumBuildingYouCanSeeInLine = 0;
                    if (line > 0)
                    {
                        positionCell = positionCell.Add(positionLine.Multiply (line));
                    }

                    int HightestBuilding = CellMatrix[positionCell.Row, positionCell.Column];
                    SumBuildingYouCanSeeInLine = 0;
                    if(HightestBuilding > 0)
                    {
                        SumBuildingYouCanSeeInLine = 1;
                   
                    }
                    for (i = 0; i < this.RowSize; i++)
                    {
                        int Building = CellMatrix[positionCell.Row, positionCell.Column];
                        if (Building > HightestBuilding)
                        {
                            SumBuildingYouCanSeeInLine++;
                            HightestBuilding = Building;
                        }
                        positionCell = positionCell.Add(positionDeltaCell);

                    }
                    DicCalculate[dir][line] = SumBuildingYouCanSeeInLine;
                }

            }
        }
        public void CalculateIfEachRowCorrect()
        {
            Boolean IsThisboardSolved = true;
            foreach (Direction dir in Directions)
            {
                for (int line = 0; line < this.RowSize; line++)
                {
                    if (this.DicUserAnswerlist[dir][line] == 0)
                    {
                        this.DicAnswerResultlist[dir][line] = AnswerResult.NotChooseYet;
                        IsThisboardSolved = false;
                    }
                    else
                    {
                        if (this.DicCorrectAnswerlist[dir][line] == this.DicUserAnswerlist[dir][line])
                        {
                            this.DicAnswerResultlist[dir][line] = AnswerResult.Correct;
                        }
                        else
                        {
                            this.DicAnswerResultlist[dir][line] = AnswerResult.Incorrect;
                            IsThisboardSolved = false;
                        }
                    }
                }
            }

            IsFinshed = IsThisboardSolved;
        }
        public void CalculateAnswer()
        {
            int iSum = 0;
            int i;
            int j;

            CalculateEachRowNumberofBuilding(true);
            CalculateIfEachRowCorrect();

           
        }
        public int this[int row, int column]
        {
            get
            {
                return this.CellvalueMatrix[row, column];
            }
            set
            {
                this.CellvalueMatrix[row, column] = value;
            }
        }
        public Board(int rowSize,
            int columnSize,
            IBoardGenerator Generator)
        {


            //  this.NumberofBlackCell = numberofBlackCell;
            this.RowSize = rowSize;
           // this.ColumnSize = columnSize;
            CellvalueMatrix = new int[RowSize, RowSize];
            CorrectCellvalueMatrix = new int[RowSize, RowSize ];


            DicAnswerResultlist = new Dictionary<Direction, AnswerResult[]>();
            DicUserAnswerlist = new Dictionary<Direction, int[]>();
            DicCorrectAnswerlist = new Dictionary<Direction, int[]>();

            Directions.ToList().ForEach(x=>
                {
                 DicAnswerResultlist.Add(x, new AnswerResult[RowSize]);
                    DicUserAnswerlist.Add(x, new int[RowSize]);
                    DicCorrectAnswerlist.Add(x, new int[RowSize]);
                }
                );


        
            
            this.BoardGenerator = Generator;
         
        }
        public Board Clone()
        {
            Board NewBoard = new Board(this.RowSize,
                this.RowSize,
                this.BoardGenerator);
            int i;
            int j;
            for (i = 0; i <= NewBoard.CellvalueMatrix.GetUpperBound(0); i++)
            {
                for (j = 0; j <= NewBoard.CellvalueMatrix.GetUpperBound(1); j++)
                {
                    NewBoard[i, j] = this[i, j]; //this.CorrectCellvalueMatrix[i, j];
                    NewBoard.CorrectCellvalueMatrix[i, j] = this.CorrectCellvalueMatrix[i, j];
                }
            }
            Directions.ToList().ForEach(x =>
            {
                for (i = 0; i < this.RowSize; i++)
                {
                    NewBoard.DicAnswerResultlist[x] = this.DicAnswerResultlist[x];
                    NewBoard.DicUserAnswerlist[x] = this.DicUserAnswerlist[x];
                    NewBoard.DicCorrectAnswerlist[x] = this.DicCorrectAnswerlist[x];
                }
            }
            );


           
            NewBoard.IsFinshed = this.IsFinshed;
            return NewBoard;
        }
        private void Board_CellValueUpdated(object sender, Position e)
        {

            //throw new NotImplementedException();
        }
        public void EnterCellValue(Position position, int CellValue)
        {
            /*
            if (this.IsFinshed)
            {
                return;
            }
            */
            if (!IsPostionValid(position))
            {
                return;
            }
            if(!IsCellValueValid(CellValue))
            {
                return;
            }

            SetValue(position, CellValue);

        }
        public String ReturnTextFor2DArray()
        {
            StringBuilder strB = new StringBuilder();
            int i;
            int j;

            String template = "                    list.Add(new Kakurasu.Position(!ROW!,!COL!));";
            for (i = 0; i < CorrectCellvalueMatrix.GetLength(0); i++)
            {
                for (j = 0; j < CorrectCellvalueMatrix.GetLength(1); j++)
                {
              
                        String text = template.Replace(@"!ROW!", i.ToString())
                            .Replace(@"!COL!", j.ToString());
                        strB.Append(text).Append(Environment.NewLine);
                    
                }

            }


            return strB.ToString();
        }
        public String ReturnText()
        {
            StringBuilder strB = new StringBuilder();
            int i;
            int j;
            for (i = 0; i < CorrectCellvalueMatrix.GetLength(0); i++)
            {
                for (j = 0; j < CorrectCellvalueMatrix.GetLength(1); j++)
                {
                    strB.Append(CorrectCellvalueMatrix[i, j] + "\t");
                }
                
            }


            return strB.ToString();
        }
      
        private static Random random = new Random();
        private static int GetRandom(int Min, int Max)
        {
            return random.Next(Min, Max);
        }
        Dictionary<String, Position> dicBlackPosition = new Dictionary<string, Position>();
        private void UpdateSumValue()
        {
            // int i;
            int i;
            int j;
            CalculateEachRowNumberofBuilding(true);
            
        }
        public void GenerateBoard()
        {
            BoardGenerator.Generate(this);
            CalculateEachRowNumberofBuilding(false);
            UpdateSumValue();

        }

        public interface IBoardGenerator
        {
            void Generate(Board board);
          //  void SetNumberofBlackCell(int noofBlackCell);

        }
        public class ManualGenerator : IBoardGenerator
        {
            // public bool[,] SetBlackCellValue;
            // public List<Position> listBlackCellValue = null;
            int[,] arr2d = null;
            public ManualGenerator(int[,] parr2d)
            {
                arr2d = parr2d;
               // listBlackCellValue = new List<Position>();
              //  plistBlackCellValue.ForEach(x => listBlackCellValue.Add(new Position(x.Row, x.Column)));


            }
            public void Generate(Board board)
            {
                int i;
                int j;
                //  listBlackCellValue.ForEach(x => board.CorrectCellvalueMatrix[x.Row, x.Column] = true);
                //board[0, 0] = 3;
               
                for(i=0;i<arr2d.GetLength(0); i++)
                {
                    for(j=0;j<arr2d.GetLength(1); j++)
                    {
                        //board[i, j] = arr2d[i, j];
                        board.CorrectCellvalueMatrix[i, j] = arr2d[i, j];
                    }
                }
            }
        }
        public class BasicGenerator : IBoardGenerator
        {
           
            public string FilePath { get; set; } = "";
            // Board board { get; set; }
            public BasicGenerator (String filePath)
            {
                this.FilePath = filePath;

            }
            public void Generate(Board board)
            {

                int i;
                int j;
                if(FilePath.Trim() == "")
                {
                    throw new Exception("Please set FilePath to point to PreGenerateDigits folder");
                }

              //  board.dicBlackPosition = new Dictionary<string, Position>();
                GenerateTargetNumber(FilePath,board);


            }

            private  Boolean GenerateTargetNumber(String FilePath, Board board)
            {
                int i;
                int j;
                var TargetCellValue = new int[board.RowSize, board.RowSize];
                board.CellvalueMatrix   = new int[board.RowSize , board.RowSize ];
                int[] arrRowTemp = new int[board.RowSize];

                Dictionary<int, List<List<int>>> DicList = GetDicList(board.RowSize , FilePath);


                for (i = 1; i <= board.RowSize; i++)
                {
                    Boolean IsValid = false;
                    while (!IsValid)
                    {
                        int indexRandommed = 0;
                        
                        indexRandommed = Utility.Random(0, DicList[i].Count);
                        List<int> lst = DicList[i][indexRandommed];
                        IsValid = true;
                        for (j = 0; j < lst.Count && IsValid; j++)
                        {
                            int k;
                            for (k = 0; k <= board.RowSize - 1; k++)
                            {
                                if (TargetCellValue[k, j] == lst[j])
                                {
                                    IsValid = false;

                                }
                            }
                        }

                        if (IsValid)
                        {
                            for (j = 0; j < lst.Count; j++)
                            {
                                //array[i - 1, j] = int.Parse(iResult.ToString().Substring(j, 1));
                                // board[i - 1, j] = lst[j];
                                TargetCellValue[i - 1, j] = lst[j];

                            }
                        }


                    }
                }

                //Swap Row;
                //int i;
                for (i = 0; i < board.RowSize; i++)
                {
                    for (j = 0; j < board.RowSize; j++)
                    {
                        board.CorrectCellvalueMatrix[i, j] = TargetCellValue[i, j];
                    }
                }
                for (i = 0; i <= 30; i++)
                {
                    int iFirstRow = Utility.Random(0, board.RowSize );
                    int iSecondRow = Utility.Random(0, board.RowSize);
                    if (iFirstRow == iSecondRow)
                    {
                        continue;
                    }
                    //int j;

                    for (j = 0; j <= board.RowSize  - 1; j++)
                    {
                        arrRowTemp[j] = board.CorrectCellvalueMatrix[iSecondRow, j];
                        board.CorrectCellvalueMatrix[iSecondRow, j] = board.CorrectCellvalueMatrix[iFirstRow, j];
                        board.CorrectCellvalueMatrix[iFirstRow, j] = arrRowTemp[j];

                    }

                }


                return true;
            }

            static Dictionary<int, List<List<int>>> _DicList = new Dictionary<int, List<List<int>>>();
            static Dictionary<int, Dictionary<int, List<List<int>>>> AllDiclist = new Dictionary<int, Dictionary<int, List<List<int>>>>();

            private static Dictionary<int, List<List<int>>> GetDicList(int BoardSize, String DicListPath)
            {
                if (!AllDiclist.ContainsKey(BoardSize))
                {
                    AllDiclist.Add(BoardSize, LoadDiclist(BoardSize, DicListPath));

                }

                return AllDiclist[BoardSize];


            }

            public static Dictionary<int, List<List<int>>> LoadDiclist(int NumberofDigit, String DicListPath)
            {

                Dictionary<int, List<List<int>>> DicList = new Dictionary<int, List<List<int>>>();
                int[] number = new int[NumberofDigit];
                int i = 0;
                for (i = 1; i <= NumberofDigit; i++)
                {
                    DicList.Add(i, new List<List<int>>());
                    number[i - 1] = i;
                }


                for (i = 1; i <= NumberofDigit; i++)
                {

                    string fileName = DicListPath + NumberofDigit.ToString() + "_" + i.ToString() + ".txt";
                    //List<Order> SortedList = objListOrder.OrderBy(o => o.OrderDate).ToList();
                    //DicList[DigitBegin].Sort();
                    System.IO.StreamReader SR = new System.IO.StreamReader(fileName);
                    string fileContent = SR.ReadToEnd();
                    SR.Close();
                    SR.Dispose();
                    string[] arrLine = fileContent.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (String line in arrLine)
                    {
                        List<int> lst = new List<int>();
                        foreach (char strchar in line.ToCharArray())
                        {
                            lst.Add(int.Parse(strchar.ToString()));
                        }
                        DicList[i].Add(lst);
                    }

                }
                return DicList;
            }
        }
    }
}
