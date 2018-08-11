from enum import Enum


class PieceType(Enum):
    X = 0
    O = 1
    Empty = 2


# https://stackoverflow.com/a/287944/3344317
class bcolors:
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'