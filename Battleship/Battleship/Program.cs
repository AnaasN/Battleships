// Notre Dame College - Computer Science A Level Year 1 - Trial Exam January 2017 do not reproduce unless allowed by notre dame catholic sixth form.
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShips
{
    class Program
    {
        public struct ShipType
        {
            public string Name;
            public int Size;
        }

        const string TrainingGame = @"C:\Training.txt";
        const string SaveFilePath = @"\Save.txt";

        private static void GetRowColumn(ref int Row, ref int Column,ref char[,] Board)
        {
            string inColumn;
            string inRow;
            bool UserInputFailled = false;
            bool OutOfRange = false;
            
            while (!UserInputFailled)
            {
                Console.Write("Please enter column: ");
                inColumn = Console.ReadLine();

                Console.Write("Please enter row: ");
                inRow = Console.ReadLine();

                try
                {
                    Column = int.Parse(inColumn);
                    Row = int.Parse(inRow);
                }
                catch
                {
                    Column = -1;
                    Row = -1;
                }
                if(Column == 10 && Column == 10) SaveGame(ref Board);//saves the game state

                if (Column >= 0 && Column <= 9 && Row >= 0 && Row <= 9) OutOfRange = false;
                else
                {
                    Console.WriteLine("Incorrect Placement, has to be a numerical value between 0 and 9.");
                    OutOfRange = true;
                }
                if(!OutOfRange) UserInputFailled = true;
            }//end while.

        }

        private static void MakePlayerMove(ref char[,] Board, ref ShipType[] Ships)
        {
            int Row = 0;
            int Column = 0;
            GetRowColumn(ref Row, ref Column, ref Board);
            if (Board[Row, Column] == 'm' || Board[Row, Column] == 'h')
            {
                Console.WriteLine("Sorry, you have already shot at the square (" + Column + "," + Row + "). Please try again.");
            }
            else if (Board[Row, Column] == '-')
            {
                Console.WriteLine("Sorry, (" + Column + "," + Row + ") is a miss.");
                Board[Row, Column] = 'm';
            }
            else
            {
                Console.WriteLine("Hit at (" + Column + "," + Row + ").");
                Board[Row, Column] = 'h';
            }
        }

        private static void SetUpBoard(ref char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    Board[Row, Column] = '-';
                }
            }
        }

        private static void LoadGame(string TrainingGame, ref char[,] Board, ref bool inFileError)//exception handling added. remember to reformat the txt file.
        {
            string Line = "";
            StreamReader BoardFile = new StreamReader(TrainingGame);
            try
            {
                for (int Row = 0; Row < 10; Row++)
                {
                    Line = BoardFile.ReadLine();
                    for (int Column = 0; Column < 10; Column++)
                    {
                        Board[Row, Column] = Line[Column];
                    }
                }
                BoardFile.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("An error has occoured while attempting to read the Training.txt file.\n  Please check the file and restart.");
                inFileError = true;
            } 
        }

        private static void PlaceRandomShips(ref char[,] Board, ShipType[] Ships)
        {
            Random RandomNumber = new Random();
            bool Valid;
            char Orientation = ' ';
            int Row = 0;
            int Column = 0;
            int HorV = 0;

            foreach (ShipType Ship in Ships)
            {
                Valid = false;
                while (Valid == false)
                {
                    Row = RandomNumber.Next(0, 10);
                    Column = RandomNumber.Next(0, 10);
                    HorV = RandomNumber.Next(0, 2);
                    if (HorV == 0)
                    {
                        Orientation = 'v';
                    }
                    else
                    {
                        Orientation = 'h';
                    }
                    Valid = ValidateBoatPosition(Board, Ship, Row, Column, Orientation);
                }
                Console.WriteLine("Computer placing the " + Ship.Name);
                PlaceShip(ref Board, Ship, Row, Column, Orientation);
            }
        }

        private static void PlaceShip(ref char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row + Scan, Column] = Ship.Name[0];
                }
            }
            else if (Orientation == 'h')
            {
                for (int Scan = 0; Scan < Ship.Size; Scan++)
                {
                    Board[Row, Column + Scan] = Ship.Name[0];
                }
            }
        }

        private static bool ValidateBoatPosition(char[,] Board, ShipType Ship, int Row, int Column, char Orientation)
        {
            if (Orientation == 'v' && Row + Ship.Size > 10)
            {
                return false;
            }
            else if (Orientation == 'h' && Column + Ship.Size > 10)
            {
                return false;
            }
            else
            {
                if (Orientation == 'v')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row + Scan, Column] != '-')
                        {
                            return false;
                        }
                    }
                }
                else if (Orientation == 'h')
                {
                    for (int Scan = 0; Scan < Ship.Size; Scan++)
                    {
                        if (Board[Row, Column + Scan] != '-')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static bool CheckWin(char[,] Board)
        {
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static void PrintBoard(char[,] Board)
        {
            Console.WriteLine();
            Console.WriteLine("The board looks like this: ");
            Console.WriteLine();
            Console.Write(" ");
            for (int Column = 0; Column < 10; Column++)
            {
                Console.Write(" " + Column + "  ");
            }
            Console.WriteLine();
            for (int Row = 0; Row < 10; Row++)
            {
                Console.Write(Row + " ");
                for (int Column = 0; Column < 10; Column++)
                {
                    if (Board[Row, Column] == '-')
                    {
                        Console.Write(" ");
                    }
                    else if (Board[Row, Column] == 'A' || Board[Row, Column] == 'B' || Board[Row, Column] == 'S' || Board[Row, Column] == 'D' || Board[Row, Column] == 'P')
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(Board[Row, Column]);
                    }
                    if (Column != 9)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine();
                
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine(@" _____     _        _____            ");
            Console.WriteLine(@"|     |___|_|___   |     |___ ___ _ _ ");
            Console.WriteLine(@"| | | | .'| |   |  | | | | -_|   | | |");
            Console.WriteLine(@"|_|_|_|__,|_|_|_|  |_|_|_|___|_|_|___|");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Load training game");
            Console.WriteLine("9. Quit");
            Console.WriteLine();
        }

        private static int GetMainMenuChoice()
        {
            int Choice = 0;
            bool IncorrectInput = false;
            Console.Write("Please enter your choice: ");

            while (!IncorrectInput)
            {
                try
                {
                    Choice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Choice = -1;
                }
                if(Choice == -1)
                {
                    IncorrectInput = !IncorrectInput;
                }
                else
                {
                    IncorrectInput = false;
                }
                Console.WriteLine();
                return Choice;
            }
            return 0;
        }

        private static void PlayGame(ref char[,] Board, ref ShipType[] Ships)
        {
            bool GameWon = false;
            while (GameWon == false)
            {
                PrintBoard(Board);
                MakePlayerMove(ref Board, ref Ships);
                GameWon = CheckWin(Board);
                if (GameWon == true)
                {
                    Console.WriteLine("All ships sunk!");
                    Console.WriteLine();
                    //SaveGame(ref Board);
                }
            }
        }

        private static void SetUpShips(ref ShipType[] Ships)
        {
            Ships[0].Name = "Aircraft Carrier";
            Ships[0].Size = 5;
            Ships[1].Name = "Battleship";
            Ships[1].Size = 4;
            Ships[2].Name = "Submarine";
            Ships[2].Size = 3;
            Ships[3].Name = "Destroyer";
            Ships[3].Size = 3;
            Ships[4].Name = "Patrol Boat";
            Ships[4].Size = 2;
        }

        static void Main(string[] args)//exception handling added.
        {
            ShipType[] Ships = new ShipType[5];
            char[,] Board = new char[10, 10];
            int MenuOption = 0;
            bool FileError = false;

            StartBanner();

            while (MenuOption != 9)
            {
                SetUpBoard(ref Board);
                SetUpShips(ref Ships);
                DisplayMenu();
                MenuOption = GetMainMenuChoice();

                if (MenuOption == 1)
                {
                    NewGameBanner();
                    PlaceRandomShips(ref Board, Ships);
                    PlayGame(ref Board, ref Ships);
                }
                else if (MenuOption == 2)
                {
                    TrainingBanner();
                    Console.WriteLine("!To save the game enter 10 as coloumn and 10 as row!");
                    LoadGame(TrainingGame, ref Board, ref FileError);
                    if(FileError == false)
                    {
                        PlayGame(ref Board, ref Ships);
                    }
                }
            }
        }

        static void SaveGame(ref char[,] Board)
        {
            StreamWriter Save = new StreamWriter(SaveFilePath);
            for (int Row = 0; Row < 10; Row++)
            {
                for (int Column = 0; Column < 10; Column++)
                {
                    Save.Write(Board[Row, Column]);
                }
                Save.Write("\n");
            }
            Save.Flush();
            Save.Close();
            Console.WriteLine("!GAME STATE SAVED!");
        }

        //Graphics

        static void StartBanner()
        {
            Console.WriteLine(@"######                                    #####                         ");
            Console.WriteLine(@"#     #   ##   ##### ##### #      ###### #     # #    # # #####   ####  ");
            Console.WriteLine(@"#     #  #  #    #     #   #      #      #       #    # # #    # #      ");
            Console.WriteLine(@"######  #    #   #     #   #      #####   #####  ###### # #    #  ####  ");
            Console.WriteLine(@"#     # ######   #     #   #      #            # #    # # #####       # ");
            Console.WriteLine(@"#     # #    #   #     #   #      #      #     # #    # # #      #    # ");
            Console.WriteLine(@"######  #    #   #     #   ###### ######  #####  #    # # #       ####  ");
        }

        static void TrainingBanner()
        {
            Console.WriteLine(@" _______        _       _             ");
            Console.WriteLine(@"|__   __|      (_)     (_)            ");
            Console.WriteLine(@"   | |_ __ __ _ _ _ __  _ _ __   __ _ ");
            Console.WriteLine(@"   | | '__/ _` | | '_ \| | '_ \ / _` |");
            Console.WriteLine(@"   | | | | (_| | | | | | | | | | (_| |");
            Console.WriteLine(@"   |_|_|  \__,_|_|_| |_|_|_| |_|\__, |");
            Console.WriteLine(@"                                 __/ |");
            Console.WriteLine(@"                                |___/ ");
        }

        static void NewGameBanner()
        {
            Console.WriteLine(@"..................................................................");
            Console.WriteLine(@".%%..%%..%%%%%%..%%...%%...........%%%%....%%%%...%%...%%..%%%%%%.");
            Console.WriteLine(@".%%%.%%..%%......%%...%%..........%%......%%..%%..%%%.%%%..%%.....");
            Console.WriteLine(@".%%.%%%..%%%%....%%.%.%%..........%%.%%%..%%%%%%..%%.%.%%..%%%%...");
            Console.WriteLine(@".%%..%%..%%......%%%%%%%..........%%..%%..%%..%%..%%...%%..%%.....");
            Console.WriteLine(@".%%..%%..%%%%%%...%%.%%............%%%%...%%..%%..%%...%%..%%%%%%.");
            Console.WriteLine(@"..................................................................");
        }

        //Graphics
    }
}
