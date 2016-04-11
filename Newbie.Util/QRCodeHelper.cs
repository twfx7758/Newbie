using System.Web;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using ZXing.QrCode;

namespace Newbie.Util
{
    /// <summary>
    /// 二维码帮助类
    /// </summary>
    public static class QRCodeHelper
    {
        /// <summary>
        /// 生成QR二维码
        /// </summary>
        /// <param name="context">内容</param>
        /// <param name="size">尺寸</param>
        public static Bitmap Create(string context, int size)
        {
            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = size,
                Height = size,
                ErrorCorrection = ErrorCorrectionLevel.H,
                Margin = 3,
                PureBarcode = true
            };
           var writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options,
            };
           return writer.Write(context);
        }

        public static Bitmap Create(string context, int size,int margin)
        {
            var options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = size,
                Height = size,
                ErrorCorrection = ErrorCorrectionLevel.H,
                Margin = margin,
                PureBarcode = true
            };
            var writer = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options,
            };
            return writer.Write(context);
        }
        /// <summary>
        /// 解析QR二维码
        /// </summary>
        public static string Decode(Bitmap bitmap)
        {
            BarcodeReader barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options = new DecodingOptions { TryHarder = true, PureBarcode = true }
            };
            barcodeReader.Options.PossibleFormats = new List<BarcodeFormat>() { BarcodeFormat.QR_CODE };

            var result = barcodeReader.Decode(bitmap);

            if (result != null)
            {
                return  HttpUtility.UrlDecode(result.Text, Encoding.UTF8);
            }

            return "";
        }
    }
}