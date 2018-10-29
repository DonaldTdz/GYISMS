using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Helpers
{
    public class QRCodeHelper
    {
        public static QRCodeGenerator qrGenerator = new QRCodeGenerator();

        public static void GenerateQRCode(string code, string savePath, QRCodeGenerator.ECCLevel level, int pixelsPerModule)
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, level);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule);
            qrCodeImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
