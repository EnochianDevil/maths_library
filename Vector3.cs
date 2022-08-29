namespace MathsLib {

    public struct Vector3 {
        
        #region --------------------------------- Properties ---------------------------------

            public double x;
            public double y;
            public double z;

        #endregion
        
        #region ------------------------------- Notable Vectors ------------------------------

           // Zero and one vector
            public static Vector3 zero  {get{ return new Vector3(  0 ,  0 ,  0 );}}
            public static Vector3 ones  {get{ return new Vector3(  1 ,  1 ,  1 );}}
         
           // Directional unit Vectors
            public static Vector3 i     {get{ return new Vector3(  1 ,  0 ,  0 );}}
            public static Vector3 j     {get{ return new Vector3(  0 ,  1 ,  0 );}}
            public static Vector3 k     {get{ return new Vector3(  0 ,  0 ,  1 );}}
            public static Vector3 left  {get{ return new Vector3(  1 ,  0 ,  0 );}}
            public static Vector3 right {get{ return new Vector3( -1 ,  0 ,  0 );}}
            public static Vector3 up    {get{ return new Vector3(  0 ,  1 ,  0 );}}
            public static Vector3 down  {get{ return new Vector3(  0 , -1 ,  0 );}}
            public static Vector3 front {get{ return new Vector3(  0 ,  0 ,  1 );}}
            public static Vector3 back  {get{ return new Vector3(  0 ,  0 , -1 );}}

        #endregion

        #region --------------------------------- Constructor --------------------------------

            public Vector3(double x, double y, double z) {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            public override string ToString() => "(" + x + "," + y + "," + z + ")";

        #endregion

        #region ------------------------------ Basic Operations ------------------------------

           // Sum
            public static Vector3 Sum(Vector3 a, params Vector3[] b){
                Vector3 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x += b[i].x;
                    total.y += b[i].y;
                    total.z += b[i].z;
                }
                return total;
            }
            public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
            public static Vector3 operator +(Vector3 a, double b)   => new Vector3(a.x + b,   a.y + b,   a.z + b);
            public static Vector3 operator +(double b, Vector3 a)   => new Vector3(a.x + b,   a.y + b,   a.z + b);

           // Subtraction
            public static Vector3 Subtract(Vector3 a, params Vector3[] b){
                Vector3 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x -= b[i].x;
                    total.y -= b[i].y;
                    total.z -= b[i].z;
                }
                return total;
            }
            public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
            public static Vector3 operator -(Vector3 a, double b)   => new Vector3(a.x - b,   a.y - b,   a.z - b);
            public static Vector3 operator -(double b, Vector3 a)   => new Vector3(a.x - b,   a.y - b,   a.z - b);
            public static Vector3 operator -(Vector3 a) => new Vector3(-a.x,-a.y,-a.z);

           // Multiplication
            public static Vector3 CrossProduct(Vector3 a, params Vector3[] b){ 
                Vector3 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total = Maths.Determinant(total.y, total.z, b[i].y, b[i].z) * Vector3.i - 
                            Maths.Determinant(total.x, total.z, b[i].x, b[i].z) * Vector3.j +
                            Maths.Determinant(total.x, total.y, b[i].x, b[i].y) * Vector3.k;
                }
                return total;
            }
            public static Vector3 DotProduct(Vector3 a, params Vector3[] b){
                Vector3 total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.x *= b[i].x;
                    total.y *= b[i].y;
                    total.z *= b[i].z;
                }
                return total;
            }
            public static Vector3 operator *(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
            public static Vector3 operator *(Vector3 a, double b)   => new Vector3(a.x * b,   a.y * b,   a.z * b);
            public static Vector3 operator *(double b, Vector3 a)   => new Vector3(a.x * b,   a.y * b,   a.z * b);

           // Division 
            public static Vector3 operator /(Vector3 a, double b) => new Vector3(a.x / b, a.y / b, a.z / b);

           // Equality
            public override bool Equals(object obj){
                if (obj.GetType()!=typeof(Vector3)) return false;
                return (this == (Vector3) obj);
            }
            public override int GetHashCode() => base.GetHashCode();
            public static bool operator ==(Vector3 a, Vector3 b) => (a.x==b.x && a.y==b.y && a.z==b.z);
            public static bool operator !=(Vector3 a, Vector3 b) => !(a.x==b.x && a.y==b.y && a.z==b.z) ;

        #endregion

        #region ------------------------------ Vector Properties -----------------------------

           // Magnitude
            public double magnitude {get{ return Maths.Sqrt(x*x + y*y + z*z);}}

           // Normalisation
            public Vector2 Normalize() => this/magnitude;
            public void NormalizeVector(){
               // Changes the vector to be normalised;
                double mag = magnitude;
                x = x/mag;
                y = y/mag;
                z = z/mag;
            }

        #endregion

        #region ----------------------------- Vector Interaction -----------------------------

           // Angle between 2 vectors. AxB = |A|.|B|.sin(theta)
            public static double Angle(Vector3 a, Vector3 b) => Maths.ArcSin(Vector3.DotProduct(a,b).magnitude / 
                                                                            (a.magnitude * b.magnitude));

           // Normal to the surface created by two vectors
            public static Vector3 Normal(Vector3 a, Vector3 b) => CrossProduct(a,b);

           // Distance between two vectors
            public static double Distance(Vector3 a, Vector3 b) => (a-b).magnitude;

        #endregion

        #region ----------------------------------- Parity -----------------------------------

            public static implicit operator Vector3(Vector2 a) => new Vector3(a.x,a.y,0);

        #endregion
    
    }

}
