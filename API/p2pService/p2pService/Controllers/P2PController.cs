using System.Web.Http;
using P2P.Model;
using P2PBusinessLayer;


namespace P2PService.Controllers
{
    public class P2PController : ApiController
    {
       private IP2PBusinessLogic _p2pBusinessLogic = new P2PBusinessLogic(); 

        [HttpPost]
        public IHttpActionResult RegisterDevice([FromBody]Device device)
        {
            if (device == null || !ModelState.IsValid)
                return BadRequest("Device information is missing");
            
            return Ok(_p2pBusinessLogic.RegisterDevice(device));
        }

        [HttpGet]
        public IHttpActionResult GetOffers(string deviceId, string storeId ,bool refreshFlag)
        {
            
            var promotions = _p2pBusinessLogic.GetOffers(deviceId, storeId, refreshFlag);
            return Ok(promotions);

            //// request without store location
            //if (storeId == null || storeId == "0")
            //{
            //    //request is from refresh button
            //   offers  = _p2pBusinessLogic.GetOffers(deviceId);
            //    //return Ok(offers);
            //}
            //else
            //{
            //    _p2pBusinessLogic.GetOffers(deviceId,storeId);
            //}
            //return Ok(200);
        }

        
        
    }
}
