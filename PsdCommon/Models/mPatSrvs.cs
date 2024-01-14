using System;
using System.Collections.Generic;
using System.Text;

namespace PsdCommon.Models
{
   public class mPatSrvs
   {
      public long hsp_id { get; set; }
      public long rcp_id { get; set; }
      public long pat_id { get; set; }
      public int dr_cost { get; set; }
      public int room_cost { get; set; }
      public int ins_cost { get; set; }
      public int dif_cost { get; set; }
      public int oth_pay { get; set; }
      public int sbs_pay { get; set; }
      public int k_ins_cost { get; set; }
      public int k_dif_cost { get; set; }
      public int ins_pay { get; set; }
      public int pat_pay { get; set; }
      public int csh_pay { get; set; }
      public int dis_pay { get; set; }
      public int uni_pay { get; set; }
   }
}
