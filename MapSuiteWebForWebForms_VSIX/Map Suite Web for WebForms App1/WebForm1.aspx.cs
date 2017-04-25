using System;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WebForms;

namespace Map_Suite_Web_for_WebForms_App1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the Map Unit. The reason for setting it to DecimalDegrees is that is what the shapefile’s unit of measure is inherently in.
                Map1.MapUnit = GeographyUnit.DecimalDegree;

                WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
                Map1.CustomOverlays.Add(worldOverlay);

                LayerOverlay layerOverlay = new LayerOverlay();
                ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(MapPath(@"App_Data\states.shp"));
                shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
                shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
                layerOverlay.Layers.Add(shapeFileLayer);
                Map1.CustomOverlays.Add(layerOverlay);

                shapeFileLayer.Open();
                Map1.CurrentExtent = shapeFileLayer.GetBoundingBox();
            }
        }
    }
}