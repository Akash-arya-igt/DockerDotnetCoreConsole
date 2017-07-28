using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace IGT.Webjet.GALConnection.GALRequest
{
    public class QueueProcessor
    {
        private GALConnect _GalConn;
        
        private const string RootPath = "GALRequestTemplate";
        private const string QueueProcessRequest = "QueueProcessing.xml";

        public QueueProcessor(GALConnect galConnect)
        {
            _GalConn = galConnect;
        }

        public int GetQueueCount(int queueNumber, string pcc)
        {
            int intQueueKnt = 0;

            XElement requestXml = GetQueueCountrequest(queueNumber, pcc);
            XElement responseXml = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXml, requestXml);

            string strKntPath = "QueueCount/HeaderCount/TotPNRBFCnt";
            var kntNode = XmlHelper.GetChildElement(responseXml, strKntPath);
            if(kntNode != null)
            {
                int.TryParse(kntNode.Value, out intQueueKnt);
            }

            return intQueueKnt;
        }

        public Dictionary<int, int> GetCount(string pcc)
        {
            int intQNum = 0;
            int intQKnt = 0;
            Dictionary<int, int> pccKnt = new Dictionary<int, int>();

            XElement requestXml = GetQueueCountrequest(-1, pcc);
            XElement responseXml = _GalConn.GalConnSubmitRequest(GalWebMethod.SubmitXml, requestXml);
            var kntNodes = XmlHelper.GetChildElementList(responseXml, "QueueCount");

            foreach (var node in kntNodes)
            {
                intQNum = 0;
                intQKnt = 0;

                var qNumNode = XmlHelper.GetChildElement(node, "QNum");
                var qKntNode = XmlHelper.GetChildElement(node, "TotPNRBFCnt");

                if(qNumNode != null && qKntNode != null)
                {
                    if( int.TryParse(qNumNode.Value, out intQNum)
                        && int.TryParse(qKntNode.Value, out intQKnt))
                    {
                        pccKnt.Add(intQNum, intQKnt);
                    }
                }
            }

            return pccKnt;
        }

        private XElement GetQueueCountrequest(int queueNumber, string pcc)
        {
            var xmlStr = File.ReadAllText(Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), RootPath, QueueProcessRequest));
            XElement reqTemplate = XElement.Parse(xmlStr);

            string strActionPath = "QueueMods/QueueSignInCountListMods/Action";
            string strQNumPath = "QueueMods/QueueSignInCountListMods/PCCAry/PCCInfo/QNum";
            string strPCCPath = "QueueMods/QueueSignInCountListMods/PCCAry/PCCInfo/PCC";

            XmlHelper.SetChildElementValue(reqTemplate, strActionPath, QueueProcessingAction.QCT);

            if (queueNumber >= 0)
                XmlHelper.SetChildElementValue(reqTemplate, strQNumPath, queueNumber);
            
            if(!string.IsNullOrEmpty(pcc))
                XmlHelper.SetChildElementValue(reqTemplate, strPCCPath, pcc);

            return reqTemplate;
        }
    }

    public enum QueueProcessingAction
    {
        QCT,    //QueueCountAction
        Q,      //ReadQueueAction
        QXI,    //SignoutQueueAction
        QR,     //RemoveAction
        QLD,    //GetPNRListAction
        QLDS    //GetUniquePNRListAction
    }
}
