using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRRCManagement;
namespace ConsoleApp
{
    class Program
    {
        //Vehicle obj = new Vehicle();
        
        public static List<Customer> customerlist = new List<Customer>();

        static void Main(string[] args)
        {
            string filename = "D:\\Teaching\\my work\\MRRC\\Data\\customer.csv";
            //var lines;
            int custID = 3, f = 0; Gender g = Gender.Female;
            Customer cs = new Customer(custID,"Ms","Rekha","G",g,"01-01-1983\n");
            
            try
            {
                f = 0;
                var lines = System.IO.File.ReadAllLines(filename).Skip(1).TakeWhile(t => t != null);
                Console.WriteLine("Details of Customers");
                Console.WriteLine("====================");
                foreach (string item in lines)
                {
                    var values = item.Split(',');
                    //need to check which type of vehicle is it
                    if (int.Parse(values[0]) == custID)
                    {
                        f = 1;
                        break;

                    }
                    customerlist.Add(new Customer(int.Parse(values[0]), values[1], values[2], values[3], (Gender)Enum.Parse(typeof(Gender), values[4]), values[5]));

                }
                if (f == 1)
                {
                    Console.WriteLine("This customer ID already exists");
                }
                else
                {
                    string csvstring;
                    //csvstring = cs.ToCSVString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
                    //File.AppendAllLines(filename, csvstring.ToString());
                    csvstring = cs.ToCSVString();
                    File.AppendAllText(filename, csvstring);
                    //File.AppendAllText(filename, csvstring);
                    Console.WriteLine("Customer details added successfully");
                    customerlist.Add(new Customer(custID, "Ms", "Rekha", "G", Gender.Female, "01-01-1983"));
                }

            }
            catch
            {
                Console.WriteLine("Cannot open the file");
                
            }

            
                
            
            
            foreach (Customer item in customerlist)
                Console.WriteLine(item);
            Console.ReadLine();
        }
    }
}
    