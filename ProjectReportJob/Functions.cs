using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Azure.WebJobs;
using ProjectReportJob.Data;
using ProjectReportJob.Model;
using Microsoft.EntityFrameworkCore;
using ProjectReportJob.Services;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using System.Runtime.InteropServices.ComTypes;
using System.Configuration;

namespace ProjectReportJob
{
    public class Functions
    {

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public void ProcessQueueMessage([QueueTrigger("messages")] GenerateReportEvent message)
        {
            //GenerateExcelForProject(message.ProjectCode, message.DestinationEmail);

            //Temporarily remove line below.
            //ISignalRService _service = SignalRService.Instance;
            //_service.SendMessage("reportemailed", "Report issued for " + message.ProjectCode); 
        }

        public void ProcessTimesheetReportMessage([QueueTrigger("livefixreports")] TimesheetReportOrder order)
        {
            SaveFileToBlob(order);
        }

        private async void SaveFileToBlob(TimesheetReportOrder order)
        {
            string blobName = "report_" + Guid.NewGuid().ToString() + ".xlsx";

            // Retrieve the connection string for use with the application. 
            string connectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            if (connectionString.Equals(""))
            {
                connectionString = ConfigurationManager.AppSettings["AzureWebJobsStorage"];
            }
            Console.Write("azure storage connection string" + connectionString + " found from configuration manager");

            // Create a BlobServiceClient object 
            var blobServiceClient = new BlobServiceClient(connectionString);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("doneillreports");
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            blobClient.UploadAsync(GenerateExcelTimesheetReport(order.reportDate)).Wait();
            //blobClient.UploadAsync(GenerateTextTimesheetReport(order.reportDate)).Wait();

            UpdateReportStatus(order, blobName);

        }

        private void UpdateReportStatus(TimesheetReportOrder order, string blobName)
        {
            using (var db = new ApplicationDbContext())
            {
                TimesheetReport report = db.TimesheetReport.Where(r => r.Id == order.Id).First();

                report.FileReference = blobName;
                report.Status = TimesheetReportStatus.Success;

                db.SaveChanges();
            }
        }

        private MemoryStream GenerateTextTimesheetReport(string weekBeginning)
        {
            string comparisonString = DateTime.Parse(weekBeginning).ToShortDateString();

            MemoryStream ms = new MemoryStream();

            TextWriter tw = new StreamWriter(ms);

            using (var db = new ApplicationDbContext())
            {
                IEnumerable<Timesheet> timesheets = db.Timesheet
                    .Include(b => b.TimesheetEntries)
                    .ToList()
                    .Where(r => r.WeekStarting.ToShortDateString().Equals(comparisonString) && (r.Status.ToString().Equals("Approved") || r.Status.ToString().Equals("Archieved")))
                    .OrderByDescending(r => r.WeekStarting);

                foreach(Timesheet ts in timesheets)
                {
                    tw.WriteLine("-------------------------------------------------");
                    tw.WriteLine("Timesheet for " + ts.Username + " Week beginning: " + ts.WeekStarting.ToShortDateString());
                    tw.WriteLine("-------------------------------------------------");
                    foreach(TimesheetEntry tse in ts.TimesheetEntries)
                    {
                        tw.WriteLine(String.Format("Day: {0}, Project Code: {1}, StartTime: {2}, EndTime: {3}", tse.Day, tse.Code, tse.StartTime, tse.EndTime));
                    }
                }
            }
            tw.Flush();

            ms.Position = 0;
            return ms;
        }

        private MemoryStream GenerateExcelTimesheetReport(String weekbeginning)
        {
            string comparisonString = DateTime.Parse(weekbeginning).ToShortDateString();

            MemoryStream spreadSheetStream = new MemoryStream();
            using(SpreadsheetDocument document = SpreadsheetDocument.Create(spreadSheetStream, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                DocumentFormat.OpenXml.UInt32Value sheetId = 1;
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetId, Name = "Timesheet" };
                sheets.Append(sheet);

                Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
                if (lstColumns == null)
                {
                    lstColumns = new Columns();

                    lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 20, CustomWidth = true });
                    lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 20, CustomWidth = true });

                    worksheetPart.Worksheet.InsertAt(lstColumns, 0);
                }
                SetupExcelHeader(worksheetPart.Worksheet.GetFirstChild<SheetData>());

                int rowIndex = 2;

                using (var db = new ApplicationDbContext())
                {
                    IEnumerable<Timesheet> timesheets = db.Timesheet
                        .Include(b => b.TimesheetEntries)
                        .ToList()
                        .Where(r => r.WeekStarting.ToShortDateString().Equals(comparisonString) && (r.Status.ToString().Equals("Approved") || r.Status.ToString().Equals("Archieved")))
                        .OrderByDescending(r => r.WeekStarting);

                    foreach (Timesheet ts in timesheets)
                    {
                        IList<TimesheetEntry> monEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> tueEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> wedEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> thursEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> friEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> satEntries = new List<TimesheetEntry>();
                        IList<TimesheetEntry> sunEntries = new List<TimesheetEntry>();

                        foreach (TimesheetEntry tse in ts.TimesheetEntries)
                        {
                            if (tse.Day.ToUpper().Equals("MON"))
                            {
                                monEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("TUE"))
                            {
                                tueEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("WED"))
                            {
                                wedEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("THURS"))
                            {
                                thursEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("FRI"))
                            {
                                friEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("SAT"))
                            {
                                satEntries.Add(tse);
                            }
                            else if (tse.Day.ToUpper().Equals("SUN"))
                            {
                                sunEntries.Add(tse);
                            }
                        }
                        List<double> dailyDuration = new List<double>();
                        double weeklyDuration = 0;


                        rowIndex = PrintRowsForDay(monEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(tueEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(wedEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(thursEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(friEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(satEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);
                        rowIndex = PrintRowsForDay(sunEntries, rowIndex, worksheetPart, ts.Username, weekbeginning, dailyDuration);

                        foreach(double daily in dailyDuration)
                        {
                            weeklyDuration += daily;
                        }
                        WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), "", "", "", "", "Weekly Payable Hours", GetDurationInformation(weeklyDuration), rowIndex);
                        rowIndex += 2;
                    }
                }

                document.Save();
                document.Close();

                spreadSheetStream.Position = 0;
                return spreadSheetStream;

            }
        }


        private int PrintRowsForDay(IList<TimesheetEntry> entries, int rowIndex, WorksheetPart worksheetPart, string username, string weekbeginning, List<double>dailyDuration)
        {
            double durationForDay = 0;

            if(entries.Count > 0)
            {
                foreach (TimesheetEntry tse in entries)
                {
                    WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), weekbeginning, username, tse.Day, tse.Code, tse.StartTime, tse.EndTime, rowIndex);
                    rowIndex += 1;
                    if (tse.Chargeable)
                    {
                        durationForDay += Duration(tse);
                    }
                }

                //Remove Lunch Break
                if(durationForDay >= (5*60))
                {
                    durationForDay -= 30;
                }

                dailyDuration.Add(durationForDay);
                //WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), "", "", "", "", "Daily Total", (durationForDay/60).ToString(), rowIndex);
                //rowIndex += 1;
            }
            return rowIndex;
        }

        private string GetDurationInformation(double durationInMinutes)
        {
            TimeSpan workInMinutes = TimeSpan.FromMinutes(durationInMinutes);
            return string.Format("{0:00}:{1:00}", (int)workInMinutes.TotalHours, workInMinutes.Minutes);

        }

        private void PrntTotalForDayRow(SheetData sheetData, int rowIndex, double total)
        {
            Row row;
            row = new Row() { RowIndex = UInt32.Parse(rowIndex.ToString()) };
            sheetData.Append(row);

            Cell weekCell = new Cell();
            weekCell.CellValue = new CellValue("");
            weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell nameCell = new Cell();
            nameCell.CellValue = new CellValue("");
            nameCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell dayCell = new Cell();
            dayCell.CellValue = new CellValue("");
            dayCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell projectCell = new Cell();
            projectCell.CellValue = new CellValue("");
            projectCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell startCell = new Cell();
            startCell.CellValue = new CellValue(total.ToString());
            startCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell endCell = new Cell();
            endCell.CellValue = new CellValue("");
            endCell.DataType = new EnumValue<CellValues>(CellValues.String);

            row.InsertAt(weekCell, 0);
            row.InsertAt(nameCell, 1);
            row.InsertAt(dayCell, 2);
            row.InsertAt(projectCell, 3);
            row.InsertAt(startCell, 4);
            row.InsertAt(endCell, 5);
        }

        private double Duration(TimesheetEntry tse)
        {
            DateTime startDateTime = DateTime.ParseExact(tse.StartTime, "H:mm", null, System.Globalization.DateTimeStyles.None);
            DateTime endDateTime = DateTime.ParseExact(tse.EndTime, "H:mm", null, System.Globalization.DateTimeStyles.None);

            return endDateTime.Subtract(startDateTime).TotalMinutes;
        }

        private void WriteRow(SheetData sheetData, String weekBeginning, String name, String day, String project, String startTime, String endTime, int rowIndex)
        {
            Row row;
            row = new Row() { RowIndex = UInt32.Parse(rowIndex.ToString()) };

            sheetData.Append(row);

            Cell weekCell = new Cell();
            weekCell.CellValue = new CellValue(weekBeginning);
            weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell nameCell = new Cell();
            nameCell.CellValue = new CellValue(name);
            nameCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell dayCell = new Cell();
            dayCell.CellValue = new CellValue(day);
            dayCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell projectCell = new Cell();
            projectCell.CellValue = new CellValue(project);
            projectCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell startCell = new Cell();
            startCell.CellValue = new CellValue(startTime);
            startCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell endCell = new Cell();
            endCell.CellValue = new CellValue(endTime);
            endCell.DataType = new EnumValue<CellValues>(CellValues.String);

            row.InsertAt(weekCell, 0);
            row.InsertAt(nameCell, 1);
            row.InsertAt(dayCell, 2);
            row.InsertAt(projectCell, 3);
            row.InsertAt(startCell, 4);
            row.InsertAt(endCell, 5);
        }

        private void SetupExcelHeader(SheetData sheetData)
        {

            // Add a row to the cell table.
            Row row;
            row = new Row() { RowIndex = 1 };
            sheetData.Append(row);

            // Add the cell to the cell table at A1.
            Cell weekCell = new Cell();
            // Set the cell value to be a numeric value of 100.
            weekCell.CellValue = new CellValue("Week");
            weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell nameCell = new Cell();
            nameCell.CellValue = new CellValue("Name");
            nameCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell dayCell = new Cell();
            dayCell.CellValue = new CellValue("Day");
            dayCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell projectCell = new Cell();
            projectCell.CellValue = new CellValue("Project");
            projectCell.DataType = new EnumValue<CellValues>(CellValues.String);


            Cell startCell = new Cell();
            startCell.CellValue = new CellValue("Start Time");
            startCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell endCell = new Cell();
            endCell.CellValue = new CellValue("End Time");
            endCell.DataType = new EnumValue<CellValues>(CellValues.String);

            //row.InsertBefore(newCell, refCell);
            row.InsertAt(weekCell, 0);
            row.InsertAt(nameCell, 1);
            row.InsertAt(dayCell, 2);
            row.InsertAt(projectCell, 3);
            row.InsertAt(startCell, 4);
            row.InsertAt(endCell, 5);
        }

        //private void GenerateExcelForProject(string projectCode, string destinationEmail)
        //{

        //    MemoryStream spreadSheetStream = new MemoryStream();
        //    using (SpreadsheetDocument document = SpreadsheetDocument.Create(spreadSheetStream, SpreadsheetDocumentType.Workbook))
        //    {
        //        // Add a WorkbookPart to the document.
        //        WorkbookPart workbookPart = document.AddWorkbookPart();
        //        workbookPart.Workbook = new Workbook();
        //        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

        //        DocumentFormat.OpenXml.UInt32Value sheetId = 1;
        //        IEnumerable<LabourWeekDetail> labourDetails = LabourDetailsForWeek(projectCode);

        //        // Add a WorksheetPart to the WorkbookPart.
        //        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        //        worksheetPart.Worksheet = new Worksheet(new SheetData());

        //        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetId, Name = projectCode };
        //        sheets.Append(sheet);

        //        Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
        //        if (lstColumns == null)
        //        {
        //            lstColumns = new Columns();

        //            lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 20, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 8, Max = 11, Width = 30, CustomWidth = true });
        //            lstColumns.Append(new Column() { Min = 12, Max = 12, Width = 20, CustomWidth = true });

        //            worksheetPart.Worksheet.InsertAt(lstColumns, 0);
        //        }

        //        SetupExcelHeader(worksheetPart.Worksheet.GetFirstChild<SheetData>());
        //        int rowIndex = 2;
        //        foreach (LabourWeekDetail week in labourDetails)
        //        {
        //            WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), week, rowIndex);
        //            rowIndex += 1;
        //        }

        //        document.Save();
        //        document.Close();

        //        string fileName = projectCode + "_labour_costs_" + DateTime.Now.Ticks + ".xlsx";
        //        string subject = "Labour Costs For " + projectCode;


        //        IEmailService emailService = SendGridEmailService.Instance;
        //        emailService.SendMail("doneill@hotmail.com", destinationEmail, subject,
        //                "Please find attached labour cost reports", "<strong>Project Labour Cost Reports</strong>",
        //                fileName, Convert.ToBase64String(spreadSheetStream.ToArray()));

        //    }
        //}

        //private void WriteRow(SheetData sheetData, LabourWeekDetail detail, int rowIndex)
        //{
        //    Row row;
        //    row = new Row() { RowIndex = UInt32.Parse(rowIndex.ToString()) };
        //    sheetData.Append(row);

        //    Cell weekCell = new Cell();
        //    weekCell.CellValue = new CellValue(detail.Week.ToShortDateString());
        //    weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell superVisorCell = new Cell();
        //    superVisorCell.CellValue = new CellValue(detail.SupervisorCost.ToString());
        //    superVisorCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell chargeHandCell = new Cell();
        //    chargeHandCell.CellValue = new CellValue(detail.ChargehandCost.ToString());
        //    chargeHandCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell electrR1Cell = new Cell();
        //    electrR1Cell.CellValue = new CellValue(detail.ElecR1Cost.ToString());
        //    electrR1Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell electrR2Cell = new Cell();
        //    electrR2Cell.CellValue = new CellValue(detail.ElecR2Cost.ToString());
        //    electrR2Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell electrR3Cell = new Cell();
        //    electrR3Cell.CellValue = new CellValue(detail.ElecR3Cost.ToString());
        //    electrR3Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell loc1Cell = new Cell();
        //    loc1Cell.CellValue = new CellValue(detail.Loc1Cost.ToString());
        //    loc1Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell loc2Cell = new Cell();
        //    loc2Cell.CellValue = new CellValue(detail.Loc2Cost.ToString());
        //    loc2Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell loc3Cell = new Cell();
        //    loc3Cell.CellValue = new CellValue(detail.Loc3Cost.ToString());
        //    loc3Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell tempCell = new Cell();
        //    tempCell.CellValue = new CellValue(detail.TempCost.ToString());
        //    tempCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell firstYearCell = new Cell();
        //    firstYearCell.CellValue = new CellValue(detail.FirstYearApprenticeCost.ToString());
        //    firstYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell secondYearCell = new Cell();
        //    secondYearCell.CellValue = new CellValue(detail.SecondYearApprenticeCost.ToString());
        //    secondYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell thirdYearCell = new Cell();
        //    thirdYearCell.CellValue = new CellValue(detail.ThirdYearApprenticeCost.ToString());
        //    thirdYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell fourthYearCell = new Cell();
        //    fourthYearCell.CellValue = new CellValue(detail.FourthYearApprenticeCost.ToString());
        //    fourthYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    Cell totalLabourCell = new Cell();
        //    totalLabourCell.CellValue = new CellValue(detail.TotalCost.ToString());
        //    totalLabourCell.DataType = new EnumValue<CellValues>(CellValues.Number);

        //    row.InsertAt(weekCell, 0);
        //    row.InsertAt(superVisorCell, 1);
        //    row.InsertAt(chargeHandCell, 2);
        //    row.InsertAt(electrR1Cell, 3);
        //    row.InsertAt(electrR2Cell, 4);
        //    row.InsertAt(electrR3Cell, 5);

        //    row.InsertAt(loc1Cell, 6);
        //    row.InsertAt(loc2Cell, 7);
        //    row.InsertAt(loc3Cell, 8);

        //    row.InsertAt(tempCell, 9);
        //    row.InsertAt(firstYearCell, 10);
        //    row.InsertAt(secondYearCell, 11);
        //    row.InsertAt(thirdYearCell, 12);
        //    row.InsertAt(fourthYearCell, 13);
        //    row.InsertAt(totalLabourCell, 14);
        //}

        //private void SetupExcelHeader(SheetData sheetData)
        //{
        //    // Get the sheetData cell table.
        //    //SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

        //    // Add a row to the cell table.
        //    Row row;
        //    row = new Row() { RowIndex = 1 };
        //    sheetData.Append(row);

        //    // Add the cell to the cell table at A1.
        //    Cell weekCell = new Cell();
        //    // Set the cell value to be a numeric value of 100.
        //    weekCell.CellValue = new CellValue("Week");
        //    weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell superVisorCostCell = new Cell();
        //    superVisorCostCell.CellValue = new CellValue("Supvervisor Cost");
        //    superVisorCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell chargeHandCostCell = new Cell();
        //    chargeHandCostCell.CellValue = new CellValue("ChargeHand Cost");
        //    chargeHandCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell electR1CostCell = new Cell();
        //    electR1CostCell.CellValue = new CellValue("ElectR1 Cost");
        //    electR1CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell electR2CostCell = new Cell();
        //    electR2CostCell.CellValue = new CellValue("ElectR2 Cost");
        //    electR2CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell electR3CostCell = new Cell();
        //    electR3CostCell.CellValue = new CellValue("ElectR3 Cost");
        //    electR3CostCell.DataType = new EnumValue<CellValues>(CellValues.String);


        //    Cell loc1CostCell = new Cell();
        //    loc1CostCell.CellValue = new CellValue("Loc1 Cost");
        //    loc1CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell loc2CostCell = new Cell();
        //    loc2CostCell.CellValue = new CellValue("Loc2 Cost");
        //    loc2CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell loc3CostCell = new Cell();
        //    loc3CostCell.CellValue = new CellValue("Loc3 Cost");
        //    loc3CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell tempCostCell = new Cell();
        //    tempCostCell.CellValue = new CellValue("Temp Cost");
        //    tempCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell firstYearApprenticeCostCell = new Cell();
        //    firstYearApprenticeCostCell.CellValue = new CellValue("First Year Apprentice Cost");
        //    firstYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell secondYearApprenticeCostCell = new Cell();
        //    secondYearApprenticeCostCell.CellValue = new CellValue("Second Year Apprentice Cost");
        //    secondYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell thirdYearApprenticeCostCell = new Cell();
        //    thirdYearApprenticeCostCell.CellValue = new CellValue("Third Year Apprentice Cost");
        //    thirdYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell fourthYearApprenticeCostCell = new Cell();
        //    fourthYearApprenticeCostCell.CellValue = new CellValue("Fourth Year Apprentice Cost");
        //    fourthYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    Cell totalLabourCostCell = new Cell();
        //    totalLabourCostCell.CellValue = new CellValue("Total Labour Cost");
        //    totalLabourCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

        //    //row.InsertBefore(newCell, refCell);
        //    row.InsertAt(weekCell, 0);
        //    row.InsertAt(superVisorCostCell, 1);
        //    row.InsertAt(chargeHandCostCell, 2);
        //    row.InsertAt(electR1CostCell, 3);
        //    row.InsertAt(electR2CostCell, 4);
        //    row.InsertAt(electR3CostCell, 5);
        //    row.InsertAt(loc1CostCell, 6);
        //    row.InsertAt(loc2CostCell, 7);
        //    row.InsertAt(loc3CostCell, 8);
        //    row.InsertAt(tempCostCell, 9);
        //    row.InsertAt(firstYearApprenticeCostCell, 10);
        //    row.InsertAt(secondYearApprenticeCostCell, 11);
        //    row.InsertAt(thirdYearApprenticeCostCell, 12);
        //    row.InsertAt(fourthYearApprenticeCostCell, 13);
        //    row.InsertAt(totalLabourCostCell, 14);
        //}

        //private IEnumerable<LabourWeekDetail> LabourDetailsForWeek(string proj)
        //{
        //    Dictionary<DateTime, LabourWeekDetail> labourDetailsByWeek = new Dictionary<DateTime, LabourWeekDetail>();

        //    IList<long> timesheetIds = GetRelevantTimesheets(proj);

        //    using (var db = new ApplicationDbContext())
        //    {
        //        IEnumerable<Timesheet> timesheets = db.Timesheet
        //            .Include(b => b.TimesheetEntries)
        //            .ToList()
        //            .Where(r => r.Status.ToString().Equals("Approved") || r.Status.ToString().Equals("Archieved"))
        //            .OrderByDescending(r => r.WeekStarting);

        //        foreach (Timesheet ts in timesheets)
        //        {
        //            if (timesheetIds.Contains(ts.Id))
        //            {
        //                if (!labourDetailsByWeek.ContainsKey(ts.WeekStarting.Date))
        //                {
        //                    LabourWeekDetail detail = BuildLabourWeekDetails(ts, proj);
        //                    if (detail.TotalCost > 0)
        //                    {
        //                        labourDetailsByWeek.Add(ts.WeekStarting.Date, detail);
        //                    }
        //                }
        //                else
        //                {
        //                    //Update the labout details that are present
        //                    LabourWeekDetail detail = labourDetailsByWeek[ts.WeekStarting.Date];
        //                    detail.ammendDetails(BuildLabourWeekDetails(ts, proj));
        //                }
        //            }
        //        }

        //        return labourDetailsByWeek.Values.ToList<LabourWeekDetail>();
        //    }
        //}

        //private IList<long> GetRelevantTimesheets(string proj)
        //{
        //    IList<long> timesheetIds = new List<long>();
        //    IEnumerable<TimesheetEntry> timesheetEntries;

        //    using (var db = new ApplicationDbContext())
        //    {
        //        timesheetEntries = db.TimesheetEntry
        //            .Include(r => r.Timesheet)
        //            .Where(r => r.Code.Equals(proj));

        //        foreach (TimesheetEntry tse in timesheetEntries)
        //        {
        //            if (!timesheetIds.Contains(tse.Timesheet.Id))
        //            {
        //                timesheetIds.Add(tse.Timesheet.Id);
        //            }
        //        }

        //    }

        //    return timesheetIds;
        //}

        //private LabourWeekDetail BuildLabourWeekDetails(Timesheet ts, string proj)
        //{
        //    //Retrieve details from timesheet and populate the LabourWeekDetail object
        //    LabourWeekDetail detail = new LabourWeekDetail(proj, ts.WeekStarting);
        //    Dictionary<string, double> hoursPerDay = RetrieveBreakdownOfHoursPerDay(ts, proj);

        //    PopulateLabourDetail(ts, detail, hoursPerDay);

        //    return detail;
        //}

        //private void PopulateLabourDetail(Timesheet ts, LabourWeekDetail detail, Dictionary<string, double> hoursPerDay)
        //{
        //    //TODO. Read from Database
        //    double rate = 44.4;

        //    foreach (var item in hoursPerDay)
        //    {
        //        double hoursWorked = item.Value;
        //        switch (ts.Role)
        //        {
        //            case "Administrator":
        //                detail.AdministratorCost += ((hoursWorked) * 10);
        //                break;
        //            case "Supervisor":
        //                detail.SupervisorCost += ((hoursWorked) * rate);
        //                break;
        //            case "ChargeHand":
        //                detail.ChargehandCost += ((hoursWorked) * rate);
        //                break;
        //            case "ElectR1":
        //                detail.ElecR1Cost += ((hoursWorked) * rate);
        //                break;
        //            case "ElectR2":
        //                detail.ElecR2Cost += ((hoursWorked) * rate);
        //                break;
        //            case "ElectR3":
        //                detail.ElecR3Cost += ((hoursWorked) * rate);
        //                break;
        //            case "Loc1":
        //                detail.Loc1Cost += ((hoursWorked) * rate);
        //                break;
        //            case "Loc2":
        //                detail.Loc2Cost += ((hoursWorked) * rate);
        //                break;
        //            case "Loc3":
        //                detail.Loc3Cost += ((hoursWorked) * rate);
        //                break;
        //            case "Temp":
        //                detail.TempCost += ((hoursWorked) * rate);
        //                break;
        //            case "First Year Apprentice":
        //                detail.FirstYearApprenticeCost += ((hoursWorked) * rate);
        //                break;
        //            case "Second Year Apprentice":
        //                detail.SecondYearApprenticeCost += ((hoursWorked) * rate);
        //                break;
        //            case "Third Year Apprentice":
        //                detail.ThirdYearApprenticeCost += ((hoursWorked) * rate);
        //                break;
        //            case "Fourth Year Apprentice":
        //                detail.FourthYearApprenticeCost += ((hoursWorked) * rate);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        //private Dictionary<string, double> RetrieveBreakdownOfHoursPerDay(Timesheet ts, string proj)
        //{

        //    Dictionary<string, double> hoursPerDay = new Dictionary<string, double>();
        //    foreach (TimesheetEntry tse in ts.TimesheetEntries)
        //    {
        //        //If the specification is satisfied then proceed
        //        if (tse.Chargeable && tse.Code.Equals(proj))
        //        {
        //            if (!hoursPerDay.ContainsKey(tse.Day))
        //            {
        //                hoursPerDay.Add(tse.Day, tse.DurationInHours());
        //            }
        //            else
        //            {
        //                double totalHours = hoursPerDay[tse.Day];
        //                totalHours += tse.DurationInHours();
        //                hoursPerDay[tse.Day] = totalHours;
        //            }
        //        }
        //    }
        //    RemoveLunchBreak(hoursPerDay);

        //    return hoursPerDay;
        //}

        //private void RemoveLunchBreak(Dictionary<string, double> hoursPerDay)
        //{
        //    //Update each of the entries to remove 30 mins for days where engineer worked >= 5 hours
        //    foreach (string key in hoursPerDay.Keys.ToList())
        //    {
        //        double hoursWorked = hoursPerDay[key];
        //        if (((key.Equals("Sat") || key.Equals("Sun")) && hoursWorked > (5))
        //            || (!(key.Equals("Sat") || key.Equals("Sun")) && hoursWorked >= (5)))
        //        {
        //            hoursWorked = hoursWorked - 0.5;
        //            hoursPerDay[key] = hoursWorked;
        //        }
        //    }
        //}
    }
}
