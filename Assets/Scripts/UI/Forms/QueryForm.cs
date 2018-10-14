using UnityEngine.UI;
using UnityEngine;
using DBMS.Systems;
using System;
using DBMS.Data.Entity;
using System.Data.SqlTypes;
using DG.Tweening;
using System.Collections.Generic;
namespace DBMS.UI
{
    public static class StringStatic
    {
        public static bool IsNotEmpty(this string s)
        {
            return s != null && s != string.Empty;
        }
    }
    public class QueryForm : AbstractInfoForm
    {
        public Button query;
        public Toggle datetimeToggle;
        public override void Awake()
        {
            
            //base.Awake();
            positions = new List<Position>();
            departments = new List<Department>();
            titles = new List<Title>();
            var notchoose = new Dropdown.OptionData("未选中");
            List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
            positions.AddRange(Kernel.Current.Sql.LoadEntitys<Position>());
            foreach (var item in positions)
            {
                list.Add(new Dropdown.OptionData(item.Name));
            }
            PositionID.ClearOptions();
            list.Add(notchoose);
            PositionID.AddOptions(list);
            //Debug.Log(PositionID.options.Count + " " + PositionID.value);
            PositionID.value = PositionID.options.Count - 1;
   

            list.Clear();
            departments.AddRange(Kernel.Current.Sql.LoadEntitys<Department>());
            foreach (var item in departments)
            {

                list.Add(new Dropdown.OptionData(item.Name));
            }
            DepartmentID.ClearOptions();
            list.Add(notchoose);
            DepartmentID.AddOptions(list);
            DepartmentID.value = DepartmentID.options.Count - 1;

            list.Clear();
            titles.AddRange(Kernel.Current.Sql.LoadEntitys<Title>());
            foreach (var item in titles)
            {
                list.Add(new Dropdown.OptionData(item.Name));
            }
            TitleID.ClearOptions();
            list.Add(notchoose); 
            TitleID.AddOptions(list);
            TitleID.value = TitleID.options.Count - 1;

            query.onClick.AddListener(() =>
            {
                string sql = null;
                bool oneWhere = true;
                if (NationText.input.text.IsNotEmpty())
                {
                    sql += $"Nation like \'%{NationText.input.text}%\' ";
                    oneWhere = false;
                }
                if(BirthDay.transform.Find("PickButton").GetComponent<Button>().interactable)
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} BirthDay=\'{(new SqlDateTime(BirthDay.DateTime)).ToSqlString()}\' ";
                }
                if (NameText.input.text.IsNotEmpty())
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} Name=\'{NameText.input.text}\' ";
                }
                if (PoliticalOutlookText.input.text.IsNotEmpty())
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} PoliticalOutlook like \'%{PoliticalOutlookText.input.text}%\' ";
                }
                if (EducationText.input.text.IsNotEmpty())
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} Education=\'{EducationText.input.text}\' ";
                }
                if (AddressText.input.text.IsNotEmpty())
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} Address like \'%{AddressText.input.text}%\' ";
                }
                if (infoText.input.text.IsNotEmpty())
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} Info like \'%{infoText.input.text}%\' ";
                }
                if (PositionID.value != PositionID.options.Count - 1)
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} PositionID={positions[PositionID.value].ID} ";
                }
                if (TitleID.value != TitleID.options.Count - 1)
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} TitleID={titles[TitleID.value].ID} ";
                }
                if (DepartmentID.value != DepartmentID.options.Count - 1)
                {
                    string and = null;
                    if (oneWhere == true)
                    {
                        oneWhere = false;
                    }
                    else
                    {
                        and = "and";
                    }
                    sql += $"{and} DepartmentID={departments[DepartmentID.value].ID} ";
                }
                try
                {
                    Debug.Log(sql);
                    var result = Kernel.Current.Sql.QueryWhere<Personnel>(sql);
       
                    Kernel.Current.Desktop.OpenNew<SearchResultForm>().AddItems(new List<Personnel>(result));
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "错误！", "输入的数据全为空或格式有错误！");
                }
            });
            datetimeToggle.onValueChanged.AddListener(x =>
            {
                BirthDay.transform.Find("PickButton").GetComponent<Button>().interactable = x;
            });

            datetimeToggle.isOn = false;

            goBack.onClick.AddListener(() => Kernel.Current.Desktop.GoBack());
        }   
    }
}
