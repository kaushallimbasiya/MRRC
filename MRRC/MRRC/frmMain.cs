using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MRRCManagement;
namespace MRRC
{
    public partial class frmMain : Form
    {
        private CRM crm;
        private Fleet fleet;
        private Customer selectedCustomer;
        private Vehicle selectedVehicle;
        List<Customer> customerlist = new List<Customer>();
        int cid;
        public frmMain()
        {
            InitializeComponent();
            SetupCRM();
            loadCustomersToGrid();
            setupFleet();
            loadVehiclesToGrid();
            loadRentalsToGrid();

        }
        private void loadRentalsToGrid()
        {
            dataGridViewRentalReport.Rows.Clear();  
            foreach (KeyValuePair<string, int> pair in fleet.rentals)
            {
                string reg = pair.Key.ToString();
                selectedVehicle = fleet.GetVehicle(reg);
                dataGridViewRentalReport.Rows.Add(pair.Key.ToString(),pair.Value.ToString(),selectedVehicle.dailyRate.ToString());
            }
            label26.Text += fleet.rentals.Count;
            label27.Text+= dataGridViewRentalReport.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToInt32(t.Cells[2].Value));
            // dataGridViewRentalReport.DataSource = fleet.rentals;
        }
        private void loadVehiclesToGrid()
        {
            dataGridView1.Rows.Clear();
            foreach(Vehicle v in fleet.GetVehicles())
            {
                dataGridView1.Rows.Add(new string[] { v.vehicleRego.ToString(), v.make.ToString(), v.model.ToString(), v.year.ToString(), v.vehicleClass.ToString(), v.numSeats.ToString(), v.transmission.ToString(), v.fuel.ToString(), v.GPS.ToString(), v.sunRoof.ToString(), v.colour.ToString(), v.dailyRate.ToString() });

            }
        }
        private void loadCustomersToGrid()
        {
            dgCustomers.Rows.Clear();
            foreach (Customer cust in crm.GetCustomers())
            {
                dgCustomers.Rows.Add(new string[] { cust.CustomerID.ToString(),cust.Title, cust.FirstName, cust.LastName, cust.DateOfBirth, cust.G.ToString() });
            }
            selectedCustomer = crm.GetCustomer(int.Parse(dgCustomers.Rows[0].Cells[0].Value.ToString()));
        }
        private void setupFleet()
        {
            fleet = new Fleet();
            fleet.fleetFile = @"..\..\..\Data\fleet.csv";
            fleet.rentalFile = @"..\..\..\Data\rentals.csv";
            fleet.LoadFromFile();
        }
        private void SetupCRM()
        {
            crm = new CRM();
            crm.crmFile = @"..\..\..\Data\customer.csv";
            crm.LoadFromFile();
            
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 20; i++)
                domainUpDownNumberofSeats.Items.Add(i);
            domainUpDownNumberofSeats.SelectedIndex = 3;

            foreach(Customer c in crm.GetCustomers())
            {
                string s = c.CustomerID.ToString() + " - " + c.Title.ToString() + " " + c.FirstName.ToString() + " " + c.LastName.ToString();
                comboBox1.Items.Add(s);
            }
        }

        private void dgCustomers_SelectionChanged(object sender, EventArgs e)
        {
            int rowsCount = dgCustomers.SelectedRows.Count;
            if (rowsCount==0||rowsCount>1)
            {
                selectedCustomer = null;
            }
            else
            {
                int selectedId = int.Parse(dgCustomers.SelectedRows[0].Cells[0].Value.ToString());
                selectedCustomer = crm.GetCustomer(selectedId);
            }
        }

        private void btnRemoveCustomer_Click(object sender, EventArgs e)
        {
            if (selectedCustomer == null)
            {
                MessageBox.Show("Please select a customer to remove", "Select", MessageBoxButtons.OKCancel);
            }
            else
            {
                if (fleet.rentals.ContainsValue(selectedCustomer.CustomerID))
                {
                    MessageBox.Show("This customer has taken a vehicle in Rent", "Can't Remove", MessageBoxButtons.OKCancel);
                }
                else
                {
                    crm.RemoveCustomer(selectedCustomer);
                    loadCustomersToGrid();
                }
            }
        }

        private void btnModifyCustomer_Click(object sender, EventArgs e)
        {
            groupBoxCustomers.Visible = true;
            groupBoxCustomers.Text = "Modify Customer";
            txtId.Enabled = false;
            if(selectedCustomer==null)
            {
                selectedCustomer = crm.GetCustomer(int.Parse(dgCustomers.Rows[0].Cells[0].Value.ToString()));
            }
            txtId.Text = selectedCustomer.CustomerID.ToString();
            txtFirstName.Text = selectedCustomer.FirstName;
            comboTitle.SelectedItem = selectedCustomer.Title.ToString();
            txtLastName.Text = selectedCustomer.LastName;
            txtDOB.Text = selectedCustomer.DateOfBirth;
            cboGender.SelectedIndex = (int)selectedCustomer.G;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtDOB.Text = "";
            cboGender.SelectedIndex = -1;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (groupBoxCustomers.Text == "Modify Customer")
            {
                selectedCustomer.FirstName = txtFirstName.Text;
                selectedCustomer.LastName = txtLastName.Text;
                selectedCustomer.DateOfBirth = txtDOB.Text;
                selectedCustomer.G = (Gender)Enum.Parse(typeof(Gender), cboGender.Text);

            }
            else
            {
                if (crm.GetCustomers().Any(x => x.CustomerID == int.Parse(txtId.Text.ToString())))
                {
                    MessageBox.Show("Customer ID already exists");
                }
                else
                {
                    Customer newCustomer = new Customer(int.Parse(txtId.Text), comboTitle.SelectedItem.ToString(), txtFirstName.Text, txtLastName.Text, (Gender)Enum.Parse(typeof(Gender), cboGender.Text), txtDOB.Text);
                    crm.AddCustomer(newCustomer);
                    MessageBox.Show("New Customer Added", "New Customer", MessageBoxButtons.OKCancel);
                }
            }


            loadCustomersToGrid();

            txtId.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtDOB.Text = "";
            cboGender.SelectedIndex = -1;
        }
        
        //private void frmMain_Leave(object sender, EventArgs e)
        //{
        //    crm.writeToFile();
        //    fleet.writeToFile();
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            groupBoxCustomers.Text = "Add Customer";
            groupBoxCustomers.Visible = true;
           
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            crm.writeToFile();
            fleet.writeToFile();
        }

        private void btnModifyVehicle_Click(object sender, EventArgs e)
        {
            groupBoxModifyVehicleDetails.Visible = true;
            groupBoxModifyVehicleDetails.Text = "Modify Vehicle";
            if(selectedVehicle==null)
            {
                selectedVehicle = fleet.GetVehicle(dataGridView1.Rows[0].Cells[0].Value.ToString());
            }

            txtRegno.Enabled = false;
            txtRegno.Text = selectedVehicle.vehicleRego;
            txtMake.Text = selectedVehicle.make;
            txtModel.Text = selectedVehicle.model;
            txtMakingYear.Text = selectedVehicle.year.ToString();
            domainUpDownNumberofSeats.SelectedItem = selectedVehicle.numSeats.ToString();
            //txtNoofSeats.Text = selectedVehicle.numSeats.ToString();
            comboFuelType.Text = selectedVehicle.fuel.ToString();

            if (bool.Parse(selectedVehicle.GPS.ToString() )== true) checkBox1.Checked = true; else checkBox1.Checked = false;
            if(bool.Parse(selectedVehicle.sunRoof.ToString())==true)checkBox1.Checked = true;else checkBox2.Checked = false;
            comboTransmission.Text = selectedVehicle.transmission.ToString();
            comboVehicleClass.Text = selectedVehicle.vehicleClass.ToString();
            txtColor.Text = selectedVehicle.colour.ToString();
            txtDailyRate.Text = selectedVehicle.dailyRate.ToString();

        }

        private void btnAddVehicle_Click(object sender, EventArgs e)
        {
            groupBoxModifyVehicleDetails.Visible = true;
            groupBoxModifyVehicleDetails.Text = "Add Vehicle";
            comboTransmission.SelectedItem = "Automatic";
            comboFuelType.SelectedItem = "Petrol";
            txtColor.Text = "Black";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int rowsCount = dataGridView1.SelectedRows.Count;
            if (rowsCount == 0 || rowsCount > 1)
            {
                selectedVehicle = null;
            }
            else
            {
                string selectedregno = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                selectedVehicle = fleet.GetVehicle(selectedregno);
            }
        }
        public int dataValidation()
        {
            int f = 0;
            if (double.Parse(txtDailyRate.Text.ToString()) < 0)
            {
                MessageBox.Show("Daily rate cannot be less than 0", "Error input", MessageBoxButtons.OKCancel);
                f = 1;
            }
            if (fleet.GetVehicle(txtRegno.Text.ToString()) != null)
            {
                MessageBox.Show("Vehicle Reg No already exists in the Fleet", "Error", MessageBoxButtons.OKCancel);
                f = 1;
            }
            if (int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString()) < 1 && int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString()) > 20)
            {
                MessageBox.Show("Invalid number of seats! Please select between 1 and 20", "Error", MessageBoxButtons.OKCancel);
                f = 1;
            }
            return f;
        }
        private void btnSubmitVehicle_Click(object sender, EventArgs e)
        {
            
            
               // if (comboVehicleClass.SelectedItem.ToString() == "Economic")
                 //   comboTransmission.SelectedItem = "Automatic";
                if (groupBoxModifyVehicleDetails.Text == "Modify Vehicle")
                {
                    selectedVehicle.vehicleRego = txtRegno.Text.ToString();
                    selectedVehicle.make = txtMake.Text.ToString();
                    selectedVehicle.model = txtModel.Text.ToString();
                    selectedVehicle.year = int.Parse(txtMakingYear.Text.ToString());
                    selectedVehicle.numSeats = int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString());
                    selectedVehicle.transmission = (Vehicle.transmissionType)Enum.Parse(typeof(Vehicle.transmissionType), comboTransmission.SelectedItem.ToString());
                    selectedVehicle.fuel = (Vehicle.FuelType)Enum.Parse(typeof(Vehicle.FuelType), comboFuelType.SelectedItem.ToString());
                    if (checkBox2.Checked)
                    {  selectedVehicle.sunRoof = true;    }
                    else { selectedVehicle.sunRoof = false; }
                    if (checkBox1.Checked) { selectedVehicle.GPS = true; }
                    else { selectedVehicle.GPS = false; }
                    //selectedVehicle.GPS = bool.Parse(comboGPS.Text.ToString());
                    selectedVehicle.colour = txtColor.Text.ToString();
                    selectedVehicle.dailyRate = double.Parse(txtDailyRate.Text.ToString());
                    selectedVehicle.vehicleClass = (Vehicle.VehicleClass)Enum.Parse(typeof(Vehicle.VehicleClass), comboVehicleClass.SelectedItem.ToString());
                MessageBox.Show("Vehicle details updated", "Updated", MessageBoxButtons.OKCancel);
                }
                else
                {
                if (dataValidation() == 0)
                {
                    if (checkBox1.Checked == true) { comboGPS.SelectedItem = "True"; } else { comboGPS.SelectedItem = "False"; }
                    if (checkBox1.Checked == true) { comboSunroof.SelectedItem = "True"; } else { comboSunroof.SelectedItem = "False"; }
                    Vehicle v = new Vehicle(txtRegno.Text.ToString(), (Vehicle.VehicleClass)Enum.Parse(typeof(Vehicle.VehicleClass), comboVehicleClass.SelectedItem.ToString()), txtMake.Text.ToString(), txtModel.Text.ToString(), int.Parse(txtMakingYear.Text.ToString()), int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString()), (Vehicle.transmissionType)Enum.Parse(typeof(Vehicle.transmissionType), comboTransmission.SelectedItem.ToString()), (Vehicle.FuelType)Enum.Parse(typeof(Vehicle.FuelType), comboFuelType.SelectedItem.ToString()), bool.Parse(comboSunroof.SelectedItem.ToString()), bool.Parse(comboGPS.SelectedItem.ToString()), double.Parse(txtDailyRate.Text.ToString()), txtColor.Text.ToString());
                    fleet.AddVehicle(v);
                    clearVehicleDetails();
                }
                   
                       
                }
                loadVehiclesToGrid();
                groupBoxModifyVehicleDetails.Visible = false;
            
        }
        public void clearVehicleDetails()
        {
            txtRegno.Text = "";
            txtMake.Text = "";
            txtModel.Text = "";
            txtMakingYear.Text = "";
            txtDailyRate.Text = "";
            comboFuelType.SelectedIndex = -1;
            comboGPS.SelectedIndex = -1;
            comboSunroof.SelectedIndex = -1;
            comboTransmission.SelectedIndex = -1;
            comboTransmission.SelectedIndex = -1;
            comboTransmission.SelectedIndex = -1;
            txtColor.Text = "";
            checkBox1.Checked = false;
            checkBox1.Checked = false;
        }
        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDownNumberofSeats_SelectedItemChanged(object sender, EventArgs e)
        {
            if(int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString())<1 && int.Parse(domainUpDownNumberofSeats.SelectedItem.ToString())>20)
            {
                MessageBox.Show("Invalid number of seats! Please select between 1 and 20", "Error", MessageBoxButtons.OKCancel);
            }

        }

        private void comboVehicleClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboVehicleClass.SelectedItem.ToString())
            {
                case "Economic": txtDailyRate.Text = "50";break;
                case "Family": txtDailyRate.Text = "80"; break;
                case "Luxury": checkBox1.Checked = true; checkBox2.Checked = true;  txtDailyRate.Text = "120"; break;
                case "Commercial":comboFuelType.SelectedItem = "Diesel";txtDailyRate.Text = "130"; break;

            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            btnSearch.Enabled = true;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            dataGridViewSearchResults.Rows.Clear();

            int from = Convert.ToInt32(numericUpDownFrom.Value);
            int to= Convert.ToInt32(numericUpDownTo.Value);
            dataGridViewSearchResults.Visible = true;
            foreach(Vehicle v in fleet.GetVehicles())
            {
                //if(fleet.rentals.Keys.)

                if (int.Parse(v.dailyRate.ToString()) >= from && int.Parse(v.dailyRate.ToString()) <= to)
                {
                    if (!fleet.rentals.ContainsKey(v.vehicleRego.ToString()))
                    {
                        dataGridViewSearchResults.Rows.Add(new string[] { v.vehicleRego.ToString(), v.make.ToString(), v.model.ToString(), v.year.ToString(), v.vehicleClass.ToString(), v.numSeats.ToString(), v.transmission.ToString(), v.fuel.ToString(), v.GPS.ToString(), v.sunRoof.ToString(), v.colour.ToString(), v.dailyRate.ToString() });
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<Vehicle> vehicles = fleet.GetVehicles();
            
            dataGridViewSearchResults.Rows.Clear();
            dataGridViewSearchResults.Visible = true;
            if (textBox1.Text.ToLower() == "any")
            {
                foreach (Vehicle v1 in vehicles)
                {
                    if (!fleet.rentals.ContainsKey(v1.vehicleRego.ToString()))
                    {
                        dataGridViewSearchResults.Rows.Add(new string[] { v1.vehicleRego.ToString(), v1.make.ToString(), v1.model.ToString(), v1.year.ToString(), v1.vehicleClass.ToString(), v1.numSeats.ToString(), v1.transmission.ToString(), v1.fuel.ToString(), v1.GPS.ToString(), v1.sunRoof.ToString(), v1.colour.ToString(), v1.dailyRate.ToString() });
                    }
                }
            }
            else
            {
                string[] vals = textBox1.Text.Split(' '); int f = 0;
                switch (vals.Count())
                {
                    case 1: foreach(Vehicle v1 in vehicles)
                        {
                            if (FindVehicle(v1, textBox1.Text.ToLower()))
                            {
                                dataGridViewSearchResults.Rows.Add(new string[] { v1.vehicleRego.ToString(), v1.make.ToString(), v1.model.ToString(), v1.year.ToString(), v1.vehicleClass.ToString(), v1.numSeats.ToString(), v1.transmission.ToString(), v1.fuel.ToString(), v1.GPS.ToString(), v1.sunRoof.ToString(), v1.colour.ToString(), v1.dailyRate.ToString() });
                                f = 1;
                            }
                        }
                        if(f==0)
                        {
                            label25.Visible = true;
                            dataGridViewSearchResults.Visible = false;
                        }
                        break;
                    case 3: 
                            foreach(Vehicle v1 in vehicles)
                            {
                                if (vals[1].ToLower() == "or")
                                {
                                    if (FindVehicle(v1, vals[0]) || FindVehicle(v1, vals[2]))
                                    {
                                        dataGridViewSearchResults.Rows.Add(new string[] { v1.vehicleRego.ToString(), v1.make.ToString(), v1.model.ToString(), v1.year.ToString(), v1.vehicleClass.ToString(), v1.numSeats.ToString(), v1.transmission.ToString(), v1.fuel.ToString(), v1.GPS.ToString(), v1.sunRoof.ToString(), v1.colour.ToString(), v1.dailyRate.ToString() });
                                        f = 1;
                                    }
                                }
                                else if(vals[1].ToLower() == "and")
                            {
                                if (FindVehicle(v1, vals[0]) && FindVehicle(v1, vals[2]))
                                {
                                    dataGridViewSearchResults.Rows.Add(new string[] { v1.vehicleRego.ToString(), v1.make.ToString(), v1.model.ToString(), v1.year.ToString(), v1.vehicleClass.ToString(), v1.numSeats.ToString(), v1.transmission.ToString(), v1.fuel.ToString(), v1.GPS.ToString(), v1.sunRoof.ToString(), v1.colour.ToString(), v1.dailyRate.ToString() });
                                    f = 1;
                                }
                            }
                            }
                        
                        if (f == 0)
                        {
                            label25.Visible = true;
                            dataGridViewSearchResults.Visible = false;
                        }
                        break;
                    case 5:
                        
                            bool result = false;
                            for (int i=0;i<vals.Count()-1;i+=3)
                            {

                            foreach (Vehicle v1 in vehicles)
                            {
                                result = FindVehicle(v1, vals[i]);
                                if (vals[i + 1].ToLower() == "and")
                                {
                                    result = result && FindVehicle(v1, vals[i + 2]);
                                }
                                else if (vals[i + 1].ToLower() == "or")
                                {
                                    result = result || FindVehicle(v1, vals[i + 2]);
                                }

                                if (result == true)
                                {
                                    dataGridViewSearchResults.Rows.Add(new string[] { v1.vehicleRego.ToString(), v1.make.ToString(), v1.model.ToString(), v1.year.ToString(), v1.vehicleClass.ToString(), v1.numSeats.ToString(), v1.transmission.ToString(), v1.fuel.ToString(), v1.GPS.ToString(), v1.sunRoof.ToString(), v1.colour.ToString(), v1.dailyRate.ToString() });
                                    f = 1;
                                }
                            }
                        }
                        if (f == 0)
                        {
                            label25.Visible = true;
                            dataGridViewSearchResults.Visible = false;
                        }

                        break;
                    default:MessageBox.Show("Query not valid", "Error", MessageBoxButtons.OKCancel);
                        break;
                    
                }
            }
            
           // (dataGridViewSearchResults.DataSource as DataTable).DefaultView.RowFilter = string.Format("vcolor like '{0}%' OR vclass '{0}%' OR vseats like '{0}%'", textBox1.Text);
        }
        public bool FindVehicle(Vehicle v,string str)
        {
            
            if (textBox1.Text.ToString().ToLower() == "gps")
            {
                //v = fleet.GetVehicles().Find(item => item.GPS == true);
                //dataGridViewSearchResults.Rows.Add(new string[] { v.vehicleRego.ToString(), v.make.ToString(), v.model.ToString(), v.year.ToString(), v.vehicleClass.ToString(), v.numSeats.ToString(), v.transmission.ToString(), v.fuel.ToString(), v.GPS.ToString(), v.sunRoof.ToString(), v.colour.ToString(), v.dailyRate.ToString() });
                return true;
            }
            else if (textBox1.Text.ToLower() == "sunroof")
            {
                return true;  }
            else
            {
                if (v.vehicleRego.ToLower() == str || v.colour.ToLower() == str || v.dailyRate.ToString() == str || v.fuel.ToString().ToLower() == str || v.make.ToLower() == str || v.model.ToLower() == str || v.numSeats.ToString() == str || v.transmission.ToString().ToLower() == str || v.vehicleClass.ToString() == str || v.year.ToString() == str)
                    return true;
                    //v = fleet.GetVehicles().Find(item => item.colour.ToLower() == textBox1.Text.ToLower() || item.vehicleClass.ToString().ToLower() == textBox1.Text.ToLower() || item.vehicleRego.ToLower() == textBox1.Text.ToLower() || item.model.ToLower() == textBox1.Text.ToLower() || item.make.ToLower() == textBox1.Text.ToLower() || textBox1.Text.StartsWith(item.numSeats.ToString()) || item.transmission.ToString().ToLower() == textBox1.Text.ToLower() || item.fuel.ToString().ToLower() == textBox1.Text.ToLower());
                //dataGridViewSearchResults.Rows.Add(new string[] { v.vehicleRego.ToString(), v.make.ToString(), v.model.ToString(), v.year.ToString(), v.vehicleClass.ToString(), v.numSeats.ToString(), v.transmission.ToString(), v.fuel.ToString(), v.GPS.ToString(), v.sunRoof.ToString(), v.colour.ToString(), v.dailyRate.ToString() });
            }
            return false;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnRent_Click(object sender, EventArgs e)
        {
            string selectedregno= dataGridViewSearchResults.Rows[0].Cells[0].Value.ToString();
            if (dataGridViewSearchResults.Rows.Count == 0)
            {
                MessageBox.Show("There is no vehicles available", "Error", MessageBoxButtons.OKCancel);
            }
            else
            {

                try
                {
                    selectedregno = dataGridViewSearchResults.SelectedRows[0].Cells[0].Value.ToString();
                    selectedVehicle = fleet.GetVehicle(selectedregno);
                    double amt = Convert.ToDouble(numericUpDown1.Value) * selectedVehicle.dailyRate;
                    label24.Text += amt.ToString();
                    if (fleet.rentals.ContainsValue(Convert.ToInt32(comboBox1.SelectedItem.ToString().Substring(0,1))))
                    {
                        MessageBox.Show("This customer has already taken a vehicle for rent", "Error", MessageBoxButtons.OKCancel);
                    }
                    else
                    {
                        DialogResult s = MessageBox.Show("Do you want to rent vehicle " + selectedregno + " to customer " + comboBox1.SelectedItem.ToString() + " for a total cost of $" + amt.ToString() + " ?", "Rental Confirmation", MessageBoxButtons.YesNo);
                        if (DialogResult.Yes == s)
                        {
                            fleet.rentals.Add(selectedVehicle.vehicleRego, Convert.ToInt32(comboBox1.SelectedItem.ToString().Substring(0, 1)));
                            dataGridViewRentalReport.Rows.Add(selectedVehicle.vehicleRego.ToString(), comboBox1.SelectedItem.ToString().Substring(0, 1).ToString(), amt.ToString());

                        }
                    }
                }
                catch { MessageBox.Show("Please select a vehicle", "Error", MessageBoxButtons.OKCancel); }
                //Vehicle v = fleet.GetVehicles().Find(Item => Item.vehicleRego == dataGridViewSearchResults.SelectedRows[0].ToString());
                
            }
        }

        private void btnRemoveVehicle_Click(object sender, EventArgs e)
        {
            try { 
               string selectedregno = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            
                Vehicle vehicle = fleet.GetVehicle(selectedregno);
                if (!fleet.rentals.ContainsKey(vehicle.vehicleRego.ToString()))
                {
                    if (this.dataGridView1.SelectedRows.Count > 0)
                    {
                        DialogResult dg = MessageBox.Show("Do you want to remove the selected vehicle?", "Confirmation", MessageBoxButtons.YesNo);
                        if (dg == DialogResult.Yes)
                        {
                            dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                            fleet.RemoveVehicle(vehicle);
                        }
                        //loadVehiclesToGrid();
                    }
                    else
                    {
                        MessageBox.Show("Select the vehicle to delete", "Error", MessageBoxButtons.OKCancel);
                    }
                }
                else
                {
                    MessageBox.Show("This vehicle cannot be deleted as is rented by someone", "Error", MessageBoxButtons.OKCancel);
                }
            }
            catch { MessageBox.Show("Select a vehicle to remove", "Error", MessageBoxButtons.OKCancel); }
        }

        private void btnReturnVehicle_Click(object sender, EventArgs e)
        {
            fleet.rentals.Remove(dataGridViewRentalReport.SelectedRows[0].Cells[0].Value.ToString());
            dataGridViewRentalReport.Rows.RemoveAt(dataGridViewRentalReport.SelectedRows[0].Index);
            label26.Text += fleet.rentals.Count;
            label27.Text += dataGridViewRentalReport.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToInt32(t.Cells[2].Value));
        }
    }
}
