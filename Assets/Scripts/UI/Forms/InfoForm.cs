using UnityEngine;
using DBMS.Data.Entity;
using UnityEngine.UI;
using DBMS.Systems;
using System;
using DBMS.Utils;
using DG.Tweening;

namespace DBMS.UI
{
    public enum InfoFormWorkMode
    {
        ViewAndModifiy = 1,
        CreateNew = 2,
    }

    public class InfoForm : AbstractInfoForm
    {

        public Text title;

        public Button UpdateToSql;
        
        public Button FacialPhotoButtion;
        public Image facialPhoto;

        public Button archivalPhotoButton;
        public Button archivalPhotoUpdateButton;
        private string archivalPhotoUpdateFileName = null;

        public InputFieldVerify PhoneText;

        public InputFieldVerify IdText;

        public InputFieldVerify IdCardText;

        public Button delete;

        private InfoFormWorkMode state;

        private Personnel personnelModify;

        private static Sprite nomalSprite = null;

        public InfoForm SetWorkMode(InfoFormWorkMode state)
        {
            this.state = state;
            switch (state)
            {
                case InfoFormWorkMode.ViewAndModifiy:
                    {
                        title.text = "人员基本信息";
                        IdText.input.interactable = false;
                        IdCardText.input.interactable = false;
                        delete.gameObject.SetActive(true);
                    }
                    break;
                case InfoFormWorkMode.CreateNew:
                    {
                        IdText.input.interactable = true;
                        IdCardText.input.interactable = true;
                        personnel = Personnel.Create();
                        title.text = "人员基本信息(创建)";
                        SetData(personnel);
                        delete.gameObject.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
            return this;
        }

        public override void Awake()
        {
            base.Awake();
            if (nomalSprite == null)
            {
                nomalSprite = facialPhoto.sprite;
            }
            IdText.input.onValueChanged.AddListener((s) =>
            {
                if (VerifyUtils.IsNumber(s))
                {
                    personnel.ID = int.Parse(s);
                    IdText.Verify = true;
                    return;
                }
                IdText.Verify = false;
            });
            IdCardText.input.onValueChanged.AddListener((s) =>
            {
                if (VerifyUtils.IsIDCard(s))
                {
                    personnel.IDCard = s;
                    IdCardText.Verify = true;
                    return;
                }
                IdCardText.Verify = false;
            });
            archivalPhotoUpdateButton.onClick.AddListener(() =>
            {
                if (personnel != null)
                {
                    string file = Win32API.GetOpenFileName();
                    if (file != null)
                    {
                        archivalPhotoUpdateFileName = file;
                    }                  
                }
            });
            archivalPhotoButton.onClick.AddListener(() =>
            {
                if (personnel != null)
                {                   
                    if (!Kernel.Current.Image.View(archivalPhotoUpdateFileName))
                    {
                        Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "打开失败!", "图片不存在或被其他程序占用无法打开.");
                    }
                }
            });
            FacialPhotoButtion.onClick.AddListener(() =>
            {
                Sprite s = Kernel.Current.Image.Load(nomalSprite.texture.width, nomalSprite.texture.height);
                if (s != null)
                {
                    facialPhoto.sprite = s;
                }
            });
            PhoneText.input.onValueChanged.AddListener((s) =>
            {
                if (s?.Length <= 20)
                {
                    PhoneText.Verify = true;
                    personnel.Phone = s;
                    return;
                }
                PhoneText.Verify = false;
            });
            UpdateToSql.onClick.AddListener(() =>
            {
                switch (state)
                {
                    case InfoFormWorkMode.ViewAndModifiy:
                        {
                            UpdateImage();
                            Debug.Log(personnel.ArchivalPhoto);
                            Debug.Log(personnel.FacialPhoto);
                            Kernel.Current.Sql.UpdateEntity(personnel);
                            Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "修改完成", "修改成功");
                        }
                        break;
                    case InfoFormWorkMode.CreateNew:
                        {
                            if (Kernel.Current.Sql.LoadEntity<Personnel>(personnel.ID) == null)
                            {
                                Kernel.Current.Desktop.OpenNew<TextBoxForm>().SetCallback("请输入人员入职信息", (x) =>
                                {
                                    EntryRecord entryRecord = new EntryRecord();
                                    entryRecord.Info = x;
                                    entryRecord.Time = DateTime.Now;
                                    entryRecord.PersonnelID = personnel.ID;
                                    Kernel.Current.Sql.Insert(entryRecord);
                                    UpdateImage();
                                    Kernel.Current.Sql.InsertEntity(personnel);
                                    Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "修改完成", "修改成功");
                                }, null);
                            }
                            else
                            {
                                Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "此编号已存在", "请尝试其他编号");
                            }
                        }
                        break;
                    default:
                        break;
                }

            });
            delete.onClick.AddListener(() =>
            {
                Kernel.Current.Desktop.OpenNew<TextBoxForm>().SetCallback("添加此人员离职信息.", x =>
                {
                    Kernel.Current.Desktop.OpenNew<YesOrNoForm>().SetDialog(() =>
                    {
                        TurnoverRecord turnoverRecord = new TurnoverRecord();
                        turnoverRecord.Info = x;
                        turnoverRecord.PersonnelID = personnel.ID;
                        turnoverRecord.Time = DateTime.Now;
                        Kernel.Current.Sql.Insert(turnoverRecord);
                        Kernel.Current.Sql.DeleteEntity(personnel);
                    }, null, "确认操作", "确认执行吗?");
                }, null);

            });
            SetWorkMode(InfoFormWorkMode.ViewAndModifiy);
        }

        private void UpdateImage()
        {
            if (archivalPhotoUpdateFileName != null)
            {
                Kernel.Current.Image.RawCopy((personnel.ArchivalPhoto != null ? (personnel.ArchivalPhoto) : (personnel.ArchivalPhoto = global::System.Guid.NewGuid().ToString())), archivalPhotoUpdateFileName, null);
            }
            if (facialPhoto.sprite != nomalSprite)
            {
                Kernel.Current.Image.Update((personnel.FacialPhoto != null ? (personnel.FacialPhoto) : (personnel.FacialPhoto = global::System.Guid.NewGuid().ToString())), facialPhoto.sprite, null);
            }
        }

        public override void SetData(Personnel personnel)
        {
            base.SetData(personnel);
            IdText.input.text = personnel.ID.ToString();
            IdCardText.input.text = personnel.IDCard;
            PhoneText.input.text = personnel.Phone;
            Debug.Log(personnel.ArchivalPhoto);
            Debug.Log(personnel.FacialPhoto);
            if (personnel.ArchivalPhoto != null)
            {
                archivalPhotoUpdateFileName = Kernel.Current.Image.GetPath(personnel.ArchivalPhoto);
            }
            if (personnel.FacialPhoto != null)
            {
                facialPhoto.sprite = Kernel.Current.Image.Query(personnel.FacialPhoto);
            }
        }

        public override void Close()
        {
            DoBGAnimClose();
            DoPanelClose().OnKill(() => {
                if (state == InfoFormWorkMode.CreateNew)
                {
                    Kernel.Current.Desktop.topBar.SetTopBarItem(true);
                }
                Destroy(gameObject);
            });
        }
    }
}
