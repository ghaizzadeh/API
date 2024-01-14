using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PsdCommon.da;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsdWebProcessApi.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class HospitalController : ControllerBase
   {
        clsClasses cc;

      [HttpGet, Route("HspListDt")]
      public string HspListDt()
      
      {
         fLoad();
         var vDt = cc.hospital.fHospitalDt();
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
