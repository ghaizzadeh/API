using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PsdCommon.Models
{
   public class mSection
   {
      public string sec_ids { get; set; }
      public int sec_id { get; set; }
      public int bed_cnt { get; set; }
      public string sec_title { get; set; }
      public int pat_now_count { get; set; } // تعداد بیماران بستری
      public int day_pat_now_count { get; set; } // تعداد بیماران بستری روز
      public int day_pat_out_count { get; set; } // تعداد بیماران ترخیص شده روز
      public int from_pat_trn_count { get; set; } // تعداد بیماران پذیرش شده از بخش دیگر
      public int to_pat_trn_count { get; set; } // تعداد بیماران انتقالی به بخش دیگر
      public List<mSection> section_dtl { get; set; }
      public double occup_prcnt {get; set;}
      public string pError { get; set; }
   }
}
