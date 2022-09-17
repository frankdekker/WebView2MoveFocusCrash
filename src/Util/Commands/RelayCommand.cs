using System;
using System.Windows.Input;
using static Util.Utility.Asserts;

namespace Util.Commands;

public class RelayCommand : RelayCommand<object?>
{
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : base(execute, canExecute) { }
}

public class RelayCommand<T> : ICommand
{
    #region Fields

    private readonly Action<T> execute;
    private readonly Predicate<T>? canExecute;

    #endregion

    #region Constructors

    public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    #endregion

    #region ICommand Members

    ///<summary>
    ///Defines the method that determines whether the command can execute in its current state.
    ///</summary>
    ///<param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
    ///<returns>
    ///true if this command can be executed; otherwise, false.
    ///</returns>
    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute(IsType<T>(parameter));
    }

    ///<summary>
    ///Occurs when changes occur that affect whether or not the command should execute.
    ///</summary>
    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    ///<summary>
    ///Defines the method to be called when the command is invoked.
    ///</summary>
    ///<param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
    public void Execute(object? parameter)
    {
        execute(IsType<T>(parameter));
    }

    #endregion
}