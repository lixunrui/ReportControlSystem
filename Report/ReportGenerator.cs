using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Data;


/*
 *  employee.name, 			0
    employee.employeecode,  1
    employee.Taxcode, 		2
    employee.rate,			3
    employee.hours,			4  
    period.start_date, 		5
    period.end_date, 		6
    category.category_name, 7 
    category.category_Type 	8
    payment.amount          9
 * */
namespace ReportControlSystem
{
    internal enum FontSizeLevel
    {
        ReportHeaderSize = 30,
        NameSize = 24,
        PaymentHeaderSize = 21,
        NormalFontSize = 12,
    }

    internal class ReportGenerator
    {
        private Microsoft.Office.Interop.Excel.Application app = null;
        private Microsoft.Office.Interop.Excel.Workbook workbook = null;
        private Microsoft.Office.Interop.Excel.Worksheet currentSheet = null;
        private Microsoft.Office.Interop.Excel.Range work_Area = null;

        String sheetName;

        internal ReportGenerator(String _sheetName = null)
        {
            CreateExcel(_sheetName);
        }

        internal void GenerateReport(List<DataTable> tables)
        {
            if (tables.Count > 0)
            {
                // create report header
                CreateHeader(1, 1, sheetName, 6, FontSizeLevel.ReportHeaderSize, true, null, Color.LightYellow);
            }

            int currenttableIndex = 0;
            int loop = 2;
            foreach (DataTable table in tables)
            {
                // create name header
                CreateHeader(loop, 1, table.Rows[0][0].ToString(), 3, FontSizeLevel.NameSize, true, null, null);
                loop += 2;

                // add ID
                CreateHeader(loop, 1, Constants.EmployeeElements.Employee_Code, 0, FontSizeLevel.NormalFontSize, false, null, null);
                AddData(loop, 2, table.Rows[0][1].ToString(), null, 1, true, false, Color.Red);

                // add rate
                loop++;
                CreateHeader(loop, 1, Constants.EmployeeElements.Employee_Rate, 0, FontSizeLevel.NormalFontSize, false, null, null);
                AddData(loop, 2, table.Rows[0][3].ToString(), null, 1, true, false, Color.Red);

                // hours
                CreateHeader(loop, 4, Constants.EmployeeElements.Employee_Hours, 0, FontSizeLevel.NormalFontSize, false, null, null);
                AddData(loop, 5, table.Rows[0][4].ToString(), null, 0, true, false, Color.Red);

                loop += 2;
                // add payment details
                CreateHeader(7, 1, "Payment Details:", 3, FontSizeLevel.PaymentHeaderSize, true, null, Color.LightYellow);

                currenttableIndex++;
                // TODO: create period header
                // ???

                loop++;
                decimal netTotal = 0;

                foreach (DataRow row in table.Rows)
                {

                    // add categories
                    CreateHeader(loop, 2, row[7].ToString(), 0, FontSizeLevel.NormalFontSize, false, null, null);
                    if (Convert.ToBoolean(row[8]))
                    {
                        AddData(loop, 3, String.Format("{0:C}",row[9]), null, 0, true, true, null);
                        netTotal += Convert.ToDecimal(row[9]);
                    }
                    else
                    {
                        AddData(loop, 4, String.Format("{0:C}",row[9]), null, 0, true, false, Color.Red);
                        netTotal -= Convert.ToDecimal(row[9]);
                    }
                   
                    loop++;
                    loop++;
                }
                
                // add net 
                CreateHeader(loop, 2, "Net Pay:", 0, FontSizeLevel.NormalFontSize,true, null, Color.Red);

                AddData(loop, 3, String.Format("{0:C}",netTotal), null, 0, true, true, Color.Red);

                loop += 4;
            }
        }

       

        internal void CreateExcel(String _sheetName = null)
        {
            sheetName = _sheetName;
            try
            {
                app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = true;
                workbook = app.Workbooks.Add(1);
                currentSheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                currentSheet.Name = sheetName;
            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startRow">Start from</param>
        /// <param name="startColumn"> start from</param>
        /// <param name="headerText">Text for the header</param>
        /// <param name="HeaderLengthInCell"> how many cell should be</param>
        /// <param name="fontLevel">How big the size should be,</param>
        internal void CreateHeader(int startRow, int startColumn, String headerText,
            int HeaderLengthInCell, FontSizeLevel fontLevel, bool isBold, Color? fontColor, Color? backGroundColor)
        {
            KeyValuePair<String,String> cell = CalcualteCellAddress(startRow, startColumn, HeaderLengthInCell, true);
            currentSheet.Cells[startRow, startColumn] = headerText;

            work_Area = currentSheet.get_Range(cell.Key, cell.Value);

            work_Area.Merge(HeaderLengthInCell);

            work_Area.ColumnWidth = headerText.Length;

            if (backGroundColor != null)
            {
                work_Area.Interior.Color = backGroundColor; // cell background color
            }
            if (isBold)
            {
                work_Area.Font.Bold = true;
            }
            
            work_Area.Font.Size = (int)fontLevel; // font size

            work_Area.Font.Color = fontColor ?? Color.Black; // font color

            if ((int)fontLevel > (int)FontSizeLevel.NormalFontSize)
            {
                work_Area.Font.Underline = true; // font under line
            }
        }


        internal void AddData(int startRow, int startColumn, String data, String format, int mergeCells, bool hasBorder, bool isBold, Color? fontColor)
        {
            currentSheet.Cells[startRow, startColumn] = data;

            KeyValuePair<String, String> cells = CalcualteCellAddress(startRow, startColumn, mergeCells, true);

            work_Area = currentSheet.get_Range(cells.Key, cells.Value);

            if (hasBorder)
            {
                Microsoft.Office.Interop.Excel.Borders borders = work_Area.Borders;
                borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
            }

            if (isBold)
            {
                work_Area.Font.Bold = true;
            }

            work_Area.Font.Color = fontColor ?? Color.Black;

            work_Area.NumberFormat = format;
            work_Area.ColumnWidth = data.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startNumber"></param>
        /// <param name="length"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        /// // pass 3 , 1, 2, true -> C1 C3 
        KeyValuePair<String, String> CalcualteCellAddress( int startY, int startX, int length, bool horizontal)
        {
            KeyValuePair<String, String> StartAndEndCellID ;

            String alphabet = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            Char startCellAlphabet = alphabet[startX - 1];
            String cellX = startCellAlphabet.ToString() + startY;

            if (horizontal)
            {
                Char endCellAlphabet = alphabet[startX + length - 1];
                String cellY = endCellAlphabet.ToString() + startY;
                StartAndEndCellID = new KeyValuePair<String, String>(cellX, cellY);
            }
            else
            {
                String cellY = startCellAlphabet.ToString() + (startY + length);
                StartAndEndCellID = new KeyValuePair<String, String>(cellX, cellY);
            }
            return StartAndEndCellID;
        }

    }
}
