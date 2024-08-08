using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DBoardGameRevise
{
    //The Board class that creates the board game itself.
    class Board
    {
        int boardSpaceNum = 0, totalSpaceP = 0, totalSpaceC = 0;
        string boardSpace = "-";
        string boardSpace2 = "*";
        string pStart;
        string CStart;
        string[] playerSpace;
        string[] newBoard;
        string[] CPUSpace;
        string[,] boardLayout;

        public Board(int bsn, string pStartSymbol, string CStartSymbol)
        {
            boardSpaceNum = bsn;
            pStart = pStartSymbol;
            CStart = CStartSymbol;
            playerSpace = new string[boardSpaceNum];
            newBoard = new string[boardSpaceNum];
            CPUSpace = new string[boardSpaceNum];
            boardLayout = new string[3,boardSpaceNum];
            for (int i = 0; i < boardSpaceNum; i++)
            {
                playerSpace[i] = boardSpace2;
                newBoard[i] = boardSpace;
                CPUSpace[i] = boardSpace2;
            }
        }

        //Creates the board after the Board object is created.
        public void createBoard()
        {
            //Finish figuring out the display later.
            for(int i1 = 0; i1 < boardSpaceNum; i1++)
            {
                boardLayout[0, i1] = playerSpace[i1];
                boardLayout[1, i1] = newBoard[i1];
                boardLayout[2, i1] = CPUSpace[i1];
            }
            boardLayout[0, 0] = pStart;
            boardLayout[2, 0] = CStart;
            for (int i2 = 0; i2 < 3; i2++)
            {
                for (int i3 = 0; i3 < boardSpaceNum; i3++)
                {
                    Console.Write(boardLayout[i2, i3]);
                }
                Console.WriteLine();
            }
        }

        //Prints the board.
        public void getBoard()
        {
            for (int i2 = 0; i2 < 3; i2++)
            {
                for (int i3 = 0; i3 < boardSpaceNum; i3++)
                {
                    Console.Write(boardLayout[i2, i3]);
                }
                Console.WriteLine();
            }
        }

        //Determines the condition to end the game.
        public bool endGame()
        {
            bool youWin = false;
            if (boardLayout[0,boardSpaceNum - 1] == pStart && !(boardLayout[0, boardSpaceNum - 1] == pStart && boardLayout[2, boardSpaceNum - 1] == CStart) && !(boardLayout[2, boardSpaceNum - 1] == CStart))
            {
                Console.WriteLine("You Win!");
                youWin = true;
            }
            else if (boardLayout[2, boardSpaceNum - 1] == CStart && !(boardLayout[0, boardSpaceNum - 1] == pStart && boardLayout[2, boardSpaceNum - 1] == CStart) && !(boardLayout[0, boardSpaceNum - 1] == pStart))
            {
                Console.WriteLine("CPU Wins!");
                youWin = true;
            }
            else if (boardLayout[0, boardSpaceNum - 1] == pStart && boardLayout[2, boardSpaceNum - 1] == CStart)
            {
                Console.WriteLine("It's a Tie!");
                youWin = true;
            }
            return youWin;
        }

        public int getBoardSpace()
        {
            return boardSpaceNum;
        }

        public void editBoardP(int playerNewSpace)
        {
            totalSpaceP = totalSpaceP + playerNewSpace;
            for (int i = 0; i < boardSpaceNum; i++)
            {
                if (boardLayout[0, i] == pStart)
                {
                    boardLayout[0, i] = "*";
                    break;
                }
            }

            if (totalSpaceP - 1 > boardSpaceNum - 1)
            {
                boardLayout[0, boardSpaceNum - 1] = pStart;
            }
            else
            {
                boardLayout[0, totalSpaceP - 1] = pStart;
            }
        }
        public void editBoardC(int CPUNewSpace)
        {
            totalSpaceC = totalSpaceC + CPUNewSpace;
            for (int i = 0; i < boardSpaceNum; i++)
            {
                if (boardLayout[2, i] == CStart)
                {
                    boardLayout[2, i] = "*";
                    break;
                }
            }

            if (totalSpaceC - 1 > boardSpaceNum - 1)
            {
                boardLayout[2, boardSpaceNum - 1] = CStart;
            }
            else
            {
                boardLayout[2, totalSpaceC - 1] = CStart;
            }
        }
    }

    //Abstract method to create a basis for a certain player or CPU.
    abstract class Playable
    {
        int score = 0;

        public abstract string getSymbol();

        public int getScore()
        {
            return score;
        }

        public void setScore(int newScore)
        {
            score = newScore;
        }

        public void addScore(int newScore)
        {
            score = score + newScore;
        }

        public void subtractScore(int newScore)
        {
            score = score - newScore;
        }
    }

    //Player Object
    class Player : Playable
    {
        string playerSymbol = "1";

        public override string getSymbol()
        {
            return playerSymbol;
        }
    }

    //CPU object
    class CPU : Playable
    {
        string CPUSymbol = "2";

        public override string getSymbol()
        {
            return CPUSymbol;
        }
    }

    class Program
    {
        //A method to have the player pick a dice type to use.
        public static int dicePick(Playable playPoint, int scoreUse)
        {
            Random rnd = new Random();
            int diceValue = 0, scoreCurrent = scoreUse, diceSwitch;
            do
            {
                Console.WriteLine("Pick a dice");
                Console.WriteLine("1. Standard Dice");
                Console.WriteLine("2. Risky Dice");
                Console.WriteLine("3. Set Value Dice");
                diceSwitch = Convert.ToInt32(Console.ReadLine());
                if (diceSwitch > 3 && diceSwitch < 1)
                {
                    Console.WriteLine("Number should be between 1-3");
                }
            } while (diceSwitch > 3 && diceSwitch < 1);

            if (diceSwitch == 1)
            {
                diceValue = rnd.Next(1, 6);
            }
            else if (diceSwitch == 2 && scoreCurrent == 3)
            {
                playPoint.subtractScore(3);
                diceValue = rnd.Next(-2, 8);
            }
            else if (diceSwitch == 3 && scoreCurrent == 5)
            {
                playPoint.subtractScore(5);
                do
                {
                    Console.WriteLine("Pick a value between 1-8");
                    diceValue = Convert.ToInt32(Console.ReadLine());
                    if (diceValue > 8 && diceValue < 1)
                    {
                        Console.WriteLine("Dice value should be between 1-8");
                    }
                } while (diceValue > 8 && diceValue < 1);
            }
            return diceValue;
        }

        //AI program for the CPU.
        public static int CPUai(Playable useCPUPoint)
        {
            Random rnd = new Random();
            int diceLetter, diceValue = 0;
            string[] choice = new string[2];

            //The points the CPU has gets analyzed.
            if (useCPUPoint.getScore() < 3)
            {
                choice[0] = "A";
            }
            else if (useCPUPoint.getScore() >= 3 && useCPUPoint.getScore() < 5)
            {
                choice[0] = "B";
            }
            else if (useCPUPoint.getScore() >= 5)
            {
                choice[0] = "C";
            }

            do
            {
                //CPU picks the dice to use. (A = Standard Dice, B = Risky Dice, C = Set Value Dice)
                diceLetter = rnd.Next(1, 3);
                switch (diceLetter)
                {
                    case 1:
                        choice[1] = "A";
                        break;
                    case 2:
                        choice[1] = "B";
                        break;
                    case 3:
                        choice[1] = "C";
                        break;
                }

                if (choice[0] == "A" && choice[1] == "A")
                {
                    diceValue = rnd.Next(1, 6);
                }
                else if (choice[0] == "B" && choice[1] == "A")
                {
                    diceValue = rnd.Next(1, 6);
                }
                else if (choice[0] == "C" && choice[1] == "A")
                {
                    diceValue = rnd.Next(1, 6);
                }
                else if (choice[0] == "B" && choice[1] == "B")
                {
                    useCPUPoint.subtractScore(3);
                    diceValue = rnd.Next(-2, 8);
                }
                else if (choice[0] == "C" && choice[1] == "C")
                {
                    useCPUPoint.subtractScore(5);
                    diceValue = rnd.Next(1, 8);
                }
                else if (choice[0] == "C" && choice[1] == "B")
                {
                    useCPUPoint.subtractScore(3);
                    diceValue = rnd.Next(-2, 8);
                }
            //The do while loop repeats if there are certain unobtainable conditions.
            } while ((choice[0] == "A" && choice[1] == "B") || (choice[0] == "A" && choice[1] == "C") || (choice[0] == "B" && choice[1] == "C"));

            return diceValue;
        }
        //Main Method
        static void Main(string[] args)
        {
            Playable p1 = new Player();
            Playable c1 = new CPU();
            Board b1 = new Board(25, p1.getSymbol(), c1.getSymbol());
            int playerDice, CPUDice;

            b1.createBoard();
            Console.WriteLine("Welcome to the Board Game");
            Console.WriteLine("Press any button to begin");
            Console.ReadKey();

            //Creates a loop to keep the game going until the win condition (b1.endGame()) is satisfied.
            do
            {
                playerDice = dicePick(p1, p1.getScore());
                b1.editBoardP(playerDice);
                Console.WriteLine("Player rolled " + playerDice);
                p1.addScore(1);
                Console.WriteLine("Player score: " + p1.getScore());

                CPUDice = CPUai(c1);
                b1.editBoardC(CPUDice);
                Console.WriteLine("CPU rolled " + CPUDice);
                c1.addScore(1);
                Console.WriteLine("CPU score: " + c1.getScore());

                b1.getBoard();
                b1.endGame();
            } while (b1.endGame() == false);
        }
    }
}
