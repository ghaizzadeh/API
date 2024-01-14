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
   public class PatientController : Controller
   {
      clsClasses cc;
      [HttpPost, Route("IsPatExist")]
      public string  IsPatExist([FromBody] mPatient  pPatient)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
         DataRow vRow = cc.pat.fPatRow(pPatient.pat_id, pPatient.hsp_id);
         if (vRow != null) return "1";
         else
            return "0";
      }

      [HttpPost, Route("PatInsert")]
      public long PatInsert([FromBody] string pPatDt)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
         DataTable vPatDt = (DataTable)JsonConvert.DeserializeObject(pPatDt, (typeof(DataTable)));
        long vRes= cc.pat.fPatientInsert(vPatDt);
         return vRes;
      }
      [HttpPost, Route("PatNowInsert")]
      public long PatNowInsert([FromBody] string pPatNowDt)
      {
         //در صورت خالی بودن ریسیپ تایپ کل بخش ها رو برمیگردونه
         fLoad();
         DataTable vPatNowDt = (DataTable)JsonConvert.DeserializeObject(pPatNowDt, (typeof(DataTable)));
         long vRes = cc.pat.fPatientNoiwInsert(vPatNowDt);
         return vRes;
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
