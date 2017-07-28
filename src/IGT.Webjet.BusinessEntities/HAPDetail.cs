using System;
using System.Collections.Generic;
using System.Text;

namespace IGT.Webjet.BusinessEntities
{
    public class HAPDetail
    {
        public HAPDetail()
        {
            //Assign GWSConnURL from configuration file
        }

        public int PCCID { get; set; }
        public string PCC { get; set; }
        public string GDS { get; set; }
        public string Profile { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string GWSConnURL { get; set; }
    }
}
