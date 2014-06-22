using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Stephanie Goedde
//Programming 2
//Customer Class
//11-28-11

namespace Accounts
{
    public class Customer : IComparable<Customer>
    {
        private int intAccount;
        private string strFirst;
        private string strLast;
        private string strAddress;
        private string strCity;
        private string strState;
        private string strZip;
        private string strHomePhone;
        private string strCellPhone;
        private decimal decBalance;

        //10 parameter constructor
        public Customer(int intAccount, string strFirst, string strLast, string strAddress, string strCity, string strState,
            string strZip, string strHomePhone, string strCellPhone, decimal decBalance)
        {
            Account = intAccount;
            FirstName = strFirst;
            LastName = strLast;
            Address = strAddress;
            City = strCity;
            State = strState;
            Zip = strZip;
            HomePhone = strHomePhone;
            CellPhone = strCellPhone;
            Balance = decBalance;
        }

        //properties to get and set values
        public int Account
        {
            get { return intAccount; }
            set { intAccount = value; }
        }

        public string FirstName
        {
            get { return strFirst; }
            set { strFirst = value; }
        }

        public string LastName
        {
            get { return strLast; }
            set { strLast = value; }
        }

        public string Address
        {
            get { return strAddress; }
            set { strAddress = value; }
        }

        public string City
        {
            get { return strCity; }
            set { strCity = value; }
        }

        public string State
        {
            get { return strState; }
            set { strState = value; }
        }

        public string Zip
        {
            get { return strZip; }
            set { strZip = value; }
        }

        public string HomePhone
        {
            get { return strHomePhone; }
            set { strHomePhone = value; }
        }

        public string CellPhone
        {
            get { return strCellPhone; }
            set { strCellPhone = value; }
        }

        public decimal Balance
        {
            get { return decBalance; }
            set { decBalance = value; }
        }

        //compares current to new-returns integer
        public int CompareTo(Customer objCompare)
        {
            int intResult;

            intResult = this.Account.CompareTo((objCompare).Account);

            return intResult;
        }
    }
}
