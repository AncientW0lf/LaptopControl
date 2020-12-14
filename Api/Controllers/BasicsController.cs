using System.Dynamic;
using System;
using Microsoft.AspNetCore.Mvc;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Threading.Tasks;
using System.Text.Json;

namespace LaptopControl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasicsController : ControllerBase, IDisposable
    {
        private readonly CoreAudioController _controller = new CoreAudioController();

        private CoreAudioDevice Device
        {
            get
            {
                return _controller.DefaultPlaybackDevice;
            }
        }

        [HttpGet("get/volume")]
        public async Task<double> GetVolume()
        {
            return await Device.GetVolumeAsync();
        }

        [HttpPost("post/volume")]
        public async Task<ActionResult> PostVolume([FromBody] JsonElement content)
        {
            dynamic json = JsonSerializer.Deserialize<ExpandoObject>(content.ToString());

            try
            {
                await Device.SetVolumeAsync(double.Parse(json.volume.ToString()));
            }
            catch (Exception exc)
            {
                return Problem(exc.Message);
            }

            return Ok();
        }

        public void Dispose()
        {
            _controller.Dispose();
        }
    }
}