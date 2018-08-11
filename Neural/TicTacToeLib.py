from Constants import PieceType, bcolors
from Node import Node
import os


class TicTacToeLib:
    player = PieceType.Empty

    def HumanVsHuman(self, player_piece):
        self.player = player_piece
        current_piece = self.player
        board = self.create_empty_board()
        self.print_board(board)

        col = 0
        row = 0

        while col != -1:
            print("{0}'s move".format(current_piece))

            row = int(input('Enter the row you want to place a piece'))
            col = int(input('Enter the column you want to place a piece (-1 to quit)'))

            if col != -1:
                board[row][col] = current_piece

                self.print_board(board)

                if self.check_win(board):
                    print('You won {0}!'.format(current_piece))
                    return
                current_piece = self.invert_piece(current_piece)

    def HumanVsComputer(self, player_piece, depth):
        self.player = player_piece

        board = self.create_empty_board()
        self.print_board(board)
        col = 0
        row = 0

        while col != -1:
            row = int(input('Enter the row you want to place a piece'))
            col = int(input('Enter the col you want to place a piece (-1 to exit)'))

            board[row][col] = self.player

            self.print_board(board)

            if self.check_win(board):
                print('You won!')
                return

            if not self.board_has_moves(board):
                print('Board Full, restarting..')
                board = self.create_empty_board()
                continue

            board = self.Ai(board, depth)
            print('Ai Move:')
            self.print_board(board)

            if self.check_win(board):
                print('The PC won!')
                return

            if not self.board_has_moves(board):
                print('Board Full, restarting..')
                board = self.create_empty_board()
                continue

    def invert_piece(self, piece):
        return PieceType.X if (piece == PieceType.O) else PieceType.O

    def check_win(self, board):
        top = (board[0][0] == board[0][1]) and (board[0][1] == board[0][2]) and board[0][2] != PieceType.Empty
        middle = board[1][0] == board[1][1] and board[1][1] == board[1][2] and board[1][2] != PieceType.Empty
        bottom = board[2][0] == board[2][1] and board[2][1] == board[2][2] and board[2][2] != PieceType.Empty

        left = board[0][0] == board[1][0] and board[1][0] == board[2][0] and board[2][0] != PieceType.Empty
        center = board[0][1] == board[1][1] and board[1][1] == board[2][1] and board[2][1] != PieceType.Empty
        right = board[0][2] == board[1][2] and board[1][2] == board[2][2] and board[2][2] != PieceType.Empty

        TLBRDiag = board[0][0] == board[1][1] and board[1][1] == board[2][2] and board[0][2] != PieceType.Empty
        BLTRDiag = board[2][0] == board[1][1] and board[1][1] == board[0][2] and board[0][2] != PieceType.Empty

        return top or middle or bottom or left or center or right or TLBRDiag or BLTRDiag

    # board operations
    def print_board(self, board):
        for r in range(len(board)):
            line_string = ''
            for c in range(len(board[r])):
                output = ' '
                if board[r][c] == PieceType.X:
                    output = bcolors.WARNING + 'X' + bcolors.ENDC
                elif board[r][c] == PieceType.O:
                    output = bcolors.OKBLUE + 'O' + bcolors.ENDC
                line_string += ' ' + output
                if c != len(board[r])-1:
                    line_string += '|'
            print(line_string)
            if r != len(board) - 1:
                print('-'*7)

    def copy_board(self, board):
        out_board = []
        for row in range(len(board)):
            out_board.append([])
            for pos in range(len(board[row])):
                out_board[row].append(board[row][pos])
        return out_board

    def create_empty_board(self):
        return [[PieceType.Empty for x in range(3)] for x in range(3)]

    def get_available_moves(self, board):
        out_board = []
        for row in range(len(board)):
            for pos in range(len(board[row])):
                if board[row][pos] == PieceType.Empty:
                    out_board.append((row, pos))
        return out_board

    def board_has_moves(self, board):
        for row in board:
            for piece in row:
                if piece == PieceType.Empty:
                    return True
        return False

    def grade_state(self, node, piece, depth):
        # if a win
        if self.check_win(node.board):
            # if the player won, the ai grades the state as a loss
            if self.player == piece:
                return -10
            # if the ai is the winner, grade the state as a win
            return 10
        # its a draw
        return 0

    def generate_move_tree(self, board, piece, depth):

        outNode = Node(board, piece)

        if depth<0 or self.check_win(board):
            return outNode

        available_moves = self.get_available_moves(board)

        for r,c in available_moves:
            new_board = self.copy_board(board)
            op_piece = self.invert_piece(piece)
            new_board[r][c] = op_piece

            node = self.generate_move_tree(new_board, op_piece, depth-1)
            outNode.children.append(node)
        return outNode

    def alpha_beta_minmax(self, node, maximizing_player, depthScore, alpha = -10000, beta = 10000):
        if len(node.children) == 0:
            return self.grade_state(node, node.updated_piece, depthScore)
        if maximizing_player:
            best_value = -10000
            for child in node.children:
                grade = self.alpha_beta_minmax(child, False, depthScore - 1, alpha, beta)
                best_value = max(best_value, grade)
                alpha = max(alpha, best_value)
                if beta <= alpha:
                    break
            return best_value
        else:
            best_value = 10000
            for child in node.children:
                grade = self.alpha_beta_minmax(child, True, depthScore - 1, alpha, beta)
                best_value = min(best_value, grade)
                beta = min(beta, best_value)
                if beta <= alpha:
                    break
            return best_value

    def Ai(self, board, depth):
        tree = self.generate_move_tree(board, self.player, depth)
        for child in tree.children:
            child.value = self.alpha_beta_minmax(child, False, depth)

        max_score = -10000
        index = -1
        for i in range(len(tree.children)):
            if tree.children[i].value > max_score:
                index = i
                max_score = tree.children[i].value
        return tree.children[index].board
