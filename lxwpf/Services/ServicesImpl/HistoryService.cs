using lxwpf.Entities;
using lxwpf.Repository;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lxwpf.Services.ServicesImpl
{
    public class HistoryService : IHistoryService
    {
        public void Add(HistoryModel record)
        {
            using var db = new AppDbContext();
            db.HistoryRecords.Add(record);
            db.SaveChanges();
        }

        public void AddRange(List<HistoryModel> records)
        {
            using var db = new AppDbContext();
            db.HistoryRecords.AddRange(records);
            db.SaveChanges();
        }

        //查询
        public List<HistoryModel> Query(DateTime start, DateTime end, string? deviceName)
        {
            using var db = new AppDbContext();
            var query = db.HistoryRecords
                .Where(r => r.RecordTime >= start && r.RecordTime <= end);

            if (!string.IsNullOrEmpty(deviceName))
            {
                query = query.Where(r => r.DeviceName == deviceName);
            }

            //查询1000条
            return query.OrderByDescending(r => r.RecordTime).Take(1000).ToList();
        }

        public void ExportToCsv(List<HistoryModel> records, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("设备,从站,地址,值,时间");

            foreach (var r in records)
            {
                sb.AppendLine($"{r.DeviceName},{r.SlaveId},{r.Address},{r.Value},{r.RecordTime:yyyy-MM-dd HH:mm:ss}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public void ExportToExcel(List<HistoryModel> records, string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("历史记录");

            // 表头
            IRow header = sheet.CreateRow(0);
            header.CreateCell(0).SetCellValue("设备");
            header.CreateCell(1).SetCellValue("从站");
            header.CreateCell(2).SetCellValue("地址");
            header.CreateCell(3).SetCellValue("值");
            header.CreateCell(4).SetCellValue("时间");

            // 数据
            for (int i = 0; i < records.Count; i++)
            {
                var r = records[i];
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(r.DeviceName ?? "");
                row.CreateCell(1).SetCellValue(r.SlaveId);
                row.CreateCell(2).SetCellValue(r.Address);
                row.CreateCell(3).SetCellValue(r.Value);
                row.CreateCell(4).SetCellValue(r.RecordTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            using var fs = new FileStream(filePath, FileMode.Create);
            workbook.Write(fs);
        }
    }
}
