using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Card.Interface
{
    public interface IMotionService
    {
        // 初始化
        bool Initialize();

        // 卡数量、轴数量
        int CardCount { get; }
        int AxisCount { get; }

        // 轴控制
        bool Home(int cardNo, int axisNo);                              // 回零
        bool MoveAbs(int cardNo, int axisNo, double position);          // 绝对移动
        bool MoveRel(int cardNo, int axisNo, double distance);          // 相对移动
        bool Jog(int cardNo, int axisNo, bool direction);               // 点动
        bool Stop(int cardNo, int axisNo);                              // 停止
        bool ServeOn(int cardNo, int axisNo);                           // 伺服使能
        bool ServeOff(int cardNo, int axisNo);                          // 伺服关闭

        // 状态读取
        double GetPos(int cardNo, int axisNo);                          // 读位置
        double GetVel(int cardNo, int axisNo);                          // 读速度
    }
}
