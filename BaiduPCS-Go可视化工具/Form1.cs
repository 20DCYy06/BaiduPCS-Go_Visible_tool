using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BaiduPCS_Go可视化工具
{

    public partial class Form1 : Form
    {
        int stateRoot = 0;  //Check RootFolder or Not
        string FileName;    //Get File/Folder Name
        Process P = new Process();  //Process user can see
        Process PReader = new Process();    //Process was hidden
        //Get FileName/FolderName and Add to ListBox
        public void AddListItems(string ItemNames)
        {
            String line;//File info in line
            this.listBox1.Items.Clear();//Clear ListBox Items
            int i=1;//Count line
            StringReader sr = new StringReader(ItemNames);//ReadLine
            //While not EOF
            while ((line = sr.ReadLine()) != null)
            {
                //RootFolder
                if (stateRoot == 0)
                {
                    //CheckLine
                    if (i > 8)
                    {
                        if (line.Length > 42)
                        {
                            //GetFile/FolderName and Add to ListBox
                            line = line.Substring(41, line.Length - 41);
                            this.listBox1.Items.Add(line);   //增加读出的内容到listbox 
                        }
                    }
                }
                //Other Folder
                if (stateRoot == 1)
                {
                    //Check line
                    if (i > 7)
                    {
                        if (line.Length > 40)
                        {//GetFile/FolderName and Add to ListBox
                            line = line.Substring(39, line.Length - 39);
                            this.listBox1.Items.Add(line);   //增加读出的内容到listbox 
                        }
                    }
                }
                i++;
            }
            //Delete Last Item
            this.listBox1.Items.RemoveAt(this.listBox1.Items.Count-1);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Login Btn
            P.StartInfo.Arguments = "login";
            P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            P.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Set Default
            this.listBox1.Items.Clear();
            P.StartInfo.FileName = "BaiduPCS-Go.exe";
            P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            P.StartInfo.Arguments = "config set -savedir C:/Downloads/";
            P.Start();
            PReader.StartInfo.RedirectStandardInput = true;
            PReader.StartInfo.RedirectStandardOutput = true;
            PReader.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            PReader.StartInfo.UseShellExecute = false;
            PReader.StartInfo.CreateNoWindow = true;
            PReader.StartInfo.FileName = "BaiduPCS-Go.exe";
            PReader.StartInfo.Arguments = "quota";
            PReader.Start();
            label1.Text = PReader.StandardOutput.ReadToEnd();
            PReader.Close();
            PReader.StartInfo.Arguments = "cd /";
            PReader.Start();
            PReader.Close();
            PReader.StartInfo.Arguments = "ls";
            PReader.Start();
            PReader.StandardOutput.ReadToEnd();
            PReader.Close();
            PReader.StartInfo.Arguments = "ls";
            PReader.Start();
            richTextBox1.Text = "复制文件或文件夹名称到上面文本框点击下载即可！" + "\n" + "\n" + "下载的文件存储在C:/Downloads文件夹下。" + "\n" + PReader.StandardOutput.ReadToEnd() + "\n\n\n";
            FileName = richTextBox1.Text;
            AddListItems(FileName);
            PReader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //List File Btn
            PReader.StartInfo.Arguments = "ls";
            PReader.Start();
            richTextBox1.Text = "复制文件或文件夹名称到上面文本框点击下载即可！" + "\n" + "\n" + "下载的文件存储在C:/Downloads文件夹下。" + "\n" + PReader.StandardOutput.ReadToEnd()+ "\n\n\n";
            FileName = richTextBox1.Text;
            AddListItems(FileName);
            PReader.Close();
            PReader.StartInfo.Arguments = "quota";
            PReader.Start();
            label1.Text = PReader.StandardOutput.ReadToEnd();
            PReader.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Help Btn
            PReader.StartInfo.Arguments = "help";
            PReader.Start();
            richTextBox1.Text = PReader.StandardOutput.ReadToEnd() + "\n" + "  图形化工具是完全基于BaiduPCS_Go工具，由 HallDave 开发。仅供参考交流，非商业化使用。\n\n\n";
            PReader.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Run BaiduPCS_Go Btn
            Process.Start("BaiduPCS-Go.exe");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Download Btn
            P.StartInfo.Arguments = "d " + textBox1.Text;
            P.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            P.Start();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            //Enter Link
            Process.Start(e.LinkText);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Logout Ensure
            DialogResult dr = MessageBox.Show("确认退出？", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK) {
                PReader.StartInfo.Arguments = "logout";
                PReader.Start();
                PReader.StandardInput.WriteLine("y");
                PReader.Close();
                MessageBox.Show("退出成功！", "提示");
            } 
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Back to RootFolder
            PReader.StartInfo.Arguments = "cd " + "/";
            PReader.Start();
            richTextBox1.Text = PReader.StandardOutput.ReadToEnd() + "\n" + "点按显示文件查看当前目录下文件。";
            stateRoot = 0;
            PReader.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Change Folder
            PReader.StartInfo.Arguments = "cd " + textBox1.Text;
            PReader.Start();
            richTextBox1.Text = PReader.StandardOutput.ReadToEnd()+ "\n" + "点按显示文件查看当前目录下文件。";
            stateRoot = 1;
            PReader.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get File/FolderName to TextBox
            textBox1.Text = this.listBox1.SelectedItem.ToString();
        }
    }
}
