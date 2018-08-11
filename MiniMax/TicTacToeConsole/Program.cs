using System;
using MiniMax_TicTacToe_Lib;

namespace TicTacToeConsole
{
    class MainClass
    {
        //BLUE = 1 
        //RED = -1 
        //EMPTY = 0
        static PieceType Player, Oponnent;
        const int AIDepth = 20;

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

            var lib = new TicTacToeLib(Player);

            if (gameMode == 1)
            {
                lib.HumanvsHuman(Player);
                return;
            }
            lib.HumanVsComputer(Player, AIDepth);
            Console.ReadLine();
        }
    }
}
