using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using UnityEngine;
using DBMS.Systems;
using IvyOrm;

namespace DBMS.Data.Entity
{
    public interface IDataEntity
    {
        int ID { get; set; }
    }

    public enum LoginType
    {
        NotLogin = 0,
        //未登录
        Administrator,
        //可以添加部门,删除部门,添加职称,删除职称,添加职位删除职位,添加人员删除人员
        DataEntryOnly,
        //只能执行数据录入操作
        DataMaintainer,
        //可以添加和删除人员
    }

    public class SystemUser : IDataEntity//系统用户
    {
        public int ID { get; set; }
        public string Username { get; set; }//用户登录名
        public string Password { get; set; }//登录密码
        public LoginType Login { get; set; }
    }

    public class Department : IDataEntity//部门
    {
        public int ID { get; set; }//部门ID
        public string Name { get; set; }//部门名称
        public string Info { get; set; }//部门备注信息
        public string Call { get; set; }//部门联系电话
        public int? MangerID { get; set; }//部门经理的ID
        public int? DeputyManagerID { get; set; }//副手
    }

    public class Position : IDataEntity //职位
    {
        public int ID { get; set; }//职位ID
        public string Name { get; set; }//职位名
        public string Info { get; set; }//职位详细信息
    }

    public class Personnel : IDataEntity //人员
    {
        public static Personnel Create()
        {
            Personnel personnel = new Personnel();
            personnel.ID = 0;
            personnel.Name = string.Empty;
            personnel.Nation = string.Empty;
            personnel.Phone = string.Empty;
            personnel.Address = string.Empty;
            personnel.FacialPhoto = null;
            personnel.ArchivalPhoto = null;
            personnel.IDCard = string.Empty;
            personnel.Info = string.Empty;
            personnel.TitleID = 0;
            personnel.PoliticalOutlook = string.Empty;
            personnel.BirthDay = DateTime.Now;
            personnel.DepartmentID = 0;
            personnel.PositionID = 0;
            personnel.Education = string.Empty;
            return personnel;
        }
        [PrimaryKey]
        public int ID { get; set; }//员工ID
        public string Nation { get; set; }//民族
        public string IDCard { get; set; }//身份证
        public string Address { get; set; }//家庭地址
        public string PoliticalOutlook { get; set; }//政治面貌
        public string Education { get; set; }//学历
        public string Name { get; set; }//员工名称
        public string Info { get; set; }//员工备注信息,简历
        public int DepartmentID { get; set; }//员工所在部门的ID
        public string Phone { get; set; }//员工手机号码
        public DateTime BirthDay { get; set; }//生日
        public int PositionID { get; set; }//员工职位
        public int TitleID { get; set; }//职称
        public string FacialPhoto { get; set; }//员工头部照片
        public string ArchivalPhoto { get; set; }//档案照片
    }

    public class Title : IDataEntity //职称
    {
        public int ID { get; set; }//职称ID
        public string Name { get; set; }//职称名称
    }

    public class TitleRecord //职称记录
    {
        public int PersonnelID { get; set; }//职称所有人ID
        public int TitleID { get; set; }//所拥有的职称
    }

    public interface IJournalRecord
    {
        DateTime Time { get; set; }//入职时间
        int PersonnelID { get; set; }//入职的员工
        string Info { get; set; }//入职备注信息,入职渠道
    }

    public class EntryRecord : IJournalRecord  //员工入职记录
    {
        public DateTime Time { get; set; }//入职时间
        public int PersonnelID { get; set; }//入职的员工
        public string Info { get; set; }//入职备注信息,入职渠道
    }

    public class TurnoverRecord : IJournalRecord//员工离职记录
    {
        public DateTime Time { get; set; }//离职时间
        public int PersonnelID { get; set; }//离职的员工
        public string Info { get; set; }//离职备注信息,离职原因
    }

}