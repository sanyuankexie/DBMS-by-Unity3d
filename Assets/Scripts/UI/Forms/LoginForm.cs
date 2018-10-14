using UnityEngine.UI;
using UnityEngine;
using DBMS.Systems;
using DBMS.Utils;
using System.Collections;
using DG.Tweening;
using DBMS.Data.Entity;

namespace DBMS.UI
{
    public class LoginForm : Form
    {
        public Button login;
        public Button quit;
        public InputField loginName;
        public InputField password;
        public RectTransform loginBox;

        public void Awake()
        {
            login.onClick.AddListener(() =>
            {
                LoginType login = Kernel.Current.VerifyLogin(loginName.text, password.text);
                if (login != LoginType.NotLogin)
                {
                    Kernel.Current.Desktop.GoBack();
                    string s = null;
                    switch (login)
                    {
                        case LoginType.Administrator:
                            s = "数据库管理员";
                            break;
                        case LoginType.DataEntryOnly:
                            s = "数据库人录入人员";
                            break;
                        case LoginType.DataMaintainer:
                            s = "数据库维护人员";
                            break;
                        default:
                            break;
                    }
                    Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(() =>
                    {
                        Kernel.Current.Desktop.OpenNew<MainForm>();
                    }, "登陆成功！", "欢迎回来: " + loginName.text + "身份为: " + s);
                    return;
                }
                Reload();
                Kernel.Current.Desktop.OpenNew<DialogForm>().SetDialog(null, "用户名或密码错误！", "请尝试重新输入。");
            });

            quit.onClick.AddListener(()=> 
            {
                Application.Quit();
            });
        }

        public override void OnOpen()
        {
            DoBoxAnimOpen();
            DoBGAnimOpen();
        }

        public void Reload()
        {
            loginName.text = string.Empty;
            password.text = string.Empty;
        }

        protected Tweener DoBoxAnimOpen()
        {
            loginBox.localScale = new Vector3(0.8f, 0.8f, 1);
            return loginBox.DOScale(Vector3.one, animationTime * 1.5f);
        }

        protected Tweener DoBoxAnimClose()
        {
            loginBox.localScale = Vector3.one;
            return loginBox.DOScale(new Vector3(0.8f, 0.8f, 1), animationTime);
        }
    }
}
