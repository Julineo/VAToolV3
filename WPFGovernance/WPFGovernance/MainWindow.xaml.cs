using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Threading.Tasks;

namespace WPFGovernance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void eIS()
        {
            //Read Input and Output folder from config file
            string Input = ConfigurationManager.AppSettings["Input"].ToString();
            string Output = ConfigurationManager.AppSettings["Output"].ToString();
            //read fields positions and lenghts from config file
            int posCODE = Convert.ToInt32(ConfigurationManager.AppSettings["posCODE"].ToString());
            int posNAME = Convert.ToInt32(ConfigurationManager.AppSettings["posNAME"].ToString());
            int posAMTDEDUCTED = Convert.ToInt32(ConfigurationManager.AppSettings["posAMTDEDUCTED"].ToString());
            int posREMARKS = Convert.ToInt32(ConfigurationManager.AppSettings["posREMARKS"].ToString());
            int lenCODE = Convert.ToInt32(ConfigurationManager.AppSettings["lenCODE"].ToString());
            int lenNAME = Convert.ToInt32(ConfigurationManager.AppSettings["lenNAME"].ToString());
            int lenAMTDEDUCTED = Convert.ToInt32(ConfigurationManager.AppSettings["lenAMTDEDUCTED"].ToString());
            int lenREMARKS = Convert.ToInt32(ConfigurationManager.AppSettings["lenREMARKS"].ToString());

            string CODE, NAME, AMTDEDUCTED = "", REMARKS = "", PPE = "";
            bool datainfo = false;
            int n;
            string[] files = Directory.GetFiles(Input);
            //Connect to database
            //SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MiscConnection"].ToString());
            //try
            //{
            //    myConnection.Open();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            ////Trancate table in database
            //try
            //{
            //    SqlCommand myCommand = new SqlCommand("TRUNCATE TABLE t0028_0", myConnection);
            //    myCommand.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}


            for (int i = 0; i < files.Length; i++)
            {
                //MessageBox.Show(Input + "\\" + System.IO.Path.GetFileName(files[i]));
                n = 0;

                foreach (string line in File.ReadLines(Input + "\\" + System.IO.Path.GetFileName(files[i])))
                {
                    //We can check "PAY PERIOD ENDING DATE 04/16/16" every string, and update it if we can find
                    try
                    {
                        if (line.Substring(50, 22) == "PAY PERIOD ENDING DATE")//this is left hardcoded, transfer 35,8 to cfg file when have time
                        {
                            PPE = (line.Substring(73, 8)).Trim();
                        }
                    }
                    catch
                    { }
                    if ((line.Length == 0) && (datainfo))
                    {
                        datainfo = false;
                    }
                    try
                    {
                        if (line.Substring(35, 8) == "SUBTOTAL")//this is left hardcoded, transfer 35,8 to cfg file when have time
                        {
                            datainfo = false;
                            continue;
                        }
                    }
                    catch { }
                    try
                    {
                        if (line.Substring(posCODE, 4) == "CODE")//Number 4 hardcoded to read "CODE" speciphically
                        {
                            datainfo = true;
                            continue;
                        }
                    }
                    catch { }
                    //MessageBox.Show(line.Length.ToString());
                    if (datainfo)
                    {
                        n = n + 1;
                        CODE = (line.Substring(posCODE, lenCODE)).Trim();
                        NAME = (line.Substring(posNAME, lenNAME)).Trim();
                        try
                        {
                            AMTDEDUCTED = (line.Substring(posAMTDEDUCTED, lenAMTDEDUCTED)).Trim();
                        }
                        catch { }
                        try
                        {
                            //Next string reads the last value from the file, we not using it here, just keep it in comment for future use
                            //REMARKS = (line.Substring(98, line.Length-98)).Trim();
                            REMARKS = (line.Substring(posREMARKS, lenREMARKS)).Trim();
                        }
                        catch { }
                        //if (REMARKS.Length > 0)//debug
                        //    MessageBox.Show("Fuck:REMARKS " + line);
                        //REMARKS = "";

                        //Converting 04/16/16 into 2016-04-16
                        PPE = "20" + PPE.Substring(6, 2) + "-" + PPE.Substring(0, 2) + "-" + PPE.Substring(3, 2);
                        MessageBox.Show("INSERT INTO t0028_0([Code],[SSN],[Name],[AmtDeducted],[PPE],[EMP ID],[LAST NAME],[FIRST NAME],[MIDDLE],[UpdatePhase],[ConstitID])VALUES('" + CODE + "','XXX-XX-XXXX','" + NAME + "'," + AMTDEDUCTED + ",'" + PPE + "',NULL,NULL,NULL,NULL,NULL,NULL)");
                        //insert line into t0028_0 table in database CODE, NAME, AMTDEDUCTED, REMARKS, PPE);
                        /*try
                        {
                            SqlCommand myCommand = new SqlCommand("INSERT INTO t0028_0([Code],[SSN],[Name],[AmtDeducted],[PPE],[EMP ID],[LAST NAME],[FIRST NAME],[MIDDLE],[UpdatePhase],[ConstitID])VALUES('" + CODE + "','XXX-XX-XXXX','" + NAME + "'," + AMTDEDUCTED + ",<PPE, smalldatetime,>,NULL,NULL,NULL,NULL,NULL,NULL)", myConnection);
                            myCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }*/

                        CODE = "";
                        NAME = "";
                        AMTDEDUCTED = "";
                        REMARKS = "";
                    }
                }

            }
            //execute sp0028_Main SP
            MessageBox.Show("Done! eIS!");
        }

        private void New()
        {
            //MessageBox.Show(ConfigurationManager.ConnectionStrings["eISConnection"].ToString());
            //MessageBox.Show(ConfigurationManager.AppSettings["Input"].ToString());
            //Read Input and Output folder from config file
            string Input = ConfigurationManager.AppSettings["Input"].ToString();
            string Output = ConfigurationManager.AppSettings["Output"].ToString();
            //read fields positions and lenghts from config file
            int posCODE = Convert.ToInt32(ConfigurationManager.AppSettings["posCODE"].ToString());
            int posNAME = Convert.ToInt32(ConfigurationManager.AppSettings["posNAME"].ToString());
            int posAMTDEDUCTED = Convert.ToInt32(ConfigurationManager.AppSettings["posAMTDEDUCTED"].ToString());
            int posREMARKS = Convert.ToInt32(ConfigurationManager.AppSettings["posREMARKS"].ToString());
            int lenCODE = Convert.ToInt32(ConfigurationManager.AppSettings["lenCODE"].ToString());
            int lenNAME = Convert.ToInt32(ConfigurationManager.AppSettings["lenNAME"].ToString());
            int lenAMTDEDUCTED = Convert.ToInt32(ConfigurationManager.AppSettings["lenAMTDEDUCTED"].ToString());
            int lenREMARKS = Convert.ToInt32(ConfigurationManager.AppSettings["lenREMARKS"].ToString());

            string CODE, NAME, AMTDEDUCTED = "", REMARKS = "", PPE = "";
            bool datainfo = false;
            int n;
            string[] files = Directory.GetFiles(Input);
            var csv = new StringBuilder();
            string facilityID;
            string facility;

            for (int i = 0; i < files.Length; i++)
            {
                //MessageBox.Show(Input + "\\" + System.IO.Path.GetFileName(files[i]));
                n = 0;
                csv.AppendLine("facilityID, facility, CODE, NAME, AMTDEDUCTED, REMARKS, PPE");
                foreach (string line in File.ReadLines(Input + "\\" + System.IO.Path.GetFileName(files[i])))
                {
                        //We can check "PAY PERIOD ENDING DATE 04/16/16" every string, and update it if we can find
                        try
                        {
                            if (line.Substring(50, 22) == "PAY PERIOD ENDING DATE")//this is left hardcoded, transfer 35,8 to cfg file when have time
                            {
                                PPE = (line.Substring(73, 8)).Trim();
                            }
                        }
                        catch
                        { }
                        if ((line.Length == 0) && (datainfo))
                        {
                            datainfo = false;
                        }
                        try
                        {
                            if (line.Substring(35, 8) == "SUBTOTAL")//this is left hardcoded, transfer 35,8 to cfg file when have time
                            {
                                datainfo = false;
                                continue;
                            }
                        }
                        catch { }                        
                        try
                        {
                            if (line.Substring(posCODE, 4) == "CODE")//Number 4 hardcoded to read "CODE" speciphically
                            {
                                datainfo = true;
                                continue;
                            }
                        }
                        catch { }
                        //MessageBox.Show(line.Length.ToString());
                        if (datainfo)
                        {
                            n = n + 1;
                            CODE = (line.Substring(posCODE, lenCODE)).Trim();
                            NAME = (line.Substring(posNAME, lenNAME)).Trim();
                            try
                            {
                                AMTDEDUCTED = (line.Substring(posAMTDEDUCTED, lenAMTDEDUCTED)).Trim();
                            }
                            catch { }
                            try
                            {
                                //Next string reads the last value from the file, we not using it here, just keep it in comment for future use
                                //REMARKS = (line.Substring(98, line.Length-98)).Trim();
                                REMARKS = (line.Substring(posREMARKS, lenREMARKS)).Trim();
                            }
                            catch { }
                            //if (REMARKS.Length > 0)//debug
                            //    MessageBox.Show("Fuck:REMARKS " + line);
                            //REMARKS = "";

                            //var first = CODE;
                            //var second = NAME;
                            //var third = AMTDEDUCTED;
                            //var forth = REMARKS;
                            facilityID = ConfigurationManager.AppSettings[CODE].ToString();
                            facility = ConfigurationManager.AppSettings[CODE + "_f"].ToString();

                            var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", facilityID, facility, CODE, NAME, AMTDEDUCTED, REMARKS, PPE);
                            csv.AppendLine(newLine);  

                            CODE = "";
                            NAME = "";
                            AMTDEDUCTED = "";
                            REMARKS = "";
                            facilityID = "";
                            facility = "";
                        }
                    
                }
                File.WriteAllText(Output + "\\" + System.IO.Path.GetFileName(files[i]), csv.ToString());
                //File.Delete(Input + "\\" + System.IO.Path.GetFileName(files[i]));
            }
            MessageBox.Show("Done! New!");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //Run old or new version of the tool depending on radiobutton selection
            if (radioButton1.IsChecked == true)
            {
                eIS();
            }
            if (radioButton2.IsChecked == true)
            {
                New();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.Text = ConfigurationManager.AppSettings["Input"].ToString();
            textBox2.Text = ConfigurationManager.AppSettings["Output"].ToString();

            if (ConfigurationManager.AppSettings["Version"].ToString() == "eIS Version")
            { radioButton1.IsChecked = true;}
            if (ConfigurationManager.AppSettings["Version"].ToString() == "New Version")
            { radioButton2.IsChecked = true; }

        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
