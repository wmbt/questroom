using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuestRoom.Types
{
    public class Promocode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public double Factor { get; set; }

        public Promocode(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("ID");
            Code = dr.GetValueOrDefault<string>("CODE");
            Factor = dr.GetValueOrDefault<double>("FACTOR");
        }
    }
}