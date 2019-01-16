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

namespace doneillspa.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthenticatedUser")]
    [Produces("application/json")]
    public class LabourDetailsController : Controller
    {
        private readonly ITimesheetRepository _repository;
        private readonly IRateRepository _rateRepository;
        private List<LabourRate> Rates = new List<LabourRate>();
        private IConfiguration Configuration;

        public LabourDetailsController(ITimesheetRepository repository, IRateRepository rateRepository, IConfiguration configuration)
        {
            _repository = repository;
            _rateRepository = rateRepository;
            Configuration = configuration;

            Rates = _rateRepository.GetRates().ToList<LabourRate>();
        }

        [Route("api/labourdetails/rates/{id}")]
        public JsonResult Delete(long id)
        {
            var existingRate = _rateRepository.GetRateById(id);

            if (existingRate != null)
            {
                _rateRepository.DeleteRate(existingRate);
                _rateRepository.Save();

                return Json(Ok());
            }

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

            var existingRate = _rateRepository.GetRateById(r.Id);

            if (existingRate == null)
            {
                return NotFound();
            }

            existingRate.EffectiveFrom = r.EffectiveFrom;
            existingRate.EffectiveTo = r.EffectiveTo;
            existingRate.RatePerHour = r.RatePerHour;

            _rateRepository.UpdateRate(existingRate);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _rateRepository.Save();

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

            _rateRepository.InsertRate(rate);
            //Save should be last thing to call at the end of a business transaction as it closes of the Unit Of Work
            _rateRepository.Save();

            return Ok();
        }

        [HttpGet]
        [Route("api/labourdetails/rates")]
        public IEnumerable<LabourRate> GetRates()
        {
            return _rateRepository.GetRates();
        }

        [HttpPost]
        [Route("api/labourdetails/report")]
        public IActionResult Download([FromBody]LabourWeekDetail[] labourDetails)
        {
            //using (SpreadsheetDocument document = SpreadsheetDocument.Create("C:\\Users\\martin.sexton\\Documents\\labour_costs.xlsx", SpreadsheetDocumentType.Workbook))
            //{
            //    // Add a WorkbookPart to the document.
            //    WorkbookPart workbookPart = document.AddWorkbookPart();
            //    workbookPart.Workbook = new Workbook();

            //    // Add a WorksheetPart to the WorkbookPart.
            //    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            //    worksheetPart.Worksheet = new Worksheet(new SheetData());

            //    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

            //    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = " Labour Costs" };
            //    sheets.Append(sheet);

            //    Columns lstColumns = worksheetPart.Worksheet.GetFirstChild<Columns>();
            //    if (lstColumns == null)
            //    {
            //        lstColumns = new Columns();

            //        lstColumns.Append(new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 2, Max = 2, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 3, Max = 3, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 4, Max = 4, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 5, Max = 5, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 6, Max = 6, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 7, Max = 7, Width = 20, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 8, Max = 11, Width = 30, CustomWidth = true });
            //        lstColumns.Append(new Column() { Min = 12, Max = 12, Width = 20, CustomWidth = true });

            //        worksheetPart.Worksheet.InsertAt(lstColumns, 0);
            //    }

            //    // Get the sheetData cell table.
            //    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            //    SetupExcelHeader(worksheetPart.Worksheet.GetFirstChild<SheetData>());
            //    int rowIndex = 2;
            //    foreach (LabourWeekDetail week in labourDetails)
            //    {
            //        WriteRow(worksheetPart.Worksheet.GetFirstChild<SheetData>(), week, rowIndex);
            //        rowIndex += 1;
            //    }

            //    workbookPart.Workbook.Save();

            //}

            //using (MemoryStream stream = new MemoryStream())
            //{
            //    using (FileStream fs = new FileStream("C:\\Users\\martin.sexton\\Documents\\labour_costs.xlsx", FileMode.Open, FileAccess.Read))
            //    {
            //        byte[] bytes = new byte[fs.Length];
            //        fs.Read(bytes, 0, (int)fs.Length);
            //        stream.Write(bytes, 0, (int)fs.Length);
            //    }
            //    var filecontents = Convert.ToBase64String(stream.ToArray());
            //    SendMail(filecontents).Wait();
            //}
            SendMail("");
            return Ok();
        }

        private async Task SendMail(string filecontent)
        {
            var apiKey = Configuration["Data:Baby:SendGridKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sexton.martin@gmail.com", "Example User");
            var subject = "Project Labour Cost";
            var to = new EmailAddress("sexton.martin@gmail.com", "Example User");
            var plainTextContent = "Please find attached labout cost reports";
            var htmlContent = "<strong>Project Labour Cost Reports</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            if (!String.IsNullOrEmpty(filecontent))
            {
                msg.AddAttachment("labour_costs.xlsx", filecontent);
            }

            var response = await client.SendEmailAsync(msg);
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
            row.InsertAt(tempCell, 6);
            row.InsertAt(firstYearCell, 7);
            row.InsertAt(secondYearCell, 8);
            row.InsertAt(thirdYearCell, 9);
            row.InsertAt(fourthYearCell, 10);
            row.InsertAt(totalLabourCell, 11);
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
            row.InsertAt(tempCostCell, 6);
            row.InsertAt(firstYearApprenticeCostCell, 7);
            row.InsertAt(secondYearApprenticeCostCell, 8);
            row.InsertAt(thirdYearApprenticeCostCell, 9);
            row.InsertAt(fourthYearApprenticeCostCell, 10);
            row.InsertAt(totalLabourCostCell, 11);
        }

        [HttpGet]
        [Route("api/labourdetails/project/{proj}")]
        public IEnumerable<LabourWeekDetail> GetByProject(string proj)
        {
            Dictionary<DateTime, LabourWeekDetail> details = new Dictionary<DateTime, LabourWeekDetail>();

            //TODO Read rates from database at this point and pass in some dictionary of role/rate to the LabourWeekDetail when creating it, so it can be use
            //to calculate the rate based on hours worked.
            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach (Timesheet ts in timesheets)
            {
                if (!details.ContainsKey(ts.WeekStarting.Date))
                {
                    LabourWeekDetail detail = new LabourWeekDetail(this.Rates);
                    detail.Week = ts.WeekStarting;

                    UpdateLaboutWeekDurations(detail, ts, proj);
                    details.Add(ts.WeekStarting.Date, detail);
                }
                else
                {
                    LabourWeekDetail detail = details[ts.WeekStarting.Date];
                    UpdateLaboutWeekDurations(detail, ts, proj);
                }
            }

            return details.Values.AsEnumerable<LabourWeekDetail>();
        }

        [HttpGet]
        [Route("api/labourdetails")]
        public IEnumerable<LabourWeekDetail> Get()
        {
            Dictionary<DateTime, LabourWeekDetail> details = new Dictionary<DateTime, LabourWeekDetail>();

            //TODO Read rates from database at this point and pass in some dictionary of role/rate to the LabourWeekDetail when creating it, so it can be use
            //to calculate the rate based on hours worked.
            IEnumerable<Timesheet> timesheets = _repository.GetTimesheets().OrderByDescending(r => r.WeekStarting);
            foreach(Timesheet ts in timesheets)
            {
                if (!details.ContainsKey(ts.WeekStarting.Date))
                {
                    LabourWeekDetail detail = new LabourWeekDetail(this.Rates);
                    detail.Week = ts.WeekStarting;

                    UpdateLaboutWeekDurations(detail, ts, String.Empty);
                    details.Add(ts.WeekStarting.Date, detail);
                }
                else
                {
                    LabourWeekDetail detail = details[ts.WeekStarting.Date];
                    UpdateLaboutWeekDurations(detail, ts, String.Empty);
                }
            }

            return details.Values.AsEnumerable<LabourWeekDetail>();
        }

        private void UpdateLaboutWeekDurations(LabourWeekDetail detail, Timesheet ts, string project)
        {
            foreach (TimesheetEntry tse in ts.TimesheetEntries)
            {
                if (!String.IsNullOrEmpty(project) && !tse.Project.Equals(project))
                {
                    continue;
                }
                TimeSpan startTimespan = TimeSpan.Parse(tse.StartTime);
                TimeSpan endTimespan = TimeSpan.Parse(tse.EndTime);
                TimeSpan result = endTimespan - startTimespan;

                switch (ts.Role)
                {
                    case "Administrator":
                        detail.AdministratorMinutes += result.TotalMinutes;
                        break;
                    case "Supervisor":
                        detail.SupervisorMinutes += result.TotalMinutes;
                        break;
                    case "ChargeHand":
                        detail.ChargehandMinutes += result.TotalMinutes;
                        break;
                    case "ElectR1":
                        detail.ElecR1Minutes += result.TotalMinutes;
                        break;
                    case "ElectR2":
                        detail.ElecR2Minutes += result.TotalMinutes;
                        break;
                    case "ElectR3":
                        detail.ElecR3Minutes += result.TotalMinutes;
                        break;
                    case "Temp":
                        detail.TempMinutes += result.TotalMinutes;
                        break;
                    case "First Year Apprentice":
                        detail.FirstYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Second Year Apprentice":
                        detail.SecondYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Third Year Apprentice":
                        detail.ThirdYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    case "Fourth Year Apprentice":
                        detail.FourthYearApprenticeMinutes += result.TotalMinutes;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}