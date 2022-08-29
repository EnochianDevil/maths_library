using System;

namespace MathsLib {

    public static partial class Maths {
        
        #region ----------------------------- Property Operations ----------------------------

           // Determinant
            public static complex Determinant(complex a, complex b, complex c, complex d){ 
               // det | a b | = a*d - b*c
               //     | c d | 
                return a*d - b*c;
            }

        #endregion
        
        #region ------------------------------ Matrix Operations -----------------------------

           // Vectorise matrix
            public static matrix Vectorize(matrix m) {
                matrix result = matrix.zero(m.size,1);
                for (int j = 0; j < m.columns; j++) {
                    for (int i = 0; i < m.rows; i++) {
                        result.data[i*m.columns+j,1] = m.data[i,j];
                    }
                }
                return result;
            }

        #endregion

    }

}
