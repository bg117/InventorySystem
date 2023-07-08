using System;
using System.Windows.Input;

namespace InventorySystem.Common;

public class RelayCommand<T> : ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute ?? (_ => true);
    }

    public bool CanExecute(object parameter)
    {
        if (parameter is not T t)
            throw new ArgumentException(
                $"Expected parameter of type {typeof(T).Name}, got {parameter?.GetType().Name ?? "null"}");

        return _canExecute(t);
    }

    public void Execute(object parameter)
    {
        if (parameter is not T t)
            throw new ArgumentException(
                $"Expected parameter of type {typeof(T).Name}, got {parameter?.GetType().Name ?? "null"}");

        _execute(t);
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

public class RelayCommand : ICommand
{
    private readonly Func<bool> _canExecute;
    private readonly Action _execute;

    public RelayCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute ?? (() => true);
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute();
    }

    public void Execute(object parameter)
    {
        _execute();
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}