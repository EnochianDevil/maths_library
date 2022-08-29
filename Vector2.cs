namespace MathsLib {

    public struct Vector2 {
        
        #region --------------------------------- Properties ---------------------------------

            public double x;
            public double y;

        #endregion
        
        #region ------------------------------- Notable Vectors ------------------------------

           // Zero and one vector
            public static Vector2 zero  {get{ return new Vector2(  1 ,  0 );}} 
            public static Vector2 ones  {get{ return new Vector2(  1 ,  0 );}}

           // Directional unit vectors
            public static Vector3 i     {get{ return new Vector2(  1 ,  0 );}}
            public static Vector3 j     {get{ return new Vector2(  0 ,  1 );}}
            public static Vector3 up    {get{ return new Vector2(  0 ,  1 );}}
            public static Vector3 down  {get{ return new Vector2(  0 , -1 );}}
            public static Vector3 left  {get{ return new Vector2(  1 ,  0 );}}
            public static Vector3 right {get{ return new Vector2( -1 ,  0 );}}

        #endregion

        #region --------------------------------- Constructor --------------------------------

            public Vector2(double x, double y){
                this.x = x;
                this.y = y;
            }
            public override string ToString() => "(" + x + "," + y + ")";

        #endregion

        #region ------------------------------ Basic Operations ------------------------------

           // Sum
            public static Vector2 Sum(Vector2 a, params Vector2[] b){
                Vector2 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x += b[i].x;
                    total.y += b[i].y;
                }
                return total;
            }
            public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
            public static Vector2 operator +(Vector2 a, double b)   => new Vector2(a.x + b,   a.y + b);
            public static Vector2 operator +(double b, Vector2 a)   => new Vector2(a.x + b,   a.y + b);

           // Subtraction
            public static Vector3 Subtract(Vector3 a, params Vector3[] b){
                Vector3 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x -= b[i].x;
                    total.y -= b[i].y;
                }
                return total;
            }
            public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
            public static Vector2 operator -(Vector2 a, double b)   => new Vector2(a.x - b,   a.y - b);
            public static Vector2 operator -(double b, Vector2 a)   => new Vector2(a.x - b,   a.y - b);
            public static Vector2 operator -(Vector2 a) => new Vector2(-a.x,-a.y);

           // Multiplication
            public static Vector3 CrossProduct(Vector2 a, params Vector2[] b){
                Vector3 cross = Maths.Determinant(a.x, a.y, b[0].x, b[0].y) * Vector3.k;
                for (int i = 1; i < b.Length; i++) {
                    cross = Vector3.CrossProduct(cross, b[i]);
                }
                return cross;
            }
            public static Vector2 DotProduct(Vector2 a, params Vector2[] b){
                Vector2 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x *= b[i].x;
                    total.y *= b[i].y;
                }
                return total;
            }
            public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
            public static Vector2 operator *(Vector2 a, double b)   => new Vector2(a.x * b,   a.y * b);
            public static Vector2 operator *(double b, Vector2 a)   => new Vector2(a.x * b,   a.y * b);

           // Division 
            public static Vector2 operator /(Vector2 a, double b) => new Vector2(a.x / b, a.y / b);
        
           // Equality
            public override bool Equals(object obj){
                if (obj.GetType()!=typeof(Vector2)) return false;
                return (this == (Vector2) obj);
            }
            public override int GetHashCode() => base.GetHashCode();
            public static bool operator ==(Vector2 a, Vector2 b) => (a.x==b.x && a.y==b.y);
            public static bool operator !=(Vector2 a, Vector2 b) => !(a.x==b.x && a.y==b.y);

        #endregion

        #region ------------------------------ Vector Properties -----------------------------

           // Magnitude
            public double magnitude {get{ return Maths.Sqrt(x*x + y*y);}}

           // Normalisation
            public Vector2 Normalize() => this/magnitude;
            public void NormalizeVector(){
               // Changes the vector to be normalised;
                double mag = magnitude;
                x = x/mag;
                y = y/mag;
            }

        #endregion
        
        #region ----------------------------- Vector Interaction -----------------------------

           // Angle between 2 vectors. AxB = |A|.|B|.sin(theta)
            public static double Angle(Vector2 a, Vector2 b) => Maths.ArcSin(Vector3.DotProduct(a,b).magnitude / 
                                                                            (a.magnitude * b.magnitude));

           // Normal to the surface created by two vectors
            public static Vector3 Normal(Vector2 a, Vector2 b) => CrossProduct(a,b);

           // Distance between two vectors
            public static double Distance(Vector2 a, Vector2 b) => (a-b).magnitude;

        #endregion

        #region ----------------------------------- Parity -----------------------------------

            public static implicit operator Vector2(Vector3 a) => new Vector2(a.x,a.y);

        #endregion

    }

}
