using lxwpf.Card.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Card
{
    public class LeadShineMotionService : IMotionService
    {
        public int CardCount { get; private set; }
        public int AxisCount { get; private set; }

        public LeadShineMotionService()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化板卡
        /// LTDMC.dmc_board_init() 返回卡数量，0 表示失败
        /// </summary>
        public bool Initialize()
        {
            CardCount = LTDMC.dmc_board_init();

            if (CardCount <= 0)
            {
                throw new Exception("雷赛板卡初始化失败");
            }

            // 读取 0 号卡的轴数量
            uint axisCount = 0;
            LTDMC.dmc_get_total_axes(0, ref axisCount);
            AxisCount = (int)axisCount;

            return true;
        }

        /// <summary>
        /// 回零
        /// LTDMC.dmc_home_move(cardNo, axisNo)
        /// </summary>
        public bool Home(int cardNo, int axisNo)
        {
            return LTDMC.dmc_home_move((ushort)cardNo, (ushort)axisNo) == 0;
        }

        /// <summary>
        /// 绝对移动
        /// 1. dmc_set_profile_unit 设置速度曲线
        /// 2. dmc_pmove_unit 模式 1 = 绝对
        /// </summary>
        public bool MoveAbs(int cardNo, int axisNo, double position)
        {
            // 设置速度参数：起始速度、最大速度、加速时间、减速时间、停止速度
            LTDMC.dmc_set_profile_unit(
                (ushort)cardNo, (ushort)axisNo,
                0,      // Min_Vel 起始速度
                100,    // Max_Vel 最大速度
                0.1,    // Tacc 加速时间（秒）
                0.1,    // Tdec 减速时间（秒）
                0);     // Stop_Vel 停止速度

            // 绝对移动，posi_mode = 1
            return LTDMC.dmc_pmove_unit(
                (ushort)cardNo, (ushort)axisNo, position, 1) == 0;
        }

        /// <summary>
        /// 相对移动
        /// dmc_pmove_unit 模式 0 = 相对
        /// </summary>
        public bool MoveRel(int cardNo, int axisNo, double distance)
        {
            LTDMC.dmc_set_profile_unit(
                (ushort)cardNo, (ushort)axisNo,
                0, 100, 0.1, 0.1, 0);

            // 相对移动，posi_mode = 0
            return LTDMC.dmc_pmove_unit(
                (ushort)cardNo, (ushort)axisNo, distance, 0) == 0;
        }

        /// <summary>
        /// 点动
        /// dmc_vmove(cardNo, axisNo, dir)
        /// dir = 1 正转，dir = 0 反转
        /// </summary>
        public bool Jog(int cardNo, int axisNo, bool direction)
        {
            ushort dir = (ushort)(direction ? 1 : 0);
            return LTDMC.dmc_vmove((ushort)cardNo, (ushort)axisNo, dir) == 0;
        }

        /// <summary>
        /// 停止
        /// dmc_stop(cardNo, axisNo, stop_mode)
        /// stop_mode = 0 减速停止
        /// </summary>
        public bool Stop(int cardNo, int axisNo)
        {
            return LTDMC.dmc_stop((ushort)cardNo, (ushort)axisNo, 0) == 0;
        }

        /// <summary>
        /// 伺服使能
        /// 用 dmc_write_sevon_pin(cardNo, axisNo, 1)
        /// </summary>
        public bool ServeOn(int cardNo, int axisNo)
        {
            return LTDMC.dmc_write_sevon_pin((ushort)cardNo, (ushort)axisNo, 1) == 0;
        }

        /// <summary>
        /// 伺服关闭
        /// 用 dmc_write_sevon_pin(cardNo, axisNo, 0)
        /// </summary>
        public bool ServeOff(int cardNo, int axisNo)
        {
            return LTDMC.dmc_write_sevon_pin((ushort)cardNo, (ushort)axisNo, 0) == 0;
        }

        /// <summary>
        /// 读位置
        /// dmc_get_position_unit(cardNo, axisNo, ref pos)
        /// </summary>
        public double GetPos(int cardNo, int axisNo)
        {
            double pos = 0;
            LTDMC.dmc_get_position_unit((ushort)cardNo, (ushort)axisNo, ref pos);
            return pos;
        }

        /// <summary>
        /// 读速度
        /// dmc_read_current_speed_unit(cardNo, axisNo, ref speed)
        /// </summary>
        public double GetVel(int cardNo, int axisNo)
        {
            double vel = 0;
            LTDMC.dmc_read_current_speed_unit((ushort)cardNo, (ushort)axisNo, ref vel);
            return vel;
        }
    }
}
