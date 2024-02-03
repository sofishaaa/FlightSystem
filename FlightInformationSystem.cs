using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using FlightSystem.Model;
using FlightSystem.Helpers;
using System.IO;
using System.Diagnostics;

namespace FlightSystem
{

    public class FlightInformationSystem
    {
        private readonly string _flightsDatabaseFilePath;

        private readonly string _outputFlightsDatabaseFilePath;

        private List<Flight> _flightSchedule;


        public FlightInformationSystem(string flightsDatabaseFilePath,
                                       string outputFlightsDatabaseFilePath)
        {
            _flightsDatabaseFilePath = flightsDatabaseFilePath;
            _outputFlightsDatabaseFilePath = outputFlightsDatabaseFilePath;
            _flightSchedule = new List<Flight>();

            ReadFlightsDb();
        }


        public string DisplayAllFlights()
        {
            var result = new StringBuilder();
            result.Append(Environment.NewLine);
            result.Append(Environment.NewLine);
            result.Append(Environment.NewLine);
            foreach (var flightInfo in _flightSchedule)
            {
                result.Append(flightInfo.Airline);
                result.Append(Environment.NewLine);

            }

            return result.ToString();
        }

        public void AddFlightInfo(Flight newFlightInfo)
        {
            _flightSchedule.Add(newFlightInfo);
        }

        public void RemoveFlightInfo(Flight flightInfoToRemove)
        {
            _flightSchedule.Remove(flightInfoToRemove);
        }

        public void GetAllFlightsByAirCompany(string airCompanyName)
        {
            var filteredData = _flightSchedule
                    .Where(flightInfo =>
            {
                if (flightInfo.Airline == airCompanyName)
                    return true;
                return false;
            })
                    .OrderBy(currentFlightInfo =>
                    {
                        return currentFlightInfo.DepartureTime;
                    })
                    .ToList();

            Console.WriteLine($"Filtered elements count:\t{filteredData.Count}");
            Console.WriteLine($"Located at:\t{_outputFlightsDatabaseFilePath}");

            SaveFlightDb(filteredData);

        }

        public void GetAllFlightsByStatus(FlightStatus status)
        {
            var filteredData = _flightSchedule
                    .Where(flightInfo =>
            {
                if (flightInfo.Status == status)
                    return true;
                return false;
            })
                    .ToList();

            Console.WriteLine($"Filtered elements count:\t{filteredData.Count}");
            Console.WriteLine($"Located at:\t{_outputFlightsDatabaseFilePath}");

            SaveFlightDb(filteredData);

        }

        private void ReadFlightsDb()
        {
            try
            {
                var content = File.ReadAllText(_flightsDatabaseFilePath);

                var rootElem = JsonDocument.Parse(content).RootElement;

                if (rootElem.ValueKind == JsonValueKind.Null)
                    throw new ArgumentNullException(nameof(rootElem));

                var flights = rootElem
                    .EnumerateObject()
                    .First()
                    .Value
                    .Deserialize<List<Flight>>(new JsonSerializerOptions()
                    {
                        Converters = {
            new TimeSpanJsonConverter(),
                        },
                    });

                _flightSchedule.Clear();
                _flightSchedule.AddRange(flights);
            }
            catch (FormatException formatException)
            {
                Console.WriteLine("Wrong JSON-data format!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

        }

        private void SaveFlightDb(List<Flight> filteredFlightsInfo = null)
        {

            var jsonText = JsonSerializer.Serialize(
                filteredFlightsInfo ?? _flightSchedule,
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault |
                                         System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            });

            File.WriteAllText(_outputFlightsDatabaseFilePath, jsonText);

        }
    }
}