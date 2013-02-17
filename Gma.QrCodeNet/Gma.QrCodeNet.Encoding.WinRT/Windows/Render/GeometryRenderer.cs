using System;
using System.Windows;

#if NETFX_CORE
using Windows.Foundation;
using Windows.UI.Xaml.Media;
#else
using System.Windows.Media;
#endif

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
    public static class GeometryRenderer
    {
        public static GeometryGroup DarkModuleGeometry(BitMatrix matrix)
        {
            GeometryCollection gCollection = new GeometryCollection();
            GeometryGroup gGroup = new GeometryGroup();
            if (matrix == null)
            {
                gGroup.Children = gCollection;
                return gGroup;
            }

            int preX = -1;
            int width = matrix.Width;
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (matrix[x, y])
                    {
                        //Set start point if preX == -1
                        if (preX == -1)
                            preX = x;
                        //If this is last module in that row. Draw rectangle
                        if (x == width - 1)
                        {
                            gCollection.Add(CreateRectGeometry(new Rect(preX, y, x - preX + 1, 1)));
                            preX = -1;
                        }
                    }
                    else if (!matrix[x, y] && preX != -1)
                    {
                        //Here will be first light module after sequence of dark module.
                        //Draw previews sequence of dark Module
                        gCollection.Add(CreateRectGeometry(new Rect(preX, y, x - preX, 1)));
                        preX = -1;
                    }
                }
            }

            gGroup.Children = gCollection;
            return gGroup;
        }

        private static RectangleGeometry CreateRectGeometry(Rect rect)
        {
            RectangleGeometry result = new RectangleGeometry();
            result.Rect = rect;
            return result;
        }
    }
}
