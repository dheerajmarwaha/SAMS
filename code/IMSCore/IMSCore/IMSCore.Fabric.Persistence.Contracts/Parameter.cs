using System.Data;

namespace IMSCore.Fabric.Persistence.Contracts
{
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection Direction { get; set; }
        public DbType DbType { get; set; }
        public int Size { get; set; }
        public Parameter()
        {
            this.Direction = ParameterDirection.Input;
            this.DbType = DbType.AnsiString;
        }
    }
}