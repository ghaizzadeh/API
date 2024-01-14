using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsdCommon.Models
{
   public class mUser
   {

      public int rcp_id { get; set; }
      public string pKey { get; set; }
      public long hsp_id { get; set; }
      public long pp_id { get; set; }
      public long app_id { get; set; }
      public int pUserId { get; set; }
      public string pUserCode { get; set; }
      //[Required(ErrorMessage = "تکمیل رمز عبور الزامی است.")]
      public string pPassword { get; set; }
      public string pUserTitle { get; set; }
      public string pWgTitle { get; set; }
      public string pLastDate { get; set; }
      public string rcp_date { get; set; }
      public string pLastTime { get; set; }
      public string pLastDateString { get; set; }
      public string pLastTimeString { get; set; }
      public string pCount { get; set; }
      public bool pIsOk { get; set; }
      public string pError { get; set; }
      public string secure_code { get; set; }
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string user_mbl { get; set; }
      public int status_id { get; set; }

      // <IsPassword>
      // <Required(ErrorMessage:="تکمیل رمز عبور جدید الزامی است.")>
      public string pNewPassword { get; set; }
      public int pSecure { get; set; }
      public int is_forget { get; set; }
      public int related_pat_no { get; set; }
      public string wg_title { get; set; }
      public byte[] user_image { get; set; }
      public string user_image_string { get; set; }
      public int pSessionUserId { get; set; }
      public string pPageUrl { get; set; }

   }
}
