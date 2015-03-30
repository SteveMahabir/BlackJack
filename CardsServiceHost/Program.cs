/*
 * Program:         CardsServiceHost.exe
 * Module:          Program.cs
 * Author:          T. Haworth
 * Date:            March 9, 2015
 * Description:     Configures a WCF service host for the CardsLibrary.Shoe class.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace CardsServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;
            try
            {
                // Address
                servHost = new ServiceHost(typeof(CardsLibrary.Shoe));

                // Manage the service’s life cycle
                servHost.Open();
                Console.WriteLine("Service started. Press a key to quit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
