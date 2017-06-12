using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Conver_To_Sbo_Icons
{
    using System.Drawing.Imaging;
    using System.IO;

    using ImageProcessor;
    using ImageProcessor.Imaging;
    using ImageProcessor.Imaging.Formats;

    /// <summary>
    /// The main form.
    /// </summary>
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.pictureBox1.AllowDrop = true;
            this.pictureBox2.AllowDrop = true;
        }

        string fileName;
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            Image image;
            try
            {
                 image = Image.FromFile(fileName);
            }
            catch
            {
                fileName = null;
                this.pictureBox1.Image = null;
                this.pictureBox2.Image = null;
                return;
            }

            this.demo1(image);
            //            this.pictureBox1.Image = image;
            this.pictureBox1.SizeMode= PictureBoxSizeMode.Zoom;
            this.pictureBox1.BackgroundImage = null;
//            this.pictureBox2.Image = image  ;
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.BackgroundImage = null;
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }

        public void demo1( Image inStream)
        {
//            byte[] photoBytes = File.ReadAllBytes(file);
            // Format is automatically detected though can be changed.
//            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Size size = new Size(16, 16);
//            using (MemoryStream inStream = new MemoryStream(photoBytes))
//            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (MemoryStream outStream2 = new MemoryStream())
                    {
                        // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                        {
                            // Load, resize, set the format and quality and save an image.
                            imageFactory
//                                .Pixelate(24)
                                .Load(inStream)
                                //                            .Resize(size)
                                .Constrain(size)
                                //                            .Format(format)
                                .BackgroundColor(Color.FromArgb(192, 220, 192))
                                .ReplaceColor(Color.Transparent, Color.FromArgb(192, 220, 192))
                                .Save(outStream)
                                .Resize(new ResizeLayer(new Size(26, 18), ResizeMode.BoxPad, AnchorPosition.Left))
                                .BackgroundColor(Color.FromArgb(192, 220, 192))
                                .Save(outStream2)
                                ;
                    }
                        // Do something with the stream.
                        pictureBox1.Image = Image.FromStream(outStream);
                        pictureBox2.Image = Image.FromStream(outStream2);

                    //                }
                }
                }
        }

        private void pictureBox1_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var img = pictureBox1.Image;
            if (img == null) return;
            if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
            {
            
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            if (fileName!=null)
            {
//                int position = fileName.LastIndexOf(".");
//                string fileName = fileName.Substring(0, position - 1);
////                活学活用：
//                System.IO.Path.GetFileName(fileName); //返回带扩展名的文件名  
                var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(this.fileName);
                var directoryName = System.IO.Path.GetDirectoryName(this.fileName);
                var path16 = Path.Combine(directoryName, fileNameWithoutExtension + "_16x16.bmp");
                var path26 = Path.Combine(directoryName, fileNameWithoutExtension + "_26x18.bmp");
                if (File.Exists(path16))
                {
                    File.Delete(path16);
                }
                if (File.Exists(path26))
                {
                    File.Delete(path26);
                }
                //                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                //                {
                //                    imageFactory.Load(pictureBox1.Image).Save(path16);
                //                }
                //                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                //                {
                //                    imageFactory.Load(pictureBox2.Image).Save(path26);
                //                }
                save24bt(pictureBox1.Image, path16);
                save24bt(pictureBox2.Image, path26);
                //                                pictureBox1.Image.Save(path16, ImageFormat.Bmp);
                //                                pictureBox2.Image.Save(path26, ImageFormat.Bmp);
                OpenFolderAndSelectFile(path16);

                //                fileName = null;
                //                this.pictureBox1.Image = null;
                //                this.pictureBox2.Image = null;
            }
        }

        public void save24bt(Image imgCheque,string filepath)
        {
            using (Bitmap blankImage = new Bitmap(imgCheque.Width, imgCheque.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(blankImage))
                {
                    g.DrawImageUnscaledAndClipped(imgCheque, new Rectangle(Point.Empty, imgCheque.Size));
                }
                blankImage.Save(filepath, ImageFormat.Bmp);
            }
        }

        /// <summary>
        /// 在资源管理器中打开文件夹并定位文件
        /// </summary>
        /// <param name="fileFullName"></param>
        private void OpenFolderAndSelectFile(String fileFullName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + fileFullName;
            System.Diagnostics.Process.Start(psi);
        }
    }
}
