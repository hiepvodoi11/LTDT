using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GraphSharp;
using Microsoft.Glee;
using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace TSP_Demo1
{
    class FileText
    {
        // Khai bao biến sử dụng
        private String _fileName;
        public String FileName
        {
            set
            {
                _fileName = value;
            }
            get
            {
                return _fileName; 
            }
        }
        private System.IO.FileStream fs;                // Khai bao File Input
        private System.IO.FileStream ft;                // Khai bao File Output
        private int[,] MaTranKe = new int[100, 100];    // Ma Tran Ke chua moi quan he cac dinh
        private int[] ChuaDuyet = new int[100];         // Dinh chua duyet
        private int[,] TOUR = new int[100,100];         // Mang luu cac Chu Trinh
        private int sodinh = 0;                         // so luong dinh - thanh pho
        private int sochutrinh = 0;                     // so luong chu trinh
        private int TourMin = 0;                        // chi so chua chu trinh nho nhat
        private int[] ArrCost = new int[100];           // Mang luu trong so chu trinh
        private int cost = 0;                           // Chi phi mot chu trinh
        private int costMin;                            // Chi phi chu trinh nho nhat
        int[] ArrTemp = new int[100];                   // Mang chua chu trinh ngan nhat

        private String strTour;


        // Các phương thức thực thi
       
        /// <summary> 
        /// Ghi File Output
        /// </summary> 
        public void WriteData()
        {
            ft = new System.IO.FileStream(@".\Output.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter sw = new StreamWriter(ft);
            for (int i = 0; i < sochutrinh; i++)
            {
                strTour = "Chu trình: ";
                for (int j = 0; j <=sodinh;j++)
                {
                    strTour = strTour + TOUR[i,j].ToString();
                    if (j < sodinh)
                    {
                        strTour = strTour + " >> ";
                    }
                }
                sw.WriteLine(strTour);
                String strCost = "Chi phí: " + ArrCost[i].ToString();
                sw.WriteLine(strCost);
                String strLine = "-------------------------------";
                sw.WriteLine(strLine);
            }
            // Xuat chu trinh ngan nhat
            costMin = GiaTriNhoNhat();
            String strTourMin = FTourMin();
            sw.WriteLine(strTourMin);
            
            String strMinCost = "Chi phí nhỏ nhất: " + costMin.ToString();
            sw.WriteLine(strMinCost);

            // Dong file
            sw.Flush();
            sw.Close();
            ft.Close();
        }

        /// <summary> 
        /// Xuat Chu Trinh Ngan Nhat
        /// </summary> 
        public String FTourMin()
        {
            int vitri = 0;
             
            // Gan Tour Min cho Ma Tran Temp
            for (int i = 0; i < sodinh; i++ )
            {
                ArrTemp[i] = TOUR[TourMin, i];
            }

            // Xac dinh vi tri dinh bat dau
            for (int i = 0; i < sodinh; i++)
            {
                if (TOUR[TourMin, i] == 0)
                {
                    vitri = i;
                }
            }

            // Dich Trai Ma Tran
            for (int i = 0; i < vitri - 0; i++ )
            {
                int temp = ArrTemp[0];
                for (int j = 0; j < sodinh - 1; j++ )
                {
                    ArrTemp[j] = ArrTemp[j + 1];
                }
                ArrTemp[sodinh - 1] = temp;
            }

            String strTourMin = "Chu trình ngắn nhất: ";
            for (int i = 0; i < sodinh; i++)
            {
                strTourMin = strTourMin + ArrTemp[i].ToString() + " >> ";
            }
            strTourMin = strTourMin + ArrTemp[0].ToString();
            return strTourMin;
        }

        /// <summary> 
        /// Đọc File
        /// </summary> 
        public void ReadData()
        {
            fs = new System.IO.FileStream(_fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs);

            // Doc dong dau tien = so dinh trong do thi
            String str = sr.ReadLine();
            sodinh = int.Parse(str);

            // Khai bao - ghi ma tran ke
            string s = sr.ReadLine();
            int i = 0;
            while (s != null)
            {
                string[] b = s.Split(' ');
                for (int j = 0; j < sodinh; j++)
                {
                    MaTranKe[i,j] = int.Parse(b[j].ToString());
                }
                i++;
                s = sr.ReadLine();
            }

            // Xu ly Thuat Toan TSP

            // Truyen DinhBatDau tim cac chu trinh
            for(i = 0; i < sodinh; i++)
            {
                // Set tat ca dinh la chua duyet == 1
                for (int j = 0; j < sodinh; j++)
                {
                    ChuaDuyet[j] = 1;
                }
                int dinhbatdau = i;
                Excute(dinhbatdau);
            }

            // Ghi ket qua ra file Output.txt
            WriteData();

            // Dong file txt
            sr.Close();
            fs.Close();
        }
        
        /// <summary> 
        /// Xu ly Thuat Toan TSP
        /// </summary> 
        public void Excute(int dinhbd)
        {
            cost = 0;
            TOUR[dinhbd,0] = dinhbd;
	        int x = 1;
            for (int i = dinhbd; i < sodinh && x < sodinh; )
	        {
		        int dinhketiep=sodinh+1;
                if (ChuaDuyet[i] == 1)
		        {
                    ChuaDuyet[i] = 0;   // Danh dau dinh da duyet
			        int min = 10000;    // Khoi tao gia tri cuc dai cho min  
			        for (int j = 0; j < sodinh; j++)
			        {
                        if (MaTranKe[i,j] < min && j != i && ChuaDuyet[j] == 1)
				        {
					        min = MaTranKe[i,j];
                            dinhketiep = j;
				        }
			        }
			        cost += min;
		        }
                i = dinhketiep;
		        TOUR[dinhbd,x] = i;
		        x++;
	        }

            // Lay Trong So tu DinhCuoiCung toi DinhBatDau
	        for (int j = 0; j < sodinh; j++)
	        {
                if (ChuaDuyet[j] == 1)
		        {
                    cost += MaTranKe[dinhbd, TOUR[dinhbd,x - 1]];
		        }
	        }

            // Gan DinhBatDau vao vi tri cuoi mang
            TOUR[dinhbd,sodinh] = dinhbd;
            sochutrinh++;
            ArrCost[dinhbd] = cost;
        }


        /// <summary> 
        /// Tim cost nho nhat
        /// </summary> 
        public int GiaTriNhoNhat()
        {
            int min = ArrCost[0];
            for(int i=0; i<sochutrinh;i++)
            {
                if(ArrCost[i]<min)
                {
                    min = ArrCost[i];
                    TourMin = i;
                }
            }
            return min;
        }

        /// <summary> 
        /// Tao do thi
        /// </summary> 
        public void Graph()
        {
            //create a form
            System.Windows.Forms.Form FormGraph = new System.Windows.Forms.Form();
            FormGraph.Text = "Đồ thị kết quả";
            FormGraph.ClientSize = new System.Drawing.Size(900, 600);
            //create a viewer object
            Microsoft.Glee.GraphViewerGdi.GViewer viewer = new Microsoft.Glee.GraphViewerGdi.GViewer();
            //create a graph object
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph");
            graph.GraphAttr.LayerDirection = LayerDirection.LR;
            graph.GraphAttr.Orientation = Microsoft.Glee.Drawing.Orientation.Portrait;
            //create the graph content
            String[] vertices = new string[100];

            for (int i = 0; i <= sodinh; i++)
            {
                vertices[i] = ArrTemp[i].ToString();
            }

            // Tao cac canh noi giua cac dinh
            for (int i = 0; i < sodinh; i++)
            {
                ChuaDuyet[i] = 1;
            }

            int dinhdau = ArrTemp[0];
            int dinhcuoi = ArrTemp[sodinh - 1];
            for(int i = 0; i < sodinh; i++)
            {
                int dinhke = ArrTemp[i + 1];
                ChuaDuyet[ArrTemp[i]] = 0;
                for (int j = 0; j < sodinh;j++)
                {
                    if(i==dinhdau && j==dinhcuoi)
                    {
                        Microsoft.Glee.Drawing.Edge CanhCuoi = graph.AddEdge(vertices[sodinh - 1], MaTranKe[ArrTemp[sodinh - 1], ArrTemp[sodinh]].ToString(), vertices[sodinh]);
                        CanhCuoi.EdgeAttr.ArrowHeadAtTarget = Microsoft.Glee.Drawing.ArrowStyle.None;
                        CanhCuoi.EdgeAttr.Color = Microsoft.Glee.Drawing.Color.Red;
                        CanhCuoi.EdgeAttr.Fontsize = 8;
                    }
                    else
                    {
                        if (ChuaDuyet[j] == 1)
                        {
                            if (dinhke == j)
                            {
                                Microsoft.Glee.Drawing.Edge Canh = graph.AddEdge(vertices[i], MaTranKe[ArrTemp[i],j].ToString(), j.ToString());
                                Canh.EdgeAttr.ArrowHeadAtTarget = Microsoft.Glee.Drawing.ArrowStyle.None;
                                Canh.EdgeAttr.Color = Microsoft.Glee.Drawing.Color.Red;
                                Canh.EdgeAttr.Fontsize = 8;
                            }
                            else
                            {
                                Microsoft.Glee.Drawing.Edge Canh = graph.AddEdge(vertices[i], MaTranKe[ArrTemp[i], j].ToString(), j.ToString());
                                Canh.EdgeAttr.ArrowHeadAtTarget = Microsoft.Glee.Drawing.ArrowStyle.None;
                                Canh.EdgeAttr.Color = Microsoft.Glee.Drawing.Color.Black;
                                Canh.EdgeAttr.Fontsize = 8;
                            }
                        }
                    }
                }
            }

            // To mau dinh
            for (int i=0; i< sodinh;i++)
            {
                Microsoft.Glee.Drawing.Node Vertex = graph.FindNode(ArrTemp[i].ToString());
                Vertex.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.PaleGreen;
                Vertex.Attr.Shape = Microsoft.Glee.Drawing.Shape.Circle;

            }

            //bind the graph to the viewer
            viewer.Graph = graph;

            //associate the viewer with the form
            FormGraph.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            FormGraph.Controls.Add(viewer);
            FormGraph.ResumeLayout();

            //show the form
            FormGraph.ShowDialog();
        }
    }
}
