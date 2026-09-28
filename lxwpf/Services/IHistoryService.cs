using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IHistoryService
    {
        void Add(HistoryModel record);
        void AddRange(List<HistoryModel> records);
        List<HistoryModel> Query(DateTime start, DateTime end, string? deviceName);

        void ExportToCsv(List<HistoryModel> records, string filePath);

        void ExportToExcel(List<HistoryModel> records, string filePath);
    }
}
