using PsdCommon.da;
using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public partial class daUser
{
   private psdWebUtility.clsClass c;
   public clsClasses cc;
   private stLocalValues v;
   /* TODO ERROR: Skipped RegionDirectiveTrivia */
   public partial struct stLocalValues
   {
      public string t;
      public string tRcp;
   }
   /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
   /* TODO ERROR: Skipped RegionDirectiveTrivia */
   public mUser fGetUserInfo(int pUserId, long pHspId)
   {
      cc = new clsClasses();
      string[] aWhere = new[] { "", "[wg.user_id=" + c.fn.fString(pUserId) + "][wg.hsp_id=" + c.fn.fString(pHspId) + "]" };
      mUser vUser = new mUser();
      string imageBase64Data = "";
      string vSelect = " TOP(1) wg.hsp_id,wg.user_title ,wg.user_id,wg.wg_id,";
      vSelect += "ISNULL(w.wg_title, ' ') AS wg_title,ISNULL(wg.user_image, ' ') AS user_image ";
      string vFrom = cc.t.WgUser + " AS wg ";
      vFrom += "left JOIN " + cc.t.Wg + " AS w ON(w.hsp_id=wg.hsp_id and wg.wg_id=w.wg_id) ";
      string vSql = c.db.fGetSql(vSelect, vFrom, aWhere[0], aWhere[1], "", "");
      DataTable vDt = c.da.fDt(vSql);
      DataRow vR = c.da.fRow(vSql);
      if (vR is null)
         return null;
      {
         var withBlock = vUser;
         withBlock.pUserTitle = vR["user_title"].ToString();
         withBlock.wg_title = vR["wg_title"].ToString();
         withBlock.user_image = Encoding.ASCII.GetBytes(vR["user_image"].ToString());
         if (DBNull.Value.Equals(withBlock.user_image))
         {
            withBlock.user_image = new byte[0];
            withBlock.user_image_string = "~/images/profile/unknown_profile.png";
            goto exit_line;
         }
         else
            imageBase64Data = Convert.ToBase64String(withBlock.user_image);
         withBlock.user_image_string = string.Format("data:image/png;base64,{0}", imageBase64Data);
      }
   exit_line:
      ;
      return vUser;
   }
   public bool fLoad(string pCnnStr, long pHspId, long pUserId, string pSender)
   {
      c = new psdWebUtility.clsClass();
      c.fLoad(pCnnStr, pHspId, pUserId, "daUser");
      fLoadLocalValues();
      return default;
   }

   private bool fLoadLocalValues()
   {
      v = new stLocalValues();
      v.t = "tbl_wg_users";
      v.tRcp = "tbl_wg_user_recep";
      return default;
   }
   ///* TODO ERROR: Skipped EndRegionDirectiveTrivia */
   //public mUser1 fUserExist(string pWhere)
   //{
   //   mUser1 fUserExistRet = default;
   //   var vUser = new mUser1();
   //   string vSql = @"
   //SELECT TOP 1 user_code,pp_id,hsp_id,user_title,user_id 
   //FROM " + v.t + @"
   //WHERE @pWhere }" + pWhere.ToString();
   //   DataRow vR = c.da.fRow(vSql);
   //   if (vR is null)
   //      goto exit_line;
   //   vUser.pUserCode = vR("user_code");
   //   vUser.pp_id = vR("pp_id");
   //   vUser.hsp_id = vR("hsp_id");
   //   vUser.pUserTitle = vR("user_title");
   //   vUser.pUserId = vR("user_id");
   //exit_line:
   //   ;
   //   fUserExistRet = vUser;
   //   return fUserExistRet;
   //}

   //public long fUserInsert(mUser1 pUser)
   //{
   //   long fUserInsertRet = default;
   //   bool vResulat;
   //   DataTable vDt = c.da.fDtEmpty(v.t);
   //   DataRow vR = vDt.NewRow;
   //   vR("hsp_id") = pUser.hsp_id;
   //   vR("user_id") = c.da.fGetNewId(v.t, "user_id", "");
   //   vR("user_code") = pUser.pUserCode;
   //   vR("pp_id") = pUser.pp_id;
   //   vR("first_name") = pUser.first_name;
   //   vR("last_name") = pUser.last_name;
   //   vR("user_mbl") = pUser.user_mbl;
   //   vDt.Rows.Add(vR);
   //   vResulat = c.da.fInsert(vDt);
   //   if (vResulat)
   //   {
   //      fUserInsertRet = vR("pp_id");
   //   }
   //   else
   //   {
   //      fUserInsertRet = 0L;
   //   }

   //   return fUserInsertRet;
   //}

   //public long fUserInsert1(mUser1 pUser)
   //{
   //   long fUserInsert1Ret = default;
   //   int vUserId = c.da.fGetNewId(v.t, "user_id", "");
   //   var vSql = "INSERT INTO " + v.t + "(hsp_id,user_id,user_code,pp_id,first_name,last_name,user_identify,user_mbl,user_title)" + "VALUES(0" + pUser.hsp_id.ToString + "," + vUserId.ToString() + ",'" + pUser.pUserCode.ToString + "',0" + pUser.pp_id.ToString + ",'" + pUser.first_name.ToString + "'" + ",'" + pUser.last_name.ToString + "','" + pUser.pPassword.ToString + "','" + pUser.user_mbl.ToString + "','" + pUser.first_name + " " + pUser.last_name + "')";
   //   int vRes;
   //   vRes = c.da.fExecute(vSql);
   //   if (vRes > 0)
   //   {
   //      fUserInsert1Ret = vUserId;
   //   }
   //   else
   //   {
   //      fUserInsert1Ret = 0L;
   //   }

   //   return fUserInsert1Ret;
   //}

   //public int fUserRecepInsert(mUser1 pUserRecepe)
   //{
   //   int fUserRecepInsertRet = default;
   //   bool vResulat;
   //   DataTable vDt = c.da.fDtEmpty(v.tRcp);
   //   DataRow vR = vDt.NewRow;
   //   vR("rcp_id") = c.da.fGetNewId(v.tRcp, "rcp_id", "");
   //   vR("pp_id") = pUserRecepe.pp_id;
   //   vR("security_code") = pUserRecepe.security_code;
   //   vR("status_id") = pUserRecepe.status_id;
   //   vR("user_code") = pUserRecepe.pUserCode;
   //   vR("rcp_date") = c.d.fFNow(pUserRecepe.rcp_date);
   //   vR("rcp_time") = c.t.fNowTimeTotalMinutes();
   //   vR("user_id") = pUserRecepe.user_id;
   //   vR("user_pass") = pUserRecepe.pPassword;
   //   vDt.Rows.Add(vR);
   //   vResulat = c.da.fInsert(vDt);
   //   if (vResulat)
   //   {
   //      fUserRecepInsertRet = vR("rcp_id");
   //   }
   //   else
   //   {
   //      fUserRecepInsertRet = 0;
   //   }

   //   return fUserRecepInsertRet;
   //}

   //public int fUserRecepUpdate(mUser1 pUserRecepe)
   //{
   //   int fUserRecepUpdateRet = default;
   //   var vWhere = "[rcp_id=" + c.fn.fString(pUserRecepe.rcp_id.ToString) + "]";
   //   var vSql = "SELECT rcp_id,rcp_date,rcp_time," + "user_id,status_id" + " FROM " + v.tRcp + " WHERE @pWhere }" + vWhere;
   //   DataTable vDt = c.da.fDt(vSql);
   //   if (vDt.Rows.Count == 0)
   //   {
   //      fUserRecepUpdateRet = 0;
   //      return fUserRecepUpdateRet;
   //   }

   //   DataRow vR = vDt.Rows(0);
   //   vR("user_id") = pUserRecepe.user_id;
   //   vR("rcp_date") = c.d.fFNow(pUserRecepe.rcp_date);
   //   vR("rcp_time") = c.t.fNowTimeTotalMinutes();
   //   vR("status_id") = pUserRecepe.status_id;
   //   vDt.TableName = v.tRcp;
   //   fUserRecepUpdateRet = c.da.fUpdate(vDt, "rcp_id");
   //   return fUserRecepUpdateRet;
   //}
}
