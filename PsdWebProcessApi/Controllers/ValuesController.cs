using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PsdCommon.da;
using PsdCommon.Models;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace PsdWebProcessApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ValuesController : ControllerBase
   {
      clsClasses cc;
      [HttpGet, Route("IcpcDt/{pHspId}")]
      //icd
      public string IcpcDt(long pHspId)
      {
         fLoad();
         var vDt = cc.icpc.fIcpcDt(pHspId);
         return JsonConvert.SerializeObject(vDt);
      }
      [HttpGet, Route("MrIcdList/{pKey}/{pType}")]
      public List<mOption> MrIcdList(string pKey, int pType)
      {
         fLoad();
         List<mOption> aOptions = new List<mOption>();
         aOptions = cc.mrIcdCode.fMrIcdList(pKey,pType);
         return aOptions;
      }
      [HttpGet, Route("MrIcdGet/{pId}")]
      public mOption  MrIcdGet( long pId)
      {
         fLoad();
         mOption VOption = new  mOption ();
         VOption = cc.mrIcdCode.fMrIcdGet(pId);
         return VOption;
      }
      //استعلام
      [HttpGet, Route("GetPeopleData/{pPpId}/{pBirthYear}/{pSrvUrl}")]
      public mPatient GetPeopleData(long pPpId, int pBirthYear, string pSrvUrl)
      {
         fLoad();
         mPatient vPatient = new mPatient();
         var vRes = fGetPeopleData(pPpId, pBirthYear, pSrvUrl);
         if (vRes != null && vRes.Result != null)
         {
            string[] aStr = vRes.Result.Split(",");
            if (aStr.Length > 0)
            {
               if (aStr[0].ToString() == "-1")
               {
                  vPatient.pError = "دریافت اطلاعات از سرویس استعلام ، لطفا از وصل بودن سرویس یا درستی اطلاعات مطمعن باشید";
                  return vPatient;
               }
               vPatient.first_name = aStr[1];
               vPatient.last_name = aStr[2];
               vPatient.father_name = aStr[3];
               if (aStr[3].ToString().ToLower() == "true")
               {
                  vPatient.is_sex = 1;
               }
               if (aStr[3].ToString().ToLower() == "false")
               {
                  vPatient.is_sex = 0;
               }


               vPatient.birth_date = cc.c.d.fFDateFormat(aStr[4]);
            }
         }
         return vPatient;
      }
      private async Task<string> fGetPeopleData(long pPpId, long pBirthYear, string pSrvUrl)
      {
         ServiceReference1.stName st;
         BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
         var address = new EndpointAddress("http://localhost:9728/srvPeople.asmx");
         var factory = new ChannelFactory<srvPeopleSoap>(basicHttpBinding, address);
         srvPeopleSoap channel = factory.CreateChannel();
         st = await channel.fGetPeopleDataAsync(pPpId.ToString(), pBirthYear);
         return st.sPpId +","+ st.sFirstName + "," + st.sLastName + "," + st.sFatherName + "," + st.sIsSex + "," + st.sBirthDate;
      }
      //is_sex
      [HttpGet, Route("SexDt")]
      public string SexDt()
      {
         fLoad();
         DataTable vDt = new DataTable();
         vDt.Columns.Add("tb_id");
         vDt.Columns.Add("tb_title");
         DataRow vR = vDt.NewRow();
         vR["tb_id"] = 0;
         vR["tb_title"] = " مرد ";
         vDt.Rows.Add(vR);
         vR = vDt.NewRow();
         vR["tb_id"] = 1;
         vR["tb_title"] = "  زن ";
         vDt.Rows.Add(vR);
           return JsonConvert.SerializeObject(vDt);
      }

      //shift_id
      [HttpGet, Route("ShiftDt")]
      public string ShiftDt()
      {
         fLoad();
         DataTable vDt = new DataTable();
         vDt.Columns.Add("tb_id");
         vDt.Columns.Add("tb_title");
         DataRow vR = vDt.NewRow();
         vR["tb_id"] = 1;
         vR["tb_title"] = " صبح ";
         vDt.Rows.Add(vR);
         vR = vDt.NewRow();
         vR["tb_id"] = 2;
         vR["tb_title"] = "  عصر ";
         vDt.Rows.Add(vR);
         return JsonConvert.SerializeObject(vDt);
      }
      //نحوه دریافت وجه
      [HttpGet, Route("GetPayWayDt")]
      public string GetPayWayDt()
      {
         fLoad();
         DataTable vDt = new DataTable();
         vDt.Columns.Add("tb_id");
         vDt.Columns.Add("tb_title");
         DataRow vR = vDt.NewRow();
         vR["tb_id"] = 1;
         vR["tb_title"] = "نقدی ";
         vDt.Rows.Add(vR);
         vR = vDt.NewRow();
         vR["tb_id"] = 2;
         vR["tb_title"] = "فیش بانکی ";
         vDt.Rows.Add(vR);
         vR = vDt.NewRow();
         vR["tb_id"] = 3;
         vR["tb_title"] = "کارت خوان";
         vDt.Rows.Add(vR);
         return JsonConvert.SerializeObject(vDt);
      }
      [HttpGet, Route("InsLimitedDt")]
      public string InsLimitedDt()
  {
   fLoad();
   DataTable vDt = new DataTable();
   vDt.Columns.Add("tb_id");
   vDt.Columns.Add("tb_title");
   DataRow vR = vDt.NewRow();
   vR["tb_id"] = 00;
   vR["tb_title"] = " بیمه کامل   ";
   vDt.Rows.Add(vR);
   vR = vDt.NewRow();
   vR["tb_id"] = 02;
   vR["tb_title"] = "بیمه در انتهای دوره ";
   vDt.Rows.Add(vR);   
   vR = vDt.NewRow();
   vR["tb_id"] = 03;
   vR["tb_title"] = "بیمه در ابتدای دوره";
   vDt.Rows.Add(vR);
   return JsonConvert.SerializeObject(vDt);
  }
      //مشخصات کاربر
      [HttpPost, Route("UserInfo")]
      public mUser UserInfo(mUser pUser)
      {
         fLoad();
         mUser vUser = new mUser();
         vUser = cc.user.fGetUserInfo(pUser.pUserId,pUser.hsp_id);
         return vUser;
      }
      [HttpGet, Route("HspInfo/{pHspId}")]
      public mHsp HspInfo(long pHspId)
      {
         fLoad();
         mHsp vHsp = new mHsp();
         vHsp = cc.hsp.fGetHspInfo(pHspId);
         return vHsp;
      }

      //مشخصات بیمارستان
      private bool fLoad()
      {
         var aId = fGetId();
         if (cc != null)
         {

            if (aId[0] == cc.ppHspId & aId[1] == cc.ppUserId)
               goto exit_line;
         }

         else cc = new clsClasses();
         cc.fLoad(aId[0], aId[1], "Reports");
         HttpContext.Session.SetString("page_url", cc.ppUrl);
      exit_line:
         ;
         cc.ppRptId = cc.c.fn.fNumeric(cc.c.fn.fString(HttpContext.Session.GetString("rpt_id")));
         return default;
      }
   
   private long[] fGetId()
   {
      long[] fGetIdRet = default;
      var aId = new[] { 0L, 0L };
      string vHspId, vUserId;
      if (HttpContext.Session.GetString("hsp_id") != null)
      {
         vHspId = HttpContext.Session.GetString("hsp_id");
         aId[0] = Convert.ToInt64(vHspId);
      }

      if (HttpContext.Session.GetString("user_id") != null)
      {
         vUserId = HttpContext.Session.GetString("user_id");
         aId[1] = Convert.ToInt32(vUserId);
      }
      fGetIdRet = aId;
      return fGetIdRet;
   }
   }

}
