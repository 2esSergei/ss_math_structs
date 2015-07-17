public abstract class ss_gyuru
{
    protected static double epsilon = 0.000001; //readonly-val
    public static ss_gyuru operator +(ss_gyuru A, ss_gyuru B) { return null; }
    public static ss_gyuru operator -(ss_gyuru A, ss_gyuru B) { return null; }
    public static ss_gyuru operator *(ss_gyuru A, ss_gyuru B) { return null; }
    public static bool operator ==(ss_gyuru A, ss_gyuru B) { return false; }
    public static bool operator !=(ss_gyuru A, ss_gyuru B) { return false; }
    public override bool Equals(object obj) { return false; }
    public override int GetHashCode() { return 0; }
    public override string ToString() { return null; }
}
public class ss_matrix : ss_gyuru
{
    protected double[] matrix_tomb;
    protected int rows;
    protected int cols;
    public ss_matrix(int rows, int cols)
    {//unsafe constuctor, recommended to use private constructor
        matrix_tomb = new double[rows * cols];
        this.rows = rows;
        this.cols = cols;
    }
    public static ss_matrix Create(int rows, int cols)
    {//object factory to safe constructor
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
    public double this[int i, int j]	//tulindexeles kivetel
    {
        get
        {
            if (i < 0 || j < 0|| i >= this.rownumber || j >= this.colnumber)
            {
                throw new System.ArgumentOutOfRangeException("");
            }
            return matrix_tomb[i * this.cols + j];
        }
        set
        {
            matrix_tomb[i * this.cols + j] = value;
        }
    }
    public static ss_matrix operator +(ss_matrix TEMP1, ss_matrix TEMP2)	//sor-oszlop elteres kivetel, null referencia kivetel
    {
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
    public static ss_matrix operator -(ss_matrix TEMP1, ss_matrix TEMP2)	//sor-oszlop elteres kivetel, null refderencia kivetel
    {
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
    {
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
    {
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
                if (TEMP1[i, j] - this[i, j] > ss_gyuru.epsilon || this[i, j] - TEMP1[i, j] > epsilon)    //(this[i, j] != TEMP1[i, j])
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
    {
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
}

class matrixTester
{
    public static void Main()
    {
        ss_matrix M = new ss_matrix(5, 4);
        ss_matrix N = new ss_matrix(4, 5);
        for (int i = 0; i < M.rownumber; i++)
        {
            for (int j = 0; j < M.colnumber; j++)
            {
                M[i, j] = 1.00;
                N[j, i] = 1;
            }
        }
        ss_matrix O = new ss_matrix(5, 5);
        O = M * N;
        for (int i = 0; i < O.rownumber; i++)
        {
            for (int j = 0; j < O.colnumber; j++)
            {
                System.Console.Write("{0} ", O[i, j]);
            }
            System.Console.WriteLine();
        }
        ss_matrix P = M;
        System.Console.WriteLine(M == P);
        System.Console.WriteLine(M);
        ss_matrix Q = ss_matrix.Create(-1, 5);
        System.Collections.Generic.List<ss_matrix> mySSlist = new System.Collections.Generic.List<ss_matrix>();
        for (int i = 0; i < 2147483647; i++)
        {
            mySSlist.Add(new ss_matrix(2147483647, 2147483647));
        }
        ss_matrix R = new ss_matrix(2147483647, 2147483647);
        System.Console.WriteLine(R[2147483646, 2147483646]);
    }
}