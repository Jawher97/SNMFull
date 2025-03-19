public class InstagramMediaInsightsResponse
{
    public List<InstagramMediaInsightsDto> Data { get; set; }
}

public class InstagramMediaInsightsDto
{
    public string Name { get; set; }
    public string Period { get; set; }
    public DateTime Date { get; set; }
    public InstagramMetricDto Metric { get; set; }
}

public class InstagramMetricDto
{
    public int Value { get; set; }
    public string Name { get; set; }
}
