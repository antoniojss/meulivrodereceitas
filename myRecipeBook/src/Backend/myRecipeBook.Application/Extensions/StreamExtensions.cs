using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace myRecipeBook.Application.Extensions
{
    internal static class StreamExtensions
    {
        internal static string? DetectImageContentType(this Stream stream)
        {
            string? contentType = null;    
            
            if (stream.Is<PortableNetworkGraphic>())
            {
                contentType = "image/png";
            }
            else if (stream.Is<JointPhotographicExpertsGroup>())
            {
                contentType = "image/jpeg";
            }
            //else if (stream.Is<GraphicsInterchangeFormat>())
            //{
            //    contentType = "image/gif";
            //}
            //else if (stream.Is<BitmapImageFile>())
            //{
            //    contentType = "image/bmp";
            //}
            //else if (stream.Is<JointPhotographicExpertsGroup2000>())
            //{
            //    contentType = "image/jp2";
            //}
            //else if (stream.Is<ExchangeableImageFileFormat>())
            //{
            //    contentType = "image/exif";
            //}
            //else if (stream.Is<WindowsMetafile>())
            //{
            //    contentType = "image/wmf";
            //}
            //else if (stream.Is<IconFile>())
            //{
            //    contentType = "image/x-icon";
            //}
            stream.Position = 0;
            return contentType; 
        }
    }
}
