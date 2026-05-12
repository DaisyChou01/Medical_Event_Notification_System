using MedicalEventApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalEventApp.Services;

public class KpiService
{
    private readonly MyProjectContext _context;

    public KpiService(MyProjectContext context)
    {
        _context = context;
    }

    public async Task<KpiReport> GetKpiReportAsync(string ym)
    {
        ValidateYm(ym);
        var lastYm = GetLastYm(ym);

        var opdCurrentQuery = _context.Opds
            .AsNoTracking()
            .Where(x => x.OpdDate.StartsWith(ym));
        var opdLastQuery = _context.Opds
            .AsNoTracking()
            .Where(x => x.OpdDate.StartsWith(lastYm));

        var currentTotal = await opdCurrentQuery.CountAsync();
        var lastYearTotal = await opdLastQuery.CountAsync();
        var currentFirst = await opdCurrentQuery.CountAsync(x => x.IsFirstTime == "Y");
        var lastYearFirst = await opdLastQuery.CountAsync(x => x.IsFirstTime == "Y");

        var totalGrowthRate = CalcGrowthRate(currentTotal, lastYearTotal);
        var firstGrowthRate = CalcGrowthRate(currentFirst, lastYearFirst);
        var currentFirstVisitRate = CalcRatioPercent(currentFirst, currentTotal);
        var lastFirstVisitRate = CalcRatioPercent(lastYearFirst, lastYearTotal);
        var rateGrowthRate = CalcGrowthRate(currentFirstVisitRate, lastFirstVisitRate);

        var inptAgg = await _context.Inpts
            .AsNoTracking()
            .Where(x => x.YearMonth == ym || x.YearMonth == lastYm)
            .GroupBy(x => x.YearMonth)
            .Select(g => new
            {
                YearMonth = g.Key,
                Visits = g.Sum(x => x.Visits),
                BedDays = g.Sum(x => x.BedDays),
                OccRate = g.Average(x => x.OccRate)
            })
            .ToListAsync();

        var currentInpt = inptAgg.FirstOrDefault(x => x.YearMonth == ym);
        var lastInpt = inptAgg.FirstOrDefault(x => x.YearMonth == lastYm);

        var currentInTotal = currentInpt?.Visits ?? 0;
        var lastInTotal = lastInpt?.Visits ?? 0;
        var currentBeddays = currentInpt?.BedDays ?? 0;
        var lastBeddays = lastInpt?.BedDays ?? 0;
        var currentOccRate = currentInpt is null ? 0m : currentInpt.OccRate * 100m;
        var lastOccRate = lastInpt is null ? 0m : lastInpt.OccRate * 100m;

        var inTotalGrowthRate = CalcGrowthRate(currentInTotal, lastInTotal);
        var beddaysGrowthRate = CalcGrowthRate(currentBeddays, lastBeddays);
        var currentAlos = CalcRatio(currentBeddays, currentInTotal);
        var lastAlos = CalcRatio(lastBeddays, lastInTotal);
        var alosGrowthRate = CalcGrowthRate(currentAlos, lastAlos);
        var occrateGrowthRate = CalcGrowthRate(currentOccRate, lastOccRate);

        var erAgg = await _context.Ers
            .AsNoTracking()
            .Where(x => x.YearMonth == ym || x.YearMonth == lastYm)
            .GroupBy(x => x.YearMonth)
            .Select(g => new
            {
                YearMonth = g.Key,
                TriageVisits = g.Sum(x => x.TriageVisits),
                BoardingVisits = g.Sum(x => x.BoardingVisits),
                AdmittedVisits = g.Sum(x => x.AdmittedVisits)
            })
            .ToListAsync();

        var currentEr = erAgg.FirstOrDefault(x => x.YearMonth == ym);
        var lastEr = erAgg.FirstOrDefault(x => x.YearMonth == lastYm);

        var currentErTotal = currentEr?.TriageVisits ?? 0;
        var lastErTotal = lastEr?.TriageVisits ?? 0;
        var currentBoarding = currentEr?.BoardingVisits ?? 0;
        var lastBoarding = lastEr?.BoardingVisits ?? 0;
        var currentAdmissionRate = CalcRatioPercent(currentEr?.AdmittedVisits ?? 0, currentErTotal);
        var lastAdmissionRate = CalcRatioPercent(lastEr?.AdmittedVisits ?? 0, lastErTotal);

        var erTotalGrowthRate = CalcGrowthRate(currentErTotal, lastErTotal);
        var boardingsGrowthRate = CalcGrowthRate(currentBoarding, lastBoarding);
        var admissionRateGrowthRate = CalcGrowthRate(currentAdmissionRate, lastAdmissionRate);

        return new KpiReport
        {
            Ym = ym,
            LastYm = lastYm,
            Opd = new OpdKpi
            {
                CurrentTotal = currentTotal,
                LastYearTotal = lastYearTotal,
                TotalGrowthRate = totalGrowthRate,
                CurrentFirst = currentFirst,
                LastYearFirst = lastYearFirst,
                FirstGrowthRate = firstGrowthRate,
                FirstVisitRate = currentFirstVisitRate,
                RateGrowthRate = rateGrowthRate,
                IsTotalGrowthAbnormal = totalGrowthRate < -10m,
                IsFirstGrowthAbnormal = firstGrowthRate < -10m,
                IsRateGrowthAbnormal = rateGrowthRate < -10m
            },
            Inpatient = new InpatientKpi
            {
                CurrentInTotal = currentInTotal,
                LastInTotal = lastInTotal,
                InTotalGrowthRate = inTotalGrowthRate,
                CurrentBeddays = currentBeddays,
                LastBeddays = lastBeddays,
                BeddaysGrowthRate = beddaysGrowthRate,
                Alos = currentAlos,
                AlosGrowthRate = alosGrowthRate,
                OccRate = currentOccRate,
                OccrateGrowthRate = occrateGrowthRate,
                IsInTotalGrowthAbnormal = inTotalGrowthRate < -10m,
                IsBeddaysGrowthAbnormal = beddaysGrowthRate < -10m,
                IsAlosGrowthAbnormal = alosGrowthRate > 10m,
                IsOccrateGrowthAbnormal = occrateGrowthRate < -10m
            },
            Er = new ErKpi
            {
                CurrentErTotal = currentErTotal,
                LastErTotal = lastErTotal,
                ErTotalGrowthRate = erTotalGrowthRate,
                CurrentBoarding = currentBoarding,
                LastBoarding = lastBoarding,
                BoardingsGrowthRate = boardingsGrowthRate,
                AdmissionRate = currentAdmissionRate,
                AdmissionRateGrowthRate = admissionRateGrowthRate,
                IsErTotalGrowthAbnormal = erTotalGrowthRate < -10m,
                IsBoardingsGrowthAbnormal = boardingsGrowthRate > 10m,
                IsAdmissionRateGrowthAbnormal = admissionRateGrowthRate < -10m
            }
        };
    }

    private static decimal CalcGrowthRate(decimal current, decimal previous)
    {
        if (previous == 0m)
        {
            return 0m;
        }

        return (current - previous) / previous * 100m;
    }

    private static decimal CalcRatio(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            return 0m;
        }

        return (decimal)numerator / denominator;
    }

    private static decimal CalcRatioPercent(int numerator, int denominator)
    {
        return CalcRatio(numerator, denominator) * 100m;
    }

    private static string GetLastYm(string ym)
    {
        var value = int.Parse(ym);
        return (value - 100).ToString("D6");
    }

    private static void ValidateYm(string ym)
    {
        if (string.IsNullOrWhiteSpace(ym) || ym.Length != 6 || !ym.All(char.IsDigit))
        {
            throw new ArgumentException("ym must be formatted as YYYYMM", nameof(ym));
        }
    }
}

public class KpiReport
{
    public string Ym { get; set; } = string.Empty;
    public string LastYm { get; set; } = string.Empty;
    public OpdKpi Opd { get; set; } = new();
    public InpatientKpi Inpatient { get; set; } = new();
    public ErKpi Er { get; set; } = new();
}

public class OpdKpi
{
    public int CurrentTotal { get; set; }
    public int LastYearTotal { get; set; }
    public decimal TotalGrowthRate { get; set; }
    public int CurrentFirst { get; set; }
    public int LastYearFirst { get; set; }
    public decimal FirstGrowthRate { get; set; }
    public decimal FirstVisitRate { get; set; }
    public decimal RateGrowthRate { get; set; }
    public bool IsTotalGrowthAbnormal { get; set; }
    public bool IsFirstGrowthAbnormal { get; set; }
    public bool IsRateGrowthAbnormal { get; set; }
}

public class InpatientKpi
{
    public int CurrentInTotal { get; set; }
    public int LastInTotal { get; set; }
    public decimal InTotalGrowthRate { get; set; }
    public int CurrentBeddays { get; set; }
    public int LastBeddays { get; set; }
    public decimal BeddaysGrowthRate { get; set; }
    public decimal Alos { get; set; }
    public decimal AlosGrowthRate { get; set; }
    public decimal OccRate { get; set; }
    public decimal OccrateGrowthRate { get; set; }
    public bool IsInTotalGrowthAbnormal { get; set; }
    public bool IsBeddaysGrowthAbnormal { get; set; }
    public bool IsAlosGrowthAbnormal { get; set; }
    public bool IsOccrateGrowthAbnormal { get; set; }
}

public class ErKpi
{
    public int CurrentErTotal { get; set; }
    public int LastErTotal { get; set; }
    public decimal ErTotalGrowthRate { get; set; }
    public int CurrentBoarding { get; set; }
    public int LastBoarding { get; set; }
    public decimal BoardingsGrowthRate { get; set; }
    public decimal AdmissionRate { get; set; }
    public decimal AdmissionRateGrowthRate { get; set; }
    public bool IsErTotalGrowthAbnormal { get; set; }
    public bool IsBoardingsGrowthAbnormal { get; set; }
    public bool IsAdmissionRateGrowthAbnormal { get; set; }
}
