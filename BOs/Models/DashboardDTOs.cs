using System;
using System.Collections.Generic;

namespace BOs.Models
{
    public class HealthSummaryData
    {
        public string ReportPeriod { get; set; }
        public SummaryData Summary { get; set; }
        public TrendsData Trends { get; set; }
    }

    public class SummaryData
    {
        public int TotalStudents { get; set; }
        public int HealthChecksCompleted { get; set; }
        public double HealthCheckCoverage { get; set; }
        public HealthIndicatorsData HealthIndicators { get; set; }
        public MedicalEventsData MedicalEvents { get; set; }
    }

    public class HealthIndicatorsData
    {
        public double AverageHeight { get; set; }
        public double AverageWeight { get; set; }
        public double VisionProblemsRate { get; set; }
    }

    public class MedicalEventsData
    {
        public int Total { get; set; }
        public int Accidents { get; set; }
        public int Illnesses { get; set; }
        public int Fevers { get; set; }
        public int Others { get; set; }
    }

    public class TrendsData
    {
        public ComparedToPreviousData ComparedToPrevious { get; set; }
    }

    public class ComparedToPreviousData
    {
        public string HealthChecks { get; set; }
        public string MedicalEvents { get; set; }
        public string AverageHeight { get; set; }
        public string AverageWeight { get; set; }
    }

    public class ComparisonData
    {
        public string Metric { get; set; }
        public PeriodData Period1 { get; set; }
        public PeriodData Period2 { get; set; }
        public ComparisonResultData Comparison { get; set; }
    }

    public class PeriodData
    {
        public string Label { get; set; }
        public int Value { get; set; }
        public BreakdownData Breakdown { get; set; }
    }

    public class BreakdownData
    {
        public int Accidents { get; set; }
        public int Illnesses { get; set; }
        public int Fevers { get; set; }
        public int Others { get; set; }
    }

    public class ComparisonResultData
    {
        public int Change { get; set; }
        public double ChangePercent { get; set; }
        public string Trend { get; set; }
        public string Significance { get; set; }
    }

    public class TrendDataPoint
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }

    public class RecentActivityData
    {
        public string Type { get; set; }
        public string StudentName { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}