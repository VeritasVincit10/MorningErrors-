using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace Morning_Errors_part2
{
    class Program
    {
        static private string outputFile = @"C:\Temp\server.csv";
        static void Main(string[] args)
        {
            if (File.Exists(outputFile)) {File.Delete(outputFile);}
          
            ReadLogFormatOne(@"C:\CSCUserData\Mamadou Tests\Business Process Service Test.txt");
            ReadLogFormatOne(@"\\pwcherwell01\C$\Program Files (x86)\Cherwell Service Management\CherwellLogs\ScheduledItems.log");
            ReadLogFormatOne(@"\\dwcherwell01\C$\Program Files (x86)\Cherwell Service Management\CherwellLogs\Services.log");
            ReadLogFormatOne(@"\\dwcherwell01\C$\Program Files (x86)\Cherwell Scheduled Processes\ProcessLog.log");
            ReadLogFormatOne(@"\\dwcherwell01\C$\Program Files (x86)\Cherwell Email.log");
            ReadLogFormatTwo(@"\\pwcherwell01\C$\Program Files (x86)\Cherwell Service Management\CherwellLogs\ServerService.log");
            ReadLogFormatTwo(@"\\dwcherwell01\C$\Program Files (x86)\ Cherwell Service Management\CherwellLogs\ServerService.log");
            ReadLogFormatThree(@"\\dwcherwell02\C$\Program Files (x86)\Cherwell Service Management\CherwellLogs\ServerService.log");
            SendEmail();
        }

        private static void SendEmail()
        {
            SmtpClient client = new SmtpClient("externalmail.cscinfo.com", 25);
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress("mamadou.fadiga@cscglobal.com"));
            email.Body = "Hey Rebecca, its Mamadou with today's errors";
            email.Subject = "Your Errors ma'am";
            if (File.Exists(outputFile))
            {
                email.Attachments.Add(new Attachment(outputFile));
            }
            else
            {
                // sends a message telling recipient of no errors
                email.Body = "Yayyy no errors today!";
            }
            email.IsBodyHtml = true;
            email.From = new MailAddress("desktopservices@cscinfo.com");
            client.Send(email);


        }

        private static void ReadLogFormatOne(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(fileName))
            {
                
                string[] lines = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines)
                {
                    // takes the line and splits the line into parts and scans accross 
                    // with the time set and cancatenated the main focus of  
                    // the time is the date to pick up the previous day's errors
                    string[] lineParts = line.Split(',');
                    string[] dateParts = lineParts[1].Split(' ');
                    string  dateToCheck = dateParts[0].Replace('\\', ' ');
                    
                    TimeSpan ts = DateTime.Now - DateTime.Parse(dateToCheck); 
                   
                    if (ts.Days == 1)
                    {
                        //The date is yesterday
                        if (line.Contains("ERRORS") || line.Contains("WARN") || line.Contains("INFO"))
                        {
                            Console.WriteLine(line);
                            sb.AppendLine(line);

                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            { 
                AppendAllText(sb.ToString());            
            }
            
        }

        private static void ReadLogFormatTwo(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(fileName))
            {
                string[] lines1 = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines1)
               {
                   string[] dateParts = line.Split(' ');
                   string[] timeParts = dateParts[1].Split(',');

                   TimeSpan ts = DateTime.Now - DateTime.Parse(dateParts[0]);

                   if (ts.Days == 1)
                    {
                       if (line.Contains("ERRORS") || line.Contains("WARN") || line.Contains("INFO"))
                        {
                            Console.WriteLine(line);
                            sb.AppendLine(line);
                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                AppendAllText(sb.ToString());
            }
        }

        private static void ReadLogFormatThree(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            if (File.Exists(fileName))
            {
                string[] lines1 = System.IO.File.ReadAllLines(fileName);
                foreach (var line in lines1)
               {
                   string[] dateParts = line.Split(' ');
                   string[] timeParts = dateParts[1].Split(',');

                   TimeSpan ts = DateTime.Now - DateTime.Parse(dateParts[0]);
                   if (ts.Days == 1)
                    {
                       if (line.Contains("ERRORS") || line.Contains("WARN") || line.Contains("INFO"))
                        {
                            Console.WriteLine(line);
                            sb.AppendLine(line);
                        }
                    }
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                AppendAllText(sb.ToString());
            }
         }
  
        private static void AppendAllText(string fileContents)
        {
            File.AppendAllText(outputFile, fileContents);
        }
    }

}
