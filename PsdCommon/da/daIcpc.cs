using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PsdCommon.da
{
   public class daIcpc
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
      public DataTable fIcpcDt(long pHspId)
      {
         string[] aWhere = { "", "" };
         string vSelect = " ic.icpc_id,ic.icpc_code,ISNULL(icpc_title,'') AS icpc_title ";
         string vFrom = " TBL_ICPC2P  AS ic ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }
   }
}
