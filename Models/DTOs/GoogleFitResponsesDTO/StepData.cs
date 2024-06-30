namespace NutrionE.Models.DTOs.GoogleFitResponsesDTO
{
    public class Value
    {
        public int intVal { get; set; }
        public double fpVal { get; set; }
        public List<object> mapVal { get; set; }
    }

    public class Point
    {
        public string startTimeNanos { get; set; }
        public string endTimeNanos { get; set; }
        public string dataTypeName { get; set; }
        public string originDataSourceId { get; set; }
        public List<Value> value { get; set; }
    }

    public class Dataset
    {
        public string dataSourceId { get; set; }
        public List<Point> point { get; set; }
    }

    public class Bucket
    {
        public string startTimeMillis { get; set; }
        public string endTimeMillis { get; set; }
        public List<Dataset> dataset { get; set; }
    }

    public class StepData
    {
        public List<Bucket> bucket { get; set; }
    }


}
