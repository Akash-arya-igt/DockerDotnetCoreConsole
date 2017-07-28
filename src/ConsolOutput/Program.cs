using System;
using System.IO;
using IGT.Webjet.GALConnection;
using IGT.Webjet.BusinessEntities;
using Microsoft.Extensions.Configuration;
using IGT.Webjet.GALConnection.GALRequest;

namespace ConsolOutput
{
    public static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] arg)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            HAPDetail objHAP = new HAPDetail()
            {
                GWSConnURL = Configuration["HAPDetail:GWSConnectURL"],
                PCC = Configuration["HAPDetail:PCC"],
                Profile = Configuration["HAPDetail:Profile"],
                UserID = Configuration["HAPDetail:UserId"],
                Password = Configuration["HAPDetail:Password"],
            };

            //Console.WriteLine("Enter Q number");
            //var strQNum = Console.ReadLine();
            //int intQnum = 0;

            //int.TryParse(strQNum, out intQnum);


            Console.WriteLine(string.Format("Getting Counts started at {0} ......", DateTime.Now.ToString()));

            GALConnect objGALConnect = new GALConnect(objHAP);
            QueueProcessor objQProcessing = new QueueProcessor(objGALConnect);

            var QCounts = objQProcessing.GetCount(string.Empty);
            Console.WriteLine(string.Format("Task completed at {0} ......", DateTime.Now.ToString()));
            foreach (var QCount in QCounts)
                Console.WriteLine(string.Format("Current Q Count of Q {0} is {1}", QCount.Key, QCount.Value));

            Console.Read();
        }
    }
}
