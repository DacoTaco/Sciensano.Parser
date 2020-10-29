using System;

namespace Sciensano.CovidJson.Parser.SciensanoModels
{
    //{\"DATE\":\"2020-03-02\",\"PROVINCE\":\"Limburg\",\"REGION\":\"Flanders\",\"TESTS_ALL\":23,\"TESTS_ALL_POS\":0}
    public class SciensanoTestsModel : IDatedSciensanoModel
    {
        public DateTime Date { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public int Tests_all { get; set; }
        public int Tests_All_Pos { get; set; }
    }
}
