using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;


namespace unpackPicture
{
    public struct TileObject
    {
        public string FileName;
        public Rectangle rect;    
        public TileObject(string _filename,Rectangle _rect)
        {
            FileName = _filename;
            rect = _rect;
        }
    }

    public partial class Form1 : Form
    {
        Bitmap myBitmap = null;
        List<TileObject> TileObjects;
        string SavePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "tpsheet")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "BMP File| *.png";
                openFileDialog1.Title = "取得影像檔";
                openFileDialog1.FilterIndex = 3;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    myBitmap = new Bitmap(openFileDialog1.FileName);
                }

                string name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                string file = Path.GetDirectoryName(openFileDialog1.FileName);
                string path = "";

                SavePath = file + "\\unpack" + name + "\\";
                path = file + "\\" + name + ".tpsheet";



                TileObjects = new List<TileObject>();
                string[] lines = System.IO.File.ReadAllLines(path);

                Point pSize = new Point(myBitmap.Width, myBitmap.Height);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] temp = lines[i].Split(';');
                    if (temp.Length == 7)
                    {
                        int w = int.Parse(temp[3]);
                        int h = int.Parse(temp[4]);
                        int x = int.Parse(temp[1]);
                        int y = pSize.Y - int.Parse(temp[2]) - h;

                        TileObject tileObj = new TileObject(_filename: temp[0], _rect: new Rectangle(x, y, w, h));
                        TileObjects.Add(tileObj);
                    }
                }
            }
            else if(comboBox1.SelectedItem.ToString() == "font")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "BMP File| *.png";
                openFileDialog1.Title = "取得影像檔";
                openFileDialog1.FilterIndex = 3;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    myBitmap = new Bitmap(openFileDialog1.FileName);
                }

                string name = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                string file = Path.GetDirectoryName(openFileDialog1.FileName);
                string path = "";

                SavePath = file + "\\unpack" + name + "\\";
                path = file + "\\" + name + ".fnt";
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode main = doc.SelectSingleNode("font/chars");//選擇節點
                if (main == null)
                    return;                

                XmlElement element = (XmlElement)main;

                TileObjects = new List<TileObject>();
                for (int i = 0; i < element.ChildNodes.Count; i++)
                {
                    int x = Convert.ToInt32(element.ChildNodes[i].Attributes["x"].Value);
                    int y = Convert.ToInt32(element.ChildNodes[i].Attributes["y"].Value);
                    int width = Convert.ToInt32(element.ChildNodes[i].Attributes["width"].Value);
                    int height = Convert.ToInt32(element.ChildNodes[i].Attributes["height"].Value);                    

                    TileObject tileObj = new TileObject(_filename: i.ToString(), _rect: new Rectangle(x, y, width, height));
                    TileObjects.Add(tileObj);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (myBitmap != null)
                {

                    if (!System.IO.Directory.Exists(SavePath))
                    {
                        System.IO.Directory.CreateDirectory(SavePath);
                        for (int i = 0; i < TileObjects.Count; i++)
                        {
                            Bitmap imageOutput = myBitmap.Clone(TileObjects[i].rect,
                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                            imageOutput.Save(SavePath + TileObjects[i].FileName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                            imageOutput.Dispose();
                        }
                        MessageBox.Show("unpack Complete !");
                    }
                    else
                    {
                        MessageBox.Show("Error : Have unpacked !");

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
    }
}
