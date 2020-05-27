using AnimalProtect.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using ImageProcessor.Processors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace AnimalProtect.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IHostingEnvironment _appEnvironment;
        
        public ImageController(IHostingEnvironment env)
        {
            _appEnvironment = env;
          
        }

        //Get images by id list
        [Route("api/[controller]/GetImageByListId")]
        [HttpPost("GetImageByListId")]
        public ActionResult<IEnumerable<Image>> Post([FromBody] List<int> ImageIds)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                var query2 = _context.Images.Where(
                    BuildContainsExpression<Image, int>(e => e.id, ImageIds));
                return query2.ToList();
            }

        }

        //Get image by id
        [Route("api/[controller]/GetImageById")]
        [HttpPost("GetImageById")]
        public ActionResult<Image> Post([FromBody] int Id)
        {
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                try
                {
                return _context.Images.FirstOrDefault(a => a.id == Id);
                }
                catch (Exception e)
                {
                    Image error = new Image();
                    error.path = e.InnerException.Message;
                    
                    return error;
                   
                }
            }
        }

        //Upload a new image and get image string
        [Route("api/[controller]/create")]
        [HttpPost("content/upload-image")]
        public async Task<string> Post(IFormFile image)
        {
            string f = Directory.GetCurrentDirectory();
            string path =  "/uploads/img/" + string.Format(@"{0}.webp", DateTime.Now.Ticks);
            int result = 0;
            string response;
            Image responseImage = new Image();
            try
            {
                using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
                {
                    int maxid = 0;
                    if (_context.Images.Count() > 0)
                    {
                        maxid = _context.Images.OrderByDescending(a => a.id).FirstOrDefault().id;
                    }

                    responseImage.id = maxid + 1;
                    if (image == null || image.Length == 0)
                    {
                        response = BadRequest().StatusCode.ToString() + "," + "Add at least 1 photo";
                    }
                    else
                    {
                        var resizedImgStream = new MemoryStream();
                        
                        using (var memoryStream = new MemoryStream())
                        {
                            await image.CopyToAsync(memoryStream);
                            
                            responseImage.sha=GetHashSha256(memoryStream);
                            if (_context.Images.Any(a => a.sha == responseImage.sha))
                            {
                                responseImage= _context.Images.First(a => a.sha == responseImage.sha);
                                response = responseImage.path;
                            }
                            else
                            {
                               
                                IResampler sampler = KnownResamplers.Lanczos3;
                                memoryStream.Position = 0;
                                SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(memoryStream);

                                // TODO: ResizeImage(img, 100, 100);
                                if (img.Width * img.Height > 2073600)
                                {
                                    if (img.Width == img.Height)
                                    {

                                        img.Mutate(x => x.Resize(1080, 1080, sampler));
                                    }
                                    else
                                    {
                                        if (img.Width > img.Height)
                                        {
                                            //16:9 case else 4:3 case
                                            if ((img.Width / img.Height) > 1.34)
                                            {
                                                img.Mutate(x => x.Resize(1920, 1080, sampler));
                                            }
                                            else
                                            {
                                                img.Mutate(x => x.Resize(1280, 960, sampler));
                                            }
                                        }
                                        else
                                        {
                                            //16:9 case else 4:3 case
                                            if ((img.Width / img.Height) > 1.34)
                                            {
                                                img.Mutate(x => x.Resize(1080, 1920, sampler));
                                            }
                                            else
                                            {
                                                img.Mutate(x => x.Resize(960, 1280, sampler));
                                            }
                                        }

                                    }

                                    img.Save(resizedImgStream, new BmpEncoder());
                                    using (var webPImage = new ImageFactory(preserveExifData: false))
                                    {
                                        resizedImgStream.Position = 0;
                                        webPImage.Load(resizedImgStream)
                                            .Format(new WebPFormat())
                                            .Quality(90)
                                            .Save(Directory.GetCurrentDirectory() + path);
                                    }
                                    response = path;
                                }
                                else
                                {
                                    using (var webPImage = new ImageFactory(preserveExifData: false))
                                    {
                                        memoryStream.Position = 0;
                                        webPImage.Load(memoryStream)
                                            .Format(new WebPFormat())
                                            .Quality(90)
                                            .Save(Directory.GetCurrentDirectory() + path);
                                    }
                                    response = path;
                                    
                                }

                                responseImage.path = path;
                                _context.Images.Add(responseImage);
                                try
                                {
                                    result = _context.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    response = e.Message;
                                }
                            }
                        };
                    }
                }
            }
            catch (Exception e)
            {
               

                response = "Fail" + Environment.NewLine + e.Message;
            }

            return response;
            }

        // DELETE api/values/5
        [Route("api/[controller]/delete")]
        [HttpDelete("delete")]
        public ActionResult<ResponseMessage> Delete([FromBody]int id)
        {
            ResponseMessage response = new ResponseMessage();
            using (AnimalProjectDbContext _context = new AnimalProjectDbContext())
            {
                int result = 0;


                if (_context.Images.Any(a => a.id == id))
                {
                    response.message = "Image is exist"; 
                    string path = _context.Images.First(a => a.id == id).path; 
                    _context.Images.Remove(_context.Images.First(a => a.id == id));
                    try
                    {
                        result = _context.SaveChanges();
                        if (result == 1)
                        {
                            response.message = "Success";
                            if (System.IO.File.Exists(Directory.GetCurrentDirectory() + path))
                            {
                                System.IO.File.Delete(Directory.GetCurrentDirectory() + path);

                            }
                        }
                        else
                        {
                            response.message = "Delete Error";

                        }
                    }
                    catch
                    {
                        response.message = "Error";
                    }



                }
                else
                {
                    response.message = "Image not found by id";
                }

            }
            return response;
        }

        static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }

            if (null == values) { throw new ArgumentNullException("values"); }

            ParameterExpression p = valueSelector.Parameters.Single();

            // p => valueSelector(p) == values[0] || valueSelector(p) == ...

            if (!values.Any())

            {

                return e => false;

            }

            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));

            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
        static string GetHashSha256(MemoryStream stream)
        {

            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] sha256Ret = sha256.ComputeHash(stream);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sha256Ret.Length; i++)
            {
                sb.Append(sha256Ret[i].ToString("x2"));
            }

            return sb.ToString();

        }

    }
}

