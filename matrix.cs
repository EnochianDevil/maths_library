using System;
using System.Collections.Generic;

namespace MathsLib {

    public struct matrix {
        
        #region --------------------------------- Properties ---------------------------------
        
            public int rows {get;}
            public int columns {get;}
            public complex[,] data;

        #endregion
        
        #region ------------------------------ Notable Matrices ------------------------------

            public static matrix zero(int rows, int columns) => CreateMatrix(new complex[rows,columns]);
            public static matrix ones(int rows, int columns) {
                matrix m = matrix.zero(rows,columns);
                for (int i = 0; i < rows; i++) {
                    for (int j = 0; j < columns; j++) {
                        m.data[i,j] = 1;
                    }
                }
                return m;
            }

            public static matrix identity(int n) {
                matrix m = matrix.zero(n,n);
                for (int i = 0; i < n; i++) {
                    m.data[i,i] = 1;
                }

                return m;
            }
        
            public static matrix hilbert(int rows, int columns) {
                matrix m = matrix.zero(rows,columns);
                for (int i = 0; i < rows; i++) {
                    for (int j = 0; j < columns; j++) {
                        m.data[i,j] = 1/(i+j+1);
                    }
                }
                return m;
            }
            public static matrix lehmer(int rows, int columns) {
                matrix m = matrix.zero(rows,columns);
                for (int i = 0; i < rows; i++) {
                    for (int j = 0; j < columns; j++) {
                        m.data[i,j] = (j>=i)? i/j : j/i;
                    }
                }
                return m;
            }
            
        #endregion

        #region --------------------------------- Constructor --------------------------------
           
            matrix(complex[,] data) { 
                this.data    = data;
                this.rows    = data.GetLength(0);
                this.columns = data.GetLength(1);
            }
            public static matrix CreateMatrix(complex[,] inputArray) => new matrix(inputArray);

            public override string ToString(){
                string str = "";
                for (int m = 0; m < rows; m++) {
                    str += "|"; 
                    for (int n = 0; n < columns; n++) {
                        str += data[m,n] + ","; 
                    }
                    str += "|\n"; 
                }
                return str;
            }

        #endregion
        
        #region ------------------------------ Basic Operations ------------------------------

           // Sum
            public static matrix Sum(matrix a, params matrix[] b){
                matrix total = a;
                for (int i = 0; i < b.Length; i++) {
                    total += b[i];
                }
                return total;
            }
            public static matrix operator +(matrix a, matrix b){ 
                if (a.rows != b.rows || a.columns != b.columns) throw new ArgumentException("matrices must be of equal lengths");
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] += b.data[i,j];
                    }
                }
                return a;
            }
            public static matrix operator +(matrix a, double b){ 
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] += b;
                    }
                }
                return a;
            }
            public static matrix operator +(double b, matrix a){ 
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] += b;
                    }
                }
                return a;
            }

           // Subtraction
            public static matrix Subtract(matrix a, params matrix[] b){
                matrix total = a;
                for (int i = 0; i < b.Length; i++) {
                    total -= b[i];
                }
                return total;
            }
            public static matrix operator -(matrix a, matrix b){
                if (a.rows != b.rows || a.columns != b.columns) throw new ArgumentException("matrices must be of equal lengths");
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] -= b.data[i,j];
                    }
                }
                return a;
            }
            public static matrix operator -(matrix a, double b){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] -= b;
                    }
                }
                return a;
            }
            public static matrix operator -(double b, matrix a){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] -= b;
                    }
                }
                return a;
            }
            public static matrix operator -(matrix a){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] = -a.data[i,j];
                    }
                }
                return a;
            }

           // Multiplication
            public static matrix Multiply(matrix a, params matrix[] b){
                matrix total = a;
                for (int i = 0; i < b.Length; i++) {
                    total *= b[i];
                }
                return total;
            }
            public static matrix operator *(matrix a, matrix b){
                if (a.rows != b.columns || a.columns != b.rows) throw new ArgumentException("matrices must be of compatible lengths");
                matrix result = matrix.zero(a.rows,b.columns);
                for (int ai = 0; ai < a.rows; ai++) {
                    for (int bj = 0; bj < b.columns; bj++) {
                        for (int bi = 0; bi < b.rows; bi++) {
                            result += a.data[ai,bi]*a.data[bi,bj];
                        }
                    }
                }
                return result;
            }
            public static matrix operator *(matrix a, double b){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] *= b;
                    }
                }
                return a;
            }
            public static matrix operator *(double b, matrix a){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] *= b;
                    }
                }
                return a;
            }

           // Division 
            public static matrix operator /(matrix a, double b){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        a.data[i,j] /= b;
                    }
                }
                return a;
            }
        
           // Equality
            public override bool Equals(object obj){
                if (obj.GetType()!=typeof(matrix)) return false;
                return (this == (matrix) obj);
            }
            public override int GetHashCode() => base.GetHashCode();
            public static bool operator ==(matrix a, matrix b){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        if (a.data[i,j] == b.data[i,j]) return false;
                    }
                }
                return true;
            }
            public static bool operator !=(matrix a, matrix b){
                for (int i = 0; i < a.rows; i++) {
                    for (int j = 0; j < a.columns; j++) {
                        if (a.data[i,j] == b.data[i,j]) return true;
                    }
                }
                return false;
            }

        #endregion

        #region ------------------------------ Matrix Properties -----------------------------

           // Size of matrix
            public int size {get{return rows*columns;}}

        #endregion
    }

}
