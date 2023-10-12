namespace rt004_tests
{
    using OpenTK.Mathematics;
    using rt004;
    using rt004.Loading;
    using rt004.MaterialLoading;
    using rt004.Materials.Loading;
    using rt004.SceneObjects;
    using rt004.SceneObjects.Loading;
    using rt004.Util;
    using rt004.UtilLoading;

    [TestFixture]
    public class Tests
    {
        Scene testScene;

        [SetUp]
        public void Setup()
        {
            TextureLoader whiteUniform = new UniformTextureLoader(Color4.White);

            var rendererSetting = new RendererSettingsLoader();
            rendererSetting.lightModel = "PhongModel";
            var materialYellow = new PhongMaterialLoader(new Color4(1f, 1f, 0.2f, 1f), 0.2f, 0.5f, 10f, 1f);
            var materialBlue = new PhongMaterialLoader(new Color4(0.2f, 0.3f, 1f, 1f), 0.5f, 0.5f, 150f, 1f);
            var materialRed = new PhongMaterialLoader(new Color4(0.8f, 0.2f, 0.2f, 1f), 0.4f, 0.5f, 80f, 1f);
            var materialGold = new PhongMaterialLoader(new Color4(0.3f, 0.2f, 0f, 1f), 0.8f, 0.5f, 400f, 1f);
            var materialWhite = new PhongMaterialLoader(new Color4(0.9f, 0.9f, 0.9f, 1f), 0.4f, 0.5f, 80f, 1f);



            SceneLoader sceneLoader = new SceneLoader();
            sceneLoader.sceneObjectLoaders.Add(new SphereLoader(new Point3D(0, 0, 0), new Vector3(0, 0, 0), materialYellow, 1f));// 0

            SceneObjectLoader sphereLoader1 = new SphereLoader(new Point3D(4, 0, 0), new Vector3(0, 0, 0), materialBlue, 0.6f);//1.0
            SceneObjectLoader sphereLoader2 = new SphereLoader(new Point3D(0, 4, -1), new Vector3(0, 0, 0), materialRed, 0.1f);//1.1

            SceneObjectLoader[] sphereLoaders = new SceneObjectLoader[] {sphereLoader1, sphereLoader2};
            sceneLoader.sceneObjectLoaders.Add(new InnerSceneObjectLoader(new Point3D(0,0,0), new Vector3(0,0,MathF.PI / 4)) { children = sphereLoaders });
            
            
            
            sceneLoader.sceneObjectLoaders.Add(new SphereLoader(new Point3D(1.5f, 0.6f, 0.1f), new Vector3(0, 0, 0), materialGold, 0.5f));
            sceneLoader.sceneObjectLoaders.Add(new PlaneLoader(new Point3D(0, -1.3f, 0), new Vector3(0, 0, 0), materialWhite));
    
            sceneLoader.ambientLightIntensity = 1f;
            sceneLoader.ambientLightColor = Color4.White;
            PointLightLoader light1 = new PointLightLoader(new Point3D(-10f, 8f, -6f), Vector3.Zero, Color4.White, 1, 1f, 1f);
            PointLightLoader light2 = new PointLightLoader(new Point3D(0f, 20f, -3f), Vector3.Zero, new Color4(0.3f, 0.3f, 0.3f, 1f), 1, 1f, 1f);
            LightSourceLoader[] lights = new LightSourceLoader[] { light1, light2 };
            sceneLoader.sceneObjectLoaders.Add(new InnerSceneObjectLoader(new Point3D(1,1,1), new Vector3(MathF.PI, 0, 0)) { children = lights});


            sceneLoader.sceneObjectLoaders.Add(new PrespectiveCameraLoader(new Point3D(0.6f, 0f, -5.6f), new Vector3(0f, -0.03f, 0), new Color4(0.1f, 0.2f, 0.3f, 0f), MathF.PI * 4f / 9f, 600, 450));

            testScene = sceneLoader.CreateInstance();
        }

        [Test, Description("tests Global position computation")]
        public void GlobalPositionTest()
        {
            Assert.IsTrue(testScene.rootSceneObject.GetChildren()[0].GlobalPosition == Point3D.Zero);

            
            InnerSceneObject rootChildAtIndexOne = (InnerSceneObject)testScene.rootSceneObject.GetChildren()[1];
            Assert.IsTrue(rootChildAtIndexOne.GetChildren()[0].GlobalPosition == new Point3D(2.8284f, 2.8284f, 0), $"object at indexes 1.0 is is not at [2, 2, 0] but at {rootChildAtIndexOne.GetChildren()[0].GlobalPosition}");
            Assert.IsTrue(rootChildAtIndexOne.GetChildren()[1].GlobalPosition == new Point3D(-2.8284f, 2.8284f, -1), $"object at indexes 1.1 is is not at [-2, 2, -1] but at {rootChildAtIndexOne.GetChildren()[1].GlobalPosition}");

            InnerSceneObject rootChildAtIndexFour = (InnerSceneObject)testScene.rootSceneObject.GetChildren()[4];
            Assert.IsTrue(rootChildAtIndexFour.GetChildren()[0].GlobalPosition == new Point3D(-9, -7, 7), $"object at indexes 4.0 is is not at [-9, -7, 7] but at {rootChildAtIndexFour.GetChildren()[0].GlobalPosition}");
            Assert.IsTrue(rootChildAtIndexFour.GetChildren()[1].GlobalPosition == new Point3D(1, -19, 4), $"object at indexes 4.1 is is not at [1, -19, 4] but at {rootChildAtIndexFour.GetChildren()[1].GlobalPosition}");
        }
    }
}