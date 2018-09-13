using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.GYEnums
{
    public enum ConfigType
    {
        设备配置 = 1,
        会议物资 = 2,
        烟农单位 = 3,
        烟农村组 = 4,
        烟叶公共 = 5
    }

    public enum ConfigModel
    {
        会议管理 = 1,
        烟叶服务 = 2
    }

    public enum RoomType
    {
        固定会议室 = 1,
        临时会议室 = 2
    }

    public enum LayoutPattern
    {
        中心模式 = 1,
        矩阵莫事 = 2
    }
}
