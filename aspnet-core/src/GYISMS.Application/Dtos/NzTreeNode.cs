using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Dtos
{
    public class NzTreeNode
    {
        public virtual string title { get; set; }
        public virtual string key { get; set; }

        /// <summary>
        /// 是否打开
        /// </summary>
        public virtual bool expanded { get; set; }

        /// <summary>
        /// 是否是树叶
        /// </summary>
        public virtual bool isLeaf { get; set; } 

        public virtual List<NzTreeNode> children { get; set; }
    }
}
