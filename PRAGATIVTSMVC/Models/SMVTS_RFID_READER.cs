using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRAGATIVTSMVC.Models
{
    public class SMVTS_RFID_READER
    {
        public int READER_ID { get; set; }

        public string vehicle_ID { get; set; }
        public string READER_IMEI { get; set; }
        public string READER_VEHICLENO { get; set; }
        public Nullable<System.DateTime> READER_CREATED_DATE { get; set; }
        public string READER_STATUS { get; set; }
    }
}