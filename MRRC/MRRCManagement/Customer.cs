using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    public enum Gender
    {
        Male, Female
    }
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender G { get; set; }
        public string DateOfBirth { get; set; }

        public Customer(int custID, string tit, string first, string
                        last, Gender g, string date)
        {
            CustomerID = custID;
            Title = tit;
            FirstName = first;
            LastName = last;
            G = g;
            DateOfBirth = date;

        }
        public string ToCSVString()
        {
            // creates an array of the user's values
            string[] values = { CustomerID.ToString(),Title, FirstName,
                        LastName,G.ToString(),DateOfBirth };
            // creates a new line
            String line = String.Join(",", values);
            // writes the line
            //sw.WriteLine(line);
            return line;
        }
        public override string ToString()
        {
            string str = CustomerID.ToString() + Title + FirstName + LastName + G.ToString() + DateOfBirth;
            return str;
        }
    }
}
