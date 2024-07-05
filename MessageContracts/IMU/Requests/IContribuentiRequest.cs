namespace MessageContracts.IMU.Requests
{
    public interface IContribuentiRequest
    {
        public string? CodiceFiscale { get; }
        public string? Denominazione { get; }
    }
}
