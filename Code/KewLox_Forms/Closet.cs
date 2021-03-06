﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KewLox_Forms
{
    public class Closet
    {
        private static List<KeyValuePair<string,int>> parts = new List<KeyValuePair<string, int>>();

        public List<KeyValuePair<string,int>> Parts
        {
            get { return parts; }
            set { parts = value; }
        }

        private static int width;

        public static int Width
        {
            get { return width; }
            set { width = value; }

        }
        public static int maxNbBoxes;
        public static int MaxNbBoxes
        {
            get { return maxNbBoxes; }
            set { maxNbBoxes = value; }

        }
        public static int minNbBoxes;
        public static int MinNbBoxes
        {
            get { return minNbBoxes; }
            set { minNbBoxes = value; }

        }
        public static int boxamount;
        public static int Boxamount
        {
            get { return boxamount; }
            set { boxamount= value; }

        }
        private static int depth;

        public static int Depth
        {
            get { return depth; }
            set { depth = value; }

        }
        private static int totalHeight;
        public static int TotalHeight
        {
            get { return totalHeight; }
            set { totalHeight = value; }

        }



        public List<KeyValuePair<string, int>> AddBoxes(string lcol, string rcol, string bcol, string dcol, string doorcol, string doormat, int totheight, int width, int depth, int boxheight, string acolor, string pcolor,bool done)
        {
            Box box = new Box();
            if (done == false)
            {
                
                box.AddConstructionParts(boxheight, lcol, rcol, bcol, dcol, doorcol, doormat, width, depth);
                foreach (KeyValuePair<string, int> boxpart in box.Parts)
                {
                    parts.Add(boxpart);
                }
            }
            if (done == true)
            {


                // All boxes have been added
                //Adding the corners, and the above pannel and 'traverses' at the end of the command
                DBConnect database = new DBConnect();

                ConstructionParts angles = new ConstructionParts() { Name = "Cornière", Height = Convert.ToString(totheight), Color = acolor };
                angles.Code = angles.MakeCode();
                KeyValuePair<string, int> anglesparts = new KeyValuePair<string, int>(angles.Code, 4);
                parts.Add(anglesparts);
                string[] query = new string[7] { "Name", "Height", "Depth", "Width", "Quantity", "OrderId", "Color" };
                string[] data = angles.AddPart(4);
                database.Insert("commandespieces", query, data);

                //Add the up panel in the end
                ConstructionParts UpP = new ConstructionParts { Depth = Convert.ToString(depth), Width = Convert.ToString(width), Name = "Panneau HB" };

                List<string> DbLink = new List<string>(6);
                String[] DbColumn;
                DbLink.Add("Name");
                DbLink.Add("Height");
                DbLink.Add("Depth");
                DbLink.Add("Width");
                DbLink.Add("Quantity");
                DbLink.Add("OrderId");
                DbLink.Add("Color");
                DbColumn = DbLink.ToArray();

                UpP.Color = pcolor;
                string[] request = UpP.AddPart(1);
                database.Insert("commandespieces", DbColumn, request);

                //Add codes for pannels to parts
                List<KeyValuePair<string, int>> pannelcodes = new List<KeyValuePair<string, int>>() { new KeyValuePair<string, int>(UpP.Code = UpP.MakeCode(), 1), };
                parts.AddRange(pannelcodes);

                //Remove the amount taken in db "stock" and update "sold"
                for (int j = 0; j < pannelcodes.Count; j++)
                {
                    database.Sold("sold", pannelcodes[j].Key, pannelcodes[j].Value);
                }



                //add the traverses for the last box (up pannel)
                ConstructionParts FrontCB = new ConstructionParts() { Width = Convert.ToString(width), Name = "Traverse AV", Color = "" };
                ConstructionParts BackCB = new ConstructionParts() { Width = Convert.ToString(width), Name = "Traverse AR", Color = "" };
                ConstructionParts SideCB = new ConstructionParts() { Depth = Convert.ToString(depth), Name = "Traverse GD", Color = "" };

                //Build codes for tasseaux & traverses
                List<KeyValuePair<string, int>> tasseauxTraverses = new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int>(FrontCB.Code = FrontCB.MakeCode(),1),
                new KeyValuePair<string, int>(BackCB.Code = BackCB.MakeCode(),1),
                new KeyValuePair<string, int>(SideCB.Code = SideCB.MakeCode(),2),
            };
                parts.AddRange(tasseauxTraverses);
                //Remove the amount taken in db "stock" and update "sold"
                for (int j = 0; j < tasseauxTraverses.Count; j++)
                {
                    database.Sold("sold", tasseauxTraverses[j].Key, tasseauxTraverses[j].Value);
                }

                string[] request1 = FrontCB.AddPart(1);
                string[] request1bis = BackCB.AddPart(1);
                string[] request2 = SideCB.AddPart(2);


                database.Insert("commandespieces", DbColumn, request1);
                database.Insert("commandespieces", DbColumn, request1bis);
                database.Insert("commandespieces", DbColumn, request2);
            }
            return Parts;
        }

        public decimal GetPrice(List<KeyValuePair<string, int>> pieces)
        {
            DBConnect database = new DBConnect();
            decimal total = 0;
            foreach (KeyValuePair<string, int> part in pieces)
            {
                string[,] prix = database.Select("`Prix-Client`", "stock", "`Code`='" + part.Key + "'");
                decimal priceperpart = Convert.ToDecimal(prix[0, 1]);
                decimal nbparts = Convert.ToDecimal(part.Value);
                total += (priceperpart * nbparts);
            }
            return total;
        }
       

        public void MakeBill(string closetprice, List<KeyValuePair<string, int>> nodup)
        {
            string text="";
            DBConnect database = new DBConnect();
            string[,] customer = database.Select("`FirstName`,`LastName`,`Address`,`Numero`,`TVA`", "commandes", "id = "+Program.Id.ToString());
            string header = "";
            string kewloxaddress="<table width ='1000px'><tr><td width='500px'>Kewlox SPRL<br>Rue et Numéro de maison<br>Code Postal et Commune<br>Telephone<br>Email<br>VAT<br>Account number & BIC</td>";
            string companyaddress= string.Format("<td width='500px'>{0} <br> {1} <br>{2} <br>Telephone number: {3} <br> VAT number: {4}</td></table>",customer[0,1],customer[1,1],customer[2,1], customer[3,1], customer[4,1]);

            string bodyheader;
            string body = "";
            string totalbody = "";
            string bodyfooter;
            string footer = "\u00a9 Kewlox 2017</html>"; //alt 0169 release alt 
            header = String.Format("<html>\n<head>\n</head>\n<body>\n<h1>Bill " +
                "n°{2}</h1>\n<h2>Kewlox thanks you for your purchase.</h2>\n</body>", "border:1px solid black;\nborder-collapse:" +
                "collapse;","border:no-border",Convert.ToString(Program.Id), "\n width:1000px;","\n width:100px;","\n width:200px;");
            bodyheader = "<table width='1000px'>\n<tr>\n<th width='200px'>Part Name</th>\n<th width='200px'>Description</th>\n<th width='100px'>Amount</th>\n<th width='200px'>Price per part</th>\n<th width='100px'>Total</th>\n</tr>";
            bodyfooter = String.Format("<td><b>Total</b></td>\n<td></td>\n<td></td>\n<td></td>\n<td>{0}</td>\n</table>", closetprice);
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))+String.Format("/Bills/bill{0}.html",Convert.ToString(Program.Id));

            text = header +kewloxaddress+companyaddress+ bodyheader;
            File.AppendAllText(path, text);
            foreach (KeyValuePair<string, int> part in nodup)
            {
                string[,] price = database.Select("`Prix-Client`,`Ref`,`Dimensions(cm)`,`Couleur`", "stock", "`Code`='" + part.Key + "'");
                string description = String.Join(" ", price[1, 1], price[2, 1], price[3, 1]);
                body = String.Format("<tr>\n<td>{0}</td>\n<td>{1}</td>\n<td>{2}</td>\n", part.Key, description, part.Value);
                body += String.Format("<td>{0}</td>\n<td>{1}</td>\n</tr>\n", price[0, 1], (Convert.ToDecimal(price[0, 1]) * Convert.ToInt32(part.Value)).ToString());
                totalbody += body;
                File.AppendAllText(path, body);
            }

            string somestring = text + totalbody + bodyfooter + footer;
            File.AppendAllText(path,bodyfooter+footer);
        }

        //Method to remove duplicate names from the list of parts, changes the number of parts from one pair and deletes the other one
        public List<KeyValuePair<string,int>> RemoveDuplicates(List<KeyValuePair<string,int>> parts)
        {
            List<string> keys = new List<string>();
            List<int> values = new List<int>();
            int duplicate = 0;
            List<KeyValuePair<string, int>> nodupparts = new List<KeyValuePair<string,int>>();
            foreach (KeyValuePair<string,int> pair in parts)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
            int i = 0;
            while (i<keys.Count-1)
            {
                while (keys.IndexOf(keys[i], i + 1) > i && keys.IndexOf(keys[i], i + 1) <= keys.Count)
                {
                    duplicate = keys.IndexOf(keys[i], i + 1);
                    values[i] += values[duplicate];
                    keys.RemoveAt(duplicate);
                    values.RemoveAt(duplicate);
                }
                i++;
            }
            for (int j = 0; j< keys.Count;j++)
            {
                nodupparts.Add(new KeyValuePair<string, int>(keys[j],values[j]));
            }
            foreach (KeyValuePair<string,int> pair in nodupparts)
            {
                Console.WriteLine(pair.Key + " " + pair.Value);
            }
            return nodupparts;
        }

    }
}
