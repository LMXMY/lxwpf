using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface ILogService
    {
        void Info(string module, string message);
        void Warn(string module, string message);
        void Error(string module, string message, Exception? ex = null);
        List<LogModel> Query(DateTime start, DateTime end, string? level, string? keyword);
    }
}
