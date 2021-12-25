using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace rightclickss
{

    class Program
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static void Main(string[] args)
        {
            // Type your path for your screenshots folder
            Console.WriteLine("Set screenshots folder location:");
            Console.WriteLine("e.g D:/FOLDER_NAME");

            string screenshots_folder = Console.ReadLine();
        

            while (true)
            {
                Thread.Sleep(100);

                for (int i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    // replace -32767 with 32769 for windows 10.
                    if (keyState == 1 || keyState == 32769)
                    {
                        if (i == 2)
                        {
                            CaptureScreen(screenshots_folder);
                        }

                        break;
                    }
                }
            }
        }

        private static void CaptureScreen(string folder_path)
        {
            int width = 0;
            int height = 0;
            ManagementObjectSearcher mydisplayResolution = new ManagementObjectSearcher("SELECT CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController");
            foreach (ManagementObject record in mydisplayResolution.Get())
            {
                width = Convert.ToInt32(record["CurrentHorizontalResolution"]);
                height = Convert.ToInt32(record["CurrentVerticalResolution"]);
            }
            string nameFile = getCurrentDate();
            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file
            Image img = sc.CaptureScreen();
            // capture this window, and save it
            sc.CaptureWindowToFile(Process.GetCurrentProcess().MainWindowHandle,folder_path+"/"+nameFile+".png", ImageFormat.Png,width,height);
        }

        private static string getCurrentDate()
        {
            DateTime now = DateTime.Now;
            string current_seconds,current_minutes,current_hrs,current_days, current_months;
            // making sure the file name is easy to read
            // e.g 6:17:25 to 061725 
            // seconds
            int seconds = now.Second;
            if (seconds < 10) current_seconds = "0" + seconds.ToString();
            else current_seconds = seconds.ToString();
            // minutes
            int minutes = now.Minute;
            if (minutes < 10) current_minutes = "0" + minutes.ToString();
            else current_minutes = minutes.ToString();
            // hours 
            int hrs = now.Hour;
            if (hrs < 10) current_hrs = "0" + hrs.ToString();
            else current_hrs = hrs.ToString();
            // days
            int days = now.Day;
            if (days < 10) current_days = "0" + days.ToString();
            else current_days = days.ToString();
            // month
            int month = now.Month;
            if (month < 10) current_months = "0" + month.ToString();
            else current_months = days.ToString();
            // year is easy to read
            string year = now.Year.ToString();

            
            return (year+current_months+current_days+current_hrs+current_minutes+current_seconds);
        }
    }
}
