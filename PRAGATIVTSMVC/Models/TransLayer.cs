
using System;
using System.ComponentModel.DataAnnotations;

public enum operation
{
    Select,
    Insert,
    Update,
    Check,
    Delete,
    Empty,
    SelectAll,
    select,
    get_structure,
    Commit,
    checkemail,
    Insertinlog,
    BranchCode,
    OTP_VERIFICATION,
    Update1,
    Stationnames,
    SelectSemi,
    SelectPartners,
    SelectClients,
    SelectCatgType,
    SelectPartnersAdmin,
    SelectClientsAdmin,
    SelectRoleTypes,
    checkIMEI
}


public class SMVTS_MAIN
{
    private int _CREATEDBY;
    public int CREATEDBY
    {
        get { return this._CREATEDBY; }
        set { this._CREATEDBY = value; }
    }

    private DateTime _CREATEDDATE;
    public DateTime CREATEDDATE
    {
        get { return this._CREATEDDATE; }
        set { this._CREATEDDATE = value; }
    }

    private int _LASTMDFBY;
    public int LASTMDFBY
    {
        get { return this._LASTMDFBY; }
        set { this._LASTMDFBY = value; }
    }

    private DateTime _LASTMDFDATE;
    public DateTime LASTMDFDATE
    {
        get { return (this._LASTMDFDATE); }
        set { this._LASTMDFDATE = value; }
    }


    private bool? _USERS_STATUS;
    public bool? USERS_STATUS
    {
        get { return (this._USERS_STATUS); }
        set { this._USERS_STATUS = value; }
    }

    private operation _OPERATION;
    public operation OPERATION
    {
        get { return (this._OPERATION); }
        set { this._OPERATION = value; }
    }


    private DateTime? _STARTDATE;
    public DateTime? STARTDATE
    {
        get { return (this._STARTDATE); }
        set { this._STARTDATE = value; }
    }

    private DateTime? _ENDDATE;
    public DateTime? ENDDATE
    {
        get { return (this._ENDDATE); }
        set { this._ENDDATE = value; }
    }

    //private int _USER_ID;
    //public int USER_ID
    //{
    //    get { return (this._USER_ID); }
    //    set { this._USER_ID = value; }
    //}
}


public class SMVTS_TRIPINFO : SMVTS_MAIN
{
    public string VehicleNo { get; set; }
    public string Reachdate { get; set; }
    public string UnloadDate { get; set; }
    public int CategId { get; set; }
    public int Userid { get; set; }
    public int DeviceId { get; set; }
}
public class SMVTS_SENDSMS_VENDORCODE : SMVTS_MAIN
{
    private string p;

    public int ID { get; set; }
    public string FORM_NAME { get; set; }
    public string VENDOR_CODE { get; set; }
    //public SMVTS_SENDSMS_VENDORCODE(int _ID,string _FORM_NAME,string _VENDOR_CODE)
    //{
    //    ID =_ID;
    //    FORM_NAME=_FORM_NAME;
    //    VENDOR_CODE =_VENDOR_CODE;
    //}

    //public SMVTS_SENDSMS_VENDORCODE(string p)
    //{
    //    // TODO: Complete member initialization
    //    this.p = p;
    //}


}

public class SMVTS_SHOPING_USERS : SMVTS_MAIN
{
    public int USER_ID { get; set; }
    public string USER_EMAILID { get; set; }
    public string USER_PASSWORD { get; set; }
    public string USER_DATE { get; set; }
    public int USER_STATUS { get; set; }
    public int USER_CREATEDBY { get; set; }
    public string USER_CREATEDDATE { get; set; }
    public int USER_MODIFIEDBY { get; set; }
    public string USER_MODIFIEDDATE { get; set; }

}
public class VehicleAutoTripDetail
{
    public int Autotrip_id { get; set; }
    public string Autotrip_VehicleNo { get; set; }
    public string Autotrip_VehicleModel { get; set; }
    public string Autotrip_StartLoc { get; set; }
    public string Autotrip_EndLoc { get; set; }
    public string Autotrip_RouteName { get; set; }
    public string Autotrip_Startdate { get; set; }
    public string Autotrip_Enddate { get; set; }
    public string Autotrip_Kms { get; set; }
    public string Autotrip_Deviceid { get; set; }
    public string Autotrip_Status { get; set; }
    public string AUTOTRIP_ROUTEID { get; set; }
    public string AUTOTRIP_AVGSPEED { get; set; }
}
public class VehicleAutoTripDetail1
{
    public string Autotrip_id { get; set; }
    public string Autotrip_VehicleNo { get; set; }
    public string Autotrip_VehicleModel { get; set; }
    public string Autotrip_StartLoc { get; set; }
    public string Autotrip_EndLoc { get; set; }
    public string Autotrip_RouteName { get; set; }
    public string Autotrip_Startdate { get; set; }
    public string Autotrip_Enddate { get; set; }
    public string Autotrip_Kms { get; set; }
    public string Autotrip_Deviceid { get; set; }
    public string Autotrip_Status { get; set; }
    public string AUTOTRIP_ROUTEID { get; set; }
    public string AUTOTRIP_AVGSPEED { get; set; }
}
public class VehicleTripDetail
{
    public string id { get; set; }
    public string VehRegNo { get; set; }
    public string UpdatedDateTime { get; set; }
    public string Transpoter { get; set; }
    public string TripID { get; set; }
    public string DriverName { get; set; }
    public string TripOrigin { get; set; }
    public string TripStartDateTime { get; set; }
    public string TripDestination { get; set; }
    public string Location { get; set; }
    public string Ignition { get; set; }
    public string Speed { get; set; }
    public string StoppedSince { get; set; }
    public string OSToday { get; set; }
    public string RAToday { get; set; }
    public string RDToday { get; set; }
    public string DistanceToday { get; set; }
    public string DistanceMonth { get; set; }
    public string TripFixedKm { get; set; }
    public string TripKmRun { get; set; }
    public string TripKmRemaining { get; set; }
    public string TripDuration { get; set; }
    public string RunHrs { get; set; }
    public string StopHrs { get; set; }
    public string OS { get; set; }
    public string OsDuration { get; set; }
    public string RA { get; set; }
    public string RD { get; set; }
    public string ND { get; set; }
    public string CR { get; set; }
    public string NearestGeofencedLoc { get; set; }
}
public class VehicleAutoTrip_Detail1
{
    public string MaxId { get; set; }

}
public class Vehicleer_tripdetaile
{
    public string ER_ID { get; set; }
    public string ER_VEHICLENO { get; set; }
    //  public string ER_VEHICLETYPE { get; set; }
    // public string ER_BOOKBRANCH { get; set; }
    // public string ER_BOOKZONE { get; set; }
    public string ER_DISPATCHDATE { get; set; }
    public string ER_FROM { get; set; }
    public string ER_TO { get; set; }
    public string ER_PARTYNAME { get; set; }
    // public string ER_DELIVERYBRANCH { get; set; }
    // public string ER_TO_ZONE { get; set; }
    public string ER_EXPECTED_DATE { get; set; }
    public string ER_REPORTING_DATE { get; set; }
    // public string ER_CURRENT_DATE { get; set; }
    //public string ER_CURRENT_STATUS { get; set; }
    // public string ER_LOCATION { get; set; }
    //public string ER_ACK_STATUS { get; set; }
    public string ER_DRIVER_NAME { get; set; }
    public string ER_DRIVER_PHONE { get; set; }
    // public string ER_CNTR_BRANCH { get; set; }
    // public string ER_FORMAN_DETAILS { get; set; }
    // public string ER_DEST_CONTROL_BRANCH { get; set; }
    //public string ER_FORMAN_BRANCH { get; set; }
    public string ER_CATEGID { get; set; }
    //public DateTime? ER_UPLOADEDDATE { get; set; }
}
public class SMVTS_PRODUCTS : SMVTS_MAIN
{
    public int PRODUCT_ID { get; set; }
    public string PRODUCT_NAME { get; set; }
    public string PRODUCT_IMAGE { get; set; }
    public string PRODUCT_DESCRIPTION { get; set; }
    public string PRODUCT_COST { get; set; }


}

public class SMVTS_ROUTES_RISKZONES : SMVTS_MAIN
{
    public string Latlongaddres { get; set; }
}

public class SMVTS_SHOPING_DETAILS : SMVTS_MAIN
{
    public int DETAILS_ID { get; set; }
    public int DETAILS_USERID { get; set; }
    public string DETAILS_USERNAME { get; set; }
    public string DETAILS_USERFULLNAME { get; set; }
    public string DETAILS_USERADDRESS { get; set; }
    public string DETAILS_USERCOUNTRY { get; set; }
    public string DETAILS_USERSTATE { get; set; }
    public string DETAILS_USERCITY { get; set; }
    public string DETAILS_USERPIN { get; set; }
    public string DETAILS_USERMOBILENO { get; set; }
    public DateTime? DETAILS_USERDATE { get; set; }
    public int DETAILS_USERSTATUS { get; set; }
    public int DETAILS_USER_CREATEDBY { get; set; }
    public DateTime? DETAILS_USER_CREATEDDATE { get; set; }
    public int DETAILS_USER_MODIFIEDBY { get; set; }
    public DateTime? DETAILS_USER_MODIFIEDDATE { get; set; }
}
public class SMVTS_SHOPINGDELIVERY_DETAILS : SMVTS_MAIN
{
    public int DELIVERY_ID { get; set; }
    public int DELIVERY_DETAILS_ID { get; set; }
    public string DELIVERY_USERNAME { get; set; }
    public string DELIVERY_USERADDRESS { get; set; }
    public string DETAILS_USERADDRESS { get; set; }
    public string DELIVERY_USERCOUNTRY_ID { get; set; }
    public string DELIVERY_USERSTATE_ID { get; set; }
    public string DELIVERY_USERCITY_ID { get; set; }
    public string DELIVERY_USERPIN { get; set; }
    public string DELIVERY_USERMOBILENO { get; set; }
    public DateTime? DELIVERY_USERDATE { get; set; }
    public int DELIVERY_DELIVERYADDRESS { get; set; }

    public int DELIVERY_USER_CREATEDBY { get; set; }
    public string DELIVERY_USER_CREATEDDATE { get; set; }
    public int DELIVERY_USER_MODIFIEDBY { get; set; }
    public string DELIVERY_USER_MODIFIEDDATE { get; set; }


}
public class smvts_flashbanner : SMVTS_MAIN
{
    public int AD_ID { get; set; }
    public string AD_NAME { get; set; }
    public string AD_EMAIL { get; set; }

    public string AD_ADDRESS { get; set; }
    public string AD_PHONENO { get; set; }
    public string AD_MOBILENO { get; set; }
    public int AD_STATUS { get; set; }
}

public class SMVTS_INCOMECALCULATOR : SMVTS_MAIN
{
    public int INCOMEID { get; set; }
    public int INCOMEVALUE { get; set; }
    public int INCOMEVALUEID { get; set; }
    public int INCOMESTATUS { get; set; }
}
public class SMVTSFLASH_TEMPLATES : SMVTS_MAIN
{
    public int TEMPLATE_ID { get; set; }
    public string TEMPLATE_NAME { get; set; }
    public string TEMPLATE_DESC { get; set; }
    public string TEMPLATE_IMAGEPATH { get; set; }
    public int TEMPLATE_STATUS { get; set; }

}
//public class SMVTS_USERS : SMVTS_MAIN
//{
//    public SMVTS_USERS()
//    {
//    }

//    public SMVTS_USERS(int _USERS_ID)
//    {
//        USERS_ID = _USERS_ID;
//    }


//    public SMVTS_USERS(int _USERS_ID, int _USERS_ROLE_ID, int _USERS_CATEGORY_ID,
//    string _USERS_USERNAME, string _USERS_PASSWORD, string _USERS_FULLNAME, bool _USERS_STATUS, string _USERS_DEVICE_IDS,
//    string _USERS_HEADERLOGO, string _USERS_HEADERLOGOURL, string _USERS_LOGO, string _USERS_LOGOURL,
//    string _USERS_MENUITEM, string _USERS_COLUMNIDS, int _USERS_TIMEZONE, string DBName)
//    {
//        USERS_ID = _USERS_ID;
//        USERS_ROLE_ID = _USERS_ROLE_ID;
//        USERS_CATEGORY_ID = _USERS_CATEGORY_ID;
//        USERS_USERNAME = _USERS_USERNAME;
//        USERS_PASSWORD = _USERS_PASSWORD;
//        USERS_FULLNAME = _USERS_FULLNAME;
//        USERS_STATUS = _USERS_STATUS;
//        USERS_DEVICE_IDS = _USERS_DEVICE_IDS;
//        USERS_HEADERLOGO = _USERS_HEADERLOGO;
//        USERS_HEADERLOGOURL = _USERS_HEADERLOGOURL;
//        USERS_LOGO = _USERS_LOGO;
//        USERS_LOGOURL = _USERS_LOGOURL;
//        USERS_MENUITEMS = _USERS_MENUITEM;
//        USERS_COLUMNIDS = _USERS_COLUMNIDS;
//        USERS_TIMEZONE = _USERS_TIMEZONE;
//        USERS_DBNAME = DBName;
//    }


//    public int USERS_ID { get; set; }
//    public int USERS_ROLE_ID { get; set; }
//    public int USERS_CATEGORY_ID { get; set; }
//    public string USERS_USERNAME { get; set; }
//    public string USERS_PASSWORD { get; set; }
//    public string USERS_FULLNAME { get; set; }
//    public bool USERS_STATUS { get; set; }
//    public string USERS_DEVICE_IDS { get; set; }
//    public string USERS_HEADERLOGO { get; set; }
//    public string USERS_HEADERLOGOURL { get; set; }
//    public string USERS_LOGO { get; set; }
//    public string USERS_LOGOURL { get; set; }
//    public string USERS_MENUITEMS { get; set; }
//    public string USERS_COLUMNIDS { get; set; }
//    public int USERS_TIMEZONE { get; set; }
//    public string USERS_DBNAME { get; set; }
//    public string USERS_PERMISSION { get; set; }


//}
public class SMVTS_USERS : SMVTS_MAIN
{
    public SMVTS_USERS()
    {
    }

    public SMVTS_USERS(int _USERS_ID)
    {
        USERS_ID = _USERS_ID;
    }


    public SMVTS_USERS(int _USERS_ID, int _USERS_ROLE_ID, int _USERS_CATEGORY_ID,
    string _USERS_USERNAME, string _USERS_PASSWORD, string _USERS_FULLNAME, string _USERS_STATUS, string _USERS_DEVICE_IDS,string _USERS_HEADERLOGO,string _USERS_HEADERLOGOURL,string _USERS_LOGO,string _USERS_LOGOURL,


    string _USERS_MENUITEM, string _USERS_COLUMNIDS, int _USERS_TIMEZONE, string DBName, string _CATEG_NAME,int _categ_catetype_id, string _categ_mobilenumber)
    {
        USERS_ID = _USERS_ID;
        USERS_ROLE_ID = _USERS_ROLE_ID;
        USERS_CATEGORY_ID = _USERS_CATEGORY_ID;
        USERS_USERNAME = _USERS_USERNAME;
        USERS_PASSWORD = _USERS_PASSWORD;
        USERS_FULLNAME = _USERS_FULLNAME;
        USERS_STATUS = _USERS_STATUS;
        USERS_DEVICE_IDS = _USERS_DEVICE_IDS;
        USERS_HEADERLOGO = _USERS_HEADERLOGO;
        USERS_HEADERLOGOURL = _USERS_HEADERLOGOURL;
        USERS_LOGO = _USERS_LOGO;
        USERS_LOGOURL = _USERS_LOGOURL;
        USERS_MENUITEMS = _USERS_MENUITEM;
        USERS_COLUMNIDS = _USERS_COLUMNIDS;
        USERS_TIMEZONE = _USERS_TIMEZONE;
        USERS_DBNAME = DBName;
        CATEG_NAME = _CATEG_NAME;
        CATEG_CATETYPE_ID = _categ_catetype_id;
        Mobilenumber = _categ_mobilenumber;
    }


    public int USERS_ID { get; set; }
    public int USERS_ROLE_ID { get; set; }
    public int USERS_CATEGORY_ID { get; set; }
    public string USERS_USERNAME { get; set; }
    public string USERS_PASSWORD { get; set; }
    public string USERS_FULLNAME { get; set; }
    public string USERS_STATUS { get; set; }
    public string USERS_DEVICE_IDS { get; set; }
    public string USERS_HEADERLOGO { get; set; }
    public string USERS_HEADERLOGOURL { get; set; }
    public string USERS_LOGO { get; set; }
    public string USERS_LOGOURL { get; set; }
    public string USERS_MENUITEMS { get; set; }
    public string USERS_COLUMNIDS { get; set; }
    public int USERS_TIMEZONE { get; set; }
    public string USERS_DBNAME { get; set; }
    public string USERS_PERMISSION { get; set; }
    public string USER_ST { get; set; }

    public string CATEG_NAME { get; set; }

    public string USERROLE_NAME { get; set; }
    public int CATEG_CATETYPE_ID { get; set; }
    public string Mobilenumber { get; set; }
    public string Email { get; set; }
}


public class SMVTS_KMLFILE : SMVTS_MAIN
{
    public int FILEID { get; set; }
    public string FILEURL { get; set; }
    public int FILECATEGID { get; set; }
}

public class SMVTS_TYRE_GRADE : SMVTS_MAIN
{
    public int GRADE_ID { get; set; }
    public string GRADE_NAME { get; set; }
    public bool GRADE_STATUS { get; set; }
}

public class SMVTS_TYRE_POSITION : SMVTS_MAIN
{
    public int POSITION_ID { get; set; }
    public string POSITION_NAME { get; set; }
    public bool POSITION_STATUS { get; set; }
}


public class SMVTS_COUNTRIES : SMVTS_MAIN
{
    public int COUNTRY_ID { get; set; }
    public string COUNTRY_NAME { get; set; }
    public string COUNTRY_DESC { get; set; }
    public int COUNTRY_CODE { get; set; }
    public string COUNTRY_TIMEZONE { get; set; }

    public SMVTS_COUNTRIES()
    {
    }

    public SMVTS_COUNTRIES(int _COUNTRY_ID)
    {
        COUNTRY_ID = _COUNTRY_ID;
    }
}

public class SMVTS_SERVICECHARGES : SMVTS_MAIN
{
    public int SERVICECHARGES_ID { get; set; }
    public string SERVICECHARGES_TIMEBASEINFORMATION { get; set; }
    public string SERVICECHARGES_AMOUNTINFORMATION { get; set; }
    public bool SERVICECHARGES_STATUS { get; set; }
    public int SERVICECHARGES_CREATEDBY { get; set; }
    public DateTime? SERVICECHARGES_CREATEDDATE { get; set; }
    public int SERVICECHARGES_MODIFIEDBY { get; set; }
    public DateTime? SERVICECHARGES_MODIFIEDDATE { get; set; }
}

public class SMVTS_CUSTOMERSERVICECHARGESOUTSTANDINGAMOUNT : SMVTS_MAIN
{
    public int SERVICECHARGES_ID { get; set; }
    public int SERVICECHARGES_CATEGID { get; set; }
    public string SERVICECHARGES_TOTALDUE { get; set; }
    public string SERVICECHARGES_MONTHLYAMOUNT { get; set; }
    public string SERVICECHARGES_MONTHNAME { get; set; }
    public DateTime? SERVICECHARGES_DATE { get; set; }
    public string SERVICECHARGES_PAYMENTSTATUS { get; set; }
    public string SERVICESCHARGES_CREATEDBY { get; set; }
    public DateTime? SERVICECHARGES_CREATEDDATE { get; set; }
    public string SERVICECHARGES_MODIFIEDBY { get; set; }
    public DateTime? SERVICECHARGES_MODIFIEDDATE { get; set; }
}
public class SMVTS_PICKUP_VEHICLES : SMVTS_MAIN
{

    public string pickup_vehicles_id { get; set; }
    public string pickup_vehicle_number { get; set; }
    public string PICKUP_VEHICLES_ROUTEID { get; set; }
    public string pickup_vehicles_categ_id { get; set; }
    public DateTime? pickup_created_date { get; set; }

}


public class SMVTS_STATES : SMVTS_MAIN
{
    public int STATE_ID { get; set; }
    public string STATE_NAME { get; set; }
    public string STATE_DESC { get; set; }
    public int STATE_COUNTRY_ID { get; set; }
    public string COUNTRY_NAME { get; set; }

    public SMVTS_STATES()
    {
    }

    public SMVTS_STATES(int _STATE_ID)
    {
        STATE_ID = _STATE_ID;
    }

}

public class SMVTS_CITIES : SMVTS_MAIN
{
    public int CITY_CITYID { get; set; }
    public string CITY_NAME { get; set; }
    public string CITY_DESC { get; set; }
    public int CITY_STATE_ID { get; set; }
    public int CITY_COUNTRY_ID { get; set; }
    public string STATE_NAME { get; set; }

    public SMVTS_CITIES()
    {
    }

    public SMVTS_CITIES(int _CITY_CITYID)
    {
        CITY_CITYID = _CITY_CITYID;
    }
}

public class SMVTS_CATEGORYTYPE : SMVTS_MAIN
{
    public int CATETYP_ID { get; set; }
    public string CATETYP_NAME { get; set; }
    public string CATETYP_DESC { get; set; }
    public bool CATETYP_STATUS { get; set; }

    public SMVTS_CATEGORYTYPE()
    {
    }

    public SMVTS_CATEGORYTYPE(int _CATETYP_ID)
    {
        CATETYP_ID = _CATETYP_ID;
    }
}

public class SMVTS_CATEGORIES : SMVTS_MAIN
{
    public string PARENT_NAME { get; set; }
    public int CATEG_ID { get; set; }
    public int CATEG_CATETYPE_ID { get; set; }
    public int CATEG_PARENT_ID { get; set; }
    public string CATEG_NAME { get; set; }
    public string CATEG_DESC { get; set; }
    public string CATEG_STATUS { get; set; }
    public string CATEG_CONTACTPERSON { get; set; }
    public string CATEG_MOBILENUMBER { get; set; }
    public string CATEG_TELEPHONE { get; set; }
    public string CATEG_FAX { get; set; }
    public string CATEG_WEBSITENAME { get; set; }
    public string CATEG_EMAILID { get; set; }
    public string CATEG_ADDRESS { get; set; }
    public int CATEG_COUNTRY_ID { get; set; }
    public int CATEG_STATE_ID { get; set; }
    public int CATEG_CITY_ID { get; set; }
    public string CATEG_ZIPCODE { get; set; }
    public int CATEG_NOOFUSERS { get; set; }
    public string CATEG_UOMSPEED { get; set; }
    public string CATEG_UOMVOLUME { get; set; }
    public int CATG_NOOFDEVICES { get; set; }
    public string CATEG_DBNAME { get; set; }
    public int CATEG_REFER_ID { get; set; }
    public string CATEG_PRODNAME { get; set; }
    public int Database_ID { get; set; }
    public string USERROLE_NAME { get; set; }
    public int ROLES_CATEGORY_ID { get; set; }
    public string Name { get; set; }
    public int ROLES_ID { get; set; }
    public string NameId { get; set; }
    public int CATEG_PACKAGE_ID { get; set; }
    public int CATEG_WLP { get; set; }
    public string Logo_Url { get; set; }
    public DateTime CATEG_VALID_TO { get; set; }
    public int MIS_ID { get; set; }
    public string username { get; set; }
    public string password { get; set; }

    public SMVTS_CATEGORIES()
    {
    }
    public SMVTS_CATEGORIES(int _CATEG_ID)
    {
        CATEG_ID = _CATEG_ID;
    }

}
public class SMVTS_OUTSTANDINGAMOUNT : SMVTS_MAIN
{
    public int OUTSTANDING_ID { get; set; }
    public int OUTSTANDING_CUSTOMERID { get; set; }
    public string OUTSTANDING_CUSTOMERNAME { get; set; }
    public DateTime? OUTSTANDING_DATE { get; set; }
    public string OUTSTANDING_AMOUNT { get; set; }
    public string OUTSTANDING_AMOUNTTYPE { get; set; }
    public bool OUTSTANDING_STATUS { get; set; }

    public string OUTSTANDING_CREATEDBY { get; set; }
    public DateTime? OUTSTANDING_CREATEDDATE { get; set; }
    public string OUTSTANDING_MODIFIEDBY { get; set; }
    public DateTime? OUTSTANDING_MODIFIEDDATE { get; set; }
    public string outstanding_totalamount { get; set; }
    public Decimal OUTSTANDING_PRESENTINVOICEAMOUNT { get; set; }
    public string OUTSTANDING_CUSTOMER_LOGINID { get; set; }

    public SMVTS_OUTSTANDINGAMOUNT()
    {
    }
    public SMVTS_OUTSTANDINGAMOUNT(int _OUTSTANDING_ID)
    {
        OUTSTANDING_ID = _OUTSTANDING_ID;
    }

}
public class SMVTS_CLIENTOUTSTANDINGAMOUNT : SMVTS_MAIN
{
    public int CLIENTOUTSTANDING_ID { get; set; }
    public int CLIENTOUTSTANDING_CUSTOMERID { get; set; }
    public string CLIENTOUTSTANDING_TOTALAMOUNT { get; set; }
    public bool CLIENTOUTSTANDING_STATUS { get; set; }
    public DateTime? CLIENTOUTSTANDING_DATE { get; set; }

    public string CLIENTOUTSTANDING_CREATEDBY { get; set; }
    public DateTime? CLIENTOUTSTANDING_CREATEDDATE { get; set; }
    public string CLIENTOUTSTANDING_MODIFIEDBY { get; set; }
    public DateTime? CLIENTOUTSTANDING_MODIFIEDDATE { get; set; }


    public SMVTS_CLIENTOUTSTANDINGAMOUNT()
    {
    }
    public SMVTS_CLIENTOUTSTANDINGAMOUNT(int _CLIENTOUTSTANDING_ID)
    {
        CLIENTOUTSTANDING_ID = _CLIENTOUTSTANDING_ID;
    }

}
public class SMVTS_SALESREPRESENTATIVE : SMVTS_MAIN
{
    //private int p1;
    //private string p2;
    //private string p3;
    //private string p4;
    //private string p5;
    //private string p6;
    //private string p7;
    //private int p8;
    //private int p9;
    //private int p10;
    //private bool p11;



    public int SALESREP_USER_ID { get; set; }
    public string SALESREP_FULLNAME { get; set; }
    public string SALESREP_ADDRESS { get; set; }
    public string SALESREP_REFERREDBY { get; set; }
    public string SALESREP_EMAILID { get; set; }
    public string SALESREP_USERNAME { get; set; }
    public string SALESREP_PASSWORD { get; set; }
    public int SALESREP_COUNTRY { get; set; }
    public int SALESREP_STATE { get; set; }
    public int SALESREP_CITY { get; set; }
    public bool SALESREP_STATUS { get; set; }
    public int SALESREP_CATEGORYID { get; set; }

    public SMVTS_SALESREPRESENTATIVE()
    {

    }
    public SMVTS_SALESREPRESENTATIVE(string _SALESREP_FULLNAME, string _SALESREP_REFERREDBY)
    {
        SALESREP_FULLNAME = _SALESREP_FULLNAME;
        SALESREP_REFERREDBY = _SALESREP_REFERREDBY;
    }
    public SMVTS_SALESREPRESENTATIVE(int _SALESREP_USER_ID, string _SALESREP_FULLNAME, string _SALESREP_ADDRESS,
 string _SALESREP_REFERREDBY, string _SALESREP_EMAILID, string _SALESREP_USERNAME, string _SALESREP_PASSWORD, int _SALESREP_COUNTRY, int _SALESREP_STATE, int _SALESREP_CITY, bool _SALESEREP_STATUS, int _SALESREP_CATEGORYID)
    {
        SALESREP_USER_ID = _SALESREP_USER_ID;
        SALESREP_FULLNAME = _SALESREP_FULLNAME;
        SALESREP_ADDRESS = _SALESREP_ADDRESS;
        SALESREP_REFERREDBY = _SALESREP_REFERREDBY;
        SALESREP_EMAILID = _SALESREP_EMAILID;
        SALESREP_USERNAME = _SALESREP_USERNAME;
        SALESREP_PASSWORD = _SALESREP_PASSWORD;
        SALESREP_COUNTRY = _SALESREP_COUNTRY;
        SALESREP_STATE = _SALESREP_STATE;
        SALESREP_CITY = _SALESREP_CITY;
        SALESREP_STATUS = _SALESEREP_STATUS;
        SALESREP_CATEGORYID = _SALESREP_CATEGORYID;

    }

    public SMVTS_SALESREPRESENTATIVE(int _USER_ID)
    {
        SALESREP_USER_ID = _USER_ID;
    }

    //public SMVTS_SALESREPRESENTATIVE(int p1, string p2, string p3, string p4, string p5, string p6, string p7, int p8, int p9, int p10, bool p11)
    //{
    //    // TODO: Complete member initialization
    //    this.p1 = p1;
    //    this.p2 = p2;
    //    this.p3 = p3;
    //    this.p4 = p4;
    //    this.p5 = p5;
    //    this.p6 = p6;
    //    this.p7 = p7;
    //    this.p8 = p8;
    //    this.p9 = p9;
    //    this.p10 = p10;
    //    this.p11 = p11;
    //}
}
public class SMVTS_REGISTRATIONBRIEFING : SMVTS_MAIN
{

    public int BRIEFING_ID { get; set; }
    public int BRIEFING_DRIVERID { get; set; }
    public int BRIEFING_TRIPID { get; set; }
    public int BRIEFING_CATEGORYID { get; set; }
    public string BRIEFING_TRANSPORTERNAME { get; set; }
    public string BRIEFING_CLEANERPASSPORTID { get; set; }
    public string BRIEFING_CLEANERNAME { get; set; }
    public string BRIEFING_TRIPSTARTPLACE { get; set; }
    public string BRIEFING_TRIPENDPLACE { get; set; }
    public string briefing_vehicles { get; set; }
    public DateTime? BRIEFING_CONTRACRDELIVERYDATE { get; set; }
    public DateTime? BRIEFING_SAVEDATE { get; set; }
    public string BRIEFING_MODEOFBRIEFING { get; set; }
    public string BRIEFING_MAIL { get; set; }
    public int BRIEFING_STATUS { get; set; }
    public string briefing_mobileno { get; set; }
    public int BRIEFING_CREATEDBY { get; set; }
    public DateTime? BRIEFING_CREATEDDATE { get; set; }
    public int BRIEFING_MODIFIEDBY { get; set; }
    public DateTime? BRIEFING_MODIFIEDDATE { get; set; }

    public SMVTS_REGISTRATIONBRIEFING()
    {
    }
    public SMVTS_REGISTRATIONBRIEFING(int _BRIEFING_ID)
    {
        BRIEFING_ID = BRIEFING_ID;
    }

}
public class smvts_drivermedical : SMVTS_MAIN
{

    public int DRIVERMEDICAL_ID { get; set; }
    public int DRIVERMEDICAL_DRIVERID { get; set; }
    public DateTime? DRIVERMEDICAL_TESTDATE { get; set; }
    public string DRIVERMEDICAL_DESCRIPTION { get; set; }

    public bool DRIVERMEDICAL_STATUS { get; set; }

    public int DRIVERMEDICAL_CREATEDBY { get; set; }
    public DateTime? DRIVERMEDICAL_CREATEDDATE { get; set; }
    public int DRIVERMEDICAL_MODIFIEDBY { get; set; }
    public DateTime? DRIVERMEDICAL_MODIFIEDDATE { get; set; }

    public smvts_drivermedical()
    {
    }
    public smvts_drivermedical(int _drivermedical_ID)
    {
        DRIVERMEDICAL_ID = DRIVERMEDICAL_ID;
    }

}
public class SMVTS_DRIVERINCENTIVE : SMVTS_MAIN
{

    public int DRIVERINCENTIVE { get; set; }
    public int DRIVERINCENTIVE_DRIVERID { get; set; }
    public string DRIVERINCENTIVE_FROM { get; set; }
    public string DRIVERINCENTIVE_TO { get; set; }
    public int DRIVERINCENTIVE_TRIPID { get; set; }
    public DateTime? DRIVERINCENTIVE_DATE { get; set; }
    public bool DRIVERINCENTIVE_STATUS { get; set; }

    public int DRIVERINCENTIVE_CREATEDBY { get; set; }
    public DateTime? DRIVERINCENTIVE_CREATEDDATE { get; set; }
    public int DRIVERINCENTIVE_MODIFIEDBY { get; set; }
    public DateTime? DRIVERINCENTIVE_MODIFIEDDATE { get; set; }

    public SMVTS_DRIVERINCENTIVE()
    {
    }
    public SMVTS_DRIVERINCENTIVE(int _DRIVERINCENTIVEID)
    {
        DRIVERINCENTIVE = DRIVERINCENTIVE;
    }

}

public class smvts_driverfatigue : SMVTS_MAIN
{

    public int FATIGUEMETER_ID { get; set; }
    public int FATIGUEMETER_DRIVERID { get; set; }
    public int fatiguemeter_tripid { get; set; }
    public int FATIGUEMETER_LEVELOFLAST7TRIPS { get; set; }

    public int FATIGUEMETER_STATUS { get; set; }

    public int FATIGUEMETER_CREATEDBY { get; set; }
    public DateTime? FATIGUEMETER_CREATEDDATE { get; set; }
    public int FATIGUEMETER_MODIFIEDBY { get; set; }
    public DateTime? FATIGUEMETER_MODIFIEDDATE { get; set; }

    public smvts_driverfatigue()
    {
    }
    public smvts_driverfatigue(int _fatigueid)
    {
        FATIGUEMETER_ID = FATIGUEMETER_ID;
    }

}

public class smvts_incidentsharing : SMVTS_MAIN
{

    public int INCIDENTSHARING_ID { get; set; }
    public int INCIDENTSHARING_DRIVERID { get; set; }
    public int INCIDENTSHARING_TRIPID { get; set; }
    public string INCIDENTSHARING_FROM { get; set; }
    public string INCIDENTSHARING_TO { get; set; }
    public string INCIDENTSHARING_DESC { get; set; }
    public DateTime? INCIDENTSHARING_DATE { get; set; }
    public bool INCIDENTSHARING_STATUS { get; set; }

    public int INCIDENTSHARING_CREATEDBY { get; set; }
    public DateTime? INCIDENTSHARING_CREATEDDATE { get; set; }
    public int INCIDENTSHARING_MODIFIEDBY { get; set; }
    public DateTime? INCIDENTSHARING_MODIFIEDDATE { get; set; }

    public smvts_incidentsharing()
    {
    }
    public smvts_incidentsharing(int _incidentsharingid)
    {
        INCIDENTSHARING_ID = INCIDENTSHARING_ID;
    }

}
public class SMVTS_DEREGISTRATION : SMVTS_MAIN
{

    public int DEREGISTRATION_ID { get; set; }
    public string DEREGISTRATION_REVERSEAUDIT { get; set; }
    public string DEREGISTRATION_REJECTED { get; set; }
    public int DEREGISTRATION_CLEANERPASSPORTID { get; set; }
    public string DEREGISTRATION_CLEANERNAME { get; set; }
    public int DEREGISTRATION_TRIPID { get; set; }
    public int DEREGISTRATION_STATUS { get; set; }

    public int DEREGISTRATION_CREATEDBY { get; set; }
    public DateTime? DEREGISTRATION_CREATEDDATE { get; set; }
    public int DEREGISTRATION_MODIFIEDBY { get; set; }
    public DateTime? DEREGISTRATION_MODIFIEDDATE { get; set; }

    public SMVTS_DEREGISTRATION()
    {
    }
    public SMVTS_DEREGISTRATION(int _DEREGISTRATIONID)
    {
        DEREGISTRATION_ID = DEREGISTRATION_ID;
    }

}
public class SMVTS_DETRIPDETAILS : SMVTS_MAIN
{

    public int TRIP_SNO { get; set; }
    public int TRIP_DRIVERID { get; set; }
    public int TRIP_ID { get; set; }
    public DateTime? TRIP_STARTDATE { get; set; }
    public DateTime? TRIP_ENDDATE { get; set; }
    public char TRIP_TOTALRUNNINGTIME { get; set; }
    public int TRIP_OS { get; set; }
    public int TRIP_RA { get; set; }
    public int TRIP_RD { get; set; }
    public char TRIP_ND { get; set; }
    public int TRIP_STATUS { get; set; }
    public int TRIP_CREATEDBY { get; set; }
    public DateTime? TRIP_CREATEDDATE { get; set; }
    public int TRIP_MODIFIEDBY { get; set; }
    public DateTime? TRIP_MODIFIEDDATE { get; set; }

    public SMVTS_DETRIPDETAILS()
    {
    }
    public SMVTS_DETRIPDETAILS(int _TRIPSNO)
    {
        TRIP_SNO = TRIP_SNO;
    }

}
public class SMVTS_DEBRIEFINGSAVE : SMVTS_MAIN
{

    public int DEBRIEFING_ID { get; set; }
    public int DEBRIEFING_DRIVERID { get; set; }

    public string DEBRIEFING_FROM { get; set; }

    public string DEBRIEFING_TO { get; set; }
    public string DEBRIEFING_GOALADHERENCE { get; set; }
    public string DEBRIEFING_completed { get; set; }


    public int DEBRIEFING_STATUS { get; set; }
    public int DEBRIEFING_CREATEDBY { get; set; }
    public DateTime? DEBRIEFING_CREATEDDATE { get; set; }
    public int DEBRIEFING_MODIFIEDBY { get; set; }
    public DateTime? DEBRIEFING_MODIFIEDDATE { get; set; }
    public SMVTS_DEBRIEFINGSAVE()
    {
    }
    public SMVTS_DEBRIEFINGSAVE(int _DEBRIEFINGSAVEID)
    {
        DEBRIEFING_ID = DEBRIEFING_ID;
    }

}

public class SMVTSBRIEFINGSAVED : SMVTS_MAIN
{

    public int BRIEFINGSAVED_ID { get; set; }
    public int BRIEFINGSAVEDDRIVER_ID { get; set; }

    public string BRIEFINGSAVEDDRIVERINCIDENTSHARING_DESC { get; set; }

    public string BRIEFINGSAVEDDRIVERDMCCOUNCELING_INPUT { get; set; }
    public string BRIEFINGSAVEDDRIVERTRAINER_INPUT { get; set; }
    public string BRIEFINGSAVEDDRIVERGOALCATEGORY { get; set; }
    public string BRIEFINGSAVEDDRIVERGOALOFCONVERSATION { get; set; }

    public int BRIEFINGSAVED_STATUS { get; set; }
    public int BRIEFINGSAVED_CREATEDBY { get; set; }
    public DateTime? BRIEFINGSAVED_CREATEDDATE { get; set; }
    public int BRIEFINGSAVED_MODIFIEDBY { get; set; }
    public DateTime? BRIEFINGSAVED_MODIFIEDDATE { get; set; }
    public SMVTSBRIEFINGSAVED()
    {
    }
    public SMVTSBRIEFINGSAVED(int _BRIEFINGSAVEDID)
    {
        BRIEFINGSAVED_ID = BRIEFINGSAVED_ID;
    }

}

// WRITTEN BY SRIDEVI ON 24/12/2009 12.30 P.M
public class SMVTS_GLOBALSETTINGS : SMVTS_MAIN
{
    public int GBLSET_ID { get; set; }
    public int GBLSET_STOPDURATION { get; set; }
    public int GBLSET_OVERSPEEDDURATION { get; set; }
    public int GBLSET_DATADURATION { get; set; }

    public SMVTS_GLOBALSETTINGS()
    {

    }
    public SMVTS_GLOBALSETTINGS(int _GBLSET_STOPDURATION, int _GBLSET_OVERSPEEDDURATION, int _GBLSET_DATADURATION)
    {
    }
}

// Written by Sridevi On 24/12/2009 5.50 P.M
public class SMVTS_DRIVERS : SMVTS_MAIN
{
    public int DRIVER_ID { get; set; }
    public int DRIVER_CATEGORY_ID { get; set; }
    public string DRIVER_NAME { get; set; }
    public string DRIVER_DESC { get; set; }
    public string DRIVER_MOBILENO { get; set; }
    public string DRIVER_ADDRESS { get; set; }
    public string DRIVER_LICENSENO { get; set; }
    public DateTime? DRIVER_ISSUEDATE { get; set; }
    public DateTime? DRIVER_EXPIRYDATE { get; set; }

    public string DRIVER_LICENSE_ISSUEDATE { get; set; }
    public string DRIVER_LICENSE_EXPIRYDATE { get; set; }

    public bool DRIVER_STATUS { get; set; }

    public string DRIVERSTATUS { get; set; }

    public string DRIVER_BLOODGROUP { get; set; }
    public string DRIVER_LANGUAGES { get; set; }
    public string DRIVER_PHOTO_PATH { get; set; }
    public byte[] DRIVER_PHOTO_DATA { get; set; }
    public string DRIVER_FORMAN { get; set; }
    public string VEHICLE_NUMBER { get; set; }

    public SMVTS_DRIVERS()
    {

    }
    public SMVTS_DRIVERS(int _DRIVER_ID)
    {
        DRIVER_ID = _DRIVER_ID;
    }
}
public class SMVTS_MAINTENANCE : SMVTS_MAIN
{
    public int MAINTENANCE_ID { get; set; }
    public int MAINTENANCE_USER_ID { get; set; }
    public string MAINTENANCE_VEHREGNUMBER { get; set; }
    public string MAINTENANCE_ENGINE_NO { get; set; }
    public string MAINTENANCE_CHASSIS_NO { get; set; }
    public DateTime MAINTENANCE_FITNESS_CERTIFICATE { get; set; }
    public float MAINTENANCE_ENGINE_OIL { get; set; }
    public float MAINTENANCE_GEARBOXOIL { get; set; }
    public float MAINTENANCE_WHEELBALANCING { get; set; }
    public DateTime? MAINTENANCE_GENERALSERVICING { get; set; }
    public float MAINTENANCE_TYREREPLACEMENT { get; set; }
    public float MAINTENANCE_POWERSTEERINGOIL { get; set; }
    public DateTime? MAINTENANCE_BREAK_FLUID { get; set; }
    public DateTime? MAINTENANCE_BREAK_LINERS { get; set; }
    public float MAINTENANCE_COOLANT { get; set; }
    public float MAINTENANCE_CATEGORY_ID { get; set; }

    public double MAINTENANCE_AIR_FILTER_INNER { get; set; }
    public double MAINTENANCE_AIR_FILTER_OUTER { get; set; }
    public double MAINTENANCE_ALTINATOR_OVERHAULING { get; set; }
    public double MAINTENANCE_BELL_CRANK_BUSHING { get; set; }
    public double MAINTENANCE_CLUTCH_KIT_CHANG { get; set; }
    public double MAINTENANCE_DIESEL_TANK_SERVICING { get; set; }
    public double MAINTENANCE_CROWAN_OIL_CHANGE { get; set; }
    public double MAINTENANCE_ENGINE_MOUNTING_CHECK { get; set; }
    public double MAINTENANCE_FANBELT_TENTION_CHECK { get; set; }
    public double MAINTENANCE_FRONT_GREASING { get; set; }
    public double MAINTENANCE_REAR_GREASING { get; set; }
    public double MAINTENANCE_FUELPUMP_SERVICING { get; set; }
    public double MAINTENANCE_SHAFT_OVERHAULING { get; set; }
    public double MAINTENANCE_RADIATOR_SERVICING { get; set; }

    public double MAINTENANCE_SELF_OVERHAULING { get; set; }
    public double MAINTENANCE_BEAMAXEL_REPAIR { get; set; }
    public double MAINTENANCE_STEARING_OIL_FILTER { get; set; }
    public double MAINTENANCE_BATTERY { get; set; }


    public DateTime? MAINTENANCERENEWAL_ENGINE_OIL_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_GEARBOXOIL_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_WHEELBALANCING_PREVDONE { get; set; }

    public DateTime? MAINTENANCERENEWAL_TYREREPLACEMENT_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_POWERSTEERINGOIL_PREVDONE { get; set; }

    public DateTime? MAINTENANCERENEWAL_COOLANT_PREVDONE { get; set; }


    public DateTime? MAINTENANCERENEWAL_AIR_FILTER_INNER_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_AIR_FILTER_OUTER_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_ALTINATOR_OVERHAULING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_BELL_CRANK_BUSHING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_CLUTCH_KIT_CHANG_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_DIESEL_TANK_SERVICING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_CROWAN_OIL_CHANGE_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_ENGINE_MOUNTING_CHECK_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_FANBELT_TENTION_CHECK_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_FRONT_GREASING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_REAR_GREASING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_FUELPUMP_SERVICING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_SHAFT_OVERHAULING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_RADIATOR_SERVICING_PREVDONE { get; set; }

    public DateTime? MAINTENANCERENEWAL_SELF_OVERHAULING_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_BEAMAXEL_REPAIR_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_STEARING_OIL_FILTER_PREVDONE { get; set; }
    public DateTime? MAINTENANCERENEWAL_BATTERY_PREVDONE { get; set; }


    public string FORM_TYPE { get; set; }

    public SMVTS_MAINTENANCE()
    {

    }

}
public class SMVTS_DISTRIBUTER : SMVTS_MAIN
{
    public int DISTRIBUTER_ID { get; set; }
    public string DISTRIBUTER_FULLNAME { get; set; }
    public string DISTRIBUTER_USERNAME { get; set; }
    public string DISTRIBUTER_EMAILID { get; set; }
    public string DISTRIBUTER_COMPANYNAME { get; set; }
    public string DISTRIBUTER_Legalstatusofyourfirm { get; set; }
    public string DISTRIBUTER_Totalexperienceinbusiness { get; set; }
    public string DISTRIBUTER_Doyouhaveanexperienceinrunningafranchiseebusiness { get; set; }
    public string DISTRIBUTER_Ifyeswhichindustry { get; set; }
    public string DISTRIBUTER_INVESTMENTRANGE { get; set; }
    public string DISTRIBUTER_WEBSITE { get; set; }
    public string DISTRIBUTER_ADDRESS { get; set; }
    public int DISTRIBUTER_COUNTRYID { get; set; }
    public int DISTRIBUTER_STATEID { get; set; }
    public int DISTRIBUTER_CITYID { get; set; }
    public string DISTRIBUTER_ZIPCODE { get; set; }
    public string DISTRIBUTER_MOBILENO { get; set; }
    public string DISTRIBUTER_ALTERNATEMOBILENO { get; set; }
    public string DISTRIBUTER_DESC { get; set; }
    public bool DISTRIBUTER_STATUS { get; set; }
    public int DISTRIBUTER_CREATEDBY { get; set; }
    public DateTime? DISTRIBUTER_CREATEDDATE { get; set; }
    public int DISTRIBUTER_MODIFIEDBY { get; set; }
    public DateTime? DISTRIBUTER_MODIFIEDDATE { get; set; }

    public SMVTS_DISTRIBUTER()
    {

    }
    public SMVTS_DISTRIBUTER(int _DISTRIBUTER_ID)
    {
        DISTRIBUTER_ID = _DISTRIBUTER_ID;
    }
}

// Written by Sridevi On 25/12/2009 12.00 P.M
public class SMVTS_SIMS : SMVTS_MAIN
{
    public int SIM_ID { get; set; }
    public int SIM_CATEGORY_ID { get; set; }
    public string SIM_OPERATORNAME { get; set; }
    public string SIM_SERIALNO { get; set; }
    public string SIM_NUMBER { get; set; }
    public string SIM_APNWEBSITE { get; set; }
    public string SIM_APNIP { get; set; }
    public int SIM_COUNTRY_ID { get; set; }
    public int SIM_STATE_ID { get; set; }
    public bool SIM_STATUS { get; set; }
    public string STATUS { get; set; }
    public int SIM_CATEGORY_TYPE_ID { get; set; }
    public SMVTS_SIMS()
    {

    }
    public SMVTS_SIMS(int _SIM_ID)
    {
        SIM_ID = _SIM_ID;
    }
}
//written by nitisha 17/11/16 3:08 p.m
public class PIPL_SIMS : SMVTS_MAIN
{
    public int SIMID { get; set; }
    public string SIM_MNO { get; set; }
    public string SIM_SNO { get; set; }
    public string SIM_PROVIDER { get; set; }
    public bool SIM_STATUS { get; set; }
    public DateTime? SIM_ISSUEDATE { get; set; }
    public int SIM_ASSIGNED_STATUS { get; set; }
    public int SIM_CATEGORY_ID { get; set; }

    public PIPL_SIMS()
    {

    }
    public PIPL_SIMS(int _SIM_ID)
    {
        SIMID = SIMID;
    }
}
public class PVTS_SIMS : SMVTS_MAIN
{
    public int PSIM_ID { get; set; }
    public string PSIM_OPERATORNAME { get; set; }
    public string PSIM_SERIALNO { get; set; }
    public string PSIM_NUMBER { get; set; }
    public bool PSIM_STATUS { get; set; }

    public PVTS_SIMS()
    {

    }
    public PVTS_SIMS(int _PSIM_ID)
    {
        PSIM_ID = _PSIM_ID;
    }
}
public class SMVTS_TYRE : SMVTS_MAIN
{
    public int TYRE_ID { get; set; }
    public string TYRE_VEHREGNUMBER { get; set; }
    public string TYRE_NUMBER { get; set; }
    public string TYRE_MAKE { get; set; }
    public string TYRE_GRADE { get; set; }
    public string TYRE_PACEID { get; set; }
    public string TYRE_ISSUEDATE { get; set; }
    public double TYRE_ONKM { get; set; }
    public double TYRE_RUNKM { get; set; }
    public int TYRE_CATEGORY_ID { get; set; }
    public int TYRE_STATUS { get; set; }


    public SMVTS_TYRE()
    {

    }

}
// Written by Sridevi On 25/12/2009 12.00 P.M
public class SMVTS_VEHLEMM : SMVTS_MAIN
{
    public int VEHLEMM_ID { get; set; }
    public int VEHLEMM_CATEGORY_ID { get; set; }
    public string VEHLEMM_NAME { get; set; }
    public string VEHLEMM_DESC { get; set; }
    public string VEHLEMM_MAKE { get; set; }
    public string VEHLEMM_MODEL { get; set; }
    public string VEHLEMM_YEAR { get; set; }
    public int VEHLEMM_NOOFTANKS { get; set; }
    public string VEHLEMM_TANKCAPACITY { get; set; }
    public string VEHLEMM_MEASURINGUNIT { get; set; }
    public string VEHICLE_TYPE { get; set; }

    public string VEHICLE_REGNUMBER { get; set; }
    public SMVTS_VEHLEMM()
    {

    }
    public SMVTS_VEHLEMM(int _VEHLEMM_ID)
    {
        VEHLEMM_ID = _VEHLEMM_ID;
    }
}
public class SMVTS_BRIEFING : SMVTS_MAIN
{
    public int BRIEFING_ID { get; set; }
    public int BRIEFING_DRIVERID { get; set; }
    public int BRIEFING_TRIPID { get; set; }
    public int BRIEFING_CATEGORYID { get; set; }
    public string BRIEFING_TRANSPORTERNAME { get; set; }
    public string BRIEFING_CLEANERPASSPORTID { get; set; }
    public string BRIEFING_CLEANERNAME { get; set; }
    public string BRIEFING_TRIPSTARTPLACE { get; set; }
    public string BRIEFING_TRIPENDPLACE { get; set; }
    public string briefing_vehicles { get; set; }
    public DateTime? BRIEFING_CONTRACRDELIVERYDATE { get; set; }
    public DateTime? BRIEFING_SAVEDATE { get; set; }
    public string BRIEFING_MODEOFBRIEFING { get; set; }
    public string BRIEFING_MAIL { get; set; }
    public int BRIEFING_STATUS { get; set; }
    public string briefing_mobileno { get; set; }
    public int BRIEFING_CREATEDBY { get; set; }
    public DateTime? BRIEFING_CREATEDDATE { get; set; }
    public int BRIEFING_MODIFIEDBY { get; set; }
    public DateTime? BRIEFING_MODIFIEDDATE { get; set; }

    public SMVTS_BRIEFING()
    {
    }
    public SMVTS_BRIEFING(int _BRIEFING_ID)
    {
        BRIEFING_ID = BRIEFING_ID;
    }

}
public class SMVTS_DRIVERHISTORY : SMVTS_MAIN
{
    public int DRIVERHISTORY_ID { get; set; }
    public int DRIVERHISTORY_DRIVERID { get; set; }
    public string DRIVERHISTORY_VEHICLENO { get; set; }
    public string DRIVERHISTORY_FROM { get; set; }
    public string DRIVERHISTORY_TO { get; set; }
    public string DRIVERHISTORY_TRIPID { get; set; }
    public DateTime? DRIVERHISTORY_TRIPSTARTDATE { get; set; }
    public DateTime? DRIVERHISTORY_TRIPENDDATE { get; set; }
    public char DRIVERHISTORY_TOTALRUNNINGTIME { get; set; }
    public char DRIVERHISTORY_NIGHTSTOPTIME { get; set; }
    public char DRIVERHISTORY_DAYSTOPTIME { get; set; }
    public decimal DRIVERHISTORY_ACTUALKM { get; set; }
    public char DRIVERHISTORY_TOTALSTOPTIME { get; set; }
    public int DRIVERHISTORY_OS { get; set; }
    public int DRIVERHISTORY_RA { get; set; }
    public int DRIVERHISTORY_RD { get; set; }
    public char DRIVERHISTORY_ND { get; set; }
    public int DRIVERHISTORY_STATUS { get; set; }

    public SMVTS_DRIVERHISTORY()
    {
    }
    public SMVTS_DRIVERHISTORY(int _DRIVERHISTORY_ID)
    {
        DRIVERHISTORY_ID = DRIVERHISTORY_ID;
    }
}
public class SMVTS_DMCCOUNCELING : SMVTS_MAIN
{
    public int DMCCOUNCELING_ID { get; set; }
    public int DMCCOUNCELING_DRIVERID { get; set; }
    public string DMCCOUNCELINGINPUT_DESC { get; set; }
    public int DMCCOUNCELINGSTATUS { get; set; }
    public int DMCCOUNCELINGCREATEDBY { get; set; }
    public DateTime? DMCCOUNCELINGCREATEDDATE { get; set; }
    public int DMCCOUNCELINGMODIFIEDBY { get; set; }
    public DateTime? DMCCOUNCELINGMODIFIEDDATE { get; set; }


    public SMVTS_DMCCOUNCELING()
    {
    }
    public SMVTS_DMCCOUNCELING(int _DMCCOUNCELING_ID)
    {
        DMCCOUNCELING_ID = DMCCOUNCELING_ID;
    }

}

public class smvts_drivertraining : SMVTS_MAIN
{
    public int DRIVERTRAINING_ID { get; set; }
    public int DRIVERTRAINING_DRIVERID { get; set; }
    public DateTime? DRIVERTRANING_DATE { get; set; }
    public string DRIVERTRAINING_DETAILS { get; set; }
    public bool DRIVERTRAINING_STATUS { get; set; }
    public int DRIVERTRAINING_CREATEDBY { get; set; }
    public DateTime? DRIVERTRAINING_CREATEDDATE { get; set; }
    public int DRIVERTRAINING_MODIFIEDBY { get; set; }
    public DateTime? DRIVERTRAINING_MODIFIEDDATE { get; set; }
    public smvts_drivertraining()
    {
    }
    public smvts_drivertraining(int _drivertraining_id)
    {

        DRIVERTRAINING_ID = DRIVERTRAINING_ID;
    }
}

public class smvts_cautionmessage : SMVTS_MAIN
{
    public int CAUTIONMESSAGE_ID { get; set; }
    public int CAUTIONMESSAGE_DRIVERID { get; set; }
    public string CAUTIONMESSAGE_TRAININGDESC { get; set; }
    public string CAUTIONMESSAGE_MEDICALDESC { get; set; }
    public int CAUTIONMESSAGE_LICENSE_DUE { get; set; }
    public bool CAUTIONMESSAGE_STATUS { get; set; }
    public int DRIVERTRAINING_CREATEDBY { get; set; }
    public DateTime? DRIVERTRAINING_CREATEDDATE { get; set; }
    public int DRIVERTRAINING_MODIFIEDBY { get; set; }
    public DateTime? DRIVERTRAINING_MODIFIEDDATE { get; set; }
    public smvts_cautionmessage()
    {
    }
    public smvts_cautionmessage(int _cautionmessage_id)
    {

        CAUTIONMESSAGE_ID = CAUTIONMESSAGE_ID;
    }
}


public class SMVTS_PARTYNAMES : SMVTS_MAIN
{
    public int PARTY_ID { get; set; }
    public string PARTY_NAME { get; set; }
    public int PARTY_CATEGID { get; set; }
    public DateTime? PARTY_INSERTEDDATE { get; set; }
    public string PARTY_LOCATION { get; set; }
    public DateTime? PARTYCONTRACT_STARTDATE { get; set; }
    public DateTime? PARTYCONTRACT_ENDDATE { get; set; }
    public bool? PARTY_STATUS { get; set; }

    public SMVTS_PARTYNAMES()
    {
    }

    public SMVTS_PARTYNAMES(int _PARTY_ID)
    {
        PARTY_ID = _PARTY_ID;
    }

}

public class SMVTS_ER_TRIPINFO : SMVTS_MAIN
{
    public int ER_ID { get; set; }
    public string ER_VEHICLENO { get; set; }
    public string ER_VEHICLETYPE { get; set; }
    public string ER_BOOKBRANCH { get; set; }
    public string ER_BOOKZONE { get; set; }
    public string ER_DISPATCHDATE { get; set; }
    public string ER_FROM { get; set; }
    public string ER_TO { get; set; }
    public string ER_PARTYNAME { get; set; }
    public int ER_CATEGID { get; set; }

    public SMVTS_ER_TRIPINFO()
    {
    }

    public SMVTS_ER_TRIPINFO(int _er_ID)
    {
        ER_ID = _er_ID;
    }
}

public class SMVTS_DEVICES : SMVTS_MAIN
{
    public int DEVICE_ID { get; set; }
    public string DEVICE_NAME { get; set; }
    public string DEVICE_SERIALNUMBER { get; set; }
    public DateTime? DEVICE_MFGDATE { get; set; }
    public int DEVICE_CATEGORY_ID { get; set; }
    public int DEVICE_COUNTRY_ID { get; set; }
    public DateTime? DEVICE_DATEOFSALE { get; set; }




    public int? DEVICE_SIM_ID { get; set; }
    public string DEVICE_CALLNUMBER1 { get; set; }
    public string DEVICE_CALLNUMBER2 { get; set; }
    public int? DEVICE_STOPDURATION { get; set; }
    public int? DEVICE_OVERSPEEDDURATION { get; set; }
    public int? DEVICE_DATADURATION { get; set; }

    public string DEVICE_STATUS { get; set; }
    public string CLIENT_NAME { get; set; }
    public string SIM_NUMBER { get; set; }

    public string DEVICE_STOPDURATION_ONE { get; set; }
    public string DEVICE_OVERSPEEDDURATION_ONE { get; set; }
    public string DEVICE_DATADURATION_ONE { get; set; }
    public string DEVICE_DATEOFSALE_ONE { get; set; }
    public string DEVICE_MFGDATE_ONE { get; set; }
    public SMVTS_DEVICES()
    {
    }

    public SMVTS_DEVICES(int _DEVICE_ID)
    {
        DEVICE_ID = _DEVICE_ID;
    }

}

public class SMVTS_FLEET_ALERT : SMVTS_MAIN
{
    public int ALERT_ID { get; set; }
    public string ALERT_VEHREGNUMBER { get; set; }
    public string ALERT_ENGINENO { get; set; }
    public string ALERT_CHASSISNO { get; set; }
    public DateTime ALERT_INS_DT { get; set; }
    public DateTime ALERT_ISP_DT { get; set; }
    public DateTime ALERT_NP_DT { get; set; }
    public DateTime ALERT_PC_DT { get; set; }
    public float ALERT_EOIL { get; set; }
    public float ALERT_GOIL { get; set; }
    public float ALERT_WBL { get; set; }
    public DateTime ALERT_GS { get; set; }
    public float ALERT_TR { get; set; }
    public float ALERT_PSO { get; set; }
    public float ALERT_COOLANT { get; set; }
    public DateTime ALERT_BF { get; set; }
    public DateTime ALERT_BL { get; set; }
    public int ALERT_CATEGORY_ID { get; set; }
    public SMVTS_FLEET_ALERT()
    {
    }

}

// Written by Sridevi On 26/12/2009 1.00 P.M
public class SMVTS_VEHICLES : SMVTS_MAIN
{
    public int VEHICLES_ID { get; set; }
    public string VEHICLES_REGNUMBER { get; set; }
    public int VEHICLES_VEHICLEMAKEMODEL_ID { get; set; }
    public int VEHICLES_CATEGORY_ID { get; set; }
    public string VEHICLES_MAXSPEED { get; set; }
    public string VEHICLES_MILEAGE { get; set; }
    public string VEHICLES_OPENINGODOMETER { get; set; }
    public string VEHICLES_CURRENTODOMETER { get; set; }
    public int VEHICLES_DEVICE_ID { get; set; }
    public int VEHICLES_DRIVER_ID { get; set; }
    public string VEHICLES_RESERVEVOLUME { get; set; }
    public string VEHICLES_TANKCAPACITY { get; set; }
    public bool VEHICLES_STATUS { get; set; }
    public string VEHICLES_ENGINE_NUMBER { get; set; }
    public string VEHICLES_CHASSIS_NO { get; set; }
    public string VEHICLES_EC { get; set; }
    public string VEHICLES_AXLETYPE { get; set; }
    public string VEHICLES_COLOR { get; set; }
    public string VEHICLES_VARIANT { get; set; }
    public string VEHICLES_MANUFACTURER { get; set; }
    public string VEHICLES_YOM { get; set; }
    public string VEHICLES_EN { get; set; }
    public string VEHICLES_FUEL { get; set; }
    public string VEHICLES_FTC { get; set; }
    public string VEHICLES_SC { get; set; }
    public string VEHICLES_INS { get; set; }
    public string VEHICLES_ISP { get; set; }
    public string VEHICLES_ISP_DT { get; set; }
    public string VEHICLES_NP { get; set; }
    public string VEHICLES_NP_DT { get; set; }
    public string VEHICLES_FC_DT { get; set; }
    public string VEHICLES_HT_DT { get; set; }
    public string VEHICLES_PC { get; set; }
    public string VEHICLES_FORMTYPE { get; set; }
    public string VEHICLES_TYPE { get; set; }
    public int USERID { get; set; }
    public double VEHICLES_AVGSPEED { get; set; }
    public string VEHICLES_TRAVELDISTINDAY { get; set; }
    public string VEHICLES_CAPACITY { get; set; }
    public int VEHICLES_VENDORMASTER_ID { get; set; }
    public int VEHICLES_OFFICEMASTER_ID { get; set; }
    public int VEHICLES_SEATINGCAPACITY { get; set; }
    public string vehicles_groupzone { get; set; }
    public string TripTime { get; set; }
    public string TripDate { get; set; }

    public SMVTS_VEHICLES()
    {

    }
    public SMVTS_VEHICLES(int _VEHICLES_ID)
    {
        VEHICLES_ID = _VEHICLES_ID;
    }
}
// Written by Sridevi On 28/12/2009 11.00 A.M
//Modified by Sirkanth on 21/12/2009 1:00 PM
public class SMVTS_LANDMARKS : SMVTS_MAIN
{
    public int LANDMARKS_ID { get; set; }
    public int LANDMARKS_CATEGORY_ID { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_LATITUDE { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_LONGITUDE { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_ADDRESS { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_CONTPERSONS { get; set; }
    public string LANDMARKS_MOBILENUMBER { get; set; }
    public int LANDMARKS_TYPE { get; set; }
    public int? LANDMARKS_LOCATIONTYPE { get; set; }
    public bool LANDMARKS_STATUS { get; set; }
    public bool LANDMARKS_GEOSTATUS { get; set; }
    public double RADIUS { get; set; }
    public int LANDMARKS_GEOFENCETYPE { get; set; }
    public string LANDMARKS_POLYPOINTS { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_STATE { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_ZONE { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LANDMARKS_NEARCITY { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LOCATIONTYPE_NAME { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string LOCATION_STATUS { get; set; }
    public string GEOFENCE_STATUS { get; set; }
    public int? LANDMARK_GEOFENCE_ID { get; set; }

    public SMVTS_LANDMARKS()
    {

    }
    public SMVTS_LANDMARKS(int _LANDMARKS_ID)
    {
        LANDMARKS_ID = _LANDMARKS_ID;
    }



    public string IN_DATE { get; set; }

    public string OUT_DATE { get; set; }

    public string DURATION { get; set; }

    public string LOCATIONTYPE { get; set; }

    public string VEHICLE_NUMBER { get; set; }

    public string TRIPCOUNT { get; set; }

}
// Written by Sridevi On 28/12/2009 11.00 A.M
public class SMVTS_LOCATIONTYPE : SMVTS_MAIN
{
    public int LOCTYPE_ID { get; set; }
    public string LOCTYPE_NAME { get; set; }
    public string LOCTYPE_DESC { get; set; }
    public string LOCTYPE_IMAGE { get; set; }

    public SMVTS_LOCATIONTYPE()
    {

    }
    public SMVTS_LOCATIONTYPE(int _LOCTYPE_ID)
    {
        LOCTYPE_ID = _LOCTYPE_ID;
    }
}

public class SMVTS_USERROLES : SMVTS_MAIN
{
    public int USERROLE_ID { get; set; }
    public int USERROLE_CATEGORYTYPE_ID { get; set; }
    public string USERROLE_NAME { get; set; }
    public string USERROLE_DESC { get; set; }
    public int USERROLE_TYPE { get; set; }
    public bool USERROLE_STATUS { get; set; }

    public SMVTS_USERROLES()
    {
    }

    public SMVTS_USERROLES(int _USERROLE_ID)
    {
        USERROLE_ID = _USERROLE_ID;
    }
}

// Written by Sridevi On 29/12/2009 4.20 P.M
public class SMVTS_ROUTES : SMVTS_MAIN
{
    public int ROUTES_ID { get; set; }
    public int ROUTES_CATEGORY_ID { get; set; }
    public string ROUTES_NAME { get; set; }
    public string ROUTES_STARTLAT { get; set; }
    public string ROUTES_STARTLONG { get; set; }
    public string ROUTES_ENDLAT { get; set; }
    public string ROUTES_ENDLONG { get; set; }
    public string ROUTES_POINTS { get; set; }
    public bool ROUTES_STATUS { get; set; }
    public string ROUTES_FROM { get; set; }
    public string ROUTES_TO { get; set; }
    public string ROUTES_VIA { get; set; }
    public string ROUTES_DEVIATION_LIMIT { get; set; }
    public string ROUTES_DISTANCE { get; set; }
    public string ROUTES_INTERMEDIATEPOINTS { get; set; }
    public string STATUS { get; set; }
    public SMVTS_ROUTES()
    {

    }
    public SMVTS_ROUTES(int _ROUTES_ID)
    {
        ROUTES_ID = _ROUTES_ID;
    }
}

public class SMVTS_ERPL_MASTERROUTES : SMVTS_MAIN
{
    public int ERP_ROUTEID { get; set; }
    public string ERP_ROUTEFROM { get; set; }
    public string ERP_ROUTETO { get; set; }
    public int ERP_ROUTECATEGID { get; set; }
    public bool ERP_ROUTESTATUS { get; set; }
}

// Written by Sridevi On 30/12/2009 11.30 A.M

public class SMVTS_DGTUPLOAD : SMVTS_MAIN
{

    public string VEHICLENO { get; set; }
    public string DRIVERNAME_CODE { get; set; }
    public string DRIVER_PHONENO { get; set; }
    public string FORMAN { get; set; }
    public string FROM_LOC { get; set; }
    public string TO_LOC { get; set; }
    public string NATURE_OF_GOODS { get; set; }
    public string REACHED_DAY { get; set; }
    public string LOADING_DATE { get; set; }
    public string TRANSIT_TIME { get; set; }
    public string ZONE { get; set; }
    public string id { get; set; }
    public string EXP_DATE_OF_DELIVERY { get; set; }
    public string TOTAL_TRIP { get; set; }
    public string REPORTING_DATE { get; set; }
    public string STATUS_OF_PLACE_ON_DATE { get; set; }
    public string UNLOADING_DATE { get; set; }
    public string REMARK { get; set; }
    public string DELAY_DAYS { get; set; }
    public string UPLOADED_DATE { get; set; }


}
public class SMVTS_FUELCALIBRATION : SMVTS_MAIN
{
    public int FUEL_ID { get; set; }
    public int FUEL_DEVICEID { get; set; }
    public float FUEL_VOLTAGE { get; set; }
    public int FUEL_PERCENTAGE { get; set; }
    public int FUEL_STATUS { get; set; }
    public string FUEL_CREATEDBY { get; set; }
    public DateTime FUEL_CREATEDDATE { get; set; }
    public string FUEL_MODIFIEDBY { get; set; }
    public DateTime FUEL_MODIFIEDDATE { get; set; }
    public string FUEL_CATEGORYID { get; set; }

}
public class SMVTS_ROUTEPLAN : SMVTS_MAIN
{
    public int VEHROUTE_ID { get; set; }
    public int VEHROUTE_CATEGORY_ID { get; set; }
    public int VEHROUTE_DEVICE_ID { get; set; }
    public int VEHROUTE_ROUTE_ID { get; set; }
    public DateTime? VEHROUTE_STARTDATE { get; set; }
    public DateTime? VEHROUTE_ENDDATE { get; set; }
    public DateTime? VEHROUTE_COMPLETEDDATE { get; set; }
    public int VEHROUTE_STATUS { get; set; }
    public double VEHROUTE_AVGSPEED { get; set; }
    public double VEHROUTE_TRAVELDISTINDAY { get; set; }
    public double VEHROUTE_DISTANCEROUTE { get; set; }

    public int CONSIGNMENT_ID { get; set; }
    public string CONSIGNMENT_TYPE { get; set; }
    public string CONSIGNMENT_DETAILS { get; set; }
    public string CONSIGNMENT_FROM { get; set; }
    public string CONSIGNMENT_CONSIGNEE_DETAILS { get; set; }
    public string CONSIGNMENT_TO { get; set; }
    public int CONSIGNMENT_CONSID { get; set; }
    public int CONSIGNMENT_MODELID { get; set; }
    public int CONSIGNMENT_VARIANTID { get; set; }
    public string CONSIGNMENT_QUANTITY { get; set; }
    public int CONSIGNMENT_DEALER { get; set; }

    public string CONSIGNMENT_REACHDAY_INSOUREC { get; set; }
    public string CONSIGNMENT_LOADINGDATE { get; set; }
    public string CONSIGNMENT_EXPDATEOFDELIVERY { get; set; }
    public string CONSIGNMENT_REPORTINGDATE { get; set; }
    public string CONSIGNMENT_UNLOADINGDATE { get; set; }
    public string CONSIGNMENT_REMARK { get; set; }

    public string CONSIGNMENT_FORMAN { get; set; }
    public string CONSIGNMENT_DRIVER_NUMBER { get; set; }
    public string CONSIGNMENT_ZONE { get; set; }
    public decimal VEHROUTE_LOAD { get; set; }
    public string VEHROUTE_TRIPNO { get; set; }
    public string VEHROUTE_REMARKS { get; set; }
    public int VEHROUTE_TOTAL_ALLOWED_DAYS { get; set; }
    public string VEHROUTE_LAST_TRIP_COMPLETION_DATE { get; set; }
    public string VEHICLES_REGNUMBER { get; set; }
    public string ROUTE_NAME { get; set; }

    public SMVTS_ROUTEPLAN()
    {

    }
    public SMVTS_ROUTEPLAN(int _VEHROUTE_ID)
    {
        VEHROUTE_ID = _VEHROUTE_ID;
    }
}
// Written by Sridevi On 04/01/2010 11.00 A.M







public class SMVTS_ROUTEPLAN1 : SMVTS_MAIN
{
    public int VEHROUTE_ID { get; set; }
    public int VEHROUTE_CATEGORY_ID { get; set; }
    public int VEHROUTE_DEVICE_ID { get; set; }
    public int VEHROUTE_ROUTE_ID { get; set; }
    public string VEHROUTE_STARTDATE { get; set; }
    public string VEHROUTE_ENDDATE { get; set; }
    public int VEHROUTE_STATUS { get; set; }
    public double VEHROUTE_AVGSPEED { get; set; }
    public double VEHROUTE_TRAVELDISTINDAY { get; set; }
    public double VEHROUTE_DISTANCEROUTE { get; set; }

    public int CONSIGNMENT_ID { get; set; }
    public string CONSIGNMENT_TYPE { get; set; }
    public string CONSIGNMENT_DETAILS { get; set; }
    public string CONSIGNMENT_FROM { get; set; }
    public string CONSIGNMENT_CONSIGNEE_DETAILS { get; set; }
    public string CONSIGNMENT_TO { get; set; }
    public int CONSIGNMENT_CONSID { get; set; }
    public int CONSIGNMENT_MODELID { get; set; }
    public int CONSIGNMENT_VARIANTID { get; set; }
    public string CONSIGNMENT_QUANTITY { get; set; }
    public int CONSIGNMENT_DEALER { get; set; }

    public DateTime? CONSIGNMENT_REACHDAY_INSOUREC { get; set; }
    public DateTime? CONSIGNMENT_LOADINGDATE { get; set; }
    public DateTime? CONSIGNMENT_EXPDATEOFDELIVERY { get; set; }
    public DateTime? CONSIGNMENT_REPORTINGDATE { get; set; }
    public DateTime? CONSIGNMENT_UNLOADINGDATE { get; set; }
    public string CONSIGNMENT_REMARK { get; set; }

    public SMVTS_ROUTEPLAN1()
    {

    }
    public SMVTS_ROUTEPLAN1(int _VEHROUTE_ID)
    {
        VEHROUTE_ID = _VEHROUTE_ID;
    }
}






















public class SMVTS_CONTRACTS : SMVTS_MAIN
{
    public int CONTRACTS_ID { get; set; }
    public int CONTRACTS_CATEGORY_ID { get; set; }
    public string CONTRACTS_NAME { get; set; }
    public string CONTRACTS_DESC { get; set; }
    public DateTime? CONTRACTS_STARTDATE { get; set; }
    public DateTime? CONTRACTS_ENDDATE { get; set; }
    public bool CONTRACTS_STATUS { get; set; }
    public string QUERY { get; set; }

    public SMVTS_CONTRACTS()
    {

    }
    public SMVTS_CONTRACTS(int _CONTRACTS_ID)
    {
        CONTRACTS_ID = _CONTRACTS_ID;
    }
}
// Written by Sridevi On 04/01/2010 11.10 A.M
public class SMVTS_SERVICECONTRACTS : SMVTS_MAIN
{
    public int SERVCONT_ID { get; set; }
    public int SERVCONT_SERVICES_ID { get; set; }
    public int SERVCONT_CONTRACTS_ID { get; set; }
    public DateTime? SERVCONT_STARTDATE { get; set; }
    public DateTime? SERVCONT_ENDDATE { get; set; }
    public bool SERVCONT_STATUS { get; set; }
    public string SERVCONT_CONTNAME { get; set; }


    public SMVTS_SERVICECONTRACTS()
    {

    }
    public SMVTS_SERVICECONTRACTS(int _SERVCONT_ID)
    {
        SERVCONT_ID = _SERVCONT_ID;
    }
}
public class SMVTS_FLEETCONFIG : SMVTS_MAIN
{
    public int ID { get; set; }
    public string REGNUMBER { get; set; }
    public string ALERT_TYPE { get; set; }
    public string SIM { get; set; }
    public int CATEGORYID { get; set; }
    public int SMS_STATUS { get; set; }
    public SMVTS_FLEETCONFIG()
    {

    }


}

public class SMVTS_FLEETCONFIG1 : SMVTS_MAIN
{
    public int FLEETCONFIG_ID { get; set; }
    public int FLEETCONFIG_CATEG_ID { get; set; }
    public string FLEETCONFIG_VEHICLENO { get; set; }
    public string FLEETCONFIG_GEOFENCE { get; set; }
    public string FLEETCONFIG_SPEEDVIOLATION { get; set; }
    public string FLEETCONFIG_STOPPAGE { get; set; }
    public string FLEETCONFIG_ENGINEOIL { get; set; }
    public string FLEETCONFIG_GEARBOXOIL { get; set; }
    public string FLEETCONFIG_INSURANCE { get; set; }
    public string FLEETCONFIG_INTERSTATEPERMIT { get; set; }
    public string FLEETCONFIG_NATIONALPERMIT { get; set; }
    public string FLEETCONFIG_POLLUTIONCONTROL { get; set; }
    public string FLEETCONFIG_TYREREPLACEMENT { get; set; }
    public string FLEETCONFIG_WHEELBALANCING { get; set; }
    public int FLEETCONFIG_CREATEDBY { get; set; }
    public string FLEETCONFIG_CREATEDDATE { get; set; }
    public int FLEETCONFIG_MODIFIEDBY { get; set; }
    public string FLEETCONFIG_MODIFIEDDATE { get; set; }

    public SMVTS_FLEETCONFIG1()
    {
    }

}

public class SMVTS_FUELTYPE : SMVTS_SIMS
{
    public int TYPEID { get; set; }
    public string TYPENAME { get; set; }


    public string Location { get; set; }
    public string Odometer { get; set; }
    public string Refildate { get; set; }
    public string Refil_fuel { get; set; }
    public string Refil_value { get; set; }
    public string Vehicleno { get; set; }
    public string STARTLEVEL { get; set; }
    public string ENDLEVEL { get; set; }
    public string THEFTLEVEL { get; set; }

    public string TRIPDATE { get; set; }
    public int FUEL { get; set; }
    public int DISTANCE { get; set; }

    public string MILLEAGE { get; set; }
}

public class SMVTS_TYRE_NEWMODEL : SMVTS_SIMS
{
    public int TYREID { get; set; }
    public string TYRE_VNO { get; set; }
    public string TYRE_NUMBER { get; set; }
    public string TYRE_MAKE { get; set; }
    public DateTime? TYRE_FITDATE { get; set; }
    public int TYRE_FITKMS { get; set; }
    public string TYRE_GRADE { get; set; }
    public string TYRE_POSITION { get; set; }
    public bool TYRE_STATUS { get; set; }
    public int TYRE_CATEGID { get; set; }
    public int Tyre_RUNKMS { get; set; }
    public DateTime? TYRE_CLOSEDDATE { get; set; }
    public SMVTS_TYRE_NEWMODEL()
    {

    }
    public SMVTS_TYRE_NEWMODEL(int _TYREID)
    {
        TYREID = _TYREID;
    }
}




// Written by Sridevi On 04/01/2010 6.00pm
public class SMVTS_SERVICE : SMVTS_MAIN
{
    public int SERVICES_ID { get; set; }
    public string SERVICES_NAME { get; set; }
    public string SERVICES_DESC { get; set; }
    public bool SERVICES_STATUS { get; set; }

    public SMVTS_SERVICE()
    {

    }
    public SMVTS_SERVICE(int _SERVICES_ID)
    {
        SERVICES_ID = _SERVICES_ID;
    }
}
public class SMVTS_GEOFENCING : SMVTS_MAIN
{
    public int GEOFENCING_ID { get; set; }
    public int GEOFENCING_CATEGORY_ID { get; set; }
    public bool GEOFENCING_CIRCLE { get; set; }
    public int GEOFENCING_RADIOUS { get; set; }
    public bool GEOFENCING_STATUS { get; set; }
}
public class SMVTS_ALERTS : SMVTS_MAIN
{
    public string ALERTTYPE { get; set; }
    public string VEHICLENO { get; set; }
    public double LATITUDE { get; set; }
    public double LONGITUDE { get; set; }

    public SMVTS_ALERTS()
    {
    }
}


public class SMVTS_SERVICEBASEDLOGIN : SMVTS_MAIN
{
    public int MyProperty { get; set; }

    public SMVTS_SERVICEBASEDLOGIN()
    {
    }
}
public class PVTS_CATEGORIES
{
    public string CATEG_NAME { get; set; }
    public string CATEG_EMAILID { get; set; }
    public string CATEG_DESC { get; set; }
    public string CATEG_CONTACTPERSON { get; set; }
    public string CATEG_MOBILENUMBER { get; set; }
    public string CATEG_TELEPHONE { get; set; }
    public string CATEG_WEBSITENAME { get; set; }
    public string CATEG_ZIPCODE { get; set; }
    public string CATEG_ADDRESS { get; set; }
    public string MESSAGE { get; set; }
    public int CATEG_STATUS { get; set; }
    public int CATEG_ID { get; set; }

    public int ROLE_ID { get; set; }

    public string ROLE_NAME { get; set; }

    public int ROLE_STATUS { get; set; }

    public string USER_NAME { get; set; }

    public int USER_ID { get; set; }

    public int USER_STATUS { get; set; }

    public string USER_PASSWORD { get; set; }

    public string USER_EMAILID { get; set; }

    public int SIM_ID { get; set; }

    public string SIM_NUMBER { get; set; }

    public string SIM_APNIP { get; set; }

    public int SIM_STATUS { get; set; }

    public string SIM_SERIALNO { get; set; }

    public string SIM_OPERATORNAME { get; set; }

    public int DEVICE_ID { get; set; }

    public string DEVICE_NAME { get; set; }

    public string DEVICE_SERIALNUMBER { get; set; }

    public int DEVICE_STATUS { get; set; }

    public int VEHICLE_STATUS { get; set; }

    public string VEHICLE_ZONE { get; set; }

    public string VEHICLE_REGNUMBER { get; set; }

    public int VEHICLE_ID { get; set; }

    public int PAGE_ID { get; set; }

    public int PAGE_STATUS { get; set; }

    public string PAGE_NAME { get; set; }

    public int PP_EDIT { get; set; }

    public int PP_CREATE { get; set; }

    public int PP_VIEW { get; set; }

    public int PP_ID { get; set; }

    public string DEVICE_MFGDATE { get; set; }

    public string SIM_APNWEBSITE { get; set; }

    public string VEHICLE_MODELNAME { get; set; }

    public string VEHICLE_CAPACITY { get; set; }

    public string VEHICLE_MILLAGE { get; set; }

    public string VEHICLE_MAXSPEED { get; set; }

    public int PANEL_ID { get; set; }

    public int PANEL_STATUS { get; set; }

    public string PANEL_NAME { get; set; }

    public string PAGE_UNDER { get; set; }

    public string CATEG_TYPE { get; set; }

    public string USER_IMAGE { get; set; }

    public string PAGE_PATH { get; set; }

    public string ICON { get; set; }

    public string PAGE_DISPLAYNAME { get; set; }
}

public class SMVTS_GRIDTRACK : SMVTS_MAIN
{
    public string VEHICLE_NO { get; set; }
    public Int16 STATUS { get; set; }
    public Int32 SPEED { get; set; }
    public string VEHSTATUS { get; set; }
    public string VEHICLENUMBER { get; set; }
    public string PLACE { get; set; }
    public DateTime DATE { get; set; }
    public Int32 ODOMETER { get; set; }
    public string IGNITION { get; set; }
    public string DRIVER_NAME { get; set; }

    public string TOTAL_KM_DAY { get; set; }

    public string DRIVERNUMBER { get; set; }
    public string DURATION { get; set; }
    public string DKM { get; set; }
    public string TRIPPLAN { get; set; }
    public string LATITUDE { get; set; }
    public string LONGITUDE { get; set; }
    public string COLOR { get; set; }
    public string COLORSTATUS { get; set; }
    public int DEVICEID { get; set; }
    public string DEVICE_ID { get; set; }
    public string TRIPDATA_DIRECTION { get; set; }
    public string TRIPDATAT_SPEED { get; set; }
    public string TRIPDATA_TRIPDATE { get; set; }
    public string DISTANCE { get; set; }
    public string TIME { get; set; }
    public string DEGREES { get; set; }
    public string AVGSPEED { get; set; }
    public string MAXSPEED { get; set; }
    public string OVERSPEED { get; set; }

    public string st_date { get; set; }
    public string ed_date { get; set; }
    public string VEHICLEMFG { get; set; }

    public string MODEL { get; set; }

    public string TANKCAPACITY { get; set; }

    public string TYPE { get; set; }
    public string WORKORDERID { get; set; }
    public string PARENT_NAME { get; set; }
    public string DEALER_NAME { get; set; }
    public string CUSTOMER_NAME { get; set; }
    public string Lastdate { get; set; }
    public string VEHICLE_IMAGE { get; set; }
    public string IMEI { get; set; }
    public string ExpDate { get; set; }
    public string DeviceName { get; set; }
    public string InstallDate { get; set; }
    public string VehicleType { get; set; }
    public string ac_status { get; set; }
    public SMVTS_GRIDTRACK()
    {

    }


    public string TOTALRUNNINGTIME { get; set; }

    public string TOTALSTOPTIME { get; set; }

    public string NIGHTSTOPTIME { get; set; }

    public string DAYRUN { get; set; }

    public string DAYSTOPS { get; set; }

    public string NIGHTRUN { get; set; }

    public string IDLEKMS { get; set; }

    public string ACERAGE_RUNNNING_SPEED { get; set; }

    public string ACERAGE_SPEED { get; set; }

    public string OS { get; set; }

    public string RA { get; set; }

    public string RD { get; set; }

    public string ND { get; set; }

    public string CD { get; set; }

    public string driverhome_stop { get; set; }

    public string servicetime_stop { get; set; }

    public string loadingtime_stop { get; set; }

    public string ACTUALKMS { get; set; }

    public string ENDLOCATION { get; set; }

    public string STARTLOCATION { get; set; }

    public int TRIPID { get; set; }

    public int Vehicles_id { get; set; }

    public string Mileage { get; set; }
    public string odometer { get; set; }

    public string DEVICE_SIM_ID { get; set; }

    public int DRIVER_ID { get; set; }

    public string SIM_MNO { get; set; }

    public string VEHICLES_RESERVEVOLUME { get; set; }
    
}
public class SMVTS_CONSIGNMENTTRACK : SMVTS_MAIN
{
    public string VEHICLE_NO { get; set; }
    public Int16 STATUS { get; set; }
    public Int32 SPEED { get; set; }
    public string VEHICLENUMBER { get; set; }
    public string PLACE { get; set; }
    public DateTime DATE { get; set; }
    public Int32 ODOMETER { get; set; }
    public string IGNITION { get; set; }
    public string DRIVER_NAME { get; set; }

    public SMVTS_CONSIGNMENTTRACK()
    {
    }
}
public class SMVTS_GEOFENCEGRIDTRACK : SMVTS_MAIN
{
    public string VEHICLE_NO { get; set; }
    public Int16 STATUS { get; set; }
    public Int32 SPEED { get; set; }
    public string VEHICLENUMBER { get; set; }
    public string PLACE { get; set; }
    public DateTime DATE { get; set; }
    public Int32 ODOMETER { get; set; }
    public string IGNITION { get; set; }
    public string DRIVER_NAME { get; set; }

    public SMVTS_GEOFENCEGRIDTRACK()
    {
    }
}
public class SMVTS_SET_GEOFENCECREDENTIAL : SMVTS_MAIN
{
    //public int CATID { get; set; }
    public string VEHICLENO { get; set; }
    public double LATITUDE { get; set; }
    public double LONGITUDE { get; set; }
    public string RADIUS { get; set; }
    public string LANDMARK { get; set; }


    public SMVTS_SET_GEOFENCECREDENTIAL()
    {
    }
}
public class SMVTS_CHANGEPASSWORD : SMVTS_MAIN
{

    public string OLDPWD { get; set; }
    public string NEWPWD { get; set; }
    public string USERNAME { get; set; }
    public string FULLNAME { get; set; }
    public int USERID { get; set; }
    public SMVTS_CHANGEPASSWORD()
    {

    }

}
public class SMVTS_SMSCONFIG : SMVTS_MAIN
{
    public int ID { get; set; }
    public string ALERTTYPE { get; set; }
    public string VEHICLENO { get; set; }
    public string CELL { get; set; }
    public int CATID { get; set; }

    public SMVTS_SMSCONFIG()
    {

    }
}
public class SMVTS_IMMOBILIZER : SMVTS_MAIN
{
    //public int ID { get; set; }
    public string VENDORID { get; set; }
    public string DEVICEID { get; set; }
    public string IMB_REQTIME { get; set; }
    public string IMB_ALERTSENTTIME { get; set; }
    public string MB_REQTIME { get; set; }
    public string MB_ALERTSENTTIME { get; set; }
    public string STATUS { get; set; }
    public int CATEID { get; set; }
    public string TYPE { get; set; }






}

public class SMVTS_CHANGEMODEM : SMVTS_MAIN
{
    public int ID { get; set; }
    public string MODEM { get; set; }
    public SMVTS_CHANGEMODEM()
    {

    }
}


public class SMVTS_TRACE_ERROR_LOG : SMVTS_MAIN
{
    public SMVTS_TRACE_ERROR_LOG()
    {
    }
    public string ERR_DESC { get; set; }
    public string USERID { get; set; }
    public string STRSQL { get; set; }
    public DateTime DATE_TIME { get; set; }
    public DateTime STARTTIME { get; set; }
    public DateTime ENDTIME { get; set; }
    //USERID,DATE_TIME,STRSQL,STARTTIME,ENDTIME

}
public class SMVTS_CREATECIRCLE : SMVTS_MAIN
{

    public string VEHICLENO { get; set; }
    public SMVTS_CREATECIRCLE()
    {

    }
}


public class SMVTS_LANDMARKS_OMKAR : SMVTS_MAIN
{
    public int LANDMARKS_ID { get; set; }
    public int LANDMARKS_CATEGORY_ID { get; set; }
    public double LANDMARKS_LATITUDE { get; set; }
    public double LANDMARKS_LONGITUDE { get; set; }
    public string LANDMARKS_ADDRESS { get; set; }
    public string LANDMARKS_CONTPERSONS { get; set; }
    public string LANDMARKS_MOBILENUMBER { get; set; }
    public int LANDMARKS_TYPE { get; set; }
    public int? LANDMARKS_LOCATIONTYPE { get; set; }
    public bool LANDMARKS_STATUS { get; set; }

    public SMVTS_LANDMARKS_OMKAR()
    {

    }
    public SMVTS_LANDMARKS_OMKAR(int _LANDMARKS_ID)
    {
        LANDMARKS_ID = _LANDMARKS_ID;
    }
}

public class SMVTS_ROLES : SMVTS_MAIN
{
    public int ROLES_ID { get; set; }
    public string ROLES_ROLENAME { get; set; }
    public int ROLES_ROLETYPE { get; set; }
    public int ROLES_CATEGORY_ID { get; set; }
    public string ROLES_ROLEDESC { get; set; }
    public string ROLES_FORMSID { get; set; }
    public string ROLES_COLUMNSID { get; set; }
    public string ROLES_DASHBOARD { get; set; }

    public SMVTS_ROLES()
    {

    }
    public SMVTS_ROLES(int _ROLES_ID)
    {
        ROLES_ID = _ROLES_ID;
    }
}
public class SMVTS_MONITERROUTES : SMVTS_MAIN
{
    public int MR_ID { get; set; }
    public int MR_CATEID { get; set; }
    public string MR_ROUTENAME { get; set; }
    public bool MR_STATUS { get; set; }
    public int MR_CREATEDBY { get; set; }
    public DateTime MR_CREATEDDATE { get; set; }
    public int MR_MODIFIEDBY { get; set; }
    public DateTime MR_MODIFIEDDATE { get; set; }

    public SMVTS_MONITERROUTES()
    {

    }
    public SMVTS_MONITERROUTES(int _MR_ID)
    {
        MR_ID = _MR_ID;
    }
}




public class SMVTS_SETTINGS : SMVTS_MAIN
{
    public int SETTINGS_ID { get; set; }
    public int SETTINGS_CATEGORY_ID { get; set; }
    // public System.Drawing.Image SETTINGS_CLIENTLOGO { get; set; }
    public byte[] SETTINGS_CLIENTLOGO { get; set; }
    public SMVTS_SETTINGS()
    {

    }
    public SMVTS_SETTINGS(int _SETTINGS_ID)
    {
        SETTINGS_ID = _SETTINGS_ID;
    }
}


public class SMVTS_CONSIGNMENT : SMVTS_MAIN
{

    public int CONSIGNMENT_ID { get; set; }
    public string CONSIGNMENT_DETAILS { get; set; }
    public string CONSIGNMENT_FROM { get; set; }
    public string CONSIGNMENT_CONSIGNEE_DETAILS { get; set; }
    public string CONSIGNMENT_TO { get; set; }
    public int CONSIGNMENT_CATEG_ID { get; set; }

    public double VEHROUTE_MIN_DISTANCE { get; set; }
    public string VEHROUTE_PLANNED_STARTDATE { get; set; }
    public string VEHROUTE_PLANNED_ENDDATE { get; set; }
    public string VEHROUTE_SCHED_TIMEDEVLIVERY { get; set; }
    public int VEHROUTE_CONSIGNMENT_ID { get; set; }
    public string CONSIGNMENT_FORMAN { get; set; }
    public string CONSIGNMENT_DRIVER_NUMBER { get; set; }
    public string CONSIGNMENT_ZONE { get; set; }


    public SMVTS_CONSIGNMENT()
    {

    }
    public SMVTS_CONSIGNMENT(int _CONSIGNMENT_ID)
    {
        CONSIGNMENT_ID = _CONSIGNMENT_ID;
    }

}


public class SMVTS_LOGOS : SMVTS_MAIN
{
    public int LOGOS_ID { get; set; }
    public int LOGOS_CATEGORY_ID { get; set; }
    public string LOGOS_NAME { get; set; }
    public string LOGOS_URL { get; set; }

    public SMVTS_LOGOS()
    {

    }
    public SMVTS_LOGOS(int _LOGOS_ID)
    {
        LOGOS_ID = _LOGOS_ID;
    }


}
public class SMVTS_ASSOCIATED : SMVTS_MAIN
{
    public string associated { get; set; }

    public string USERS_USERNAME { get; set; }
    public string USERS_ID { get; set; }
}

public class SMVTS_ZONES : SMVTS_MAIN
{
    public int ZONE_ID { get; set; }
    public string ZONE_CODE { get; set; }
    public string ZONE_NAME { get; set; }
    public bool ZONE_STATUS { get; set; }
    public int ZONE_CATEG_ID { get; set; }

    public SMVTS_ZONES()
    {
    }
    public SMVTS_ZONES(int _ZONE_ID)
    {
        ZONE_ID = _ZONE_ID;
    }
}

public class SMVTS_CONTROLLING_BRANCH : SMVTS_MAIN
{
    //public string OPERATION	{ get; set; }
    public string BRANCH_ID { get; set; }
    public string ZONECODE { get; set; }
    public string BRANCH_NAME { get; set; }
    public string LOCATION_NAME { get; set; }
    public string ABBREVIATION { get; set; }
    public string ADDRESS_BUILDING { get; set; }
    public string ADDRESS_AREA { get; set; }
    public string ADDRESS_STREET { get; set; }
    public string ADDRESS_MILESTONE { get; set; }
    public int ADDRESS_COUNTRY { get; set; }
    public int ADDRESS_STATE { get; set; }
    public int ADDRESS_CITY { get; set; }
    public string ADDRESS_PHONE1 { get; set; }
    public string ADDRESS_PHONE2 { get; set; }
    public string ADDRESS_FAX { get; set; }
    public string CONTACT_PERSON1 { get; set; }
    public string CONTACT_MOBILE1 { get; set; }
    public string CONTACT_EMAIL1 { get; set; }
    public string CONTACT_PERSON2 { get; set; }
    public string CONTACT_MOBILE2 { get; set; }
    public string CONTACT_EMAIL2 { get; set; }
    public string CONTACT_PAN { get; set; }
    public string CONTACT_WEBSITE { get; set; }
    public string CONTACT_PINCODE { get; set; }
    public string PRINTING_ADDRESS { get; set; }
    public bool STATUS { get; set; }
    public int CATEG_ID { get; set; }
    public int CNTRBRANCH_ADDRESS_COUNTRY { get; set; }


    public SMVTS_CONTROLLING_BRANCH()
    {
    }
    public SMVTS_CONTROLLING_BRANCH(string _BRANCH_ID)
    {
        BRANCH_ID = _BRANCH_ID;
    }
}


public class SMVTS_PLACE_MASTER : SMVTS_MAIN
{
    public string PLACE_CODE { get; set; }
    public string PLACE_NAME { get; set; }
    public string PLACE_STATE { get; set; }
    public int PLACE_CNTRBRANCH_ID { get; set; }
    public bool PLACE_STATUS { get; set; }
    public int PLACE_LANDMARK_ID { get; set; }
    public int PLACE_CATEG_ID { get; set; }
    public string PLACE_SEARCHLANDMARK { get; set; }
    public int PLACE_ZONECODE { get; set; }

    public SMVTS_PLACE_MASTER()
    {
    }
    public SMVTS_PLACE_MASTER(string _PLACE_CODE)
    {
        PLACE_CODE = _PLACE_CODE;
    }
}


public class SMVTS_SHIFTMASTER : SMVTS_MAIN
{

    public int SHIFTMASTER_ID { get; set; }
    public int SHIFTMASTER_CATEGID { get; set; }
    public string SHIFTMASTER_SHIFTNAME { get; set; }
    public string SHIFTMASTER_DESC { get; set; }
    public string SHIFTPLANNING_DEVICE_IDS { get; set; }
    public string SHIFTMASTER_STARTDATETIME { get; set; }
    public string SHIFTMASTER_ENDDATETIME { get; set; }
    public bool SHIFTMASTER_STATUS { get; set; }
    public int SHIFTMASTER_CREATEDBY { get; set; }
    public string SHIFTMASTER_CREATEDDATE { get; set; }
    public int SHIFTMASTER_MODIFIEDBY { get; set; }
    public string SHIFTMASTER_MODIFIEDDATE { get; set; }

    public SMVTS_SHIFTMASTER()
    {
    }
    public SMVTS_SHIFTMASTER(int _SHIFTMASTER_ID)
    {
        SHIFTMASTER_ID = _SHIFTMASTER_ID;
    }
}


public class SMVTS_SHIFTPLANNING : SMVTS_MAIN
{

    public int SHIFTPLANNING_ID { get; set; }
    public int SHIFTPLANNING_CATEGID { get; set; }
    public int SHIFTPLANNING_SHIFTMASTER_ID { get; set; }
    public int SHIFTPLANNING_ROUTES_ID { get; set; }
    public string SHIFTPLANNING_DEVICE_IDS { get; set; }
    public int SHIFTPLANNING_CREATEDBY { get; set; }
    public string SHIFTPLANNING_CREATEDDATE { get; set; }
    public int SHIFTPLANNING_MODIFIEDBY { get; set; }
    public string SHIFTPLANNING_MODIFIEDDATE { get; set; }
    public string SHIFTPLANNING_NAME { get; set; }



    public SMVTS_SHIFTPLANNING()
    {
    }
    public SMVTS_SHIFTPLANNING(int _SHIFTPLANNING_ID)
    {
        SHIFTPLANNING_ID = _SHIFTPLANNING_ID;
    }
}

public class SMVTS_FORMS : SMVTS_MAIN
{
    public int FORMS_ID { get; set; }
    public string FORMS_NAME { get; set; }
    public string FORMS_DESC { get; set; }

    public int COLUMN_ID { get; set; }
    public string COLUMN_NAME { get; set; }
}

public class SMVTS_DEMODEVICES : SMVTS_MAIN
{
    public int Client_Id { get; set; }
    public string Client_Name { get; set; }
    public string Client_Address { get; set; }
    public string Client_Instance { get; set; }
    public int Client_Instance_Id { get; set; }
    public DateTime Client_Start_Date { get; set; }
    public int Client_Total_Days { get; set; }
    public int Client_Total_Devices { get; set; }
    public string Client_Contact_Person { get; set; }
    public string Client_Mobile_Number { get; set; }
    public string Client_Prospect_By { get; set; }
    public string Client_Created_By { get; set; }
    public int Client_Status { get; set; }
}

public class smvts_vendormasters : SMVTS_MAIN
{
    public string vendor_name { get; set; }
    public string vendor_address { get; set; }
    public string description { get; set; }
    public string contact_person { get; set; }
    public string contact_mobilenumber { get; set; }
    public int status { get; set; }
    public int created_by { get; set; }
    public DateTime created_date { get; set; }
    public int modified_by { get; set; }
    public DateTime modified_date { get; set; }
    public int category_id { get; set; }
    public int command { get; set; }
    public int vendor_id { get; set; }
}

public class SMVTS_DEALERS : SMVTS_MAIN
{
    public string DEALERS_NAME { get; set; }
    public string DEALER_address { get; set; }
    public string DEALER_description { get; set; }
    public string DERALER_CONTACTPERSON { get; set; }
    public string DEALER_MOBILE { get; set; }
    public int DEALER_STATUS { get; set; }
    public int DEALER_CATEGORYID { get; set; }
    public int DEALER_ID { get; set; }
}

public class smvts_officemasters : SMVTS_MAIN
{
    public string office_name { get; set; }
    public string office_description { get; set; }
    public int command { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public int status { get; set; }
    public int created_by { get; set; }
    public DateTime created_date { get; set; }
    public int modified_by { get; set; }
    public DateTime modified_date { get; set; }
    public int category_id { get; set; }
    public int officemaster_id { get; set; }
}


public class SMVTS_ROUTES_PICKUP : SMVTS_MAIN
{
    public int PICKUP_ID { get; set; }
    public int PICKUP_ROUTEID { get; set; }
    public string PICKUP_POINTS { get; set; }
    public int PICKUP_CATEGORY_ID { get; set; }

    public SMVTS_ROUTES_PICKUP(int ID)
    {
        PICKUP_ROUTEID = ID;
    }
}

public class SMVTS_PICKUP_NAMES : SMVTS_MAIN
{

    public int PICKUPPOINT_NAME_ID { get; set; }
    public int PICKUPNAMES_POINT_ID { get; set; }
    public string PICKUPPOINT_NAMES { get; set; }
    public string PICKUPPOINT_NAMES_LAT { get; set; }
    public string PICKUPPOINT_NAMES_LNG { get; set; }
    public int PICKUPPOINT_NAMES_ROUTE_ID { get; set; }
    public int PICKUPPOINTS_NAME_TYPE { get; set; }

}

public class SMVTS_DASHBOARDFORCABS : SMVTS_MAIN
{

    public int OFFICEMASTER_ID { get; set; }
    public int SHIFTMASTER_ID { get; set; }

}
public class SMVTS_DRIVER_REMARKS : SMVTS_MAIN
{
    public int REMARK_ID { get; set; }
    public int REMARK_DRIVER_ID { get; set; }
    public string REMARK_AWARD { get; set; }
    public string REMARK_PENALITY_TYPE { get; set; }
    public string REMARK_PENALITY_DESCRIPTION { get; set; }
    public int REMARK_CATEGORY_ID { get; set; }
    public int REMARK_STATUS { get; set; }
}
public class SMVTS_ASSIGNDRIVER : SMVTS_MAIN
{
    public int ASSIGNDRIVERS_ID { get; set; }
    public int ASSIGNDRIVERS_DRIVERID { get; set; }
    public int ASSIGNDRIVERS_VEHICLEID { get; set; }
    public DateTime ASSIGNDRIVERS_DATE { get; set; }
    public int ASSIGNDRIVERS_STATUS { get; set; }
    public int ASSIGNDRIVER_CATEGORYID { get; set; }
}

public class SMVTS_ASSIGN_TRACTOR_LM : SMVTS_MAIN
{
    public int TRACTORLM_ID { get; set; }
    public int TRACTORLM_DEVICE_ID { get; set; }
    public int TRACTORLM_SOURCE_ID { get; set; }
    public string TRACTORLM_LANDMARKS { get; set; }
    public int TRACTORLM_STATUS { get; set; }
    public int TRACTORLM_CATEGID { get; set; }
}


public class SMVTS_FUELINFO : SMVTS_MAIN
{
    public int @FUELINFO_ID { get; set; }
    public int FUELINFO_CATEG_ID { get; set; }
    public int FUELINFO_DEVICE_ID { get; set; }
    public string FUELINFO_VEHICLE_NO { get; set; }
    public string FUELINFO_DRIVERNAME { get; set; }
    public string FUELINFO_CURRODOMETER { get; set; }
    public DateTime FUELINFO_DATETIME { get; set; }
    public double FUELINFO_NOOFLITERS { get; set; }
    public double FUELINFO_AMOUNT { get; set; }
    public string FUELINFO_PERTOLPUMP { get; set; }
    public int VEHICLES_DEVICEID { get; set; }
    public string FUELINFO_FUELTYPE { get; set; }
    public string FUELINFO_UNIT { get; set; }
    public int FUELINFO_USERID { get; set; }

    public SMVTS_FUELINFO()
    {
    }
    public SMVTS_FUELINFO(int _FUELINFO_ID)
    {
        FUELINFO_ID = _FUELINFO_ID;
    }
}
public class SMVTS_ASSIGNEDGEOFENCESIMS : SMVTS_MAIN
{
    public int CATEGORY_ID { get; set; }
    public int ID { get; set; }
    public string SIMS { get; set; }
    public string VEHICLEID { get; set; }
    public int LANDID { get; set; }
    public int STATUS { get; set; }
}
public class CUSTOMISED_ETA : SMVTS_MAIN
{
    public int CATEGORY_ID { get; set; }
    public int VEHICLEID { get; set; }
}
public class SMVTS_VEHICLE_MODELS : SMVTS_MAIN
{
    public int VEHMODELIMAGES_ID { get; set; }
    public string VEHMODELIMAGES_COLOR { get; set; }
    public int VEHMODELIMAGES_VALUE { get; set; }
    public string VEHMODEL_IMAGENAME { get; set; }

}
public class SMVTS_ROUTES_ASSIGNGEOFENCE : SMVTS_MAIN
{
    public int ROUTES_ASSIGNGEOFENCE_ID { get; set; }
    public string ROUTES_ASSIGNGEOFENCE_POINTSID { get; set; }
    public int ROUTES_ASSIGNGEOFENCE_STARTID { get; set; }
    public int ROUTES_ASSIGNGEOFENCE_ENDID { get; set; }
    public int ROUTES_ASSIGNGEOFENCE_CATEGORYID { get; set; }
    public string ROUTES_ASSIGNGEOFENCE_NAME { get; set; }
    public int ROUTES_ASSIGNGEOFENCE_STATUS { get; set; }
}
public class SMVTS_RECTIFIED_DEVICES : SMVTS_MAIN
{
    public int RECTIFIED_DEVICEID { get; set; }
    public string RECTIFIED_VEHICLES_NUMBER { get; set; }
    public int RECTIFIED_REMARKS_ID { get; set; }
    public string RECTIFIED_REMARKS { get; set; }
    public string RECTIFIED_BY { get; set; }
    public DateTime RECTIFIED_DATE { get; set; }

}

public class SMVTS_DEMOREQUEST : SMVTS_MAIN
{
    public int DEMOREQUEST_ID { get; set; }
    public string DEMOREQUEST_REQUESTORNAME { get; set; }
    public string DEMOREQUEST_COMPANYNAME { get; set; }
    public string DEMOREQUEST_DATETIME { get; set; }
    public string DEMOREQUEST_EMAILID { get; set; }
    public string DEMOREQUEST_MOBILENO { get; set; }
    public string DEMOREQUEST_WEBSITE { get; set; }
    public string DEMOREQUEST_ADDRESS { get; set; }
    public string DEMOREQUEST_FleetSize { get; set; }
    public string DEMOREQUEST_Type_of_Bussiness { get; set; }
}
public class SMVTS_RAISETICKET : SMVTS_MAIN
{
    public int DEMOREQUEST_ID { get; set; }
    public string DEMOREQUEST_REQUESTORNAME { get; set; }
    public string DEMOREQUEST_COMPANYNAME { get; set; }
    public string DEMOREQUEST_DATETIME { get; set; }
    public string DEMOREQUEST_EMAILID { get; set; }
    public string DEMOREQUEST_MOBILENO { get; set; }

    public string DEMOREQUEST_DESC { get; set; }
    public string DEMOREQUEST_COMPLETEDBY { get; set; }
    public string DEMOREQUEST_COMPLETEDDATE { get; set; }
    public string DEMOREQUEST_COMMENTS { get; set; }
    public int DEMOREQUEST_STATUS { get; set; }


    public int TICKET_ID { get; set; }
    public string TICKET_REQUESTORNAME { get; set; }
    public string TICKET_DATETIME { get; set; }
    public string TICKET_COMPANYNAME { get; set; }
    public string TICKET_TICKETDESC { get; set; }
    public string TICKET_MOBILENO { get; set; }
}

public class SMVTS_TICKETCLOSE : SMVTS_MAIN
{
    public int CLOSE_ID { get; set; }
    public int CLOSE_TICKETID { get; set; }
    public string CLOSE_COMPLETEDBY { get; set; }
    public string CLOSE_COMPLETEDDATE { get; set; }
    public string CLOSE_COMMENTS { get; set; }
}

public class SMVTS_ENQUIRYFORM : SMVTS_MAIN
{
    public int ENQUIRYFORM_ID { get; set; }
    public string ENQUIRYFORM_NAME { get; set; }
    public string ENQUIRYFORM_COMPANYNAME { get; set; }
    public string ENQUIRYFORM_DATETIME { get; set; }
    public string ENQUIRYFORM_DESC { get; set; }
    public string ENQUIRYFORM_EMAILID { get; set; }
    public string ENQUIRYFORM_MOBILENO { get; set; }
    public string ENQUIRYFORM_WEBSITE { get; set; }
    public string ENQUIRYFORM_ADDRESS { get; set; }
    public string ENQUIRYFORM_FleetSize { get; set; }
    public string ENQUIRYFORM_Type_of_Bussiness { get; set; }
}



public class SMVTS_FEEDBACK : SMVTS_MAIN
{
    public int FEEDBACK_ID { get; set; }
    public string FEEDBACK_NAME { get; set; }
    public string FEEDBACK_EMAILID { get; set; }
    public string FEEDBACK_DATETIME { get; set; }
    public string FEEDBACK_MOBILENO { get; set; }
    public string FEEDBACK_DESC { get; set; }

}

public class SMVTS_SHAREWITHFRIEND : SMVTS_MAIN
{
    public int SHAREWITHFRIEND_ID { get; set; }
    public string SHAREWITHFRIEND_FROMEMAILID { get; set; }
    public string SHAREWITHFRIEND_TOEMAILID { get; set; }
    public string SHAREWITHFRIEND_DATE { get; set; }
    public string SHAREWITHFRIEND_DESC { get; set; }
}
public class SMVTS_TYREINFO : SMVTS_MAIN
{
    public int TYREINFO_ID { get; set; }
    public int TYREINFO_TYREID { get; set; }
    public int TYREINFO_PLACEID { get; set; }
    public string TYREINFO_TYRENO { get; set; }
    public string TYREINFO_GRADE { get; set; }
    public string TYREINFO_FITTEDDATE { get; set; }
    public string TYREINFO_FITTEDREADING { get; set; }
    public string TYREINFO_RUNKMS { get; set; }
    public string TYREINFO_MAKE { get; set; }
    public string TYREINFO_CATEGID { get; set; }



}

public class SMVTS_TYREMASTER : SMVTS_MAIN
{
    public int Tyre_Id { get; set; }
    public int DeviceId { get; set; }
    public int CategId { get; set; }
    public int status { get; set; }
    public int NofTyres { get; set; }
}

public class SMVTS_ROUTEABBR : SMVTS_MAIN
{
    public string Route_from { get; set; }
    public string Route_fullname { get; set; }
    public string Zone { get; set; }
    public string USERS_CATEGORY_ID { get; set; }
    public string Routeabr_id { get; set; }
}

public class SMVTS_ACCIDENTVEHICLES : SMVTS_MAIN
{
    public int ACCIDENTVEHICLE_ID { get; set; }
    public int ACCIDENTVEHICLE_DEVICE_ID { get; set; }
    public DateTime? ACCIDENTVEHICLE_DATE { get; set; }
    public DateTime? ACCIDENTVEHICLE_UPDATEDDATE { get; set; }
    public DateTime? ACCIDENTVEHICLE_MODIFIED_DATE { get; set; }
    public int ACCIDENTVEHICLE_STATUS { get; set; }
    public int ACCIDENTVEHICLE_CATEGORYID { get; set; }
}


public class SMVTS_TRANSACTION_DETAILS : SMVTS_MAIN
{

    public int DETAILSID { get; set; }
    public int DETAILS_PRODUCTID { get; set; }
    public float DETAILS_AMOUNT { get; set; }
    public float DETAILS_SHIPPINGCHARGES { get; set; }
    public int DETAILS_QUANTITY { get; set; }
    public float DETAILS_SUBTOTAL { get; set; }
    public int DETAILS_TRANSACTIONID { get; set; }


    public SMVTS_TRANSACTION_DETAILS()
    {
    }
    public SMVTS_TRANSACTION_DETAILS(int _DETAILSID)
    {
        DETAILSID = _DETAILSID;
    }

    public SMVTS_TRANSACTION_DETAILS(int _DETAILSID, int _DETAILS_PRODUCTID, float _DETAILS_AMOUNT, float _DETAILS_SHIPPINGCHARGES,
                                        int _DETAILS_QUANTITY, float _DETAILS_SUBTOTAL, int _DETAILS_TRANSACTIONID)
    {
        DETAILSID = _DETAILSID;
        DETAILS_PRODUCTID = _DETAILS_PRODUCTID;
        DETAILS_AMOUNT = _DETAILS_AMOUNT;
        DETAILS_SHIPPINGCHARGES = _DETAILS_SHIPPINGCHARGES;
        DETAILS_QUANTITY = _DETAILS_QUANTITY;
        DETAILS_SUBTOTAL = _DETAILS_SUBTOTAL;
        DETAILS_TRANSACTIONID = _DETAILS_TRANSACTIONID;

    }


}

public class SMVTS_TRANSACTION_BILLINGADDRESS : SMVTS_MAIN
{
    public int BILLINGID { get; set; }
    public int BILLING_USERID { get; set; }
    public int BILLING_TRANSACTIONID { get; set; }
    public string DETAILS_USERFULLNAME { get; set; }
    public string DETAILS_USERADDRESS { get; set; }
    public int DETAILS_USERCOUNTRY_ID { get; set; }
    public int DETAILS_USERSTATE_ID { get; set; }
    public int DETAILS_USERCITY_ID { get; set; }
    public string DETAILS_USERPIN { get; set; }
    public string DETAILS_USERMOBILENO { get; set; }
    public DateTime DETAILS_USERDATE { get; set; }
    public int DETAILS_USERSTATUS { get; set; }
    public DateTime DETAILS_USER_CREATEDDATE { get; set; }

    public SMVTS_TRANSACTION_BILLINGADDRESS()
    {

    }

    public SMVTS_TRANSACTION_BILLINGADDRESS(int _BILLINGID,
    int _BILLING_USERID,
    int _BILLING_TRANSACTIONID,
    string _DETAILS_USERFULLNAME,
    string _DETAILS_USERADDRESS,
    int _DETAILS_USERCOUNTRY_ID,
    int _DETAILS_USERSTATE_ID,
    int _DETAILS_USERCITY_ID,
    string _DETAILS_USERPIN,
    string _DETAILS_USERMOBILENO,
    DateTime _DETAILS_USERDATE,
    int _DETAILS_USERSTATUS,
    DateTime _DETAILS_USER_CREATEDDATE)
    {
        BILLINGID = _BILLINGID;
        BILLING_USERID = _BILLING_USERID;
        BILLING_TRANSACTIONID = _BILLING_TRANSACTIONID;
        DETAILS_USERFULLNAME = _DETAILS_USERFULLNAME;
        DETAILS_USERADDRESS = _DETAILS_USERADDRESS;
        DETAILS_USERCOUNTRY_ID = _DETAILS_USERCOUNTRY_ID;
        DETAILS_USERSTATE_ID = _DETAILS_USERSTATE_ID;
        DETAILS_USERCITY_ID = _DETAILS_USERCITY_ID;
        DETAILS_USERPIN = _DETAILS_USERPIN;
        DETAILS_USERMOBILENO = _DETAILS_USERMOBILENO;
        DETAILS_USERDATE = _DETAILS_USERDATE;
        DETAILS_USERSTATUS = _DETAILS_USERSTATUS;
        DETAILS_USER_CREATEDDATE = _DETAILS_USER_CREATEDDATE;
    }

}
public class SMVTS_SHOPPING_TRANSACTION : SMVTS_MAIN
{

    public int TRANSACTIONID { get; set; }
    public int TRANSACTION_USERID { get; set; }
    public float TRANSACTION_TOTALAMOUNT { get; set; }
    public DateTime TRANSACTION_TRANSACTIONDATE { get; set; }
    public int TRANSACTION_STATUS { get; set; }


    public SMVTS_SHOPPING_TRANSACTION()
    {
    }
    public SMVTS_SHOPPING_TRANSACTION(int _TRANSACTIONID)
    {
        TRANSACTIONID = _TRANSACTIONID;
    }

    public SMVTS_SHOPPING_TRANSACTION(int _TRANSACTIONID, int _TRANSACTION_USERID, float _TRANSACTION_TOTALAMOUNT, DateTime _TRANSACTION_TRANSACTIONDATE,
                                        int _TRANSACTION_STATUS)
    {
        TRANSACTIONID = _TRANSACTIONID;
        TRANSACTION_USERID = _TRANSACTION_USERID;
        TRANSACTION_TOTALAMOUNT = _TRANSACTION_TOTALAMOUNT;
        TRANSACTION_TRANSACTIONDATE = _TRANSACTION_TRANSACTIONDATE;
        TRANSACTION_STATUS = _TRANSACTION_STATUS;


    }


}
public class SMVTS_INVOICE : SMVTS_MAIN
{

    public int INV_ID { get; set; }
    public string INV_NUMBER { get; set; }
    public int INV_QUANTITY { get; set; }
    public string INV_AMOUNT { get; set; }
    public string INV_DESC { get; set; }
    public int INV_CATEGID { get; set; }

    public SMVTS_INVOICE()
    {
    }
    public SMVTS_INVOICE(int _INV_ID)
    {
        INV_ID = _INV_ID;
    }

    public SMVTS_INVOICE(string _INV_NUMBER, int _INV_QUANTITY, string _INV_AMOUNT,
                                        string _INV_DESC, int _INV_CATEGID)
    {
        INV_CATEGID = _INV_CATEGID;
        INV_NUMBER = _INV_NUMBER;
        INV_QUANTITY = _INV_QUANTITY;
        INV_AMOUNT = _INV_AMOUNT;
        INV_DESC = _INV_DESC;


    }

}


public class SMVTS_OUTSTANDINGTRANSATION : SMVTS_MAIN
{

    public int Ost_id { get; set; }
    public int OUTSTANDINGTRANSATION_TRANSACTIONID { get; set; }
    public int OUTSTANDINGTRANSATION_categid { get; set; }
    public string OUTSTANDINGTRANSATION_monthname { get; set; }
    public string OUTSTANDINGTRANSATION_monthamount { get; set; }


    public SMVTS_OUTSTANDINGTRANSATION()
    {
    }
    public SMVTS_OUTSTANDINGTRANSATION(int _Ost_id)
    {
        Ost_id = _Ost_id;
    }

    public SMVTS_OUTSTANDINGTRANSATION(int _OUTSTANDINGTRANSATION_TRANSACTIONID, int _OUTSTANDINGTRANSATION_categid,
                                        string _OUTSTANDINGTRANSATION_monthname, string _OUTSTANDINGTRANSATION_monthamount)
    {
        OUTSTANDINGTRANSATION_TRANSACTIONID = _OUTSTANDINGTRANSATION_TRANSACTIONID;
        OUTSTANDINGTRANSATION_categid = _OUTSTANDINGTRANSATION_categid;
        OUTSTANDINGTRANSATION_monthname = _OUTSTANDINGTRANSATION_monthname;
        OUTSTANDINGTRANSATION_monthamount = _OUTSTANDINGTRANSATION_monthamount;



    }

}
public class SMVTS_Database : SMVTS_MAIN
{
    public int Database_ID { get; set; }
    public string Database_Name { get; set; }

    public SMVTS_Database()
    {
    }

    public SMVTS_Database(int _Database_ID)
    {
        Database_ID = _Database_ID;
    }
}
public class PVTS_TRIPINFO : SMVTS_MAIN
{
    public int TRIPINFO_ID { get; set; }
    public long TRIPINFO_TRIPID { get; set; }
    public string TRIPINFO_VNO { get; set; }
    public int TRIPINFO_VDEVICEID { get; set; }
    public string TRIPINFO_DRIVERNAME { get; set; }
    public int TRIPINFO_DRIVERID { get; set; }
    public int TRIPINFO_DRIVER_PASSPORTNO { get; set; }
    public string TRIPINFO_CLEANER_PASSPORTNAME { get; set; }
    public string TRIPINFO_CLEANER_PASSPORTNO { get; set; }
    public string TRIPINFO_REG_DRIVERMOBILE { get; set; }
    public int TRIPINFO_STARTID { get; set; }
    public int TRIPINFO_ENDID { get; set; }
    public int TRIPINFO_CASTROLROUTE_ID { get; set; }
    public int TRIPINFO__JP_ID { get; set; }
    public int TRIPINFO_OS_VALUE { get; set; }
    public int TRIPINFO_RA_VALUE { get; set; }
    public int TRIPINFO_RD_VALUE { get; set; }
    public int TRIPINFO_ND_VALUE { get; set; }
    public int TRIPINFO_CD_VALUE { get; set; }
    public string TRIPINFO_MODEOFBRIEFINGTEXT { get; set; }
    public int TRIPINFO_TRIPSTATUS { get; set; }
    public string TRIPINFO_CREATIONDATE { get; set; }
    public string TRIPINFO_STARTDATE { get; set; }
    public string TRIPINFO_EXPECTEDDATE { get; set; }
    public string TRIPINFO_DELIVERYDATE { get; set; }
    public string TRIPINFO_CLOSINGDATE { get; set; }
    public int TRIPINFO_CATEGID { get; set; }
    public string TRIPINFO_CAUTIONMESSAGE { get; set; }
    public int TRIPINFO_GOAL_ID { get; set; }
    public string TRIPINFO_GOALCONVERSATION { get; set; }
    public int TRIPINFO_INCIDENT_ID { get; set; }
    public int TRIPINFO_DMCINPUT_ID { get; set; }
    public int TRIPINFO_TRAINERINPUT_ID { get; set; }

}
public class PVTS_DRIVERHISTORY : SMVTS_MAIN
{
    public int SNO { get; set; }
    public int TRIPID { get; set; }
    public string TRIPINFO_VNO { get; set; }
    public int TRIPINFO_VDEVICEID { get; set; }
    public string TRIPINFO_DRIVERNAME { get; set; }
    public int DRIVERID { get; set; }
    public string TRIPINFO_DRIVER_PASSPORTNO { get; set; }
    public string TRIPINFO_CLEANER_PASSPORTNAME { get; set; }
    public string TRIPINFO_CLEANER_PASSPORTNO { get; set; }
    public string TRIPINFO_REG_DRIVERMOBILE { get; set; }
    public int TRIPINFO_STARTID { get; set; }
    public int TRIPINFO_ENDID { get; set; }
    public int TRIPINFO_CASTROLROUTE_ID { get; set; }
    public int TRIPINFO__JP_ID { get; set; }
    public int TRIPINFO_OS_VALUE { get; set; }
    public int TRIPINFO_RA_VALUE { get; set; }
    public int TRIPINFO_RD_VALUE { get; set; }
    public int TRIPINFO_ND_VALUE { get; set; }
    public int TRIPINFO_CD_VALUE { get; set; }
    public bool TRIP_STATUS { get; set; }
    public DateTime TRIPINFO_CREATIONDATE { get; set; }
    public DateTime TRIPINFO_STARTDATE { get; set; }
    public DateTime TRIPINFO_EXPECTEDDATE { get; set; }
    public DateTime TRIPINFO_DELIVERYDATE { get; set; }
    public DateTime TRIPINFO_CLOSINGDATE { get; set; }
    public int TRIPINFO_CATEGID { get; set; }
    public string TRIPINFO_CAUTIONMESSAGE { get; set; }

    public PVTS_DRIVERHISTORY()
    {

    }
    public PVTS_DRIVERHISTORY(int _SNO)
    {
        SNO = _SNO;
    }
}
public class PVTS_DRIVERHISTORY_cd : SMVTS_MAIN
{
    public int VIOLATION_ID { get; set; }
    public string VIOLATION_VNO { get; set; }
    public int VIOLATION_TRIPID { get; set; }
    public string VIOLATION_TYPE { get; set; }
    public string VIOLATION_STARTLOCATION { get; set; }
    public string VIOLATION_ENDLOCATION { get; set; }
    public DateTime VIOLATION_STARTDATE { get; set; }
    public DateTime VIOLATION_ENDDATE { get; set; }
    public int VIOLATION_CATEGID { get; set; }
    public bool VIOLATION_STATUS { get; set; }
    public string VIOLATION_REMARKS { get; set; }
    public int VIOLATION_DRIVERID { get; set; }
    public Decimal DISTANCE_DURATION { get; set; }
    public int VIOLATION_VALUE { get; set; }

    public PVTS_DRIVERHISTORY_cd()
    {

    }
    public PVTS_DRIVERHISTORY_cd(int _VIOLATION_ID)
    {
        VIOLATION_ID = _VIOLATION_ID;
    }
}
public class pvts_notworkingdevices : SMVTS_MAIN
{
    public int NW_ID { get; set; }
    public string NW_VNO { get; set; }
    public int NW_DEVICEID { get; set; }
    public DateTime NW_LASTRECDATE { get; set; }
    public string NW_LASTLOCATION { get; set; }
    public string NW_COLOR { get; set; }
    public string NW_SERVICE_REMARKS { get; set; }
    public DateTime NW_SERVICE_DATE { get; set; }
    public DateTime NW_RECORDINSERT_DATE { get; set; }

    public string NW_LASTRECDATE_ONE { get; set; }

    public pvts_notworkingdevices()
    {

    }
    public pvts_notworkingdevices(int _NW_ID)
    {
        NW_ID = _NW_ID;
    }
}

public class PVTS_TRAINERINPUT : SMVTS_MAIN
{
    public int TRAINER_ID { get; set; }
    public string TRAINER_TYPE { get; set; }
    public string TRAINER_DESC { get; set; }
    public DateTime? TRAINER_DATE { get; set; }
    public int TRAINER_CREATEDBY { get; set; }
    public int TRAINER_STATUS { get; set; }
}
public class PVTS_INCIDENTSHARING : SMVTS_MAIN
{
    public int INCIDENT_ID { get; set; }
    public string INCIDENT_TYPE { get; set; }
    public DateTime? INCIDENT_TYPEDATE { get; set; }
    public int INCIDENT_STATUS { get; set; }
}

public class SMVTS_ROUTE_BUDGET : SMVTS_MAIN
{
    public int RBUDGET_ID { get; set; }
    public int RBUDGET_CATEGID { get; set; }
    public string RBUDGET_VEHMODELID { get; set; }
    public string RBUDGET_FROMID { get; set; }
    public string RBUDGET_TOID { get; set; }
    public int RBUDGET_UPTOID { get; set; }
    public int RBUDGET_TRIPDAYS { get; set; }
    public string RBUDGET_ROUTECODE { get; set; }
    public float RBUDGET_DISTANCE { get; set; }
    public Decimal RBUDGET_AVG_LTR { get; set; }
    public Decimal RBUDGET_FUELQUANTITY { get; set; }
    public Decimal RBUDGET_FUELPERLITER { get; set; }
    public Decimal RBUDGET_FIXED_CASH { get; set; }
    public DateTime? RBUDGET_STARTDATE { get; set; }
    public DateTime? RBUDGET_ENDDDATE { get; set; }
    public string RBUDGET_REMARKS { get; set; }
    public DateTime? RBUDGET_CREATEDDATE { get; set; }
    public bool RBUDGET_STATUS { get; set; }
    public string RBUDGET_ROUTENAME { get; set; }
    public string RBUDGET_SUBROUTE { get; set; }

    public SMVTS_ROUTE_BUDGET()
    {

    }
    public SMVTS_ROUTE_BUDGET(int _RBUDGET_ID)
    {
        RBUDGET_ID = _RBUDGET_ID;
    }
}

public class SMVTS_LORRYTRACKING_TRACKINGON : SMVTS_MAIN
{
    public int LT_TRON_ID { get; set; }
    public string LT_TRON_NAMES { get; set; }
}

public class SMVTS_LORRYENTRY_NONPLACMENT : SMVTS_MAIN
{
    public int LE_NONPLACEMENT_ID { get; set; }
    public string LE_NONPLACEMENT_NAMES { get; set; }

}
public class SMVTS_LORRY_TRACKING : SMVTS_MAIN
{
    public int LTRACKING_ID { get; set; }
    public int LTRACKING_TRIPID { get; set; }
    public string LTRACKING_LORRYNO { get; set; }
    public DateTime? LTRACKING_DATE { get; set; }
    public string LTRACKING_FROMSTATION { get; set; }
    public string LTRACKING_TOSTATION { get; set; }
    public DateTime? LTRACKING_REPORTDATE { get; set; }
    public string LTRACKING_TIME { get; set; }
    public string LTRACKING_TRACKREMARK { get; set; }
    public DateTime? LTRACKING_DELVDATE { get; set; }
    public string LTRACKING_TACKINGON { get; set; }
    public string LTRACKING_LOCATION { get; set; }
    public string LTRACKING_CREATEDBY { get; set; }
    public DateTime? LTRACKING_CREATEDDATE { get; set; }
    public int LTRACKING_STATUS { get; set; }
    public string LTRACKING_PARTYNAME { get; set; }
    public string LTRACKING_UNLOADLOCATION { get; set; }
    public DateTime? LTRACKING_UNLOADDATE { get; set; }
}

public class SMVTS_LORRY_TRACKING_LOG : SMVTS_MAIN
{
    public int LTRACKING_ID { get; set; }
    public int LTRACKING_TRIPID { get; set; }
    public string LTRACKING_LORRYNO { get; set; }
    public DateTime? LTRACKING_DATE { get; set; }
    public string LTRACKING_FROMSTATION { get; set; }
    public string LTRACKING_TOSTATION { get; set; }
    public DateTime? LTRACKING_REPORTDATE { get; set; }
    public string LTRACKING_TIME { get; set; }
    public string LTRACKING_TRACKREMARK { get; set; }
    public DateTime? LTRACKING_DELVDATE { get; set; }
    public string LTRACKING_TACKINGON { get; set; }
    public string LTRACKING_LOCATION { get; set; }
    public string LTRACKING_CREATEDBY { get; set; }
    public DateTime? LTRACKING_CREATEDDATE { get; set; }
    public string LTRACKING_MODIFIEDBY { get; set; }
    public DateTime? LTRACKING_MODOFIEDDATE { get; set; }
    public int LTRACKING_STATUS { get; set; }
    public string LTRACKING_PARTYNAME { get; set; }
    public string LTRACKING_UNLOADLOCATION { get; set; }
    public DateTime? LTRACKING_UNLOADDATE { get; set; }
}

public class SMVTS_LORRY_ENTRY_LOG : SMVTS_MAIN
{
    public int LENTRY_ID { get; set; }
    public string LENTRY_TRAFFICCODE { get; set; }
    public string LENTRY_TRAFFICINCHRGENAME { get; set; }
    public string LENTRY_LORRYNO { get; set; }
    public string LENTRY_NONPLACEMENTREASON { get; set; }
    public string LENTRY_REMARKS { get; set; }
    public string LENTRY_BOOKINGTYPE { get; set; }
    public string LENTRY_PARTYCODE { get; set; }
    public string LENTRY_PARTYNAME { get; set; }
    public string LENTRY_FROMCODE { get; set; }
    public string LEMNTRY_FROMSTATION { get; set; }
    public string LENTRY_TOCODE { get; set; }
    public string LENTRY_TODESTINATION { get; set; }
    public string LENTRY_UPTO { get; set; }
    public string LENTRY_VEHICLETYPE { get; set; }
    public DateTime? LENTRY_ORDERDATETIME { get; set; }
    public DateTime? LENTRY_PLACEDTIME { get; set; }
    public string LENTRY_BROKERCODE { get; set; }
    public string LENTRY_BROKERNAME { get; set; }
    public int LENTRY_STOPPOINTS { get; set; }
    public string LENTRY_RATETYPE { get; set; }
    public Decimal LENTRY_RATESETTLED { get; set; }
    public Decimal LENTRY_GUARANTEEWT { get; set; }
    public Decimal LENTRY_KANTAWT { get; set; }
    public Decimal LENTRY_LRWT { get; set; }
    public int LENTRY_HIRE { get; set; }
    public int LENTRY_ADVANCE { get; set; }
    public string LENTRY_LORRYDETAIN { get; set; }
    public DateTime? LENTRY_REPORTTIME { get; set; }
    public DateTime? LENTRY_DESPATCHTIME { get; set; }
    public string LENTRY_SUPERVCODE { get; set; }
    public string LENTRY_SUPERVBOOKINGNAME { get; set; }
    public DateTime? LENTRY_EXPECTEDDLYDATE { get; set; }
    public string LENTRY_CHALLANNO { get; set; }
    public int LENTRY_GRNO { get; set; }
    public string LENTRY_INVOICENO { get; set; }
    public string LENTRY_DRIVERNAME { get; set; }
    public string LENTRY_DRIVERPHNO { get; set; }
    public string LENTRY_OWNERNAME { get; set; }
    public string LENTRY_OWNERPHNO { get; set; }
    public DateTime? LENTRY_REPORTINGDATE { get; set; }
    public DateTime? LENTRY_DELIVERYDATE { get; set; }
    public string LENTRY_DLYAT { get; set; }
    public string LENTRY_ACKNAME { get; set; }
    public string LENTRY_ACKCODE { get; set; }
    public string LENTRY_ACKREMARKS { get; set; }
    public string LENTRY_CREATEDBY { get; set; }
    public DateTime? LENTRY_CREATEDDATE { get; set; }
    public string LENTRY_MODIFIEDBY { get; set; }
    public DateTime? LENTRY_MODIFIEDDATE { get; set; }
    public int LENTRY_STATUS { get; set; }
}
public class SMVTS_LORRY_ENTRY : SMVTS_MAIN
{
    public int LENTRY_ID { get; set; }
    public string LENTRY_TRAFFICCODE { get; set; }
    public string LENTRY_TRAFFICINCHRGENAME { get; set; }
    public string LENTRY_LORRYNO { get; set; }
    public string LENTRY_NONPLACEMENTREASON { get; set; }
    public string LENTRY_REMARKS { get; set; }
    public string LENTRY_BOOKINGTYPE { get; set; }
    public string LENTRY_PARTYCODE { get; set; }
    public string LENTRY_PARTYNAME { get; set; }
    public string LENTRY_FROMCODE { get; set; }
    public string LEMNTRY_FROMSTATION { get; set; }
    public string LENTRY_TOCODE { get; set; }
    public string LENTRY_TODESTINATION { get; set; }
    public string LENTRY_UPTO { get; set; }
    public string LENTRY_VEHICLETYPE { get; set; }
    public DateTime? LENTRY_ORDERDATETIME { get; set; }
    public DateTime? LENTRY_PLACEDTIME { get; set; }
    public string LENTRY_BROKERCODE { get; set; }
    public string LENTRY_BROKERNAME { get; set; }
    public int LENTRY_STOPPOINTS { get; set; }
    public string LENTRY_RATETYPE { get; set; }
    public Decimal LENTRY_RATESETTLED { get; set; }
    public Decimal LENTRY_GUARANTEEWT { get; set; }
    public Decimal LENTRY_KANTAWT { get; set; }
    public Decimal LENTRY_LRWT { get; set; }
    public int LENTRY_HIRE { get; set; }
    public int LENTRY_ADVANCE { get; set; }
    public string LENTRY_LORRYDETAIN { get; set; }
    public DateTime? LENTRY_REPORTTIME { get; set; }
    public DateTime? LENTRY_DESPATCHTIME { get; set; }
    public string LENTRY_SUPERVCODE { get; set; }
    public string LENTRY_SUPERVBOOKINGNAME { get; set; }
    public DateTime? LENTRY_EXPECTEDDLYDATE { get; set; }
    public string LENTRY_CHALLANNO { get; set; }
    public int LENTRY_GRNO { get; set; }
    public string LENTRY_INVOICENO { get; set; }
    public string LENTRY_DRIVERNAME { get; set; }
    public string LENTRY_DRIVERPHNO { get; set; }
    public string LENTRY_OWNERNAME { get; set; }
    public string LENTRY_OWNERPHNO { get; set; }
    public DateTime? LENTRY_REPORTINGDATE { get; set; }
    public DateTime? LENTRY_DELIVERYDATE { get; set; }
    public string LENTRY_DLYAT { get; set; }
    public string LENTRY_ACKNAME { get; set; }
    public string LENTRY_ACKCODE { get; set; }
    public string LENTRY_ACKREMARKS { get; set; }
    public string LENTRY_CREATEDBY { get; set; }
    public DateTime? LENTRY_CREATEDDATE { get; set; }
    public int LENTRY_STATUS { get; set; }
}
public class smvts_job_er_info : SMVTS_MAIN
{
    public DateTime? Gps_Reportingdatetime { get; set; }
    public DateTime? Gps_Dispatchdatetime { get; set; }
    public DateTime? GPS_Uloadingdatetime { get; set; }
    public DateTime? er_expected_date { get; set; }
    public decimal Gps_Distance { get; set; }
    public string Gps_Status { get; set; }
    public string Gps_Location { get; set; }
    public DateTime? modified_date { get; set; }
    public string Vehicleno { get; set; }
    public string bookbranch { get; set; }
    public DateTime? currentdate { get; set; }
    public int remarksid { get; set; }
    public string REMARK_DESC { get; set; }
    public string From_loc { get; set; }
    public string er_partyname { get; set; }
    public string operation { get; set; }



}

public class SMVTS_ER_upload : SMVTS_MAIN
{
    public int ER_ID { get; set; }
    public string ER_VEHICLENO { get; set; }
    public string ER_VEHICLETYPE { get; set; }
    public string ER_BOOKBRANCH { get; set; }
    public string ER_BOOKZONE { get; set; }
    public DateTime ER_DISPATCHDATE { get; set; }
    public string ER_FROM { get; set; }
    public string ER_TO { get; set; }
    public string ER_PARTYNAME { get; set; }
    public string ER_DELIVERYBRANCH { get; set; }
    public string ER_TO_ZONE { get; set; }
    public string ER_EXPECTED_DATE { get; set; }
    public string ER_REPORTING_DATE { get; set; }
    public string ER_CURRENT_DATE { get; set; }
    public string ER_CURRENT_STATUS { get; set; }
    public string ER_LOCATION { get; set; }
    public string ER_ACK_STATUS { get; set; }
    public string ER_DRIVER_NAME { get; set; }
    public string ER_DRIVER_PHONE { get; set; }
    public string ER_CNTR_BRANCH { get; set; }
    public string ER_FORMAN_DETAILS { get; set; }
    public string ER_DEST_CONTROL_BRANCH { get; set; }
    public string ER_FORMAN_BRANCH { get; set; }
    public string DATE { get; set; }
    public string ER1 { get; set; }
    public string ER2 { get; set; }
    public string ER3 { get; set; }
    public string ER4 { get; set; }
    public string ER5 { get; set; }
}

public class SMVTS_MOBILEAPP : SMVTS_MAIN
{
    public int ID { get; set; }
    public int CATAGORY_ID { get; set; }
    public string MOBILE_NO { get; set; }
    public string OTP { get; set; }
    public string MOBILEIMEI { get; set; }
    public DateTime CREATED_DATE { get; set; }

}





public class SMVTS_PICKUPPOINTS_SMS : SMVTS_MAIN
{
    public int SrNo { get; set; }
    public int SMS_ID { get; set; }
    public string SMS_VEHICLENO { get; set; }
    public string SMS_PICKUPPOINT { get; set; }
    public DateTime? SMS_PICKUPTIME { get; set; }
    public string SMS_CUSTNAME { get; set; }
    public string SMS_CUSTMOBILE { get; set; }
    public string SMS_ROUTENAME { get; set; }
    public string SMS_ORIGIN { get; set; }
    public string SMS_DEST { get; set; }
    public string SMS_AGE { get; set; }
    public string SMS_ORIGIN_PASSEGNER { get; set; }
    public string SMS_DEST_PASSENGER { get; set; }
    public int SMS_CATEGID { get; set; }
    public DateTime? SMS_DROPTIME { get; set; }
    public string SMS_DROPPOINT { get; set; }
    public string SMS_PNR_NO { get; set; }
    public int SMS_STATUS { get; set; }


}

public class SMVTS_WORKORDER_TRIPS : SMVTS_MAIN
{
    public int TRIP_ID { get; set; }
    public int TRIP_WORKORDER_ID { get; set; }
    public string TRIP_VEHICLENO { get; set; }
    public string TRIP_DEVICEID { get; set; }
    public string TRIP_LR_NO { get; set; }
    public string TRIP_LR_DATE { get; set; }
    public int TRIP_CREATED_BY { get; set; }
    public DateTime? TRIP_CREATED_DATETIME { get; set; }
    public int TRIP_MODIFIED_BY { get; set; }
    public DateTime? TIRP_MODIFIED_DATETIME { get; set; }
    public int TRIP_CATEGORY_ID { get; set; }
    public int TRIP_STATUS { get; set; }
    public DateTime? TRIP_REPORT_INTIME_PORT { get; set; }
    public DateTime? TRIP_REPORT_OUTTIME_PORT { get; set; }
    public DateTime? TRIP_PLANT_INTIME { get; set; }
    public DateTime? TRIP_PLANT_OUTTIME { get; set; }
    public DateTime? TRIP_UNLOAD_INTIME { get; set; }
    public DateTime? TRIP_UNLOAD_OUTTIME { get; set; }
    public int TRIP_CALCULATION_TYPE { get; set; }
}
public class SMVTS_ERPL_LOADINGSTAFF : SMVTS_MAIN
{

    public int LOADINGSTAFF_ID { get; set; }
    public string LOADINGSTAFF_NAME { get; set; }
    public string LOADINGSTAFF_MOBILENUMBER { get; set; }
    public string LOADINGSTAFF_BRANCH { get; set; }
    public DateTime LOADINGSTAFF_CREATEDDATE { get; set; }
    public DateTime LOADINGSTAFF_MODIFIEDDATE { get; set; }
    public string LOADINGSTAFF_OTP { get; set; }
    public string LOADINGSTAFF_IMEI { get; set; }
    public SMVTS_ERPL_LOADINGSTAFF()
    {
    }

    public SMVTS_ERPL_LOADINGSTAFF(int _LOADINGSTAFF_ID)
    {
        LOADINGSTAFF_ID = _LOADINGSTAFF_ID;
    }

}
public class SMVTS_TRIPINFO_STATIONS : SMVTS_MAIN
{

    public int STATION_ID { get; set; }
    public string STATION_CODE { get; set; }
    public string STATION_NAME { get; set; }
    public string STATION_DESC { get; set; }
    public string SATATION_LAT { get; set; }
    public string SATATION_LNG { get; set; }
    public int STATION_CREATED_BY { get; set; }
    public DateTime STATION_CREATED_DATE { get; set; }
    public int STATION_MODIFIED_BY { get; set; }
    public DateTime STATION_MODIFIED_DATE { get; set; }
    public int STATION_STATUS { get; set; }
    public SMVTS_TRIPINFO_STATIONS()
    {
    }

    public SMVTS_TRIPINFO_STATIONS(int _STATION_ID)
    {
        STATION_ID = _STATION_ID;
    }
}
public class SMVTS_TRIPINFO_TRANSITTIME : SMVTS_MAIN
{

    public int TT_ID { get; set; }
    public int TT_FROM_ID { get; set; }
    public int TT_TO_ID { get; set; }
    public int TT_DAYS { get; set; }
    public int TT_DISTANCE { get; set; }
    public int TT_STATUS { get; set; }
    public int TT_CREATED_BY { get; set; }
    public DateTime TT_CREATED_DATE { get; set; }
    public int TT_MODIFIED_BY { get; set; }
    public DateTime TT_MODIFIED_DATE { get; set; }

    public SMVTS_TRIPINFO_TRANSITTIME()
    {
    }

    public SMVTS_TRIPINFO_TRANSITTIME(int _TT_ID)
    {
        TT_ID = _TT_ID;
    }
}


public class VSP_GATE_MASTER : SMVTS_MAIN
{
    public int GATE_ID { get; set; }
    public string GATE_NAME { get; set; }
    public DateTime? GATE_START_TIME { get; set; }
    public DateTime? GATE_END_TIME { get; set; }
    public string GATE_VEHICLES_ALLOWED { get; set; }
    public int GATE_STATUS { get; set; }
    public int GATE_CREATED_BY { get; set; }
    public DateTime? GATE_CREATED_DATE { get; set; }
    public int GATE_MODIFIED_BY { get; set; }
    public DateTime? GATE_MODIFIED_DATE { get; set; }

    public VSP_GATE_MASTER()
    {

    }
    public VSP_GATE_MASTER(int _GATE_ID)
    {
        GATE_ID = _GATE_ID;
    }
}

public class VSP_TRIP_MASTER : SMVTS_MAIN
{
    public int TM_ID { get; set; }
    public int TM_LSGP_ID { get; set; }
    public int TM_ASSET_DEVICE_ID { get; set; }
    public int TM_VEHICLE_ID { get; set; }
    public int TM_ENTRY_GATE_ID { get; set; }
    public int TM_EXIT_GATE_ID { get; set; }
    public int TM_ROUTE_ID { get; set; }
    public int TM_DESTINATION_ID { get; set; }
    public DateTime? TM_START_TIME { get; set; }
    public DateTime? TM_END_TIME { get; set; }
    public int TM_STATUS { get; set; }
    public int TM_CREATED_BY { get; set; }
    public DateTime? TM_CREATED_DATETIME { get; set; }
    public int TM_MODIFIED_BY { get; set; }
    public DateTime? TM_MODIFIED_DATETIME { get; set; }

    public int TM_ACTIVE { get; set; }

    public string TM_VEHICLE_REG_NUMBER { get; set; }

    public VSP_TRIP_MASTER()
    {

    }
    public VSP_TRIP_MASTER(int _TM_ID)
    {
        TM_ID = _TM_ID;
    }
}

public class VSP_LSGP_MASTER
{
    public int LSGP_ID { get; set; }
    public string LSGP_NO { get; set; }
    public string LSGP_PO_NO { get; set; }
    public string LSGP_SALES_ORDER { get; set; }
    public int LSGP_NO_OF_TONS { get; set; }
    public int LSGP_STATUS { get; set; }
    public int LSGP_CREATED_BY { get; set; }
    public DateTime? LSGP_CREATED_DATE { get; set; }
    public int LSGP_MODIFIED_BY { get; set; }
    public DateTime? LSGP_MODIFIED_DATE { get; set; }


    public VSP_LSGP_MASTER()
    {

    }
    public VSP_LSGP_MASTER(int _LSGP_ID)
    {
        LSGP_ID = _LSGP_ID;
    }
   
}

public class SMVTS_CUSTOMERPACKAGE
{
    public int PACKAGE_ID { get; set; }
    public string PACKAGE_NAME { get; set; }
    public string PACKAGE_FORM_IDS { get; set; }
    public int CREATEDBY { get; set; }
    public DateTime CREATEDDATE { get; set; }
    public int MODIFIEDBY { get; set; }
    public DateTime MODIFIEDDATE { get; set; }
    public string PACKAGE_COLUMNIDS { get; set; }
    public decimal PACKAGE_PRICE { get; set; }
    public int NUM_OF_DAYS { get; set; }
    public int CATEG_ID { get; set; }
}
// Created date 12-04-2019 by Ajith
public class Orders
{
    public int InstallationId { get; set; }
    public int PackageId { get; set; }
    public string Vehicle_No { get; set; }
    public string Device_IMEI_NO { get; set; }
    public string Order_Type { get; set; }
    public DateTime? Order_Created_Time { get; set; }
    public DateTime? Order_Expiry_Time { get; set; }
}

public class response
{
    public string status { get; set; }
    // public string result { get; set; }
}

public class  AlertTypes
{
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string AlertType { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Value { get; set; }
    
}

public class SMVTS_VEHICLEMODELS
{
public int     VEHLEMM_ID { get; set; }
    public int VEHLEMM_CATEGORY_ID { get; set; }
    public string VEHLEMM_NAME { get; set; }
    public string VEHLEMM_DESC { get; set; }
    public string VEHLEMM_MAKE { get; set; }
    public string VEHLEMM_MODEL { get; set; }
    public string VEHLEMM_YEAR { get; set; }
    public int VEHLEMM_NOOFTANKS { get; set; }
    public string VEHLEMM_TANKCAPACITY { get; set; }
    public string VEHLEMM_MEASURINGUNIT { get; set; }
    public int VEHLEMM_CREATEDBY { get; set; }
    public DateTime VEHLEMM_CREATEDDATE { get; set; }
    public int VEHLEMM_MODIFIEDBY { get; set; }
    public DateTime VEHLEMM_MODIFIEDDATE { get; set; }
    public string VEHLEMM_IMAGEVALUE { get; set; }
}

//Added by Ajith
public class SMVTS_ALERTS_CONFIG
{
    public int CONFIG_ID { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string CONFIG_TYPE { get; set; }
    public int CONFIG_CATEGID { get; set; }
    public int CONFIG_USERID { get; set; }
    public int CONFIG_STATUS { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public int CONFIG_INTERVAL { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string CONFIG_OPTIONAL1 { get; set; }
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string CONFIG_OPTIONAL2 { get; set; }
}

public class SMVTS_ASSIGNGEOFENCE_LANDMARKS
{
 public int   ID { get; set; }
    public int USERS_ID { get; set; }
    public string ASSIGNGEOFENCE_VEHICLEID { get; set; }
    public int ASSIGNGEOFENCE_CATEGID { get; set; }
    public int ASSIGNGEOFENCE_LANDID { get; set; }
    public string ASSIGNGEOFENCE_SIMS { get; set; }
    public bool ASSIGNGEOFENCE_STATUS { get; set; }
    public string VehicleNumbers { get; set; }
    public string LandMark { get; set; }

}

public class SMVTS_PACKAGE_ORDERS
{
    public int PACKAGE_ORDERID  { get; set; }
    public int ORDER_CATEGORY_ID { get; set; }
    public string ORDER_CATEGORY_NAME { get; set; }
    public int ORDER_TYPE { get; set; }
    public decimal ORDER_AMOUNT { get; set; }
    public bool ASSIGNGEOFENCE_STATUS { get; set; }
    public int ORDER_CREATEDBY { get; set; }
    public DateTime ORDER_CREATEDDATE { get; set; }
    public int ORDER_MODIFIEDBY { get; set; }
    public DateTime ORDER_MODIFIEDDATE { get; set; }
    public int ORDER_STATUS { get; set; }
}
public class SMVTS_DEVICETYPES
{
    public int ID { get; set; }
    public string DEVICE_TYPENAME { get; set; }
    public int CREATEDBY { get; set; }
    public DateTime CREATEDDATE { get; set; }
    public int MODIFIEDBY { get; set; }
    public DateTime MODIFIEDDATE { get; set; }
}
public class SMVTS_ORDERS
{
  public int ORDER_ID { get; set; }
    public string ORDER_NAME { get; set; }
    public int ORDER_CATEGORYID { get; set; }
    public DateTime ORDER_CREATEDATE { get; set; }
    public DateTime ORDER_EXPDATE { get; set; }
    public string ORDER_PACKAGE { get; set; }
    public decimal ORDER_PRICE { get; set; }
    public int ORDER_MODIFIEDBY { get; set; }
    public DateTime ORDER_MODIFIEDDATE { get; set; }
    public DateTime ORDER_STARTDATE { get; set; }
    public int ORDER_STATUS { get; set; }
    public int ORDER_DAYS { get; set; }
    public string DATE { get; set; }
    public string startdate { get; set; }
    public string enddate { get; set; }
    public string categName { get; set; }
    public string SerialNumber { get; set; }
    public string Status { get; set; }
    public string DEVICE_ID { get; set; }
    public string ExpDate { get; set; }
}
public class smvts_tt_devid
{
 public int id { get; set; }
public int DEVICEID { get; set; }
public string IMEI { get; set; }
public int CATEG_ID { get; set; }
}
public class SMVTS_ALL_SIMS
{
  public int ID { get; set; }
public string SIMS { get; set; }
public int CATEG_ID { get; set; }
}
public class CategType
{
  public string Type { get; set; }
    public int Value { get; set; }
}
public class SMVTS_DEVICE_WALLET
{
 public int ID { get; set; }
    public int CATEG_ID { get; set; }
    public string CATEG_NAME { get; set; }
    public string PARENT_NAME { get; set; }
    public int TOTAL { get; set; }
    public int AVAILABLE { get; set; }
    public int ALLOCATED { get; set; }
    public int INSTALLED { get; set; }
    public int REFUND { get; set; }
    public int WITHDRAW { get; set; }
    public int CREATEDBY { get; set; }
    public int DEALER_ID { get; set; }
    public int STATUS { get; set; }
    public DateTime CREAREDDATE { get; set; }
}
public class OTPCode
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}
public  class KMSReport
{
    public string VehicleNumber { get; set; }
    public int Kms { get; set; }
}
public class SMVTS_DEVICE_RENEWALS
{
    public int ID { get; set; }
    public int RENEW_TYPE { get; set; }
    public string FITMENT_DATE { get; set; }
    public string INSTALL_DATE { get; set; }
    public string RENEWAL_DATE { get; set; }
    public string VEH_CHASSIS_NUMBER { get; set; }
    public string VEH_ENGINE_NUMBER { get; set; }
    public string VEH_TYPE { get; set; }
    public string VEH_MAKER { get; set; }
    public string VEH_MODEL { get; set; }
    public string VEH_MODEL_YEAR { get; set; }
    public string VEH_REG_DATE { get; set; }
    public string DEVICE_NAME { get; set; }
    public string DEVICE_IMEI { get; set; }
    public string SIM_NUMBER { get; set; }
    public string SIM_OPERATOR { get; set; }
    public string CUSTOMER_NAME { get; set; }
    public string CUSTOMER_MOBILE { get; set; }
    public string CUSTOMER_ADDRESS { get; set; }
    public string CUSTOMER_PINCODE { get; set; }
    public string VEHICLE_IMAGE_URL { get; set; }
    public string VEHICLE_RC_IMAGE { get; set; }
    public string DEVICE_IMAGE_URL { get; set; }
    public int CREATEDBY { get; set; }
    public DateTime CREATEDATE { get; set; }
    public int MODIFIEDBY { get; set; }
    public DateTime MODIFIEDDATE { get; set; }
    public string RENEWAL_CERTIFICATE { get; set; }
    public string DIVISION { get; set; }
    public string STATE { get; set; }
    public int RENEW_DEVICEID { get; set; }
    public string VEH_REGNUMBER { get; set; }

}
public class SMVTS_ADDRESS
{
    public int city_id { get; set; }
    public string city_name { get; set; }
    public int area_id { get; set; }
    public string area_name { get; set; }
    public int state_id { get; set; }
    public string state_name { get; set; }
    public int country_id { get; set; }
    public string country_name { get; set; }
}
public class smvts_divisions
{
 public int    ID { get; set; }
    public string DIVISION_NAME { get; set; }
public int DIVISION_STATEID{ get; set; }
    public int STATUS{ get; set; }
 public int CREATEDBY{ get; set; }
    public DateTime CREATEDDATE{ get; set; }
    public int  MODIFIEDBY { get; set; }
public DateTime MODIFIEDDATE{ get; set; }
}

public class CUSTOMER_SIMS
{
    public int SIM_ID { get; set; }
    public string SIM_NUMBER { get; set; }
}
public class SHIFT_DEVICE
{
    public string CustomerName { get; set; }
    public string VehicleNumber { get; set; }
    public string DevicenNumber { get; set; }
    public string SIMNumber { get; set; }
    public string DealerName { get; set; }
}
public class searchdata
{
    public string Distributorname { get; set; }
    public string DealerName { get; set; }
    public string CustomerName { get; set; }
    public string UserName { get; set; }
    public string UserPassword { get; set; }
    public string VehicleRegNumebr { get; set; }
    public string IMEI { get; set; }
    public string SimNumber { get; set; }
    public string MobileNo { get; set; }
    public String StartDate { get; set; }
    public string EndDate { get; set; }
    public string Status { get; set; }
    public string SIMOperator { get; set; }
    public int DEVICE_ID { get; set; }
    public int CATEG_ID { get; set; }
    public int verified { get; set; }
    public string Location { get; set; }
    public string LastUpdated { get; set; }
    public string Device_Name { get; set; }
    public string OTP { get; set; }
    public string order_id { get; set; }
}
public class GET_IGNITION
{
    public string VNO { get; set; }
    public string deviceid { get; set; }
    public string startdate { get; set; }
    public string Enddate { get; set; }
    public string Timediff { get; set; }
    public string status { get; set; }
    public string Lndmkadd { get; set; }
    public string Noofmins { get; set; }
    public string KMS { get; set; }
}
public class acreport
{
    public string VNO { get; set; }
    public string deviceid { get; set; }
    public string startdate { get; set; }
    public string Enddate { get; set; }
    public string Timediff { get; set; }
    public string status { get; set; }
    public string Lndmkadd { get; set; }
    

}
public class DevieTokens
{
    public string UserId { get; set; }
    public string Token { get; set; }
}
public class SendNotifications
{
    public string Token { get; set; }
    public string Message { get; set; }
}

public class Expiry_data
{
    public string Distributorname { get; set; }
    public string DealerName { get; set; }
    public string CustomerName { get; set; }
    public string VehicleRegNumebr { get; set; }
    public string DeviceName { get; set; }
    public string IMEI { get; set; }
    public string SimNumber { get; set; }
    public String StartDate { get; set; }
    public string EndDate { get; set; }
    public string CustomerMobile { get; set; }
}
public class smswallet
{
    public string customerName { get; set; }
    public int categid { get; set; }
    public int available { get; set; }
    public int used { get; set; }
    public int total { get; set; }
    public string rechargeDate { get; set; }
}
public class packets
{
    public string newdata { get; set; }
    public string date { get; set; }
}
public class Device_Type
{
    public string Vehicle_Color { get; set; }
    public string Device_Name { get; set; }

}
public class Sim_stock
{
    public int sim_id { get; set; }
    public string sim_number { get; set; }
    public string sim_network { get; set; }
    public string sim_status { get; set; }
    public string sim_iccid { get; set; }
    public string uploadedDate { get; set; }
    public string sim_apn { get; set; }
    public string DistributorName { get; set; }
    public string DealerName { get; set; }
    public int status { get; set; }
    public string activeDate { get; set; }
    public string accountId { get; set; }
    public string plan { get; set; }
    public decimal price { get; set; }
    public string ACTIVE_DATE { get; set; }
    public string Remarks { get; set; }
}
public class SMVTS_ALL_SIMS_stock
{
    public int SIM_ID { get; set; }
  public string SIM_NUMBER { get; set; }
    public string SIM_NETWORK { get; set; }
    public string SIM_ICCD { get; set; }
    public string SIM_APN { get; set; }
    public decimal SIM_PRICE { get; set; }
    public int CREATEDBY { get; set; }
    public DateTime CREATEDDATE { get; set; }
    public int MODIFIEDBY { get; set; }
    public DateTime MODIFIEDDATE { get; set; }
    public int SIM_STATUS { get; set; }
    public int DISTRIBUTOR_ID { get; set; }
    public int DEALER_ID { get; set; }
    public int SIM_LOCK { get; set; }
    public string SIM_REMARKS1 { get; set; }
    public string SIM_REMARKS2 { get; set; }
    public string SIM_PLAN { get; set; }
    public DateTime ACTIVE_DATE { get; set; }
    public DateTime TERMINATE_DATE { get; set; }
    public string ACCOUNT_ID { get; set; }
}
public class SMVTS_SIM_NETWORKS
{
    public int ID { get; set; }
    public string network { get; set; }

}
public class SMVTS_SIM_APNS
{
    public int ID { get; set; }
    public string apn_name { get; set; }

}
public class SMVTS_SIM_PLANS
{
    public int ID { get; set; }
    public string plan_name { get; set; }

}
public class SMVTS_SIM_ACCOUNTS
{
    public int ID { get; set; }
    public string account_name { get; set; }

}
public class shift_device_customers
{
    public int categ_id { get; set; }
    public string customerName { get; set; }
    public string DealerName { get; set; }
}
public class check_sim
{
    public string device_serialnumber { get; set; }
    public string sim_number { get; set; }
    public bool status { get; set; }
}
public class student
{
 public int   ID { get; set; }
 public string   STUDENT_ID { get; set; }
 public string   STUDENT_NAME { get; set; }
 public DateTime   STUDENT_DOB { get; set; }
    public string dob { get; set; }
  public string  STUDENT_CLASS { get; set; }
  public string  PARENT1_NAME { get; set; }
 public string   PARENT1_MOBILE { get; set; }
    public string PARENT1_EMAIL { get; set; }
    public string    PARENT2_NAME { get; set; }
public string    PARENT2_MOBILE { get; set; }

    public string ROUTES_CODE { get; set; }
    public int   STATUS { get; set; }
 public int   CREATEDBY { get; set; }
public  DateTime   CREATEDDATE { get; set; }
public int MODIFIEDBY { get; set; }
public DateTime MOVIFIEDDATE { get; set; }
    
}

public class ctpl_events
{
 public int   EVENT_ID { get; set; }
    public string EVENT_NAME { get; set; }
    public string ENENT_DESC { get; set; }
  public int   EVENT_STATUS { get; set; }
    public int CREATEDBY { get; set; }
  public DateTime  CREATEDATE { get; set; }
}
public class ctpl_events_configuration
{
    public int CONFIG_ID { get; set; }
    public int CONFIG_EVENTID { get; set; }
    public bool CONFIG_APPALERT { get; set; }
    public bool CONFIG_SMSALERT { get; set; }
    public bool CONFIG_EMAILALERT { get; set; }
    public bool CONFIG_STATUS { get; set; }
    public int CONFIG_CREATED_BY { get; set; }
    public DateTime CONFIG_CREATED_DATE { get; set; }
    public int CONFIG_MODIFIED_BY { get; set; }
    public DateTime CONFIG_MODIFIED_DATE { get; set; }
    public int CONFIG_CATEG_ID { get; set; }
    public string AppAlert { get; set; }
    public string SmsAlert { get; set; }
    public string EmailAlert { get; set; }
    public string EventName { get; set; }
}

public class ctpl_eventconfig_details
{
    public int DETAILS_ID { get; set; }
    public int DETAILS_CONFIGID { get; set; }
    public string DETAILS_VEHICLEID { get; set; }
    public int DETAILS_CATEGID { get; set; }
    public int DETAILS_LANDMARKID { get; set; }
    public string DETAILS_MOBILENO { get; set; }
    public string DETAILS_TOMAIL { get; set; }
    public string DETAILS_CCMAIL { get; set; }
    public string DETAILS_BCCMAIL { get; set; }
    public string LANDMARK_ADDRESS { get; set; }
    public int GEOFENCE_ID { get; set; }
}