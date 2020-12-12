using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    public class Vehicle
    {
        public string vehicleRego { get; set; }
        public string make{get;set;}
        public string model{get;set;}
        public VehicleClass vehicleClass { get; set; }
        public int year { get; set; }
        public int numSeats { get; set; }
        public transmissionType transmission { get; set; }
        public FuelType fuel { get; set; }
        public bool GPS { get; set; }
        public bool sunRoof { get; set; }
        public double dailyRate { get; set; }
        public string colour { get; set; }

        public enum FuelType
        {
            Diesel, Petrol
        }
        public enum transmissionType
        {
            Manual,Automatic, Manual_or_automatic
        }
        public enum VehicleClass
        {
            Economy, Family, Luxury, Commercial
        }
        public Vehicle(string rego, VehicleClass vclass, string make1, string model1, int year1)
        {
            vehicleRego = rego;
            vehicleClass = vclass;
            make = make1;
            model = model1;
            year = year1;
            numSeats = 4;
            transmission = transmissionType.Automatic;
            fuel = FuelType.Diesel;
            GPS = false;
            sunRoof = false;
            colour = "black";
            if (vclass == VehicleClass.Economy)
            {
                transmission = transmissionType.Automatic;
                dailyRate = 50;
            }
            else if(vclass == VehicleClass.Family)
            {
                transmission = transmissionType.Manual_or_automatic;
                dailyRate = 80;
            }
            else if(vclass==VehicleClass.Luxury)
            {
                GPS = true;
                sunRoof = true;
                dailyRate = 120;
            }
            else if(vclass==VehicleClass.Commercial)
            {
                make = "diesel engine";
                dailyRate = 130;
            }
        }
        public Vehicle(string rego, VehicleClass vclass, string make1, string model1, int year1,int seats,transmissionType trtype,FuelType ftype,bool gps1,bool sroof,double rate,string clr)
        {
            vehicleRego = rego;
            vehicleClass = vclass;
            make = make1;
            model = model1;
            year = year1;
            numSeats = seats;
            transmission = trtype;
            fuel = ftype;
            GPS = gps1;
            sunRoof = sroof;
            dailyRate = rate;
            colour = clr;
        }
        public string ToCSVString()
        {
            // creates an array of the user's values
            string[] values = {vehicleRego,make,model,vehicleClass.ToString(),year.ToString(),numSeats.ToString(),transmission.ToString(),fuel.ToString(),GPS.ToString(),sunRoof.ToString(),dailyRate.ToString(),colour };
            // creates a new line
            string line = String.Join(",", values);
            // returns the line
            
            return line;
        }
        public override string ToString()
        {
            string str = vehicleRego.ToString() + make.ToString() + model.ToString() + vehicleClass.ToString() +year.ToString() + numSeats.ToString()+transmission.ToString()+fuel.ToString()+GPS.ToString()+sunRoof.ToString()+dailyRate.ToString()+colour.ToString();
            return str;
        }
        public List<string> GetAttributeList()
        {
            List<string> vs = new List<string>();
            vs.Add(vehicleRego.ToString());
            vs.Add(make.ToString());
            vs.Add(model.ToString());
            vs.Add(vehicleClass.ToString());
            vs.Add(year.ToString());
            vs.Add(vehicleClass.ToString());
            vs.Add(numSeats.ToString() + " - Seater");
            vs.Add(transmission.ToString());
            vs.Add(fuel.ToString());
            if (GPS == true)
                vs.Add("GPS");
            else
                vs.Add("No GPS");
            if (sunRoof == true)
                vs.Add("Sunroof");
            else
                vs.Add("No Sunroof");
            vs.Add(dailyRate.ToString());
            vs.Add(colour.ToString());

            return vs;
        }
    }

   //class Economy : Vehicle
   // {
   //     public Economy(string rego, VehicleClass vclass, string make1, string model1, int year1) : base(rego, vclass, make1, model1, year1)
   //     {
   //     }
   //     public Economy(string rego, VehicleClass vclass, string make1, string model1, int year1, int seats, transmissionType trtype, FuelType ftype, bool gps1, bool sroof, double rate, string clr) : base(rego, vclass, make1, model1, year1, seats, trtype, ftype, gps1, sroof, rate, clr)
   //     {

   //     }

   // }

   //  class Family : Vehicle
   // {
   //     public Family(string rego, VehicleClass vclass, string make1, string model1, int year1, int seats, transmissionType trtype, FuelType ftype, bool gps1, bool sroof, double rate, string clr):base(rego, vclass, make1, model1,  year1,  seats,  trtype,  ftype,  gps1,  sroof,  rate,  clr)
   //     {

   //     }
   //     public Family(string rego, VehicleClass vclass, string make1, string model1, int year1) : base(rego, vclass, make1, model1, year1)
   //     {
   //     }
   // }
   // class Luxury : Vehicle
   // {
   //     public Luxury(string rego, VehicleClass vclass, string make1, string model1, int year1, int seats, transmissionType trtype, FuelType ftype, bool gps1, bool sroof, double rate, string clr) : base(rego, vclass, make1, model1, year1, seats, trtype, ftype, gps1, sroof, rate, clr)
   //     {

   //     }
   //     public Luxury(string rego, VehicleClass vclass, string make1, string model1, int year1) : base(rego, vclass, make1, model1, year1)
   //     {
   //     }
   // }
   // class Commercial : Vehicle
   // {
   //     public Commercial(string rego, VehicleClass vclass, string make1, string model1, int year1, int seats, transmissionType trtype, FuelType ftype, bool gps1, bool sroof, double rate, string clr) : base(rego, vclass, make1, model1, year1, seats, trtype, ftype, gps1, sroof, rate, clr)
   //     {

   //     }
   //     public Commercial(string rego, VehicleClass vclass, string make1, string model1, int year1) : base(rego, vclass, make1, model1, year1)
   //     {
   //     }
   // }
}
