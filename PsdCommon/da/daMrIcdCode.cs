using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PsdCommon.da
{
  public class daMrIcdCode
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

      private string[] fGetWhere( string pKey, int pType)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "", "" };
         if (pType==1)
         {
            aWhere[0] += "icd_code LIKE '" + pKey.ToString() + "%'";
         }
         else if(pType==2)
         {
            aWhere[0] += "icd_title LIKE '" + pKey.ToString() + "%'";
         }
         aWhere[1] += "[icd.is_delete=0]";
         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }
      public List<mOption> fMrIcdList( string pKey, int pType)
      {
         DataTable vDt = fMrIcdListDt( pKey, pType);
         List<DataRow> rows = vDt.Rows.Cast<DataRow>().ToList();
         List<mOption> aOptions = new List<mOption>();
         DataRow vRow;
         mOption vOption;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {
               vOption = new mOption();
               vOption.id = int.Parse(row["icd_id"].ToString());
               if (pType == 1)
               {
                  vOption.value = row["icd_code"].ToString();
               }
               else if (pType == 2)
               {
                  vOption.value = row["icd_title"].ToString();
               }
             
             
               aOptions.Add(vOption);
            }
         }
         return aOptions;
      }
      public DataTable fMrIcdListDt( string pKey, int pType)
      {
         var aWhere = fGetWhere(pKey, pType);
         string vSelect = "icd.icd_id,icd.icd_code,ISNULL(icd.icd_title,'') AS icd_title ";
         string vFrom = "" + cc.t.mrIcdCode + " As icd ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "icd.icd_code");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }

      public  mOption  fMrIcdGet(long pIcdId)
      {
    
         List<mOption> aOptions = new List<mOption>();
         DataRow vRow = fMrIcdDt( pIcdId);
         mOption vOption=new mOption(); 
         if (vRow != null)
         {
            vOption = new mOption { id = int.Parse(vRow["icd_id"].ToString()),
               value = vRow["icd_title"].ToString(),
               code = vRow["icd_code"].ToString()};
            
         }
         return vOption;
      }
      public DataRow fMrIcdDt(long pIcdId)
      {
         string[] aWhere = {"","" };
         aWhere[1] = "[icd.icd_id =" + pIcdId.ToString() + "]";
         string vSelect = "icd.icd_id,icd.icd_code,ISNULL(icd.icd_title,'') AS icd_title ";
         string vFrom = "" + cc.t.mrIcdCode + " As icd ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "icd.icd_code");
         DataRow vRow = c.da.fRow(vSql);
         return vRow;

      }
   }
}
