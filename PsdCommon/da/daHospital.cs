using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PsdCommon.da
{
   public class daHospital
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

      public DataTable fHospitalDt()
      {
         string vSelect = " h.hsp_id ,ISNULL(h.hsp_title,'') AS hsp_title ";
         vSelect += ",l.lat_value AS lat,l.lng_value AS lng,l.loc_property AS url";
         vSelect += ",0 AS dr_count,0 AS bed_count,0 AS type ";
         string vFrom = "" + cc.t.hospital + " As h ";
         vFrom += "INNER JOIN tbl_hsp_locations L ON(h.hsp_id=l.hsp_id )";
         string vSql = c.db.fGetSql(vSelect, vFrom, "", "", "", "h.hsp_title");
         DataTable vDt = c.da.fDt(vSql);
         if (vDt != null &&  vDt.Rows.Count>0)
         {
            foreach (DataRow row in vDt.Rows) { 
               if(row["url"].ToString().Length > 0)
               {
                  row["dr_count"] = int.Parse(c.fn.fGetFieldValue(row["url"].ToString(), "dr_count"));
                  row["bed_count"] = int.Parse(c.fn.fGetFieldValue(row["url"].ToString(), "bed_count"));
                  row["type"] = int.Parse( c.fn.fGetFieldValue(row["url"].ToString(), "type"));
                  row["url"] ="http://"+ c.fn.fGetFieldValue(row["url"].ToString(), "pad");
          
               }
            }

         }
         return vDt;

      }
      public mHsp fGetHspInfo(long pHspId)
      {
         mHsp vHsp = new mHsp();
         string imageBase64Data = "";
         var vSql = "SELECT tb_image  FROM tbl_tbase_images WHERE @pWhere }[status_id=100][tb_id=1] ";
         DataRow vR = c.da.fRow(vSql);
         if (vR is null)
         {
            vHsp.tb_image = "";
            return vHsp;
         }
         vHsp.img_File = Encoding.ASCII.GetBytes(vR["tb_image"].ToString());
         if (DBNull.Value.Equals(vHsp.img_File))
            vHsp.img_File = new byte[0];
         else
            imageBase64Data = Convert.ToBase64String(vHsp.img_File);
         vHsp.tb_image = string.Format("data:image/png;base64,{0}", imageBase64Data);
         string vCnnStr = cc.c.daByHspId(pHspId).ppCnnStr;
         vHsp.hsp_title = cc.c.ss.fGetAppProperty(vCnnStr, 1);
         vHsp.soc_title = cc.c.ss.fGetAppProperty(vCnnStr, 3);
         return vHsp;
      }
   }
}
