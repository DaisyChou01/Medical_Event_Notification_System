using System.Net;
using System.Net.Mail;
using System.Text;

namespace MedicalEventApp.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAbnormalKpiAsync(KpiReport report, string toEmail)
    {
        var abnormalItems = GetAbnormalItems(report);
        if (abnormalItems.Count == 0)
        {
            return;
        }

        var host = _configuration["Email:Host"] ?? _configuration["Email:SmtpHost"];
        var portValue = _configuration["Email:Port"] ?? _configuration["Email:SmtpPort"];
        var username = _configuration["Email:Username"];
        var fromEmail = _configuration["Email:FromEmail"];
        var fromName = _configuration["Email:FromName"];
        var password = _configuration["Email:Password"];
        var enableSslValue = _configuration["Email:EnableSsl"];

        if (string.IsNullOrWhiteSpace(host) ||
            string.IsNullOrWhiteSpace(portValue) ||
            string.IsNullOrWhiteSpace(fromEmail) ||
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        var port = int.TryParse(portValue, out var parsedPort) ? parsedPort : 587;
        var enableSsl = !bool.TryParse(enableSslValue, out var parsedSsl) || parsedSsl;
        var smtpUsername = string.IsNullOrWhiteSpace(username) ? fromEmail : username;

        using var message = new MailMessage
        {
            Subject = $"[KPI Alert] {report.Ym} Abnormal Indicators",
            Body = BuildHtmlBody(report, abnormalItems),
            IsBodyHtml = true
        };
        message.From = string.IsNullOrWhiteSpace(fromName)
            ? new MailAddress(fromEmail)
            : new MailAddress(fromEmail, fromName);
        message.To.Add(toEmail);

        using var smtpClient = new SmtpClient(host, port)
        {
            EnableSsl = enableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(smtpUsername, password)
        };

        await smtpClient.SendMailAsync(message);
    }

    private static List<(string Category, string Indicator, decimal Value)> GetAbnormalItems(KpiReport report)
    {
        var items = new List<(string Category, string Indicator, decimal Value)>();

        if (report.Opd.IsTotalGrowthAbnormal) items.Add(("OPD", "TotalGrowthRate (%)", report.Opd.TotalGrowthRate));
        if (report.Opd.IsFirstGrowthAbnormal) items.Add(("OPD", "FirstGrowthRate (%)", report.Opd.FirstGrowthRate));
        if (report.Opd.IsRateGrowthAbnormal) items.Add(("OPD", "RateGrowthRate (%)", report.Opd.RateGrowthRate));

        if (report.Inpatient.IsInTotalGrowthAbnormal) items.Add(("Inpatient", "InTotalGrowthRate (%)", report.Inpatient.InTotalGrowthRate));
        if (report.Inpatient.IsBeddaysGrowthAbnormal) items.Add(("Inpatient", "BeddaysGrowthRate (%)", report.Inpatient.BeddaysGrowthRate));
        if (report.Inpatient.IsAlosGrowthAbnormal) items.Add(("Inpatient", "AlosGrowthRate (%)", report.Inpatient.AlosGrowthRate));
        if (report.Inpatient.IsOccrateGrowthAbnormal) items.Add(("Inpatient", "OccrateGrowthRate (%)", report.Inpatient.OccrateGrowthRate));

        if (report.Er.IsErTotalGrowthAbnormal) items.Add(("ER", "ERTotalGrowthRate (%)", report.Er.ErTotalGrowthRate));
        if (report.Er.IsBoardingsGrowthAbnormal) items.Add(("ER", "BoardingsGrowthRate (%)", report.Er.BoardingsGrowthRate));
        if (report.Er.IsAdmissionRateGrowthAbnormal) items.Add(("ER", "AdmissionRateGrowthRate (%)", report.Er.AdmissionRateGrowthRate));

        return items;
    }

    private static string BuildHtmlBody(KpiReport report, List<(string Category, string Indicator, decimal Value)> abnormalItems)
    {
        var builder = new StringBuilder();
        builder.Append("<h3>KPI Abnormal Alert</h3>");
        builder.Append($"<p>YM: <strong>{report.Ym}</strong>, Last YM: <strong>{report.LastYm}</strong></p>");
        builder.Append("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse:collapse;'>");
        builder.Append("<thead><tr style='background:#f2f2f2;'><th>Category</th><th>Indicator</th><th>Value</th></tr></thead><tbody>");

        foreach (var item in abnormalItems)
        {
            builder.Append("<tr>");
            builder.Append($"<td>{item.Category}</td>");
            builder.Append($"<td>{item.Indicator}</td>");
            builder.Append($"<td style='color:#dc3545;font-weight:bold;'>{item.Value:F2}%</td>");
            builder.Append("</tr>");
        }

        builder.Append("</tbody></table>");
        return builder.ToString();
    }
}
