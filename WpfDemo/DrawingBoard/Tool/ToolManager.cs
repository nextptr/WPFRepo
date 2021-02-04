using System.Collections.Generic;
using DrawingBoard.Primitive;

namespace DrawingBoard.Tool
{
    public class ToolManager
    {
        public static ToolManager Instance { get { return GetInstance(); } }
        private static ToolManager _instance = null;

        private List<ITool> _tools;
        
        public  Stack<PrimitiveBase> _undoStk;
        public Stack<PrimitiveBase> _redoStk;

        private ToolManager()
        {
            _tools = new List<ITool>();
            _tools.Add(new ToolPointer());
            _tools.Add(new ToolDot());
            _tools.Add(new ToolLine());
            _tools.Add(new ToolRectangle());
            _tools.Add(new ToolCircle());
            _tools.Add(new ToolPolyLine());
            _undoStk = new Stack<PrimitiveBase>();
            _redoStk = new Stack<PrimitiveBase>();
        }

        private static ToolManager GetInstance()
        {
            if (null == _instance)
            {
                _instance = new ToolManager();
            }
            return _instance;
        }

        public ITool Get(ToolType toolType)
        {
            return _tools[(int)toolType];
        }
    }
}
