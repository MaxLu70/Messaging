using MassTransit;
using MessageContracts.IMU.Requests;
using MessageContracts.IMU.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MessageGateway.Controllers.Imu
{
    [Route("/api/imu/contribuenti")]
    public class ImuContribuentiController : ControllerBase
    {
        private readonly IRequestClient<IContribuentiRequest> messageClient;
        public ImuContribuentiController(IRequestClient<IContribuentiRequest> messageClient)
        {
            this.messageClient = messageClient;
        }

        [HttpGet("contribuenti")]
        public async Task<IActionResult> ElencoContribuenti(string codiceFiscale, string denominazione, CancellationToken cancellationToken)
        {
            var request = new
            {
                CodiceFiscale = codiceFiscale,
                Denominazione = denominazione
            };
            var result = await messageClient.GetResponse<IContribuentiResponse>(request, cancellationToken);
            if (result != null)
            {
                var c = result.Message.Contribuenti;

                return Ok(c);
            }
            return NotFound();
        }
    }
}

