using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CDLinkedList;

//Stephanie Goedde
//Programming 2
//Accounts Form
//11-28-11

namespace Accounts
{
    public partial class frmAccounts : Form
    {
        private acctList<Customer>.List clients1 = new acctList<Customer>.List();

        private StreamReader accountFileReader;
        private FileStream input;
        private StreamWriter accountFileWriter;
        private FileStream output;

        private bool isDirtyBoolean = false; //determines if changes made to list

        string strFirst, strLast, strAddress, strCity, strState, strZip, strHomePhone, strCellPhone;
        int intAccount;
        decimal decBalance;
        string strType = "";

        public frmAccounts()
        {
            InitializeComponent();
        }

        //method to clear all text boxes
        public void ClearTextBoxes()
        {
            foreach (Control ctlObj in Controls)
                if (ctlObj is TextBox)
                    ctlObj.Text = string.Empty;
        }

        //method to disable all buttons
        public void DisableButtons()
        {
            foreach (Control ctlObj in Controls)
                if (ctlObj is Button)
                    ctlObj.Enabled = false;
        }

        //reads all data from file and adds to list
        public void Read()
        {
            //loop will run until no more data in file
            while (accountFileReader.EndOfStream != true)
            {
                //sets each line to variable
                intAccount = int.Parse(accountFileReader.ReadLine());
                strFirst = accountFileReader.ReadLine();
                strLast = accountFileReader.ReadLine();
                strAddress = accountFileReader.ReadLine();
                strCity = accountFileReader.ReadLine();
                strState = accountFileReader.ReadLine();
                strZip = accountFileReader.ReadLine();
                strHomePhone = accountFileReader.ReadLine();
                strCellPhone = accountFileReader.ReadLine();
                decBalance = decimal.Parse(accountFileReader.ReadLine());
                
                //sends data to customer class
                Customer custCollection = new Customer(intAccount, strFirst, strLast, strAddress, strCity, strState, 
                    strZip, strHomePhone, strCellPhone, decBalance);

                //add each increment of customer class to list 
                clients1.Add(custCollection);
            }//end loop                
        }

        //enables all controls on form
        private void EnableControls(bool turnOn)
        {
            //loops until all controls have been checked 
            for (int i = 0; i < this.Controls.Count; i++)
            {
                Control myControl = Controls[i];
                if (turnOn)
                {
                    myControl.Enabled = true;
                }
                else
                {
                    myControl.Enabled = false;
                }
            }//end loop
        }        

        //happens when form loads
        private void frmAccounts_Load(object sender, EventArgs e)
        {
            EnableControls(false); //disables controls
            btnOpen.Enabled = true; //open button enabled
        }

        //open button click event
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string strFileName;

            //initializes open file dialog box
            OpenFileDialog fileChooser = new OpenFileDialog();

            //checks if file exists and is a text file
            //sets to text file if not already
            fileChooser.CheckFileExists = true;
            fileChooser.DefaultExt = "txt";
            fileChooser.AddExtension = true;

            //opens dialog box
            DialogResult result = fileChooser.ShowDialog();

            //checks if cancel button is clicked
            if (result != DialogResult.Cancel)
            {
                strFileName = fileChooser.FileName;

                //checks if filename was entered
                if (strFileName == string.Empty || strFileName == null)
                {
                    MessageBox.Show("Invalid File Name.");
                }
                else
                {
                    //initializes filestream and sets to open file and read access
                    input = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                    accountFileReader = new StreamReader(input);

                    try
                    {
                        Read(); //reads file

                        //sets First to current
                        clients1.listPrevious = clients1.First.NodePrevious;
                        clients1.current = clients1.First;
                        clients1.listNext = clients1.First.NodeNext;

                        Display();//puts data in text boxes
                        SaveControls();//disables and endables buttons
                    }
                    //catches if file does not have correct # of variables or wrong type of data
                    catch (FormatException)
                    {
                        MessageBox.Show("Corrupt File. Try different file.");
                        EnableControls(false);
                        btnOpen.Enabled = true;
                    }

                    accountFileReader.Close(); //closes filereader
                }
            }
        }

        //puts data in text boxes per current item in list
        private void Display()
        {
            //displays data from list in text boxes per current
            txtBxAccount.Text = clients1.current.Data.Account.ToString();
            txtBxFirst.Text = clients1.current.Data.FirstName.ToString();
            txtBxLast.Text = clients1.current.Data.LastName.ToString();
            txtBxAddress.Text = clients1.current.Data.Address.ToString();
            txtBxCity.Text = clients1.current.Data.City.ToString();
            txtBxState.Text = clients1.current.Data.State.ToString();
            txtBxZip.Text = clients1.current.Data.Zip.ToString();
            txtBxHomePhone.Text = clients1.current.Data.HomePhone.ToString();
            txtBxCellPhone.Text = clients1.current.Data.CellPhone.ToString();
            txtBxBalance.Text = clients1.current.Data.Balance.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //sets listNext to current
            clients1.listPrevious = clients1.listPrevious.NodeNext;
            clients1.current = clients1.current.NodeNext;
            clients1.listNext = clients1.current.NodeNext;

            Display();//puts in text boxes
            SaveControls();//enables and disables controls            
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            //sets listPrevious to current
            clients1.listPrevious = clients1.listPrevious.NodePrevious;
            clients1.current = clients1.current.NodePrevious;
            clients1.listNext = clients1.current.NodePrevious;

            Display();//puts data in text boxes
            SaveControls();//enables and disables controls
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //clears text boxes and enables text boxes but disables buttons
            ClearTextBoxes();
            EnableControls(true);
            DisableButtons();

            //enables save and cancel
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            txtBxAccount.Focus(); //sets focus

            //sets type to add for switch statement
            strType = "Add";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //enables textboxes and disables buttons
            EnableControls(true);
            DisableButtons();

            //enables save and cancel
            btnSave.Enabled = true;
            btnCancel.Enabled = true;

            txtBxAccount.Focus(); //sets focux

            //sets type to edit for switch statement
            strType = "Edit";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //warning to double check delete intentions
            DialogResult result = MessageBox.Show("Are you sure you want to delete this?", "Warning", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            //if user clicks yes
            if (result == DialogResult.Yes)
            {
                Customer custCollection = new Customer(intAccount, strFirst, strLast, strAddress, strCity, strState,
                    strZip, strHomePhone, strCellPhone, decBalance);

                //remove from list at index
                clients1.Delete(custCollection);

                Display();                
                SaveControls(); //disable and enable
                isDirtyBoolean = true; //changes made to form
            }
            else
            {
                SaveControls(); //disable and enable
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //reset display and buttons
            Display();
            SaveControls();
        }

        //method to disable and enable controls
        private void SaveControls()
        {
            EnableControls(false);
            btnAdd.Enabled = true;
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            btnExit.Enabled = true;
            menuStrip1.Enabled = true;
            
            //determines if current is 1st or last and to enable or disable next and previous buttons
            if (clients1.current == clients1.Last)
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = true;
            }
            else if (clients1.current == clients1.First)
            {
                btnNext.Enabled = true;
                btnPrevious.Enabled = false;
            }
            else
            {
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            int intTest = 0; //test variable

            try
            {
                //get new or changed data from textboxes and set to variables
                intAccount = int.Parse(txtBxAccount.Text);
                strFirst = txtBxFirst.Text;
                strLast = txtBxLast.Text;
                strAddress = txtBxAddress.Text;
                strCity = txtBxCity.Text;
                strState = txtBxState.Text;
                strZip = txtBxZip.Text;
                strHomePhone = txtBxHomePhone.Text;
                strCellPhone = txtBxCellPhone.Text;
                decBalance = decimal.Parse(txtBxBalance.Text);

                //test if each textbox is filled and if not returns a 1
                foreach(Control ctlobj in Controls)
                {
                    if(ctlobj.Text==string.Empty)
                        intTest = 1;
                }

                //if text boxes are null-message displayed
                if (intTest == 1)
                {
                    MessageBox.Show("All fields must be complete.", "Error");
                }
                else
                {
                    //sends data to customer class
                    Customer custCollection = new Customer(intAccount, strFirst, strLast, strAddress, strCity, strState, strZip,
                        strHomePhone, strCellPhone, decBalance);

                    //determines which button pressed and adds or edits accordingly
                    switch (strType)
                    {
                        case "Add":
                            clients1.Add(custCollection);
                            Display();
                            SaveControls();
                            break;
                        case "Edit":
                            clients1.Delete(custCollection);
                            clients1.Add(custCollection);
                            Display();
                            SaveControls();
                            break;
                        default:

                            break;
                    }
                                        
                    isDirtyBoolean = true; //changes made to data
                }
            }            
            catch (FormatException) //balance field must be a #
            {
                MessageBox.Show("Must have a numeric value for balance.", "Error");
                txtBxBalance.Focus();
            }        
        }

        //displays first and last name, address, city, state, and home and cell phone #'s
        private void contactInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strOutput = clients1.current.Data.FirstName + " " + clients1.current.Data.LastName + "\n" +
            clients1.current.Data.Address + "\n" +
            clients1.current.Data.City + ", " + clients1.current.Data.State + " " + clients1.current.Data.Zip + "\n" +
            "Home Phone: " + clients1.current.Data.HomePhone + "\n" +
            "Cell Phone: " + clients1.current.Data.CellPhone;

            MessageBox.Show(strOutput, "Contact Info");
        }

        //displays account #, first and last name, and balance
        private void accountInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strOutput = clients1.current.Data.Account.ToString() + "\n" +
                clients1.current.Data.FirstName + " " + clients1.current.Data.LastName +
                "\nBalance: " + clients1.current.Data.Balance.ToString("C");

            MessageBox.Show(strOutput, "Account Info");
        }

        //closes form and asks to save changes if made
        private void btnExit_Click(object sender, EventArgs e)
        {
            //determines if chaanges made to data
            if (isDirtyBoolean)
            {
                DialogResult result;
                string fileName;

                //initializes save file dialog and opens dialog box
                using (SaveFileDialog fileChooser = new SaveFileDialog())
                {
                    fileChooser.CheckFileExists = false;
                    result = fileChooser.ShowDialog();
                    fileName = fileChooser.FileName;
                }

                if (result == DialogResult.OK)
                {
                    //determines if filename was entered-gives message if not
                    if (fileName == string.Empty || fileName == null)
                    {
                        MessageBox.Show("Must select a valid file. Try again.", "Error");
                    }
                    else
                    {
                        //initializes filestream and sets to create and write
                        output = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                        accountFileWriter = new StreamWriter(output);

                        clients1.current = clients1.First;//sets current to beginning of list

                        //loop writes each element to diff line in file until reaches last
                        while (clients1.current != clients1.Last)
                        {
                            accountFileWriter.WriteLine(clients1.current.Data.Account.ToString());
                            accountFileWriter.WriteLine(clients1.current.Data.FirstName);
                            accountFileWriter.WriteLine(clients1.current.Data.LastName);
                            accountFileWriter.WriteLine(clients1.current.Data.Address);
                            accountFileWriter.WriteLine(clients1.current.Data.City);
                            accountFileWriter.WriteLine(clients1.current.Data.State);
                            accountFileWriter.WriteLine(clients1.current.Data.Zip);
                            accountFileWriter.WriteLine(clients1.current.Data.HomePhone);
                            accountFileWriter.WriteLine(clients1.current.Data.CellPhone);
                            accountFileWriter.WriteLine(clients1.current.Data.Balance.ToString());
                            clients1.current = clients1.current.NodeNext;
                        }//end loop

                        if (clients1.current == clients1.Last)//adds Last data to file
                        {
                            accountFileWriter.WriteLine(clients1.current.Data.Account.ToString());
                            accountFileWriter.WriteLine(clients1.current.Data.FirstName);
                            accountFileWriter.WriteLine(clients1.current.Data.LastName);
                            accountFileWriter.WriteLine(clients1.current.Data.Address);
                            accountFileWriter.WriteLine(clients1.current.Data.City);
                            accountFileWriter.WriteLine(clients1.current.Data.State);
                            accountFileWriter.WriteLine(clients1.current.Data.Zip);
                            accountFileWriter.WriteLine(clients1.current.Data.HomePhone);
                            accountFileWriter.WriteLine(clients1.current.Data.CellPhone);
                            accountFileWriter.WriteLine(clients1.current.Data.Balance.ToString());
                        }

                        accountFileWriter.Close(); //close filewriter
                        Application.Exit(); //close application
                    }
                }
                else
                {
                    Application.Exit(); //close application
                }
            }
            else
            {
                Application.Exit(); //close application
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gets current font and displays font dialog box
            fontDialog1.Font = this.Font;
            fontDialog1.ShowDialog();
            this.Font = fontDialog1.Font; //sets chosen font
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gets current backcolor and opens color dialog box
            colorDialog1.Color = this.BackColor;
            colorDialog1.ShowDialog();
            this.BackColor = colorDialog1.Color; // sets chosen backcolor
        }
    }
}
