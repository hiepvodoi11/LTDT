using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GraphSharp;
using Microsoft.Glee;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;


namespace TSP_Demo1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        // Event Click button Open File
        private void btnOpen_Click(object sender, EventArgs e)
        {

            // Mở 1 file txt
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                txtContent.Text = File.ReadAllText(openFileDialog1.FileName);

                FileText ft = new FileText();
                ft.FileName = openFileDialog1.FileName;
                ft.ReadData();

                //Xuat ket qua
                txt_kq.Text = File.ReadAllText(@".\Output.txt");
                btn_Graph.Enabled = true;
            }
        }

        // Event Click button Exit
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        // Event Click button Graph
        private void btn_Graph_Click(object sender, EventArgs e)
        {
            FileText ft = new FileText();
            ft.FileName = openFileDialog1.FileName;
            ft.ReadData();
            ft.Graph();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, txtContent.Text);
            }
        }
    }
}
