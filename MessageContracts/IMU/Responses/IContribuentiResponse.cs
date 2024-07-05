using MessageContracts.IMU.Model;

namespace MessageContracts.IMU.Responses
{
    public interface IContribuentiResponse
    {
        public IEnumerable<Contribuente> Contribuenti { get; }
    }
}
