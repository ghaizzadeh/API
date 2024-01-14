using System;
using System.Collections.Generic;
using System.Data;
using PsdCommon.Models;
using System.Text;

namespace PsdCommon.da
{
   public class daPatient
   {
      private psdWebUtility.clsClass c;
      public clsClasses cc;
      public void fLoad(psdWebUtility.clsClass pWebClass, clsClasses pClass)
      {
         c = pWebClass;
         cc = pClass;
      }
      public string ppError { get; set; }

      private string[] fGetWhere(int? pOstId)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "", "" };
         if (pOstId != null && pOstId > 0)
         {
            aWhere[1] = "[c.ost_id=" + pOstId.ToString() + "]";
         }

         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }
      public DataRow fPatRow(long pPatId, long pHspId)
      {
         string[] aWhere = { "", "[p.pat_id=" + pPatId.ToString() + "][p.hsp_id=" + pHspId.ToString() + "]" };
         string vSelect = " p.pat_id ";
         string vFrom = "tbl_patients As p ";
         vFrom += "INNER JOIN tbl_pat_now AS pn ON(p.pat_id=pn.pat_id AND p.hsp_id=pn.hsp_id)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "");
         DataRow  vRow = c.da.fRow (vSql);
         return vRow;
      }
      public long fPatientInsert(DataTable pPatDt)
      {
         pPatDt.TableName = "tbl_patients";
         long i = c.da.fInsert(pPatDt);
         return i;
      }
      public long fPatientNoiwInsert(DataTable pPatNowDt)
      {
         pPatNowDt.TableName = "tbl_pat_now";
         long i = c.da.fInsert(pPatNowDt);
         return i;
      }

   }
}
