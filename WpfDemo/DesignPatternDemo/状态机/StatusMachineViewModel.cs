using DesignPatternDemo.common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Common;
using System.Collections.ObjectModel;
using System.Windows;

namespace DesignPatternDemo.StatusMachine
{
    public class StatusMachineViewModel : Screen, IPage
    {
        private string name = "状态机";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private TestParam param;
        public TestParam Param
        {
            get
            {
                return param;
            }
            set
            {
                param = value;
                OnPropertyChanged(nameof(Param));
            }
        }

        private StatusProgress proc = new StatusProgress();

        private ObservableCollection<int> measureDatas;
        public ObservableCollection<int> MeasureDatas
        {
            get
            {
                return measureDatas;
            }
            set
            {
                measureDatas = value;
                OnPropertyChanged(nameof(MeasureDatas));
            }
        }

        public StatusMachineViewModel()
        {
            MeasureDatas = new ObservableCollection<int>();
            Param = new TestParam();
            proc.DataReceiveEvent += Proc_DataReceiveEvent;
            proc.MeasureFinishEvent += Proc_MeasureFinishEvent;
        }

        private void Proc_DataReceiveEvent(object arg)
        {
            MeasureDatas.Add((int)arg);
        }
        private void Proc_MeasureFinishEvent(bool arg)
        {
            MessageBox.Show("测试结束");
        }

        public void btnBack()
        {
            var router = IoC.Get<IRouter>();
            router.GoBack();
            Param = new TestParam();
        }
        public async void btnStart()
        {
            try
            {
                proc.TimeAcq = this.Param.TimeAcq * 1000;
                proc.DelyTim = this.Param.DelyTime * 1000;
                proc.Seed =    this.Param.Seed;

                bool retFlag = false;
                await Task.Run(() =>
                {
                    retFlag = proc.ReStartProgress();
                });
                if (retFlag)
                {
                    //MeasureProcess = 100;
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void btnStop()
        {
            if (proc == null )
                return;
            proc.Stop();
        }
    }

    public class TestParam: NotifyPropertyChanged
    {
        private int seed;
        public int Seed
        {
            get
            {
                return seed;
            }
            set
            {
                seed = value;
                OnPropertyChanged(nameof(Seed));
            }
        }

        private int delyTime;
        public int DelyTime
        {
            get
            {
                return delyTime;
            }
            set
            {
                delyTime = value;
                OnPropertyChanged(nameof(DelyTime));
            }
        }

        private int timeAcq;
        public int TimeAcq
        {
            get
            {
                return timeAcq;
            }
            set
            {
                timeAcq = value;
                OnPropertyChanged(nameof(TimeAcq));
            }
        }

        public TestParam()
        {
            this.Seed = DateTime.Now.Millisecond;
            this.DelyTime =1;
            this.TimeAcq = 2;
        }
    }
}
