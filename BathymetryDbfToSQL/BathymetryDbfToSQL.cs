using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using ICSharpCode.SharpZipLib.Zip;

namespace BathymetryDbfToSQL
{
    public partial class BathymetryDbfToSQL : Form
    {
        List<ColorVal> ColorValList = new List<ColorVal>();
        string FillColorValueStr = "";
        public BathymetryDbfToSQL()
        {
            InitializeComponent();
            ColorValList = FillColorValues();
            StringBuilder sb = new StringBuilder();
            foreach (ColorVal ColVal in ColorValList)
            {
                sb.AppendLine(@"	<Style id=""C_" + ColVal.Value.ToString().Replace(".", "_") + @""">");
                sb.AppendLine(@"		<LineStyle>");
                sb.AppendLine(@"			<color>" + ColVal.ColorHexStr + "</color>");
                sb.AppendLine(@"			<width>1</width>");
                sb.AppendLine(@"		</LineStyle>");
                sb.AppendLine(@"		<PolyStyle>");
                sb.AppendLine(@"			<color>" + ColVal.ColorHexStr + "</color>");
                sb.AppendLine(@"			<width>1</width>");
                sb.AppendLine(@"		</PolyStyle>");
                sb.AppendLine(@"		<IconStyle>");
                sb.AppendLine(@"			<color>" + ColVal.ColorHexStr + "</color>");
                sb.AppendLine(@"			<scale>1.3</scale>");
                sb.AppendLine(@"			<Icon>");
                sb.AppendLine(@"				<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>");
                sb.AppendLine(@"			</Icon>");
                sb.AppendLine(@"			<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/>");
                sb.AppendLine(@"		</IconStyle>");
                sb.AppendLine(@"		<LabelStyle>");
                sb.AppendLine(@"			<color>" + ColVal.ColorHexStr + "</color>");
                sb.AppendLine(@"		</LabelStyle>");
                sb.AppendLine(@"	</Style>");
            }

            FillColorValueStr = sb.ToString();
        }

        private void butListDBFFiles_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (FileInfo f in new DirectoryInfo(textBoxDbfFilesDirPath.Text).GetFiles())
            {
                sb.AppendLine(f.Name);
            }

            richTextBoxResults.Text = sb.ToString();
        }

        private void butStartTransfer_Click(object sender, EventArgs e)
        {
            bool SaveInDB = true;
            bool ShowResInRTB = false;
            int NumberOfFile = 1800;
            bool DoLine = true;
            bool DoSound = true;
            int NumberOfRecordBatchSave = 1000;
            Excecute(SaveInDB, ShowResInRTB, NumberOfFile, DoLine, DoSound, NumberOfRecordBatchSave);
        }

        private MemoryStream FileToMemoryStream(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            MemoryStream myMemoryStream = new MemoryStream();
            FileStream myFileStream = fi.OpenRead();
            myFileStream.CopyTo(myMemoryStream);
            myFileStream.Flush();
            myMemoryStream.Position = 0;
            return myMemoryStream;
        }

        private void Excecute(bool SaveInDB, bool ShowResInRTB, int NumberOfFile, bool DoLine, bool DoSound, int NumberOfRecordBatchSave)
        {
            StringBuilder sb = new StringBuilder();

            int DoNumbRows = 100000000;

            int CountFile = 0;
            DateTime dtFileAfterDate;
            if (!DateTime.TryParse(textBoxFileAfterDate.Text, out dtFileAfterDate))
            {
                MessageBox.Show("Wrong date format");
                return;
            }
            foreach (FileInfo f in new DirectoryInfo(textBoxDbfFilesDirPath.Text).GetFiles().Where(d => d.CreationTime > dtFileAfterDate))
            {
                richTextBoxResults.AppendText("Loading [" + f.Name.Substring(0, f.Name.Length - 4) + "].\r\n");
                Application.DoEvents();

                double LongitudeMin = 180;
                double LongitudeMax = -180;
                double LatitudeMin = 90;
                double LatitudeMax = -90;

                lblCurrentFile.Text = f.Name;
                lblCurrentFile.Refresh();
                Application.DoEvents();
                if (f.Name.Contains("SOUNDG"))
                {
                    if (!DoSound)
                        continue;

                    FileInfo fi = new FileInfo(f.FullName);
                    MemoryStream ms = FileToMemoryStream(f.FullName);
                    byte[] ByteArray = ms.ToArray();
                    int pos = 0;

                    Int32 TotalRowCount = BitConverter.ToInt32(ByteArray, 4);

                    CHSChart chsChart = new CHSChart();
                    chsChart.CHSChartName = f.Name.Substring(0, f.Name.Length - 4);

                    if (CHSChartInDB(chsChart.CHSChartName, TotalRowCount))
                        continue;

                    CountFile += 1;
                    if (CountFile > NumberOfFile)
                    {
                        richTextBoxResults.AppendText("Maximum number of file done ...");
                        return;
                    }
                    chsChart.CHSFileName = f.Name;
                    if (SaveInDB)
                    {
                        using (BathymetryEntities be = new BathymetryEntities())
                        {
                            be.CHSCharts.Add(chsChart);
                            try
                            {
                                be.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error Message [" + ex.Message + "]");
                            }
                        }
                    }

                    sb.AppendLine();

                    pos = 32;
                    sb.Append(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 7) + "\t");
                    pos = 64;
                    sb.Append(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 7) + "\t");
                    pos = 96;
                    sb.Append(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 7) + "\t");
                    pos = 130;
                    sb.AppendLine();

                    int CurrentRow = 0;
                    List<CHSDepth> cdListToAdd = new List<CHSDepth>();

                    while (pos < ByteArray.Length)
                    {
                        CurrentRow += 1;

                        if (CurrentRow > DoNumbRows)
                            break;

                        lblStatusTxt.Text = "Reading row " + CurrentRow + " of " + TotalRowCount;
                        lblStatusTxt.Refresh();
                        Application.DoEvents();
                        CHSDepth cd = new CHSDepth();
                        cd.CHSChartID = chsChart.CHSChartID;
                        cd.Longitude = double.Parse(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 19));
                        pos += 19;
                        cd.Latitude = double.Parse(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 19));
                        pos += 19;
                        cd.Depth = double.Parse(ASCIIEncoding.ASCII.GetString(ByteArray, pos, 19));
                        pos += 19;
                        pos += 1;
                        cd.LineValue = -999;

                        if (LongitudeMin > cd.Longitude)
                            LongitudeMin = (double)cd.Longitude;

                        if (LongitudeMax < cd.Longitude)
                            LongitudeMax = (double)cd.Longitude;

                        if (LatitudeMin > cd.Latitude)
                            LatitudeMin = (double)cd.Latitude;

                        if (LatitudeMax < cd.Latitude)
                            LatitudeMax = (double)cd.Latitude;

                        cdListToAdd.Add(cd);

                        // this will save at every NumberOfRecordBatchSave depth
                        if (CurrentRow % NumberOfRecordBatchSave == 0)
                        {
                            lblStatusTxt.Text = "Reading row " + CurrentRow + " of " + TotalRowCount + " --- Transfering " + NumberOfRecordBatchSave.ToString() + " rows to SQL DB";
                            lblStatusTxt.Refresh();
                            Application.DoEvents();
                            chsChart.LongitudeMax = LongitudeMax;
                            chsChart.LongitudeMin = LongitudeMin;
                            chsChart.LatitudeMax = LatitudeMax;
                            chsChart.LatitudeMin = LatitudeMin;
                            using (BathymetryEntities be = new BathymetryEntities())
                            {
                                be.CHSDepths.AddRange(cdListToAdd);
                                be.SaveChanges();
                            }

                            cdListToAdd = new List<CHSDepth>();
                        }

                        sb.Append(cd.Longitude + "\t");
                        sb.Append(cd.Latitude + "\t");
                        sb.Append(cd.Depth + "\t");
                        sb.AppendLine();
                    }
                    lblStatusTxt.Text = "Saving last records";
                    lblStatusTxt.Refresh();
                    Application.DoEvents();
                    chsChart.LongitudeMax = LongitudeMax;
                    chsChart.LongitudeMin = LongitudeMin;
                    chsChart.LatitudeMax = LatitudeMax;
                    chsChart.LatitudeMin = LatitudeMin;
                    using (BathymetryEntities be = new BathymetryEntities())
                    {
                        be.CHSDepths.AddRange(cdListToAdd);
                        be.SaveChanges();
                    }

                    lblStatusTxt.Text = "Done ...";
                    lblStatusTxt.Refresh();
                    Application.DoEvents();

                    if (ShowResInRTB)
                    {
                        richTextBoxResults.Text = sb.ToString();
                    }
                    if (NumberOfFile == CountFile)
                        break;
                }
                else
                {
                    if (!DoLine)
                        continue;

                    OleDbConnection oConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=C:\CSSP\CHS Bathymetry\File_dbf;Extended Properties=dBase III");
                    OleDbCommand command = new OleDbCommand("select * from " + f.Name, oConn);

                    oConn.Open();

                    DataTable dt = new DataTable();
                    dt.Load(command.ExecuteReader());

                    oConn.Close();  //close connection to the .dbf file

                    //create a reader for the datatable
                    DataTableReader reader = dt.CreateDataReader();

                    CHSChart chsChart = new CHSChart();
                    chsChart.CHSChartName = f.Name.Substring(0, f.Name.Length - 4);

                    if (CHSChartInDB(chsChart.CHSChartName, dt.Rows.Count))
                        continue;

                    CountFile += 1;
                    if (CountFile > NumberOfFile)
                    {
                        richTextBoxResults.AppendText("Maximum number of file done ...");
                        return;
                    }
                    chsChart.CHSFileName = f.Name;
                    if (SaveInDB)
                    {
                        using (BathymetryEntities be = new BathymetryEntities())
                        {
                            be.CHSCharts.Add(chsChart);
                            try
                            {
                                be.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error Message [" + ex.Message + "]");
                            }
                        }
                    }

                    sb.AppendLine();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        sb.Append(dt.Columns[i].ColumnName + "\t");
                    }
                    sb.AppendLine();

                    List<CHSDepth> cdListToAdd = new List<CHSDepth>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i > DoNumbRows)
                            break;

                        lblStatusTxt.Text = "Reading row " + i.ToString() + " of " + dt.Rows.Count;
                        lblStatusTxt.Refresh();
                        Application.DoEvents();
                        CHSDepth cd = new CHSDepth();
                        cd.CHSChartID = chsChart.CHSChartID;
                        cd.LineValue = (double)dt.Rows[i].ItemArray[0];
                        cd.Depth = (double)dt.Rows[i].ItemArray[1];
                        cd.Longitude = (double)dt.Rows[i].ItemArray[2];
                        cd.Latitude = (double)dt.Rows[i].ItemArray[3];

                        if (LongitudeMin > cd.Longitude)
                            LongitudeMin = (double)cd.Longitude;

                        if (LongitudeMax < cd.Longitude)
                            LongitudeMax = (double)cd.Longitude;

                        if (LatitudeMin > cd.Latitude)
                            LatitudeMin = (double)cd.Latitude;

                        if (LatitudeMax < cd.Latitude)
                            LatitudeMax = (double)cd.Latitude;

                        cdListToAdd.Add(cd);

                        // this will save at every NumberOfRecordBatchSave depth
                        if (i % NumberOfRecordBatchSave == 0)
                        {
                            lblStatusTxt.Text = "Reading row " + i.ToString() + " of " + dt.Rows.Count + " --- Transfering " + NumberOfRecordBatchSave.ToString() + " rows to SQL DB";
                            lblStatusTxt.Refresh();
                            Application.DoEvents();
                            chsChart.LongitudeMax = LongitudeMax;
                            chsChart.LongitudeMin = LongitudeMin;
                            chsChart.LatitudeMax = LatitudeMax;
                            chsChart.LatitudeMin = LatitudeMin;
                            using (BathymetryEntities be = new BathymetryEntities())
                            {
                                be.CHSDepths.AddRange(cdListToAdd);
                                be.SaveChanges();
                            }

                            cdListToAdd = new List<CHSDepth>();
                        }

                        sb.Append(cd.LineValue + "\t");
                        sb.Append(cd.Depth + "\t");
                        sb.Append(cd.Longitude + "\t");
                        sb.Append(cd.Latitude + "\t");
                        sb.AppendLine();
                    }
                    lblStatusTxt.Text = "Saving last records";
                    lblStatusTxt.Refresh();
                    Application.DoEvents();
                    chsChart.LongitudeMax = LongitudeMax;
                    chsChart.LongitudeMin = LongitudeMin;
                    chsChart.LatitudeMax = LatitudeMax;
                    chsChart.LatitudeMin = LatitudeMin;
                    using (BathymetryEntities be = new BathymetryEntities())
                    {
                        be.CHSDepths.AddRange(cdListToAdd);
                        be.SaveChanges();
                    }

                    reader.Close();

                    lblStatusTxt.Text = "Done ...";
                    lblStatusTxt.Refresh();
                    Application.DoEvents();

                    if (ShowResInRTB)
                    {
                        richTextBoxResults.Text = sb.ToString();
                    }
                    if (NumberOfFile == CountFile)
                        break;
                }
                if (SaveInDB)
                {

                }
                sb.AppendLine("Done ... " + f.Name);
            }

        }

        private bool CHSChartInDB(string ChartName, int TotalRowCount)
        {
            using (BathymetryEntities be = new BathymetryEntities())
            {

                // checking if chart already exist
                CHSChart chsChartExist = (from c in be.CHSCharts
                                          where c.CHSChartName == ChartName
                                          select c).FirstOrDefault();

                if (chsChartExist != null)
                {
                    int CountOfCHSDepth = (from d in be.CHSDepths
                                           where d.CHSChartID == chsChartExist.CHSChartID
                                           select d).Count();

                    if (CountOfCHSDepth == TotalRowCount)
                    {
                        richTextBoxResults.AppendText("Chart [" + ChartName + "] already loaded.\r\n");
                        Application.DoEvents();
                        return true;
                    }

                    be.CHSCharts.Remove(chsChartExist);
                    try
                    {
                        be.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        richTextBoxResults.AppendText("Error while trying to delete [" + ChartName + "]\r\n");
                        richTextBoxResults.AppendText("Error message [" + ex.Message + "]\r\n");
                    }
                }
            }

            return false;
        }

        private void butListChartsInDB_Click(object sender, EventArgs e)
        {
            using (BathymetryEntities be = new BathymetryEntities())
            {
                List<CHSChart> chsChartList = (from c in be.CHSCharts select c).ToList<CHSChart>();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("List of CHSCharts found in DB. Count = [" + chsChartList.Count + "]");
                sb.AppendLine();
                foreach (CHSChart c in chsChartList)
                {
                    sb.AppendLine(c.CHSChartName + " ID = [" + c.CHSChartID + "] Min Long = [" + c.LongitudeMin + "] Max Long = [" + c.LongitudeMax + "] Min Lat = [" + c.LatitudeMin + "] Max Lat = [" + c.LatitudeMax + "]");
                }

                richTextBoxResults.Text = sb.ToString();
            }
        }


        private string GetColorStyleID(double ColVal, List<ColorVal> ColorValList)
        {
            string ColValStr = "";
            ColorVal lowVal = (from cv in ColorValList where cv.Value <= ColVal orderby cv.Value descending select cv).First();
            ColorVal highVal = (from cv in ColorValList where cv.Value >= ColVal orderby cv.Value select cv).First();

            if ((ColVal - lowVal.Value) < (highVal.Value - ColVal))
            {
                ColValStr = "C_" + lowVal.Value.ToString().Replace(".", "_");
            }
            else
            {
                ColValStr = "C_" + highVal.Value.ToString().Replace(".", "_"); ;
            }
            return ColValStr;
        }

        private List<ColorVal> FillColorValues()
        {
            List<ColorVal> ColorValList = new List<ColorVal>();
            ColorValList.Add(new ColorVal() { Value = -100000, ColorHexStr = "ffffffff" });
            ColorValList.Add(new ColorVal() { Value = 0, ColorHexStr = "ffffffff" });
            ColorValList.Add(new ColorVal() { Value = 0.1, ColorHexStr = "ff0000ff" });
            ColorValList.Add(new ColorVal() { Value = 0.3, ColorHexStr = "ff0033ff" });
            ColorValList.Add(new ColorVal() { Value = 0.5, ColorHexStr = "ff0066ff" });
            ColorValList.Add(new ColorVal() { Value = 0.8, ColorHexStr = "ff0099ff" });
            ColorValList.Add(new ColorVal() { Value = 1, ColorHexStr = "ff00ccff" });
            ColorValList.Add(new ColorVal() { Value = 2, ColorHexStr = "ff00ffff" });
            ColorValList.Add(new ColorVal() { Value = 3, ColorHexStr = "ff00ffcc" });
            ColorValList.Add(new ColorVal() { Value = 5, ColorHexStr = "ff00ff99" });
            ColorValList.Add(new ColorVal() { Value = 7, ColorHexStr = "ff00ff66" });
            ColorValList.Add(new ColorVal() { Value = 10, ColorHexStr = "ff00ff33" });
            ColorValList.Add(new ColorVal() { Value = 12, ColorHexStr = "ff00ff00" });
            ColorValList.Add(new ColorVal() { Value = 15, ColorHexStr = "ff00cc00" });
            ColorValList.Add(new ColorVal() { Value = 20, ColorHexStr = "ff009900" });
            ColorValList.Add(new ColorVal() { Value = 30, ColorHexStr = "ffff0000" });
            ColorValList.Add(new ColorVal() { Value = 45, ColorHexStr = "ffff0033" });
            ColorValList.Add(new ColorVal() { Value = 70, ColorHexStr = "ffff0066" });
            ColorValList.Add(new ColorVal() { Value = 100, ColorHexStr = "ffff0099" });
            ColorValList.Add(new ColorVal() { Value = 140, ColorHexStr = "ffff00cc" });
            ColorValList.Add(new ColorVal() { Value = 200, ColorHexStr = "ffff00ff" });
            ColorValList.Add(new ColorVal() { Value = 250, ColorHexStr = "ffcc00ff" });
            ColorValList.Add(new ColorVal() { Value = 400, ColorHexStr = "ff9900ff" });
            ColorValList.Add(new ColorVal() { Value = 600, ColorHexStr = "ff6600ff" });
            ColorValList.Add(new ColorVal() { Value = 900, ColorHexStr = "ff3300ff" });
            ColorValList.Add(new ColorVal() { Value = 1400, ColorHexStr = "ff0000ff" });
            ColorValList.Add(new ColorVal() { Value = 2000, ColorHexStr = "ffcccccc" });
            ColorValList.Add(new ColorVal() { Value = 3000, ColorHexStr = "ff999999" });
            ColorValList.Add(new ColorVal() { Value = 5000, ColorHexStr = "ff666666" });
            ColorValList.Add(new ColorVal() { Value = 7500, ColorHexStr = "ff333333" });
            ColorValList.Add(new ColorVal() { Value = 10000, ColorHexStr = "ff000000" });
            return ColorValList;
        }

        private void SaveInKMZFileStream(string KMZFileName, string KMLFileName, StringBuilder sbKML)
        {
            FileInfo fi = new FileInfo(KMZFileName);
            FileStream fs = fi.Create();
            ZipOutputStream zos = new ZipOutputStream(fs, sbKML.Length);
            byte[] zipByte = System.Text.Encoding.UTF8.GetBytes(sbKML.ToString());

            ZipEntry ze = new ZipEntry(KMLFileName);
            ze.DateTime = DateTime.Now;
            ze.Size = zipByte.Length;
            zos.PutNextEntry(ze);
            zos.SetLevel(3);
            zos.IsStreamOwner = true;
            zos.Write(zipByte, 0, zipByte.Length);
            zos.CloseEntry();
            zos.Flush();
            zos.Close();
            fs.Close();
        }

        private string TopOfKML(string DocName)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">");
            sb.AppendLine(@"<Document>");
            sb.AppendLine(@"	<name>" + DocName + "</name>");
            sb.AppendLine(@"    	<open>1</open>");
            sb.AppendLine(FillColorValueStr);
            return sb.ToString();
        }

        private void butCreateLineDepthKMZ_Click(object sender, EventArgs e)
        {
            richTextBoxResults.Text = "";

            List<CHSChart> chsChartList = new List<CHSChart>();
            using (BathymetryEntities be3 = new BathymetryEntities())
            {
                chsChartList = chsChartList = (from c in be3.CHSCharts.AsNoTracking() where !c.CHSChartName.Contains("SOUND") orderby c.CHSChartName select c).ToList();
            }

            // creating CHSChart KMZ files
            bool OKStart = false;
            foreach (CHSChart chsChart in chsChartList)
            {
                if (chsChart.CHSChartName == textBoxLineStartAt.Text && OKStart == false)
                {
                    OKStart = true;
                }
                if (OKStart)
                {
                    richTextBoxResults.AppendText("Doing ... " + chsChart.CHSChartName + "\r\n");
                }
                else
                {
                    richTextBoxResults.AppendText("Skipping ... " + chsChart.CHSChartName + "\r\n");
                    continue;
                }

                StringBuilder sb = new StringBuilder();

                FileInfo fi = new FileInfo(textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kmz");
                if (fi.Exists)
                {
                    continue;
                }
                else
                {
                    StreamWriter sw = fi.CreateText();
                    sw.WriteLine("empty");
                    sw.Close();
                }

                lblCurrentFile2.Text = chsChart.CHSChartName;
                lblCurrentFile2.Refresh();
                Application.DoEvents();

                List<CHSDepth> chsDepthList = new List<CHSDepth>();
                using (BathymetryEntities be = new BathymetryEntities())
                {
                    be.Database.CommandTimeout = 6000;

                    bool Found = true;
                    int skip = 0;
                    int take = 10000;
                    while (Found)
                    {
                        List<CHSDepth> chsDepthList2 = (from c in be.CHSDepths.AsNoTracking()
                                                        where c.CHSChartID == chsChart.CHSChartID
                                                        orderby c.CHSDepthID
                                                        select c).Skip(skip).Take(take).ToList();

                        if (chsDepthList2.Count > 0)
                        {
                            chsDepthList.AddRange(chsDepthList2);

                            skip += take;
                        }
                        else
                        {
                            Found = false;
                        }
                    }
                }

                if (chsDepthList.Count == 0)
                {
                    richTextBoxResults.AppendText("No Depth found for CHSChart [" + chsChart.CHSChartName + "]\r\n");
                    return;
                }
                else
                {
                    sb.Append(TopOfKML(chsChart.CHSChartName));

                    double OldLineValue = -999;
                    int CountDepth = 0;
                    foreach (CHSDepth d in chsDepthList.OrderBy(d => d.Depth).ThenBy(d => d.LineValue))
                    {
                        CountDepth += 1;
                        lblStatusTxt2.Text = "Doing " + CountDepth;
                        lblStatusTxt2.Refresh();
                        Application.DoEvents();

                        if (d.LineValue != OldLineValue)
                        {
                            if (CountDepth != 1)
                            {
                                sb.AppendLine();
                                sb.AppendLine(@"				</coordinates>");
                                sb.AppendLine(@"			</LineString>");
                                sb.AppendLine(@"		</Placemark>");
                            }

                            OldLineValue = (double)d.LineValue;

                            sb.AppendLine(@"		<Placemark>");
                            sb.AppendLine(@"			<name>-" + d.Depth + "</name>");
                            sb.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)d.Depth, ColorValList) + "</styleUrl>");
                            sb.AppendLine(@"			<LineString>");
                            sb.AppendLine(@"				<tessellate>1</tessellate>");
                            sb.AppendLine(@"				<coordinates>");
                        }
                        else
                        {
                            sb.Append(string.Format("{0},{1},0 ", d.Longitude, d.Latitude));
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine(@"				</coordinates>");
                    sb.AppendLine(@"			</LineString>");
                    sb.AppendLine(@"		</Placemark>");

                    sb.AppendLine(@"</Document>");
                    sb.AppendLine(@"</kml>");

                }

                SaveInKMZFileStream(textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kmz", textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kml", sb);

                lblStatusTxt2.Text = chsChart.CHSChartName + " KML Saved";
                lblStatusTxt2.Refresh();
                Application.DoEvents();
            }
        }
        private void butCreateSoundDepthKMZ_Click(object sender, EventArgs e)
        {
            double SoundBlockPortion = (double)1 / (double)400;

            List<CHSChart> chsChartList = new List<CHSChart>();
            using (BathymetryEntities be = new BathymetryEntities())
            {
                richTextBoxResults.Text = "";

                chsChartList = chsChartList = (from c in be.CHSCharts where c.CHSChartName.Contains("SOUND") orderby c.CHSChartName select c).ToList();
            }

            // creating CHSChart KMZ files
            bool OKStart = false;
            foreach (CHSChart chsChart in chsChartList)
            {
                if (chsChart.CHSChartName == textBoxSoundStartAt.Text && OKStart == false)
                {
                    OKStart = true;
                }
                if (OKStart)
                {
                    richTextBoxResults.AppendText("Doing ... " + chsChart.CHSChartName + "\r\n");
                }
                else
                {
                    richTextBoxResults.AppendText("Skipping ... " + chsChart.CHSChartName + "\r\n");
                    continue;
                }
                StringBuilder sb = new StringBuilder();

                lblCurrentFile2.Text = chsChart.CHSChartName;
                lblCurrentFile2.Refresh();
                Application.DoEvents();

                FileInfo fi = new FileInfo(textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kmz");
                if (fi.Exists)
                {
                    continue;
                }
                else
                {
                    StreamWriter sw = fi.CreateText();
                    sw.WriteLine("empty");
                    sw.Close();
                }

                List<CHSDepth> chsDepthList = new List<CHSDepth>();
                using (BathymetryEntities be2 = new BathymetryEntities())
                {
                    chsDepthList = (from d in be2.CHSDepths
                                    where d.CHSChartID == chsChart.CHSChartID
                                    select d).ToList();
                }

                if (chsDepthList.Count == 0)
                {
                    richTextBoxResults.AppendText("No Depth found for CHSChart [" + chsChart.CHSChartName + "]\r\n");
                    return;
                }
                else
                {
                    int CountDepth = 0;
                    sb.Append(TopOfKML(chsChart.CHSChartName));

                    double SoundBlockSize = Math.Min(Math.Abs((double)chsChart.LongitudeMax - (double)chsChart.LongitudeMin), Math.Abs((double)chsChart.LatitudeMax - (double)chsChart.LatitudeMin));

                    SoundBlockSize = SoundBlockSize * SoundBlockPortion;

                    foreach (CHSDepth d in chsDepthList.OrderBy(d => d.Depth))
                    {
                        CountDepth += 1;
                        lblStatusTxt2.Text = "Doing " + CountDepth;
                        lblStatusTxt2.Refresh();
                        Application.DoEvents();

                        sb.AppendLine(@"		<Placemark>");
                        sb.AppendLine(@"			<name>-" + d.Depth + "</name>");
                        sb.AppendLine(@"			<styleUrl>#" + GetColorStyleID((double)d.Depth, ColorValList) + "</styleUrl>");
                        sb.AppendLine(@"			<Polygon>");
                        sb.AppendLine(@"				<tessellate>1</tessellate>");
                        sb.AppendLine(@"				<outerBoundaryIs>");
                        sb.AppendLine(@"				    <LinearRing>");
                        sb.AppendLine(@"				        <coordinates>");
                        sb.Append(string.Format("{0},{1},0 ", d.Longitude - SoundBlockSize, d.Latitude - SoundBlockSize));
                        sb.Append(string.Format("{0},{1},0 ", d.Longitude - SoundBlockSize, d.Latitude + SoundBlockSize));
                        sb.Append(string.Format("{0},{1},0 ", d.Longitude + SoundBlockSize, d.Latitude + SoundBlockSize));
                        sb.Append(string.Format("{0},{1},0 ", d.Longitude + SoundBlockSize, d.Latitude - SoundBlockSize));
                        sb.Append(string.Format("{0},{1},0 ", d.Longitude - SoundBlockSize, d.Latitude - SoundBlockSize));
                        sb.AppendLine();
                        sb.AppendLine(@"			          	</coordinates>");
                        sb.AppendLine(@"				    </LinearRing>");
                        sb.AppendLine(@"				</outerBoundaryIs>");
                        sb.AppendLine(@"			</Polygon>");
                        sb.AppendLine(@"		</Placemark>");
                    }
                    sb.AppendLine(@"</Document>");
                    sb.AppendLine(@"</kml>");
                }

                SaveInKMZFileStream(textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kmz", textBoxDbfFilesDirPath.Text + @"\KMZ\" + chsChart.CHSChartName + ".kml", sb);

                lblStatusTxt2.Text = chsChart.CHSChartName + " KML Saved";
                lblStatusTxt2.Refresh();
                Application.DoEvents();
            }
        }
        private void butCreate_indexKMZ_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            richTextBoxResults.Text = "";

            // creating the Index.KMZ document
            sb.Append(TopOfKML("Index"));

            List<CHSChart> chsChartList = new List<CHSChart>();
            using (BathymetryEntities be = new BathymetryEntities())
            {
                chsChartList = (from c in be.CHSCharts where !c.CHSChartName.Contains("SOUND") orderby c.CHSChartName select c).ToList();
            }

            lblCurrentFile2.Text = "Index";
            lblCurrentFile2.Refresh();
            Application.DoEvents();
            foreach (CHSChart chsChart in chsChartList)
            {
                lblStatusTxt2.Text = "Doing " + chsChart.CHSChartName;
                double MinLongitude = (double)chsChart.LongitudeMin;
                double MaxLongitude = (double)chsChart.LongitudeMax;
                double MinLatitude = (double)chsChart.LatitudeMin;
                double MaxLatitude = (double)chsChart.LatitudeMax;

                CHSChart SoundChart = new CHSChart();
                using (BathymetryEntities be = new BathymetryEntities())
                {
                    SoundChart = (from c in be.CHSCharts where c.CHSChartName.Contains(chsChart.CHSChartName + "SOUNDG") select c).FirstOrDefault<CHSChart>();
                }

                if (SoundChart != null)
                {
                    if (SoundChart.LongitudeMin < MinLongitude)
                        MinLongitude = (double)SoundChart.LongitudeMin;

                    if (SoundChart.LongitudeMax > MaxLongitude)
                        MaxLongitude = (double)SoundChart.LongitudeMax;

                    if (SoundChart.LatitudeMin < MinLatitude)
                        MinLatitude = (double)SoundChart.LatitudeMin;

                    if (SoundChart.LatitudeMax > MaxLatitude)
                        MaxLatitude = (double)SoundChart.LatitudeMax;
                }

                lblStatusTxt2.Refresh();
                Application.DoEvents();

                sb.AppendLine(@"	<Folder>");
                sb.AppendLine(@"		<name>" + chsChart.CHSChartName + "</name>");
                sb.AppendLine(@"    	<Folder>");
                sb.AppendLine(@"    		<name>Data Extent</name>");
                sb.AppendLine(@"           	<visibility>1</visibility>");
                sb.AppendLine(@"       		<Placemark>");
                sb.AppendLine(@"       			<name>" + chsChart.CHSChartName + "</name>");
                string styleUrlStr = GetColorStyleID((double)((MaxLongitude - MinLongitude) + (MaxLatitude - MinLatitude)) * 100, ColorValList);
                sb.AppendLine(@"       			<styleUrl>#" + styleUrlStr + "</styleUrl>");
                sb.AppendLine(@"       			<Point>");
                sb.AppendLine(@"       				<coordinates>" + MaxLongitude + "," + MaxLatitude + ",0 </coordinates>");
                sb.AppendLine(@"       			</Point>");
                sb.AppendLine(@"       		</Placemark>");
                sb.AppendLine(@"        	<Placemark>");
                sb.AppendLine(@"        		<name>" + chsChart.CHSChartName + "</name>");
                sb.AppendLine(@"        		<styleUrl>#C_0</styleUrl>");
                sb.AppendLine(@"        		<LineString>");
                sb.AppendLine(@"        			<tessellate>1</tessellate>");
                sb.AppendLine(@"        			<coordinates>");
                sb.Append(string.Format("{0},{1},0 ", MinLongitude, MinLatitude));
                sb.Append(string.Format("{0},{1},0 ", MinLongitude, MaxLatitude));
                sb.Append(string.Format("{0},{1},0 ", MaxLongitude, MaxLatitude));
                sb.Append(string.Format("{0},{1},0 ", MaxLongitude, MinLatitude));
                sb.Append(string.Format("{0},{1},0 ", MinLongitude, MinLatitude));
                sb.AppendLine();
                sb.AppendLine(@"        		    </coordinates>");
                sb.AppendLine(@"        		</LineString>");
                sb.AppendLine(@"        	</Placemark>");
                sb.AppendLine(@"    	</Folder>");
                sb.AppendLine(@"    	<Folder>");
                sb.AppendLine(@"    		<name>Line Data</name>");
                sb.AppendLine(@"           	<visibility>0</visibility>");
                sb.AppendLine(@"            <NetworkLink>");
                sb.AppendLine(@"            	<name>" + chsChart.CHSChartName + "</name>");
                sb.AppendLine(@"            	<visibility>0</visibility>");
                sb.AppendLine(@"            	<Link>");
                sb.AppendLine(@"            	    <href>Y:\" + chsChart.CHSChartName + ".kmz</href>");
                sb.AppendLine(@"            	</Link>");
                sb.AppendLine(@"            </NetworkLink>");
                sb.AppendLine(@"    	</Folder>");
                sb.AppendLine(@"    	<Folder>");
                sb.AppendLine(@"    		<name>Sound Data</name>");
                sb.AppendLine(@"           	<visibility>0</visibility>");
                sb.AppendLine(@"            <NetworkLink>");
                sb.AppendLine(@"            	<name>" + chsChart.CHSChartName + "SOUNDG</name>");
                sb.AppendLine(@"            	<visibility>0</visibility>");
                sb.AppendLine(@"            	<Link>");
                sb.AppendLine(@"            	    <href>Y:\" + chsChart.CHSChartName + "SOUNDG.kmz</href>");
                sb.AppendLine(@"            	</Link>");
                sb.AppendLine(@"            </NetworkLink>");
                sb.AppendLine(@"    	</Folder>");
                sb.AppendLine(@"	</Folder>");
            }

            sb.AppendLine(@"</Document>");
            sb.AppendLine(@"</kml>");

            richTextBoxResults.AppendText(sb.ToString());

            SaveInKMZFileStream(textBoxDbfFilesDirPath.Text + @"\KMZ\_index.kmz", textBoxDbfFilesDirPath.Text + @"\KMZ\_index.kml", sb);

            lblStatusTxt2.Text = "_index.kmz Saved";
            lblStatusTxt2.Refresh();
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<CHSChart> chsChartList = new List<CHSChart>();
            using (BathymetryEntities be = new BathymetryEntities())
            {
                chsChartList = (from c in be.CHSCharts.AsNoTracking()
                                where c.LongitudeMin == null
                                select c).ToList();
            }

            richTextBoxResults.AppendText($"{ chsChartList.Count } to do\r\n");
            foreach (CHSChart chsChart in chsChartList)
            {
                double? minLat = null;
                double? maxLat = null;
                double? minLng = null;
                double? maxLng = null;
                using (BathymetryEntities be = new BathymetryEntities())
                {
                    richTextBoxResults.AppendText($"Doing { chsChart.CHSChartName } \r\n");
                    richTextBoxResults.Refresh();
                    Application.DoEvents();

                    minLat = (double)(from c in be.CHSDepths
                                      where c.CHSChartID == chsChart.CHSChartID
                                      && c.Latitude != null
                                      select c.Latitude).Min();

                    maxLat = (double)(from c in be.CHSDepths
                                      where c.CHSChartID == chsChart.CHSChartID
                                      && c.Latitude != null
                                      select c.Latitude).Max();

                    minLng = (double)(from c in be.CHSDepths
                                      where c.CHSChartID == chsChart.CHSChartID
                                      && c.Latitude != null
                                      select c.Longitude).Min();

                    maxLng = (double)(from c in be.CHSDepths
                                      where c.CHSChartID == chsChart.CHSChartID
                                      && c.Latitude != null
                                      select c.Longitude).Max();

                }

                using (BathymetryEntities be2 = new BathymetryEntities())
                {
                    CHSChart chsChartToUpdate = (from c in be2.CHSCharts
                                                 where c.CHSChartID == chsChart.CHSChartID
                                                 select c).FirstOrDefault();

                    chsChartToUpdate.LatitudeMin = minLat;
                    chsChartToUpdate.LatitudeMax = maxLat;
                    chsChartToUpdate.LongitudeMin = minLng;
                    chsChartToUpdate.LongitudeMax = maxLng;

                    try
                    {
                        be2.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        int seilfj = 34;
                    }
                }

            }

        }

    }
}

