using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PsdCommon.da
{
   public class daDoctor
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

      private string[] fGetWhere(long pHspId,int? pRecep,int? pIsAss)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "",""};
        
         if (pRecep != null && pRecep>0)
         {
            aWhere[1] += "[d.is_recep="+pRecep.ToString()+"]";
         }
         if (pRecep != null &&  pIsAss >0 )
         {
            aWhere[1] = "[d.is_ass=1]";
         }
         aWhere[1] += "[d.hsp_id=" + pHspId.ToString() + "][d.is_delete=0]";
         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }

      public DataTable fDoctorDt(long pHspId,string pType,int? pRecep,int? pIsAss)
      {
         var aWhere = fGetWhere(pHspId, pRecep, pIsAss);
         string vSelect = " ISNULL(d.doctor_id, 0) AS doctor_id, ";
         if (pType == "name")
         {
            vSelect += " '(' + ISNULL(d.last_name,'') + ' ' + ISNULL(d.first_name,'') + ')' AS dr_name ";
         }
         if (pType == "name_spc")
         {
            vSelect += " '(' + ISNULL(d.last_name,'') + ' ' + ISNULL(d.first_name,'') + '-' + ISNULL(tb.tb_title,'') + ')' AS dr_name_spc  ";
         }
         if (pType == "name_spc_mdc")
         {
            vSelect += " '(' + ISNULL(d.last_name,'') + ' ' + ISNULL(d.first_name,'') + '-' + ISNULL(tb.tb_title,'') + ' - ' + CONVERT(NVARCHAR(20), ISNULL(d.mdc_id,0)) + ')' AS dr_name_spc ";
         }
         string vFrom = "" + cc.t.doctor + " As d ";
         vFrom += " LEFT JOIN "+cc.t.tbase+" AS tb ON(d.spc_id=tb.tb_id AND tb.status_id=165001)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "d.doctor_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }
   }
}
