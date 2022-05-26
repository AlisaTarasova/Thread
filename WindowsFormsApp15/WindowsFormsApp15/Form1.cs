using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {        
        static public Random rnd = new Random();
        static public string[] words = { "год", "человек","время","дело","жизнь","день","рука",
            "работа","место","вопрос","лицо","глаз","страна","друг","сторона","дом","случай",
            "ребенок","система","вид","конец","отношение","проблема","земля","решение"};

        string fileName = null;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
                MyInvoke(label1, () =>
                {
                    if (fileName == null)
                        label1.Text = "Файл не выбран";
                    else
                        label1.Text =  "Выбранный файл: " + fileName;
                });
            }
        }

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void WriteToFile()
        {
            try
            {
                Stream writer = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.Read);
                StreamWriter myWriter = new StreamWriter(writer);

                for (int i = 0; i < 30; i++)
                {
                    string str = words[rnd.Next(words.Length)];
                    MyInvoke(listBox2, () =>
                    {
                        listBox2.Items.Add(str);
                    });
                    myWriter.WriteLine(str);
                    myWriter.Flush();
                    Thread.Sleep(3000);
                }
            }
            catch
            {
                MessageBox.Show("В файл уже производит запись!");
            }
           
        }

        private void ReadInFile()
        {
            Stream reader = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader myReader = new StreamReader(reader);
            string str;
            while ((str = myReader.ReadLine()) != null)
            {
               MyInvoke(listBox1, () =>
               {
                    listBox1.Items.Add(str);
               });               
               Thread.Sleep(3000);
            }         
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
        }  

        private void MyInvoke(Control item, Action action)
        {
            if (item.InvokeRequired)
            {
                item.Invoke(new Action(action));
                return;
            }
            action();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FileName == null)
            {
                MessageBox.Show("Файл не выбран!");
                return;
            }           
            
            Thread thread1 = new Thread(WriteToFile);
            thread1.Start();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            FileName = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (FileName == null)
            {
                MessageBox.Show("Файл не выбран!");
                return;
            }

            Thread thread2 = new Thread(ReadInFile);
            thread2.Start();          
        }
    }
}

