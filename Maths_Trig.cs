using System;

namespace MathsLib {

    public static partial class Maths {

        // Sources:
        //      https://allenchou.net/2014/02/game-math-faster-sine-cosine-with-polynomial-curves/;
        //      http://www.milefoot.com/math/complex/functionsofi.htm;
        //      http://www.people.vcu.edu/~rhammack/reprints/cmj210-217.pdf;
        //      http://scipp.ucsc.edu/~haber/archives/physics116A10/arc_10.pdf;
        //      https://proofwiki.org/wiki/Arctangent_of_Imaginary_Number;
        //      https://www.researchgate.net/publication/259385247_Fast_computation_of_arctangent_functions_for_embedded_applications_A_comparative_analysis;
        //      https://jfdube.wordpress.com/2011/12/06/trigonometric-look-up-tables-revisited/;
        //      https://ieeexplore.ieee.org/document/1628884;
        
        #region ------------------------------- Look-Up Tables -------------------------------

            static double[] X {
                get{
                    return new double[101] {
                        0f, 0.062831853f, 0.125663706f, 0.188495559f, 0.251327412f, 0.314159265f, 0.376991118f, 0.439822972f, 0.502654825f, 0.565486678f, 0.628318531f, 0.691150384f, 0.753982237f,
                        0.81681409f, 0.879645943f,0.942477796f, 1.005309649f, 1.068141502f, 1.130973355f, 1.193805208f, 1.256637061f, 1.319468915f, 1.382300768f, 1.445132621f, 1.507964474f,
                        1.570796327f, 1.63362818f, 1.696460033f, 1.759291886f, 1.822123739f, 1.884955592f, 1.947787445f, 2.010619298f, 2.073451151f, 2.136283004f, 2.199114858f, 2.261946711f,
                        2.324778564f, 2.387610417f, 2.45044227f, 2.513274123f, 2.576105976f, 2.638937829f, 2.701769682f, 2.764601535f, 2.827433388f, 2.890265241f, 2.953097094f, 3.015928947f,
                        3.078760801f, 3.141592654f, 3.204424507f, 3.26725636f, 3.330088213f, 3.392920066f, 3.455751919f, 3.518583772f, 3.581415625f, 3.644247478f, 3.707079331f, 3.769911184f,
                        3.832743037f, 3.89557489f, 3.958406744f, 4.021238597f, 4.08407045f, 4.146902303f, 4.209734156f, 4.272566009f, 4.335397862f, 4.398229715f, 4.461061568f, 4.523893421f,
                        4.586725274f, 4.649557127f, 4.71238898f, 4.775220833f, 4.838052687f, 4.90088454f, 4.963716393f, 5.026548246f, 5.089380099f, 5.152211952f, 5.215043805f, 5.277875658f,
                        5.340707511f, 5.403539364f, 5.466371217f, 5.52920307f, 5.592034923f, 5.654866776f, 5.71769863f, 5.780530483f, 5.843362336f, 5.906194189f, 5.969026042f, 6.031857895f,
                        6.094689748f, 6.157521601f, 6.220353454f, 6.283185307f
                    };
                }
            }

            static double[] X_2 {
                get{
                    return new double[101] {
                        -pi/2, -1.5393804f, -1.507964474f, -1.476548547f, -1.445132621f, -1.413716694f, -1.382300768f, -1.350884841f, -1.319468915f, -1.288052988f, -1.256637061f, -1.225221135f,
                        -1.193805208f, -1.162389282f, -1.130973355f, -1.099557429f, -1.068141502f, -1.036725576f, -1.005309649f, -0.973893723f, -0.942477796f, -0.91106187f, -0.879645943f,
                        -0.848230016f, -0.81681409f, -0.785398163f, -0.753982237f, -0.72256631f, -0.691150384f, -0.659734457f, -0.628318531f, -0.596902604f, -0.565486678f, -0.534070751f,
                        -0.502654825f, -0.471238898f, -0.439822972f, -0.408407045f, -0.376991118f, -0.345575192f, -0.314159265f, -0.282743339f, -0.251327412f, -0.219911486f, -0.188495559f,
                        -0.157079633f, -0.125663706f, -0.09424778f, -0.062831853f, -0.031415927f, 0f, 0.031415927f, 0.062831853f, 0.09424778f, 0.125663706f, 0.157079633f, 0.188495559f,
                        0.219911486f, 0.251327412f, 0.282743339f, 0.314159265f, 0.345575192f, 0.376991118f, 0.408407045f, 0.439822972f, 0.471238898f, 0.502654825f, 0.534070751f, 0.565486678f,
                        0.596902604f, 0.628318531f, 0.659734457f, 0.691150384f, 0.72256631f, 0.753982237f, 0.785398163f, 0.81681409f, 0.848230016f, 0.879645943f, 0.91106187f, 0.942477796f,
                        0.973893723f, 1.005309649f, 1.036725576f, 1.068141502f, 1.099557429f, 1.130973355f, 1.162389282f, 1.193805208f, 1.225221135f, 1.256637061f, 1.288052988f, 1.319468915f,
                        1.350884841f, 1.382300768f, 1.413716694f, 1.445132621f, 1.476548547f, 1.507964474f, 1.5393804f, pi/2
                    };
                }
            }
        
            static double[] SIN_X {
                get{
                    return new double[101] {
                         0f, 0.06279052f, 0.125333234f, 0.187381315f, 0.248689887f, 0.309016994f, 0.368124553f, 0.425779292f, 0.481753674f, 0.535826795f, 0.587785252f, 0.63742399f, 0.684547106f,
                         0.728968627f, 0.770513243f, 0.809016994f, 0.844327926f, 0.87630668f, 0.904827052f, 0.929776486f, 0.951056516f, 0.968583161f, 0.982287251f, 0.992114701f, 0.998026728f,
                         1f, 0.998026728f, 0.992114701f, 0.982287251f, 0.968583161f, 0.951056516f, 0.929776486f, 0.904827052f, 0.87630668f, 0.844327926f, 0.809016994f, 0.770513243f, 0.728968627f,
                         0.684547106f, 0.63742399f, 0.587785252f, 0.535826795f, 0.481753674f, 0.425779292f, 0.368124553f, 0.309016994f, 0.248689887f, 0.187381315f, 0.125333234f, 0.06279052f, 0f,
                         -0.06279052f, -0.125333234f, -0.187381315f, -0.248689887f, -0.309016994f, -0.368124553f, -0.425779292f, -0.481753674f, -0.535826795f, -0.587785252f, -0.63742399f,
                         -0.684547106f, -0.728968627f, -0.770513243f, -0.809016994f, -0.844327926f, -0.87630668f, -0.904827052f, -0.929776486f, -0.951056516f, -0.968583161f, -0.982287251f,
                         -0.992114701f, -0.998026728f, -1f, -0.998026728f, -0.992114701f, -0.982287251f, -0.968583161f, -0.951056516f, -0.929776486f, -0.904827052f, -0.87630668f, -0.844327926f,
                         -0.809016994f, -0.770513243f, -0.728968627f, -0.684547106f, -0.63742399f, -0.587785252f, -0.535826795f, -0.481753674f, -0.425779292f, -0.368124553f, -0.309016994f,
                         -0.248689887f, -0.187381315f, -0.125333234f, -0.06279052f, 0f
                    };
                }
            }
        
            static double[] TAN_X_2 {
                get{
                    return new double[101] {
                         -1E+16f, -31.82051595f, -15.89454484f, -10.57889499f, -7.915815088f, -6.313751515f, -5.242183581f, -4.473742829f, -3.894742855f, -3.442022577f, -3.077683537f,
                         -2.777606854f, -2.525711689f, -2.310863654f, -2.125108173f, -1.962610506f, -1.818993247f, -1.690907656f, -1.57574786f, -1.471455316f, -1.37638192f, -1.289192232f,
                         -1.20879235f, -1.134277349f, -1.06489184f, -1f, -0.939062506f, -0.881618592f, -0.827271946f, -0.775679511f, -0.726542528f, -0.679599298f, -0.634619298f, -0.591398351f,
                         -0.549754652f, -0.509525449f, -0.470564281f, -0.432738642f, -0.395928009f, -0.360022153f, -0.324919696f, -0.290526857f, -0.25675636f, -0.223526483f, -0.190760202f,
                         -0.15838444f, -0.126329378f, -0.094527831f, -0.062914667f, -0.031426266f, 0f, 0.031426266f, 0.062914667f, 0.094527831f, 0.126329378f, 0.15838444f, 0.190760202f,
                         0.223526483f, 0.25675636f, 0.290526857f, 0.324919696f, 0.360022153f, 0.395928009f, 0.432738642f, 0.470564281f, 0.509525449f, 0.549754652f, 0.591398351f, 0.634619298f,
                         0.679599298f, 0.726542528f, 0.775679511f, 0.827271946f, 0.881618592f, 0.939062506f, 1f, 1.06489184f, 1.134277349f, 1.20879235f, 1.289192232f, 1.37638192f, 1.471455316f,
                         1.57574786f, 1.690907656f, 1.818993247f, 1.962610506f, 2.125108173f, 2.310863654f, 2.525711689f, 2.777606854f, 3.077683537f, 3.442022577f, 3.894742855f, 4.473742829f,
                         5.242183581f, 6.313751515f, 7.915815088f, 10.57889499f, 15.89454484f, 31.82051595f, 1E+16f
                    };
                }
            }

        #endregion

        #region ----------------------------------- Units ------------------------------------

           // Degrees and Radians conversion
            public static double DegreesToRadian(double alpha) => alpha * pi/180;
            public static double RadianToDegrees(double alpha) => alpha * 180/pi;

           // Reduce angle to [0,2pi]
            public static double ReduceRadians_2pi(double alpha){
                alpha -= Round(alpha/tau)*tau;
                if (alpha < 0) alpha += tau;
                return alpha;
            }

           // Reduce angle to [-pi/2,pi/2]
            public static double ReduceRadians_pihalf(double alpha){
                if (alpha > 0) {
                    while (alpha > pi / 2) alpha -= pi;
                } else {
                    while (alpha < -pi/2) alpha += pi;
                }
                return alpha;
            }

           // Check for infinity values
            static double CheckInf(double value) => (value >= 10000)? double.PositiveInfinity : ((value <= -10000)? double.NegativeInfinity : value);

        #endregion

        #region --------------------------- Trignometric Functions ---------------------------

           // Sine, Cosine and Tangent
            public static complex Sin(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Sin(alpha);
                if (units == "deg" || units == "degrees") return Sin(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Sine function)");
            }
            public static complex Sin(complex alpha) {
               // If alpha is real, return normaly. Otherwise, return trignometric identities
                if (alpha.imaginary == 0) return Sin(alpha.real);
                double r = Sin(alpha.real) * Cosh(alpha.imaginary);
                double i = Cos(alpha.real) * Sinh(alpha.imaginary);
                return new complex(r,i);
            }
            static double Sin(double alpha) {
               // Reduce radians in order to improve performance
                if (alpha > tau || alpha < 0) alpha = ReduceRadians_2pi(alpha);

               // Calculate Sin
                double x = Array.Find(X, val => val >= alpha);
                int idx  = Array.IndexOf(X,x);
                if (x - alpha <= 0.0001) return SIN_X[idx];

                return SIN_X[idx-1] + (alpha - X[idx-1]) * (SIN_X[idx]-SIN_X[idx-1]) / (X[idx]-X[idx-1]);
            }

            public static complex Cos(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Cos(alpha);
                if (units == "deg" || units == "degrees") return Cos(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Cosine function)");
            }
            public static complex Cos(complex alpha) {
               // If alpha is real, return normaly. Otherwise, return trignometric identities
                if (alpha.imaginary == 0) return Cos(alpha.real);
                double r = Cos(alpha.real) * Cosh(alpha.imaginary);
                double i = Sin(alpha.real) * Sinh(alpha.imaginary);
                return new complex(r,-i);
            }
            static double Cos(double alpha) => Sin(pi/2-alpha);

            public static complex Tan(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Tan(alpha);
                if (units == "deg" || units == "degrees") return Tan(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Tangent function)");
            }
            public static complex Tan(complex alpha) {
               // If alpha is real, return normaly. Otherwise, return trignometric identities
                if (alpha.imaginary == 0) return Tan(alpha.real);

                double a = Tan(alpha.real);
                double b = Tanh(alpha.imaginary);
                double c = 1 + a*a*b*b;

                double r = (a - a*b*b) / c;
                double i = (b + a*a*b) / c;
                return new complex(r,i);
            }
            static double Tan(double alpha){
               // Reduce radians in order to improve performance
                if (Abs(alpha) > pi/2) alpha = ReduceRadians_pihalf(alpha);

               // Calculate Tan
                double x = Array.Find(X_2, val => val >= alpha);
                int idx  = Array.IndexOf(X_2,x);
                if (X_2[idx] - alpha <= 0.0001) return CheckInf(TAN_X_2[idx]);

                return CheckInf(TAN_X_2[idx-1] + (alpha - X_2[idx-1]) * (TAN_X_2[idx]-TAN_X_2[idx-1]) / (X_2[idx]-X_2[idx-1]));
            }


           // Hyperbolic Sine, Cosine and Tangent
            public static complex Sinh(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Sinh(alpha);
                if (units == "deg" || units == "degrees") return Sinh(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic Sine function)");
            }
            public static complex Sinh(complex alpha) {
               // If alpha is real, return normaly. Otherwise, return trignometric identities
                if (alpha.imaginary == 0) return Sinh(alpha.real);
                double r = Sinh(alpha.real) * Cos(alpha.imaginary);
                double i = Cosh(alpha.real) * Sin(alpha.imaginary);
                return new complex(r,i);
            }
            static double Sinh(double alpha) => (Exp(alpha) - Exp(-alpha))/2;

            public static complex Cosh(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Cosh(alpha);
                if (units == "deg" || units == "degrees") return Cosh(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic Cosine function)");
            }
            public static complex Cosh(complex alpha) {
               // If alpha is real, return normaly. Otherwise, return trignometric identities
                if (alpha.imaginary == 0) return Cosh(alpha.real);
                double r = Cosh(alpha.real) * Cos(alpha.imaginary);
                double i = Sinh(alpha.real) * Sin(alpha.imaginary);
                return new complex(r,i);
            }
            static double Cosh(double alpha) => (Exp(alpha) + Exp(-alpha))/2;

            public static complex Tanh(complex alpha, string units) {
                if (units == "rad" || units == "radians") return Tanh(alpha);
                if (units == "deg" || units == "degrees") return Tanh(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic tangent function)");
            }
            public static complex Tanh(complex alpha) => Sinh(alpha)/Cosh(alpha);


           // Secant, Cosecant and Cotangent
            public static complex Sec(complex alpha, string units){
                if (units == "rad" || units == "radians") return Sec(alpha);
                if (units == "deg" || units == "degrees") return Sec(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Secant function)");
            }
            public static complex Sec(complex alpha) => 1/Cos(alpha);

            public static complex Csc(complex alpha, string units){
                if (units == "rad" || units == "radians") return Csc(alpha);
                if (units == "deg" || units == "degrees") return Csc(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Cosecant function)");
            }
            public static complex Csc(complex alpha) => 1/Sin(alpha);

            public static complex Cot(complex alpha, string units){
                if (units == "rad" || units == "radians") return Cot(alpha);
                if (units == "deg" || units == "degrees") return Cot(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Cotangent function)");
            }
            public static complex Cot(complex alpha) => 1/Tan(alpha);
     
        
           // Hyperbolic Secant, Cosecant and Cotangent
            public static complex Sech(complex alpha, string units){
                if (units == "rad" || units == "radians") return Sech(alpha);
                if (units == "deg" || units == "degrees") return Sech(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic secant function)");
            }
            public static complex Sech(complex alpha) => 1/Cosh(alpha);

            public static complex Csch(complex alpha, string units){
                if (units == "rad" || units == "radians") return Csch(alpha);
                if (units == "deg" || units == "degrees") return Csch(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic cosecant function)");
            }
            public static complex Csch(complex alpha) => 1/Sinh(alpha);

            public static complex Coth(complex alpha, string units){
                if (units == "rad" || units == "radians") return Coth(alpha);
                if (units == "deg" || units == "degrees") return Coth(DegreesToRadian(alpha));

                throw new ArgumentException(units + " is not a valid unit for this operation (Hyperbolic cotangent function)");
            }
            public static complex Coth(complex alpha) => Cosh(alpha)/Sinh(alpha);


           // Arcsine, Arccosine, Arctangent and Arctangent of 2 numbers   => improve in future
            public static complex ArcSin(complex value) => -complex.i * Ln(complex.i * value + Sqrt(1-value*value));

            public static complex ArcCos(complex value) => pi/2 - ArcSin(value);

            public static complex ArcTan(complex value){
               // Handle real input and cases i, -i
                if (value.imaginary == 0) return ArcTan((double)value.real);
                if (value == complex.i || value == -complex.i) return double.NaN;

                return complex.i/2 * (Ln(1-complex.i*value) - Ln(1+complex.i*value));  
            }
            static double ArcTan(double value){
                if (value == 0) return 0;
                if (value <  0) return -ArcTan(-value);
                if (value >  1) return pi/2 - ArcTan(1/value);
                return pi/4*value + 0.273 * (1 - value);
            }


           // Hyperbolic Arcsine, Arccosine and Arctangent
            public static complex ArcSinh(complex value) => Ln(value + Sqrt(value*value + 1));

            public static complex ArcCosh(complex value) => Ln(value + Sqrt(value*value - 1));

            public static complex ArcTanh(complex value){
               // Handle obvious values
                if(value == 0)      return 0;
                if(value == 1)      return double.PositiveInfinity;
                if(value == -1)     return double.NegativeInfinity;
                if(value.imaginary == 0 && value.real >= 100000) return -pi/2 * complex.i;
                return Ln((1+value)/(1-value))/2;
            }


           // Arcsecant, Arccosecant and Arccotangent -> Low accuracy due to error propagation. Possibly improve
            public static complex ArcSec(complex value) => ArcCos(1/value);

            public static complex ArcCsc(complex value) => ArcSin(1/value);

            public static complex ArcCot(complex value) => pi/2 - ArcTan(value);
        

           // Hyperbolic Arcsecant, Arcosecant and Arcotangent -> Low accuracy due to error propagation. Possibly improve
            public static complex ArcSech(complex value) => Ln((1+Sqrt(1-value*value))/value);

            public static complex ArcCsch(complex value) => Ln(1/value + Sqrt(1/(value*value)+1));

            public static complex ArcCoth(complex value) => Ln((value+1)/(value-1))/2;

        #endregion

    }

}
