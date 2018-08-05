using System.Collections.Generic;

namespace MiniMax_TicTacToe_Lib
{
    class Node
    {
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

        public Node(sbyte[][] _board, sbyte _piece) : this()
        {
            Board = _board;
            UpdatedPiece = _piece;
        }
    }
}