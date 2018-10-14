using DBMS.Data.Entity;
using UnityEngine;
using UnityEngine.UI;
using DBMS.Systems;
using System.Collections.Generic;
namespace DBMS.UI
{
    public class DListForm : ListForm<Department>
    {

        public override void Awake()
        {
            base.Awake();
            List<Department> ll = new List<Department>();
            var li = Kernel.Current.Sql.LoadEntitys<Department>();
            foreach (var item in li)
            {
                if (item.ID!=0)
                {
                    ll.Add(item);
                }
            }
            AddItems(ll);
        }

        public override void ClearItem(Transform item)
        {
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            item.GetComponentInChildren<Text>().text = string.Empty;
        }

        public override void MachiningData(Department value, Transform item)
        {
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<DInfoForm>().SetData(value);
            });
            item.GetComponentInChildren<Text>().text = $"部门编号: {value.ID } 部门名称: {value.Name} 部门信息: {value.Info}";
        }
    }
}
