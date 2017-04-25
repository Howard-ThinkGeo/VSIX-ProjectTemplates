using System;
using System.Windows.Forms;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WinForms;

namespace Map_Suite_Desktop_for_WinForms_App1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            winformsMap1.MapUnit = GeographyUnit.DecimalDegree;

            WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
            winformsMap1.Overlays.Add(worldOverlay);

            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(@"..\..\AppData\states.shp");
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.Military();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);
            winformsMap1.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();
            winformsMap1.CurrentExtent = shapeFileLayer.GetBoundingBox();
            winformsMap1.Refresh();
        }
    }
}
