using System;
using System.Windows.Input;

namespace Saper
{
    public class AdvancedRelayCommand : ICommand
    {
        #region Private Members

        /// <summary>
        /// Action to execute
        /// </summary>
        private Action<object> _action { get; set; }

        /// <summary>
        /// Boolean that specifies if you can or cannot execute an action
        /// </summary>
        private Predicate<object> _canExecute { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="action"></param>
        /// <param name="canExecute"></param>
        public AdvancedRelayCommand(Action<object> action, Predicate<object> canExecute)
        {
            _action = action ?? throw new NullReferenceException("action");
            _canExecute = canExecute;
        }

        /// <summary>
        /// Constructor 2? TO:DO 
        /// </summary>
        /// <param name="action"></param>
        public AdvancedRelayCommand(Action<object> action) : this(action, null)
        {

        }

        #endregion

        #region Public Events

        /// <summary>
        /// Event that is fired when the value <see cref="CanExecute(object)"/> value has changed
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Can execute whether received true or false
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action.Invoke(parameter);
        }

        #endregion
    }
}
