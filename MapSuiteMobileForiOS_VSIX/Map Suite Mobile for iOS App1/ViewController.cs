using System;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.iOS;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;
using UIKit;

namespace Map_Suite_Mobile_for_iOS_App1
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            MapView mapView = new MapView(View.Frame);
            View.Add(mapView);

            mapView.MapUnit = GeographyUnit.DecimalDegree;
            WorldStreetAndImageryOverlay worldOverlay = new WorldStreetAndImageryOverlay();
            mapView.Overlays.Add(worldOverlay);

            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(Path.Combine("AppData", "states.shp"));
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.ProtectedArea();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);
            mapView.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();
            mapView.CurrentExtent = shapeFileLayer.GetBoundingBox();
            mapView.Refresh();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}