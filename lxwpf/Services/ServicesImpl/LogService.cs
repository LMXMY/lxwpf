using lxwpf.Entities;
using lxwpf.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services.ServicesImpl
{
    public class LogService : ILogService
    {
        //写法
        public void Info(string module, string message)
        {
            Write("Info", module, message, null);
        }
        public void Warn(string module, string message) => Write("Warn", module, message, null);
        public void Error(string module, string message, Exception? ex = null) => Write("Error", module, message, ex);

        private void Write(string level, string module, string message, Exception? ex)
        {
            try
            {
                using var db = new AppDbContext();
                db.Logs.Add(new LogModel
                {
                    Level = level,
                    Module = module,
                    Message = message,
                    Exception = ex?.ToString(),
                    LogTime = DateTime.Now
                });
                db.SaveChanges();
            }
            catch
            {
                // 日志写失败，不能再抛，吞掉
            }
        }

        public List<LogModel> Query(DateTime start, DateTime end, string? level, string? keyword)
        {
            using var db = new AppDbContext();
            var query = db.Logs.Where(l => l.LogTime >= start && l.LogTime <= end);

            if (!string.IsNullOrEmpty(level))
                query = query.Where(l => l.Level == level);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(l => l.Message != null && l.Message.Contains(keyword));

            return query.OrderByDescending(l => l.LogTime).Take(1000).ToList();
        }
    }
}
