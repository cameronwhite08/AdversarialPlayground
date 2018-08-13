from TicTacToeLib import TicTacToeLib
from Constants import PieceType

ai_max_depth = 6

if __name__ == '__main__':
    print('Welcome to TicTacToe!')

    print('Please choose a game option:')
    print('1)Human vs Human')
    print('2)Human Vs Computer')
    print('3)Computer Vs Computer')
    print('4)Computer Vs Computer (Saving the states)')
    game_mode = int(input())

    piece = input('Enter the piece you want to play: \"x\" or \"o\"')

    lib = TicTacToeLib()

    player = PieceType.X if piece is 'x' else PieceType.O
    while True:
        if game_mode == 1:
            lib.HumanVsHuman(player)
            exit()
        if game_mode == 2:
            lib.HumanVsComputer(player, ai_max_depth)
        if game_mode == 3:
            lib.ComputerVsComputer(player, ai_max_depth)
        if game_mode == 4:
            lib.ComputerVsComputer_Save(player, ai_max_depth)




