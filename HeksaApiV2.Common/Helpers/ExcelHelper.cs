using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HeksaApiV2.Common.Helpers
{
    public class ExcelHelper
    {
        public static ExcelDataHelper ReadExcel(IFormFile file)
        {
            var data = new ExcelDataHelper();

            // Check if the file is excel
            if (file.Length <= 0)
            {
                data.Status.Message = "You uploaded an empty file";
                return data;
            }

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                data.Status.Message = "Please upload a valid excel file of version 2007 and above";
                return data;
            }

            // Open the excel document
            WorkbookPart workbookPart; List<Row> rows;
            try
            {
                var document = SpreadsheetDocument.Open(file.OpenReadStream(), false);
                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();

                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();

            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file";
                return data;
            }

            // Read the header
            if (rows.Count > 0)
            {
                var row = rows[0];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(text);
                }
            }

            // Read the sheet data
            if (rows.Count > 1)
            {
                for (var i = 1; i < rows.Count; i++)
                {
                    var dataRow = new List<string>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }

            return data;
        }

        public static Cell GetCell(Worksheet worksheet, string columnName, long rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);

            if (row == null) return null;

            var FirstRow = row.Elements<Cell>().Where(c => string.Compare
            (c.CellReference.Value, columnName +
            rowIndex, true) == 0).FirstOrDefault();

            if (FirstRow == null) return null;

            return FirstRow;
        }

        public static Row GetRow(Worksheet worksheet, long rowIndex)
        {
            Row row = worksheet.GetFirstChild<SheetData>().
            Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row == null)
            {
                throw new ArgumentException(String.Format("No row with index {0} found in spreadsheet", rowIndex));
            }
            return row;
        }

        private static IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private static int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        public static string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(
                        Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        public static List<string> ObjectToList(object value)
        {
            List<string> lists = null;
            if (value != null)
            {
                lists = new List<string>();
                foreach (System.ComponentModel.PropertyDescriptor descriptor in System.ComponentModel.TypeDescriptor.GetProperties(value))
                {
                    if (descriptor != null && descriptor.Name != null)
                    {
                        lists.Add(descriptor.Name);
                    }
                }
            }
            return lists;
        }

        public static Dictionary<string, int> ModeltoDictionary(List<string> model, List<string> headers)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (string key in model)
            {
                string modelkey = headers.Where(x => x.ToLower() == key.ToLower()).FirstOrDefault();
                if (!string.IsNullOrEmpty(modelkey))
                {
                    dictionary.Add(modelkey, headers.IndexOf(modelkey));
                }
            }
            return dictionary;
        }

        public static void CheckData(List<string> data, List<string> header)
        {
            if (data.Count < header.Count)
            {
                data.Add(string.Empty);
                CheckData(data, header);
            }
        }

        public static T ConvertRawDataToModel<T>(List<string> rawdata, Dictionary<string, int> dataKey, T result) where T : class
        {
            T model = result;
            foreach (System.ComponentModel.PropertyDescriptor descriptor in System.ComponentModel.TypeDescriptor.GetProperties(typeof(T)))
            {
                if (descriptor != null && descriptor.Name != null)
                {
                    if (dataKey.ContainsKey(descriptor.Name))
                    {
                        int index = dataKey[descriptor.Name];
                        switch (descriptor.PropertyType.Name)
                        {
                            case "Decimal":
                                descriptor.SetValue(model, !string.IsNullOrWhiteSpace(rawdata[index]) ? Convert.ToDecimal(rawdata[index]) : 0);
                                break;
                            case "DateTime":
                                {
                                    if (!string.IsNullOrWhiteSpace(rawdata[index]))
                                    {
                                        DateTime a = new DateTime();
                                        if (DateTime.TryParse(rawdata[index], out a))
                                            descriptor.SetValue(model, Convert.ToDateTime(rawdata[index]));
                                        else
                                            descriptor.SetValue(model, new DateTime(1900, 1, 1));
                                    }
                                    else
                                        descriptor.SetValue(model, new DateTime(1900, 1, 1));
                                }
                                break;
                            case "Int64":
                                descriptor.SetValue(model, !string.IsNullOrWhiteSpace(rawdata[index]) ? Convert.ToInt64(rawdata[index]) : 0);
                                break;
                            case "Int32":
                                descriptor.SetValue(model, !string.IsNullOrWhiteSpace(rawdata[index]) ? Convert.ToInt32(rawdata[index]) : 0);
                                break;
                            default: //string
                                descriptor.SetValue(model, rawdata[index]);
                                break;

                        }
                    }
                }
            }

            return model;
        }
    }

    public class ExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }
    public class ExcelDataHelper
    {
        public ExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public ExcelDataHelper()
        {
            Status = new ExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }
}
