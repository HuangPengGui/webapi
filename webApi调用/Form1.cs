using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace webApi调用
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.AllowDrop = true;//设置控件允许拖拽文件
        }
        public string base64_img = null;
        private void set_result(string result) {
            if (ImgUtils.IsBase64(result))
            {
                Image img = ImgUtils.ConvertBase64ToImage(result);
                pictureBox2.Image = img;
                richTextBox1.Visible = false;
                pictureBox2.Visible = true;
            }
            else
            {
                richTextBox1.Text = result;
                richTextBox1.Visible = true;
                pictureBox2.Visible = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
///"https://aistudio.baidu.com/serving/online/10173?apiKey=ba835c6c-e03e-4b7a-b844-51f749386416";
            string url = DES.Decrypt(apiKey_input.Text);
            string content= "{\"input\": \"" + base64_img + "\" }";
            string result = NetUtils.Post(url, content);
            set_result(result);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.Filter = "Image Files (*.jpg;*.bmp;*.png)|*.jpg;*.bmp;*.png";
            ofg.Multiselect = false;
            //调用对象的函数
            if (ofg.ShowDialog() == DialogResult.OK)
            {
                //获取文件的路径
                string filepath = ofg.FileName;
                read_image(filepath);
            }
        }
        private void read_image(string filepath) {

            using (FileStream file = new FileStream(filepath, FileMode.Open))
            {
                Image img = Image.FromStream(file);
                img = ImgUtils.resizeImg(img, pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = img;
                base64_img = ImgUtils.ConvertImageToBase64(pictureBox1.Image);
                set_result(base64_img);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string encrypt=DES.Encrypt(apiKey_input.Text);
            string decrypt=DES.Decrypt(encrypt);
            richTextBox1.Text = encrypt+"\n"+ decrypt;
            apiKey_input.Text = encrypt;
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            read_image(s[s.Length-1]);
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
