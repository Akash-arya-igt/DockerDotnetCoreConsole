using System.Net;
using System.Xml.Linq;
using System.Threading.Tasks;
using IGT.Webjet.BusinessEntities;
using System;
using System.Collections.Generic;
//using GALReference;

namespace IGT.Webjet.GALConnection
{
    public class GALConnect : GALProxy
    {
        public string SessionID { get { return _token; } set { _token = value; } }
        private string _token = "";
        private string _filter;
        private HAPDetail _hapDetail;

        public GALConnect(HAPDetail _pHAPDetail)
        {
            _filter = string.Empty;
            _hapDetail = _pHAPDetail;

            base.UserName = _pHAPDetail.UserID;
            base.Password = _pHAPDetail.Password;
            base.Url = _pHAPDetail.GWSConnURL;
            base.Profile = _pHAPDetail.Profile;
        }


        public XElement GalConnSubmitRequest(GalWebMethod galMethod, XElement request)
        {
            var resultBodyTask = this.SubmitRequestAsync(galMethod, _token, request, _filter);
            resultBodyTask.Wait();
            return resultBodyTask.Result;
        }

        public async Task<XElement> GalConnSubmitRequestAsync(GalWebMethod galMethod, XElement request)
        {
            var resultBody = await this.SubmitRequestAsync(galMethod, _token, request, _filter);
            return resultBody;
        }
    }
}
