using UnityEngine.UI;
using UnityEngine;
using DBMS.Systems;
using DBMS.Data.Entity;
using System.Collections.Generic;
using DBMS.Utils;
namespace DBMS.UI
{
    public class SearchMenu : Meun
    {
        InputField input;
        protected override void Awake()
        {
            base.Awake();
            input = transform.parent.Find("InputField").GetComponent<InputField>();
            actionDir.Add("按编号查询", () =>
            {
                SempleQueryWhere("ID",
                    () => VerifyUtils.IsNumber(input.text),
                    ()=> Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "不是数字!", "编号必须为数字."));
            });
            actionDir.Add("按民族查询", () =>
            {
                SempleQueryWhere("Nation", null, () => NullWarmDialog());
                
            });
            actionDir.Add("按身份证查询", () =>
            {
                SempleQueryWhere("IDCard", () => VerifyUtils.IsIDCard(input.text), () => MessageDialog("身份证无效"));
            });
            actionDir.Add("按地址查询", () =>
            {
                SempleQueryWhere("Address", null, () => NullWarmDialog());
            });
            actionDir.Add("按政治面貌查询", () =>
            {
                SempleQueryWhere("PoliticalOutlook", null, () => NullWarmDialog());
            });
            actionDir.Add("按学历查询", () =>
            {
                SempleQueryWhere("Education", null, () => NullWarmDialog());
            });
            actionDir.Add("按部门查询", () =>
            {
                if (input.text != string.Empty && input.text != null)
                {
                    var departments = Kernel.Current.Sql.LoadEntitys<Department>();
                    var id = global::System.Array.Find(departments, x => x.Name == input.text);
                    if (id != null)
                    {
                        Kernel.Current.Desktop.OpenNew<SearchResultForm>().AddItems(new List<Personnel>(Kernel.Current.Sql.QueryWhere<Personnel>("DepartmentID=" + id.ID)));
                    }
                    else
                    {
                        Kernel.Current.Desktop.OpenNew<SearchResultForm>();
                    }
                    input.text = string.Empty;
                    return;
                }
                input.text = string.Empty;
                NullWarmDialog();
            });
            actionDir.Add("按姓名查询", () =>
            {
                SempleQueryWhere("Name", null, () => NullWarmDialog());
            });
            actionDir.Add("按手机号码查询", () =>
            {
                SempleQueryWhere("Phone", () => input.text.Length == 11, () => MessageDialog("电话号码必须为11位!"));
            });
            actionDir.Add("按职位查询", () =>
            {
                if (input.text != string.Empty && input.text != null)
                {
                    var departments = Kernel.Current.Sql.LoadEntitys<Position>();
                    var id = global::System.Array.Find(departments, x => x.Name == input.text);
                    if (id != null)
                    {
                        Kernel.Current.Desktop.OpenNew<SearchResultForm>().AddItems(new List<Personnel>(Kernel.Current.Sql.QueryWhere<Personnel>("PositionID=" + id.ID)));
                    }
                    else
                    {
                        Kernel.Current.Desktop.OpenNew<SearchResultForm>();
                    }
                    input.text = string.Empty;
                    return;
                }
                input.text = string.Empty;
                NullWarmDialog();
            });
        }
        void SempleQueryWhere(string where, System.Func<bool> func, System.Action action)
        {
            if (input.text != string.Empty && input.text != null && (func != null ? func() : true))
            {
                string sql = null;
                if (typeof(Personnel).GetProperty(where).PropertyType == typeof(string))
                {
                    sql = $"[{where}]= \'{input.text}\'";
                }
                else
                {
                    sql = $"[{where}]= {input.text}";
                }
                Kernel.Current.Desktop.OpenNew<SearchResultForm>().AddItems(new List<Personnel>(Kernel.Current.Sql.QueryWhere<Personnel>(sql)));
                input.text = string.Empty;
                return;
            }
            input.text = string.Empty;
            action?.Invoke();
        }
        static void NullWarmDialog()
        {
            Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "不能为空!", "搜索参数不能为空.");
        }
        static void MessageDialog(string str)
        {
            Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, str, "请重新输入.");
        }
    }
}
