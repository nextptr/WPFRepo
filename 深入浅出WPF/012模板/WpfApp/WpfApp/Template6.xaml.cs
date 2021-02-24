using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Template6.xaml 的交互逻辑
    /// </summary>
    public partial class Template6 : UserControl
    {
        List<plcBtnEntity> plcBtnEntities = new List<plcBtnEntity>();

        public Template6()
        {
            InitializeComponent();
            initPlcBtn();
            initEvent();
        }
        private void initPlcBtn()
        {
            //0
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray分离X前进", 104, 102, 104, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray分离X后退", 105, 103, 105, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray定位Y前进", 108, 106, 108, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray定位Y后退", 109, 107, 109, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray整理前进", 112, 110, 112, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray整理后退", 113, 111, 113, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray整理前进", 116, 114, 116, "3", "4"));
            plcBtnEntities.Add(new plcBtnEntity("PL1_Tray整理后退", 117, 115, 117, "3", "4"));

            //8
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray分离X前进", 134, 132, 134, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray分离X后退", 135, 133, 135, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray定位Y前进", 138, 136, 138, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray定位Y后退", 139, 137, 139, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray整理前进", 142, 140, 142, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray整理后退", 143, 141, 143, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray整理前进", 146, 144, 146, "3", "4"));
            plcBtnEntities.Add(new plcBtnEntity("PL2_Tray整理后退", 147, 145, 147, "3", "4"));

            //16
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray分离X前进", 164, 162, 164, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray分离X后退", 165, 163, 165, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray定位Y前进", 168, 166, 168, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray定位Y后退", 169, 167, 169, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray整理前进", 172, 170, 172, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray整理后退", 173, 171, 173, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray整理前进", 176, 174, 176, "3", "4"));
            plcBtnEntities.Add(new plcBtnEntity("PU1_Tray整理后退", 177, 175, 177, "3", "4"));

            //24
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray分离X前进", 194, 192, 194, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray分离X后退", 195, 193, 195, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray定位Y前进", 198, 196, 198, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray定位Y后退", 199, 197, 199, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray整理前进", 202, 200, 202, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray整理后退", 203, 201, 203, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray整理前进", 206, 204, 206, "3", "4"));
            plcBtnEntities.Add(new plcBtnEntity("PU2_Tray整理后退", 207, 205, 207, "3", "4"));

            //32
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray分离X前进", 224, 222, 224, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray分离X后退", 225, 223, 225, "X1", "X2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray定位Y前进", 228, 226, 228, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray定位Y后退", 229, 227, 229, "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray整理前进", 232, 230, 232, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray整理后退", 233, 231, 233, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray整理前进", 236, 234, 236, "3", "4"));
            plcBtnEntities.Add(new plcBtnEntity("PU3_Tray整理后退", 237, 235, 237, "3", "4"));

            //40
            plcBtnEntities.Add(new plcBtnEntity("Tray定位平台前进", 256, 250, 252, 254, 256, "X1", "X2", "Y1", "Y2"));
            plcBtnEntities.Add(new plcBtnEntity("Tray定位平台前进", 257, 251, 253, 255, 257, "X1", "X2", "Y1", "Y2"));

            //42
            plcBtnEntities.Add(new plcBtnEntity("上料Cell搬送吸真空", 259, 259));
            plcBtnEntities.Add(new plcBtnEntity("上料Cell搬送破真空", 260, 260));
            plcBtnEntities.Add(new plcBtnEntity("上料Tray搬送吸真空", 261, 261));
            plcBtnEntities.Add(new plcBtnEntity("上料Tray搬送破真空", 262, 262));

            //46
            plcBtnEntities.Add(new plcBtnEntity("下料Cell搬送吸真空", 280, 280));
            plcBtnEntities.Add(new plcBtnEntity("下料Cell搬送破真空", 281, 281));
            plcBtnEntities.Add(new plcBtnEntity("下料Tray搬送吸真空", 282, 282));
            plcBtnEntities.Add(new plcBtnEntity("下料Tray搬送破真空", 283, 283));

            //50
            plcBtnEntities.Add(new plcBtnEntity("ULD_Cell搬送Z气缸上升", 290, 290));
            plcBtnEntities.Add(new plcBtnEntity("ULD_Cell搬送Z气缸下降", 291, 291));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台气缸伸出", 292, 292));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台气缸缩回", 293, 293));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台气缸伸出", 294, 294));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台气缸缩回", 295, 295));
            plcBtnEntities.Add(new plcBtnEntity("超声波清洁开", 296, 296));
            plcBtnEntities.Add(new plcBtnEntity("超声波清洁关", 297, 297));

            //58
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台吸真空1", 300, 300));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台破真空1", 301, 301));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台吸真空2", 302, 302));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台破真空2", 303, 303));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台真空检3", 304, 304));
            plcBtnEntities.Add(new plcBtnEntity("LD翻转平台破真空3", 305, 305));
            plcBtnEntities.Add(new plcBtnEntity("LD缓存平台吸真空1", 308, 306, 308, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("LD缓存平台破真空1", 309, 307, 309, "1", "2"));

            plcBtnEntities.Add(new plcBtnEntity("机器人吸真空", 310, 310));
            plcBtnEntities.Add(new plcBtnEntity("机器人破真空", 311, 311));

            //68
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台吸真空1", 320, 320));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台破真空1", 321, 321));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台吸真空2", 322, 322));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台破真空2", 323, 323));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台吸真空3", 324, 324));
            plcBtnEntities.Add(new plcBtnEntity("ULD翻转平台破真空3", 325, 325));
            plcBtnEntities.Add(new plcBtnEntity("ULD缓存平台吸真空", 326, 326, 328, "1", "2"));
            plcBtnEntities.Add(new plcBtnEntity("ULD缓存平台破真空", 327, 327, 329, "1", "2"));
        }

        private void initEvent()
        {
            setElement(grid8_1, 0, 8);
            setElement(grid8_2, 8, 16);
            setElement(grid8_3, 16, 24);
            setElement(grid8_4, 24, 32);
            setElement(grid8_5, 32, 40);

            setElement(grid2_1, 40, 42);

            setElement(grid4_1, 42, 46);
            setElement(grid4_2, 46, 50);

            setElement(grid8_6, 50, 58);
            setElement(grid8_7, 58, 66);
            setElement(grid2_2, 66, 68);

            setElement(grid8_8, 68, 76);
        }

        private void setElement(UniformGrid uni,int beg,int end)
        {
            for (int i = beg; i < end; i++)
            {
                Button btn = new Button();
                btn.DataContext = plcBtnEntities[i];
                btn.PreviewMouseLeftButtonDown += BtnCylinder_PreviewMouseLeftButtonDown;
                btn.PreviewMouseLeftButtonUp += BtnCylinder_PreviewMouseLeftButtonUp;
                uni.Children.Add(btn);
            }
        }
        private void BtnCylinder_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button b = sender as Button;
            plcBtnEntity entity = b.DataContext as plcBtnEntity;
            entity.Status1 = true;
        }
        private void BtnCylinder_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button b = sender as Button;
            plcBtnEntity entity = b.DataContext as plcBtnEntity;
        }
    }

    public class plcBtnEntity : NotifyPropertyChanged
    {
        private string pointName = "";
        private int _posCount = 1;
        private bool _status1 = false;
        private bool _status2 = false;
        private bool _status3 = false;
        private bool _status4 = false;

        private string _description1 = "";
        private string _description2 = "";
        private string _description3 = "";
        private string _description4 = "";

        public int ReadCmdId1 = -1;
        public int ReadCmdId2 = -1;
        public int ReadCmdId3 = -1;
        public int ReadCmdId4 = -1;
        public int WritCmdId = -1;
        public string WriteCmd = "";

        public string PointName
        {
            get
            {
                return pointName;
            }
            set
            {
                pointName = value;
                OnPropertyChanged("PointName");
            }
        }
        public int PosCount
        {
            get
            {
                return _posCount;
            }
            set
            {
                _posCount = value;
                OnPropertyChanged(nameof(PosCount));
            }
        }
        public bool Status1
        {
            get => _status1;
            set
            {
                _status1 = value;
                OnPropertyChanged(nameof(Status1));
            }
        }
        public bool Status2
        {
            get => _status2;
            set
            {
                _status2 = value;
                OnPropertyChanged(nameof(Status2));
            }
        }
        public bool Status3
        {
            get => _status3;
            set
            {
                _status3 = value;
                OnPropertyChanged(nameof(Status3));
            }
        }
        public bool Status4
        {
            get => _status4;
            set
            {
                _status4 = value;
                OnPropertyChanged(nameof(Status4));
            }
        }


        public string Description1
        {
            get
            {
                return _description1;
            }
            set
            {
                _description1 = value;
                OnPropertyChanged("Description1");
            }
        }
        public string Description2
        {
            get
            {
                return _description2;
            }
            set
            {
                _description2 = value;
                OnPropertyChanged("Description2");
            }
        }
        public string Description3
        {
            get
            {
                return _description3;
            }
            set
            {
                _description3 = value;
                OnPropertyChanged("Description3");
            }
        }
        public string Description4
        {
            get
            {
                return _description4;
            }
            set
            {
                _description4 = value;
                OnPropertyChanged("Description4");
            }
        }


        public plcBtnEntity(string name, int writId, int readId1, string des1 = "")
        {
            PointName = name;
            ReadCmdId1 = readId1;
            Description1 = des1;
            WritCmdId = writId;
            WriteCmd = $"PC_TO_PLC_CMD[{writId}]";
        }
        public plcBtnEntity(string name, int writId, int readId1, int readId2, string des1, string des2)
        {
            PointName = name;
            ReadCmdId1 = readId1;
            ReadCmdId2 = readId2;
            Description1 = des1;
            Description2 = des2;
            WritCmdId = writId;
            PosCount = 2;
            WriteCmd = $"PC_TO_PLC_CMD[{writId}]";
        }
        public plcBtnEntity(string name, int writId, int readId1, int readId2, int readId3, int readId4, string des1, string des2, string des3, string des4)
        {
            PointName = name;
            ReadCmdId1 = readId1;
            ReadCmdId2 = readId2;
            ReadCmdId3 = readId3;
            ReadCmdId4 = readId4;
            Description1 = des1;
            Description2 = des2;
            Description3 = des3;
            Description4 = des4;
            WritCmdId = writId;
            PosCount = 4;
            WriteCmd = $"PC_TO_PLC_CMD[{writId}]";
        }
    }
}
