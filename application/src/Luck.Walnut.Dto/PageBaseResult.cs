namespace Luck.Walnut.Dto
{
    public class PageBaseResult<T>
    {
        public PageBaseResult(int total, T[] data)
        {
            Total = total;
            Data = data;
        }

        public int Total { get; set; }
        public T[] Data { get; set; }
    }
}
