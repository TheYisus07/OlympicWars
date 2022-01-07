using ContractsOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerMain
    {
        public static int Main(string[] args)
        {

            using (ServiceHost host = new ServiceHost(typeof(PlayerService)))
            {
                host.Open();
                Console.WriteLine("Server is running");
                Console.ReadLine();
            }
            return 0;
        }
    }
}