using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WebApi;

namespace Map_Suite_Web_for_WebApi_App1.Controllers
{
    [RoutePrefix("Map_Suite_Web_for_WebApi_App1")]
    public class HelloWorldController : ApiController
    {
        [Route("tile/{z}/{x}/{y}")]
        [HttpGet]
        public HttpResponseMessage GetTile(int z, int x, int y)
        {
            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(HttpContext.Current.Server.MapPath("~/App_Data/states.shp"));
            Proj4Projection proj4 = new Proj4Projection(Proj4Projection.GetWgs84ParametersString(), Proj4Projection.GetSphericalMercatorParametersString());
            if (!proj4.IsOpen) proj4.Open();
            statesLayer.FeatureSource.Projection = proj4;
            statesLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(statesLayer);

            return DrawLayerOverlay(layerOverlay, z, x, y);
        }

        private HttpResponseMessage DrawLayerOverlay(LayerOverlay layerOverlay, int z, int x, int y)
        {
            using (Bitmap bitmap = new Bitmap(256, 256))
            {
                PlatformGeoCanvas geoCanvas = new PlatformGeoCanvas();
                RectangleShape boundingBox = WebApiExtentHelper.GetBoundingBoxForXyz(x, y, z, GeographyUnit.Meter);
                geoCanvas.BeginDrawing(bitmap, boundingBox, GeographyUnit.Meter);
                layerOverlay.Draw(geoCanvas);
                geoCanvas.EndDrawing();

                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Png);

                HttpResponseMessage msg = new HttpResponseMessage(HttpStatusCode.OK);
                msg.Content = new ByteArrayContent(ms.ToArray());
                msg.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                return msg;
            }
        }
    }
}