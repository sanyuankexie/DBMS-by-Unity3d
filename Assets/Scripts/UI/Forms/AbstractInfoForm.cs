using UnityEngine;
using System.Collections.Generic;
using DBMS.Data.Entity;
using UnityEngine.UI;
using DBMS.Utils;
using DBMS.Systems;
using SpringGUI;
using DG.Tweening;

namespace DBMS.UI
{
    public abstract class AbstractInfoForm : CanGoBackForm
    {
        [System.Serializable]
        public struct InputFieldVerify
        {
            public InputField input;
            public bool Verify;
        }

        public InputFieldVerify infoText;

        public InputFieldVerify AddressText;

        public InputFieldVerify PoliticalOutlookText;

        public InputFieldVerify NameText;

        public InputFieldVerify EducationText;

        public InputFieldVerify NationText;

        public Dropdown DepartmentID;
        protected List<Department> departments;

        public Dropdown PositionID;
        protected List<Position> positions;

        public Dropdown TitleID;
        protected List<Title> titles;

        public DatePicker BirthDay;

        public RectTransform panel;

        protected Personnel personnel;

        public override void Awake()
        {
            base.Awake();
            departments = new List<Department>();
            positions = new List<Position>();
            titles = new List<Title>();
            TitleID.onValueChanged.AddListener((index) =>
            {
                personnel.TitleID = titles[index].ID;
            });
            DepartmentID.onValueChanged.AddListener((index) =>
            {
                personnel.DepartmentID = departments[index].ID;
            });
            PositionID.onValueChanged.AddListener((index) =>
            {
                personnel.PositionID = positions[index].ID;
            });
            infoText.input.onValueChanged.AddListener((s) =>
            {
                if (s == null || s == string.Empty)
                {
                    personnel.Info = string.Empty;
                    infoText.Verify = true;
                    return;
                }
                else if (s.Length <= 100)
                {
                    personnel.Info = s;
                    infoText.Verify = true;
                    return;
                }
                infoText.Verify = false;
            });
            AddressText.input.onValueChanged.AddListener((s) =>
            {
                if (s == null)
                {
                    AddressText.Verify = false;
                    return;
                }
                else if (s.Length <= 50)
                {
                    AddressText.Verify = true;
                    personnel.Address = s;
                    return;
                }
                AddressText.Verify = false;
            });
            NameText.input.onValueChanged.AddListener((s) =>
            {
                if (s == null || s == string.Empty)
                {
                    NameText.Verify = false;
                    return;
                }
                else if (s.Length <= 20)
                {
                    NameText.Verify = true;
                    personnel.Name = s;
                    return;
                }
                NameText.Verify = false;
            });
            EducationText.input.onValueChanged.AddListener((s) =>
            {
                if (s?.Length <= 20)
                {
                    EducationText.Verify = true;
                    personnel.Education = s;
                    return;
                }
                EducationText.Verify = false;
            });
            NationText.input.onValueChanged.AddListener((s) =>
            {
                if (s?.Length <= 20)
                {
                    NationText.Verify = true;
                    personnel.Nation = s;
                    return;
                }
                NationText.Verify = false;
            });
            PoliticalOutlookText.input.onValueChanged.AddListener((s) =>
            {
                if (s?.Length <= 20)
                {
                    PoliticalOutlookText.Verify = true;
                    personnel.PoliticalOutlook = s;
                    return;
                }
                PoliticalOutlookText.Verify = false;
            });
        }

        public override void OnOpen()
        {
            DoBGAnimOpen();
            DoPanelOpen();
        }

        public virtual void SetData(Personnel personnel)
        {
            if (personnel == null)
            {
                Debug.LogError("NULL");
                return;
            }
            this.personnel = personnel;
            infoText.input.text = personnel.Info;
            AddressText.input.text = personnel.Address;
            PoliticalOutlookText.input.text = personnel.PoliticalOutlook;
            NameText.input.text = personnel.Name;
            //DepartmentID
            departments.Clear();
            departments.AddRange(Kernel.Current.Sql.LoadEntitys<Department>());
            List<Dropdown.OptionData> olist = new List<Dropdown.OptionData>();
            foreach (var item in departments)
            {
                olist.Add(new Dropdown.OptionData(item.Name));
            }
            DepartmentID.options.Clear();
            DepartmentID.AddOptions(olist);
            DepartmentID.value = departments.FindIndex(x => x.ID == personnel.DepartmentID);
            //PositionID
            olist = new List<Dropdown.OptionData>();
            positions.Clear();
            positions.AddRange(Kernel.Current.Sql.LoadEntitys<Position>());
            foreach (var item in positions)
            {
                olist.Add(new Dropdown.OptionData(item.Name));
            }           
            PositionID.options.Clear();
            PositionID.AddOptions(olist);
            PositionID.value = positions.FindIndex(x => x.ID == personnel.PositionID);
            //TitleID
            olist = new List<Dropdown.OptionData>();

            titles.Clear();
            titles.AddRange(Kernel.Current.Sql.LoadEntitys<Title>());
            foreach (var item in titles)
            {
                olist.Add(new Dropdown.OptionData(item.Name));
            }
            TitleID.options.Clear();
            TitleID.AddOptions(olist);
            olist.Clear();
            TitleID.value = positions.FindIndex(x => x.ID == personnel.TitleID);
            //
            EducationText.input.text = personnel.Education;
            NationText.input.text = personnel.Nation;
            BirthDay.DateTime = personnel.BirthDay;
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

        public override void Close()
        {
            DoBGAnimClose();
            DoPanelClose().OnKill(() => base.Close());
        }
    }
}
