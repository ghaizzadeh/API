using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PsdCommon.da
{
 public  class daService
   {
      private psdWebUtility.clsClass c;
      public clsClasses cc;
      private string vpFNow = "";
      public int ppSourceType { get; set; } = 4;
      public string ppAppPrvCode { get; set; } = "a";
      public void fLoad(psdWebUtility.clsClass pWebClass, clsClasses pClass)
      {
         c = pWebClass;
         cc = pClass;
      }
      public string ppError { get; set; }
      public mPatSrvs fGetPatSrvParam(int pRcpId,long pHspId)
      {
         DataRow vRow = fGetPatSrvDt(pRcpId,pHspId);
         mPatSrvs vOption = new mPatSrvs();
         if (vRow != null)
         {
            vOption = new mPatSrvs
            {
               rcp_id = int.Parse(vRow["rcp_id"].ToString()),
               dr_cost = int.Parse(vRow["dr_cost"].ToString()),
               room_cost = int.Parse(vRow["room_cost"].ToString()),
               ins_cost = int.Parse(vRow["ins_cost"].ToString()),
               dif_cost  = int.Parse(vRow["dif_cost"].ToString()),
               k_ins_cost = int.Parse(vRow["k_ins_cost"].ToString()),
               k_dif_cost = int.Parse(vRow["k_dif_cost"].ToString()),
               ins_pay = int.Parse(vRow["ins_pay"].ToString()),
               oth_pay = int.Parse(vRow["oth_pay"].ToString()),
               pat_pay= int.Parse(vRow["pat_pay"].ToString()),
               sbs_pay = int.Parse(vRow["sbs_pay"].ToString()),
               uni_pay = int.Parse(vRow["uni_pay"].ToString()),
               dis_pay = int.Parse(vRow["dis_pay"].ToString()),
               csh_pay = int.Parse(vRow["csh_pay"].ToString()),
            };
         }
         return vOption;
      }
      public DataRow fGetPatSrvDt(int pRcpId,long pHspId)
      {
         string[] aWhere = { "", "" };
         aWhere[1] = "[ps.rcp_id=" + c.fn.fString(pRcpId) + "][ps.hsp_id=" + c.fn.fString(pHspId) + "]";
         string vSelect = "ps.rcp_id, ISNULL(ps.dr_cost, 0) AS dr_cost, ISNULL(ps.room_cost, 0) AS room_cost, ";
         vSelect += " ISNULL(ps.ins_cost, 0) AS ins_cost, ISNULL(ps.dif_cost, 0) AS dif_cost, ";
         vSelect+= "  ISNULL(ps.k_ins_cost, 0) AS k_ins_cost, ISNULL(ps.k_dif_cost, 0) AS k_dif_cost, ";
         vSelect += " ISNULL(ps.ins_pay, 0) AS ins_pay, ISNULL(ps.oth_pay, 0) AS oth_pay, ";
         vSelect += " ISNULL(ps.pat_pay, 0) AS pat_pay, ISNULL(ps.uni_pay, 0) AS uni_pay, ";
         vSelect += "ISNULL(ps.sbs_pay, 0) AS sbs_pay,ISNULL(ps.csh_pay, 0) AS csh_pay,";
         vSelect += "ISNULL(ps.dis_pay, 0) AS dis_pay";
         string vFrom =   cc.t.patSrvs + " AS ps ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "");
         DataRow vRow = c.da.fRow(vSql);
         return vRow;

      }
   }
}
