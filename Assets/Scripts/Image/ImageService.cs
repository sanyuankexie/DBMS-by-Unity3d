using UnityEngine;
using System;
using System.IO;
using DBMS.Utils;
using System.Diagnostics;
namespace DBMS.Systems
{
    /// <summary>
    /// 摘要
    ///    :图片仓储服务
    /// </summary>
    public class ImageService
    {
        private void CheckDirectory()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/photo"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/photo");
            }
        }

        private readonly ScaleWorker scaleWorker;

        public ImageService()
        {
            scaleWorker = new ScaleWorker();
        }

        public void Delete(string guid)
        {
            CheckDirectory();
            string f = GetPath(guid);
            if (File.Exists(f))
            {
                File.Delete(f);
            }
        }

        public void Update(string guid, Sprite sprite, AsyncCallback callback)
        {
            if (guid == null)
            {
                UnityEngine.Debug.LogError("NULL");
                return;
            }
            CheckDirectory();
            Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
            tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                (int)sprite.rect.width, (int)sprite.rect.height));
            tex.Apply();
            // 写入成PNG文件  
            Byte[] bytes = tex.EncodeToPNG();
            string fileurli = GetPath(guid);
            UnityEngine.Debug.Log(fileurli);
            Kernel.Current.TaskRun(() => File.WriteAllBytes(fileurli, bytes), callback);//异步写入
        }

        public Sprite Query(string guid)
        {
            CheckDirectory();
            if (guid == null)
            {
                UnityEngine.Debug.LogError("NULL");
                return null;
            }
            WWW www = new WWW("file://" + GetPath(guid));
            Texture2D texture2 = www.texture;
            Sprite ret = Sprite.Create(texture2, new Rect(0, 0, texture2.width, texture2.height), Vector2.zero);
            texture2 = null;
            www = null;
            Resources.UnloadUnusedAssets();
            return ret;
        }

        public Sprite Load(string filePath, int width, int hight)
        {
            CheckDirectory();
            if (filePath != null)
            {
                WWW www = new WWW("file://" + filePath);
                Texture2D texture2 = www.texture;
                scaleWorker.ScaleBilinear(texture2, width, hight);
                var s = Sprite.Create(texture2, new Rect(0, 0, width, hight), Vector2.zero);
                www = null;
                texture2 = null;
                Resources.UnloadUnusedAssets();
                return s;
            }
            return null;
        }

        public Sprite Load(int width, int hight)
        {
            string fs = Win32API.GetOpenFileName();
            if (fs != null)
            {
                return Load(fs, width, hight);
            }
            return null;
        }

        public bool View(string file)
        {
            UnityEngine.Debug.Log(file);
            if (File.Exists(file))
            {
                try
                {
                    var proc = Process.Start(file);
                    proc.WaitForExit();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log($"Exception Occurred :{ex.Message},{ ex.StackTrace.ToString()}");
                    return false;
                }
                return true;
            }
            return false;
        }

        public string GetPath(string s)
        {
            return $"{Application.persistentDataPath}/photo/{s}.png";
        }

        public void RawCopy(string guid, string src, AsyncCallback callback)
        {
            guid = GetPath(guid);
            UnityEngine.Debug.Log(src);
            UnityEngine.Debug.Log(guid);
            if (guid == null)
            {
                UnityEngine.Debug.LogError("NULL");
                return;
            }
            Kernel.Current.TaskRun(() =>
            {
                UnityEngine.Debug.Log("ddd");
                try
                {
                    File.Copy(src, guid, true);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log(e);
                }
            }, callback);
        }
    }
}