using System;
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
            try
            {
                if (device == null || !ModelState.IsValid)
                {
                    WriteLogToFile.WriteLog("Device information is missing");
                    return BadRequest("Device information is missing");
                }
                    

                return Ok(_p2pBusinessLogic.RegisterDevice(device));
            }
            catch (Exception ex)
            {
                WriteLogToFile.WriteLog(ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetOffers(string deviceId, string storeId ,bool refreshFlag)
        {
            try
            {
                var promotions = _p2pBusinessLogic.GetOffers(deviceId, storeId, refreshFlag);
                return Ok(promotions);
            }
            catch (Exception ex)
            {
                WriteLogToFile.WriteLog(ex.Message);
                return InternalServerError();
            }
        }
    }
}
