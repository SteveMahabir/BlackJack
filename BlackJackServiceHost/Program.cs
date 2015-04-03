using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace BlackJackServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;
            try
            {
                // Address
                servHost = new ServiceHost(typeof(CardsLibrary.Game));

                // Manage the service’s life cycle
                servHost.Open();
                Console.WriteLine("Service Successfully Started!  [press any key to shutdown service]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
                if (servHost != null)
                    servHost.Close();
            }
        }
    }
}
