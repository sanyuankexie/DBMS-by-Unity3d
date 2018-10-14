using UnityEngine.EventSystems;
using UnityEngine;
namespace DBMS.Utils
{
    public static class StaticUtils
    {
        public static RectTransform GetRectTransform(this UIBehaviour uIBehaviour)
        {
            return uIBehaviour.transform as RectTransform;
        }
    }

}