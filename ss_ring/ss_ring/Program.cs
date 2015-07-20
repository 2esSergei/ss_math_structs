/// <summary>
/// Valamit irtam ide.
/// </summary>
public abstract class ss_gyuru
{
    protected static readonly double epsilon = 0.000001;
    public static ss_gyuru operator +(ss_gyuru A, ss_gyuru B) { return null; }
    public static ss_gyuru operator -(ss_gyuru A, ss_gyuru B) { return null; }
    public static ss_gyuru operator *(ss_gyuru A, ss_gyuru B) { return null; }
    public static bool operator ==(ss_gyuru A, ss_gyuru B) { return false; }
    public static bool operator !=(ss_gyuru A, ss_gyuru B) { return false; }
    public override bool Equals(object obj) { return false; }
    public override int GetHashCode() { return 0; }
    public override string ToString() { return null; }
}
public class ss_matrix : ss_gyuru, System.IDisposable
{
    protected double[] matrix_tomb;
    protected int rows;
    protected int cols;
    private bool disposed = false;
    private ss_matrix(int rows, int cols)
    {//unsafe constuctor so it is private
        try
        {
            matrix_tomb = new double[rows * cols];
            this.rows = rows;
            this.cols = cols;
        }
        catch (System.OutOfMemoryException e)
        {
            System.Console.WriteLine("Constructor Out of Memory exception: " + e.Message);
            throw new System.OutOfMemoryException();
        }
    }
    public static ss_matrix Create(int rows, int cols)
    {//object factory to safe constructor, maybe should handle System.Exception
        if (rows < 1 || cols < 1)
        {//invalid row or col number
            return null;
        }
        try
        {//allocation exception
            return new ss_matrix(rows, cols);
        }
        catch (System.OutOfMemoryException e)
        {
            System.Console.WriteLine("Object has NULL reference: {0}", e.Message);
            return null;
        }
    }
    public int rownumber
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
    public int colnumber
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
    public double this[int i, int j]
    {
        get
        {
            if (i < 0 || j < 0|| i >= this.rownumber || j >= this.colnumber)
            {//Out of Range
                string message = "READ ELEMENT: Not exist element with index of (" + i + ", " + j + ")";
                throw new System.ArgumentOutOfRangeException(message);
            }
            else
            {
                return matrix_tomb[i * this.cols + j];
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
                matrix_tomb[i * this.cols + j] = value;
            }
        }
    }
    public static ss_matrix operator +(ss_matrix TEMP1, ss_matrix TEMP2)
    {
        if(TEMP1.rownumber != TEMP2.rownumber || TEMP1.colnumber != TEMP2.colnumber)
        {//Addition mathematical rules, handle only part of null matrix situation
            string message = "Operation fail: can not use '+' operator for matrix (" + TEMP1.rownumber + " x " + TEMP1.colnumber + ") and matrix ("
                + TEMP2.rownumber + " x " + TEMP2.colnumber + ")";
            throw new System.Exception(message);
        }
        if(TEMP1 == null || TEMP2 == null)
        {//this protect when all of them is null
            return null;
        }
        ss_matrix TEMP3 = new ss_matrix(TEMP1.rownumber, TEMP1.colnumber);
        for (int i = 0; i < TEMP1.rownumber; i++)
        {
            for (int j = 0; j < TEMP1.colnumber; j++)
            {
                TEMP3[i, j] = TEMP1[i, j] + TEMP2[i, j];
            }
        }
        return TEMP3;
    }
    public static ss_matrix operator -(ss_matrix TEMP1, ss_matrix TEMP2)
    {//Addition mathematical rules: rows and cols value is same
        if (TEMP1.rownumber != TEMP2.rownumber || TEMP1.colnumber != TEMP2.colnumber)
        {//handle only part of null matrix situation
            string message = "Operation fail: can not use '-' operator for matrix (" + TEMP1.rownumber + " x " + TEMP1.colnumber + ") and matrix ("
                + TEMP2.rownumber + " x " + TEMP2.colnumber + ")";
            throw new System.Exception(message);
        }
        if (TEMP1 == null || TEMP2 == null)
        {//this protect when all of them is null
            return null;
        }
        ss_matrix TEMP3 = new ss_matrix(TEMP1.rownumber, TEMP1.colnumber);
        for (int i = 0; i < TEMP1.rownumber; i++)
        {
            for (int j = 0; j < TEMP1.colnumber; j++)
            {
                TEMP3[i, j] = TEMP1[i, j] - TEMP2[i, j];
            }
        }
        return TEMP3;
    }
    public static ss_matrix operator *(ss_matrix TEMP1, ss_matrix TEMP2)	//sor-oszlop elteres kivetel, null refderencia kivetel
    {//Multiplication mathematical rules, first's cols vaule equal the secund's rows value
        if (TEMP1 == null || TEMP2 == null)
        {//if one of them is null, than solution is null (it is better choose like than at +/- operator)
            return null;
        }
        if( TEMP1.colnumber != TEMP2.rownumber)
        {
            string message = "Operation fail: can not use '*' operator for matrix (" + TEMP1.rownumber + " x " + TEMP1.colnumber + ") and matrix ("
                + TEMP2.rownumber + " x " + TEMP2.colnumber + ")";
            throw new System.Exception(message);
        }
        ss_matrix TEMP3 = new ss_matrix(TEMP1.rownumber, TEMP2.colnumber);
        for (int i = 0; i < TEMP1.rownumber; i++)
        {
            for (int j = 0; j < TEMP2.colnumber; j++)
            {
                for (int k = 0; k < TEMP1.colnumber; k++)
                {
                    TEMP3[i, j] += TEMP1[i, k] * TEMP2[k, j];
                }
            }
        }
        return TEMP3;
    }
    public override bool Equals(System.Object obj)
    {//this is safe
        if (obj == null)
        {
            return false;
        }
        ss_matrix TEMP1 = obj as ss_matrix;
        if ((System.Object)TEMP1 == null)
        {
            return false;
        }
        if (TEMP1.rownumber != this.rownumber || TEMP1.colnumber != this.colnumber)
        {
            return false;
        }
        for (int i = 0; i < TEMP1.rownumber; i++)
        {
            for (int j = 0; j < TEMP1.colnumber; j++)
            {
                if (TEMP1[i, j] - this[i, j] > ss_gyuru.epsilon || this[i, j] - TEMP1[i, j] > epsilon)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator ==(ss_matrix TEMP1, ss_matrix TEMP2)
    {
        return TEMP1.Equals(TEMP2);
    }
    public static bool operator !=(ss_matrix TEMP1, ss_matrix TEMP2)
    {
        return !(TEMP1 == TEMP2);
    }
    public override string ToString()
    {//string builder kell
        string s = "";
        if (this == null)
        {
            return null;
        }
        else
        {
            for (int i = 0; i < this.rownumber; i++)
            {
                for (int j = 0; j < this.colnumber; j++)
                {
                    s += this[i, j];
                    s += "\t";
                }
                s += "\n";
            }
        }
        return s;
    }
    public void Dispose()
    {
        Dispose(true);
        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC.SupressFinalize to
        // take this object off the finalization queue
        // and prevent finalization code for this object
        // from executing a second time.
        System.GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!this.disposed)
        {
            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose resources.
                System.Array.Clear(matrix_tomb, 0, matrix_tomb.Length);
                matrix_tomb = null;
            }
            // Note disposing has been done.
            disposed = true;
        }
    }
}

class matrixTester
{
    public static void Main()
    {
        ss_matrix M = ss_matrix.Create(5, 4);
        ss_matrix N = ss_matrix.Create(4, 5);
        ss_matrix P = ss_matrix.Create(5, 4);
        for (int i = 0; i < M.rownumber; i++)
        {
            for (int j = 0; j < M.colnumber; j++)
            {
                M[i, j] = 1.00;
                N[j, i] = 1;
                P[i, j] = 1;
            }
        }
        //M = M + N;
        ss_matrix O = ss_matrix.Create(5, 5);
        //ss_matrix S = M * O;
        O = M * N;
        System.Console.WriteLine(O);
        System.Console.WriteLine(M == P);
        P[4, 3] = 3;
        System.Console.WriteLine(M == P);
        System.Console.WriteLine(M);
        System.Console.WriteLine(P);
        //double x = P[5, 3];
        //P[5, 3] = 5;
        ss_matrix Q = ss_matrix.Create(-1, 5);
        System.Collections.Generic.List<ss_matrix> mySSlist = new System.Collections.Generic.List<ss_matrix>();
        for (int i = 0; i < 2147483647; i++)
        {
            int ii = 0;
            try
            {
                mySSlist.Add(ss_matrix.Create(2147483647, 2147483647));
            }
            catch (System.OutOfMemoryException e)
            {
                System.Console.WriteLine("Kivetel a tombnel. A ciklus {0}. lepeseben. {1}", i, e.Message);
                mySSlist[ii].Dispose();
                mySSlist.RemoveAt(ii);
                ii++;
            }
        }
        ss_matrix R = ss_matrix.Create(2147483647, 2147483647);
        System.Console.WriteLine(R[2147483646, 2147483646]);
    }
}