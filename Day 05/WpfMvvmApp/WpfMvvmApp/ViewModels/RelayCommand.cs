using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmApp.ViewModels
{

    //ViewModel과 View를 Glue하기 위한 클래스
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;     //실행처리를 위한 generic
        private readonly Predicate<T> canExecute;//실행여부를 판단하는 generic

        //실행여부에 따라서 이벤트를 추가해주거나 삭제하는 이벤트 핸들러
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }

        }
        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }


        //execute만 파라미터 받는 생성자
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        //execute, canExecute 를 파라미터로 받는 생성자
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentException(nameof(execute));
            this.canExecute = canExecute;
        }
    }
}
