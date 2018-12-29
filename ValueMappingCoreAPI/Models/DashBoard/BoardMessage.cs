using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueMappingCoreAPI.Models.DashBoard
{
    [Serializable]
    public class BoardMessage
    {
        //public BoardMessage(Guid guid,string msg)
        //{
        //    MsgID = guid;
        //    MsgContent = msg;
        //}
        public Guid MsgID { get; set; }
        public string MsgContent { get; set; }
    }
}
