using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingBoard.Primitive;

namespace DrawingBoard.DrawingControl
{
    public class CommandStack
    {

        public static CommandStack Instance { get { return GetInstance(); } }

        private static CommandStack _instance = null;

        // 记录操作的类型


        // 命令栈中存放的元素对象


        public Stack UndoStack; // Undo撤销栈
        public Stack RedoStack; // Redo恢复栈
        public CommandStack()
        {
            UndoStack = new Stack(); // 构造函数中实例化
            RedoStack = new Stack(); // 构造函数中实例化
        }

        private static CommandStack GetInstance()
        {
            if (null == _instance)
            {
                _instance = new CommandStack();
            }
            return _instance;
        }

        /// <summary>
        /// 往Undo撤销命令栈中添加一个元素
        /// </summary>
        /// <param name="commandimgType">被操作的Image控件</param>
        /// <param name="commandType">命令的种类</param>
        /// <param name="obj">附带的数据</param>
        //public CommandInfo Add(List<PrimitiveBase> pbsList, CommandActionType commandType, ScanDocumentParameterItem scanDocItem = null, PrimitiveLaserParameterDictionary dic = null)
        //{
        //    CommandInfo commandInfo = new CommandInfo();
        //    commandInfo.PbsArry = pbsList;
        //    commandInfo.CommandType = commandType;
        //    commandInfo.ScanDocumentParameterItem = scanDocItem;
        //    commandInfo.DicLaser = dic;
        //    return commandInfo;
        //    // 压入栈中，这里没有考虑栈的容量
        //    //UndoStack.Push(commandInfo);          
        //}

    }

    //public class CommandInfo
    //{
    //    public PrimitiveBase Pbs { get; set; } // 被操作的前台Image控件
    //    public CommandActionType CommandType { get; set; } // 操作的类型

    //    public ScanDocumentParameterItem ScanDocumentParameterItem { get; set; }

    //    public int PrmIndex { get; set; }

    //    public List<PrimitiveBase> PbsArry { get; set; }

    //    public LaserParameterItem LaserItem { get; set; }

    //    public PrimitiveLaserParameterDictionary DicLaser { get; set; }

    //}

    public enum CommandActionType
    {
        Move,       // 平移或者修改
        Add,     // 新增
        Del,    // 删除
        Cut,     // 剪切
        Copy,    // 右转
        Update, //修改属性
    }
}
