using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PsdCommon.da;
using PsdCommon.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PsdWebProcessApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class SectionController : ControllerBase
   {
      clsClasses cc;

      [HttpPost , Route("IsPatExist")]
      public string IsPatExist([FromBody]  string  pHspId)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
          DataTable dt = (DataTable)JsonConvert.DeserializeObject(pHspId, (typeof(DataTable)));
         return "";
       }

      [HttpGet, Route("SecListDt/{pHspId}/{pRecepType?}")]
      public string SecListDt(long pHspId,int? pRecepType)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
         var vDt = cc.section.fSecDt(pHspId, pRecepType);
         return JsonConvert.SerializeObject(vDt);
      }
      [HttpGet, Route("SecListByRecepType/{pHspId}/{pId?}")]
      public List<mOption> SecListByRecepType(long pHspId ,int? pId)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
         List<mOption> aOptions = new List<mOption>();
         aOptions = cc.section.fSecListByRecepType(pHspId,pId);
         return aOptions;
      }
      [HttpGet, Route("UserSecList/{pUserId}/{pHspId}/{pSecType}")]
      public List<mSection> UserSecList(long pUserId, long pHspId,string pSecType)
      {

         fLoad();
         List<mSection> aSection = new List<mSection>();
         var vSecDt = cc.c.user.fGrantAppIdList(pUserId, 0, 3);
         string vSecIds = "";
         foreach (DataRow vR in vSecDt.Rows)
         {
            vSecIds += vR["detail_id"].ToString() + ",";
         }
         vSecIds = vSecIds.ToString().TrimEnd(',');
         if (pSecType=="NO")
         {
            pSecType = "";
         }
         var vThisSecTypeDt = cc.section.fGetThisSecType(pHspId, vSecIds, pSecType);
         aSection = cc.section.fThisUserSections(pHspId, vThisSecTypeDt, vSecIds);
         return aSection;
      }


      //لیست سرویس های یک بخش خاص را برمیگرداند
      [HttpGet, Route("SecSrvList/{pHspId}/{pSecId}/{pSourceType}/{pSrvCode?}")]
      public List<mService> SecSrvList(long pHspId, int pSecId, int pSourceType, string pSrvCode)
      {
          fLoad();
         List<mService> aList = new List<mService>();
         aList = cc.section.fSecSrvList(pHspId, pSecId, pSourceType, pSrvCode);
         return aList;
      }
      //لیست دارو های یک بخش خاص را برمیگرداند
      [HttpGet, Route("SecDrugList/{pHspId}/{pSecId}/{pSourceType}/{pDrugCode?}")]
      public List<mPhDrug> SecDrugList(long pHspId, int pSecId, int pSourceType, string pDrugCode)
      {
         fLoad();
         List<mPhDrug> aList = new List<mPhDrug>();
         aList = cc.section.fSecDrugList(pHspId, pSecId, pSourceType, pDrugCode);
         return aList;
      }
      //لیست  پزشکان یک بخش خاص را برمیگرداند
      [HttpGet, Route("SecDoctorDt/{pHspId}/{pSecId}/{pSourceType}")]
      public string SecDoctorDt(long pHspId, int pSecId, int pSourceType)
      {
         fLoad();
         var vDt = cc.section.fSecDoctorDt(pHspId, pSecId, pSourceType);
         return JsonConvert.SerializeObject(vDt);
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
//