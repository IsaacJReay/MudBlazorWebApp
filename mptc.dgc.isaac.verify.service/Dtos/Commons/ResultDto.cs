namespace mptc.dgc.isaac.verify.service.Dtos.Commons
{
    public class ResultDto<T>
    {
        public string? Messages { get; set; }
        public bool Succeeded { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }

    }
}