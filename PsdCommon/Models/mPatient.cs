using System;
using System.Collections.Generic;
using System.Text;

namespace PsdCommon.Models
{
  public class mPatient
   {
      public long pp_id { get; set; }
      public long pat_id { get; set; }
      public long hsp_id { get; set; }
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string father_name { get; set; }
      public int is_sex { get; set; }
      public string birth_date { get; set; }
      public string ins_no2 { get; set; }
      public string home_adr { get; set; }
      public string h_id { get; set; }
      //رشته برگردانده شده از سرویس استحقاق درمان
      public string ins_service_str { get; set; }
      //صندوق بیمه بازگردانده شده 
      public string ins_id_list { get; set; }
      public string pError { get; set; }
      public List<mInsurance> InsList { get; set; }
      public List<mInsurance> OthInsList { get; set; }
   }
}
