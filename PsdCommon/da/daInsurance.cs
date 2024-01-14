using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PsdCommon.da
{
   public class daInsurance
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
      private string[] fGetWhere(long pHspId, int? pInstype, int? pTopInsId)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "i.top_ins_id>0", "" };
         if (pInstype != null && pInstype > 0)
         {
            aWhere[1] = "[i.ins_type=" + pInstype.ToString() + "]";
         }
         if (pTopInsId != null)
         {
            aWhere[1] = "[i.top_ins_id=" + pTopInsId.ToString() + "]";
         }
         aWhere[1] += "[i.hsp_id=" + pHspId.ToString() + "][i.is_delete=0]";
         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }
      public DataTable fInsDt(long pHspId, int? pInstype, int? pTopInsId)
      {
         var aWhere = fGetWhere(pHspId, pInstype, pTopInsId);
         string vSelect = " i.ins_id,ISNULL(i.ins_title,'') AS ins_title,ins_type ";
         string vFrom = "" + cc.t.insurance + " As i ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "i.ins_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }
      //استحقاق

      public DataTable fGetSepasInsIdDt(string pBtCode, int pInsId, long pHspId,int? vIsTop)
      {
         var aWhere = new[] { "", "" };
         string vNowDate = cc.c.d.fFNow();
         if (pInsId > 0)
         {
            aWhere[0] = "i.ins_id=0" + pInsId.ToString() + " AND ";
         }
         if (pBtCode.Length > 0)
         {
            if (vIsTop!=null && vIsTop > 0)
            {
               aWhere[0] = "db9.bt_code IN(" + pBtCode.ToString() + ") AND ";
            }else
            aWhere[0] = "db8.bt_code IN(" + pBtCode.ToString() + ") AND ";
         }
         aWhere[0] += "is_delete = 0 AND top_ins_id> 0 AND ins_type = 01 AND i.hsp_id = 0" + pHspId.ToString();
         aWhere[0] += " AND (is_unlimited=1 OR ( from_date<='" + vNowDate + "' AND to_date>='" + vNowDate + "')) ";
         string vSelect = "i.ins_id,ins_title,";
         vSelect += " ISNULL(db8.bt_code,0) AS bt_code,";
         vSelect += " db9.bt_code AS bt_code2,db9.bt_title AS bt_title,top_ins_id ";
         string vFrom = cc.t.insurance + " AS i";
         vFrom += " LEFT JOIN tbl_sp_relations AS dr8 ON(i.ins_id=dr8.psd_id AND i.hsp_id=dr8.hsp_id AND dr8.data_type=30)";
         vFrom += " LEFT JOIN tbl_sp_relations AS dr9 ON(i.ins_id=dr9.psd_id AND i.hsp_id=dr9.hsp_id AND dr9.data_type=8)";
         vFrom += " LEFT JOIN tbl_sp_baseTables AS db9 ON(dr9.sp_id=db9.bt_id AND db9.status_id=210 AND db9.data_type=8)";
         vFrom += " LEFT JOIN tbl_ins_funds AS  f ON (i.ins_id=f.ins_id AND i.hsp_id=f.hsp_id)";
         vFrom += " LEFT JOIN tbl_sp_baseTables AS db8 ON(f.fnd_ins_id=db8.bt_id AND db8.status_id=210 AND db8.data_type=30)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "ins_title");
         DataTable vDt = c.da.fDt(vSql);
         DataTable vDt2 = vDt.Clone();
         int vLastInsId = 0;
         foreach (DataRow row in vDt.Rows)
         {
           
            if (vLastInsId != int.Parse(row["ins_id"].ToString()))
            {
               DataRow vRow = vDt2.NewRow();
               vRow["ins_id"] = row["ins_id"];
               vRow["ins_title"] = row["ins_title"];
               vRow["bt_code"] = row["bt_code"];
               vRow["bt_code2"] = row["bt_code2"];
               vDt2.Rows.Add(vRow);
            }
               vLastInsId = int.Parse(row["ins_id"].ToString());
         }
            return vDt2;
      }
      public DataTable fGetSepasOthInsIdDt(string pBtCode, int pInsId, long pHspId)
      {
         var aWhere = new[] { "", "" };
         string vNowDate = cc.c.d.fFNow();
         if (pInsId > 0)
         {
            aWhere[0] = "i.ins_id=0" + pInsId.ToString() + " AND ";
         }
         if (pBtCode.Length > 0)
         {
            aWhere[0] = "db9.bt_code IN(" + pBtCode.ToString() + ") AND ";
         }

         aWhere[0] += "is_delete = 0 AND top_ins_id> 0 AND ins_type = 01 AND i.hsp_id = 0" + pHspId.ToString();
         aWhere[0] += " AND (is_unlimited=1 OR ( from_date<='" + vNowDate + "' AND to_date>='" + vNowDate + "')) ";
         string vSelect = " i.ins_id,ins_title,db8.bt_code,";
         vSelect += " db9.bt_code AS bt_code2,db9.bt_title AS bt_title,top_ins_id ";
         string vFrom = cc.t.insurance + " AS i";
         vFrom += " LEFT JOIN tbl_sp_relations AS dr8 ON(i.ins_id=dr8.psd_id AND i.hsp_id=dr8.hsp_id AND dr8.data_type=30)";
         vFrom += " LEFT JOIN tbl_sp_baseTables AS db8 ON(dr8.sp_id=db8.bt_id AND db8.status_id=210 AND db8.data_type=30)";
         vFrom += " LEFT JOIN tbl_sp_relations AS dr9 ON(i.ins_id=dr9.psd_id AND i.hsp_id=dr9.hsp_id AND dr9.data_type=8)";
         vFrom += " LEFT JOIN tbl_sp_baseTables AS db9 ON(dr9.sp_id=db9.bt_id AND db9.status_id=210 AND db9.data_type=8)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "ins_title");
         DataTable vDt = c.da.fDt(vSql);
         DataTable vDt2 = vDt.Clone();
         int vLastInsId = 0;
         foreach (DataRow row in vDt.Rows)
         {

            if (vLastInsId != int.Parse(row["ins_id"].ToString()))
            {
               DataRow vRow = vDt2.NewRow();
               vRow["ins_id"] = row["ins_id"];
               vRow["ins_title"] = row["ins_title"];
               vDt2.Rows.Add(vRow);
            }
            vLastInsId = int.Parse(row["ins_id"].ToString());
         }
         return vDt2;
      }
      public List<mInsurance> fGetSepasInsIdList(string pBtCode, int pInsId, long pHspId)
      {
         DataTable vDt = fGetSepasInsIdDt(pBtCode, pInsId, pHspId,0);
         List<mInsurance> aIns = new List<mInsurance>();
         mInsurance vIns;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {
               vIns = new mInsurance();
               vIns.ins_id = int.Parse(row["ins_id"].ToString());
               vIns.ins_title = row["ins_title"].ToString();
               vIns.bt_code = int.Parse(row["bt_code"].ToString());
               vIns.bt_code2 = int.Parse(row["bt_code2"].ToString());
                aIns.Add(vIns);
            }
         }
         return aIns;
      }
      public List<mInsurance> fGetSepasOthInsIdList(string pBtCode, int pInsId, long pHspId)
      {
         DataTable vDt = fGetSepasOthInsIdDt(pBtCode, pInsId, pHspId);
         List<mInsurance> aIns = new List<mInsurance>();
         mInsurance vIns;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {
               vIns = new mInsurance();
               vIns.ins_id = int.Parse(row["ins_id"].ToString());
               vIns.ins_title = row["ins_title"].ToString();
               vIns.bt_code = int.Parse(row["bt_code"].ToString());
               aIns.Add(vIns);
            }
         }
         return aIns;
      }

   }
}
