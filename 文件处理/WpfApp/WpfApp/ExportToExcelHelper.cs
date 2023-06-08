using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfApp.Common;

namespace WpfApp
{
    public class ExportToExcelHelper
    {
        private readonly string _baseDir = AppDomain.CurrentDomain.BaseDirectory + "\\ExportData\\";
        private const int MaxRowCount = 65535;
        private const int UserAllowMaxRowCount = 500;
        public Action<string> msg = null;

        private static ExportToExcelHelper _instance;
        public static ExportToExcelHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExportToExcelHelper();
                }
                return _instance;
            }
            private set { }
        }
        private ExportToExcelHelper()
        {
            CheckDirExist(_baseDir);

        }

        /// <summary>
        /// HSSFWorkbook .xls 
        /// XSSFWorkbook .xlsx 
        /// </summary>
        /// <param name="excelItem"></param>
        public void CreateExcel(ExcelFile excelFile)
        {
            //创建Excel表
            HSSFWorkbook workbook = new HSSFWorkbook();

            //设置Header
            ICellStyle columnHeaderStyle = workbook.CreateCellStyle();
            columnHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;
            columnHeaderStyle.FillPattern = FillPattern.SolidForeground;
            columnHeaderStyle.Alignment = HorizontalAlignment.Center;

            //设置背景色
            ICellStyle cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            cellStyle1.FillPattern = FillPattern.SolidForeground;
            cellStyle1.Alignment = HorizontalAlignment.Center;

            ICellStyle cellStyle2 = workbook.CreateCellStyle();
            cellStyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            cellStyle2.FillPattern = FillPattern.SolidForeground;
            cellStyle2.Alignment = HorizontalAlignment.Center;

            for (int i = 0; i < excelFile.ListSheet.Count; i++)
            {
                int sheetIndex = 0;

                //取出一个表
                ExcelSheet sheetItem = excelFile.ListSheet[i];
                ISheet sheet = workbook.CreateSheet(sheetItem.SheetName + "【" + sheetIndex + "】");

                int dataRowCount = sheetItem.ListColums[0].Cols.Count;
                int dataColCount = sheetItem.ListColums.Count;
                int sheetRowCounter = 0;

                for (int j = 0; j < dataRowCount; j++)
                {
                    if (sheetRowCounter >= MaxRowCount)  //大于表规定行数，建立新表
                    {
                        sheetRowCounter = 0;
                        sheetIndex++;
                        sheet = workbook.CreateSheet(sheetItem.SheetName + "【" + sheetIndex + "】");

                        if (j != 0) //重建新表后，建立Header将会占用一次J++，因此需要J--
                            j--;
                    }

                    IRow row = sheet.CreateRow(sheetRowCounter); //创建Excel行
                    for (int k = 0; k < dataColCount; k++)       //将列数据写入Excel行中
                    {
                        if (sheetRowCounter == 0)
                        {
                            ICell cell = row.CreateCell(k);
                            cell.SetCellValue(sheetItem.ListColums[k].ColHeader);
                            cell.CellStyle = columnHeaderStyle;

                            //设置列宽
                            int columnWidth = sheet.GetColumnWidth(k) / 256;

                            int length = Encoding.UTF8.GetBytes(cell.ToString()).Length;

                            if (columnWidth < length + 1)
                            {
                                columnWidth = length + 1;
                            }

                            sheet.SetColumnWidth(k, columnWidth * 256);
                        }
                        else
                        {
                            ICell cell = row.CreateCell(k);
                            cell.SetCellValue(sheetItem.ListColums[k].Cols[j]);

                            if (sheetRowCounter % 2 == 0)
                            {
                                cell.CellStyle = cellStyle1;
                            }
                            else
                            {
                                cell.CellStyle = cellStyle2;
                            }
                        }
                    }

                    sheetRowCounter++;
                }
            }

            FileStream file2007 = new FileStream(_baseDir + "\\" + excelFile.FileName + ".xls", FileMode.Create);
            workbook.Write(file2007);
            file2007.Close();
            workbook.Close();
        }

        public void CreateHeader(string baseDir, string workSheetName, List<string> columnHeader)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            //设置Header
            ICellStyle columnHeaderStyle = workbook.CreateCellStyle();
            columnHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;
            columnHeaderStyle.FillPattern = FillPattern.SolidForeground;
            columnHeaderStyle.Alignment = HorizontalAlignment.Center;

            //设置背景色
            ICellStyle cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            cellStyle1.FillPattern = FillPattern.SolidForeground;
            cellStyle1.Alignment = HorizontalAlignment.Center;

            ICellStyle cellStyle2 = workbook.CreateCellStyle();
            cellStyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            cellStyle2.FillPattern = FillPattern.SolidForeground;
            cellStyle2.Alignment = HorizontalAlignment.Center;

            ISheet sheet = null;
            int sheetCounter = 0;

            while (true)
            {
                sheet = workbook.GetSheet("台盘数据【" + sheetCounter + "】");

                if (sheet == null)
                {
                    sheet = workbook.CreateSheet("台盘数据【" + sheetCounter + "】");
                    break;
                }
                else
                {
                    if (sheet.LastRowNum >= UserAllowMaxRowCount)
                    {
                        sheetCounter++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int lastRowNum = sheet.LastRowNum;

            if (lastRowNum == 0)
            {
                IRow rowHeader = sheet.CreateRow(lastRowNum);

                for (int i = 0; i < columnHeader.Count; i++)
                {
                    ICell cell = rowHeader.CreateCell(i);
                    cell.SetCellValue(columnHeader[i]);
                    cell.CellStyle = columnHeaderStyle;

                    //设置列宽
                    int columnWidth = sheet.GetColumnWidth(i) / 256;

                    int length = Encoding.UTF8.GetBytes(cell.ToString()).Length;

                    if (columnWidth < length + 1)
                    {
                        columnWidth = length + 1;
                    }

                    sheet.SetColumnWidth(i, columnWidth * 256);
                }

                lastRowNum++;
            }

            FileStream file2007 = new FileStream(baseDir + "\\" + workSheetName + ".xls", FileMode.Create);
            workbook.Write(file2007);
            file2007.Close();
            workbook.Close();
        }

        public void ExportRowDataToExcel(string baseDir, string workSheetName, List<string> columnHeader, List<string> rowData)
        {
            HSSFWorkbook workbook = null;
            FileStream file2007 = null;

            if (File.Exists(baseDir + "\\" + workSheetName + ".xls"))
            {
                file2007 = new FileStream(baseDir + "\\" + workSheetName + ".xls", FileMode.OpenOrCreate, FileAccess.Read);
                //file2007.Seek(0, SeekOrigin.Begin);

                workbook = WorkbookFactory.Create(file2007) as HSSFWorkbook;

            }
            else
            {
                workbook = new HSSFWorkbook();
            }


            //设置Header
            ICellStyle columnHeaderStyle = workbook.CreateCellStyle();
            columnHeaderStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index;
            columnHeaderStyle.FillPattern = FillPattern.SolidForeground;
            columnHeaderStyle.Alignment = HorizontalAlignment.Center;

            //设置背景色
            ICellStyle cellStyle1 = workbook.CreateCellStyle();
            cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            cellStyle1.FillPattern = FillPattern.SolidForeground;
            cellStyle1.Alignment = HorizontalAlignment.Center;

            ICellStyle cellStyle2 = workbook.CreateCellStyle();
            cellStyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            cellStyle2.FillPattern = FillPattern.SolidForeground;
            cellStyle2.Alignment = HorizontalAlignment.Center;

            ISheet sheet = null;
            int sheetCounter = 0;

            while (true)
            {
                sheet = workbook.GetSheet("台盘数据【" + sheetCounter + "】");

                if (sheet == null)
                {
                    sheet = workbook.CreateSheet("台盘数据【" + sheetCounter + "】");
                    break;
                }
                else
                {
                    if (sheet.LastRowNum >= UserAllowMaxRowCount)
                    {
                        sheetCounter++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            int lastRowNum = sheet.LastRowNum;

            if (lastRowNum == 0)
            {
                IRow rowHeader = sheet.CreateRow(lastRowNum);

                for (int i = 0; i < columnHeader.Count; i++)
                {
                    ICell cell = rowHeader.CreateCell(i);
                    cell.SetCellValue(columnHeader[i]);
                    cell.CellStyle = columnHeaderStyle;

                    //设置列宽
                    int columnWidth = sheet.GetColumnWidth(i) / 256;

                    int length = Encoding.UTF8.GetBytes(cell.ToString()).Length;

                    if (columnWidth < length + 1)
                    {
                        columnWidth = length + 1;
                    }

                    sheet.SetColumnWidth(i, columnWidth * 256);
                }
            }

            IRow row = sheet.CreateRow(++lastRowNum);

            for (int i = 0; i < rowData.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(rowData[i]);

                if (lastRowNum % 2 == 0)
                {
                    cell.CellStyle = cellStyle1;
                }
                else
                {
                    cell.CellStyle = cellStyle2;
                }
            }

            if (!File.Exists(baseDir + "\\" + workSheetName + ".xls"))
            {
                file2007 = new FileStream(baseDir + "\\" + workSheetName + ".xls", FileMode.OpenOrCreate, FileAccess.Write);
                workbook.Write(file2007);
                file2007.Close();
                workbook.Close();
            }
            else
            {
                file2007 = new FileStream(baseDir + "\\" + workSheetName + ".xls", FileMode.OpenOrCreate, FileAccess.Write);
                workbook.Write(file2007);
                file2007.Close();
                workbook.Close();
            }
        }

        private void CheckDirExist(string productDir)
        {
            if (!Directory.Exists(productDir))
            {
                Directory.CreateDirectory(productDir);
            }
        }

        /// <summary>
        /// 根据HTML文件生成PlcConfig.txt
        /// </summary>
        /// <param name="srcFileName"></param>
        /// <param name="dstFileName"></param>
        public void ReadPlc(string srcFileName, string dstFileName)
        {
            msg?.Invoke("开始处理PLC文件");
            XSSFWorkbook srcWorkbook = null;
            FileStream srcFileStream = null;
            FileStream dstFileStream = new FileStream(dstFileName, FileMode.Create, FileAccess.Write);
            StreamWriter dstStreamWrite = new StreamWriter(dstFileStream);

            if (File.Exists(srcFileName))
            {
                srcFileStream = new FileStream(srcFileName, FileMode.OpenOrCreate, FileAccess.Read);
                srcWorkbook = WorkbookFactory.Create(srcFileStream) as XSSFWorkbook;
            }
            else
            {
                msg?.Invoke("PLC文件为空");
                return;
            }
            createAxis(srcWorkbook, dstStreamWrite);          //创建轴参数
            createTPTLAndGroup(srcWorkbook, dstStreamWrite,out int ioIndex);  //创建TP、TL数据以及分组
            createIOState(srcWorkbook, dstStreamWrite, ioIndex);       //IO状态监控
            createAlarmWarning(srcWorkbook, dstStreamWrite);  //添加报警数据
            dstStreamWrite.Close();
            dstFileStream.Close();
            srcFileStream.Close();
            srcWorkbook.Close();

            msg?.Invoke("PLC文件处理完成");
        }

        private void createAxis(XSSFWorkbook srcBook, StreamWriter stream)
        {
            ISheet sheet = srcBook.GetSheet("MotionParaInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在MotionParaInfo表");
                return;
            }

            Dictionary<int, plcAxis> dic = new Dictionary<int, plcAxis>();

            int sheetRowCount = sheet.LastRowNum;
            for (int i = 0; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line == null)
                {
                    continue;
                }
                try
                {
                    if (line.Cells[0].CellType == CellType.Numeric)
                    {//创建新轴
                        int id = (int)line.Cells[0].NumericCellValue;
                        plcAxis tmp = new plcAxis();
                        tmp.ID = id;
                        tmp.Name = line.Cells[1].StringCellValue;
                        dic[id] = tmp;
                        if (line.Cells[3].CellType == CellType.String && getAxisTrig2(line.Cells[3].StringCellValue, out int axisId, out int trigId))
                        {
                            var pos = line.Cells[4].NumericCellValue;
                            var vel = line.Cells[5].NumericCellValue;
                            dic[axisId].TrigLs.Add(new axisTrig(trigId, line.Cells[2].StringCellValue, pos, vel));
                        }
                    }
                    else if (line.Cells[0].CellType == CellType.Blank
                          && line.Cells[2].CellType == CellType.String
                          && line.Cells[3].CellType == CellType.String)
                    {//创建trig
                        double pos = 0;
                        double vel = 0;
                        if (line.Cells[4].CellType == CellType.Numeric)
                        {
                            pos = line.Cells[4].NumericCellValue;
                            vel = line.Cells[5].NumericCellValue;
                        }
                        if (getAxisTrig2(line.Cells[3].StringCellValue, out int axisId, out int trigId))
                        {
                            dic[axisId].TrigLs.Add(new axisTrig(trigId, line.Cells[2].StringCellValue, pos, vel));
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            //写入Axis文件
            writAxis(dic, stream);
        }
        private bool getAxisTrig(string arg, out int axisId, out int trigId)
        {
            axisId = trigId = -1;
            int i = 0;
            for (; i < arg.Length; i++)
            {
                string tmp = "";
                if (arg[i] == '[')
                {
                    i++;
                    for (int j = i; j < arg.Length; j++)
                    {
                        if (arg[j] != ']')
                        {
                            tmp += arg[j];
                        }
                        else
                        {
                            axisId = int.Parse(tmp);
                            goto lab;
                        }
                    }
                }
            }

            lab:

            for (; i < arg.Length; i++)
            {
                string tmp = "";
                if (arg[i] == '[')
                {
                    i++;
                    for (int j = i; j < arg.Length; j++)
                    {
                        if (arg[j] != ']')
                        {
                            tmp += arg[j];
                        }
                        else
                        {
                            trigId = int.Parse(tmp);
                            goto end;
                        }
                    }
                }
            }

            end:
            return (-1 != axisId) && (-1 != trigId);
        }
        private bool getAxisTrig2(string arg, out int axisId, out int trigId)
        {
            axisId = trigId = -1;
            try
            {
                var arr = arg.Split('.');
                if (arr.Length == 2)
                {
                    var r1 = Regex.Match(arr[0], @"\d+");
                    var r2 = Regex.Match(arr[1], @"\d+");
                    int.TryParse(r1.Value, out axisId);
                    int.TryParse(r2.Value, out trigId);
                    return true;
                }
            }
            catch
            { }
            return false;
        }
        private void writAxis(Dictionary<int, plcAxis> dic, StreamWriter stream)
        {
            foreach (var ky in dic.Keys)
            {
                stream.WriteLine($"//{dic[ky].Name}");
                stream.WriteLine("[Mode = Position, GroupName = State]");
                stream.WriteLine($"SDC_AXIS_STATUS.SDC_AXIS_STATUS_Struct_Block.ListOfAxisStatusStruct({dic[ky].ID})");
                stream.WriteLine("{");
                stream.WriteLine("    InplaceSignal()");
                stream.WriteLine("    { ");
                if (dic[ky].TrigLs.Count == 0)
                {
                    stream.WriteLine($"		bool Inplace_{dic[ky].ID}_0 (0) : InPosFlag_0 ");
                }
                else
                {
                    foreach (var item in dic[ky].TrigLs)
                    {
                        stream.WriteLine($"		bool Inplace_{dic[ky].ID}_{item.ID} ({item.ID}) : InPosFlag_{item.ID} ");
                    }
                }
                stream.WriteLine("	}");
                stream.WriteLine("}");
                stream.WriteLine("[Mode = Position, GroupName = Control]");
                stream.WriteLine($"SDC_AXIS_CMD.SDC_AXIS_CMD_Struct_Block.ListOfAxisCMDStruct({dic[ky].ID})    ");
                stream.WriteLine("{");
                stream.WriteLine("    PositionTrigger()");
                stream.WriteLine("    { ");
                if (dic[ky].TrigLs.Count == 0)
                {
                    stream.WriteLine($"	    PulseOffOnOff PositionTrigger_{dic[ky].ID}_0 (0)  : PosMove_sw_0 ");
                }
                else
                {
                    foreach (var item in dic[ky].TrigLs)
                    {
                        stream.WriteLine($"	    PulseOffOnOff PositionTrigger_{dic[ky].ID}_{item.ID} ({item.ID})  : PosMove_sw_{item.ID} ");
                    }
                }
                stream.WriteLine("	}");
                stream.WriteLine("}");
                stream.WriteLine("[Mode = Position, GroupName = Value]");
                stream.WriteLine($"SDC_AXIS_MotionPAR.SDC_AXIS_MotionPAR_Struct_Block.ListOfAxisMotionPara({dic[ky].ID})");
                stream.WriteLine("{");
                stream.WriteLine("    Poses()");
                stream.WriteLine("    { ");
                foreach (var item in dic[ky].TrigLs)
                {
                    if (item.Name == "备用")
                    {
                        stream.WriteLine($"	   float 备用 ({item.ID} , min: 0.000, max: 100.000, default: 10.000)  : Pos_{item.ID} ");
                    }
                    else
                    {
                        double min = 0;
                        double max = 0;
                        double dft = item.DefaultPos;
                        if (dft > 0)
                        {
                            max = dft + 100;
                        }
                        else if (dft < 0)
                        {
                            min = dft - 100;
                        }
                        else if (dft == 0)
                        {
                            max = 100;
                        }
                        stream.WriteLine($"	   float {item.Name} ({item.ID} , min: {min.ToString("f2")}, max: {max.ToString("f2")}, default: {dft.ToString("f2")})  : Pos_{item.ID} ");
                    }
                }

                stream.WriteLine("	}");
                stream.WriteLine("}");
                stream.WriteLine("[Mode = Position, GroupName = Velocity]");
                stream.WriteLine($"SDC_AXIS_MotionPAR.SDC_AXIS_MotionPAR_Struct_Block.ListOfAxisMotionPara({dic[ky].ID})");
                stream.WriteLine("{");
                stream.WriteLine("    Vels()");
                stream.WriteLine("    { ");
                foreach (var item in dic[ky].TrigLs)
                {
                    if (item.Name == "备用")
                    {
                        stream.WriteLine($"	   float vel_{dic[ky].ID}_{item.ID} ({item.ID}, min: 1.0, max: 5.0, default: 1.0)  : Vel_{item.ID} ");
                    }
                    else
                    {
                        stream.WriteLine($"	   float vel_{dic[ky].ID}_{item.ID} ({item.ID}, min: 1.0, max: {(item.DefaultVel + item.DefaultVel / 2).ToString("f1")}, default: {item.DefaultVel.ToString("f1")})  : Vel_{item.ID} ");
                    }
                }
                stream.WriteLine("	}");
                stream.WriteLine("}");
            }
        }

        private void createTPTLAndGroup(XSSFWorkbook srcBook, StreamWriter stream ,out int ioId)
        {
            ioId = 0;
            ISheet sheet = srcBook.GetSheet("TPInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在TPInfo表");
                return;
            }
            List<ioItem> TPLs = new List<ioItem>();
            int sheetRowCount = sheet.LastRowNum;
            for (int i = 1; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count == 2)
                {
                    TPLs.Add(new ioItem() { Key = "null", Value = line.Cells[1].StringCellValue, Index = i - 1 });
                }
                else if (line.Cells.Count == 3)
                {
                    TPLs.Add(new ioItem() { Key = line.Cells[2].StringCellValue, Value = line.Cells[1].StringCellValue, Index = i - 1 });
                }
                else
                {
                    TPLs.Add(new ioItem() { Key = "略", Value = "", Index = i - 1 });
                }
            }

            sheet = srcBook.GetSheet("TLInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在TLInfo表");
                return;
            }
            List<string> TLLs = new List<string>();
            sheetRowCount = sheet.LastRowNum;
            for (int i = 0; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count >= 2)
                {
                    TLLs.Add(line.Cells[1].StringCellValue);
                }
                else
                {
                    TLLs.Add("");
                }
            }

            stream.WriteLine("");
            stream.WriteLine("//IO控制部分");
            stream.WriteLine("[Mode = IOCmd, GroupName = Whole, GroupIndex = 0]");
            stream.WriteLine("SDC_PARA.SDC_PARA_Struct_Block()");
            stream.WriteLine("{");
            stream.WriteLine("	IOControls()");
            stream.WriteLine("	{");

            //IO分组
            Dictionary<string, List<ioItem>> dic = new Dictionary<string, List<ioItem>>();
            for (int i = 0; i < TPLs.Count; i++)
            {
                if (TPLs[i].Value != "" && TLLs[i] != "")
                {
                    TPLs[i].FullStr = $"		bool {TPLs[i].Value} ({i},\"{TPLs[i].Key}\" -> TL_{i}) : TP_{i}";
                    stream.WriteLine(TPLs[i].FullStr);
                    if (TPLs[i].Key != "")
                    {
                        if (!dic.ContainsKey(TPLs[i].Key))
                        {
                            dic[TPLs[i].Key] = new List<ioItem>();
                        }
                        dic[TPLs[i].Key].Add(TPLs[i]);
                    }
                }
            }
            stream.WriteLine("	}");
            stream.WriteLine("}");
            //生成IoGroup

            int gid = 1;
            foreach (var ky in dic.Keys)
            {
                stream.WriteLine($"[Mode = IOCmd, GroupName = {ky}, GroupIndex = {gid}]");
                stream.WriteLine($"SDC_PARA.SDC_PARA_Struct_Block()");
                stream.WriteLine('{');
                iopair(stream, dic[ky]);
                stream.WriteLine('}');
                gid++;
            }
            ioId = gid;
        }
        private void createTPTL(XSSFWorkbook srcBook, XSSFWorkbook dstBook)
        {
            ISheet sheet = srcBook.GetSheet("TPInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在TPInfo表");
                return;
            }
            List<string> TPLs = new List<string>();
            int sheetRowCount = sheet.LastRowNum;
            for (int i = 1; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count >= 2)
                {
                    TPLs.Add(line.Cells[1].StringCellValue);
                }
                else
                {
                    TPLs.Add("");
                }
            }

            sheet = srcBook.GetSheet("TLInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在TLInfo表");
                return;
            }
            List<string> TLLs = new List<string>();
            sheetRowCount = sheet.LastRowNum;
            for (int i = 0; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count >= 2)
                {
                    TLLs.Add(line.Cells[1].StringCellValue);
                }
                else
                {
                    TLLs.Add("");
                }
            }

            //设置背景色
            ICellStyle cellStyle1 = dstBook.CreateCellStyle();
            cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            cellStyle1.FillPattern = FillPattern.SolidForeground;
            cellStyle1.Alignment = HorizontalAlignment.Left;

            ICellStyle cellStyle2 = dstBook.CreateCellStyle();
            cellStyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            cellStyle2.FillPattern = FillPattern.SolidForeground;
            cellStyle2.Alignment = HorizontalAlignment.Left;

            ISheet newSheet = dstBook.GetSheet("TPTL");
            if (newSheet == null)
            {
                newSheet = dstBook.CreateSheet("TPTL");
            }

            newSheet.SetColumnWidth(0, 30 * 256);
            newSheet.SetColumnWidth(1, 30 * 256);
            newSheet.SetColumnWidth(2, 60 * 256);

            int index = 0;
            for (int i = 0; i < TPLs.Count; i++)
            {
                if (TPLs[i] == "" || TLLs[i] == "")
                {

                }
                else
                {
                    IRow row = newSheet.CreateRow(index);
                    var cellTP = row.CreateCell(0);
                    var cellTL = row.CreateCell(1);
                    var cellTXT = row.CreateCell(2);
                    cellTP.SetCellValue(TPLs[i]);
                    cellTL.SetCellValue(TLLs[i]);
                    cellTXT.SetCellValue($"bool {TPLs[i].TrimEnd("指令".ToCharArray())} ({i},\"Operation\" -> TL_{i}) : TP_{i}");
                    cellTL.CellStyle = i % 2 == 0 ? cellStyle1 : cellStyle2;
                    cellTP.CellStyle = i % 2 == 0 ? cellStyle1 : cellStyle2;
                    cellTXT.CellStyle = i % 2 == 0 ? cellStyle1 : cellStyle2;
                    index++;
                }
            }
        }

        private void createIOState(XSSFWorkbook srcBook, StreamWriter stream,int ioIndex)
        {
            ISheet sheet = srcBook.GetSheet("InputInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在InputInfo表");
                return;
            }
            stream.WriteLine("");
            stream.WriteLine("//IO状态部分");
            stream.WriteLine($"[Mode = IOState, GroupName = Input, GroupIndex = {ioIndex}]");
            stream.WriteLine("SDC_DATA.SDC_DATA_Struct_Block()");
            stream.WriteLine("{");
            stream.WriteLine("	Input()");
            stream.WriteLine("	{");
            int id = 0;
            for (int i = 1; i < sheet.LastRowNum; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count == 3)
                {
                    stream.WriteLine($"		bool {line.Cells[2].StringCellValue}   		  ( {id}, \"{line.Cells[1].StringCellValue}\") : Input_{id}");
                }
                id++;
            }
            stream.WriteLine("	}");
            stream.WriteLine("}");


            sheet = srcBook.GetSheet("OutputInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在OutputInfo表");
                return;
            }
            stream.WriteLine("");
            stream.WriteLine($"[Mode = IOState, GroupName = Output, GroupIndex = {ioIndex + 1}]");
            stream.WriteLine("SDC_DATA.SDC_DATA_Struct_Block()");
            stream.WriteLine("{");
            stream.WriteLine("	Output()");
            stream.WriteLine("	{");
            id = 0;
            for (int i = 1; i < sheet.LastRowNum; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count == 3)
                {
                    string lab = line.Cells[1].StringCellValue;
                    string dsc = line.Cells[2].StringCellValue;
                    if (lab == "" || dsc == "")
                        continue;

                    stream.WriteLine($"		bool {dsc}   		  ( {id}, \"{lab}\") : Output_{id}");
                }
                id++;
            }
            stream.WriteLine("	}");
            stream.WriteLine("}");
        }

        private void createAlarmWarning(XSSFWorkbook srcBook, StreamWriter stream)
        {

            ISheet sheet = srcBook.GetSheet("AlarmInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在AlarmInfo表");
                return;
            }
            int alarmSheetRowCount = sheet.LastRowNum;


            sheet = srcBook.GetSheet("WarningInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在WarningInfo表");
                return;
            }
            int warningSheetRowCount = sheet.LastRowNum;


            stream.WriteLine("");
            stream.WriteLine("[Mode = Alarm]");
            stream.WriteLine("SDC_DATA.SDC_DATA_Struct_Block()");
            stream.WriteLine("{");
            for (int i = 0; i < alarmSheetRowCount; i++)
            {
                stream.WriteLine("   Alarms() {" + $" bool Alarm_{i}( {i} ) : Alarm_{i}" + "}");
            }
            stream.WriteLine("}");
            stream.WriteLine("");
            stream.WriteLine("[Mode = Warning]");
            stream.WriteLine("SDC_DATA.SDC_DATA_Struct_Block()");
            stream.WriteLine("{");
            for (int i = 0; i < warningSheetRowCount; i++)
            {
                stream.WriteLine("   Warnings() {" + $" bool Warning_{i}( {i} ) : Warning_{i}" + "}");
            }
            stream.WriteLine("}");
            stream.WriteLine("");
        }
        private void createAlarmWarning(XSSFWorkbook srcBook, XSSFWorkbook dstBook)
        {
            ISheet sheet = srcBook.GetSheet("AlarmInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在AlarmInfo表");
                return;
            }
            int alarmSheetRowCount = sheet.LastRowNum;


            sheet = srcBook.GetSheet("WarningInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在WarningInfo表");
                return;
            }
            int warningSheetRowCount = sheet.LastRowNum;


            //设置背景色
            ICellStyle cellStyle1 = dstBook.CreateCellStyle();
            cellStyle1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightTurquoise.Index;
            cellStyle1.FillPattern = FillPattern.SolidForeground;
            cellStyle1.Alignment = HorizontalAlignment.Left;

            ICellStyle cellStyle2 = dstBook.CreateCellStyle();
            cellStyle2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
            cellStyle2.FillPattern = FillPattern.SolidForeground;
            cellStyle2.Alignment = HorizontalAlignment.Left;

            ISheet newSheet = dstBook.GetSheet("Aralm");
            if (newSheet == null)
            {
                newSheet = dstBook.CreateSheet("Aralm");
            }
            newSheet.SetColumnWidth(0, 60 * 256);

            for (int i = 0; i < alarmSheetRowCount; i++)
            {
                IRow row = newSheet.CreateRow(i);
                var cellTP = row.CreateCell(0);
                cellTP.SetCellValue("   Alarms() {" + $" bool Alarm_{i}( {i} ) : Alarm_{i}" + "}");
                cellTP.CellStyle = i % 2 == 0 ? cellStyle1 : cellStyle2;
            }

            newSheet = dstBook.GetSheet("Warning");
            if (newSheet == null)
            {
                newSheet = dstBook.CreateSheet("Warning");
            }
            newSheet.SetColumnWidth(0, 60 * 256);
            for (int i = 0; i < warningSheetRowCount; i++)
            {
                IRow row = newSheet.CreateRow(i);
                var cellTP = row.CreateCell(0);
                cellTP.SetCellValue("   Warnings() {" + $" bool Warning_{i}( {i} ) : Warning_{i}" + "}");
                cellTP.CellStyle = i % 2 == 0 ? cellStyle1 : cellStyle2;
            }
        }


        /// <summary>
        /// PlcConfig.txt中的IO分组
        /// </summary>
        /// <param name="oriFilePath"></param>
        /// <param name="oriFileName"></param>
        public void CreateIOGROUP(string oriFilePath, string oriFileName)
        {
            FileStream fs = new FileStream(Path.Combine(oriFilePath, oriFileName), FileMode.Open);
            FileStream fw = new FileStream(Path.Combine(oriFilePath, "IO组" + oriFileName), FileMode.Create);
            StreamReader sr = new StreamReader(fs);
            StreamWriter sw = new StreamWriter(fw);

            string line = ""; //找到IO集合
            string mode = "";
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains("GroupName = Whole"))
                {
                    line = sr.ReadLine();
                    mode = line.Split('_')[0];
                    break;
                }
            }

            //IO分组
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (line.Contains('}'))
                    break;
                string[] arr = line.Split('"');
                if (arr != null && arr.Length == 3)
                {
                    if (!dic.ContainsKey(arr[1]))
                    {
                        dic[arr[1]] = new List<string>();
                    }
                    dic[arr[1]].Add(line);
                }
            }

            //生成文件
            int gid = 1;
            foreach (var ky in dic.Keys)
            {
                sw.WriteLine($"[Mode = IOCmd, GroupName = {ky}, GroupIndex = {gid}]");
                sw.WriteLine($"{mode}_PARA.{mode}_PARA_Struct_Block()");
                sw.WriteLine('{');
                iopair(sw, dic[ky]);
                sw.WriteLine('}');
                gid++;
            }

            sw.Close();
            sr.Close();
            fw.Close();
            fs.Close();
            msg?.Invoke("PLC文件IO分组完成");
        }
        private void createIOGROUP(XSSFWorkbook srcBook, string txtName)
        {
            ISheet sheet = srcBook.GetSheet("TLInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在TLInfo表");
                return;
            }
            List<string> TLLs = new List<string>();
            int sheetRowCount = sheet.LastRowNum;
            for (int i = 0; i < sheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if (line.Cells.Count >= 2)
                {
                    TLLs.Add(line.Cells[1].StringCellValue.TrimEnd("状态".ToCharArray()));
                }
                else
                {
                    TLLs.Add("");
                }
            }
            FileStream dstStream = new FileStream(txtName, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter wrStream = new StreamWriter(dstStream);

            int index = 1;
            for (int i = 0; i < TLLs.Count; i += 2)
            {
                if (TLLs[i] != "" && (i + 1) < TLLs.Count && TLLs[i + 1] != "")
                {
                    wrStream.WriteLine($"    IOControls{index}()");
                    wrStream.WriteLine("    {");
                    wrStream.WriteLine($"       bool {TLLs[i]} ({i},\"Operation\" -> TL_{i})");
                    wrStream.WriteLine($"       bool {TLLs[i + 1]} ({i + 1},\"Operation\" -> TL_{i + 1})");
                    wrStream.WriteLine("    }");
                    index++;
                }
            }
            wrStream.Close();
            dstStream.Close();
        }
        private void iopair(StreamWriter st, List<string> ls)
        {
            int gid = 1;
            for (int i = 0; i < ls.Count; i++)
            {
                int index1 = 0;
                int index2 = 0;
                index1 = getId(ls[i]);
                if (ls.Count > i + 1)
                {
                    index2 = getId(ls[i + 1]);
                }
                if (index1 == (index2 - 1))
                {
                    st.WriteLine($"	IOControls{gid}()");
                    st.WriteLine("	{");
                    st.WriteLine(ls[i]);
                    st.WriteLine(ls[i + 1]);
                    st.WriteLine("	}");
                    i++;
                    gid++;
                }
                else
                {
                    st.WriteLine($"	IOControls{gid}()");
                    st.WriteLine("	{");
                    st.WriteLine(ls[i]);
                    st.WriteLine("	}");
                    gid++;
                }
            }
        }
        private void iopair(StreamWriter st, List<ioItem> ls)
        {
            int gid = 1;
            for (int i = 0; i < ls.Count; i++)
            {
                int index1 = ls[i].Index;
                int index2 = 0;
                if (ls.Count > i + 1)
                {
                    index2 = ls[i + 1].Index;
                }
                if (index1 == (index2 - 1))
                {
                    st.WriteLine($"	IOControls{gid}()");
                    st.WriteLine("	{");
                    st.WriteLine(ls[i].FullStr);
                    st.WriteLine(ls[i + 1].FullStr);
                    st.WriteLine("	}");
                    i++;
                    gid++;
                }
                else
                {
                    st.WriteLine($"	IOControls{gid}()");
                    st.WriteLine("	{");
                    st.WriteLine(ls[i].FullStr);
                    st.WriteLine("	}");
                    gid++;
                }
            }
        }

        private int getId(string str)
        {
            int index = -1;
            string strId = "";
            bool flag = false;
            foreach (var item in str)
            {
                if (item == '(')
                {
                    flag = true;
                }
                else if (flag && item == ',')
                {
                    break;
                }
                else if (flag == true)
                {
                    strId += item;
                }
            }
            int.TryParse(strId, out index);
            return index;
        }

        /// <summary>
        /// 根据Html生成数据库文件
        /// </summary>
        /// <param name="srcFileName"></param>
        /// <param name="dstFileName"></param>
        public void AddPcAlarmDatabase(string srcFileName, string srcFilePath)
        {
            //msg?.Invoke("开始处理Excel文件");
            XSSFWorkbook srcWorkbook = null;
            FileStream srcFileStream = null;

            if (File.Exists(srcFileName))
            {
                srcFileStream = new FileStream(srcFileName, FileMode.OpenOrCreate, FileAccess.Read);
                srcWorkbook = WorkbookFactory.Create(srcFileStream) as XSSFWorkbook;
            }
            else
            {
                msg?.Invoke("Excel文件为空");
                return;
            }

            try
            {
                SQLiteHelper.Instance.CreateDB(srcFilePath, "MaxwellDatabase.db");
                SQLiteHelper.Instance.DeleteTableVAlue("AlarmLookupTab","AlarmType = 'PC'");

                msg?.Invoke("开始添加PC报警文件");
                fromPCExcelCreateDatabase(srcWorkbook, srcFilePath);
            }
            catch (Exception ex)
            {
                msg?.Invoke(ex.Message);
            }
            finally
            {
                srcFileStream.Close();
                srcWorkbook.Close();
                msg?.Invoke("PLC文件处理完成");
            }
        }

        private void fromPCExcelCreateDatabase(XSSFWorkbook srcBook, string srcFilePath)
        {
            //获取Html数据
            ISheet sheet = srcBook.GetSheet("Sheet1");
            if (sheet == null)
            {
                msg?.Invoke("不存在Sheet1表");
                return;
            }
            int alarmSheetRowCount = sheet.LastRowNum;
            List<plcAlarmWarningItem> alarmLs = new List<plcAlarmWarningItem>();
            for (int i = 0; i < alarmSheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                if(i!=0) 
                {
                    string msg = "";
                    string res = "";
                    string sol = "";
                    string lev = "";

                    msg = line.Cells[1].StringCellValue;
                    res = line.Cells[2].StringCellValue;
                    sol = line.Cells[3].StringCellValue;
                    lev = line.Cells[5].StringCellValue;

                    alarmLs.Add(new plcAlarmWarningItem(msg, res, sol, lev));
                }
            }


            List<string> headerLs = new List<string>() { "AlarmID", "AlarmMessage", "AlarmReason", "AlarmSolution", "AlarmType", "AlarmLevel" };

            List<object[]> rowLs = new List<object[]>();
            for (int i = 0; i < alarmLs.Count; i++)
            {
                object[] arr = new object[6];
                arr[0] = i.ToString();
                arr[1] = alarmLs[i].Msg;
                arr[2] = alarmLs[i].Reason;
                arr[3] = alarmLs[i].Solution;
                arr[4] = "PC";
                arr[5] = alarmLs[i].Level;
                rowLs.Add(arr);
            }
            SQLiteHelper.Instance.AppendHugeTableData(Path.Combine(srcFilePath, "MaxwellDatabase.db"), "AlarmLookupTab", headerLs, rowLs);

            //msg?.Invoke("PC报警文件添加OK");
        }

        public void CreateAlarmDatabase(string srcFileName, string srcFilePath)
        {
            //msg?.Invoke("开始处理Excel文件");
            XSSFWorkbook srcWorkbook = null;
            FileStream srcFileStream = null;

            if (File.Exists(srcFileName))
            {
                srcFileStream = new FileStream(srcFileName, FileMode.OpenOrCreate, FileAccess.Read);
                srcWorkbook = WorkbookFactory.Create(srcFileStream) as XSSFWorkbook;
            }
            else
            {
                //msg?.Invoke("Excel文件为空");
                return;
            }

            try
            {
                //msg?.Invoke("初始化数据库");
                //清除数据库数据
                SQLiteHelper.Instance.CreateDB(srcFilePath, "MaxwellDatabase.db");
                SQLiteHelper.Instance.DeleteTableAllValues("AlarmLookupTab");

                //初始化软件报错

                /*
                SQLiteHelper.Instance.ReadyExecute();
                for (int i = 0; i < 300; i++)
                {
                    List<KeyValuePair<string, string>> ls = new List<KeyValuePair<string, string>>();
                    ls.Add(new KeyValuePair<string, string>("AlarmID", i.ToString()));
                    ls.Add(new KeyValuePair<string, string>("AlarmMessage", "msg"));
                    ls.Add(new KeyValuePair<string, string>("AlarmReason", "原因"));
                    ls.Add(new KeyValuePair<string, string>("AlarmSolution", "解决方法"));
                    ls.Add(new KeyValuePair<string, string>("AlarmType", "软件"));
                    ls.Add(new KeyValuePair<string, string>("AlarmLevel", "Alarm"));
                    SQLiteHelper.Instance.Ex_AddTableValue("AlarmLookupTab", ls);
                }
                SQLiteHelper.Instance.ExecuteFinish();
                */

                List<string> headerLs = new List<string>() { "AlarmID", "AlarmMessage", "AlarmReason", "AlarmSolution", "AlarmType", "AlarmLevel" };

                List<object[]> rowLs = new List<object[]>();
                for (int i = 0; i < 300; i++)
                {
                    object[] arr = new object[6];
                    arr[0] = i.ToString();
                    arr[1] = "msg";
                    arr[2] = "原因";
                    arr[3] = "解决方法";
                    arr[4] = "PC";
                    arr[5] = "Alarm";
                    rowLs.Add(arr);
                }
                SQLiteHelper.Instance.AppendHugeTableData(Path.Combine(srcFilePath, "MaxwellDatabase.db"), "AlarmLookupTab", headerLs, rowLs);

                //msg?.Invoke("开始生成PLC报警文件");
                fromExcelCreateDatabase(srcWorkbook, srcFilePath);
            }
            catch (Exception ex)
            {
                msg?.Invoke(ex.Message);
            }
            finally
            {
                srcFileStream.Close();
                srcWorkbook.Close();
                msg?.Invoke("PLC文件处理完成");
            }
        }
        private void fromExcelCreateDatabase(XSSFWorkbook srcBook, string srcFilePath)
        {
            //获取Html数据
            ISheet sheet = srcBook.GetSheet("AlarmInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在AlarmInfo表");
                return;
            }
            int alarmSheetRowCount = sheet.LastRowNum;
            List<plcAlarmWarningItem> alarmLs = new List<plcAlarmWarningItem>();
            for (int i = 0; i < alarmSheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                int id = GetID(line);

                if (i == 25)
                {
                    int a = 10;
                }
                if (id != -1)
                {
                  
                    string msg = "";
                    string res = "";
                    string sol = "";
                    string levTmp = "";
                    string lev = "";
                    if (line.Cells.Count == 2) //报警项
                    {
                        msg = line.Cells[1].StringCellValue;
                    }
                    else if (line.Cells.Count == 3)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                    }
                    else if (line.Cells.Count == 4)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                        sol = line.Cells[3].StringCellValue;
                    }
                    else if (line.Cells.Count >= 5)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                        sol = line.Cells[3].StringCellValue;
                        levTmp = line.Cells[4].StringCellValue;
                        if (levTmp.Length>0)
                        {
                            lev = levTmp[0].ToString();
                        }
                    }


                    if (msg == "")
                    {
                        msg = "Alarm_" + (i - 1);
                    }
                    if (res == "")
                    {
                        res = "原因";
                    }
                    if (sol == "")
                    {
                        sol = "解决方案";
                    }
                    if (lev == "")
                    {
                        lev = "null";
                    }

                    alarmLs.Add(new plcAlarmWarningItem(msg, res, sol, lev));
                }
            }

            sheet = srcBook.GetSheet("WarningInfo");
            if (sheet == null)
            {
                msg?.Invoke("不存在WarningInfo表");
                return;
            }
            int warningSheetRowCount = sheet.LastRowNum;
            List<plcAlarmWarningItem> warningLs = new List<plcAlarmWarningItem>();
            //for (int i = 0; i < warningSheetRowCount; i++)
            //{
            //    var line = sheet.GetRow(i);
            //    int id = GetID(line);
            //    if (id != -1)
            //    {
            //        if (line.Cells.Count >= 2)
            //        {
            //            string str = line.Cells[1].StringCellValue;
            //            if (str == "")
            //            {
            //                str = "Warning_" + (i - 1);
            //            }
            //            warningLs.Add(str);
            //        }
            //        else
            //        {
            //            warningLs.Add("Warning_" + i);
            //        }
            //    }
            //}
            for (int i = 0; i < warningSheetRowCount; i++)
            {
                var line = sheet.GetRow(i);
                int id = GetID(line);

                if (i == 25)
                {
                    int a = 10;
                }
                if (id != -1)
                {

                    string msg = "";
                    string res = "";
                    string sol = "";
                    string levTmp = "";
                    string lev = "";
                    if (line.Cells.Count == 2) //报警项
                    {
                        msg = line.Cells[1].StringCellValue;
                    }
                    else if (line.Cells.Count == 3)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                    }
                    else if (line.Cells.Count == 4)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                        sol = line.Cells[3].StringCellValue;
                    }
                    else if (line.Cells.Count >= 5)
                    {
                        msg = line.Cells[1].StringCellValue;
                        res = line.Cells[2].StringCellValue;
                        sol = line.Cells[3].StringCellValue;
                        levTmp = line.Cells[4].StringCellValue;
                        if (levTmp.Length > 0)
                        {
                            lev = levTmp[0].ToString();
                        }
                    }


                    if (msg == "")
                    {
                        msg = "Warning_" + (i - 1);
                    }
                    if (res == "")
                    {
                        res = "原因";
                    }
                    if (sol == "")
                    {
                        sol = "解决方案";
                    }
                    if (lev == "")
                    {
                        lev = "null";
                    }
                    warningLs.Add(new plcAlarmWarningItem(msg, res, sol, lev));
                }
            }


            /*
            SQLiteHelper.Instance.ReadyExecute();
            for (int i = 0; i < alarmLs.Count; i++)
            {
                List<KeyValuePair<string, string>> ls = new List<KeyValuePair<string, string>>();
                ls.Add(new KeyValuePair<string, string>("AlarmID", (10000 + i).ToString()));
                ls.Add(new KeyValuePair<string, string>("AlarmMessage", alarmLs[i].Msg));
                ls.Add(new KeyValuePair<string, string>("AlarmReason", alarmLs[i].Reason));
                ls.Add(new KeyValuePair<string, string>("AlarmSolution", alarmLs[i].Solution));
                ls.Add(new KeyValuePair<string, string>("AlarmType", "PLC"));
                ls.Add(new KeyValuePair<string, string>("AlarmLevel", alarmLs[i].Level));
                SQLiteHelper.Instance.Ex_AddTableValue("AlarmLookupTab", ls);
            }
            for (int i = 0; i < warningLs.Count; i++)
            {
                List<KeyValuePair<string, string>> ls = new List<KeyValuePair<string, string>>();
                ls.Add(new KeyValuePair<string, string>("AlarmID", (20000 + i).ToString()));
                ls.Add(new KeyValuePair<string, string>("AlarmMessage", warningLs[i]));
                ls.Add(new KeyValuePair<string, string>("AlarmReason", "原因"));
                ls.Add(new KeyValuePair<string, string>("AlarmSolution", "解决方法"));
                ls.Add(new KeyValuePair<string, string>("AlarmType", "PLC"));
                ls.Add(new KeyValuePair<string, string>("AlarmLevel", "Warning"));
                SQLiteHelper.Instance.Ex_AddTableValue("AlarmLookupTab", ls);
            }
            SQLiteHelper.Instance.ExecuteFinish();
            */

            List<string> headerLs = new List<string>() { "AlarmID", "AlarmMessage", "AlarmReason", "AlarmSolution", "AlarmType", "AlarmLevel" };

            List<object[]> rowLs = new List<object[]>();
            for (int i = 0; i < alarmLs.Count; i++)
            {
                object[] arr = new object[6];
                arr[0] = (10000 + i).ToString();
                arr[1] = alarmLs[i].Msg;
                arr[2] = alarmLs[i].Reason;
                arr[3] = alarmLs[i].Solution;
                arr[4] = "PLC";
                arr[5] = alarmLs[i].Level;
                rowLs.Add(arr);
            }
            for (int i = 0; i < warningLs.Count; i++)
            {
                object[] arr = new object[6];
                arr[0] = (20000 + i).ToString();
                arr[1] = warningLs[i].Msg;
                arr[2] = warningLs[i].Reason;
                arr[3] = warningLs[i].Solution;
                arr[4] = "PLC";
                arr[5] = warningLs[i].Level;
                rowLs.Add(arr);
            }
          
            SQLiteHelper.Instance.AppendHugeTableData(Path.Combine(srcFilePath, "MaxwellDatabase.db"), "AlarmLookupTab", headerLs, rowLs);


            msg?.Invoke("PLC报警文件生成OK");
        }
        private int GetID(IRow row)
        {
            if (row.Cells.Count ==0)
            {
                return -1;
            }
            string str = row.Cells[0].StringCellValue;
            if (!str.Contains("SDC"))
            {
                return -1;
            }
            var dd= Regex.Match(str,@"[\d+]");
            if (dd.Captures.Count >= 1)
            {
                return int.Parse(dd.Captures[0].Value);
            }
            return -1;
        }
    }

    //xls 模板
    public class ExcelFile
    {
        public string FileName;
        public List<ExcelSheet> ListSheet = new List<ExcelSheet>();
        public ExcelFile(string name)
        {
            FileName = name;
        }
    }
    public class ExcelSheet
    {
        public string SheetName;
        public List<ExcelCol> ListColums = new List<ExcelCol>();
        public ExcelSheet(string name)
        {
            SheetName = name;
        }
    }
    public class ExcelCol
    {
        public string ColHeader = "";
        public List<double> Cols = new List<double>();
        public ExcelCol(string name)
        {
            ColHeader = name;
        }
    }

    //axis 模板
    public class plcAxis
    {
        public int ID;
        public string Name;
        public List<axisTrig> TrigLs = new List<axisTrig>();
    }
    public class axisTrig
    {
        public int ID;
        public string Name;
        public double DefaultPos;
        public double DefaultVel;
        public axisTrig(int id,string nm,double pos,double vel)
        {
            ID = id;
            Name = nm;
            DefaultPos = pos;
            DefaultVel = vel;
        }
    }

    public class ioItem
    {
        public int Index;
        public string Key;
        public string Value;
        public string FullStr;
    }

    public class plcAlarmWarningItem
    {
        public string Msg;
        public string Reason = "原因";
        public string Solution = "解决方案";
        public string Level = ""; //等级A、B、C

        public plcAlarmWarningItem()
        { }
        public plcAlarmWarningItem(string alm, string reason, string sol,string lev)
        {
            Msg = alm;
            Reason = reason;
            Solution = sol;
            Level = lev;
        }
    }
}
