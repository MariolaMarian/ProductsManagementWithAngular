using AutoMapper;

namespace ProductsMngmtAPI.Helpers.Converters
{
    public class ImageSourceToByteArrayConverter : IValueConverter<string, byte[]>
    {
        public byte[] Convert(string sourceMember, ResolutionContext context)
        {
            if(sourceMember.Contains("http"))
                return null;

            var imageDataByteArray = System.Convert.FromBase64String(sourceMember.Replace("data:image/jpeg;base64,","").Replace("data:image/png;base64,",""));
            return imageDataByteArray;
        }
    }
}