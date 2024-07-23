namespace BackEnd.Class
{
    public class RequestInfo
    {
        public int skip { get; set; } = 0;
        public int take { get; set; }
        public bool requireTotalCount { get; set; } = false;
        public string group { get; set; }
    }
}
