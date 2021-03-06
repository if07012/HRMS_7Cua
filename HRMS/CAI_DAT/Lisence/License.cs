using System;
using System.Collections.Generic;
using System.Text;

namespace EVSoft.HRMSLicense
{
    public class License
    {
        //Thông số mặc định máy tính khiemvk
        private const string ProcessorId = "078BFBFF00020FF2";
        private const string BIOS_Version = "AMI10000505";
        private const string BaseBoard_Manufacturer = "ASUSTEKCOMPUTERINC";
        private const string BaseBoard_SerialNumber = "MB1234567890";
        private const string DiskDrive_Signature = "316674784";
        public const string UserName = "Evsoft";
        public const string Password = "Evsoft";

        /// <summary>
        /// Lấy thông tin phần cứng
        /// </summary>
        /// <returns> Dạng xxxxx-xxxxx-xxxxx-xxxxx-xxxxx</returns>
        public static string GetSystemInfo()
        {
            string text1 = SystemInfo.RunQuery("Processor", "ProcessorId");
            if (text1 == "")
                text1 = ProcessorId;
            string text2 = SystemInfo.RunQuery("BIOS", "Version");
            if (text2 == "")
                text2 = BIOS_Version;
            string text3 = SystemInfo.RunQuery("BaseBoard", "Manufacturer");
            if (text3 == "")
                text3 = BaseBoard_Manufacturer;
            string text4 = SystemInfo.RunQuery("BaseBoard", "SerialNumber");
            if (text4 == "")
                text4 = BaseBoard_SerialNumber;
            string text5 = SystemInfo.RunQuery("DiskDrive", "Signature");
            if (text5 == "")
                text5 = DiskDrive_Signature;
            int num = 1;
            string text = "";
            while (text.Length < 25)
            {
                switch (num)
                {
                    case 1:
                        text +=text1[0];
                        text1 = text1.Remove(0, 1);
                        num++;
                        break;

                    case 2:
                        text += text2[0];
                        text2 = text2.Remove(0, 1);
                        num++;
                        break;

                    case 3:
                        text += text3[0];
                        text3 = text3.Remove(0, 1);
                        num++;
                        break;

                    case 4:
                        text += text4[0];
                        text4 = text4.Remove(0, 1);
                        num++;
                        break;

                    case 5:
                        text += text5[0];
                        text5 = text5.Remove(0, 1);
                        num = 1;
                        break;
                }
            }
            return text.Insert(20, "-").Insert(15, "-").Insert(10, "-").Insert(5, "-");
        }

        public static string GenerationKey(string strSystemInfoKey)
        {
            try
            {
                string text1 = strSystemInfoKey.Split(new char[] { '-' })[0].ToUpper();
                string text2 = strSystemInfoKey.Split(new char[] { '-' })[1].ToUpper();
                string text3 = strSystemInfoKey.Split(new char[] { '-' })[2].ToUpper();
                string text4 = strSystemInfoKey.Split(new char[] { '-' })[3].ToUpper();
                string text5 = strSystemInfoKey.Split(new char[] { '-' })[4].ToUpper();

                //Mã hóa key
                Encryption enc = new Encryption(UserName, Password);
                text1 = SystemInfo.RemoveUseLess(enc.Encrypt(text1).ToUpper());
                text2 = SystemInfo.RemoveUseLess(enc.Encrypt(text2).ToUpper());
                text3 = SystemInfo.RemoveUseLess(enc.Encrypt(text3).ToUpper());
                text4 = SystemInfo.RemoveUseLess(enc.Encrypt(text4).ToUpper());
                text5 = SystemInfo.RemoveUseLess(enc.Encrypt(text5).ToUpper());

                return text1.Substring(0, 5) + "-" + text2.Substring(0, 5) + "-" + text3.Substring(0, 5) + "-" + text4.Substring(0, 5) + "-" + text5.Substring(0, 5);
            }
            catch(Exception ex)
            {
                return "";
            }
        }
    }
}
