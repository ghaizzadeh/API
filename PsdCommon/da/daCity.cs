using System;
using System.Collections.Generic;
using System.Data;
using PsdCommon.Models;
using System.Text;
using System.Linq;

namespace PsdCommon.da
{
   public class daCity
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
      public List<mOption> fCityList(int? pOstId)
      {
         DataTable vDt=  fCityDtByOst(pOstId);
         List<DataRow> rows = vDt.Rows.Cast<DataRow>().ToList();
         List<mOption> aOptions= new List<mOption>();
         mOption vOption;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {
               vOption = new mOption();
               vOption.id=int.Parse(row["city_id"].ToString());
               vOption.value = row["city_title"].ToString();
               aOptions.Add(vOption);
            }
         }
         return aOptions;
      }
      public DataTable fCityDtByOst(int? pOstId)
      {
         var aWhere = fGetWhere(pOstId);
         string vSelect = " c.city_id,ISNULL(c.city_title,'') AS city_title ";
         string vFrom = "" + cc.t.city + " As c ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "c.city_id");
         DataTable vDt = c.da.fDt(vSql);

         return vDt;

      }
      public DataTable fCityDtByCityId(int pCityId)
      {
         string [] aWhere = { "", "[c.city_id=" + pCityId.ToString() + "]" };
         string vSelect = " c.city_id,ISNULL(c.city_title,'') AS city_title,ost_id ";
         string vFrom = "" + cc.t.city + " As c ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "c.city_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }
   }
}
