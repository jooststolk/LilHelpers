using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Tools.ClassLibrary
{
    public static class QRcode
    {
        public static Bitmap createQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(30);
            return qrCodeImage;
        }
        public static void Print(Image qrImage, string headerText)
        {
            if (qrImage == null)
            {
                MessageBox.Show("No QR code image to print." + ex.Message, "No QR Image!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Or brew your own and add logging etc.
                return;
            }

            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (sender, e) => PrintPage(e, qrImage, headerText);

            PrintDialog printDialog = new PrintDialog
            {
                Document = pd
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }

        private static void PrintPage(PrintPageEventArgs e, Image qrImage, string headerText)
        {
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            int padding = 20;

            SizeF headerSize = e.Graphics.MeasureString(headerText, headerFont);
            int headerHeight = (int)headerSize.Height + padding;

            float scale = 0.25f;
            int imgWidth = (int)(qrImage.Width * scale);
            int imgHeight = (int)(qrImage.Height * scale);

            Rectangle printArea = e.MarginBounds;
            int imgX = printArea.X + (printArea.Width - imgWidth) / 2;
            int imgY = printArea.Y + headerHeight;

            int headerX = printArea.X + (printArea.Width - (int)headerSize.Width) / 2;
            int headerY = printArea.Y;
            e.Graphics.DrawString(headerText, headerFont, Brushes.Black, new PointF(headerX, headerY));
            e.Graphics.DrawImage(qrImage, new Rectangle(imgX, imgY, imgWidth, imgHeight));
        }

        public static void Save(Image qrImage, string folderPath, string filePath)
        {
            if (qrImage == null)
            { 
                return;
            }

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    qrImage.Save(ms, ImageFormat.Bmp);
                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        qrImage.Dispose(); // Optional: dispose original image early

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }

                        bmp.Save(filePath, ImageFormat.Jpeg);
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error saving QR code: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Or brew your own and add logging etc.
            }
        }
    }
}
