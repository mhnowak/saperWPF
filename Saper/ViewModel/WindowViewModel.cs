using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Saper
{
    public class WindowViewModel : BaseViewModel
    {
        #region Private Members

        /// <summary>
        /// Game Class
        /// </summary>
        private Game PlaySaper { get; set; } = new Game();

        /// <summary>
        /// Private Window
        /// </summary>
        private Window _Window;

        /// <summary>
        /// Can Execute
        /// </summary>
        private CanExecuteList CanExecuteList { get; set; }

        /// <summary>
        /// That many squares are on the board
        /// </summary>
        private int SquaresCount { get; set; } = 100;

        /// <summary>
        /// How many bombs u already guessed (if all bombs are flagged u won) 
        /// It's named Test because it has to be changed
        /// </summary>
        private int Test { get; set; }

        #endregion

        #region Game Proporties

        /// <summary>
        /// Actual Time
        /// </summary>
        public string SecsS { get; set; } = "00";

        /// <summary>
        /// Actual Time
        /// </summary>
        public string MinsS { get; set; } = "00";

        /// <summary>
        /// Bombs - Flags
        /// </summary>
        public int FlagsCount { get; set; }

        /// <summary>
        /// Flag
        /// </summary>
        public bool FlagBool { get; set; }

        /// <summary>
        /// Square Value List
        /// </summary>
        public SquareValueList SquareValue { get; set; }

        #endregion

        #region Commands

        /// <summary>
        /// Logo Command
        /// </summary>
        public ICommand LogoCommand { get; set; }

        /// <summary>
        /// Minimize button
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// Close buttoon
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// Easy Level button
        /// </summary>
        public ICommand SquareCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// <param name="window"></param>
        public WindowViewModel(Window window)
        {
            // Pass window (needed to minimize and close buttons)
            _Window = window;

            // What's the content of the button
            SquareValue = new SquareValueList(SquaresCount);

            // Definies if square is active or not 
            CanExecuteList = new CanExecuteList(SquaresCount);

            // Create Commands
            MinimizeCommand = new RelayCommand(() => _Window.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(() => _Window.Close());
            LogoCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(_Window, GetMousePosition()));
            SquareCommand = new AdvancedRelayCommand(RandomSquare, RandomSquareCanExecute);
            
            // Sets up the board
            SaperGameExe();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Square button
        /// </summary>
        /// <param name="parameter"></param>
        private void RandomSquare(object parameter)
        {
            // If timer isn't active then active it as soon as user clicks a button
            if (IsGoing == false)
                Start();

            // Flagging
            if (FlagBool == true)
            { 
                // Just flag it 
                Flag(parameter);
            }
            // Playing
            else
            {
                // Put a number in it or if it's a bomb u lose or if it's a 0 reveal squares around
                RevealSquare(parameter);
            }
            // If you scored 16 test poins u won
            if (Test == 16)
                Win();
        }

        /// <summary>
        /// Definies if RandomSquare can execute (If it has been pressed already it can't)
        /// </summary>
        /// <param name="parameter"></param>
        private bool RandomSquareCanExecute(object parameter)
        {
            return CanExecuteList[Convert.ToInt32(parameter)];
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Clicking on a button reveals a number
        /// </summary>
        /// <param name="parameter"></param>
        private void RevealSquare(object parameter)
        {
            // Put this number into the board
            SquareValue[Convert.ToInt32(parameter)] = Convert.ToString(PlaySaper.Board[Convert.ToInt32(parameter)]);

            // Something to make Zero Selected work
            PlaySaper.ZeroToNull[Convert.ToInt32(parameter)] = true;

            // You can't execute this button again
            CanExecuteList[Convert.ToInt32(parameter)] = false;

            // If u found 0 u found at least another 8 squares
            if (PlaySaper.Board[Convert.ToInt32(parameter)] == 0)
            {
                // Zero Selected method
                ZeroSelected(Convert.ToInt32(parameter));
            }
            // If you found a bomb the game is over
            else if (PlaySaper.Board[Convert.ToInt32(parameter)] < 0)
                Lose();
        }

        /// <summary>
        /// Flag
        /// </summary>
        private void Flag(object parameter)
        {
            // Unflag
            if (SquareValue[Convert.ToInt32(parameter)] != "")
            {
                // after unflagging remove the test point too
                Test -= PlaySaper.BombBoard[Convert.ToInt32(parameter)] == true ? 1 : -1;

                // Flags used ++ because u removed some
                FlagsCount++;

                // When u unflag leave the square empty
                SquareValue[Convert.ToInt32(parameter)] = "";
            }
            // Flagging
            else
            {
                // Add test point if u flagged a bomb
                Test += PlaySaper.BombBoard[Convert.ToInt32(parameter)] == true ? 1 : -1;

                // -- Flag because u flagged smth
                FlagsCount--;

                // Put an F icon inside the square so u know that its flagged
                SquareValue[Convert.ToInt32(parameter)] = "F";
            }
        }

        /// <summary>
        /// Helper method to clear all stats
        /// </summary>
        private void Over()
        {
            // Every square is ""
            SquareValue = new SquareValueList(SquaresCount);

            // From now on any button can execute
            CanExecuteList = new CanExecuteList(SquaresCount);

            // new game new board etc. etc.
            PlaySaper = new Game();

            // new Game
            SaperGameExe();

            // Timer
            Reset();
        }

        /// <summary>
        /// Does smth when u win
        /// </summary>
        private void Win()
        {
            // Freeze the timer when u win
            Freeze();

            // Message to show 
            MessageBox.Show("You won!");

            // Reset variables
            Over();
        }

        /// <summary>
        /// Does smth when u lose
        /// </summary>
        private void Lose()
        {
            // Freeze the timer when u lose
            Freeze();

            // Message to show 
            MessageBox.Show("Game Over");

            // Reset variables
            Over();
        }

        /// <summary>
        /// Sets up a board and other important things on the beginning
        /// </summary>
        private void SaperGameExe()
        {
            // Reset the win value
            Test = 0;

            // Count flags from 16
            FlagsCount = 16;
            // Sets up the bombs
            PlaySaper.SetUpBombs(FlagsCount);

            // Fills up board with items
            PlaySaper.SetUpBoard();

        }

        /// <summary>
        /// Whenever you found zero
        /// </summary>
        /// <param name="p">parameter</param>
        private void ZeroSelected(int p)
        {
            // Make number visible
            SquareValue[p] = PlaySaper.Board[p] == 0 ? "" : PlaySaper.Board[p].ToString();

            // You can't click this button anymore
            CanExecuteList[p] = false;

            PlaySaper.ZeroToNull[p] = true;

            // Top
            if (!(p == 0 || p == 90 || p == 10 || p == 20 || p == 30 || p == 40 || p == 50 || p == 60 || p == 70 || p == 80))
                AnotherZero(p, -1);

            // Bottom
            if (!(p == 9 || p == 99 || p == 19 || p == 29 || p == 39 || p == 49 || p == 59 || p == 69 || p == 79 || p == 89))
                AnotherZero(p, 1);

            // Right
            if(!((p >= 90 && p <= 99)))
                AnotherZero(p, 10);

            // Left
            if (!(p >= 0 && p <= 9))
                AnotherZero(p, -10);

            // Right-Top
            if (!((p >= 90 && p <= 99) || p == 10 || p == 20 || p == 30 || p == 40 || p == 50 || p == 60 || p == 70 || p == 80 || p == 0))
                AnotherZero(p, 9);

            // Right-Bot
            if (!(p == 9 || p == 90 || p == 99 || (p > 90 && p < 99) || p == 19 || p == 29 || p == 39 || p == 49 || p == 59 || p == 69 || p == 79 || p == 89))
                AnotherZero(p, 11);
            
            // Left-Top
            if(!((p >= 0 && p <= 10) || p == 10 || p == 20 || p == 30 || p == 40 || p == 50 || p == 60 || p == 70 || p == 80 || p == 90))
                AnotherZero(p, -11);

            // Left-Bot
            if (!((p>=0 && p<=9) || p == 19 || p == 29 || p == 39 || p == 49 || p == 59 || p == 69 || p == 79 || p == 89 || p == 99))
                AnotherZero(p, -9);
                        
        }

        /// <summary>
        /// Whenever you find another zero
        /// </summary>
        /// <param name="p">parameter(p)</param>
        /// <param name="val">Cordinates of the item</param>
        private void AnotherZero(int p, int val)
        {
            // Call another Item only when it's 0 and only when it has nothing inside
            if ((PlaySaper.Board[p + val] == 0) && (PlaySaper.ZeroToNull[p + val] == false))
            {
                // Call this item if it's 0
                ZeroSelected(p + val);
            }

            // Show this item
            SquareValue[p + val] = PlaySaper.Board[p + val] == 0 ? "" : PlaySaper.Board[p + val].ToString();
            PlaySaper.ZeroToNull[p + val] = true;
            // Make it uncicable
            CanExecuteList[p + val] = false;
        }

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns>Mouse position</returns>
        private Point GetMousePosition()
        {
            // Position of the mouse relative to the window
            var position = Mouse.GetPosition(_Window);

            // Add the window position so its a "ToScreen"
            return new Point(position.X + _Window.Left, position.Y + _Window.Top);
        }

        #endregion

        #region Timer

        /// <summary>
        /// Seconds
        /// </summary>
        private int Secs { get; set; } = 0;

        /// <summary>
        /// Minutes
        /// </summary>
        private int Mins { get; set; } = 0;

        /// <summary>
        /// Is Timer going or not
        /// </summary>
        public bool IsGoing { get; set; } = false;

        /// <summary>
        /// Starts the timer
        /// </summary>
        public async Task Start()
        {
            // Timer is now going
            IsGoing = true;

            // Wait 1 s till u start increasing i
            await Task.Delay(1000);

            // as long as timer is going
            while (IsGoing == true)
            {
                // Every second add 1 to seconds
                Secs++;

                // If u hit 60s reset it and add 1min
                if(Secs == 60)
                {
                    Secs = 0;
                    Mins++;
                }
                // Makes sure everything statys as 2 digit number
                SecsS = Secs < 10 ? "0" + Convert.ToString(Secs) : Convert.ToString(Secs);
                MinsS = Mins < 10 ? "0" + Convert.ToString(Mins) : Convert.ToString(Mins);

                // wait 1 second till u do everything again
                await Task.Delay(1000);
            }
        }

        /// <summary>
        /// Resets the timer
        /// </summary>
        public void Reset()
        {
            Secs = 0;
            Mins = 0;
            MinsS = "00";
            SecsS = "00";
        }

        /// <summary>
        /// Freezes the timer
        /// </summary>
        public void Freeze()
        {
            IsGoing = false;
        }

        #endregion
    }
}
