using System.Diagnostics;

namespace MotionDemo.Process
{
    public abstract class ActionBase 
    {
        protected string _name = "";
        private Stopwatch _sw_run = new Stopwatch();
        private bool _isStart = false;
        private bool _isRunning;

    }
}
