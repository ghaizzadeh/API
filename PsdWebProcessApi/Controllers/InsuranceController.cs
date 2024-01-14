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
using System.Text;
using System.Threading.Tasks;

namespace PsdWebProcessApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class InsuranceController : ControllerBase
   {
      clsClasses cc;

      [HttpGet, Route("InsListDt/{pHspId}/{pInstype?}/{pTopInsId?}")]
      public string InsListDt(long pHspId, int? pInstype, int? pTopInsId)
      {
         fLoad();
         var vDt = cc.insurance.fInsDt(pHspId, pInstype, pTopInsId);
         return JsonConvert.SerializeObject(vDt);
      }
      //برگردان لیست بیمه های اصلی یا مکمل بدون استحقاق 
      [HttpGet, Route("InsList/{pHspId}/{pInstype?}/{pTopInsId?}")]
      public List<mInsurance> InsList(long pHspId, int? pInstype, int? pTopInsId)
      {
         fLoad();
         List<mInsurance> aInsList = new List<mInsurance>();
         mInsurance vIns;
         var vDt = cc.insurance.fInsDt(pHspId, pInstype, pTopInsId);
         if (vDt.Rows.Count == 0)
         {
            return aInsList;
         }
         foreach (DataRow vElement in vDt.Rows)
         {
            vIns = new mInsurance { ins_id = int.Parse(vElement["ins_id"].ToString()), ins_title = vElement["ins_title"].ToString() };
            aInsList.Add(vIns);
         }


         return aInsList;
      }
      //برگزدان کد سازملان بیمه گر در سپاس 
      [HttpGet, Route("GetBtCode2ByInsId/{pHspId}/{pInsId}")]

      //لیست بیمه های اصلی با صندوق بازگردانده شده در استحقاق
      [HttpGet, Route("GetSepasInsIdDt/{pInsIdList}/{pHspId}")]
      public string GetSepasInsIdDt(string pInsIdList, long pHspId)
      {
         fLoad();
         var vDt = cc.insurance.fGetSepasInsIdDt(pInsIdList + ",'36','37'", 0, pHspId, 1);
         return JsonConvert.SerializeObject(vDt);
      }
      //لیست بیمه های مکمل با صندوق بازگردانده شده در استحقاق
      [HttpGet, Route("GetSepasOthInsIdDt/{pInsIdList}/{pHspId}")]
      public string GetSepasOthInsIdDt(string pInsIdList, long pHspId)
      {
         fLoad();
         var vDt = cc.insurance.fGetSepasOthInsIdDt(pInsIdList, 0, pHspId);
         return JsonConvert.SerializeObject(vDt);
      }
      public string GetBtCode2ByInsId(long pHspId, int pInsId)
      {
         fLoad();
         DataTable vDt = cc.insurance.fGetSepasInsIdDt("", pInsId, pHspId, 0);
         if (vDt == null && vDt.Rows.Count > 0)
         {
            return "";
         }
         else
         {
            return vDt.Rows[0]["bt_code2"].ToString();
         }

      }
      //استحقاق
      [HttpGet, Route("GetInsService/{pPpId}/{pMdcId}/{pHspId}")]
      public mPatient GetInsService(long pPpId, int pMdcId,long pHspId)
      {
         fLoad();
         mPatient vPatient = new mPatient();
         string vExpDate;
         string vInsIdList = "";
         string vTopInsIdList = "";
         string vSuccess = "";
         string[] vInsArray = { };
         string vPeopleUrl = cc.fGetIniValue("srvPeople", "Url");
         try
         {
            var vRes = fGetInService(pPpId, pMdcId, vPeopleUrl);
            if (vRes == null)
            {
               vPatient.pError = "No-Service";
               return vPatient;
            }
            string vServiceStr = vRes.Result;
            if (vServiceStr == null)
            {
               vPatient.pError = "No-Service";
               return vPatient;
            }

            vSuccess = cc.c.fn.fGetFieldValue(vServiceStr, "Is_Success");
            if ((vSuccess != "1"))
            {
               vPatient.pError = "No-Service";
               return vPatient;
            }
            vPatient.birth_date = cc.c.fn.fGetFieldValue(vServiceStr, "birth_date");
            vPatient.birth_date = cc.c.d.fFDateFormat(vPatient.birth_date);
            vPatient.first_name = cc.c.fn.fGetFieldValue(vServiceStr, "first_Name");
            vPatient.last_name = cc.c.fn.fGetFieldValue(vServiceStr, "last_Name");
            vPatient.home_adr = cc.c.fn.fGetFieldValue(vServiceStr, "address");

            vPatient.ins_service_str = vServiceStr;
            string vSex = cc.c.fn.fGetFieldValue(vServiceStr, "sex_ststus");
            if ((vSex == "2"))
            {
               vPatient.is_sex = 1;
            }
            else
            {
               vPatient.is_sex = 0;
            }
            string vToday = cc.c.d.fFNow();
            string vInsIdStr = cc.c.fn.fGetFieldValue(vServiceStr, "Ins_id_list");
            if (!(vInsIdStr == null))
            {
               vInsArray = vInsIdStr.Split(';');
            }
            vInsIdList = "";
            vTopInsIdList = "";
            foreach (string vItem in vInsArray)
            {
               vExpDate = (vItem + "_exp_date");
               vExpDate = cc.c.fn.fGetFieldValue(vServiceStr, vExpDate);
               if ((vExpDate.Length == 8))
               {
                  if (Convert.ToInt64(vToday) > Convert.ToInt64(vExpDate))
                  {
                     continue;
                  }
               }
               vTopInsIdList += cc.c.fn.fGetFieldValue(vServiceStr, "Top_" + vItem.ToString()) + ",";
               vInsIdList += "'" + vItem.ToString() + "',";
            }
            vInsIdList = vInsIdList.TrimEnd(',');
            vTopInsIdList = vInsIdList.TrimEnd(',');
            vPatient.ins_id_list = vTopInsIdList.Replace(",", ";");
            vPatient.InsList = cc.insurance.fGetSepasInsIdList(vInsIdList + ",'36','37'", 0, pHspId);
            vPatient.OthInsList = cc.insurance.fGetSepasOthInsIdList(vInsIdList, 0, pHspId);
            return vPatient;
         }
         catch (Exception ex)
         {
            if (vSuccess != "1")
            {
               vPatient.pError = "No-Service";
            }
            return vPatient;
         }

      }
      private async Task<string> fGetInService(long pPpId, int pMdcId, string pSrvUrl)
      {
         BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
         //var address = new EndpointAddress("http://localhost:9728/srvPeople.asmx");
         var address = new EndpointAddress(pSrvUrl);
         var factory = new ChannelFactory<srvPeopleSoap>(basicHttpBinding, address);
         srvPeopleSoap channel = factory.CreateChannel();
         return await channel.fGetInsuranceData2Async(pPpId.ToString(), pMdcId);
      }
      //h_id
      [HttpGet, Route("GetHid/{pPpId}/{pMdcId}/{pInsurerValue}/{pInsurerTitle}")]
      public mPatient GetHid(long pPpId, int pMdcId, long pInsurerValue, string pInsurerTitle)
      {
         fLoad();
         mPatient vPatient = new mPatient();
         string vHidStr = "";
         try
         {
            var akey = Encoding.UTF8.GetBytes("PMICLIENTSERVICE");
            var aIv = Encoding.UTF8.GetBytes("701823003436");
            string vHspCode = cc.c.core.u.fGetParameters(1212);
            //vHspCode = "FBEA7C5A-57A7-48D9-80BA-78A74984C3AA";
            //var x = cTest.d("GUzypmgxgt/vnghjul/n6pyFZkuxRGw/X1F0HrD2ZNQ48dKPNZ9EKw==")
            var cTest = cc.c.core.get_tst(akey, aIv);
            vHspCode = cTest.d(vHspCode);
            var vRes = fGetHid(pPpId, pMdcId, vHspCode, pInsurerValue, pInsurerTitle);
            if (vRes == null)
            {
               vPatient.pError = "No-Service";
               return vPatient;
            }
            if (vRes.Result == null)
            {
               vPatient.pError = "No-Service";
               return vPatient;
            }
            vHidStr = vRes.Result;
            string vResulAction = cc.c.fn.fGetFieldValue(vHidStr, "Action");
            if (vResulAction != "1")
            {
               vPatient.pError = cc.c.fn.fGetFieldValue(vHidStr, "Error");
            }
            vPatient.h_id = cc.c.fn.fGetFieldValue(vHidStr, "Result");
            return vPatient;
         }
         catch (Exception ex)
         {
            vPatient.pError = "No-Service";
            return vPatient;
         }
      }
      private async Task<string> fGetHid(long pPpId, int pMdcId, string pHspCode, long pInsurerValue, string pInsurerTitle)
      {
         BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
         var address = new EndpointAddress("http://localhost:9728/srvPeople.asmx");
         var factory = new ChannelFactory<srvPeopleSoap>(basicHttpBinding, address);
         srvPeopleSoap channel = factory.CreateChannel();
         return await channel.fGetHIdAsync(pPpId.ToString(), pMdcId, pHspCode, pInsurerValue, pInsurerTitle, 0);
      }
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
