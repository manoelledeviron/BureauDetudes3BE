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
    public partial class Form7_Final_bill : Form
    {
        public static Closet closet1;
        public static Closet Closet
        {
            get { return closet1; }
            set { closet1 = value; }
        }
        public Form7_Final_bill()
        {
            InitializeComponent();

            DBConnect database = new DBConnect();

            //Select(string columns, string table, string equal)
            //string query = "SELECT " + columns + " FROM " + table + " WHERE " + equal;

           // Price.Text= database.Select("Prix", "Commande", Program.Id);
            // lastname.Text = database.Select("LastName", "Commande", Convert.ToString(Program.Id));

        }

        //Pour afficher des variables stocker dans dans des cases d'une page precedente faire appel à la variable et partager celle-ci au sein de tout la class mere Form

        private void Form7_Final_bill_Load(object sender, EventArgs e)
        {
            //To show the name of the customer and the price
            
            
        }

        //To return to Main Menu
        private void Title_Click(object sender, EventArgs e)
        {
            Welcome_form frm = new Welcome_form(Closet);
            frm.Show();
            Hide();
        }

        //To go backward
        private void Return_btn_Click(object sender, EventArgs e)
        {
            Form6_Resume frm = new Form6_Resume();
            frm.Show();
            Hide();
        }

        //To go forward
        private void Pay_btn_Click(object sender, EventArgs e)
        {
            //Open a browser to show the bill or a pop up for now

            MessageBox.Show("Thank you");
        }

        //To show the result of the bill
        private void Price_TextChanged(object sender, EventArgs e)
        {

        }

        private void LastName_TextChanged(object sender, EventArgs e)
        {
        
        }
    }
}