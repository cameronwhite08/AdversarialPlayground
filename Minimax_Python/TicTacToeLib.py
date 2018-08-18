from Constants import PieceType, bcolors
from Node import Node
import random
from itertools import chain
import pickle


class TicTacToeLib:
    player = PieceType.Empty

    # <editor-fold desc="Play Styles">
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
                self.print_board(board)
                continue

            board = self.Ai(board, self.invert_piece(self.player), depth)
            print('Ai Move:')
            self.print_board(board)

            if self.check_win(board):
                print('The PC won!')
                return

            if not self.board_has_moves(board):
                print('Board Full, restarting..')
                board = self.create_empty_board()
                self.print_board(board)
                continue

    def ComputerVsComputer(self, player_piece, depth):
        self.player = player_piece

        board = self.create_empty_board()
        self.print_board(board)

        col = 0

        while col != -1:
            board = self.Ai(board, self.player, depth)
            print('Ai 1 Move:')
            self.print_board(board)

            if self.check_win(board):
                print('Ai 1 won!')
                return

            if not self.board_has_moves(board):
                print('Board Full, restarting..')
                board = self.create_empty_board()
                self.print_board(board)
                continue

            board = self.Ai(board, self.invert_piece(self.player), depth)
            print('Ai 2 Move:')
            self.print_board(board)

            if self.check_win(board):
                print('Ai 2won!')
                return

            if not self.board_has_moves(board):
                print('Board Full, restarting..')
                board = self.create_empty_board()
                self.print_board(board)
                continue

    def ComputerVsComputer_Save(self, player_piece, depth):
        self.player = player_piece

        printing = False

        board = self.create_empty_board()
        if printing:
            self.print_board(board)

        col = 0
        uniques = set()
        all = list()

        while len(uniques) < 10000:
            initial_board = board

            board = self.Ai(board, self.player, depth)
            if printing:
                print('Ai 1 Move:')
                self.print_board(board)

            # save some processing cycles by only computing once
            fib = self.flatten_board(initial_board)
            fb = self.flatten_board(board)

            uniques.add((fib, fb))
            all.append((fib, fb))

            if self.check_win(board):
                print('Ai 1 won!')
                board = self.create_empty_board()
                # return uniques, all
                continue

            if not self.board_has_moves(board):
                board = self.create_empty_board()
                if printing:
                    self.print_board(board)
                    print('Board Full, restarting..')
                continue
            initial_board = board
            board = self.Ai(board, self.invert_piece(self.player), depth)
            if printing:
                print('Ai 2 Move:')
                self.print_board(board)

            # want to have all training data representing the same "start player"
            # get 2x data by using ai1 and ai2 inverted to be like ai1
            # save some processing cycles by only computing once
            fib = self.sanitize_board(self.flatten_board(initial_board))
            fb = self.sanitize_board(self.flatten_board(board))
            uniques.add((fib, fb))
            all.append((fib, fb))

            if self.check_win(board):
                print('Ai 2 won!')
                board = self.create_empty_board()
                # return uniques, all
                continue

            if not self.board_has_moves(board):
                board = self.create_empty_board()
                if printing:
                    self.print_board(board)
                    print('Board Full, restarting..')
                continue
            if len(uniques) % 10 is 0:
                print("Unique len: {0}".format(len(uniques)))
                print("All len: {0}".format(len(all)))
            if len(uniques) % 100 is 0:
                self.save_data(uniques, 'uniques{0}.pkl'.format(len(uniques)))
                self.save_data(all, 'all{0}.pkl'.format(len(all)))

    def ComputerVsDumbComputer_Save(self, player_piece, depth):
        self.player = player_piece

        printing = False

        board = self.create_empty_board()
        if printing:
            self.print_board(board)

        uniques = set()
        all = list()

        while len(uniques) < 10000:
            initial_board = board

            board = self.Ai(board, self.player, depth)
            if printing:
                print('Ai 1 Move:')
                self.print_board(board)

            # save some processing cycles by only computing once
            fib = self.flatten_board(initial_board)
            fb = self.flatten_board(board)

            uniques.add((fib, fb))
            all.append((fib, fb))

            if self.check_win(board):
                print('Ai 1 won!')
                board = self.create_empty_board()
                # return uniques, all
                continue

            if not self.board_has_moves(board):
                board = self.create_empty_board()
                if printing:
                    self.print_board(board)
                    print('Board Full, restarting..')
                continue

            board = self.DumbAi(board, self.invert_piece(self.player), depth)
            if printing:
                print('Ai 2 Move:')
                self.print_board(board)

            if self.check_win(board):
                print('Ai 2 won!')
                board = self.create_empty_board()
                # return uniques, all
                continue

            if not self.board_has_moves(board):
                board = self.create_empty_board()
                if printing:
                    self.print_board(board)
                    print('Board Full, restarting..')
                continue
            if len(uniques) % 10 is 0:
                print("Unique len: {0}".format(len(uniques)))
                print("All len: {0}".format(len(all)))
            if len(uniques) % 100 is 0:
                self.save_data(uniques, 'Pickles/new_uniques{0}.pkl'.format(len(uniques)))
                self.save_data(all, 'Pickles/new_all{0}.pkl'.format(len(all)))

    # </editor-fold>

    def invert_piece(self, piece):
        return PieceType.X if (piece == PieceType.O) else PieceType.O

    def save_data(self, item, filename):
        output = open(filename, 'wb')
        pickle.dump(item, output)
        output.close()

    # <editor-fold desc="Board Operations">
    def check_win(self, board):
        top = (board[0][0] == board[0][1]) and (board[0][1] == board[0][2]) and board[0][2] != PieceType.Empty
        middle = board[1][0] == board[1][1] and board[1][1] == board[1][2] and board[1][2] != PieceType.Empty
        bottom = board[2][0] == board[2][1] and board[2][1] == board[2][2] and board[2][2] != PieceType.Empty

        left = board[0][0] == board[1][0] and board[1][0] == board[2][0] and board[2][0] != PieceType.Empty
        center = board[0][1] == board[1][1] and board[1][1] == board[2][1] and board[2][1] != PieceType.Empty
        right = board[0][2] == board[1][2] and board[1][2] == board[2][2] and board[2][2] != PieceType.Empty

        TLBRDiag = board[0][0] == board[1][1] and board[1][1] == board[2][2] and board[2][2] != PieceType.Empty
        BLTRDiag = board[2][0] == board[1][1] and board[1][1] == board[0][2] and board[0][2] != PieceType.Empty

        return top or middle or bottom or left or center or right or TLBRDiag or BLTRDiag

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

    def flatten_board(self, board):
        flat = tuple(chain.from_iterable(board))
        sanitized = []
        for elem in flat:
            if elem is PieceType.X:
                sanitized.append(1)
            elif elem is PieceType.O:
                sanitized.append(-1)
            else:
                sanitized.append(0)
        return tuple(sanitized)

    def sanitize_board(self, board):
        return tuple(-x if x is not 0 else x for x in board)
    # </editor-fold>

    # <editor-fold desc="AI">
    def grade_state(self, node, piece, depth):
        # if a win
        if self.check_win(node.board):
            # if the updated piece was the current players piece, it is a win
            if node.updated_piece == piece:
                return 10
            # if the updated piece was the other players piece, it is a loss
            return -10
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

    def alpha_beta_minmax(self, node, current_piece, maximizing_player, depthScore, alpha = -10000, beta = 10000):
        if len(node.children) == 0:
            return self.grade_state(node, current_piece, depthScore)
        if maximizing_player:
            best_value = -10000
            for child in node.children:
                grade = self.alpha_beta_minmax(child, current_piece, False, depthScore - 1, alpha, beta)
                best_value = max(best_value, grade)
                alpha = max(alpha, best_value)
                if beta <= alpha:
                    break
            return best_value
        else:
            best_value = 10000
            for child in node.children:
                grade = self.alpha_beta_minmax(child, current_piece, True, depthScore - 1, alpha, beta)
                best_value = min(best_value, grade)
                beta = min(beta, best_value)
                if beta <= alpha:
                    break
            return best_value

    def Ai(self, board, piece, depth):
        tree = self.generate_move_tree(board, self.invert_piece(piece), depth)
        for child in tree.children:
            child.value = self.alpha_beta_minmax(child, piece, False, depth)

        max_score = -10000
        index = -1
        # get the overall max
        for i in range(len(tree.children)):
            if tree.children[i].value > max_score:
                index = i
                max_score = tree.children[i].value
        indices = []
        # get all states that are of equal opportunity to the best
        for i in range(len(tree.children)):
            if tree.children[i].value >= max_score:
                indices.append(i)

        return tree.children[random.choice(indices)].board

    def DumbAi(self, board, piece, depth):

        moves = self.get_available_moves(board)

        ind = random.choice(moves)

        b = self.copy_board(board)

        b[ind[0]][ind[1]] = piece

        return b

    # </editor-fold>

