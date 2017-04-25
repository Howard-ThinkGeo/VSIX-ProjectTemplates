using System.Web.Mvc;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Mvc;
using ThinkGeo.MapSuite.Styles;

namespace Map_Suite_Web_for_MVC_App1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Map map = new Map("Map1",
               new System.Web.UI.WebControls.Unit(800, System.Web.UI.WebControls.UnitType.Pixel),
               new System.Web.UI.WebControls.Unit(600, System.Web.UI.WebControls.UnitType.Pixel));

            map.MapUnit = GeographyUnit.DecimalDegree;

            WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
            map.CustomOverlays.Add(worldOverlay);

            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(Request.MapPath("~/App_Data/states.shp"));
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);
            map.CustomOverlays.Add(layerOverlay);

            shapeFileLayer.Open();
            map.CurrentExtent = shapeFileLayer.GetBoundingBox();

            return View(map);
        }
    }
}