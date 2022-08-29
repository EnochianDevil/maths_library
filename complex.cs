using System;
using System.Collections.Generic;
using System.Text;

namespace MathsLib {

    public struct complex {
        
        #region --------------------------------- Properties ---------------------------------
            public double real;
            public double imaginary;
        #endregion
        
        #region ------------------------------- Notable Complex ------------------------------

           // 0 + 1*i
            public static complex i {get{ return new (0,1);}}

        #endregion

        #region --------------------------------- Constructor --------------------------------

            public complex(double real, double imaginary) {
                this.real      = real;
                this.imaginary = imaginary;
            }
            public override string ToString(){
                if (imaginary==0) return real.ToString();
                if (real==0) return imaginary + "i";
                if (imaginary<0) return real + "-" + -imaginary + "i";
                return real + "+" + imaginary + "i";
            }
            
        #endregion

        #region ------------------------------ Basic Operations ------------------------------
           // Sum
            public static complex Sum(complex a, params complex[] b){
                complex total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.real      += b[i].real;
                    total.imaginary += b[i].imaginary;
                }
                return total;
            }
            public static complex operator +(complex a, complex b) => new(a.real + b.real, a.imaginary + b.imaginary);

           // Subtraction
            public static complex Subtract(complex a, params complex[] b){
                complex total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.real      -= b[i].real;
                    total.imaginary -= b[i].imaginary;
                }
                return total;
            }
            public static complex operator -(complex a, complex b) => new(a.real - b.real, a.imaginary - b.imaginary);
            public static complex operator -(complex a) => new(-a.real, -a.imaginary);
        
           // Multiplication
            public static complex Multiply(complex a, params complex[] b){
                complex total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.real      = a.real * b[i].real - a.imaginary * b[i].imaginary;
                    total.imaginary = a.real * b[i].imaginary + a.imaginary * b[i].real;
                }
                return total;
            }
            public static complex operator *(complex a, complex b) => new(a.real * b.real - a.imaginary * b.imaginary,
                                                                          a.real * b.imaginary + a.imaginary * b.real);
                           
           // Division
            public static complex Divide(complex a, params complex[] b){
                complex total = a;
                for (int i = 0; i < b.Length; i++) {
                    total.real      = (total.real * b[i].real - total.imaginary * b[i].imaginary) / 
                                      (b[i].real  * b[i].real - b[i].imaginary  * b[i].imaginary);
                    total.imaginary = (total.real * b[i].imaginary + total.imaginary * b[i].real) / 
                                      (b[i].real  * b[i].real - b[i].imaginary  * b[i].imaginary);
                }
                return total;
            }
            public static complex operator /(complex a, complex b) => new ((a.real * b.real + a.imaginary * b.imaginary) / 
                                                                           (b.real * b.real + b.imaginary * b.imaginary), 
                                                                           (a.imaginary * b.real - a.real * b.imaginary) / 
                                                                           (b.real * b.real + b.imaginary * b.imaginary));
                            
            // Equality
            public override bool Equals(object obj){
                if (obj.GetType()!=typeof(complex)) return false;
                return (this == (complex) obj);
            }
            public override int GetHashCode() => base.GetHashCode();
            public static bool operator ==(complex a, complex b) =>  (a.real==b.real && a.imaginary==b.imaginary);
            public static bool operator !=(complex a, complex b) => !(a.real==b.real && a.imaginary==b.imaginary);
        #endregion
        
        #region ----------------------------- Complex Properties -----------------------------

           // Magnitude (Distance from origin to number)
            public double magnitude {get{return Maths.Round(Maths.Sqrt(real*real + imaginary*imaginary),6);}}

           // Argument (Angle between vector formed from origin to complex number and Vector2.i)
            public double arg {
                get{
                    
                    if (real == 0) {
                        if(imaginary > 0) return Maths.pi/2;
                        if(imaginary < 0) return -Maths.pi/2;
                        return double.NaN;
                    } else {
                        if(real > 0) return Maths.ArcTan(imaginary/real);
                        if(imaginary == 0) return Maths.pi;  
                        if(imaginary >  0) return Maths.ArcTan(imaginary/real) + Maths.pi;  
                        return Maths.ArcTan(imaginary/real) - Maths.pi;
                    }
                }
            }

           // Euler representation of Complex number euler[0] * e^(i*euler[1]);
            public double[] euler {get{return new double[2]{magnitude,arg};}}

        #endregion

        #region ----------------------------------- Parity -----------------------------------

            public static implicit operator complex(Vector2 a) => new (a.x, a.y);
            public static implicit operator complex(double a)  => new (a,0);
            public static implicit operator double(complex a) {
                if (a.imaginary == 0) return a.real;
                throw new ArgumentException("Values with imaginary component cannot be implicity interpreted as a real number");
            }

        #endregion

    }

}
