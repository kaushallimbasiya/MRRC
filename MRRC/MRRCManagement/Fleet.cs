using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace MRRCManagement
{
    public class Fleet
    {
        private List<Vehicle> vehicles;
        public Dictionary<string, int> rentals = new Dictionary<string, int>();
        public string fleetFile { get; set; }
        public string rentalFile { get; set; }
        public Fleet()
        {
            vehicles = new List<Vehicle>();
        }
        public Fleet(List<Vehicle> vehicles)
        {
            this.vehicles = new List<Vehicle>(vehicles);
        }
        public void AddVehicle(Vehicle v)
        {
            vehicles.Add(v);
        }
        public void RemoveVehicle(Vehicle v)
        {
            vehicles.Remove(v);
        }
        public Vehicle GetVehicle(string regno)
        {
            foreach (Vehicle v in vehicles)
            {
                if (v.vehicleRego== regno)
                {
                    return v;
                }
            }
            return null;
        }
        public void LoadFromFile()
        {
            //load vehicles
            try
            {
                var lines = File.ReadAllLines(fleetFile).Skip(1);
                //var lines = System.IO.File.ReadAllLines(fleetFile).Skip(1).TakeWhile(t => t != null);
                
                foreach (string item in lines)
                {
                    var values = item.Split(',');
                    //need to check which type of vehicle is it
                    
                    vehicles.Add(new Vehicle(values[0],(Vehicle.VehicleClass)Enum.Parse(typeof(Vehicle.VehicleClass),values[3]),values[1],values[2], int.Parse(values[4]),int.Parse(values[5]),(Vehicle.transmissionType)Enum.Parse(typeof(Vehicle.transmissionType),values[6]),(Vehicle.FuelType)Enum.Parse(typeof(Vehicle.FuelType),values[7]),bool.Parse(values[8]),bool.Parse(values[9]),double.Parse(values[10]),values[11]));
                   
                }
            }
            catch
            {
                
            }
            //load rentals
            try
            {
                using (StreamReader reader = new StreamReader(rentalFile))
                {
                    string line = reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        rentals.Add(parts[0], int.Parse(parts[1]));

                    }
                }
            }
            catch { }
        }
        public void writeToFile()
        {
            using (System.IO.StreamWriter file1 = new StreamWriter(fleetFile))
            {

                file1.WriteLine("Rego,Make,Model,VehicleClass,Year,NumSeats,Transmission,Fuel,GPS,SunRoof,DailyRate,Colour");

                foreach (Vehicle v in vehicles)
                {
                    file1.WriteLine(v.ToCSVString());
                }
                file1.Close();
            }
            using (System.IO.StreamWriter file1 = new StreamWriter(rentalFile))
            {

                file1.WriteLine("Vehicel,Customer");

                foreach (var v in rentals)
                {
                    file1.WriteLine(v.Key.ToString()+","+v.Value.ToString());
                }
                file1.Close();
            }
        }

        public List<Vehicle> GetVehicles() { return vehicles; }
    }
}
