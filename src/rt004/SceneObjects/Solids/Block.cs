using OpenTK.Mathematics;
using rt004.Util;

namespace rt004.SceneObjects
{
    public class Block : Solid
    {
        public readonly float sideA;
        public readonly float sideB;

        public Block() : base(Vector3.Zero, Vector3.Zero)
        {
            sideA = 2;
            sideB = 1;
        }


        public Block(Vector3 position, Vector3 rotation, Color4 color, float sideA, float sideB) : base (position, rotation, color)
        {
            this.sideA = sideA;
            this.sideB = sideB;
        }

        public override bool TryGetRayIntersection(Line line, out float parameter)
        {
            throw new NotImplementedException();
        }
    }
}

namespace rt004.SceneObjects.Loading
{
    public class BlockLoader : SolidLoader
    {
        public float sideA;
        public float sideB;

        public BlockLoader()
        {

        }

        public BlockLoader(Vector3 position, Vector3 rotation, Color4 color, float sideA, float sideB) : base(position, rotation, color)
        {
            this.sideA = sideA;
            this.sideB = sideB;
        }

        public override SceneObject CreateInstance()
        {
            return new Block(position, rotation, color, sideA, sideB);
        }
    }
}
