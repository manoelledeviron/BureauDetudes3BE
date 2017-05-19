﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KewLox_Forms
{
    public partial class Form5_Signup : Form
    {
        public static Closet closet1;
        public static Closet Armoire
        {
            get { return closet1; }
            set { closet1 = value; }
        }
        public static List<KeyValuePair<string, int>> nodup;
        public static List<KeyValuePair<string, int>> Nodup
        {
            get { return nodup; }
            set { nodup = value; }
        }
        public static decimal price;
        public static decimal Price
        {
            get { return price; }
            set { price = value; }
        }



        public Form5_Signup(List<KeyValuePair<string, int>> nodupli,decimal prix)
        {
                  
            InitializeComponent();
            Nodup = nodupli;
            Price = prix;
        }
        private void Form5_Signup_Load(object sender, EventArgs e)
        {

        }

        //To return to Main Menu
        private void Title_Click(object sender, EventArgs e)
        {
            Welcome_form frm = new Welcome_form(Armoire);
            frm.Show();
            Hide();
        }

        //To go backward
        private void Return_btn_Click(object sender, EventArgs e)
        {
            Form4_Catalog2 frm = new Form4_Catalog2();
            frm.Show();
            Hide();
        }

        //To go forward
        private void Confirm_btn_Click(object sender, EventArgs e)
        {
            DBConnect database = new DBConnect();
            Closet closet1= new Closet();


            //Update(string table, string namecolumn1, string namecolumn2, string value1, int value2)
            //string query = "UPDATE " + table + " SET " + namecolumn1 + "='" + value1 + "' WHERE " + namecolumn2 + "='" + value2 + "'";
            database.Update("commandes", "Login", "`id`", Login.Text, Convert.ToInt32(Program.Id));
            database.Update("commandes", "Password", "`id`", Password.Text, Convert.ToInt32(Program.Id));

            database.Update("commandes", "FirstName", "`id`", firstname.Text, Convert.ToInt32(Program.Id));
            database.Update("commandes", "LastName", "`id`", lastname.Text, Convert.ToInt32(Program.Id));
            //database.Update("commandes", "Address", "`id`", address.Text, Convert.ToInt32(Program.Id));
            database.Update("commandes", "Numero", "`id`", phone.Text, Convert.ToInt32(Program.Id));
            database.Update("commandes", "Email", "`id`", mail.Text, Convert.ToInt32(Program.Id));
            database.Update("commandes", "Prix", "`id`", Convert.ToString(Price), Convert.ToInt32(Program.Id));
            //database.Update("commandes", "enterprise", "`id`", enterprise.Text, Convert.ToInt32(Program.Id));
            //database.Update("commandes", "TVA", "`id`", tva.Text, Convert.ToInt32(Program.Id));
            closet1.MakeBill(Price, Nodup);


            Form7_Final_bill frm = new Form7_Final_bill();
            frm.Show();
            this.Close();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
