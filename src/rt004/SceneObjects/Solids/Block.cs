using OpenTK.Mathematics;
using rt004.Util;
using System.Reflection.Metadata.Ecma335;

namespace rt004.SceneObjects
{
    /*
    public class Block : Solid
    {
        public readonly float sideA
        {
            get => sideA;
            set
            {
                sideA = value;
                planes[PlaneIndex.Top] = ;
                planes[1] = ;
            }
        }
        public readonly float sideB;
        public readonly float sideC;

        private Plane[] planes = new Plane[6];

        enum PlaneIndex
        {
            Front,
            Back,
            Left,
            Right,
            Bottom,
            Top

        }

        public Block(Scene parentScene, Vector3 position, Vector3 rotation, Color4 color, float sideA, float sideB, float sideC) : base (parentScene, position, rotation, color)
        {
            this.sideA = sideA;
            this.sideB = sideB;
            this.sideC = sideC;
        }

        public override Vector3 GetNormalAt(Vector3 position)
        {
            for 
            throw new NotImplementedException();
        }

        public override bool TryGetRayIntersection(Line line, out float parameter)
        {
            throw new NotImplementedException();
        }
    }
    */
}

namespace rt004.SceneObjects.Loading
{
    /*
    public class BlockLoader : SolidLoader
    {
        public float sideA;
        public float sideB;
        public float sideC;

        public BlockLoader()
        {
            sideA = 2;
            sideB = 1;
        }

        public BlockLoader(Vector3 position, Vector3 rotation, Color4 color, float sideA, float sideB, float sideC) : base(position, rotation, color)
        {
            this.sideA = sideA;
            this.sideB = sideB;
            this.sideC = sideC;
        }

        public override SceneObject CreateInstance(Scene parentScene)
        {
            return new Block(parentScene, position, rotation, color, sideA, sideB, sideC);
        }
    }
    */
}
