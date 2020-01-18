using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Calender.Data
{
    public static class Calender
    {
        static int currentIndex = 0;
        [DataContract(Name = "PanchangData", Namespace = "http://www.jyotishcalendar.com")]
       public class PanchangData
        {
            [DataMember(Name = "Year")]
            public int Year;
            [DataMember(Name = "Month")]
            public int Month;
            [DataMember(Name = "Day")]
            public int Day;
            [DataMember(Name = "FieldValues")]
            public String[] _fieldValues;
            public PanchangData()
            {

            }
            public PanchangData(int year, int month, int day)
            {
                _fieldValues = new String[(int)FieldType.MaxFields];
                Year = year;
                Day = day;
                Month = month;
            }
        };

        [DataContract(Name = "YearlyPanchangData", Namespace = "http://www.jyotishcalendar.com")]
        [KnownType(typeof(PanchangData))]
        class YearlyPanchangData
        {
            [DataMember(Name = "PanchangDataArray")]
            public PanchangData[] _panchangData;
        }

        struct PatternToScan
        {
            public String _RegExp; // Regexp to apply to this line
            public bool _Skip; // Skip this entry
            public FieldType _FieldType; // Type to initialize
            public PatternToScan(String regExp, bool skip, FieldType fieldType)
            {
                _RegExp = regExp;
                _Skip = skip;
                _FieldType = fieldType;
            }
        };

        static PatternToScan[] scanPatterns;

        public static PanchangData GetCalendarDataPerCityAndYear(int date, int month, int Year, String UrlToken, TimeZoneValues timeZone)
        {
            var dayResult = new PanchangData();
            HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
            YearlyPanchangData yearPanchangData = new YearlyPanchangData();
            PanchangData[] panchangData = new PanchangData[12 * 31];
            yearPanchangData._panchangData = panchangData;
            int day = 0;
            String fileName = String.Format("{0}RssFeed\\Calendar-{1}-{2}.html", AppDomain.CurrentDomain.BaseDirectory,month,Year);
            DataContractSerializer ser = new DataContractSerializer(typeof(YearlyPanchangData));
            try
            {
                HtmlDocument document = new HtmlDocument();
                if (File.Exists(fileName))
                {
                    document.Load(fileName);
                }
                else
                {
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    String url = String.Format("http://www.mypanchang.com/phppanchang.php?yr={0}&cityhead=&cityname={2}&monthtype=0&mn={1}", Year, month - 1, UrlToken);
                    try
                    {
                        var data = web.Load(url);
                        document = data;
                        var sr = new StreamWriter(fs);
                        sr.Write(data.Text);
                    }
                    catch (Exception exp)
                    {
                        
                    }
                }
                day = 0;
                PanchangData pData = null;
                String log = null;
                foreach (HtmlNode node in document.DocumentNode.SelectNodes("//table"))
                {
                    HtmlNodeCollection coll = node.ChildNodes;
                    foreach (HtmlNode data in coll)
                    {
                        log += data.InnerText.Trim();
                        log += "\n";
                        log += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
                        log += "\n";
                        //Console.WriteLine(log);
                        String input = data.InnerText;
                        String pattern1 = @"Panchang for";
                        Match match = Regex.Match(input, pattern1);
                        if (match.Success)
                        {
                            if (pData != null)
                            {
                                //    Console.WriteLine("Year {0} Month {1} Day {2}", Year, month, day);
                                for (int j = 0; j < (int)(FieldType.MaxFields); j++)
                                {
                                    if ((j != (int)FieldType.None) && (j != (int)FieldType.TamilYear) && (j != (int)FieldType.Festival))
                                    {
                                        if (pData._fieldValues[j] == null)
                                        {
                                            Console.WriteLine("Field Value {0} is null", ((FieldType)j).ToString());
                                            Console.WriteLine(log);
                                        }
                                    }

                                    //Console.WriteLine("{0}: {1}", (FieldType)j, pData._fieldValues[j]);
                                }
                                // Stash away the old data
                                panchangData[(month - 1) * 31 + (day - 1)] = pData;
                            };
                            // A new day got started.
                            pData = new PanchangData(Year, month, day + 1);
                            day++;
                            log = null;
                            //if (day > 30) break;
                        }

                        pattern1 = @"Shalivahan Shaka: (\d\d\d\d) \((\w+) Samvatsara\),\&nbsp;(\w+) Year \(North India\) (\d\d\d\d),\&nbsp;(\w+ \w+) \(Gujarat\) (\d\d\d\d),\&nbsp; Ayana:(\w+) \&nbsp;Ritu:(\w+),  Vedic Ritu:(\w+), Amavasyant\s+(\w+) (\w+) Paksha,\&nbsp;Tamil Month: (\w+)";
                        // Shalivahan Shaka: 1937 (Manmatha Samvatsara),&nbsp;Vikrami Year (North India) 2072,&nbsp;Vikram Samvat (Gujarat) 2071,&nbsp; Ayana:Uttarayana &nbsp;Ritu:Grishma,  Vedic Ritu:Grishma, Amavasyant Adhika Ashaadha Shukla Paksha,&nbsp;Tamil Month: Aani

                        match = Regex.Match(input, pattern1);
                        currentIndex = 0;
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.TamilYear] = match.Groups[2].Value + " " + match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.NorthYear] = match.Groups[3].Value + " " + match.Groups[4].Value;
                            pData._fieldValues[(int)FieldType.GujaratYear] = match.Groups[5].Value + " " + match.Groups[6].Value;
                            pData._fieldValues[(int)FieldType.Ayana] = match.Groups[7].Value;
                            pData._fieldValues[(int)FieldType.Ritu] = match.Groups[8].Value;
                            pData._fieldValues[(int)FieldType.VedicRitu] = match.Groups[9].Value;
                            pData._fieldValues[(int)FieldType.SanskritMonth] = match.Groups[10].Value;
                            pData._fieldValues[(int)FieldType.Paksha] = match.Groups[11].Value;
                            pData._fieldValues[(int)FieldType.TamilMonth] = match.Groups[12].Value;
                            pData._fieldValues[(int)FieldType.Festival] = null;
                        }

                        // For some cases there is an extra word after Amavasyant
                        String pattern1_5 = @"Shalivahan Shaka: (\d\d\d\d) \((\w+) Samvatsara\),\&nbsp;(\w+) Year \(North India\) (\d\d\d\d),\&nbsp;(\w+ \w+) \(Gujarat\) (\d\d\d\d),\&nbsp; Ayana:(\w+) \&nbsp;Ritu:(\w+),  Vedic Ritu:(\w+), Amavasyant\s+\w+ (\w+) (\w+) Paksha,\&nbsp;Tamil Month: (\w+)";
                        // Shalivahan Shaka: 1937 (Manmatha Samvatsara),&nbsp;Vikrami Year (North India) 2072,&nbsp;Vikram Samvat (Gujarat) 2071,&nbsp; Ayana:Uttarayana &nbsp;Ritu:Grishma,  Vedic Ritu:Grishma, Amavasyant Adhika Ashaadha Shukla Paksha,&nbsp;Tamil Month: Aani

                        match = Regex.Match(input, pattern1_5);
                        currentIndex = 0;
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.TamilYear] = match.Groups[2].Value + " " + match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.NorthYear] = match.Groups[3].Value + " " + match.Groups[4].Value;
                            pData._fieldValues[(int)FieldType.GujaratYear] = match.Groups[5].Value + " " + match.Groups[6].Value;
                            pData._fieldValues[(int)FieldType.Ayana] = match.Groups[7].Value;
                            pData._fieldValues[(int)FieldType.Ritu] = match.Groups[8].Value;
                            pData._fieldValues[(int)FieldType.VedicRitu] = match.Groups[9].Value;
                            pData._fieldValues[(int)FieldType.SanskritMonth] = match.Groups[10].Value;
                            pData._fieldValues[(int)FieldType.Paksha] = match.Groups[11].Value;
                            pData._fieldValues[(int)FieldType.TamilMonth] = match.Groups[12].Value;
                            pData._fieldValues[(int)FieldType.Festival] = null;
                        }

                        // @"Sunrise:07:36:10Sunset:17:08:38Moonrise:23:54:46";
                        String pattern2 = @"Sunrise:([\d:]+)Sunset:([\d:]+)Moonrise:([\d:]+)";
                        match = Regex.Match(input, pattern2);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.Sunrise] = match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.Sunset] = match.Groups[2].Value;
                            pData._fieldValues[(int)FieldType.Moonrise] = match.Groups[3].Value;
                        }

                        pattern2 = @"Sunrise:([\d:]+)Sunset:([\d:]+)Moonrise:None";
                        match = Regex.Match(input, pattern2);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.Sunrise] = match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.Sunset] = match.Groups[2].Value;
                            pData._fieldValues[(int)FieldType.Moonrise] = "None";
                        }

                        String pattern3 = @"Sun:(\w+)Entering";
                        match = Regex.Match(input, pattern3);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.SunRasi] = match.Groups[1].Value;
                        }

                        String pattern4 = @"Chandra:(\w+)Entering";
                        match = Regex.Match(input, pattern4);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.MoonRasi] = match.Groups[1].Value;
                        }

                        String pattern5 = @"Chandra:(\w+)Entering";
                        match = Regex.Match(input, pattern5);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.MoonRasi] = match.Groups[1].Value;
                        }

                        pattern1 = @"Tithi:([\w ]+)End time:[\w \d:+]+Nakshatra:([\w\.]+) ";
                        match = Regex.Match(input, pattern1);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.Tithi] = match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.Nakshatra] = match.Groups[2].Value;
                        }
                        //Rahukalam:11:10:50-12:22:24Yamagandam:14:45:31-15:57:05Gulikai:08:47:43-09:59:17Abhijit Muhurta:12:03:19-12:41:29
                        pattern1 = @"Rahukalam:([\d:-]+)Yamagandam:([\d:-]+)Gulikai:([\d:-]+)";
                        match = Regex.Match(input, pattern1);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.RahuKalam] = match.Groups[1].Value;
                            pData._fieldValues[(int)FieldType.YamaGandam] = match.Groups[2].Value;
                            pData._fieldValues[(int)FieldType.Gulikai] = match.Groups[3].Value;
                        }

                        pattern1 = @"Yoga:(\w+)End";
                        match = Regex.Match(input, pattern1);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.Yoga] = match.Groups[1].Value;
                        }

                        pattern1 = @"Karana:(\w+)End";
                        match = Regex.Match(input, pattern1);
                        if (match.Success)
                        {
                            pData._fieldValues[(int)FieldType.Karana] = match.Groups[1].Value;
                        }

                    }
                }
                panchangData[(month - 1) * 31 + (day - 1)] = pData;
                dayResult = panchangData.Where(x => !Object.ReferenceEquals(x, null) && x.Day == date).FirstOrDefault();
               
            }
            catch (Exception e)
            {
            }
            return dayResult;
        }
    }
}
