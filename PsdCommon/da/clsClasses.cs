using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PsdCommon.da
{
   public class clsClasses
   {
      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      private psdWebUtility.clsClass _c;

      public psdWebUtility.clsClass c
      {
         get
         {
            psdWebUtility.clsClass cRet = default;
            cRet = _c;
            return cRet;
         }
      }

      public string ppUrl
      {
         get
         {
            string ppUrlRet = default;
            string vUrl = "";
            if (ppCnnType <= 1)
            {
               vUrl = "http://localhost:40960/";
            }
            else
            {
               vUrl = "http://nobat.mazums.ac.ir/dashboard4/";
            }

            ppUrlRet = vUrl;
            return ppUrlRet;
         }
      }
      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */


      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      public long ppHspId { get; set; }
      public long ppUserId { get; set; }
      public string ppSender { get; set; }
      public double ppRptId { get; set; }
      public double ppCnnType { get; set; }
      public double ppTopAppId { get; set; }
      public long ppDefaultAppId { get; set; }
      public bool ppIsViewUtility { get; set; }
      public bool vppIsLoad { get; set; }

      public bool fLoad(double pHspId, long pUserId, string pSender)
      {
         ppHspId = 0L;
         ppUserId = pUserId;
         ppSender = pSender;
         _c = new psdWebUtility.clsClass();
         fLoadDb();
         if (pHspId == 0L)
         {
            pHspId = c.fn.fNumeric(c.fn.fString(c.ss.fGetAppProperty(db.sCnnString, 4)));
         }

         ppHspId = Convert.ToInt64(pHspId);
         c.fLoad(db.sCnnStr, ppHspId, ppUserId, ppSender);
         ppCnnType = c.fn.fNumeric(fGetIniValue("Connection", "CnnType"));
         ppTopAppId = c.fn.fNumeric(fGetIniValue("App", "TopAppId"));
         ppIsViewUtility = c.fn.fBoolean(fGetIniValue("App", "IsViewUtility"));
         return default;
      }

      public string fGetIniValue(string pSection, string pField)
      {
         string fGetIniValueRet = default;
         string vValue = "";
         string vFile = fGetIniFile();
         if (vFile.Length == 0)
            goto exit_line;
         vValue = c.fl.fGetValue(vFile, pSection, pField);
      exit_line:
         ;
         fGetIniValueRet = vValue;
         return fGetIniValueRet;
      }

      public string fGetIniFile()
      {
         string fGetIniFileRet = default;
         //string vFile = HttpContext.Current.Server.MapPath("~/App_Data/psd.ini");

         string vFile = Path.Combine(
                      Directory.GetCurrentDirectory(), "App_Data/psd.ini", ""
                  );


         if (c.fl.fExitsPathFile(vFile, true))
            goto exit_line;
         vFile = "";
      exit_line:
         ;
         fGetIniFileRet = vFile;
         return fGetIniFileRet;
      }
      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      public string[] fAppTitles()
      {
         string[] fAppTitlesRet = default;
         var aTitle = new[] { "", "", "" };
         aTitle[0] = c.ss.fGetAppProperty(db.sCnnString, 3);
         double vTopAppId = c.fn.fNumeric(fGetIniValue("App", "TopAppId").ToString());
         if (vTopAppId > 0L)
         {
            aTitle[1] = c.da.fScalar("app_title", "tbl_apps", "", "[app_id=" + vTopAppId.ToString() + "]");
         }

         if (aTitle[1].Length == 0)
            aTitle[1] = c.ss.fGetAppProperty(db.sCnnString, 1);
         aTitle[2] = c.ss.fGetAppProperty(db.sCnnString, 6); // url
      exit_line:
         ;
         if (aTitle[0].Length == 0)
            aTitle[0] = "-";
         if (aTitle[1].Length == 0)
            aTitle[1] = "-";
         if (aTitle[2].Length == 0)
            aTitle[2] = "#";
         fAppTitlesRet = aTitle;
         return fAppTitlesRet;
      }

      public partial struct stDb
      {
         public string sCnnStr;
         public string sCnnString;
         public string sHisCnnStr;
         public string sHisCnnString;
         public bool isLoad;
      }

      private stDb _db = new stDb();

      public stDb db
      {
         get
         {
            stDb dbRet = default;
            if (!_db.isLoad)
               fLoadDb();
            dbRet = _db;
            return dbRet;
         }
      }

      private bool fLoadDb()
      {
         if (ppCnnType == 0)
            ppCnnType = c.fn.fNumeric(fGetIniValue("Connection", "CnnType"));
         _db = new stDb();
         if (ppCnnType <= 1)
         {
            //195.181.36.211 
            //_db.sCnnStr = "195.181.36.211/Booali_Sari.Net/sa/sa_961/2000000";
            //_db.sCnnStr = "192.168.1.31/Booali_Sari.Net/app_sql/pooya_sql/2000000";
            //_db.sCnnStr = "192.168.1.31/Behdasht/app_sql/pooya_sql/2000000";
            //_db.sCnnStr = "192.168.116.10/db_Shahrvand_Sari/app_sql/pooya_sql/20000000";
            //_db.sCnnStr = "94.183.107.132,60000/Booali_Sari.Net/app_sql/pooya_sql/2000000";
            //_db.sCnnStr = "195.181.36.211,3941/Booali_Sari.Net/psd_web/web@987/2000000";
            //_db.sCnnStr = "192.168.1.246,3941/db_shirgah_new/psd_web/web@987/2000000";
            //_db.sCnnStr = "192.168.1.246,3941/db_shirgah_new/psd_web/web@987/2000000";
            //_db.sCnnStr = "172.16.6.31/db_shirgah_new/app_sql/pooya_sql/2000000";
            //_db.sCnnStr = "192.168.116.10/db_Shahrvand_Sari/app_sql/pooya_sql/20000000";
            //_db.sCnnStr = "192.168.1.246,3941/db_shirgah_new/psd_web/web@987/2000000";
            //_db.sCnnStr = "(local)/Booali_Sari.Net///2000000";
            //_db.sCnnStr = "(local)/Booali_Sari.Net/sa/sa_1364/2000000";
            _db.sCnnStr= fGetIniValue("Connection", "Cnnstr");
            _db.sHisCnnStr = "(local)/@pDatabase/sa/psdco80/2000000";
         }
         else if (ppCnnType == 2)
         {
            _db.sCnnStr = "172.16.6.31/db_shirgah_new/pooya/123a@/2000000";
            //_db.sCnnStr = "192.168.116.10/db_Shahrvand_Sari/app_sql/pooya_sql/20000000";
            _db.sHisCnnStr = "192.168.1.31/@pDatabase/app_sql/pooya_sql/2000000";
         }
         else if (ppCnnType == 3)
         {
            _db.sCnnStr = "192.168.1.31/db_NobatDehi_Sari/app_sql/pooya_sql/2000000";
            _db.sHisCnnStr = "192.168.1.31/@pDatabase/app_sql/pooya_sql/2000000";
         }
         else
         {
            _db.sCnnStr = "172.16.3.77/db_NobatDehi_Sari/web/pooya_web/2000000";
            _db.sHisCnnStr = "172.16.5.12/@pDatabase/sa/sa_70182300/2000000";
         }

         _db.sCnnString = fGetCnnString(_db.sCnnStr);
         _db.sHisCnnString = fGetCnnString(_db.sHisCnnStr);
         return default;
      }

      private string fGetCnnString(string pCnnStr)
      {
         string fGetCnnStringRet = default;
         var aStr = pCnnStr.ToString().Split('/');
         fGetCnnStringRet = c.db.fGetCnnString(aStr[0], aStr[1], aStr[2], aStr[3]);
         return fGetCnnStringRet;
      }
      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      public partial struct stTable
      {
         public string app;
         public string services;
         public string secSrvs;
         public string hospital;
         public string appVer;
         public string insurance;
         public string section;
         public string service;
         public string patSrvs;
         public string pat;
         public string patNow;
         public string city;
         public string mrIcdCode;
         public string doctor;
         public string tbase;
         public string WgUser;
         public string Wg;
         public string rpt;
         public bool isLoad;
      }

      private stTable _t = new stTable();

      public stTable t
      {
         get
         {
            stTable tRet = default;
            if (!_t.isLoad)
               fLoadTableName();
            tRet = _t;
            return tRet;
         }
      }

      private bool fLoadTableName()
      {
         _t.app = "tbl_apps";
         _t.secSrvs = "tbl_sec_srvs";
         _t.services = "tbl_services";
         _t.hospital = "tbl_hospital";
         _t.rpt = "tbl_app_rpts";
         _t.appVer = "tbl_app_versions";
         _t.insurance = "tbl_insurance";
         _t.section = "tbl_sections";
         _t.patSrvs = "tbl_pat_srvs";
         _t.pat = "tbl_patients";
         _t.patNow = "tbl_pat_now";
         _t.city = "tbl_city";
         _t.mrIcdCode = "tbl_mr_icdcode";
         _t.doctor = "tbl_doctors";
         _t.tbase = "tbl_tbase_tables";
         _t.WgUser = "tbl_wg_users";
         _t.Wg = "tbl_wg";

         _t.isLoad = true;
         return default;
      }
      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */

      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      private daHospital _hsp;
      public daHospital hsp
      {
         get
         {
            if (_hsp == null)
            {
               _hsp = new daHospital();
               _hsp.fLoad(c, this);
            }
            return _hsp;
         }
      }
      private daUser _user;

      public daUser user
      {
         get
         {
            daUser userRet = default;
            if (_user is null)
            {
               _user = new daUser();
               _user.fLoad(this.db.sCnnStr, this.ppHspId, this.ppUserId, this.ppSender);
            }

            userRet = _user;
            return userRet;
         }
      }

      private daInsurance _insurance;

      public daInsurance insurance
      {
         get
         {
            daInsurance userRet = default;
            if (_insurance is null)
            {
               _insurance = new daInsurance();
               _insurance.fLoad(c, this);
            }

            userRet = _insurance;
            return userRet;
         }
      }
      private daHospital _hospital;

      public daHospital  hospital
      {
         get
         {
            daHospital userRet = default;
            if (_hospital is null)
            {
               _hospital = new daHospital();
               _hospital.fLoad(c, this);
            }

            userRet = _hospital;
            return userRet;
         }
      }
      private daDoctor _doctor;

      public daDoctor doctor
      {
         get
         {
            daDoctor userRet = default;
            if (_doctor is null)
            {
               _doctor = new daDoctor();
               _doctor.fLoad(c, this);
            }

            userRet = _doctor;
            return userRet;
         }
      }
      private daCity _city;

      public daCity city
      {
         get
         {
            daCity userRet = default;
            if (_city is null)
            {
               _city = new daCity();
               _city.fLoad(c, this);
            }

            userRet = _city;
            return userRet;
         }
      }
      private daService _Service;

      public daService Service
      {
         get
         {
            daService userRet = default;
            if (_Service is null)
            {
               _Service = new daService();
               _Service.fLoad(c, this);
            }

            userRet = _Service;
            return userRet;
         }
      }
      private daSection _section;

      public daSection section
      {
         get
         {
            daSection userRet = default;
            if (_section is null)
            {
               _section = new daSection();
               _section.fLoad(c, this);
            }

            userRet = _section;
            return userRet;
         }
      }
      private daPatient _pat;

      public daPatient pat
      {
         get
         {
            daPatient userRet = default;
            if (_pat is null)
            {
               _pat = new daPatient();
               _pat.fLoad(c, this);
            }
            userRet = _pat;
            return userRet;
         }
      }
      private daIcpc _icpc;

      public daIcpc icpc
      {
         get
         {
            daIcpc userRet = default;
            if (_icpc is null)
            {
               _icpc = new daIcpc();
               _icpc.fLoad(c, this);
            }

            userRet = _icpc;
            return userRet;
         }
      }


      private daMrIcdCode _mrIcdCode;

      public daMrIcdCode mrIcdCode
      {
         get
         {
            daMrIcdCode userRet = default;
            if (_mrIcdCode is null)
            {
               _mrIcdCode = new daMrIcdCode();
               _mrIcdCode.fLoad(c, this);
            }

            userRet = _mrIcdCode;
            return userRet;
         }
      }


      /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
      /* TODO ERROR: Skipped RegionDirectiveTrivia */
      private string vppFNow = "";

      public string ppFNow
      {
         get
         {
            string ppFNowRet = default;
            if (vppFNow.Length == 0)
               vppFNow = c.d.fFNow();
            ppFNowRet = vppFNow;
            return ppFNowRet;
         }
      }
   }
}
