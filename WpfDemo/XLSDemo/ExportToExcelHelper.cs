using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XLSDemo
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

        //*****
        public void ReadPlc(string srcFileName,string dstFileName)
        {
            msg?.Invoke("开始处理PLC文件");
            XSSFWorkbook srcWorkbook = null;
            XSSFWorkbook dstWorkbook = new XSSFWorkbook();
            FileStream srcFileStream = null;
            FileStream dstFileStream = new FileStream(dstFileName, FileMode.OpenOrCreate);

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

            createTPTL(srcWorkbook, dstWorkbook);
            createAlarmWarning(srcWorkbook, dstWorkbook);
            createIOGROUP(srcWorkbook, dstFileName.Replace("xlsx", "txt"));
            dstWorkbook.Write(dstFileStream);
            dstFileStream.Close();
            dstWorkbook.Close();
            srcFileStream.Close();
            srcWorkbook.Close();

            msg?.Invoke("PLC文件处理完成");
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

            newSheet.SetColumnWidth(0, 30*256);
            newSheet.SetColumnWidth(1, 30*256);
            newSheet.SetColumnWidth(2, 60*256);

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
            for (int i = 0; i < TLLs.Count; i+=2)
            {
                if (TLLs[i] != "" && (i + 1) < TLLs.Count && TLLs[i + 1] != "") 
                {
                    wrStream.WriteLine($"    IOControls{index}()");
                    wrStream.WriteLine("    {");
                    wrStream.WriteLine($"       bool {TLLs[i]} ({i},\"Operation\" -> TL_{i})");
                    wrStream.WriteLine($"       bool {TLLs[i+1]} ({i+1},\"Operation\" -> TL_{i+1})");
                    wrStream.WriteLine("    }");
                    index++;
                }
            }
            wrStream.Close();
            dstStream.Close();
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
    }

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
}
