using Android.App;
using Android.OS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;

namespace Map_Suite_Mobile_for_Android_App1
{
    [Activity(Label = "Map_Suite_Mobile_for_Android_App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly static string AssetsDataDictionary = @"AppData";
        private readonly static string SampleDataDictionary = @"mnt/sdcard/MapSuiteSampleData/";

        private MapView map1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Collection<string> unLoadDatas = CollectUnloadDatas(SampleDataDictionary, AssetsDataDictionary);
            UploadDataFiles(SampleDataDictionary, unLoadDatas);

            map1 = FindViewById<MapView>(Resource.Id.map1);
            map1.MapUnit = GeographyUnit.DecimalDegree;
            WorldStreetsAndImageryOverlay worldOverlay = new WorldStreetsAndImageryOverlay();
            map1.Overlays.Add(worldOverlay);

            LayerOverlay layerOverlay = new LayerOverlay();
            ShapeFileFeatureLayer shapeFileLayer = new ShapeFileFeatureLayer(Path.Combine(SampleDataDictionary, AssetsDataDictionary, "states.shp"));
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = WorldStreetsAreaStyles.ProtectedArea();
            shapeFileLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            layerOverlay.Layers.Add(shapeFileLayer);
            map1.Overlays.Add(layerOverlay);

            shapeFileLayer.Open();
            map1.CurrentExtent = shapeFileLayer.GetBoundingBox();
            map1.Refresh();
        }

        private Collection<string> CollectUnloadDatas(string targetDirectory, string sourceDirectory)
        {
            Collection<string> result = new Collection<string>();

            foreach (string filename in Assets.List(sourceDirectory))
            {
                string sourcePath = Path.Combine(sourceDirectory, filename);
                string targetPath = Path.Combine(targetDirectory, sourcePath);

                if (!string.IsNullOrEmpty(Path.GetExtension(sourcePath)) && !File.Exists(targetPath))
                {
                    result.Add(sourcePath);
                }
                else if (string.IsNullOrEmpty(Path.GetExtension(sourcePath)))
                {
                    foreach (string item in CollectUnloadDatas(targetDirectory, sourcePath))
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        private void UploadDataFiles(string targetDirectory, IEnumerable<string> sourcePathFilenames)
        {
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

            foreach (string sourcePathFilename in sourcePathFilenames)
            {
                string targetPathFilename = Path.Combine(targetDirectory, sourcePathFilename);
                if (!File.Exists(targetPathFilename))
                {
                    string targetPath = Path.GetDirectoryName(targetPathFilename);
                    if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
                    Stream sourceStream = Assets.Open(sourcePathFilename);
                    FileStream fileStream = File.Create(targetPathFilename);
                    sourceStream.CopyTo(fileStream);
                    fileStream.Close();
                    sourceStream.Close();
                }
            }
        }
    }
}

