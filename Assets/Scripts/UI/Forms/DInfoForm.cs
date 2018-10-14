namespace DBMS.UI
{
    using DBMS.Systems;
    using DBMS.Data.Entity;
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;

    public enum DInfoFormWorkMode
    {
        CreateNew,
        Nomal,
    }
    public class DInfoForm : CanGoBackForm
    {
        public InputField ID;
        public InputField Info;
        public Button Manager;
        private Personnel ManagerPersonnel;
        public Button DeputyManager;
        private Personnel DeputyManagerPersonnel;
        public InputField Call;
        public InputField Name;
        private DInfoFormWorkMode state;
        public Button update;
        public Button delete;
        public RectTransform panel;
        private Department department;
        public override void Awake()
        {
            base.Awake();
            Manager.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<InfoForm>().SetData(ManagerPersonnel);
            });
            DeputyManager.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<InfoForm>().SetData(DeputyManagerPersonnel);
            });
            delete.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<YesOrNoForm>().SetDialog(() =>
                {
                    Personnel[] pl = Kernel.Current.Sql.QueryWhere<Personnel>($"DepartmentID{department.ID}");
                    for (int i = 0; i < pl.Length; i++)
                    {
                        pl[i].DepartmentID = 0;
                        Kernel.Current.Sql.UpdateEntity(pl[i]);
                    }
                }, null, "警告", "删除部门将会影响所有在此部门的人员");
            });
            update.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<YesOrNoForm>().SetDialog(() => 
                {
                    Kernel.Current.Sql.UpdateEntity(department);
                }, null, "请确认","确认执行吗？");
            });
            SetWorkMode(DInfoFormWorkMode.Nomal);
        }
        public override void OnOpen()
        {
            DoBGAnimOpen();
            DoPanelOpen();
        }
        protected Tweener DoPanelOpen()
        {
            panel.localScale = new Vector3(0.8f, 0.8f, 1);
            return panel.DOScale(Vector3.one, animationTime);
        }
        protected Tweener DoPanelClose()
        {
            panel.localScale = Vector3.one;
            return panel.DOScale(new Vector3(0.8f, 0.8f, 1), animationTime);
        }
        public void SetWorkMode(DInfoFormWorkMode state)
        {
            this.state = state;
            switch (state)
            {
                case DInfoFormWorkMode.CreateNew:
                    break;
                case DInfoFormWorkMode.Nomal:
                    {
                        ID.interactable = false;
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetData(Department department)
        {
            this.department = department;
            if (department.MangerID != null)
            {
                ManagerPersonnel = Kernel.Current.Sql.LoadEntity<Personnel>(department.MangerID.Value);
                Manager.GetComponentInChildren<Text>().text = ManagerPersonnel.Name;
            }
            else
            {
                Manager.interactable = false;
                Manager.GetComponentInChildren<Text>().text = "无";
            }
            if (department.DeputyManagerID != null)
            {
                DeputyManagerPersonnel = Kernel.Current.Sql.LoadEntity<Personnel>(department.DeputyManagerID.Value);
                Manager.GetComponentInChildren<Text>().text = DeputyManagerPersonnel.Name;
            }
            else
            {
                DeputyManager.interactable = false;
                DeputyManager.GetComponentInChildren<Text>().text = "无";
            }
            ID.text = department.ID.ToString();
            Info.text = department.Info;
            Name.text = department.Name;
            if (department.Call == null && department.Call == string.Empty)
            {
                Call.text = string.Empty;
            }
            else
            {
                Call.text = department.Call;
            }
        }
        public override void Close()
        {
            DoBGAnimClose();
            DoPanelClose().OnKill(() => Destroy(gameObject));
        }

    }
}
