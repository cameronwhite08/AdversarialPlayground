using System.Collections.Generic;

namespace MiniMax_TicTacToe_Lib
{
    public class Node
    {
        public readonly PieceType UpdatedPiece;

        public int Value;

        public readonly PieceType[][] Board;
        public List<Node> Children;

        public Node()
        {
            Value = -1;
            Children = new List<Node>();
        }

        public Node(PieceType[][] _board, PieceType _piece) : this()
        {
            Board = _board;
            UpdatedPiece = _piece;
        }
    }
}