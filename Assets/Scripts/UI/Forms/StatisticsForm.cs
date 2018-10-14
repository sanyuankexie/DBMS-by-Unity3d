using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using DBMS.Data.Entity;
using System.Collections.Generic;
using DBMS.Systems;
using System.Linq;
namespace DBMS.UI
{
    public class StatisticsForm : CanGoBackForm
    {
        public Dropdown dropdown;
        public WMG_Pie_Graph pie;
        public BarGraph bar;
        public Button lookPie;
        public Button lookAxis;
        public RectTransform panel;
        public override void Awake()
        {
            base.Awake();
            pie.Init();
            pie.interactivityEnabled = true;
            //pie.useDoughnut = true;
            pie.doughnutPercentage = 0.5f;
            pie.explodeLength = 0;
            pie.WMG_Pie_Slice_MouseEnter += (pieGraph, aSlice, hover) =>
            {
                //Debug.Log("Pie Slice Hover: " + pieGraph.sliceLabels[aSlice.sliceIndex]);
                if (hover)
                {
                    Vector3 newPos = pie.getPositionFromExplode(aSlice, 30);
                    WMG_Anim.animPosition(aSlice.gameObject, 1, Ease.OutQuad, newPos);
                }
                else
                {
                    Vector3 newPos = pie.getPositionFromExplode(aSlice, 0);
                    WMG_Anim.animPosition(aSlice.gameObject, 1, DG.Tweening.Ease.OutQuad, newPos);
                }
            };
            lookPie.onClick.AddListener(() =>
            {
                pie.gameObject.SetActive(true);
                bar.gameObject.SetActive(false);
                switch (dropdown.value)
                {
                    case 0:
                        {
                            Department[] arr = Kernel.Current.Sql.LoadEntitys<Department>();
                            pie.sliceLabels.SetList(arr.Select(x=>x.Name));
                            float[] farr = new float[arr.Length];
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var l = Kernel.Current.Sql.QueryWhere<Personnel>($"DepartmentID={arr[i].ID}").Length;
                                farr[i] = l;
                            }
                            Color[] colors = new Color[farr.Length];
                            for (int i = 0; i < colors.Length; i++)
                            {
                                colors[i] = new Color(Random.value, farr[i], Random.value);
                            }
                            pie.sliceColors.SetList(colors);
                            pie.sliceValues.SetList(farr);      
                            
                        }
                        break;
                    case 1:
                        {
                            Title[] arr = Kernel.Current.Sql.LoadEntitys<Title>();
                            pie.sliceLabels.SetList(arr.Select(x => x.Name));
                            float[] farr = new float[arr.Length];
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var l = Kernel.Current.Sql.QueryWhere<Personnel>($"TitleID={arr[i].ID}").Length;
                                farr[i] = l;
                            }
                            Color[] colors = new Color[farr.Length];
                            for (int i = 0; i < colors.Length; i++)
                            {
                                colors[i] = new Color(Random.value, farr[i], Random.value);
                            }
                            pie.sliceColors.SetList(colors);
                            pie.sliceValues.SetList(farr);
                        }
                        break;
                    case 2:
                        {

                            Position[] arr = Kernel.Current.Sql.LoadEntitys<Position>();
                            pie.sliceLabels.SetList(arr.Select(x => x.Name));
                            float[] farr = new float[arr.Length];
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var l = Kernel.Current.Sql.QueryWhere<Personnel>($"PositionID={arr[i].ID}").Length;
                                farr[i] = l;
                            }
                            Color[] colors = new Color[farr.Length];
                            for (int i = 0; i < colors.Length; i++)
                            {
                                colors[i] = new Color(Random.value, farr[i], Random.value);
                            }
                            pie.sliceColors.SetList(colors);
                            pie.sliceValues.SetList(farr); 
                        }
                        break;
                    default:
                        break;
                }
            });
            lookAxis.onClick.AddListener(() =>
            {
                pie.gameObject.SetActive(false);
                bar.gameObject.SetActive(true);
                switch (dropdown.value)
                {
                    case 0:
                        {
                            Department[] arr = Kernel.Current.Sql.LoadEntitys<Department>();
                            float[] farr = new float[arr.Length];
                            float count = 0;
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var l = Kernel.Current.Sql.QueryWhere<Personnel>($"DepartmentID={arr[i].ID}").Length;
                                farr[i] = l;
                                count += l;
                            }
                            bar.SetValues(arr.Select(x => x.Name).Zip(farr, (x, y) =>
                              {
                                  return new KeyValuePair<string, float>(x, y);
                              }).ToList(),count);
                        }
                        break;
                    case 1:
                        {
                            Title[] arr = Kernel.Current.Sql.LoadEntitys<Title>();
                            float[] farr = new float[arr.Length];
                            float count = 0;
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var l = Kernel.Current.Sql.QueryWhere<Personnel>($"DepartmentID={arr[i].ID}").Length;
                                farr[i] = l;
                                count += l;
                            }
                            bar.SetValues(arr.Select(x => x.Name).Zip(farr, (x, y) =>
                            {
                                return new KeyValuePair<string, float>(x, y);
                            }).ToList(),count);
                        }
                        break;
                    case 2:
                        {
                            Position[] arr = Kernel.Current.Sql.LoadEntitys<Position>();
                            float[] farr = new float[arr.Length];
                            float count = 0;
                            for (int i = 0; i < arr.Length; i++)
                            {
                                var wherelen = Kernel.Current.Sql.QueryWhere<Personnel>($"PositionID={arr[i].ID}").Length;
                                farr[i] = wherelen;
                                count += wherelen;
                            }
                            bar.SetValues(arr.Select(x => x.Name).Zip(farr, (x, y) =>
                            {
                                return new KeyValuePair<string, float>(x, y);
                            }).ToList(),count);
                        }
                        break;
                    default:
                        break;
                }
            });
        }
        public override void OnOpen()
        {
            panel.localScale = new Vector3(0.8f, 0.8f, 1);
            panel.DOScale(Vector3.one, animationTime);
            DoBGAnimOpen();
        }
        public override void Close()
        {
            bar.Clear();
            panel.localScale = Vector3.one;
            panel.DOScale(new Vector3(0.8f, 0.8f, 1), animationTime);
            DoBGAnimClose().OnKill(() => {    
                base.Close();
            });
        }
    }
}
