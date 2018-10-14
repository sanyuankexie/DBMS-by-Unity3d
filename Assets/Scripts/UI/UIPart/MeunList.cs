namespace DBMS.UI
{
    using UnityEngine;
    using DBMS.Systems;
    using DBMS.Utils;

    public class MeunList : Meun
    {
        protected override void Awake()
        {
            base.Awake();
            actionDir.Add("设置壁纸", () =>
            {
                string sss = Win32API.GetOpenFileName();
                Sprite s = Kernel.Current.Image.Load(sss, 1400, 900);
                if (s != null)
                {
                    Kernel.Current.Desktop.background.sprite = s;
                    PlayerPrefs.SetString("Desktop", sss);
                }
            });
        }
    }
}
