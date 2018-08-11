from Constants import PieceType


class Node:

    def __init__(self, board=None, piece=None):
        self.value = -1
        self.children = []
        self.board = [[]]
        self.updated_piece = PieceType.Empty

        if board is not None:
            self.board = board
        if piece is not None:
            self.updated_piece = piece
