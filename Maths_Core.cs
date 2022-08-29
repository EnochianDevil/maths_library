using System;


namespace MathsLib {

    public static partial class Maths {
        
        // Sources:
        //      http://www.milefoot.com/math/complex/sqrootsimag.htm;
        //      http://www.milefoot.com/math/complex/exponentofi.htm;
        //      http://www.milefoot.com/math/complex/functionsofi.htm;
        //      http://scipp.ucsc.edu/~haber/archives/physics116A10/arc_10.pdf
        //      https://math.stackexchange.com/questions/3381629/what-is-the-fastest-algorithm-for-finding-the-natural-logarithm-of-a-big-number;
        //      https://math.stackexchange.com/questions/462443/calculating-non-integer-exponent
              
        #region ------------------------------ Usefull Constants -----------------------------

            public static double pi   {get{ return 3.141592653589793115997963468544f;}}
            public static double tau  {get{ return 6.283185307179586476925286766559f;}}
            public static double e    {get{ return 2.718281828459045235360287471352f;}}
            public static double phi  {get{ return 1.618033988749894848204586834365f;}}
            public static double ln2  {get{ return 0.693147180559945309417232121458f;}}
            public static double ln10 {get{ return 2.302585092994045684017991454684f;}}

            public static double[] primes {
                get{
                    return new double[168] {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101,	103, 107, 109, 113, 127, 131, 137, 139, 149, 151,
                        157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349,
                        353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569,
                        571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787,
                        797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997
                    };
                }
            }
            public static double[] fibonacci {
                get {
                    return new double[17] {0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987};
                }
            }

        #endregion

        #region ------------------------------ Simple Functions ------------------------------

           // Absolute value
            public static double Abs(double value) => (value >= 0 )? value : -value;

           // Sign of value
            public static double SignOf(double value) => (value >= 0 )? 1 : -1;

           // Max and min 
            public static double Max (params double[] b) {
                if (b.Length == 0) throw new ArgumentNullException();
                double max = 0;
                foreach (double _b in b) {
                    max = (max > _b)? max : _b;       
                }
                return max;
            }
            public static double Min (params double[] b) {
                if (b.Length == 0) throw new ArgumentNullException();
                double max = 9999999999999999999;
                foreach (double _b in b) {
                    max = (max < _b)? max : _b;       
                }
                return max;
            }

           // Check if value is integer
            public static bool IsInt(double value) => (value - ((long)value)) <= 0.00000001;

           // Check if value is divisible by another
            public static bool IsDivisibleBy(double a, double b) => (a%b == 0);
       
           // Rounding functions
            public static complex Round(complex value) => Round(value,0);
            public static complex Round(complex value, int n){
                double pow = Power((double)10,n);
                value.real = ((long) (value.real * pow + ((SignOf(value.real)==1)? 0.5:-0.5))) / pow;
                value.imaginary = ((long) (value.imaginary * pow + ((SignOf(value.imaginary)==1)? 0.5:-0.5))) / pow;
                return value;
            }
            
            public static complex Floor(complex value) => Floor(value,0);
            public static complex Floor(complex value, int n) {
                double pow  = Power((double)10,n);
                value.real = ((long) (value.real * pow)) / pow;
                value.imaginary = ((long) (value.imaginary * pow)) / pow;
                return value;
            }   

            public static complex Ceil(complex value) => Ceil(value,0);
            public static complex Ceil(complex value, int n) {
                double pow = Power((double)10,n);
                value.real = ((long) (value.real * pow + ((value.real==0 || IsInt(value.real))? 0:SignOf(value.real)))) / pow;
                value.imaginary = ((long) (value.imaginary * pow + ((value.imaginary==0 || IsInt(value.imaginary))? 0:SignOf(value.imaginary)))) / pow;
                return value;
            }

           // Fraction conversion
            public static double[] Decimal2Fraction(double value) {
                double[] frac = Decimal2Fraction_10(value);
                return SimplifyFraction((int)frac[0], (int)frac[1]);
            }
            public static double[] Decimal2Fraction_10(double value) {
                double[] frac = new double[2]; 
                frac[0] = value;
                frac[1] = 1;
                while (!IsInt(frac[0])) {
                    frac[0] *= 10;
                    frac[1] *= 10;
                }
                return frac;
            }
            public static double[] SimplifyFraction(int numerator, int denominator) {
                foreach (int prime in primes) {
                    if (prime > Min(numerator, denominator)/2 || numerator == 1 || denominator == 1) break;
                    while (numerator%prime == 0 && denominator%prime==0) {
                        numerator /= prime;
                        denominator /= prime;
                    }
                }
                return new double[2]{numerator,denominator};
            }

        #endregion
        
        #region ----------------------------------- Powers -----------------------------------

            public static complex Power(complex value, complex exp) {

               // Return obvious results
                if (exp == 0) return 1;
                if (exp == 1) return value;

               // Handle real exponents
                if (exp.imaginary == 0) {
                    complex pow;
                    if (IsInt(exp)){
                        if (exp > 0) {
                            pow = value;
                            long n = (long)exp - 1;
                            while (n > 0) {
                                pow *= value;
                                n--;
                            }
                        } else {
                            pow = 1;
                            long n = (long)Abs(exp);
                            while (n > 0) {
                                pow /= value;
                                n--;
                            }
                        } 
                        return pow;
                    }

                   // Handle non-integer cases by reducing problem to root
                    return Exp(exp*Ln(value));

                }

               // Handle imaginary values and exponents
                complex z_1 = Power(value, exp.real);
                complex z_2 = Cos(exp.imaginary * Ln(value));
                complex z_3 = Sin(exp.imaginary * Ln(value)) * complex.i;
                return  z_1 * (z_2 + z_3);
            }

           // Exponential function
            public static complex Exp(complex value) {
                if (value.imaginary == 0) return Exp(value.real);
                return Exp(value.real) * (Cos(value.imaginary) + complex.i*Sin(value.imaginary));
            }
            static double Exp(double value) {
               // Return obvious values
                if (Abs(value - 0)   <= 0.00001) return 1;
                if (Abs(value - ln2) <= 0.00001) return 2;
                if (Abs(value - 1)   <= 0.00001) return e;
                    
               // Reduction such that Ln(x) = Ln(a.2^n) = Ln(a) + n*Ln(2)
                double k = Floor(value/ln2);
                double a = value - k*ln2;
                double e_a = 1 + a + a*a/2 + a*a*a/6 + a*a*a*a/24 + a*a*a*a*a/120 + a*a*a*a*a*a/720 + a*a*a*a*a*a*a/5040;
                return e_a * Power(2,k);
            }

        #endregion

        #region ----------------------------------- Roots ------------------------------------

           // Square root
            public static complex Sqrt(complex value) {
               // Compute roots of real value
                if (value.imaginary == 0) {
                    if (value.real < 0) return new complex(0,Sqrt(-value.real));
                    return RootNewton(value,2,1);
                }

               // Compute roots of complex value. Only produces positive root, since complex numbers have 2 roots
                double r = Round(Sqrt((value.magnitude + value.real)/2),7);
                double i = Round(Sqrt((value.magnitude - value.real)/2) * value.imaginary/Abs(value.imaginary),7);
                return new complex(r,i);
            }
            
           // Other roots
            public static complex Root(complex value, complex exp){
                if (IsInt(exp.real) && exp.imaginary==0 && value.imaginary==0){
                    if (value.real < 0) return Power(complex.i,Abs((int)exp)-1) * RootNewton(-value.real, (int)exp.real, -value/exp.real);
                    return RootNewton(value.real, (int)exp.real, value/exp.real);
                }
                return Power(value, 1/exp);
            }
            static double RootNewton(double value, int exp,  double estimate)   {
               // If estimate^exp ~= value, return
                double estimate_value = estimate;
                for (int i = 1; i < exp; i++) {
                    estimate_value *= estimate;
                }
                if (Abs(estimate_value - value) <= 0.0001) return Round(estimate,7);

               // Calculate new estimate as: Estimate_i+1 = estimate_i - f(estimate_i)/f'(estimate_i), f(x) = x^2 - value (error)
                return RootNewton(value, exp, ((value/(estimate_value/estimate)) + (exp-1)*estimate) / exp);
            }


        #endregion

        #region --------------------------------- Logarithms ---------------------------------

           // Natural logarith
            public static complex Ln(complex value) => Ln(value.magnitude) + complex.i * value.arg;
            static complex Ln(double value) {
               // Return obvious values
                if (Abs(value - 10) <= 0.00001) return ln10;
                if (Abs(value - e)  <= 0.00001) return 1;
                if (Abs(value - 2)  <= 0.00001) return ln2;
                if (Abs(value - 1)  <= 0.00001) return 0;
                if (Abs(value - 0)  <= 0.00001) return double.NaN;

               // Reduction such that Ln(x) = Ln(a.2^n) = Ln(a) + n*Ln(2)
                int n = 0;
                while (value > 1.33f) {
                    n += 1;
                    value /= 2;
                }

               // Calculate Ln(a) and add n*Ln(2)
                double a  = (value-1)/(value+1);
                double a2 = a*a;
                double a4 = a2*a2;
                double a8 = a4*a4;
                double ln_a = a + a*a2/3 + a*a4/5 + a*a4*a2/7 + a*a8/9 + a*a8*a2/11 + a*a8*a4/13;
                return ln_a*2 + n*ln2;
            }
        
           // Logariths in base 10
            public static complex Log10(complex value) => Ln(value)/ln10;
            
           // Logariths in base 2
            public static complex Log2(complex value)  => Ln(value)/ln2;
           
           // Logariths in other bases
            public static complex Log(complex value, complex b){
                if (b==1) return double.NaN;
                return Ln(value)/Ln(b);
            }

        #endregion

    }

}
