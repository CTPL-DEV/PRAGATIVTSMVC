using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRAGATIVTSMVC.Models
{
    public class ParentTicketing1
    {

        public string Ticket_Id { get; set; }
        public string TicketMobileNo { get; set; }

            public string Ticket_StudentId { get; set; }
            public string Ticket_Desc { get; set; }
            public string Ticket_Status { get; set; }
            public string Ticket_CreatedBy { get; set; }
            public string Ticket_CreatedDate { get; set; }

            public string Ticket_ClosedDate { get; set; }


        public string Ticket_Remarks { get; set; }



    }
}