using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniMax_TicTacToe_Lib
{
    class MainClass
    {
        //BLUE = 1 RED = -1 EMPTY = 0
        static sbyte Player;
        static sbyte Computer;

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect R!");
            Console.WriteLine("Enter the Board Width");
            boardWidth = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Board Height");
            boardHeight = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the Win Length");
            winLength = int.Parse(Console.ReadLine());

            Console.WriteLine("Chose a game option:");
            Console.WriteLine("1)Human vs Human");
            Console.WriteLine("2)Human vs Computer");
            var gameMode = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the piece you want to play. \"r\" or \"b\"");
            var pieceC = Console.ReadLine();

            Player = (pieceC.ToLower().Equals("r")) ? (sbyte)-1 : (sbyte)1;
            Computer = (sbyte)-Player;

            if (gameMode == 1)
            {
                HumanvsHuman(Player);
                return;
            }
            HumanVsComputer();
        }

        public static void HumanvsHuman(sbyte piece)
        {
            var board = CreateEmptyBoard();
            PrintBoard(board);
            var column = 0;

            while (column != -1)
            {
                Console.WriteLine("Enter the column you want to drop a piece. Enter -1 to Quit the Game.");
                var cPlayer = piece == -1 ? 'r' : 'b';
                Console.WriteLine("{0} move", cPlayer);
                column = int.Parse(Console.ReadLine());

                if (column != -1)
                {
                    var index = DropPiece(board, column, piece);

                    PrintBoard(board);

                    if (IsWin(board, piece, index.Item1, index.Item2))
                    {
                        Console.WriteLine("You Won, {0}!", cPlayer);
                        return;
                    }
                    piece = (sbyte)-piece;
                }
            }
        }

        public static void HumanVsComputer()
        {
            var board = CreateEmptyBoard();
            PrintBoard(board);
            var column = 0;
            while (column != -1)
            {
                Console.WriteLine("Enter the column you want to drop a piece");
                column = int.Parse(Console.ReadLine());


                var index = DropPiece(board, column, Player);

                PrintBoard(board);

                if (IsWin(board, Player, index.Item1, index.Item2))
                {
                    Console.WriteLine("You Won, Human!");
                    return;
                }

                //make pc move
                board = AI(board, index.Item1, index.Item2, Player, 6);
                Console.WriteLine("AI Move:");
                PrintBoard(board);

                if (IsWin(board, Computer, index.Item1, index.Item2))
                {
                    Console.WriteLine("The Pc Won!");
                    return;
                }

            }
        }

        #region Win
        static bool IsWin(sbyte[][] board, sbyte piece, int row, int column)
        {
            //would stand still
            //CheckWin(board, piece, row, column, 0, 0);

            //checks north
            return CheckWin(board, piece, row, column, 1, 0) ||
            //checks south
            CheckWin(board, piece, row, column, -1, 0) ||
            //checks east
            CheckWin(board, piece, row, column, 0, 1) ||
            //checks west
            CheckWin(board, piece, row, column, 0, -1) ||
            //chacks ne
            CheckWin(board, piece, row, column, 1, 1) ||
            //chacks nw
            CheckWin(board, piece, row, column, 1, -1) ||
            //chacks sw
            CheckWin(board, piece, row, column, -1, -1) ||
                //chacks se
                CheckWin(board, piece, row, column, -1, 1);
        }

        static bool CheckWin(sbyte[][] board, sbyte piece, int _row, int _column, int rowDelta, int colDelta)
        {
            var pieceCount = 0;
            var counter = 0;
            var row = _row;
            var column = _column;
            //while inside the counds of the current board and less than winlength iterations
            while (counter < winLength && row < boardHeight && row > -1 && column < boardWidth && column > -1)
            {
                //if the desired player piece, continue normally
                if (board[row][column].Equals(piece))
                {
                    pieceCount++;
                }
                else
                {
                    if (pieceCount == 3)
                    {
                        //reverse the delta
                        rowDelta = -rowDelta;
                        colDelta = -colDelta;

                        //get index opposite direction 1 step away
                        var r = _row + rowDelta;
                        var c = _column + colDelta;
                        //make sure still inside the board
                        if (r < boardHeight && r > -1 && c < boardWidth && c > -1)
                        {
                            //if a player counter is 3 at this point, check if the 4th piece is right before where we started looking
                            if (board[r][c].Equals(piece))
                                return true;
                        }
                    }
                }

                if (pieceCount >= winLength)
                    return true;

                row += rowDelta;
                column += colDelta;
                counter++;
            }
            return false;
        }
        #endregion

        #region Board Operations
        static void PrintBoard(sbyte[][] board)
        {
            //created to maintain default system console color
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = board.Length - 1; i > -1; i--)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    var output = 'o';
                    if (board[i][j].Equals(-1))
                    {
                        output = 'r';
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (board[i][j].Equals(1))
                    {
                        output = 'b';
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(" " + output);
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static sbyte[][] CopyBoard(sbyte[][] board)
        {
            var newBoard = new sbyte[board.Length][];
            for (int i = 0; i < board.Length; i++)
            {
                newBoard[i] = new sbyte[board[i].Length];
                for (int j = 0; j < board[i].Length; j++)
                {
                    newBoard[i][j] = board[i][j];
                }
            }
            return newBoard;
        }

        static sbyte[][] CreateEmptyBoard()
        {
            var tempBoard = new sbyte[boardHeight][];
            for (int i = 0; i < boardHeight; i++)
            {
                tempBoard[i] = new sbyte[boardWidth];
            }

            for (int i = 0; i < tempBoard.Length; i++)
            {
                for (int j = 0; j < tempBoard[i].Length; j++)
                {
                    tempBoard[i][j] = 0;
                }
            }
            return tempBoard;
        }


        static bool IsEmpty(sbyte[][] board)
        {
            for (int i = 0; i < board[0].Length; i++)
            {
                //only need to look at bottom of columns in board
                //if a cell is not empty the table is not empty
                if (board[0][i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        static int GradeState(Node state, int depth)
        {
            //if a win
            if (IsWin(state.Board, Player, state.UpdatedRow, state.UpdatedColumn))
                return depth;
            //if a loss
            if (IsWin(state.Board, Computer, state.UpdatedRow, state.UpdatedColumn))
                return -depth;


            //TODO TEMPORARY TODO
            return 0;
        }

        #region AI
        static Node GenerateMoveTree(sbyte[][] board, sbyte piece, int row, int column, int depth)
        {
            var outNode = new Node(board, piece, row, column);

            //checking for win, and stopping here if so.
            if (depth < 0 || IsWin(board, piece, row, column))
            {
                return outNode;
            }

            outNode.AvailableMoves = AvailableMoves(board);
            foreach (var index in outNode.AvailableMoves)
            {
                var newBoard = CopyBoard(board);
                sbyte oppositePiece = (sbyte)-piece;
                var rc = DropPiece(newBoard, index, oppositePiece);

                var node = GenerateMoveTree(newBoard, oppositePiece, rc.Item1, rc.Item2, depth - 1);
                outNode.Children.Add(node);
            }

            return outNode;
        }

        static int AlphaBetaMinMax(Node node, bool maxamizingPlayer, int depthScore, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if (node.Children.Count == 0)
            {
                return GradeState(node, depthScore);
            }
            if (maxamizingPlayer)
            {
                var bestValue = int.MinValue;
                foreach (var child in node.Children)
                {
                    var grade = AlphaBetaMinMax(child, false, depthScore - 1, alpha, beta);
                    bestValue = Math.Max(bestValue, grade);
                    alpha = Math.Max(alpha, bestValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return bestValue;
            }
            else
            {
                var bestValue = int.MaxValue;
                foreach (var child in node.Children)
                {
                    var grade = AlphaBetaMinMax(child, true, depthScore - 1, alpha, beta);
                    bestValue = Math.Min(bestValue, grade);
                    beta = Math.Min(beta, bestValue);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return bestValue;
            }

        }

        static sbyte[][] AI(sbyte[][] board, int row, int column, sbyte piece, int depth)
        {
            var tree = GenerateMoveTree(board, piece, row, column, depth);
            foreach (var child in tree.Children)
            {
                child.Value = AlphaBetaMinMax(child, false, depth);
            }

            var maxScore = int.MinValue;
            var index = -1;
            for (int i = 0; i < tree.Children.Count; i++)
            {
                if (tree.Children[i].Value > maxScore)
                {
                    index = i;
                    maxScore = tree.Children[i].Value;
                }
            }
            return tree.Children[index].Board;
        }
        #endregion
    }

    class Node
    {
        public readonly int UpdatedRow;
        public readonly int UpdatedColumn;
        public readonly sbyte UpdatedPiece;

        public int Value;

        public readonly sbyte[][] Board;
        public int[] AvailableMoves;
        public List<Node> Children;

        public Node()
        {
            Value = -1;
            Children = new List<Node>();
        }

        public Node(sbyte[][] _board, sbyte _piece, int _row, int _column) : this()
        {
            Board = _board;
            UpdatedRow = _row;
            UpdatedColumn = _column;
            UpdatedPiece = _piece;
        }
    }
}