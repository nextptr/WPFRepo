using Stylet;
using System.Linq;


namespace MathSemulation
{
    public class MainViewModel : Screen
    {
        private MathSemViewModel _MathSem;
        public MathSemViewModel MathSem
        {
            get { return _MathSem; }
            set
            {
                _MathSem = value;
                NotifyOfPropertyChange(nameof(MathSem));
            }
        }

        private OXYpoltViewModel _OXYpolt;
        public OXYpoltViewModel OXYpolt
        {
            get { return _OXYpolt; }
            set
            {
                _OXYpolt = value;
                NotifyOfPropertyChange(nameof(OXYpolt));
            }
        }

        //OnPropertyChanged
        public MainViewModel()
        {
            MathSem = new MathSemViewModel();
            OXYpolt = new OXYpoltViewModel();
            MathSem.AreaEvent += OXYpolt.AddVal;
        }
    }
}
