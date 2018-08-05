using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniMax_TicTacToe_Lib
{
    public enum PieceType : sbyte{
        X, O, Empty
    }

    class MainClass
    {
        //BLUE = 1 
        //RED = -1 
        //EMPTY = 0
        static PieceType Player, Oponnent;
        const int AIDepth = 6;

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Tic Tac Toe!");

            Console.WriteLine("Chose a game option:");
            Console.WriteLine("1)Human vs Human");
            Console.WriteLine("2)Human vs Computer");
            var gameMode = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the piece you want to play. \"x\" or \"o\"");
            var pieceC = Console.ReadLine();

            Player = pieceC.ToLower().Equals("x") ? PieceType.X : PieceType.O;
            Oponnent = (Player == PieceType.O) ? PieceType.X : PieceType.O;

            if (gameMode == 1)
            {
                HumanvsHuman(Player);
                return;
            }
            HumanVsComputer();
        }

        public static void HumanvsHuman(PieceType startPiece)
        {
            var currentPiece = startPiece;
            var board = CreateEmptyBoard();
            PrintBoard(board);
            var column = 0;
            var row = 0;

            while (column != -1)
            {
                Console.WriteLine("{0}'s move", currentPiece);

                Console.WriteLine("Enter the row you want to drop a piece");
                row = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter the column you want to drop a piece");
                column = int.Parse(Console.ReadLine());

                board[row][column] = currentPiece;

                if (column != -1)
                {
                    PrintBoard(board);

                    if (CheckWin(board))
                    {
                        Console.WriteLine("You Won, {0}!", currentPiece);
                        return;
                    }
                    currentPiece = (currentPiece == PieceType.O) ? PieceType.X : PieceType.O;
                }
            }
        }

        public static void HumanVsComputer()
        {
            var board = CreateEmptyBoard();
            PrintBoard(board);
            var column = 0;
            var row = 0;
            while (column != -1)
            {
                Console.WriteLine("Enter the column you want to drop a piece");
                column = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the row you want to drop a piece");
                row = int.Parse(Console.ReadLine());


                board[row][column] = Player;

                PrintBoard(board);

                if (CheckWin(board))
                {
                    Console.WriteLine("You Won!");
                    return;
                }

                //make pc move
                board = AI(board, Player, AIDepth);
                Console.WriteLine("AI Move:");
                PrintBoard(board);

                if (CheckWin(board))
                {
                    Console.WriteLine("The Pc Won!");
                    return;
                }

            }
        }

        static bool CheckWin(PieceType[][] board)
        {
            var top = board[0][0] == board[0][1] && board[0][1] == board[0][2];
            var middle = board[1][0] == board[1][1] && board[1][1] == board[1][2];
            var bottom = board[2][0] == board[2][1] && board[2][1] == board[2][2];

            var left = board[0][0] == board[1][0] && board[1][0] == board[2][0];
            var center = board[0][1] == board[1][1] && board[1][1] == board[2][1];
            var right = board[0][2] == board[1][2] && board[1][2] == board[2][2];

            var TLBRDiag = board[0][0] == board[1][1] && board[1][1] == board[2][2];
            var BLTRDiag = board[2][0] == board[1][1] && board[1][1] == board[0][2];

            return top || middle || bottom || left || center || right || TLBRDiag || BLTRDiag;
        }

        #region Board Operations
        static void PrintBoard(PieceType[][] board)
        {
            //created to maintain default system console color
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i <board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    var output = ' ';
                    if (board[i][j] == PieceType.X)
                    {
                        output = 'X';
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (board[i][j] == PieceType.O)
                    {
                        output = 'O';
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(" " + output);
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static PieceType[][] CopyBoard(PieceType[][] board)
        {
            var newBoard = new PieceType[board.Length][];
            for (int i = 0; i < board.Length; i++)
            {
                newBoard[i] = new PieceType[board[i].Length];
                for (int j = 0; j < board[i].Length; j++)
                {
                    newBoard[i][j] = board[i][j];
                }
            }
            return newBoard;
        }

        static PieceType[][] CreateEmptyBoard()
        {
            var boardHeight = 3;

            var tempBoard = new PieceType[boardHeight][];

            for (int i = 0; i < boardHeight; i++)
            {
                tempBoard[i] = new PieceType[boardHeight];

                for (int j = 0; j < tempBoard[i].Length; j++)
                {
                    tempBoard[i][j] = PieceType.Empty;
                }
            }
            return tempBoard;
        }

        static Tuple<int,int>[] AvailableMoves(PieceType[][] board)
        {
            var available = new List<Tuple<int, int>>();

            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] == PieceType.Empty)
                    {
                        available.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return available.ToArray();
        }

        #endregion

        static int GradeState(Node state, PieceType piece, int depth)
        {
            //if a win
            if (CheckWin(state.Board)){

                //only used by AI, so good score for player
                if (Player == piece)
                {
                    return depth;
                }
                //'bad' for pc win
                return -depth;
            }

            //TODO TEMPORARY TODO
            //draw
            return 0;
        }

        #region AI
        static Node GenerateMoveTree(PieceType[][] board, PieceType piece, int depth)
        {
            var outNode = new Node(board, piece);

            //checking for win, and stopping here if so.
            if (depth < 0 || CheckWin(board))
            {
                return outNode;
            }

            var availableMoves = AvailableMoves(board);
            foreach (var indices in availableMoves)
            {
                var newBoard = CopyBoard(board);
                PieceType oppositePiece = (piece == PieceType.O) ? PieceType.X : PieceType.O;
                newBoard[indices.Item1][indices.Item2] = oppositePiece;

                var node = GenerateMoveTree(newBoard, oppositePiece, depth - 1);
                outNode.Children.Add(node);
            }

            return outNode;
        }

        static int AlphaBetaMinMax(Node node, bool maxamizingPlayer, int depthScore, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if (node.Children.Count == 0)
            {
                return GradeState(node, node.UpdatedPiece, depthScore);
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

        static PieceType[][] AI(PieceType[][] board, PieceType piece, int depth)
        {
            var tree = GenerateMoveTree(board, piece, depth);
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
}