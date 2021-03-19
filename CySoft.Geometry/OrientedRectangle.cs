using System;
using System.Numerics;

namespace CySoft.Geometry
{
    public readonly struct OrientedRectangle
    {
        /// <summary>
        /// Represents an oriented rectangle or box.
        /// </summary>
        /// <remarks>Properties ending in "Squared" require no square root calculations. The other properties require
        /// one square root calculation. <c>WidthSquared</c> and <c>HeightSquared</c> are calculated in the constructor.
        /// The other properties are calculated on the fly.</remarks>
        /// <param name="v0">Rectangle vertex 0</param>
        /// <param name="v1">Rectangle vertex 1</param>
        /// <param name="v2">Rectangle vertex 2</param>
        /// <param name="v3">Rectangle vertex 3</param>
        public OrientedRectangle(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3)
        {
            V0 = v0; V1 = v1; V2 = v2; V3 = v3;
            WidthSquared = (v0 - v1).LengthSquared();
            HeightSquared = (v1 - v2).LengthSquared();
        }

        public readonly Vector2 V0;
        public readonly Vector2 V1;
        public readonly Vector2 V2;
        public readonly Vector2 V3;

        public float WidthSquared { get; }
        public float HeightSquared { get; }

        public float Width => MathF.Sqrt(WidthSquared);
        public float Height => MathF.Sqrt(HeightSquared);

        public float AreaSquared => WidthSquared * HeightSquared;
        public float Area => MathF.Sqrt(WidthSquared * HeightSquared);

        public float MinSideSquared => MathF.Min(WidthSquared, HeightSquared);
        public float MaxSideSquared => MathF.Max(WidthSquared, HeightSquared);

        public float MinSide => MathF.Sqrt(MathF.Min(WidthSquared, HeightSquared));
        public float MaxSide => MathF.Sqrt(MathF.Max(WidthSquared, HeightSquared));

        /// <summary>
        /// Ratio of the smallest side squared divided by the longest side squared. A value in the range 0..1.
        /// </summary>
        /// <remarks>Does not require square roots to be calculated.</remarks>
        public float RatioSquared => MathF.Min(WidthSquared, HeightSquared) / MathF.Max(WidthSquared, HeightSquared);

        /// <summary>
        /// Ratio of the smallest side divided by the longest side. A value in the range 0..1.
        /// </summary>
        public float Ratio => MathF.Sqrt(MathF.Min(WidthSquared, HeightSquared) / MathF.Max(WidthSquared, HeightSquared));
    }
}
