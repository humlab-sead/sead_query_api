namespace SeadQueryCore
{
    public class ResultSpecificationField
    {
        public int SpecificationFieldId { get; set; }
        public int SpecificationId { get; set; }
        public string FieldTypeId { get; set; }
        public int ResultFieldId { get; set; }
        public int SequenceId { get; set; }

        public virtual ResultSpecification Specification { get; set; }
        public virtual ResultFieldType FieldType { get; set; }
        public virtual ResultField ResultField { get; set; }
    }
}
