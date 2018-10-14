using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using DBMS.Data.Entity;
using System.Collections.Generic;
using UnityEngine.Events;
using DBMS.Utils;
namespace DBMS.Systems
{
    public class ExcelService
    {
        public static Dictionary<string, string> Map;
        static ExcelService()
        {
            Map = new Dictionary<string, string>();
            Map.Add("ID", "编号");
            Map.Add("Nation", "民族");
            Map.Add("IDCard", "身份证");
            Map.Add("Address", "家庭地址");
            Map.Add("PoliticalOutlook", "政治面貌");
            Map.Add("Education", "学历");
            Map.Add("Name", "姓名");
            Map.Add("Info", "备注信息");
            Map.Add("DepartmentID", "所在部门");
            Map.Add("Phone", "手机号码");
            Map.Add("BirthDay", "生日");
            Map.Add("PositionID", "职位");
            Map.Add("TitleID", "职称");
        }

        public void SaveToExcel(List<Personnel> list, UnityAction callback)
        {
            string FileName = Win32API.GetSaveFileName();
            if (FileName == null)
            {
                return;
            }
            Kernel.Current.TaskRun(() =>
            {
                Dictionary<int, string> buffer1 = new Dictionary<int, string>();
                Dictionary<int, string> buffer2 = new Dictionary<int, string>();
                Dictionary<int, string> buffer3 = new Dictionary<int, string>();
                XSSFWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet("Excel输出");
                IRow row = sheet.CreateRow(0);
                var props = typeof(Personnel).GetProperties();
                for (int index = 0; index < props.Length; index++)
                {
                    switch (props[index].Name)
                    {
                        case "FacialPhoto":
                        case "ArchivalPhoto":
                            break;
                        default:
                            {
                                ICell cell = row.CreateCell(index);
                                cell.SetCellType(CellType.String);
                                cell.SetCellValue(Map[props[index].Name]);
                            }
                            break;
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    row = sheet.CreateRow(i + 1);
                    for (int index = 0; index < props.Length; index++)
                    {
                        switch (props[index].Name)
                        {
                            case "FacialPhoto":
                            case "ArchivalPhoto":
                                break;
                            case "DepartmentID":
                                {
                                    int val = (int)props[index].GetValue(list[i]);
                                    if (!buffer1.ContainsKey(val))
                                    {
                                        buffer1.Add(val, Kernel.Current.Sql.LoadEntity<Department>(val).Name);
                                    }
                                    ICell cell = row.CreateCell(index);
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(buffer1[val]);
                                }
                                break;
                            case "PositionID":
                                {
                                    int val = (int)props[index].GetValue(list[i]);
                                    if (!buffer2.ContainsKey(val))
                                    {
                                        buffer2.Add(val, Kernel.Current.Sql.LoadEntity<Position>(val).Name);
                                    }
                                    ICell cell = row.CreateCell(index);
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(buffer2[val]);
                                }
                                break;
                            case "TitleID":
                                {
                                    int val = (int)props[index].GetValue(list[i]);
                                    if (!buffer3.ContainsKey(val))
                                    {
                                        buffer3.Add(val, Kernel.Current.Sql.LoadEntity<Title>(val).Name);
                                    }
                                    ICell cell = row.CreateCell(index);
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(buffer3[val]);
                                }
                                break;
                            default:
                                {
                                    ICell cell = row.CreateCell(index);
                                    cell.SetCellType(CellType.String);
                                    cell.SetCellValue(props[index].GetValue(list[i]).ToString());
                                }
                                break;
                        }
                    }
                }
                MemoryStream ms = new MemoryStream();
                book.Write(ms);
                book = null;
                using (FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
                ms.Close();
                ms.Dispose();
            }, x => callback?.Invoke());
        }
    }
}