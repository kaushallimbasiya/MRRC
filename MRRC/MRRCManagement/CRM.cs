using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    public class CRM
    {
        private List<Customer> customerlist = new List<Customer>();
        public string crmFile { get; set; }
        public CRM()
        {
            customerlist = new List<Customer>();
        }
        public CRM(List<Customer> customers)
        {
            this.customerlist = new List<Customer>(customers);
        }
        public List<Customer> GetCustomers()
        {
            return customerlist;
        }
        public void AddCustomer(Customer customer)
        {
            customerlist.Add(customer);
        }
        public void RemoveCustomer(Customer customer)
        {
            customerlist.Remove(customer);
        }
        public Customer GetCustomer(int CID)
        {
            foreach (Customer customer in customerlist)
            {
                if (customer.CustomerID == CID)
                {
                    return customer;
                }
            }
            return null;
        }

        public void LoadFromFile()
        {
            System.IO.StreamReader fileReader = new System.IO.StreamReader(crmFile);

            string line;

            //read and ignore the header
            fileReader.ReadLine();

            while ((line = fileReader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                Customer newCustomer = new Customer(int.Parse(parts[0]), parts[1], parts[2], parts[3],(Gender)Enum.Parse(typeof(Gender), parts[4]),parts[5]);
                customerlist.Add(newCustomer);
            }
            fileReader.Close();

        }
        public void writeToFile()
        {
            using (System.IO.StreamWriter file1 = new StreamWriter(@crmFile))
            {

                file1.WriteLine("CustomerID, Title, FirstName, LastName, Gender, DOB");

                foreach (Customer c in customerlist)
                {
                    file1.WriteLine(c.ToCSVString());
                }
                file1.Close();
            }
        }


    }
}
