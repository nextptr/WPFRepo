using Microsoft.Win32;
using simpleMvvm.Common;

namespace simpleMvvm.ViewModels
{
    /// <summary>
    /// 对UI的建模
    /// </summary>
    public class UiViewModel:NotificationObject
    {
        //两个输入数据，一个输出数据
        private double input1;
        public double Input1
        {
            get
            {
                return input1;
            }
            set
            {
                input1 = value;
                this.RaisePropertyChanged("Input1");
            }
        }

        private double input2;
        public double Input2
        {
            get
            {
                return input2;
            }
            set
            {
                input2 = value;
                this.RaisePropertyChanged("Input2");
            }
        }

        private double result;
        public double Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                this.RaisePropertyChanged("Result");
            }
        }

        //两个操作命令

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        private void Add(object parameter)
        {
            this.Result = this.Input1 + this.Input2;
        }
        private void Save(object parametr)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.ShowDialog();
        }

        public UiViewModel()
        {
            //关联命令和实际的操作函数
            this.AddCommand = new DelegateCommand();
            this.AddCommand.ExecuteAction = new System.Action<object>(this.Add);
            this.SaveCommand = new DelegateCommand();
            this.SaveCommand.ExecuteAction = new System.Action<object>(this.Save);
        }
    }
}
