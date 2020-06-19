using System;
using AutoMapper;

namespace ProductsMngmtAPI.Helpers.Converters
{
    public class ByteArrayToImageSourceConverter : ITypeConverter<byte[],string>
    {
        public string Convert(byte[] source, string destination, ResolutionContext context)
        {
            if(source != null && source.Length > 1)
                return System.Convert.ToBase64String(source);
            return "https://i5.walmartimages.ca/images/Large/799/2_r/6000196087992_R.jpg";
        }
    }
}