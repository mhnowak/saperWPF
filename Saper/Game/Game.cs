using System;
using System.Collections.Generic;

namespace Saper
{
    public class Game
    {
        #region Private Members

        /// <summary>
        /// helper method to produce random numbers
        /// </summary>
        private Random Rnd { get; set; } = new Random();

        /// <summary>
        /// helper method to produce random numbers
        /// </summary>
        private List<int> HelperList { get; set; } = new List<int>();

        #endregion

        #region Public Members

        /// <summary>
        /// List where are the bombs
        /// </summary>
        public List<bool> BombBoard { get; set; } = new List<bool>();

        /// <summary>
        /// List where are the bombs
        /// </summary>
        public List<bool> ZeroToNull { get; set; } = new List<bool>();

        /// <summary>
        /// Flag
        /// </summary>
        public List<bool> BoolFlag { get; set; } = new List<bool>();

        /// <summary>
        /// List with the numbers on the board
        /// </summary>
        public List<int> Board { get; set; } = new List<int>();

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor
        /// </summary>
        public Game()
        {
            // Prepare the board to contain no bombs at the beginning 
            ListFillWithValue(false, BombBoard, 100);
            ListFillWithValue(false, ZeroToNull, 100);
            ListFillWithValue(false, BoolFlag, 100);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to set up a board
        /// </summary>
        /// <param name="parameter"></param>
        public void SetUpBoard()
        {
            // Iterate thru every bomboard item
            for (int i = 0; i < BombBoard.Count; i++)
            {
                // If bomb is found..
                if (BombBoard[i] == true)
                {
                    // Add a bomb
                    Board.Add(-10);
                }
                else
                    Board.Add(0);
            }

            
            for (int i = 0; i < Board.Count; i++)
            {
                // +1 to the nearest points
                // Corners
                if(Board[i] < 0)
                {
                    if (i == 0)
                    {
                        Board[10]++;
                        Board[11]++;
                        Board[1]++;
                    }
                    else if (i == 9)
                    {
                        Board[8]++;
                        Board[19]++;
                        Board[18]++;
                    }
                    else if (i == 90)
                    {
                        Board[80]++;
                        Board[81]++;
                        Board[91]++;
                    }
                    else if (i == 99)
                    {
                        Board[98]++;
                        Board[88]++;
                        Board[89]++;
                    }
                    else if (i > 0 && i < 9)
                    {
                        Board[i + 10]++;
                        Board[i + 11]++;
                        Board[i + 9]++;
                        Board[i + 1]++;
                        Board[i - 1]++;
                    }
                    else if (i > 90 && i < 99)
                    {
                        Board[i - 10]++;
                        Board[i - 11]++;
                        Board[i - 9]++;
                        Board[i + 1]++;
                        Board[i - 1]++;
                    }
                    else if (i == 10 || i == 20 || i == 30 || i == 40 || i == 50 || i == 60 || i == 70 || i == 80)
                    {
                        Board[i + 11]++;
                        Board[i - 9]++;
                        Board[i + 10]++;
                        Board[i + 1]++;
                        Board[i - 10]++;
                    }
                    else if (i == 19 || i == 29 || i == 39 || i == 49 || i == 59 || i == 69 || i == 79 || i == 89)
                    {
                        Board[i + 9]++;
                        Board[i - 11]++;
                        Board[i + 10]++;
                        Board[i - 1]++;
                        Board[i - 10]++;
                    }
                    else
                    {
                        Board[i + 9]++;
                        Board[i - 9]++;
                        Board[i + 10]++;
                        Board[i + 1]++;
                        Board[i - 10]++;
                        Board[i - 1]++;
                        Board[i - 11]++;
                        Board[i + 11]++;
                    }
                }
            }

            for (int i = 0; i < Board.Count; i++)
            {
                if (Board[i] < 0)
                    Board[i] = -1;
            }

        }

        /// <summary>
        /// Fills board with the bombs
        /// </summary>
        /// <param name="thatMany">How many bombs does your board contain</param>
        public void SetUpBombs(int thatMany)
        {
            // Helper thatMany unique randomed items
            HelperList = RandWORepeat(100, thatMany);

            // In the randomed value put a bomb
            for (int i = 0; i < HelperList.Count; i++)
            {
                BombBoard[HelperList[i]] = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Fills bool list with specified value
        /// </summary>
        /// <param name="value">Fills list with this value</param>
        /// <param name="list">List to fill</param>
        /// <param name="thatMany">That many times list will be filled with this value</param>
        private void ListFillWithValue(bool value, List<bool> list, int thatMany)
        {
            for (int i = 0; i < thatMany; i++)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Get int List without repeating
        /// </summary>
        /// <param name="range">Range of the random numbers</param>
        /// <param name="thatMany">How many items do you want from there</param>
        /// <returns></returns>
        private List<int> RandWORepeat(int range, int thatMany)
        {
            // list to know if this item is reserved or not
            List<bool> numbers = new List<bool>();

            // list to return
            List<int> randomed = new List<int>();

            // on create you have all false values
            ListFillWithValue(false, numbers, range);

            // fill this list thatMany times with random number check if he's already filled
            for (int i = 0; i < thatMany; i++)
            {
                // random num to add
                int rand = 1;

                // get another random number as long as you have some free value
                do
                {
                    rand = Rnd.Next(range);
                } while (numbers[rand] == true);

                // Add this random number to the list and set that this number is already choosed
                randomed.Add(rand);
                numbers[rand] = true;
            }

            // Return randomed list 
            return randomed;
        }

        #endregion

    }
}
