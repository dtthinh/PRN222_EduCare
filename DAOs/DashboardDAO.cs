using BOs.Data;
using BOs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAOs
{
    public class DashboardDAO
    {
        private readonly DataContext _context;

        public DashboardDAO(DataContext context)
        {
            _context = context;
        }

        private (DateTime StartDate, DateTime EndDate) GetDateRangeFromPeriod(string period)
        {
            DateTime endDate = DateTime.Now.Date;
            DateTime startDate;

            switch (period?.ToLower())
            {
                case "7d":
                    startDate = endDate.AddDays(-7);
                    break;
                case "30d":
                    startDate = endDate.AddDays(-30);
                    break;
                case "90d":
                    startDate = endDate.AddDays(-90);
                    break;
                case "1y":
                    startDate = endDate.AddYears(-1);
                    break;
                default:
                    startDate = endDate.AddDays(-30);
                    break;
            }
            return (startDate.Date, endDate.Date);
        }

        public async Task<Dictionary<string, int>> GetDashboardOverviewAsync()
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);
            var lowStockThreshold = 20;
            var expiringThreshold = DateTime.Now.AddMonths(3);

            return new Dictionary<string, int>
            {
                { "Tổng số Học sinh", await _context.Students.AsNoTracking().CountAsync() },
                { "Tổng số Y tá", await _context.Accounts.AsNoTracking().CountAsync(a => a.Role.RoleName == "Nurse") },
                { "Sự kiện y tế (Tháng này)", await _context.MedicalEvents.AsNoTracking().CountAsync(m => m.Date >= oneMonthAgo) },
                { "Cảnh báo tồn kho", await _context.MedicalSupplies.AsNoTracking().CountAsync(s => s.Quantity < lowStockThreshold || (s.ExpiredDate != null && s.ExpiredDate <= expiringThreshold)) }
            };
        }

        public async Task<List<RecentActivityData>> GetRecentActivitiesAsync(int page, int pageSize, string? type)
        {
            var query = _context.MedicalEvents
                .AsNoTracking()
                .Include(me => me.Student)
                .OrderByDescending(me => me.Date)
                .Select(me => new RecentActivityData
                {
                    Type = me.Type,
                    StudentName = me.Student.Fullname,
                    Timestamp = me.Date,
                    Description = me.Description
                });

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<(Dictionary<string, int> byGender, Dictionary<string, int> byAge, List<(string className, int count)> byClass)> GetStudentDistributionAsync()
        {
            var byGender = await _context.Students
                .AsNoTracking()
                .GroupBy(s => s.Gender.ToLower().Trim())
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            var currentYear = DateTime.Now.Year;
            var studentsWithAge = await _context.Students.AsNoTracking().Select(s => new { Age = currentYear - s.DateOfBirth.Year }).ToListAsync();
            var byAge = studentsWithAge.GroupBy(s => s.Age switch
            {
                var age when age >= 6 && age <= 8 => "6-8 tuổi",
                var age when age >= 9 && age <= 11 => "9-11 tuổi",
                _ => "Khác"
            }).ToDictionary(g => g.Key, g => g.Count());

            var byClassData = await _context.Classes
                .AsNoTracking()
                .Select(c => new { ClassName = c.ClassName, Count = c.Students.Count() })
                .ToListAsync();
            var byClass = byClassData.Select(c => (c.ClassName, c.Count)).ToList();

            return (byGender, byAge, byClass);
        }

        public async Task<(List<(string month, double value, int count)> averageHeight, List<(string month, double value, int count)> averageWeight)> GetGrowthTrendsAsync(string period, string ageGroup)
        {
            return (new List<(string, double, int)>(), new List<(string, double, int)>());
        }

        public async Task<(List<(string Date, int Accidents, int Illnesses, int Fevers, int Others)> Timeline, Dictionary<string, int> TotalsByType)> GetMedicalEventsTimelineAsync(string eventType, string period)
        {
            var (startDate, endDate) = GetDateRangeFromPeriod(period);
            var query = _context.MedicalEvents.AsNoTracking().Where(m => m.Date >= startDate && m.Date <= endDate);

            if (!string.IsNullOrEmpty(eventType) && eventType.ToLower() != "all")
            {
                query = query.Where(m => m.Type == eventType);
            }

            var data = await query
                .GroupBy(m => m.Date.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Accidents = g.Count(m => m.Type == "Tai nạn"),
                    Illnesses = g.Count(m => m.Type == "Bệnh tật"),
                    Fevers = g.Count(m => m.Type == "Sốt"),
                    Others = g.Count(m => m.Type == "Khác")
                })
                .ToDictionaryAsync(x => x.Date);

            var timeline = new List<(string Date, int Accidents, int Illnesses, int Fevers, int Others)>();
            for (var day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                if (data.TryGetValue(day, out var item))
                {
                    timeline.Add((day.ToString("dd/MM"), item.Accidents, item.Illnesses, item.Fevers, item.Others));
                }
                else
                {
                    timeline.Add((day.ToString("dd/MM"), 0, 0, 0, 0));
                }
            }

            var totals = new Dictionary<string, int>
            {
                { "Tai nạn", await query.CountAsync(m => m.Type == "Tai nạn") },
                { "Bệnh tật", await query.CountAsync(m => m.Type == "Bệnh tật") },
                { "Sốt", await query.CountAsync(m => m.Type == "Sốt") },
                { "Khác", await query.CountAsync(m => m.Type == "Khác") }
            };

            return (timeline, totals);
        }

        public async Task<List<(string Condition, int Count, double Percentage)>> GetCommonConditionsAsync(string period)
        {
            var (startDate, endDate) = GetDateRangeFromPeriod(period);
            var query = _context.MedicalEvents.AsNoTracking().Where(m => m.Date >= startDate && m.Date <= endDate);
            var totalEvents = await query.CountAsync();
            if (totalEvents == 0) return new List<(string, int, double)>();
            var data = await query.GroupBy(m => m.Type).Select(g => new { Condition = g.Key, Count = g.Count() }).ToListAsync();
            return data.Select(x => (x.Condition, x.Count, (double)x.Count / totalEvents * 100)).ToList();
        }

        public async Task<(List<(string Name, int Used, int Remaining)> MostUsedSupplies, List<(string Name, int Current, int Minimum)> LowStockAlerts, List<(string Name, string ExpiryDate, int Quantity)> ExpiringItems)> GetSupplyUsageAsync(string period)
        {
            var (startDate, endDate) = GetDateRangeFromPeriod(period);
            var expiryThreshold = DateTime.Now.AddMonths(3);
            int lowStockThreshold = 20;

            var mostUsedData = await _context.MedicalEventMedicalSupplies
                .AsNoTracking()
                .Where(mems => mems.MedicalEvent.Date >= startDate && mems.MedicalEvent.Date <= endDate)
                .GroupBy(mems => mems.MedicalSupply.Name)
                .Select(g => new { Name = g.Key, Used = g.Sum(x => x.QuantityUsed), Remaining = g.First().MedicalSupply.Quantity })
                .OrderByDescending(x => x.Used)
                .Take(5)
                .ToListAsync();
            var mostUsedSupplies = mostUsedData.Select(x => (x.Name, x.Used, x.Remaining)).ToList();

            var lowStockData = await _context.MedicalSupplies.AsNoTracking()
                .Where(s => s.Quantity < lowStockThreshold)
                .Select(s => new { s.Name, s.Quantity })
                .ToListAsync();
            var lowStockAlerts = lowStockData.Select(s => (s.Name, s.Quantity, lowStockThreshold)).ToList();

            var expiringData = await _context.MedicalSupplies.AsNoTracking()
                .Where(s => s.ExpiredDate != null && s.ExpiredDate <= expiryThreshold)
                .Select(s => new { s.Name, s.ExpiredDate, s.Quantity })
                .ToListAsync();
            var expiringItems = expiringData.Select(s => (s.Name, s.ExpiredDate.Value.ToString("dd/MM/yyyy"), s.Quantity)).ToList();

            return (mostUsedSupplies, lowStockAlerts, expiringItems);
        }
    }
}