using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSAS
{
    class SSmatrix<T> : ILinearSpace<T> where T : IConvertible
    {
        //base data-members
        protected T[] matrix_array;
        protected ulong rows;
        protected ulong cols;
        private bool disposed = false;
        protected static readonly double epsilon = 0.000001;
        protected T neutral_unit;
        //constructor and destructor methods
        private SSmatrix(ulong rows, ulong cols)
        {//unsafe constuctor so it is private
            try
            {
                matrix_array = new T[rows * cols];
                this.rows = rows;
                this.cols = cols;
            }
            catch (System.OutOfMemoryException e)
            {
                System.Console.WriteLine("Constructor Out of Memory exception: " + e.Message);
                throw new System.OutOfMemoryException();
            }
            var type = typeof(T);
            if (type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            try
            {
                neutral_unit = (T)(Object)1;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("The type is not supported with standard unit 1. Use UnitInfo property. " + ex.Message);
            }
        }
        public static SSmatrix<T> Create(ulong rows, ulong cols)
        {//object factory to safe constructor, maybe should handle System.Exception
            if (rows == 0 || cols == 0)
            {//invalid row or col number, but not fail
                return null;
            }
            try
            {//allocation exception
                return new SSmatrix<T>(rows, cols);
            }
            catch (System.OutOfMemoryException e)
            {
                System.Console.WriteLine("Object has NULL reference: {0}", e.Message);
                return null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose resources.
                    System.Array.Clear(matrix_array, 0, matrix_array.Length);
                    matrix_array = null;
                }
                // Note disposing has been done.
                disposed = true;
            }
        }
        //properties and indexer
        public ulong rownumber
        {
            get
            {
                if (this == null)
                {
                    return 0;
                }
                else
                {
                    return this.rows;
                }
            }
        }
        public ulong colnumber
        {
            get
            {
                if (this == null)
                {
                    return 0;
                }
                else
                {
                    return this.cols;
                }
            }
        }
        public T this[ulong i, ulong j]
        {
            get
            {
                if (i < 0 || j < 0 || i >= this.rownumber || j >= this.colnumber)
                {//Out of Range
                    string message = "READ ELEMENT: Not exist element with index of (" + i + ", " + j + ")";
                    throw new System.ArgumentOutOfRangeException(message);
                }
                else
                {
                    return matrix_array[i * this.cols + j];
                }
            }
            set
            {
                if (i < 0 || j < 0 || i >= this.rownumber || j >= this.colnumber)
                {//Out of Range
                    string message = "WRITE ELEMENT: Not exist element with index of (" + i + ", " + j + ")";
                    throw new System.ArgumentOutOfRangeException(message);
                }
                else
                {
                    matrix_array[i * this.cols + j] = value;
                }
            }
        }
        public T UnitInfo
        {
            set
            {
                neutral_unit = (T)(Object)value;
            }
            get
            {
                return neutral_unit;
            }
        }
        //Interface criterias
        protected static SSmatrix<T> Operation(SSmatrix<T> ELEMENT1, SSmatrix<T> ELEMENT2)
        {
            if (ELEMENT1.rownumber != ELEMENT2.rownumber || ELEMENT1.colnumber != ELEMENT2.colnumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '+' operator for matrix (" + ELEMENT1.rownumber + " x " + ELEMENT1.colnumber + ") and matrix ("
                    + ELEMENT2.rownumber + " x " + ELEMENT2.colnumber + ")";
                throw new System.Exception(message);
            }
            if (ELEMENT1 == null || ELEMENT2 == null)
            {//this protect when all of them is null
                return null;
            }
            SSmatrix<T> ELEMENT3 = new SSmatrix<T>(ELEMENT1.rownumber, ELEMENT1.colnumber);
            var type = typeof(T);
            if (type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < ELEMENT1.rownumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.colnumber; j++)
                {
                    //ELEMENT3[i, j] = ELEMENT1[i, j] + ELEMENT2[i, j];
                    try
                    {
                        ELEMENT3[i, j] = (T)(Object)(ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) + ELEMENT2[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("The operation failed.", ex);
                    }
                }
            }
            return ELEMENT3;
        }
        public static SSmatrix<T> operator +(SSmatrix<T> ELEMNET1, SSmatrix<T> ELEMENT2)
        {
            return Operation(ELEMNET1, ELEMENT2);
        }
        protected static SSmatrix<T> Operation_inverse(SSmatrix<T> ELEMENT1, SSmatrix<T> ELEMENT2)
        {
            if (ELEMENT1.rownumber != ELEMENT2.rownumber || ELEMENT1.colnumber != ELEMENT2.colnumber)
            {//Addition mathematical rules, handle only part of null matrix situation
                string message = "Operation fail: can not use '+' operator for matrix (" + ELEMENT1.rownumber + " x " + ELEMENT1.colnumber + ") and matrix ("
                    + ELEMENT2.rownumber + " x " + ELEMENT2.colnumber + ")";
                throw new System.Exception(message);
            }
            if (ELEMENT1 == null || ELEMENT2 == null)
            {//this protect when all of them is null
                return null;
            }
            SSmatrix<T> ELEMENT3 = new SSmatrix<T>(ELEMENT1.rownumber, ELEMENT1.colnumber);
            var type = typeof(T);
            if (type == typeof(String) || type == typeof(DateTime))
            {
                throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
            }
            for (ulong i = 0; i < ELEMENT1.rownumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.colnumber; j++)
                {
                    //ELEMENT3[i, j] = ELEMENT1[i, j] - ELEMENT2[i, j];
                    try
                    {
                        ELEMENT3[i, j] = (T)(Object)(ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - ELEMENT2[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("The operation failed.", ex);
                    }
                }
            }
            return ELEMENT3;
        }
        public static SSmatrix<T> operator -(SSmatrix<T> ELEMNET1, SSmatrix<T> ELEMENT2)
        {
            return Operation_inverse(ELEMNET1, ELEMENT2);
        }
        public override bool Equals(System.Object obj)
        {//this is safe
            if (obj == null)
            {
                return false;
            }
            SSmatrix<T> ELEMENT1 = obj as SSmatrix<T>;
            if ((System.Object)ELEMENT1 == null)
            {
                return false;
            }
            if (ELEMENT1.rownumber != this.rownumber || ELEMENT1.colnumber != this.colnumber)
            {
                return false;
            }
            for (ulong i = 0; i < ELEMENT1.rownumber; i++)
            {
                for (ulong j = 0; j < ELEMENT1.colnumber; j++)
                {
                    var type = typeof(T);
                    if (type == typeof(String) || type == typeof(DateTime))
                    {
                        throw new ArgumentException(String.Format("The type {0} is not supported", type.FullName), "T");
                    }
                    try
                    {
                        if (ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) > epsilon
                            || this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) - ELEMENT1[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) > epsilon)
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw new ApplicationException("RETURN FALSE Check the type for Equals." + ex.Message);
                    }
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(SSmatrix<T> ELEMENT1, SSmatrix<T> ELEMENT2)
        {
            return ELEMENT1.Equals(ELEMENT2);
        }
        public static bool operator !=(SSmatrix<T> ELEMENT1, SSmatrix<T> ELEMENT2)
        {
            return !(ELEMENT1 == ELEMENT2);
        }
        public SSmatrix<T> Neutral_element
        {
            get
            {
                ulong max_side;
                if (this.rownumber > this.colnumber)
                {
                    max_side = this.rownumber;
                }
                else
                {
                    max_side = this.colnumber;
                }
                SSmatrix<T> ELEMENT1 = new SSmatrix<T>(this.rownumber, this.colnumber);
                for(ulong i = 0; i < max_side; i++)
                {
                    ELEMENT1[i, i] = this.UnitInfo;
                }
                return ELEMENT1;
            }
        }
        protected SSmatrix<T> MultiplyScalar(T scalar,ref SSmatrix<T> ELEMENT1)
        {
            SSmatrix<T> ELEMENT1 = new SSmatrix<T>(this)
            for(ulong i = 0; i < this.rownumber; i++)
            {
                for(ulong j = 0; j < this.colnumber; j++)
                {
                    this[i, j] = (T)(Object)(this[i, j].ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo) * scalar.ToDouble(System.Globalization.NumberFormatInfo.CurrentInfo));
                }
            }
        }
    }
}
