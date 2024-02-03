using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using FlightSystem.Model;
using Microsoft.VisualBasic;

namespace FlightSystem
{
    public class Program
    {
        public enum MenuOptions
        {
            AddFlightInfo = 1,
            RemoveFlightInfo,
            GetAllFlightByAirCompany,
            GetAllFlightByStatus,
            Exit,

        }

        public static Flight GetFlightInfoFromUser()
        {
            var flightInfo = new Flight();
            Console.WriteLine("Enter flight number:\t");
            flightInfo.Airline = Console.ReadLine();
            Console.WriteLine("Enter airline:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter destination:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter departure time:\t");
            flightInfo.Airline = Console.ReadLine();
            Console.WriteLine("Enter arrival time:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter gate:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter status:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter duration:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter aircraft type:\t");
            flightInfo.FlightNumber = Console.ReadLine();
            Console.WriteLine("Enter terminal:\t");


            return flightInfo;
        }

        public static FlightStatus GetFlightStatusFromUser()
        {
            Console.WriteLine("Select flight status:");
            Console.WriteLine("1) OnTime");
            Console.WriteLine("2) Delayed");
            Console.WriteLine("3) Cancelled");
            Console.WriteLine("4) Boarding");
            Console.WriteLine("5) InFlight");

            Byte flightStatus;

            while (!Byte.TryParse(Console.ReadLine(), out flightStatus))
            {
                Console.WriteLine("Wrong output, try again!");
            }

            FlightStatus selectedflightStatus = (FlightStatus)(flightStatus - 1);

            return selectedflightStatus;

        }

        public static void Menu(FlightInformationSystem flightSystem)
        {

            Console.WriteLine("Select menu item:");
            Console.WriteLine("1) Add flight info");
            Console.WriteLine("2) Remove flight info");
            Console.WriteLine("3) Get all flights by aircompany");
            Console.WriteLine("4) Get all flights by status");
            Console.WriteLine("5) Exit");

            Byte menuOption;

            while (!Byte.TryParse(Console.ReadLine(), out menuOption))
            {
                Console.WriteLine("Wrong output, try again!");
            }

            MenuOptions selectedOption = (MenuOptions)menuOption;

            switch (selectedOption)
            {
                case MenuOptions.AddFlightInfo:
                    flightSystem.AddFlightInfo(GetFlightInfoFromUser());
                    Console.ReadLine();
                    break;
                case MenuOptions.RemoveFlightInfo:
                    //flightSystem.RemoveFlightInfo();
                    Console.ReadLine();
                    break;
                case MenuOptions.GetAllFlightByAirCompany:
                    Console.WriteLine("Enter aircompany name:");
                    flightSystem.GetAllFlightsByAirCompany(Console.ReadLine());
                    Console.ReadLine();
                    break;
                case MenuOptions.GetAllFlightByStatus:
                    flightSystem.GetAllFlightsByStatus(GetFlightStatusFromUser());
                    Console.ReadLine();
                    break;
                case MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

        }
        static void Main(string[] args)
        {

            var sourceFilePath = Path.Combine(AppContext.BaseDirectory, @"flights_data.json");
            var outputFilePath = Path.Combine(AppContext.BaseDirectory, @"result.json");

            var flightSystem = new FlightInformationSystem(sourceFilePath,
                                                          outputFilePath);
            while (true)
            {
                Menu(flightSystem);
            }

        }
    }
}

