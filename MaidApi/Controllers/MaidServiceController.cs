using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaidApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaidServiceController : ControllerBase
    {
        [HttpGet("LoginPage/Message"), HttpHeader("version" , "1.0" )]
        public async Task<string> GetLoginMessageV1()
        {
            var res = await Task.FromResult("Welcome to the Maid Service App!");
            return res;
        }

        [HttpGet("LoginPage/Message"), HttpHeader("version", "2.0")]
        public async Task<string> GetLoginMessageV2()
        {
            var res = await Task.Run(() => "Unwelcome to the Maid Service App! 🎉");
            return res;
        }

        [HttpGet("Logo"), HttpHeader("version", "1.0")]
        public async Task<string> GetImageUrlV1()
        {
            var res = await Task.FromResult("https://i1.sndcdn.com/artworks-L1CET0ScnAzzuvlk-m21Cpg-t200x200.jpg");
            return res;
        }

        [HttpGet("Logo"), HttpHeader("version", "2.0")]
        public async Task<string> GetImageUrlV2()
        {
            var res = await Task.FromResult("https://th.bing.com/th/id/R.03e2a7b7cb568a393207ba4300e3f508?rik=%2fL38Z2%2bWR%2b4t1Q&riu=http%3a%2f%2fcdn.shopify.com%2fs%2ffiles%2f1%2f0017%2f9578%2f4765%2fproducts%2f185_1200x1200.jpg%3fv%3d1548433806&ehk=xrsvaPMzRkcKKc7Sw%2fACKdHXYIH5sqo174iXiVxuRJ0%3d&risl=&pid=ImgRaw&r=0");
            return res;
        }

        [HttpGet("LoginPage/Fontsize"), HttpHeader("version", "1.0")]
        public async Task<int> GetFontSizeV1()
        {
            var res = await Task.FromResult(18);
            return res;
        }

        [HttpGet("LoginPage/Fontsize"), HttpHeader("version", "2.0")]
        public async Task<int> GetFontSizeV2()
        {
            var res = await Task.FromResult(48);
            return res;
        }
    }
}
