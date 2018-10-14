using UnityEngine;
using DBMS.Data.Entity;
using DBMS.Data;
using DBMS.Utils;
using System.Collections;
using UnityEngine.Events;
using DBMS.UI;
using System;
using System.Threading.Tasks;
namespace DBMS.Systems
{
    public class Kernel : MonoBehaviour
    {
        public static Kernel Current { get; private set; }
        public Desktop Desktop; 
        public MsSqlService Sql { get; private set; }
        public ImageService Image { get; private set; }
        public ExcelService Excel { get; private set; }
        private void Awake()
        {
            Debug.Log(Application.persistentDataPath);
            Current = this;
            StartCoroutine(LoadStyle());
            Sql = new MsSqlService();
            Image = new ImageService();
            Excel = new ExcelService();
            Application.logMessageReceived += (string condition, string stackTrace, LogType type) =>
            {
                if (type == LogType.Error)
                {
#if !UNITY_EDITOR
                    Desktop.OpenNew<DialogForm>().SetDialog(() => Application.Quit(), "系统内部错误", condition);
#endif
                }
            };
        }


        private void Start()
        {
            Desktop.OpenNew<LoginForm>();
        }
        public LoginType VerifyLogin(string name, string password)
        {

            SystemUser[] users = Sql.LoadEntitys<SystemUser>();
            foreach (var item in users)
            {
                //Debug.Log(item.Username + " " + item.Password + " " + item.Login);
                if (item.Username == name && item.Password == password)
                {
                    LoginType = item.Login;
                    return LoginType;
                }
            }
            return LoginType.NotLogin;
        }
        public LoginType LoginType = LoginType.NotLogin;
        private IEnumerator LoadStyle()
        {
            Screen.SetResolution(1440, 900, false);
            yield return new WaitForSeconds(0.1f);
            Win32API.SetWindowStyle();
        }
        public Coroutine TaskRun(Action action, AsyncCallback callback)
        {
            return StartCoroutine(WaitTaskIsCompleted<object>(() =>
            {
                action?.Invoke();
                return null;
            }, callback));
        }
        public Coroutine TaskRun<T>(Func<T> action, AsyncCallback callback)
        {
            return StartCoroutine(WaitTaskIsCompleted(action, callback));
        }
        private IEnumerator WaitTaskIsCompleted<T>(Func<T> action, AsyncCallback callback)
        {
            Task task = Task.Run(action);
            while (!task.IsCanceled)
            {
                yield return null;
            }
            callback?.Invoke(task);
        }
    }
}
