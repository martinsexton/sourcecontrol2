using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using doneillspa.DataAccess;
using doneillspa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using doneillspa.Services.Email;
using Microsoft.AspNetCore.Http;
using System.Collections;
using doneillspa.Services.Document;
using doneillspa.Services;

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class LabourDetailsController : Controller
    {
        private readonly ITimesheetService _timesheetService;
        private readonly IProjectService _projectService;
        private List<LabourRate> Rates = new List<LabourRate>();
        private readonly IEmailService _emailService;
        private readonly IDocumentService _documentService;

        public LabourDetailsController(ITimesheetService tss, IProjectService ps, IEmailService emailService, IDocumentService docService)
        {
            _timesheetService = tss;
            _projectService = ps;
            _emailService = emailService;
            _documentService = docService;

            Rates = _projectService.GetRates().ToList<LabourRate>();
        }

        [Route("api/labourdetails/rates/{id}")]
        public JsonResult Delete(long id)
        {
            _projectService.DeleteRate(id);
            return Json(Ok());
        }

        [HttpPut]
        [Route("api/labourdetails/rates")]
        public IActionResult Put([FromBody]LabourRate r)
        {
            if (r == null)
            {
                return BadRequest();
            }

            _projectService.UpdateRate(r);

            return new NoContentResult();
        }

        [HttpPost]
        [Route("api/labourdetails/rates")]
        public IActionResult Post([FromBody]LabourRate rate)
        {
            if (rate == null)
            {
                return BadRequest();
            }

           long id = _projectService.SaveRate(rate.EffectiveFrom, rate.EffectiveTo,rate.RatePerHour, rate.OverTimeRatePerHour, rate.Role);

            return Ok(id);
        }

        [HttpGet]
        [Route("api/labourdetails/rates")]
        public IEnumerable<LabourRate> GetRates()
        {
            IEnumerable<LabourRate> rates = _projectService.GetRates();
            return rates;
        }

        private List<string> RetrieveProjectsWithTimesheets()
        {
            List<string> projects = new List<string>();
            IEnumerable<Timesheet> timesheets = _timesheetService.GetTimesheets().Where(r => r.Status.ToString().Equals("Approved"))
                        .OrderByDescending(r => r.WeekStarting);

            foreach (Timesheet ts in timesheets)
            {
                foreach (TimesheetEntry tse in ts.TimesheetEntries)
                {
                    if (!projects.Contains(tse.Code))
                    {
                        projects.Add(tse.Code);
                    }
                }
            }

            return projects;
        }

        [HttpGet]
        [Route("api/labourdetails/report")]
        public IActionResult DownloadFullReport()
        {
            List<string> projects = RetrieveProjectsWithTimesheets();
            GenerateExcelForProjects(projects);

            return Ok();
        }

        [HttpGet]
        [Route("api/labourdetails/report/{projectName}")]
        public IActionResult Download(string projectName)
        {
            List<string> projects = new List<string>();
            projects.Add(projectName);
            GenerateExcelForProjects(projects);

            return Ok();
        }

        private void GenerateExcelForProjects(List<string> projects)
        {
            string usermail = HttpContext.Session.GetString("UserEmail");

            MemoryStream spreadSheetStream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(spreadSheetStream, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                DocumentFormat.OpenXml.UInt32Value sheetId = 1;
                foreach (string name in projects)
                {
                    IEnumerable<LabourWeekDetail> labourDetails = LabourDetailsForWeek(name);

                    // Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = sheetId, Name = name };
                    sheets.Append(sheet);

                    Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
                    if (lstColumns == null)
                    {
                        lstColumns = new Columns();

                        lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 20, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 8, Max = 11, Width = 30, CustomWidth = true });
                        lstColumns.Append(new Column() { Min = 12, Max = 12, Width = 20, CustomWidth = true });

                        worksheetPart.Worksheet.InsertAt(lstColumns, 0);
                    }

                    // Get the sheetData cell table.
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    SetupExcelHeader(worksheetPart.Worksheet.GetFirstChild<SheetData>());
                    int rowIndex = 2;
                    foreach (LabourWeekDetail week in labourDetails)
                    {
                        WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), week, rowIndex);
                        rowIndex += 1;
                    }

                    document.Save();

                    //Increment SheetID
                    sheetId++;

                }

                document.Close();

                string fileName = "";
                string subject = "";

                if(projects.Count == 1)
                {
                    fileName = projects[0] + "_labour_costs_" + DateTime.Now.Ticks + ".xlsx";
                    subject = "Labour Costs For "+ projects[0];
                }
                else
                {
                    fileName = "labour_costs_" + DateTime.Now.Ticks + ".xlsx";
                    subject = "Labour Costs";
                }

                _emailService.SendMail("doneill@hotmail.com", usermail, subject,
                        "Please find attached labour cost reports", "<strong>Project Labour Cost Reports</strong>",
                        fileName, Convert.ToBase64String(spreadSheetStream.ToArray()));

                //Save Document to document service
                _documentService.SaveDocument(fileName, spreadSheetStream.ToArray());
            }
        }


        private void WriteRow(SheetData sheetData, LabourWeekDetail detail, int rowIndex)
        {
            Row row;
            row = new Row() { RowIndex = UInt32.Parse(rowIndex.ToString()) };
            sheetData.Append(row);

            Cell weekCell = new Cell();
            weekCell.CellValue = new CellValue(detail.Week.ToShortDateString());
            weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell superVisorCell = new Cell();
            superVisorCell.CellValue = new CellValue(detail.SupervisorCost.ToString());
            superVisorCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell chargeHandCell = new Cell();
            chargeHandCell.CellValue = new CellValue(detail.ChargehandCost.ToString());
            chargeHandCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell electrR1Cell = new Cell();
            electrR1Cell.CellValue = new CellValue(detail.ElecR1Cost.ToString());
            electrR1Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell electrR2Cell = new Cell();
            electrR2Cell.CellValue = new CellValue(detail.ElecR2Cost.ToString());
            electrR2Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell electrR3Cell = new Cell();
            electrR3Cell.CellValue = new CellValue(detail.ElecR3Cost.ToString());
            electrR3Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell loc1Cell = new Cell();
            loc1Cell.CellValue = new CellValue(detail.Loc1Cost.ToString());
            loc1Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell loc2Cell = new Cell();
            loc2Cell.CellValue = new CellValue(detail.Loc2Cost.ToString());
            loc2Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell loc3Cell = new Cell();
            loc3Cell.CellValue = new CellValue(detail.Loc3Cost.ToString());
            loc3Cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell tempCell = new Cell();
            tempCell.CellValue = new CellValue(detail.TempCost.ToString());
            tempCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell firstYearCell = new Cell();
            firstYearCell.CellValue = new CellValue(detail.FirstYearApprenticeCost.ToString());
            firstYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell secondYearCell = new Cell();
            secondYearCell.CellValue = new CellValue(detail.SecondYearApprenticeCost.ToString());
            secondYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell thirdYearCell = new Cell();
            thirdYearCell.CellValue = new CellValue(detail.ThirdYearApprenticeCost.ToString());
            thirdYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell fourthYearCell = new Cell();
            fourthYearCell.CellValue = new CellValue(detail.FourthYearApprenticeCost.ToString());
            fourthYearCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            Cell totalLabourCell = new Cell();
            totalLabourCell.CellValue = new CellValue(detail.TotalCost.ToString());
            totalLabourCell.DataType = new EnumValue<CellValues>(CellValues.Number);

            row.InsertAt(weekCell, 0);
            row.InsertAt(superVisorCell, 1);
            row.InsertAt(chargeHandCell, 2);
            row.InsertAt(electrR1Cell, 3);
            row.InsertAt(electrR2Cell, 4);
            row.InsertAt(electrR3Cell, 5);

            row.InsertAt(loc1Cell, 6);
            row.InsertAt(loc2Cell, 7);
            row.InsertAt(loc3Cell, 8);

            row.InsertAt(tempCell, 9);
            row.InsertAt(firstYearCell, 10);
            row.InsertAt(secondYearCell, 11);
            row.InsertAt(thirdYearCell, 12);
            row.InsertAt(fourthYearCell, 13);
            row.InsertAt(totalLabourCell, 14);
        }
        private void SetupExcelHeader(SheetData sheetData)
        {
            // Get the sheetData cell table.
            //SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            // Add a row to the cell table.
            Row row;
            row = new Row() { RowIndex = 1 };
            sheetData.Append(row);

            // Add the cell to the cell table at A1.
            Cell weekCell = new Cell();
            // Set the cell value to be a numeric value of 100.
            weekCell.CellValue = new CellValue("Week");
            weekCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell superVisorCostCell = new Cell();
            superVisorCostCell.CellValue = new CellValue("Supvervisor Cost");
            superVisorCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell chargeHandCostCell = new Cell();
            chargeHandCostCell.CellValue = new CellValue("ChargeHand Cost");
            chargeHandCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell electR1CostCell = new Cell();
            electR1CostCell.CellValue = new CellValue("ElectR1 Cost");
            electR1CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell electR2CostCell = new Cell();
            electR2CostCell.CellValue = new CellValue("ElectR2 Cost");
            electR2CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell electR3CostCell = new Cell();
            electR3CostCell.CellValue = new CellValue("ElectR3 Cost");
            electR3CostCell.DataType = new EnumValue<CellValues>(CellValues.String);


            Cell loc1CostCell = new Cell();
            loc1CostCell.CellValue = new CellValue("Loc1 Cost");
            loc1CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell loc2CostCell = new Cell();
            loc2CostCell.CellValue = new CellValue("Loc2 Cost");
            loc2CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell loc3CostCell = new Cell();
            loc3CostCell.CellValue = new CellValue("Loc3 Cost");
            loc3CostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell tempCostCell = new Cell();
            tempCostCell.CellValue = new CellValue("Temp Cost");
            tempCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell firstYearApprenticeCostCell = new Cell();
            firstYearApprenticeCostCell.CellValue = new CellValue("First Year Apprentice Cost");
            firstYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell secondYearApprenticeCostCell = new Cell();
            secondYearApprenticeCostCell.CellValue = new CellValue("Second Year Apprentice Cost");
            secondYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell thirdYearApprenticeCostCell = new Cell();
            thirdYearApprenticeCostCell.CellValue = new CellValue("Third Year Apprentice Cost");
            thirdYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell fourthYearApprenticeCostCell = new Cell();
            fourthYearApprenticeCostCell.CellValue = new CellValue("Fourth Year Apprentice Cost");
            fourthYearApprenticeCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            Cell totalLabourCostCell = new Cell();
            totalLabourCostCell.CellValue = new CellValue("Total Labour Cost");
            totalLabourCostCell.DataType = new EnumValue<CellValues>(CellValues.String);

            //row.InsertBefore(newCell, refCell);
            row.InsertAt(weekCell, 0);
            row.InsertAt(superVisorCostCell, 1);
            row.InsertAt(chargeHandCostCell, 2);
            row.InsertAt(electR1CostCell, 3);
            row.InsertAt(electR2CostCell, 4);
            row.InsertAt(electR3CostCell, 5);
            row.InsertAt(loc1CostCell, 6);
            row.InsertAt(loc2CostCell, 7);
            row.InsertAt(loc3CostCell, 8);
            row.InsertAt(tempCostCell, 9);
            row.InsertAt(firstYearApprenticeCostCell, 10);
            row.InsertAt(secondYearApprenticeCostCell, 11);
            row.InsertAt(thirdYearApprenticeCostCell, 12);
            row.InsertAt(fourthYearApprenticeCostCell, 13);
            row.InsertAt(totalLabourCostCell, 14);
        }

        [HttpGet]
        [Route("api/labourdetails/project/timesheetentries/{proj}/{day}/{month}/{year}")]
        public IEnumerable<TimesheetEntryDto> GetTimesheetsForProjectOnDate(string proj, int day, int month, int year)
        {
            List<TimesheetEntryDto> entries = new List<TimesheetEntryDto>();
            DateTime weekStaring = new DateTime(year, month, day);

            IEnumerable<Timesheet> timesheets = _timesheetService.GetTimesheets().Where(r => r.Status.ToString().Equals("Approved"))
                .OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                if (!ts.WeekStarting.Date.Equals(weekStaring.Date))
                {
                    continue;
                }
                else
                {
                    foreach(TimesheetEntry tse in ts.TimesheetEntries)
                    {
                        if (tse.Code.Equals(proj))
                        {
                            TimesheetEntryDto tsedto = new TimesheetEntryDto();
                            tsedto.DateCreated = tse.DateCreated;
                            tsedto.Day = tse.Day;
                            tsedto.Details = tse.Details;
                            tsedto.EndTime = tse.EndTime;
                            tsedto.Id = tse.Id;
                            tsedto.Code = tse.Code;
                            tsedto.StartTime = tse.StartTime;
                            tsedto.Username = tse.Timesheet.Username;

                            entries.Add(tsedto);
                        }
                    }
                }
            }
            return entries;
        }

        [HttpGet]
        [Route("api/labourdetails/project/{proj}")]
        public IEnumerable<LabourWeekDetail> GetByProject(string proj)
        {
            return LabourDetailsForWeek(proj);
        }

        private IEnumerable<LabourWeekDetail> LabourDetailsForWeek(string proj)
        {
            Dictionary<DateTime, LabourWeekDetail> labourDetailsByWeek = new Dictionary<DateTime, LabourWeekDetail>();

            IEnumerable<Timesheet> timesheets = _timesheetService.GetTimesheets().Where(r => r.Status.ToString().Equals("Approved"))
                .OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                if (!labourDetailsByWeek.ContainsKey(ts.WeekStarting.Date))
                {
                    LabourWeekDetail detail = ts.BuildLabourWeekDetails(this.Rates, proj);
                    if (detail.TotalCost > 0)
                    {
                        labourDetailsByWeek.Add(ts.WeekStarting.Date, ts.BuildLabourWeekDetails(this.Rates, proj));
                    }
                }
                else
                {
                    //Update the labout details that are present
                    LabourWeekDetail detail = labourDetailsByWeek[ts.WeekStarting.Date];
                    detail.ammendDetails(ts.BuildLabourWeekDetails(this.Rates, proj));
                }
            }

            return labourDetailsByWeek.Values.ToList<LabourWeekDetail>();
        }
    }
}