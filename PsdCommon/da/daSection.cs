using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PsdCommon.da
{
   public class daSection
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
      private string[] fGetWherePatNow(long pHspId, int pSecId, string pDate)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "", "" };
         aWhere[1] = "[p.hsp_id=" + pHspId.ToString() + "][p.status_id=0][pn.pn_status_id=0]";
         if (pSecId > 0)
         {
            aWhere[1] += "[pn.sec_id=" + pSecId.ToString() + "]";
         }
         if (pDate != null && pDate != "")
         {
            aWhere[1] += "[p.recep_date=" + pDate.ToString() + "]";
         }
         aWhere[0] = "(pn.bed_id>0)";
         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }
      private string[] fGetWhere(long pHspId, int? pRecepType)
      {
         string[] fGetWhereRet = default;
         var aWhere = new[] { "", "" };
         if (pRecepType != null && pRecepType > 0)
         {
            if (pRecepType == 1 || pRecepType == 2)
            {
               aWhere[0] += "s.sec_type IN(11,12)";
            }

         }
         aWhere[1] += "[s.hsp_id=" + pHspId.ToString() + "]";
         fGetWhereRet = aWhere;
         return fGetWhereRet;
      }
      public List<mOption> fSecListByRecepType(long pHspId, int? pRecepType)
      {
         DataTable vDt = fSecDt(pHspId, pRecepType);
         List<DataRow> rows = vDt.Rows.Cast<DataRow>().ToList();
         List<mOption> aOptions = new List<mOption>();
         DataRow vRow;
         mOption vOption;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {

               vOption = new mOption();
               vOption.id = int.Parse(row["sec_id"].ToString());
               vOption.value = row["sec_title"].ToString();
               aOptions.Add(vOption);
            }
         }
         return aOptions;
      }

      public DataTable fSecDt(long pHspId, int? pRecepType)
      {
         var aWhere = fGetWhere(pHspId, pRecepType);
         string vSelect = " s.sec_id,ISNULL(s.sec_title,'') AS sec_title, sec_type ";
         string vFrom = "" + cc.t.section + " As s ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "s.sec_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }

      public DataTable fGetThisSecType(long pHspId, string pSecIds, string pSecType)
      {
         string[] aWhere = new[] { "", "" };
         aWhere[1] = "[s.hsp_id=" + pHspId.ToString() + "]";
         if (pSecType.Length > 0)
         {
            aWhere[0] = " s.sec_type IN (" + pSecType.ToString() + ") AND ";
         }
         //aWhere[0] = "s.sec_type IN (11,12) AND s.sec_id IN (" + pSecIds.ToString() + ")";
         aWhere[0] += " s.sec_id IN (" + pSecIds.ToString() + ")";
         string vSelect = " s.sec_id AS sec_id, ISNULL(s.sec_title,'') AS sec_title ";
         string vFrom = "" + cc.t.section + " As s ";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }
      public List<mSection> fThisUserSections(long pHspId, DataTable pSecDt, string pSecIds)
      {
         var aList = new List<mSection>();
         var vSection = new mSection();
         string vDate = cc.c.d.fFNow();
         foreach (DataRow vR in pSecDt.Rows)
         {
            vSection = new mSection();
            vSection.sec_ids = pSecIds;
            vSection.sec_id = int.Parse(vR["sec_id"].ToString());
            vSection.sec_title = vR["sec_title"].ToString();
            aList.Add(vSection);
         }
         return aList;
      }

      //لیست  سرویس های بخش
      public List<mService> fSecSrvList(long pHspId, int pSecId, int pSourceType, string pSrvCode)
      {
         DataTable vDt = fSecSrvDt(pHspId, pSecId, pSourceType, pSrvCode);
         List<DataRow> rows = vDt.Rows.Cast<DataRow>().ToList();
         List<mService> aList = new List<mService>();
         DataRow vRow;
         mService vService;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {

               vService = new mService
               {
                  srv_title = (string)row["srv_title"],
                  srv_code = (string)row["srv_code"],
                  srv_id = int.Parse(row["srv_id"].ToString()),
                  ins_cost = int.Parse(row["ins_cost"].ToString()),
                  dif_cost = int.Parse(row["dif_cost"].ToString())
               };
               aList.Add(vService);
            }
         }
         return aList;
      }
      public DataTable fSecSrvDt(long pHspId, int pSecId, int pSourceType, string pSrvCode)
      {
         var aWhere = new[] { "", "[secsrv.sec_id=" + pSecId.ToString() + "][secsrv.source_type=" + pSourceType.ToString() + "][srv.hsp_id=" + pHspId.ToString() + "]" };
         string vSelect = "";
         if (pSrvCode != null)
         {
            if (pSrvCode.Length > 0)
            {
               aWhere[0] += "srv.srv_code = " + pSrvCode.ToString() + "";
               vSelect = " Top 1 ";
            }
         }
         vSelect += "srv.srv_id,ISNULL(srv.srv_title,'') AS srv_title ,ISNULL(srv.srv_code,'') AS srv_code,";
         vSelect += "ISNULL(srv.ins_cost,0) AS ins_cost,ISNULL(srv.dif_cost,0) AS dif_cost ";
         string vFrom = "" + cc.t.services + " As srv ";
         vFrom += " INNER JOIN " + cc.t.secSrvs + " AS secsrv ON(srv.srv_id=secsrv.srv_id AND srv.hsp_id=secsrv.hsp_id)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "srv.srv_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }

      //لیست  دارو های بخش
      public List<mPhDrug> fSecDrugList(long pHspId, int pSecId, int pSourceType, string pDrugCode)
      {
         DataTable vDt = fSecDrugDt(pHspId, pSecId, pSourceType, pDrugCode);
         List<DataRow> rows = vDt.Rows.Cast<DataRow>().ToList();
         List<mPhDrug> aList = new List<mPhDrug>();
         DataRow vRow;
         mPhDrug vDrug;
         if (vDt != null)
         {
            foreach (DataRow row in vDt.Rows)
            {

               vDrug = new mPhDrug
               {
                  drug_title = (string)row["drug_title"],
                  drug_code = (string)row["drug_code"],
                  drug_id = int.Parse(row["drug_id"].ToString()),
                  ins_cost = int.Parse(row["ins_cost"].ToString()),
                  dif_cost = int.Parse(row["dif_cost"].ToString())
               };
               aList.Add(vDrug);
            }
         }
         return aList;
      }
      public DataTable fSecDrugDt(long pHspId, int pSecId, int pSourceType, string pDrugCode)
      {
         var aWhere = new[] { "", "[secsrv.sec_id=" + pSecId.ToString() + "][secsrv.source_type=" + pSourceType.ToString() + "][d.hsp_id=" + pHspId.ToString() + "]" };
         string vSelect = "";
         if (pDrugCode!= null)
         {
            if (pDrugCode.Length > 0)
            {
               aWhere[1] += "[d.drug_code = " + pDrugCode.ToString() + "]";
               vSelect = " Top 1 ";
            }
         }

         vSelect += "d.drug_id,ISNULL(d.drug_title,'') AS drug_title ,";
         vSelect += "ISNULL(d.DRUG_CODE,'') AS drug_code,ISNULL(d.ins_cost,0) AS ins_cost";
         vSelect += ",ISNULL(d.dif_cost,0) as dif_cost ";
         string vFrom = "tbl_ph_drugs As d ";
         vFrom += " INNER JOIN " + cc.t.secSrvs + " AS secsrv ON(d.drug_id=secsrv.srv_id AND d.hsp_id=secsrv.hsp_id)";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "d.drug_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;
      }
      //لیست  دارو های بخش
      public DataTable fSecDoctorDt(long pHspId, int pSecId, int pSourceType)
      {
         var aWhere = new[] { "", "[secsrv.sec_id=" + pSecId.ToString() + "][d.hsp_id=" + pHspId.ToString() + "]" };
         string vSelect = " ";
         vSelect += "d.doctor_id,ISNULL(d.first_name,'') +' ' +ISNULL(d.last_name,0) AS doctor_name";
         string vFrom = "tbl_doctors As d ";
         vFrom += " INNER JOIN " + cc.t.secSrvs + " AS secsrv ON(d.doctor_id=secsrv.srv_id AND d.hsp_id=secsrv.hsp_id AND secsrv.source_type=" + pSourceType.ToString() + ")";
         string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "d.doctor_id");
         DataTable vDt = c.da.fDt(vSql);
         return vDt;

      }


      //گرفتن 
      public string fGetDoctorName(int pMdcId)
      {
         DataRow vR = default;
         string vWhere = "[d.mdc_id=" + c.fn.fString(pMdcId) + "]";
         string vSelect = "  ISNULL(d.first_name, ' ')+' '+ISNULL( d.last_name, ' ') AS dr_name  ";
         string vFrom = cc.t.doctor + " AS d ";
         string vSql = c.db.fGetSql(vSelect, vFrom, "", vWhere, "", "");
         vR = c.da.fRow(vSql);
         string vRes = "";
         if (vR != null)
         {
            vRes = vR["dr_name"].ToString();
         }
         return vRes;
      }
   }
}
