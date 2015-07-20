using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSAS
{
    public interface IHalf_Group
    {
        Object OperationHG(Object ELEMENT1, Object ELEMENT2);
        bool Equals(Object obj);
        int GetHashCode();
        Object Neutral_elementHG { get; }
    }
    public interface IGroup
    {
        Object Operation(Object ELEMENT1, Object ELEMENT2);
        Object Operation_inverse(Object ELEMENT1, Object ELEMENT2);
        bool Equals(Object obj);
        int GetHashCode();
        Object Neutral_element { get; }
    }
    public interface IMultiplication_Group
    {
        Object OperationMG(Object ELEMENT1, Object ELEMENT2);
        Object Operation_inverseMG(Object ELEMENT1, Object ELEMENT2);
        bool Equals(Object obj);
        int GetHashCode();
        Object Neutral_elementMG { get; }
    }
    public interface IAdditional_Group
    {
        Object OperationAG(Object ELEMENT1, Object ELEMENT2);
        Object Operation_inverseAG(Object ELEMENT1, Object ELEMENT2);
        bool Equals(Object obj);
        int GetHashCode();
        Object Neutral_elementAG { get; }
    }
    public interface IRing : IGroup, IHalf_Group { }
    public interface IBody : IAdditional_Group, IMultiplication_Group { }
}
