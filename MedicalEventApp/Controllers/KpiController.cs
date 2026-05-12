using MedicalEventApp.Models;
using MedicalEventApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalEventApp.Controllers;

public class KpiController : Controller
{
    private const string DefaultNotifyEmail = "yuyumy2004@gmail.com";
    private readonly KpiService _kpiService;
    private readonly MyProjectContext _context;
    private readonly EmailService _emailService;
    private readonly IConfiguration _configuration;

    public KpiController(KpiService kpiService, MyProjectContext context, EmailService emailService, IConfiguration configuration)
    {
        _kpiService = kpiService;
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Latest()
    {
        var latestYm = await _context.Yms
            .AsNoTracking()
            .MaxAsync(x => x.Yearmonth);

        var report = await _kpiService.GetKpiReportAsync(latestYm);

        if (HasAnyAbnormal(report))
        {
            var notifyEmail = _configuration["Email:ToEmail"] ?? DefaultNotifyEmail;
            await _emailService.SendAbnormalKpiAsync(report, notifyEmail);
        }

        return PartialView("_KpiReportTable", report);
    }

    private static bool HasAnyAbnormal(KpiReport report)
    {
        return report.Opd.IsTotalGrowthAbnormal ||
               report.Opd.IsFirstGrowthAbnormal ||
               report.Opd.IsRateGrowthAbnormal ||
               report.Inpatient.IsInTotalGrowthAbnormal ||
               report.Inpatient.IsBeddaysGrowthAbnormal ||
               report.Inpatient.IsAlosGrowthAbnormal ||
               report.Inpatient.IsOccrateGrowthAbnormal ||
               report.Er.IsErTotalGrowthAbnormal ||
               report.Er.IsBoardingsGrowthAbnormal ||
               report.Er.IsAdmissionRateGrowthAbnormal;
    }
}
